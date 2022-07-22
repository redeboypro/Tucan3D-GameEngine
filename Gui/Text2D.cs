using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using Tucan3D_GameEngine.Rendering.Common;

namespace Tucan3D_GameEngine.Gui
{
    public class Font
    {
        private int textureId;

        private static string CHAR_SHEET = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890!@#$%^&*()-=_+[]{}\\|;:'\".,<>/?`~ ";
        private VertexArrayData[] VAOs = new VertexArrayData[CHAR_SHEET.Length];
        private FontShader shader;

        private static char[] fontCharArray = CHAR_SHEET.ToCharArray();
        public static char[] CharSheet => fontCharArray;

        public FontShader Shader => shader;

        public VertexArrayData GetCharVertexArrayData(char character) => VAOs[CHAR_SHEET.IndexOf(character)];

        public int TextureId => textureId;

        public Font(FontShader shader, string file, string charSheet = "")
        {
            if (charSheet != string.Empty)
            {
                CHAR_SHEET = charSheet;
                VAOs = new VertexArrayData[CHAR_SHEET.Length];
                fontCharArray = CHAR_SHEET.ToCharArray();
            }

            this.shader = shader;
            
            GL.Hint(HintTarget.PerspectiveCorrectionHint, HintMode.Nicest);

            GL.GenTextures(1, out textureId);
            GL.BindTexture(TextureTarget.Texture2D, textureId);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);
            
            Bitmap bitmap = new Bitmap(file);

            BitmapData data = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height),
                ImageLockMode.ReadWrite, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, data.Width, data.Height, 0,
                OpenTK.Graphics.OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte, data.Scan0);

            bitmap.UnlockBits(data);
            
            var sheetToArray = CHAR_SHEET.ToCharArray();
            
            var vertices = new float[] {0,0, 1,0, 1,1, 0,1};
            var textureCoords = new float[] {0,1, 1,1, 1,0, 0,0};
            
            for (int i = 0; i < sheetToArray.Length; i++)
            {
                float CharSize = 1 / 16f;
                char c = sheetToArray[i];
                int y = c >> 4;
                int x = c & 0b1111;

                var left = x * CharSize;
                var right = left + CharSize * 1;
                var top = y * CharSize;
                var bottom = top + CharSize;

                textureCoords[0] = textureCoords[6] = left;
                textureCoords[2] = textureCoords[4] = right;
                textureCoords[1] = textureCoords[3] = bottom;
                textureCoords[5] = textureCoords[7] = top;
                vertices[2] = vertices[4] = 1;

                VAOs[i] = new VertexArrayData(vertices, textureCoords);
            }
        }
        
    }

    public class Text2D : UIElement
    {
        private string text = string.Empty;
        private Font font;
        private bool fillBackground;
        private Color4 color = Color4.White;
        private bool ignoreLineSplitters;

        public string Text
        {
            get => text;
            set => text = value;
        }
        
        public bool FillBackground
        {
            get => fillBackground;
            set => fillBackground = value;
        }
        
        public bool IgnoreLineSplitters
        {
            get => ignoreLineSplitters;
            set => ignoreLineSplitters = value;
        }

        public Color4 Color
        {
            get => color;
            set => color = value;
        }

        public Text2D(Font font)
        {
            this.font = font;
            RecalculateBounds();
        }

        public bool PointIsInsideBounds(Vector2 point)
        {
            var pt = new Vector2(point.X, point.Y);
            var len = text.Length * scale.X;
            
            return position.X < pt.X && position.X + len > pt.X 
                                     && position.Y + scale.Y > pt.Y && position.Y < pt.Y;
        }

        public override void Draw()
        {
            var textToCharArray = text.ToCharArray();
                
            font.Shader.Start();

            GL.ActiveTexture(TextureUnit.Texture0);
            GL.BindTexture(TextureTarget.Texture2D, font.TextureId);
            font.Shader.SetUniform("charTexture",0);
            
            int x = 0, y = 0;
            for (int i = 0; i < textToCharArray.Length; i++)
            {
                if (Font.CharSheet.Contains(textToCharArray[i]))
                {
                    Vector3 positionT = new Vector3(Position.X + x * scale.X, Position.Y - y * scale.Y, 0);

                    Vector3 scaleT;
                    scaleT.X = scale.X;
                    scaleT.Y = scale.Y;
                    scaleT.Z = 1;

                    var matrix = Matrix4.CreateScale(scaleT) * Matrix4.CreateTranslation(positionT);
                    font.Shader.SetUniform("charMatrix", matrix);
                    font.Shader.SetUniform("fillBG", fillBackground);
                    font.Shader.SetUniform("textColor", color);

                    GL.BindVertexArray(font.GetCharVertexArrayData(textToCharArray[i]).Id);
                    GL.EnableVertexAttribArray(0);
                    GL.EnableVertexAttribArray(1);

                    GL.DrawArrays(PrimitiveType.TriangleFan, 0, 4);

                    GL.DisableVertexAttribArray(0);
                    GL.DisableVertexAttribArray(1);
                    
                    x++;
                }
                else if (textToCharArray[i] == '\n' && !ignoreLineSplitters)
                {
                    x = 0;
                    y++;
                }
            }
            
            GL.BindVertexArray(0);

            font.Shader.Stop();
        }
    }

}
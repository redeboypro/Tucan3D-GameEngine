using System;
using System.Drawing;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Input;
using Tucan3D_GameEngine.Rendering;

namespace Tucan3D_GameEngine.Gui.Extends
{
    public class Button : UIElement
    {
        private Action action;
        private Text2D textElement;
        private Image2D imageElement;
        
        public Button(string header, Action action, TextureData textureData, GUIShader guiShader, Font font)
        {
            this.action = action;
            
            textElement = new Text2D(font)
            {
                Text = header,
                Color = Color4.DarkGray
            };
            scale = new Vector2(textElement.Text.Length * textElement.Scale.X, textElement.Scale.Y + 0.05f);
            
            imageElement = new Image2D(guiShader, textureData)
            {
                Scale = this.scale
            };

            OnTranslate = () =>
            {
                imageElement.Position = this.Position;
                textElement.Position = this.Position
                                       + Vector2.UnitY * 0.025f 
                                       + Vector2.UnitX * 0.01f;
            };
        }

        public override void Draw()
        {
            imageElement.Draw();
            textElement.Draw();
        }

        public override void OnMouseDown(MouseButtonEventArgs eventArgs)
        {
            var mousePoint = MathUtils.GetNormalisedCoordinates(eventArgs.X, eventArgs.Y);
            if (imageElement.PointIsInsideBounds(mousePoint.X, mousePoint.Y))
            {
                textElement.Color = Color4.Gray;
                action?.Invoke();
            }
        }
        
        public override void OnMouseUp(MouseButtonEventArgs eventArgs)
        {
            textElement.Color = Color4.DarkGray;
        }
    }
}
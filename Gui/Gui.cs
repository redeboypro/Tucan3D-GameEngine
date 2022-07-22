using System;
using System.Collections.Generic;
using System.Drawing;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Input;
using Tucan3D_GameEngine.Gui.Extends;
using Tucan3D_GameEngine.Rendering;
using Tucan3D_GameEngine.Rendering.Common;

namespace Tucan3D_GameEngine.Gui
{
    public static class Gui
    {
        private static List<UIElement> guiElements = new List<UIElement>();
        
        private static Font font;
        private static FontShader fontShader;
        
        private static GUIShader guiShader;
        private static VertexArrayData quadVertexData;
        public static VertexArrayData QuadVertexData => quadVertexData;

        public static void Initialize(string FontFileName)
        {
            fontShader = new FontShader();
            font = new Font(fontShader, FontFileName);

            guiShader = new GUIShader();
            quadVertexData = new VertexArrayData(new float[] { 0,1,  0,0,  1,1,  1,0 }, 2);
        }

        public static void DrawElements()
        {
            foreach (var e in guiElements)
            {
                if (e.GetType().IsAssignableFrom(typeof(ScrollPanel)))
                {
                    ((ScrollPanel) e).DrawItemList();
                }
                else
                {
                    e.Draw();
                }
            }
        }

        public static void OnMouseDown(MouseButtonEventArgs eventArgs)
        {
            foreach (var e in guiElements)
            {
                e.OnMouseDown(eventArgs);
            }
        }
        
        public static void OnMouseUp(MouseButtonEventArgs eventArgs)
        {
            foreach (var e in guiElements)
            {
                e.OnMouseUp(eventArgs);
            }
        }
        
        public static void OnMouseMove(MouseMoveEventArgs eventArgs)
        {
            foreach (var e in guiElements)
            {
                e.OnMouseMove(eventArgs);
            }
        }

        public static Image2D Image(TextureData textureData, float x, float y, Vector2 scale)
        {
            Vector2 position;
            position.X = x;
            position.Y = y;
            
            var out_gui = new Image2D(guiShader, textureData)
            {
                Position = position,
                Scale = scale,
            };
            
            guiElements.Add(out_gui);
            return out_gui;
        }

        public static Text2D Text(string text, float x, float y, Vector2 scale, bool ignoreLineSplitters = false)
        {
            Vector2 position;
            position.X = x;
            position.Y = y;
            
            var out_gui = new Text2D(font)
            {
                Text = text,
                Position = position,
                Scale = scale,
                IgnoreLineSplitters = ignoreLineSplitters
            };
            
            guiElements.Add(out_gui);
            return out_gui;
        }
        
        public static ScrollPanel ScrollPanel(Action action, TextureData textureData, float x, float y, Vector2 scale)
        {
            Vector2 position;
            position.X = x;
            position.Y = y;

            var out_gui = new ScrollPanel(font, guiShader, textureData, action)
            {
                ScrollShift = position.Y,
                Position = position,
                Scale = scale,
            };
            
            guiElements.Add(out_gui);
            return out_gui;
        }
        
        public static Button Button(string header, Action action, TextureData textureData, float x, float y)
        {
            Vector2 position;
            position.X = x;
            position.Y = y;

            var out_gui = new Button(header, action, textureData, guiShader, font)
            {
                Position = new Vector2(x, y)
            };
            
            guiElements.Add(out_gui);
            return out_gui;
        }
    }
}
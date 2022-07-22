using System;
using System.Collections.Generic;
using System.Linq;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;
using Tucan3D_GameEngine.Rendering;
using Tucan3D_GameEngine.WorldObjects;

namespace Tucan3D_GameEngine.Gui.Extends
{
    public class ScrollPanel : Image2D
    {
        private List<ScrollPanelItem> items = new List<ScrollPanelItem>();
        
        private Font font;
        
        private GUIShader guiShader;
        private TextureData textureData;

        private float scrollShift;
        private bool isInDragMode;

        private ScrollPanelItem selected;
        public ScrollPanelItem Selected => selected;
        public void AssignSelectedItem(ScrollPanelItem item) => selected = item;

        private Action action;
        public Action SelectedItemChanged => action;

        public float ScrollShift
        {
            get => scrollShift;
            set
            {
                scrollShift = value;

                foreach (var i in items)
                {
                    Vector2 position;
                    position.X = Position.X + 0.05f;
                    position.Y = ScrollShift + scale.Y * 0.85f - items.IndexOf(i) * 0.1f;

                    i.Position = position;
                }
            }
        }

        public ScrollPanel(Font font, GUIShader guiShader, TextureData textureData, Action action) : base(guiShader, textureData)
        {
            this.guiShader = guiShader;
            this.font = font;
            this.textureData = textureData;
            this.action = action;
            RecalculateBounds();
        }

        public void Add(string header)
        {
            Vector2 position;
            position.X = Position.X + 0.05f;
            position.Y = ScrollShift + scale.Y * 0.85f - items.Count * 0.1f;
            
            items.Add(new ScrollPanelItem(header, this, font)
            {
                Position = position,
                IgnoreLineSplitters = true
            });
        }

        public override void OnMouseDown(MouseButtonEventArgs eventArgs)
        {
            var mouseMousePoint = MathUtils.GetNormalisedCoordinates(eventArgs.X, eventArgs.Y);
                
            if (eventArgs.Button == MouseButton.Left && PointIsInsideBounds(mouseMousePoint.X, mouseMousePoint.Y))
            {
                foreach (var i in items)
                {
                    Vector2 pt;
                    pt.X = mouseMousePoint.X;
                    pt.Y = mouseMousePoint.Y;
                    
                    i.OnClick(pt);
                }
                isInDragMode = true;
            }
        }

        public override void OnMouseMove(MouseMoveEventArgs eventArgs)
        {
            if (isInDragMode)
            {
                ScrollShift -= eventArgs.YDelta / 150f;
            }
        }

        public override void OnMouseUp(MouseButtonEventArgs eventArgs)
        {
            if (eventArgs.Button == MouseButton.Left)
            {
                isInDragMode = false;
            }
        }

        public void Clear()
        {
            for(var i = 0; i < items.Count; i++)
            {
                items[i] = null;
            }
            
            items.Clear();
            
            GC.Collect();
            GC.WaitForPendingFinalizers();
        }

        public void RefreshFrom(string[] source)
        {
            Clear();

            foreach (var item in source)
            {
                Add(item);
            }
        }
        
        public void RefreshFrom(List<string> source)
        {
            Clear();

            foreach (var item in source)
            {
                Add(item);
            }
        }
        
        public void RefreshFrom(WorldObject[] source)
        {
            Clear();

            foreach (var item in source)
            {
                Add(item.Name);
            }
        }
        
        public void RefreshFrom(List<WorldObject> source)
        {
            Clear();

            foreach (var item in source)
            {
                Add(item.Name);
            }
        }

        public void DrawItemList()
        {
            Draw();

            ScrollShift = MathHelper.Clamp(ScrollShift, Position.Y, Position.Y + (items.Count - 6) * 0.1f);

            Vector2 position = MathUtils.GetUnormalisedCoordinates(Position.X, -Position.Y);
            Vector2 scale = MathUtils.GetUnormalisedCoordinates(this.scale.X - 1,  1 - this.scale.Y);

            GL.Scissor((int)position.X, (int)position.Y, (int)scale.X, (int)scale.Y);
            GL.Enable(EnableCap.ScissorTest);
            foreach (var i in items)
            {
                i.Draw();
            }
            GL.Disable(EnableCap.ScissorTest);
        }
    }
}
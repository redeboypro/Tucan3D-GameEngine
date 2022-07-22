using System;
using System.Collections.Generic;
using System.Linq;
using OpenTK;
using OpenTK.Input;

namespace Tucan3D_GameEngine.Gui
{
    public abstract class UIElement : IDisposable
    {
        protected Vector2 position = Vector2.Zero;
        protected Vector2 scale = Vector2.One * 0.05f;
        protected Action OnTranslate;
        protected Action OnScale;

        public Vector2 Position
        {
            get => position;
            set
            {
                position = value;
                RecalculateBounds();
                OnTranslate?.Invoke();
            }
        }
        
        public Vector2 Scale
        {
            get => scale;
            set
            {
                scale = value;
                RecalculateBounds();
                OnScale?.Invoke();
            }
        }

        public bool PointIsInsideBounds(float x, float y) => x < Max.X && x > Min.X && y < Max.Y && y > Min.Y;

        public virtual void OnMouseDown(MouseButtonEventArgs eventArgs)
        {
            
        }

        public virtual void OnMouseUp(MouseButtonEventArgs eventArgs)
        {
            
        }
        
        public virtual void OnMouseMove(MouseMoveEventArgs eventArgs)
        {
            
        }

        public void RecalculateBounds()
        {
            Bounds[0] = new Vector2(Position.X, Position.Y);
            Bounds[1] = new Vector2(Position.X, Position.Y + scale.Y);
            Bounds[2] = new Vector2(Position.X + scale.X, Position.Y + scale.Y);
            Bounds[3] = new Vector2(Position.X + scale.X, Position.Y);
            
            var minX = new [] { Bounds[0].X,
                Bounds[1].X, Bounds[2].X, Bounds[3].X }.Min();

            var maxX = new [] { Bounds[0].X,
                Bounds[1].X, Bounds[2].X, Bounds[3].X }.Max();

            var minY = new [] { Bounds[0].Y,
                Bounds[1].Y, Bounds[2].Y, Bounds[3].Y }.Min();

            var maxY = new [] { Bounds[0].Y,
                Bounds[1].Y, Bounds[2].Y, Bounds[3].Y }.Max();

            Min = new Vector2(minX, minY);
            Max = new Vector2(maxX, maxY);
        }

        protected Vector2[] Bounds = new Vector2[4];
        protected Vector2 Min, Max;
        
        public abstract void Draw();
        
        public void Dispose()
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();
        }
    }
}
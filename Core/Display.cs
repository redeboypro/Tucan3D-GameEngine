using System;
using System.IO;
using System.Windows;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;
using OpenTK.Wpf;
using Tucan3D_GameEngine.Gui;
using Tucan3D_GameEngine.Rendering;
using Tucan3D_GameEngine.Rendering.Common;
using Tucan3D_GameEngine.WorldObjects;
using Tucan3D_GameEngine.WorldObjects.Common;

namespace Tucan3D_GameEngine.Core
{
    public class Display : GameWindow
    {
        protected Action LoadEvent;
        private ShaderData shaderData;

        public Display()
        {
            Title = "Tucan3D";
            Width = 800;
            Height = 600;
        }
        private Fbo fbo;

        protected override void OnLoad(EventArgs e)
        {
            GL.Enable(EnableCap.Blend);
            GL.Enable(EnableCap.CullFace);
            GL.Enable(EnableCap.Texture2D);
            GL.Enable(EnableCap.DepthTest);

            shaderData = new ShaderData();
            
            Gui.Gui.Initialize("font.png");

            fbo = new Fbo(800, 600, Fbo.DEPTH_RENDER_BUFFER);
            PostProcessing.Initialize();
            
            LoadEvent?.Invoke();
            
            SceneExtension.OnLoad();
        }

        protected override void OnMouseDown(MouseButtonEventArgs e)
        {
            Gui.Gui.OnMouseDown(e);
        }

        protected override void OnMouseUp(MouseButtonEventArgs e)
        {
            Gui.Gui.OnMouseUp(e);
        }

        protected override void OnMouseMove(MouseMoveEventArgs e)
        {
            Gui.Gui.OnMouseMove(e);
        }

        protected override void OnUpdateFrame(FrameEventArgs args)
        {
            SceneExtension.OnUpdateFrame();
        }

        protected override void OnRenderFrame(FrameEventArgs args)
        {
            fbo.BindFrameBuffer();
            GL.ClearColor(Color4.CornflowerBlue);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            SceneExtension.OnRenderFrame(shaderData);
            fbo.UnbindFrameBuffer();
            
            PostProcessing.Start();
            PostProcessing.Update(fbo.ColourTexture);
            GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);
            Gui.Gui.DrawElements();
            PostProcessing.End();
            SwapBuffers();
        }
    }
}
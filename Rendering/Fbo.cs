using System;
using OpenTK.Graphics.OpenGL4;

namespace Tucan3D_GameEngine.Rendering.Common
{
	public class Fbo
	{
		public static int NONE = 0;
		public static int DEPTH_TEXTURE = 1;
		public static int DEPTH_RENDER_BUFFER = 2;

		private int width;
		private int height;

		private int frameBuffer;

		private int colourTexture;
		private int depthTexture;

		private int depthBuffer;

		public Fbo(int width, int height, int depthBufferType)
		{
			this.width = width;
			this.height = height;
			InitializeFrameBuffer(depthBufferType);
		}

		public void Clear()
		{
			GL.DeleteFramebuffers(1, ref frameBuffer);
			GL.DeleteTextures(1, ref colourTexture);
			GL.DeleteTextures(1, ref depthTexture);
			GL.DeleteRenderbuffers(1, ref depthBuffer);
		}

		public void BindFrameBuffer()
		{
			GL.BindFramebuffer(FramebufferTarget.Framebuffer, frameBuffer);
			GL.Viewport(0, 0, width, height);
		}

		public void UnbindFrameBuffer()
		{
			GL.BindFramebuffer(FramebufferTarget.Framebuffer, 0);
			GL.Viewport(0, 0, 800, 600);
		}

		public void BindToRead()
		{
			GL.BindTexture(TextureTarget.Texture2D, 0);
			GL.BindFramebuffer(FramebufferTarget.ReadFramebuffer, frameBuffer);
			GL.ReadBuffer(ReadBufferMode.ColorAttachment0);
		}

		public int ColourTexture => colourTexture;
		public int DepthTexture => depthTexture;

		private void InitializeFrameBuffer(int type)
		{
			CreateFrameBuffer();
			CreateTextureAttachment();
			if (type == DEPTH_RENDER_BUFFER)
			{
				CreateDepthBufferAttachment();
			}
			else if (type == DEPTH_TEXTURE)
			{
				CreateDepthTextureAttachment();
			}

			UnbindFrameBuffer();
		}

		private void CreateFrameBuffer()
		{
			GL.GenFramebuffers(1, out frameBuffer);
			GL.BindFramebuffer(FramebufferTarget.Framebuffer, frameBuffer);
			GL.DrawBuffer(DrawBufferMode.ColorAttachment0);
		}

		private void CreateTextureAttachment()
		{
			GL.GenTextures(1, out colourTexture);
			GL.BindTexture(TextureTarget.Texture2D, colourTexture);
			GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba8, width, height, 0, PixelFormat.Rgba,
				PixelType.UnsignedByte, (IntPtr) null);
			GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter,
				(int) TextureMagFilter.Linear);
			GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter,
				(int) TextureMagFilter.Linear);
			GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS,
				(int) TextureWrapMode.ClampToEdge);
			GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT,
				(int) TextureWrapMode.ClampToEdge);
			GL.FramebufferTexture2D(FramebufferTarget.Framebuffer, FramebufferAttachment.ColorAttachment0,
				TextureTarget.Texture2D, colourTexture, 0);
		}

		private void CreateDepthTextureAttachment()
		{
			GL.GenTextures(1, out depthTexture);
			GL.BindTexture(TextureTarget.Texture2D, depthTexture);
			GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.DepthComponent24, width, height, 0,
				PixelFormat.DepthComponent, PixelType.Float, (IntPtr) null);
			GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter,
				(int) TextureMagFilter.Linear);
			GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter,
				(int) TextureMagFilter.Linear);
			GL.FramebufferTexture2D(FramebufferTarget.Framebuffer, FramebufferAttachment.DepthAttachment,
				TextureTarget.Texture2D, depthTexture, 0);
		}

		private void CreateDepthBufferAttachment()
		{
			GL.GenRenderbuffers(1, out depthBuffer);
			GL.BindRenderbuffer(RenderbufferTarget.Renderbuffer, depthBuffer);
			GL.RenderbufferStorage(RenderbufferTarget.Renderbuffer, RenderbufferStorage.DepthComponent24, width,
				height);
			GL.FramebufferRenderbuffer(FramebufferTarget.Framebuffer, FramebufferAttachment.DepthAttachment,
				RenderbufferTarget.Renderbuffer, depthBuffer);
		}
	}
}
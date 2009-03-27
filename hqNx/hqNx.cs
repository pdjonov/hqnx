/******************************************************************************
	hqNx - GPU-gased (XNA) implementation of hq2x, hq3x, and hq4x
	Copyright (C) 2009 Philip Djonov

	This program is free software; you can redistribute it and/or modify it
	under the terms of the GNU General Public License as published by the Free
	Software Foundation; either version 2.1 of the License, or (at your option)
	any later version.

	This program is distributed in the hope that it will be useful, but
	WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY
	or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU General Public License
	for more details.

	You should have received a copy of the GNU General Public License along
	with this program; if not, write to the
	
		Free Software Foundation, Inc.
		51 Franklin Street, Fifth Floor
		Boston, MA  02110-1301, USA.
******************************************************************************/

using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace hqNx
{

	public partial class Hqnx : DrawableGameComponent
	{

		private Effect fx;
		private EffectPass patternPass, hq2xPass, hq3xPass, hq4xPass;
		private EffectParameter tapsParam, imageDimsParam;

		private Texture3D lut2x, lut3x, lut4x;
		private RenderTarget2D patternBuffer;
		private VertexDeclaration screenQuadDecl;

		private ContentManager content;

		#region Full-Screen Quad
		private struct ScreenQuadVert
		{
			public float x, y;
			public float u, v;

			public ScreenQuadVert( float x, float y, float u, float v )
			{
				this.x = x;
				this.y = y;
				this.u = u;
				this.v = v;
			}

			public static readonly VertexElement[] VertexElements = new VertexElement[]
				{
					new VertexElement( 0, 0, VertexElementFormat.Vector2, VertexElementMethod.Default, VertexElementUsage.Position, 0 ),
					new VertexElement( 0, 8, VertexElementFormat.Vector2, VertexElementMethod.Default, VertexElementUsage.TextureCoordinate, 0 ),
				};

			public static readonly int Stride = 16;
		}

		private ScreenQuadVert[] screenFill = new ScreenQuadVert[4];

		private void GenScreenQuad( Rectangle rc )
		{
			Viewport vp = GraphicsDevice.Viewport;

			float vpWs = 2.0F / vp.Width;
			float vpHs = -2.0F / vp.Height;

			screenFill[0] = new ScreenQuadVert( rc.X, rc.Y, 0, 0 );
			screenFill[1] = new ScreenQuadVert( rc.X + rc.Width, rc.Y, 1, 0 );
			screenFill[2] = new ScreenQuadVert( rc.X, rc.Y + rc.Height, 0, 1 );
			screenFill[3] = new ScreenQuadVert( rc.X + rc.Width, rc.Y + rc.Height, 1, 1 );

			for( int i = 0; i < 4; i++ )
			{
				screenFill[i].x = (screenFill[i].x - 0.5F) * vpWs - 1;
				screenFill[i].y = (screenFill[i].y - 0.5F) * vpHs + 1;
			}
		}
		#endregion

		public Hqnx( Game game )
			: base( game )
		{
		}

		public override void Initialize()
		{
			content = new ContentManager( Game.Services, "Content\\hqNx" );

			base.Initialize();
		}

		protected override void LoadContent()
		{
			base.LoadContent();

			screenQuadDecl = new VertexDeclaration( GraphicsDevice, ScreenQuadVert.VertexElements );

			//load up the effect

			fx = content.Load<Effect>( "hqNx" );

			//ToDo: split the ExtractPattern into multiple ps_2_0 passes
			//for ancient cards that can't handle ps_2_a, detect and handle here

			EffectTechnique tech = fx.Techniques["hqNx"];		
			patternPass = tech.Passes["ExtractPattern"];
			hq2xPass = tech.Passes["hq2x"];
			hq3xPass = tech.Passes["hq3x"];
			hq4xPass = tech.Passes["hq4x"];

			tapsParam = fx.Parameters["Taps"];
			imageDimsParam = fx.Parameters["ImageDims"];

			//create the blending kernel lookup tables
			//ToDo: generate these offline - they are static and this is a retarded waste of CPU

			//ToDo: support cards that can't handle NPOT textures (hq3x case)

			lut2x = new Texture3D( GraphicsDevice, 4 * 2, 16, 256, 1, TextureUsage.None, SurfaceFormat.Color );
			InitLutNx( lut2x, 2, hq2x.FilterCase );
			lut3x = new Texture3D( GraphicsDevice, 9 * 2, 16, 256, 1, TextureUsage.None, SurfaceFormat.Color );
			InitLutNx( lut3x, 3, hq3x.FilterCase );
			lut4x = new Texture3D( GraphicsDevice, 16 * 2, 16, 256, 1, TextureUsage.None, SurfaceFormat.Color );
			InitLutNx( lut4x, 4, hq4x.FilterCase );

			//bind them to the effect

			fx.Parameters["Lut2x"].SetValue( lut2x );
			fx.Parameters["Lut3x"].SetValue( lut3x );
			fx.Parameters["Lut4x"].SetValue( lut4x );
		}

		protected override void UnloadContent()
		{
			//null stuff out to make sure errors are noticed if
			//resources are touched during a load cycle

			preparedImage = null;

			Helpers.DisposeAndNull( ref patternBuffer );

			Helpers.DisposeAndNull( ref lut4x );
			Helpers.DisposeAndNull( ref lut3x );
			Helpers.DisposeAndNull( ref lut2x );

			Helpers.DisposeAndNull( ref screenQuadDecl );

			patternPass = null;
			hq2xPass = null;
			hq3xPass = null;
			hq4xPass = null;
			fx = null;

			content.Unload();

			base.UnloadContent();
		}

		private void RequirePatternBuffer( int dx, int dy )
		{
			//ToDo: augment the shader so that the pattern
			//buffer can be larger than the actual image,
			//allowing us to reuse it for multiple stretches

			//ToDo: support cards that can't handle NPOT textures
			//dx = Helpers.RoundUpToPow2( dx );
			//dy = Helpers.RoundUpToPow2( dy );

			if( patternBuffer == null || patternBuffer.Width != dx || patternBuffer.Height != dy )
			{
				Helpers.DisposeAndNull( ref patternBuffer );
				patternBuffer = new RenderTarget2D( GraphicsDevice, dx, dy, 1, SurfaceFormat.Color );
			}
		}

		private Vector2[] taps = new Vector2[9];
		private void SetupTaps( int width, int height )
		{
			float du = 1.0F / width;
			float dv = 1.0F / height;

			taps[0] = new Vector2( -du, -dv );
			taps[1] = new Vector2( 0, -dv );
			taps[2] = new Vector2( du, -dv );
			taps[3] = new Vector2( -du, 0 );
			taps[4] = new Vector2( 0, 0 );
			taps[5] = new Vector2( du, 0 );
			taps[6] = new Vector2( -du, dv );
			taps[7] = new Vector2( 0, dv );
			taps[8] = new Vector2( du, dv );

			tapsParam.SetValue( taps );
			imageDimsParam.SetValue( new Vector2( width, height ) );
		}

		private Texture2D preparedImage;

		/// <summary>
		/// Prepares an image for magnification.
		/// </summary>
		/// <param name="image"></param>
		/// <remarks>
		/// Note that this will change the render target, but will
		/// not revert it to its previous value - you must immediately
		/// set the desired next target after calling this function.
		/// </remarks>
		public void Prepare( Texture2D image )
		{
			if( image == null )
				throw new ArgumentNullException( "image" );

			SetupTaps( image.Width, image.Height );

			//setup to render to the pattern image
			RequirePatternBuffer( image.Width, image.Height );

			GraphicsDevice.DepthStencilBuffer = null;
			GraphicsDevice.SetRenderTarget( 0, patternBuffer );

			Viewport vp = new Viewport();
			vp.X = 0;
			vp.Y = 0;
			vp.Width = image.Width;
			vp.Height = image.Height;
			vp.MinDepth = 0;
			vp.MaxDepth = 1;

			GraphicsDevice.Viewport = vp;

			GraphicsDevice.Textures[0] = image;

			GraphicsDevice.VertexDeclaration = screenQuadDecl;

			fx.Begin();

			//extract the pattern bits
			GenScreenQuad( new Rectangle( 0, 0, image.Width, image.Height ) );

			patternPass.Begin();
			GraphicsDevice.DrawUserPrimitives( PrimitiveType.TriangleStrip, screenFill, 0, 2 );
			patternPass.End();

			fx.End();

			preparedImage = image;
		}

		public void Draw2x( Point origin )
		{
			if( preparedImage == null )
				throw new InvalidOperationException();

			MagnifyNx( origin, 2, hq2xPass );
		}

		public void Draw3x( Point origin )
		{
			if( preparedImage == null )
				throw new InvalidOperationException();

			MagnifyNx( origin, 3, hq3xPass );
		}

		public void Draw4x( Point origin )
		{
			if( preparedImage == null )
				throw new InvalidOperationException();

			MagnifyNx( origin, 4, hq4xPass );
		}

		private void MagnifyNx( Point origin, int n, EffectPass scalePass )
		{
			GraphicsDevice.VertexDeclaration = screenQuadDecl;

			fx.Begin();

			//bind the final target and the rendered pattern
			GraphicsDevice.Textures[0] = preparedImage;
			GraphicsDevice.Textures[1] = patternBuffer.GetTexture();

			//scale!
			GenScreenQuad( new Rectangle( origin.X, origin.Y, preparedImage.Width * n, preparedImage.Height * n ) );

			scalePass.Begin();
			GraphicsDevice.DrawUserPrimitives( PrimitiveType.TriangleStrip, screenFill, 0, 2 );
			scalePass.End();

			fx.End();

			GraphicsDevice.Textures[1] = null;
		}
	}

}
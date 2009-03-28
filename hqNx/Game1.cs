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
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;

namespace hqNx
{
	/// <summary>
	/// This is the main type for your game
	/// </summary>
	public class Game1 : Game
	{
		private GraphicsDeviceManager graphics;

		private Hqnx hqNx;

		public Game1()
		{
			graphics = new GraphicsDeviceManager( this );
			graphics.PreparingDeviceSettings += delegate( object sender, PreparingDeviceSettingsEventArgs e )
			{
				e.GraphicsDeviceInformation.PresentationParameters.EnableAutoDepthStencil = false;
			};

			graphics.SynchronizeWithVerticalRetrace = false;
			IsFixedTimeStep = false;

			Content.RootDirectory = "Content";

			hqNx = new Hqnx( this );
			Components.Add( hqNx );
		}

		#region Framerate stuff
		private ulong numFrames;

		private const int NumTimings = 30;

		private int updateTimeIdx;
		private TimeSpan[] updateTimes = new TimeSpan[NumTimings];
		private int drawTimeIdx;
		private TimeSpan[] drawTimes = new TimeSpan[NumTimings];

		private StringBuilder statusText = new StringBuilder( 256 );

		private TimeSpan Average( TimeSpan[] times )
		{
			TimeSpan ret = times[0];
			for( int i = 1; i < times.Length; i++ )
				ret += times[i];

			return new TimeSpan( ret.Ticks / times.Length );
		}

		private void UpdateFrameStats( GameTime gameTime )
		{
			updateTimes[updateTimeIdx] = gameTime.ElapsedGameTime;
			updateTimeIdx = (updateTimeIdx + 1) % updateTimes.Length;
		}

		private void DrawFrameStats( GameTime gameTime )
		{
			numFrames++;

			drawTimes[drawTimeIdx] = gameTime.ElapsedRealTime;
			drawTimeIdx = (drawTimeIdx + 1) % drawTimes.Length;

			statusText.Length = 0;

			if( IsFixedTimeStep )
			{
				TimeSpan avgUpdateTime = Average( updateTimes );
				TimeSpan avgDrawTime = Average( drawTimes );

				statusText.AppendFormat( "{0:00.0} ms/update, target: {1:00.0} ms/frame, hitting: {2:00.0} ms/frame",
					(float)avgUpdateTime.TotalMilliseconds, (float)TargetElapsedTime.TotalMilliseconds,
					(float)avgDrawTime.TotalMilliseconds );
			}
			else
			{
				TimeSpan avgDrawTime = Average( drawTimes );

				statusText.AppendFormat( "{0:00.0} ms/frame", (float)avgDrawTime.TotalMilliseconds );
			}

			spriteBatch.Begin();
			spriteBatch.DrawString( statsFont, statusText, new Vector2( 4, 4 ), Color.White );
			spriteBatch.End();
		}
		#endregion

		/// <summary>
		/// Allows the game to perform any initialization it needs to before starting to run.
		/// This is where it can query for any required services and load any non-graphic
		/// related content.  Calling base.Initialize will enumerate through any components
		/// and initialize them as well.
		/// </summary>
		protected override void Initialize()
		{
			// TODO: Add your initialization logic here

			base.Initialize();
		}

		private static readonly string[] TestImageNames =
		{
			"Tests\\test_original",
			"Tests\\mailbox_original",
			"Tests\\Sonic2-001",
			"Tests\\randam_orig",
		};

		private SpriteBatch spriteBatch;
		private SpriteFont statsFont;
		private Texture2D testImage;
		private Texture2D[] testImages = new Texture2D[TestImageNames.Length];
		private int testImageIndex = 0;

		/// <summary>
		/// LoadContent will be called once per game and is the place to load
		/// all of your content.
		/// </summary>
		protected override void LoadContent()
		{
			spriteBatch = new SpriteBatch( GraphicsDevice );
			
			statsFont = Content.Load<SpriteFont>( "StatsFont" );

			for( int i = 0; i < TestImageNames.Length; i++ )
				testImages[i] = Content.Load<Texture2D>( TestImageNames[i] );

			testImage = testImages[testImageIndex];
		}

		/// <summary>
		/// UnloadContent will be called once per game and is the place to unload
		/// all content.
		/// </summary>
		protected override void UnloadContent()
		{
			Array.Clear( testImages, 0, testImages.Length );
			testImage = null;
			
			statsFont = null;

			Helpers.DisposeAndNull( ref spriteBatch );
		}

		private GamePadState lastState;

		/// <summary>
		/// Allows the game to run logic such as updating the world,
		/// checking for collisions, gathering input, and playing audio.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		protected override void Update( GameTime gameTime )
		{
			GamePadState inState = GamePad.GetState( PlayerIndex.One );

			if( inState.Buttons.Back == ButtonState.Pressed )
				this.Exit();

			offset += inState.ThumbSticks.Right * new Vector2( -3, 3 ) * (float)gameTime.ElapsedGameTime.TotalMilliseconds;
			rotation += inState.ThumbSticks.Left.X * 0.003F * (float)gameTime.ElapsedGameTime.TotalMilliseconds;

			if( inState.Buttons.X == ButtonState.Pressed &&
				lastState.Buttons.X != ButtonState.Pressed )
			{
				testImageIndex = (testImageIndex + 1) % testImages.Length;
				testImage = testImages[testImageIndex];

				offset = Vector2.Zero;
				rotation = 0;
			}

			lastState = inState;

			UpdateFrameStats( gameTime );

			base.Update( gameTime );
		}

		private float rotation = 0;
		private Vector2 offset = Vector2.Zero;

		/// <summary>
		/// This is called when the game should draw itself.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		protected override void Draw( GameTime gameTime )
		{
			hqNx.Prepare( testImage );

			//preparing the image for stretching flips us to another render target,
			//restore to the backbuffer and get ready to draw
			GraphicsDevice.SetRenderTarget( 0, null );
			GraphicsDevice.Clear( Color.DimGray );

			int Pad = 10;
			int yMin = 40;
			Vector2 org = offset + new Vector2( Pad, yMin );

			Vector2 size = new Vector2( testImage.Width, testImage.Height );

			spriteBatch.Begin();
			spriteBatch.Draw( testImage, org, null, Color.White, rotation, size / 2.0F, 1, SpriteEffects.None, 0 );
			org.X += size.X * 1.5F + Pad;
			spriteBatch.End();

			hqNx.Draw2x( org, rotation, size * 2.0F / 2.0F );
			org.X += testImage.Width * 2.5F + Pad;

			hqNx.Draw3x( org, rotation, size * 3.0F / 2.0F );
			org.X += size.X * 3.5F + Pad;

			hqNx.Draw4x( org, rotation, size * 4.0F / 2.0F );
			org.X += size.X * 4.5F + Pad;

			DrawFrameStats( gameTime );

			base.Draw( gameTime );
		}
	}
}

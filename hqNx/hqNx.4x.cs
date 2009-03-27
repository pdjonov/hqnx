﻿/******************************************************************************
	hqNx - GPU-gased (XNA) implementation of hq2x, hq3x, and hq4x
	Copyright (C) 2009 Philip Djonov

	****
		This file contains functions *very* closely based on the demo
		implementation found in the original hqNx project, released under
		the GNU GPL 2.1 and originally copyrighted:
			Copyright (C) 2003 MaxSt ( maxst@hiend3d.com )
 
		http://www.hiend3d.com/hq4x.html
	****

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

	partial class Hqnx
	{
		private static class hq4x
		{
			private const int PixPitch = 10;
			private const int RowPitch = PixPitch * 4;
			private const int PatternPitch = RowPitch * 4;

			private static unsafe void PIXEL00_0( float* pOut ) { Interp0( pOut, 5 ); }
			private static unsafe void PIXEL00_11( float* pOut ) { Interp1( pOut, 5, 4 ); }
			private static unsafe void PIXEL00_12( float* pOut ) { Interp1( pOut, 5, 2 ); }
			private static unsafe void PIXEL00_20( float* pOut ) { Interp2( pOut, 5, 2, 4 ); }
			private static unsafe void PIXEL00_50( float* pOut ) { Interp5( pOut, 2, 4 ); }
			private static unsafe void PIXEL00_80( float* pOut ) { Interp8( pOut, 5, 1 ); }
			private static unsafe void PIXEL00_81( float* pOut ) { Interp8( pOut, 5, 4 ); }
			private static unsafe void PIXEL00_82( float* pOut ) { Interp8( pOut, 5, 2 ); }
			private static unsafe void PIXEL01_0( float* pOut ) { Interp0( pOut + PixPitch, 5 ); }
			private static unsafe void PIXEL01_10( float* pOut ) { Interp1( pOut + PixPitch, 5, 1 ); }
			private static unsafe void PIXEL01_12( float* pOut ) { Interp1( pOut + PixPitch, 5, 2 ); }
			private static unsafe void PIXEL01_14( float* pOut ) { Interp1( pOut + PixPitch, 2, 5 ); }
			private static unsafe void PIXEL01_21( float* pOut ) { Interp2( pOut + PixPitch, 2, 5, 4 ); }
			private static unsafe void PIXEL01_31( float* pOut ) { Interp3( pOut + PixPitch, 5, 4 ); }
			private static unsafe void PIXEL01_50( float* pOut ) { Interp5( pOut + PixPitch, 2, 5 ); }
			private static unsafe void PIXEL01_60( float* pOut ) { Interp6( pOut + PixPitch, 5, 2, 4 ); }
			private static unsafe void PIXEL01_61( float* pOut ) { Interp6( pOut + PixPitch, 5, 2, 1 ); }
			private static unsafe void PIXEL01_82( float* pOut ) { Interp8( pOut + PixPitch, 5, 2 ); }
			private static unsafe void PIXEL01_83( float* pOut ) { Interp8( pOut + PixPitch, 2, 4 ); }
			private static unsafe void PIXEL02_0( float* pOut ) { Interp0( pOut + PixPitch + PixPitch, 5 ); }
			private static unsafe void PIXEL02_10( float* pOut ) { Interp1( pOut + PixPitch + PixPitch, 5, 3 ); }
			private static unsafe void PIXEL02_11( float* pOut ) { Interp1( pOut + PixPitch + PixPitch, 5, 2 ); }
			private static unsafe void PIXEL02_13( float* pOut ) { Interp1( pOut + PixPitch + PixPitch, 2, 5 ); }
			private static unsafe void PIXEL02_21( float* pOut ) { Interp2( pOut + PixPitch + PixPitch, 2, 5, 6 ); }
			private static unsafe void PIXEL02_32( float* pOut ) { Interp3( pOut + PixPitch + PixPitch, 5, 6 ); }
			private static unsafe void PIXEL02_50( float* pOut ) { Interp5( pOut + PixPitch + PixPitch, 2, 5 ); }
			private static unsafe void PIXEL02_60( float* pOut ) { Interp6( pOut + PixPitch + PixPitch, 5, 2, 6 ); }
			private static unsafe void PIXEL02_61( float* pOut ) { Interp6( pOut + PixPitch + PixPitch, 5, 2, 3 ); }
			private static unsafe void PIXEL02_81( float* pOut ) { Interp8( pOut + PixPitch + PixPitch, 5, 2 ); }
			private static unsafe void PIXEL02_83( float* pOut ) { Interp8( pOut + PixPitch + PixPitch, 2, 6 ); }
			private static unsafe void PIXEL03_0( float* pOut ) { Interp0( pOut + PixPitch + PixPitch + PixPitch, 5 ); }
			private static unsafe void PIXEL03_11( float* pOut ) { Interp1( pOut + PixPitch + PixPitch + PixPitch, 5, 2 ); }
			private static unsafe void PIXEL03_12( float* pOut ) { Interp1( pOut + PixPitch + PixPitch + PixPitch, 5, 6 ); }
			private static unsafe void PIXEL03_20( float* pOut ) { Interp2( pOut + PixPitch + PixPitch + PixPitch, 5, 2, 6 ); }
			private static unsafe void PIXEL03_50( float* pOut ) { Interp5( pOut + PixPitch + PixPitch + PixPitch, 2, 6 ); }
			private static unsafe void PIXEL03_80( float* pOut ) { Interp8( pOut + PixPitch + PixPitch + PixPitch, 5, 3 ); }
			private static unsafe void PIXEL03_81( float* pOut ) { Interp8( pOut + PixPitch + PixPitch + PixPitch, 5, 2 ); }
			private static unsafe void PIXEL03_82( float* pOut ) { Interp8( pOut + PixPitch + PixPitch + PixPitch, 5, 6 ); }
			private static unsafe void PIXEL10_0( float* pOut ) { Interp0( pOut + RowPitch, 5 ); }
			private static unsafe void PIXEL10_10( float* pOut ) { Interp1( pOut + RowPitch, 5, 1 ); }
			private static unsafe void PIXEL10_11( float* pOut ) { Interp1( pOut + RowPitch, 5, 4 ); }
			private static unsafe void PIXEL10_13( float* pOut ) { Interp1( pOut + RowPitch, 4, 5 ); }
			private static unsafe void PIXEL10_21( float* pOut ) { Interp2( pOut + RowPitch, 4, 5, 2 ); }
			private static unsafe void PIXEL10_32( float* pOut ) { Interp3( pOut + RowPitch, 5, 2 ); }
			private static unsafe void PIXEL10_50( float* pOut ) { Interp5( pOut + RowPitch, 4, 5 ); }
			private static unsafe void PIXEL10_60( float* pOut ) { Interp6( pOut + RowPitch, 5, 4, 2 ); }
			private static unsafe void PIXEL10_61( float* pOut ) { Interp6( pOut + RowPitch, 5, 4, 1 ); }
			private static unsafe void PIXEL10_81( float* pOut ) { Interp8( pOut + RowPitch, 5, 4 ); }
			private static unsafe void PIXEL10_83( float* pOut ) { Interp8( pOut + RowPitch, 4, 2 ); }
			private static unsafe void PIXEL11_0( float* pOut ) { Interp0( pOut + RowPitch + PixPitch, 5 ); }
			private static unsafe void PIXEL11_30( float* pOut ) { Interp3( pOut + RowPitch + PixPitch, 5, 1 ); }
			private static unsafe void PIXEL11_31( float* pOut ) { Interp3( pOut + RowPitch + PixPitch, 5, 4 ); }
			private static unsafe void PIXEL11_32( float* pOut ) { Interp3( pOut + RowPitch + PixPitch, 5, 2 ); }
			private static unsafe void PIXEL11_70( float* pOut ) { Interp7( pOut + RowPitch + PixPitch, 5, 4, 2 ); }
			private static unsafe void PIXEL12_0( float* pOut ) { Interp0( pOut + RowPitch + PixPitch + PixPitch, 5 ); }
			private static unsafe void PIXEL12_30( float* pOut ) { Interp3( pOut + RowPitch + PixPitch + PixPitch, 5, 3 ); }
			private static unsafe void PIXEL12_31( float* pOut ) { Interp3( pOut + RowPitch + PixPitch + PixPitch, 5, 2 ); }
			private static unsafe void PIXEL12_32( float* pOut ) { Interp3( pOut + RowPitch + PixPitch + PixPitch, 5, 6 ); }
			private static unsafe void PIXEL12_70( float* pOut ) { Interp7( pOut + RowPitch + PixPitch + PixPitch, 5, 6, 2 ); }
			private static unsafe void PIXEL13_0( float* pOut ) { Interp0( pOut + RowPitch + PixPitch + PixPitch + PixPitch, 5 ); }
			private static unsafe void PIXEL13_10( float* pOut ) { Interp1( pOut + RowPitch + PixPitch + PixPitch + PixPitch, 5, 3 ); }
			private static unsafe void PIXEL13_12( float* pOut ) { Interp1( pOut + RowPitch + PixPitch + PixPitch + PixPitch, 5, 6 ); }
			private static unsafe void PIXEL13_14( float* pOut ) { Interp1( pOut + RowPitch + PixPitch + PixPitch + PixPitch, 6, 5 ); }
			private static unsafe void PIXEL13_21( float* pOut ) { Interp2( pOut + RowPitch + PixPitch + PixPitch + PixPitch, 6, 5, 2 ); }
			private static unsafe void PIXEL13_31( float* pOut ) { Interp3( pOut + RowPitch + PixPitch + PixPitch + PixPitch, 5, 2 ); }
			private static unsafe void PIXEL13_50( float* pOut ) { Interp5( pOut + RowPitch + PixPitch + PixPitch + PixPitch, 6, 5 ); }
			private static unsafe void PIXEL13_60( float* pOut ) { Interp6( pOut + RowPitch + PixPitch + PixPitch + PixPitch, 5, 6, 2 ); }
			private static unsafe void PIXEL13_61( float* pOut ) { Interp6( pOut + RowPitch + PixPitch + PixPitch + PixPitch, 5, 6, 3 ); }
			private static unsafe void PIXEL13_82( float* pOut ) { Interp8( pOut + RowPitch + PixPitch + PixPitch + PixPitch, 5, 6 ); }
			private static unsafe void PIXEL13_83( float* pOut ) { Interp8( pOut + RowPitch + PixPitch + PixPitch + PixPitch, 6, 2 ); }
			private static unsafe void PIXEL20_0( float* pOut ) { Interp0( pOut + RowPitch + RowPitch, 5 ); }
			private static unsafe void PIXEL20_10( float* pOut ) { Interp1( pOut + RowPitch + RowPitch, 5, 7 ); }
			private static unsafe void PIXEL20_12( float* pOut ) { Interp1( pOut + RowPitch + RowPitch, 5, 4 ); }
			private static unsafe void PIXEL20_14( float* pOut ) { Interp1( pOut + RowPitch + RowPitch, 4, 5 ); }
			private static unsafe void PIXEL20_21( float* pOut ) { Interp2( pOut + RowPitch + RowPitch, 4, 5, 8 ); }
			private static unsafe void PIXEL20_31( float* pOut ) { Interp3( pOut + RowPitch + RowPitch, 5, 8 ); }
			private static unsafe void PIXEL20_50( float* pOut ) { Interp5( pOut + RowPitch + RowPitch, 4, 5 ); }
			private static unsafe void PIXEL20_60( float* pOut ) { Interp6( pOut + RowPitch + RowPitch, 5, 4, 8 ); }
			private static unsafe void PIXEL20_61( float* pOut ) { Interp6( pOut + RowPitch + RowPitch, 5, 4, 7 ); }
			private static unsafe void PIXEL20_82( float* pOut ) { Interp8( pOut + RowPitch + RowPitch, 5, 4 ); }
			private static unsafe void PIXEL20_83( float* pOut ) { Interp8( pOut + RowPitch + RowPitch, 4, 8 ); }
			private static unsafe void PIXEL21_0( float* pOut ) { Interp0( pOut + RowPitch + RowPitch + PixPitch, 5 ); }
			private static unsafe void PIXEL21_30( float* pOut ) { Interp3( pOut + RowPitch + RowPitch + PixPitch, 5, 7 ); }
			private static unsafe void PIXEL21_31( float* pOut ) { Interp3( pOut + RowPitch + RowPitch + PixPitch, 5, 8 ); }
			private static unsafe void PIXEL21_32( float* pOut ) { Interp3( pOut + RowPitch + RowPitch + PixPitch, 5, 4 ); }
			private static unsafe void PIXEL21_70( float* pOut ) { Interp7( pOut + RowPitch + RowPitch + PixPitch, 5, 4, 8 ); }
			private static unsafe void PIXEL22_0( float* pOut ) { Interp0( pOut + RowPitch + RowPitch + PixPitch + PixPitch, 5 ); }
			private static unsafe void PIXEL22_30( float* pOut ) { Interp3( pOut + RowPitch + RowPitch + PixPitch + PixPitch, 5, 9 ); }
			private static unsafe void PIXEL22_31( float* pOut ) { Interp3( pOut + RowPitch + RowPitch + PixPitch + PixPitch, 5, 6 ); }
			private static unsafe void PIXEL22_32( float* pOut ) { Interp3( pOut + RowPitch + RowPitch + PixPitch + PixPitch, 5, 8 ); }
			private static unsafe void PIXEL22_70( float* pOut ) { Interp7( pOut + RowPitch + RowPitch + PixPitch + PixPitch, 5, 6, 8 ); }
			private static unsafe void PIXEL23_0( float* pOut ) { Interp0( pOut + RowPitch + RowPitch + PixPitch + PixPitch + PixPitch, 5 ); }
			private static unsafe void PIXEL23_10( float* pOut ) { Interp1( pOut + RowPitch + RowPitch + PixPitch + PixPitch + PixPitch, 5, 9 ); }
			private static unsafe void PIXEL23_11( float* pOut ) { Interp1( pOut + RowPitch + RowPitch + PixPitch + PixPitch + PixPitch, 5, 6 ); }
			private static unsafe void PIXEL23_13( float* pOut ) { Interp1( pOut + RowPitch + RowPitch + PixPitch + PixPitch + PixPitch, 6, 5 ); }
			private static unsafe void PIXEL23_21( float* pOut ) { Interp2( pOut + RowPitch + RowPitch + PixPitch + PixPitch + PixPitch, 6, 5, 8 ); }
			private static unsafe void PIXEL23_32( float* pOut ) { Interp3( pOut + RowPitch + RowPitch + PixPitch + PixPitch + PixPitch, 5, 8 ); }
			private static unsafe void PIXEL23_50( float* pOut ) { Interp5( pOut + RowPitch + RowPitch + PixPitch + PixPitch + PixPitch, 6, 5 ); }
			private static unsafe void PIXEL23_60( float* pOut ) { Interp6( pOut + RowPitch + RowPitch + PixPitch + PixPitch + PixPitch, 5, 6, 8 ); }
			private static unsafe void PIXEL23_61( float* pOut ) { Interp6( pOut + RowPitch + RowPitch + PixPitch + PixPitch + PixPitch, 5, 6, 9 ); }
			private static unsafe void PIXEL23_81( float* pOut ) { Interp8( pOut + RowPitch + RowPitch + PixPitch + PixPitch + PixPitch, 5, 6 ); }
			private static unsafe void PIXEL23_83( float* pOut ) { Interp8( pOut + RowPitch + RowPitch + PixPitch + PixPitch + PixPitch, 6, 8 ); }
			private static unsafe void PIXEL30_0( float* pOut ) { Interp0( pOut + RowPitch + RowPitch + RowPitch, 5 ); }
			private static unsafe void PIXEL30_11( float* pOut ) { Interp1( pOut + RowPitch + RowPitch + RowPitch, 5, 8 ); }
			private static unsafe void PIXEL30_12( float* pOut ) { Interp1( pOut + RowPitch + RowPitch + RowPitch, 5, 4 ); }
			private static unsafe void PIXEL30_20( float* pOut ) { Interp2( pOut + RowPitch + RowPitch + RowPitch, 5, 8, 4 ); }
			private static unsafe void PIXEL30_50( float* pOut ) { Interp5( pOut + RowPitch + RowPitch + RowPitch, 8, 4 ); }
			private static unsafe void PIXEL30_80( float* pOut ) { Interp8( pOut + RowPitch + RowPitch + RowPitch, 5, 7 ); }
			private static unsafe void PIXEL30_81( float* pOut ) { Interp8( pOut + RowPitch + RowPitch + RowPitch, 5, 8 ); }
			private static unsafe void PIXEL30_82( float* pOut ) { Interp8( pOut + RowPitch + RowPitch + RowPitch, 5, 4 ); }
			private static unsafe void PIXEL31_0( float* pOut ) { Interp0( pOut + RowPitch + RowPitch + RowPitch + PixPitch, 5 ); }
			private static unsafe void PIXEL31_10( float* pOut ) { Interp1( pOut + RowPitch + RowPitch + RowPitch + PixPitch, 5, 7 ); }
			private static unsafe void PIXEL31_11( float* pOut ) { Interp1( pOut + RowPitch + RowPitch + RowPitch + PixPitch, 5, 8 ); }
			private static unsafe void PIXEL31_13( float* pOut ) { Interp1( pOut + RowPitch + RowPitch + RowPitch + PixPitch, 8, 5 ); }
			private static unsafe void PIXEL31_21( float* pOut ) { Interp2( pOut + RowPitch + RowPitch + RowPitch + PixPitch, 8, 5, 4 ); }
			private static unsafe void PIXEL31_32( float* pOut ) { Interp3( pOut + RowPitch + RowPitch + RowPitch + PixPitch, 5, 4 ); }
			private static unsafe void PIXEL31_50( float* pOut ) { Interp5( pOut + RowPitch + RowPitch + RowPitch + PixPitch, 8, 5 ); }
			private static unsafe void PIXEL31_60( float* pOut ) { Interp6( pOut + RowPitch + RowPitch + RowPitch + PixPitch, 5, 8, 4 ); }
			private static unsafe void PIXEL31_61( float* pOut ) { Interp6( pOut + RowPitch + RowPitch + RowPitch + PixPitch, 5, 8, 7 ); }
			private static unsafe void PIXEL31_81( float* pOut ) { Interp8( pOut + RowPitch + RowPitch + RowPitch + PixPitch, 5, 8 ); }
			private static unsafe void PIXEL31_83( float* pOut ) { Interp8( pOut + RowPitch + RowPitch + RowPitch + PixPitch, 8, 4 ); }
			private static unsafe void PIXEL32_0( float* pOut ) { Interp0( pOut + RowPitch + RowPitch + RowPitch + PixPitch + PixPitch, 5 ); }
			private static unsafe void PIXEL32_10( float* pOut ) { Interp1( pOut + RowPitch + RowPitch + RowPitch + PixPitch + PixPitch, 5, 9 ); }
			private static unsafe void PIXEL32_12( float* pOut ) { Interp1( pOut + RowPitch + RowPitch + RowPitch + PixPitch + PixPitch, 5, 8 ); }
			private static unsafe void PIXEL32_14( float* pOut ) { Interp1( pOut + RowPitch + RowPitch + RowPitch + PixPitch + PixPitch, 8, 5 ); }
			private static unsafe void PIXEL32_21( float* pOut ) { Interp2( pOut + RowPitch + RowPitch + RowPitch + PixPitch + PixPitch, 8, 5, 6 ); }
			private static unsafe void PIXEL32_31( float* pOut ) { Interp3( pOut + RowPitch + RowPitch + RowPitch + PixPitch + PixPitch, 5, 6 ); }
			private static unsafe void PIXEL32_50( float* pOut ) { Interp5( pOut + RowPitch + RowPitch + RowPitch + PixPitch + PixPitch, 8, 5 ); }
			private static unsafe void PIXEL32_60( float* pOut ) { Interp6( pOut + RowPitch + RowPitch + RowPitch + PixPitch + PixPitch, 5, 8, 6 ); }
			private static unsafe void PIXEL32_61( float* pOut ) { Interp6( pOut + RowPitch + RowPitch + RowPitch + PixPitch + PixPitch, 5, 8, 9 ); }
			private static unsafe void PIXEL32_82( float* pOut ) { Interp8( pOut + RowPitch + RowPitch + RowPitch + PixPitch + PixPitch, 5, 8 ); }
			private static unsafe void PIXEL32_83( float* pOut ) { Interp8( pOut + RowPitch + RowPitch + RowPitch + PixPitch + PixPitch, 8, 6 ); }
			private static unsafe void PIXEL33_0( float* pOut ) { Interp0( pOut + RowPitch + RowPitch + RowPitch + PixPitch + PixPitch + PixPitch, 5 ); }
			private static unsafe void PIXEL33_11( float* pOut ) { Interp1( pOut + RowPitch + RowPitch + RowPitch + PixPitch + PixPitch + PixPitch, 5, 6 ); }
			private static unsafe void PIXEL33_12( float* pOut ) { Interp1( pOut + RowPitch + RowPitch + RowPitch + PixPitch + PixPitch + PixPitch, 5, 8 ); }
			private static unsafe void PIXEL33_20( float* pOut ) { Interp2( pOut + RowPitch + RowPitch + RowPitch + PixPitch + PixPitch + PixPitch, 5, 8, 6 ); }
			private static unsafe void PIXEL33_50( float* pOut ) { Interp5( pOut + RowPitch + RowPitch + RowPitch + PixPitch + PixPitch + PixPitch, 8, 6 ); }
			private static unsafe void PIXEL33_80( float* pOut ) { Interp8( pOut + RowPitch + RowPitch + RowPitch + PixPitch + PixPitch + PixPitch, 5, 9 ); }
			private static unsafe void PIXEL33_81( float* pOut ) { Interp8( pOut + RowPitch + RowPitch + RowPitch + PixPitch + PixPitch + PixPitch, 5, 6 ); }
			private static unsafe void PIXEL33_82( float* pOut ) { Interp8( pOut + RowPitch + RowPitch + RowPitch + PixPitch + PixPitch + PixPitch, 5, 8 ); }

			private static unsafe void InternalFilterCase( float* pOut, int pattern, int cmp2 )
			{
				switch( pattern )
				{
				case 0:
				case 1:
				case 4:
				case 32:
				case 128:
				case 5:
				case 132:
				case 160:
				case 33:
				case 129:
				case 36:
				case 133:
				case 164:
				case 161:
				case 37:
				case 165:
					{
						PIXEL00_20( pOut );
						PIXEL01_60( pOut );
						PIXEL02_60( pOut );
						PIXEL03_20( pOut );
						PIXEL10_60( pOut );
						PIXEL11_70( pOut );
						PIXEL12_70( pOut );
						PIXEL13_60( pOut );
						PIXEL20_60( pOut );
						PIXEL21_70( pOut );
						PIXEL22_70( pOut );
						PIXEL23_60( pOut );
						PIXEL30_20( pOut );
						PIXEL31_60( pOut );
						PIXEL32_60( pOut );
						PIXEL33_20( pOut );
						break;
					}
				case 2:
				case 34:
				case 130:
				case 162:
					{
						PIXEL00_80( pOut );
						PIXEL01_10( pOut );
						PIXEL02_10( pOut );
						PIXEL03_80( pOut );
						PIXEL10_61( pOut );
						PIXEL11_30( pOut );
						PIXEL12_30( pOut );
						PIXEL13_61( pOut );
						PIXEL20_60( pOut );
						PIXEL21_70( pOut );
						PIXEL22_70( pOut );
						PIXEL23_60( pOut );
						PIXEL30_20( pOut );
						PIXEL31_60( pOut );
						PIXEL32_60( pOut );
						PIXEL33_20( pOut );
						break;
					}
				case 16:
				case 17:
				case 48:
				case 49:
					{
						PIXEL00_20( pOut );
						PIXEL01_60( pOut );
						PIXEL02_61( pOut );
						PIXEL03_80( pOut );
						PIXEL10_60( pOut );
						PIXEL11_70( pOut );
						PIXEL12_30( pOut );
						PIXEL13_10( pOut );
						PIXEL20_60( pOut );
						PIXEL21_70( pOut );
						PIXEL22_30( pOut );
						PIXEL23_10( pOut );
						PIXEL30_20( pOut );
						PIXEL31_60( pOut );
						PIXEL32_61( pOut );
						PIXEL33_80( pOut );
						break;
					}
				case 64:
				case 65:
				case 68:
				case 69:
					{
						PIXEL00_20( pOut );
						PIXEL01_60( pOut );
						PIXEL02_60( pOut );
						PIXEL03_20( pOut );
						PIXEL10_60( pOut );
						PIXEL11_70( pOut );
						PIXEL12_70( pOut );
						PIXEL13_60( pOut );
						PIXEL20_61( pOut );
						PIXEL21_30( pOut );
						PIXEL22_30( pOut );
						PIXEL23_61( pOut );
						PIXEL30_80( pOut );
						PIXEL31_10( pOut );
						PIXEL32_10( pOut );
						PIXEL33_80( pOut );
						break;
					}
				case 8:
				case 12:
				case 136:
				case 140:
					{
						PIXEL00_80( pOut );
						PIXEL01_61( pOut );
						PIXEL02_60( pOut );
						PIXEL03_20( pOut );
						PIXEL10_10( pOut );
						PIXEL11_30( pOut );
						PIXEL12_70( pOut );
						PIXEL13_60( pOut );
						PIXEL20_10( pOut );
						PIXEL21_30( pOut );
						PIXEL22_70( pOut );
						PIXEL23_60( pOut );
						PIXEL30_80( pOut );
						PIXEL31_61( pOut );
						PIXEL32_60( pOut );
						PIXEL33_20( pOut );
						break;
					}
				case 3:
				case 35:
				case 131:
				case 163:
					{
						PIXEL00_81( pOut );
						PIXEL01_31( pOut );
						PIXEL02_10( pOut );
						PIXEL03_80( pOut );
						PIXEL10_81( pOut );
						PIXEL11_31( pOut );
						PIXEL12_30( pOut );
						PIXEL13_61( pOut );
						PIXEL20_60( pOut );
						PIXEL21_70( pOut );
						PIXEL22_70( pOut );
						PIXEL23_60( pOut );
						PIXEL30_20( pOut );
						PIXEL31_60( pOut );
						PIXEL32_60( pOut );
						PIXEL33_20( pOut );
						break;
					}
				case 6:
				case 38:
				case 134:
				case 166:
					{
						PIXEL00_80( pOut );
						PIXEL01_10( pOut );
						PIXEL02_32( pOut );
						PIXEL03_82( pOut );
						PIXEL10_61( pOut );
						PIXEL11_30( pOut );
						PIXEL12_32( pOut );
						PIXEL13_82( pOut );
						PIXEL20_60( pOut );
						PIXEL21_70( pOut );
						PIXEL22_70( pOut );
						PIXEL23_60( pOut );
						PIXEL30_20( pOut );
						PIXEL31_60( pOut );
						PIXEL32_60( pOut );
						PIXEL33_20( pOut );
						break;
					}
				case 20:
				case 21:
				case 52:
				case 53:
					{
						PIXEL00_20( pOut );
						PIXEL01_60( pOut );
						PIXEL02_81( pOut );
						PIXEL03_81( pOut );
						PIXEL10_60( pOut );
						PIXEL11_70( pOut );
						PIXEL12_31( pOut );
						PIXEL13_31( pOut );
						PIXEL20_60( pOut );
						PIXEL21_70( pOut );
						PIXEL22_30( pOut );
						PIXEL23_10( pOut );
						PIXEL30_20( pOut );
						PIXEL31_60( pOut );
						PIXEL32_61( pOut );
						PIXEL33_80( pOut );
						break;
					}
				case 144:
				case 145:
				case 176:
				case 177:
					{
						PIXEL00_20( pOut );
						PIXEL01_60( pOut );
						PIXEL02_61( pOut );
						PIXEL03_80( pOut );
						PIXEL10_60( pOut );
						PIXEL11_70( pOut );
						PIXEL12_30( pOut );
						PIXEL13_10( pOut );
						PIXEL20_60( pOut );
						PIXEL21_70( pOut );
						PIXEL22_32( pOut );
						PIXEL23_32( pOut );
						PIXEL30_20( pOut );
						PIXEL31_60( pOut );
						PIXEL32_82( pOut );
						PIXEL33_82( pOut );
						break;
					}
				case 192:
				case 193:
				case 196:
				case 197:
					{
						PIXEL00_20( pOut );
						PIXEL01_60( pOut );
						PIXEL02_60( pOut );
						PIXEL03_20( pOut );
						PIXEL10_60( pOut );
						PIXEL11_70( pOut );
						PIXEL12_70( pOut );
						PIXEL13_60( pOut );
						PIXEL20_61( pOut );
						PIXEL21_30( pOut );
						PIXEL22_31( pOut );
						PIXEL23_81( pOut );
						PIXEL30_80( pOut );
						PIXEL31_10( pOut );
						PIXEL32_31( pOut );
						PIXEL33_81( pOut );
						break;
					}
				case 96:
				case 97:
				case 100:
				case 101:
					{
						PIXEL00_20( pOut );
						PIXEL01_60( pOut );
						PIXEL02_60( pOut );
						PIXEL03_20( pOut );
						PIXEL10_60( pOut );
						PIXEL11_70( pOut );
						PIXEL12_70( pOut );
						PIXEL13_60( pOut );
						PIXEL20_82( pOut );
						PIXEL21_32( pOut );
						PIXEL22_30( pOut );
						PIXEL23_61( pOut );
						PIXEL30_82( pOut );
						PIXEL31_32( pOut );
						PIXEL32_10( pOut );
						PIXEL33_80( pOut );
						break;
					}
				case 40:
				case 44:
				case 168:
				case 172:
					{
						PIXEL00_80( pOut );
						PIXEL01_61( pOut );
						PIXEL02_60( pOut );
						PIXEL03_20( pOut );
						PIXEL10_10( pOut );
						PIXEL11_30( pOut );
						PIXEL12_70( pOut );
						PIXEL13_60( pOut );
						PIXEL20_31( pOut );
						PIXEL21_31( pOut );
						PIXEL22_70( pOut );
						PIXEL23_60( pOut );
						PIXEL30_81( pOut );
						PIXEL31_81( pOut );
						PIXEL32_60( pOut );
						PIXEL33_20( pOut );
						break;
					}
				case 9:
				case 13:
				case 137:
				case 141:
					{
						PIXEL00_82( pOut );
						PIXEL01_82( pOut );
						PIXEL02_60( pOut );
						PIXEL03_20( pOut );
						PIXEL10_32( pOut );
						PIXEL11_32( pOut );
						PIXEL12_70( pOut );
						PIXEL13_60( pOut );
						PIXEL20_10( pOut );
						PIXEL21_30( pOut );
						PIXEL22_70( pOut );
						PIXEL23_60( pOut );
						PIXEL30_80( pOut );
						PIXEL31_61( pOut );
						PIXEL32_60( pOut );
						PIXEL33_20( pOut );
						break;
					}
				case 18:
				case 50:
					{
						PIXEL00_80( pOut );
						PIXEL01_10( pOut );
						if( Diff( cmp2, 2, 6 ) )
						{
							PIXEL02_10( pOut );
							PIXEL03_80( pOut );
							PIXEL12_30( pOut );
							PIXEL13_10( pOut );
						}
						else
						{
							PIXEL02_50( pOut );
							PIXEL03_50( pOut );
							PIXEL12_0( pOut );
							PIXEL13_50( pOut );
						}
						PIXEL10_61( pOut );
						PIXEL11_30( pOut );
						PIXEL20_60( pOut );
						PIXEL21_70( pOut );
						PIXEL22_30( pOut );
						PIXEL23_10( pOut );
						PIXEL30_20( pOut );
						PIXEL31_60( pOut );
						PIXEL32_61( pOut );
						PIXEL33_80( pOut );
						break;
					}
				case 80:
				case 81:
					{
						PIXEL00_20( pOut );
						PIXEL01_60( pOut );
						PIXEL02_61( pOut );
						PIXEL03_80( pOut );
						PIXEL10_60( pOut );
						PIXEL11_70( pOut );
						PIXEL12_30( pOut );
						PIXEL13_10( pOut );
						PIXEL20_61( pOut );
						PIXEL21_30( pOut );
						if( Diff( cmp2, 6, 8 ) )
						{
							PIXEL22_30( pOut );
							PIXEL23_10( pOut );
							PIXEL32_10( pOut );
							PIXEL33_80( pOut );
						}
						else
						{
							PIXEL22_0( pOut );
							PIXEL23_50( pOut );
							PIXEL32_50( pOut );
							PIXEL33_50( pOut );
						}
						PIXEL30_80( pOut );
						PIXEL31_10( pOut );
						break;
					}
				case 72:
				case 76:
					{
						PIXEL00_80( pOut );
						PIXEL01_61( pOut );
						PIXEL02_60( pOut );
						PIXEL03_20( pOut );
						PIXEL10_10( pOut );
						PIXEL11_30( pOut );
						PIXEL12_70( pOut );
						PIXEL13_60( pOut );
						if( Diff( cmp2, 8, 4 ) )
						{
							PIXEL20_10( pOut );
							PIXEL21_30( pOut );
							PIXEL30_80( pOut );
							PIXEL31_10( pOut );
						}
						else
						{
							PIXEL20_50( pOut );
							PIXEL21_0( pOut );
							PIXEL30_50( pOut );
							PIXEL31_50( pOut );
						}
						PIXEL22_30( pOut );
						PIXEL23_61( pOut );
						PIXEL32_10( pOut );
						PIXEL33_80( pOut );
						break;
					}
				case 10:
				case 138:
					{
						if( Diff( cmp2, 4, 2 ) )
						{
							PIXEL00_80( pOut );
							PIXEL01_10( pOut );
							PIXEL10_10( pOut );
							PIXEL11_30( pOut );
						}
						else
						{
							PIXEL00_50( pOut );
							PIXEL01_50( pOut );
							PIXEL10_50( pOut );
							PIXEL11_0( pOut );
						}
						PIXEL02_10( pOut );
						PIXEL03_80( pOut );
						PIXEL12_30( pOut );
						PIXEL13_61( pOut );
						PIXEL20_10( pOut );
						PIXEL21_30( pOut );
						PIXEL22_70( pOut );
						PIXEL23_60( pOut );
						PIXEL30_80( pOut );
						PIXEL31_61( pOut );
						PIXEL32_60( pOut );
						PIXEL33_20( pOut );
						break;
					}
				case 66:
					{
						PIXEL00_80( pOut );
						PIXEL01_10( pOut );
						PIXEL02_10( pOut );
						PIXEL03_80( pOut );
						PIXEL10_61( pOut );
						PIXEL11_30( pOut );
						PIXEL12_30( pOut );
						PIXEL13_61( pOut );
						PIXEL20_61( pOut );
						PIXEL21_30( pOut );
						PIXEL22_30( pOut );
						PIXEL23_61( pOut );
						PIXEL30_80( pOut );
						PIXEL31_10( pOut );
						PIXEL32_10( pOut );
						PIXEL33_80( pOut );
						break;
					}
				case 24:
					{
						PIXEL00_80( pOut );
						PIXEL01_61( pOut );
						PIXEL02_61( pOut );
						PIXEL03_80( pOut );
						PIXEL10_10( pOut );
						PIXEL11_30( pOut );
						PIXEL12_30( pOut );
						PIXEL13_10( pOut );
						PIXEL20_10( pOut );
						PIXEL21_30( pOut );
						PIXEL22_30( pOut );
						PIXEL23_10( pOut );
						PIXEL30_80( pOut );
						PIXEL31_61( pOut );
						PIXEL32_61( pOut );
						PIXEL33_80( pOut );
						break;
					}
				case 7:
				case 39:
				case 135:
					{
						PIXEL00_81( pOut );
						PIXEL01_31( pOut );
						PIXEL02_32( pOut );
						PIXEL03_82( pOut );
						PIXEL10_81( pOut );
						PIXEL11_31( pOut );
						PIXEL12_32( pOut );
						PIXEL13_82( pOut );
						PIXEL20_60( pOut );
						PIXEL21_70( pOut );
						PIXEL22_70( pOut );
						PIXEL23_60( pOut );
						PIXEL30_20( pOut );
						PIXEL31_60( pOut );
						PIXEL32_60( pOut );
						PIXEL33_20( pOut );
						break;
					}
				case 148:
				case 149:
				case 180:
					{
						PIXEL00_20( pOut );
						PIXEL01_60( pOut );
						PIXEL02_81( pOut );
						PIXEL03_81( pOut );
						PIXEL10_60( pOut );
						PIXEL11_70( pOut );
						PIXEL12_31( pOut );
						PIXEL13_31( pOut );
						PIXEL20_60( pOut );
						PIXEL21_70( pOut );
						PIXEL22_32( pOut );
						PIXEL23_32( pOut );
						PIXEL30_20( pOut );
						PIXEL31_60( pOut );
						PIXEL32_82( pOut );
						PIXEL33_82( pOut );
						break;
					}
				case 224:
				case 228:
				case 225:
					{
						PIXEL00_20( pOut );
						PIXEL01_60( pOut );
						PIXEL02_60( pOut );
						PIXEL03_20( pOut );
						PIXEL10_60( pOut );
						PIXEL11_70( pOut );
						PIXEL12_70( pOut );
						PIXEL13_60( pOut );
						PIXEL20_82( pOut );
						PIXEL21_32( pOut );
						PIXEL22_31( pOut );
						PIXEL23_81( pOut );
						PIXEL30_82( pOut );
						PIXEL31_32( pOut );
						PIXEL32_31( pOut );
						PIXEL33_81( pOut );
						break;
					}
				case 41:
				case 169:
				case 45:
					{
						PIXEL00_82( pOut );
						PIXEL01_82( pOut );
						PIXEL02_60( pOut );
						PIXEL03_20( pOut );
						PIXEL10_32( pOut );
						PIXEL11_32( pOut );
						PIXEL12_70( pOut );
						PIXEL13_60( pOut );
						PIXEL20_31( pOut );
						PIXEL21_31( pOut );
						PIXEL22_70( pOut );
						PIXEL23_60( pOut );
						PIXEL30_81( pOut );
						PIXEL31_81( pOut );
						PIXEL32_60( pOut );
						PIXEL33_20( pOut );
						break;
					}
				case 22:
				case 54:
					{
						PIXEL00_80( pOut );
						PIXEL01_10( pOut );
						if( Diff( cmp2, 2, 6 ) )
						{
							PIXEL02_0( pOut );
							PIXEL03_0( pOut );
							PIXEL13_0( pOut );
						}
						else
						{
							PIXEL02_50( pOut );
							PIXEL03_50( pOut );
							PIXEL13_50( pOut );
						}
						PIXEL10_61( pOut );
						PIXEL11_30( pOut );
						PIXEL12_0( pOut );
						PIXEL20_60( pOut );
						PIXEL21_70( pOut );
						PIXEL22_30( pOut );
						PIXEL23_10( pOut );
						PIXEL30_20( pOut );
						PIXEL31_60( pOut );
						PIXEL32_61( pOut );
						PIXEL33_80( pOut );
						break;
					}
				case 208:
				case 209:
					{
						PIXEL00_20( pOut );
						PIXEL01_60( pOut );
						PIXEL02_61( pOut );
						PIXEL03_80( pOut );
						PIXEL10_60( pOut );
						PIXEL11_70( pOut );
						PIXEL12_30( pOut );
						PIXEL13_10( pOut );
						PIXEL20_61( pOut );
						PIXEL21_30( pOut );
						PIXEL22_0( pOut );
						if( Diff( cmp2, 6, 8 ) )
						{
							PIXEL23_0( pOut );
							PIXEL32_0( pOut );
							PIXEL33_0( pOut );
						}
						else
						{
							PIXEL23_50( pOut );
							PIXEL32_50( pOut );
							PIXEL33_50( pOut );
						}
						PIXEL30_80( pOut );
						PIXEL31_10( pOut );
						break;
					}
				case 104:
				case 108:
					{
						PIXEL00_80( pOut );
						PIXEL01_61( pOut );
						PIXEL02_60( pOut );
						PIXEL03_20( pOut );
						PIXEL10_10( pOut );
						PIXEL11_30( pOut );
						PIXEL12_70( pOut );
						PIXEL13_60( pOut );
						if( Diff( cmp2, 8, 4 ) )
						{
							PIXEL20_0( pOut );
							PIXEL30_0( pOut );
							PIXEL31_0( pOut );
						}
						else
						{
							PIXEL20_50( pOut );
							PIXEL30_50( pOut );
							PIXEL31_50( pOut );
						}
						PIXEL21_0( pOut );
						PIXEL22_30( pOut );
						PIXEL23_61( pOut );
						PIXEL32_10( pOut );
						PIXEL33_80( pOut );
						break;
					}
				case 11:
				case 139:
					{
						if( Diff( cmp2, 4, 2 ) )
						{
							PIXEL00_0( pOut );
							PIXEL01_0( pOut );
							PIXEL10_0( pOut );
						}
						else
						{
							PIXEL00_50( pOut );
							PIXEL01_50( pOut );
							PIXEL10_50( pOut );
						}
						PIXEL02_10( pOut );
						PIXEL03_80( pOut );
						PIXEL11_0( pOut );
						PIXEL12_30( pOut );
						PIXEL13_61( pOut );
						PIXEL20_10( pOut );
						PIXEL21_30( pOut );
						PIXEL22_70( pOut );
						PIXEL23_60( pOut );
						PIXEL30_80( pOut );
						PIXEL31_61( pOut );
						PIXEL32_60( pOut );
						PIXEL33_20( pOut );
						break;
					}
				case 19:
				case 51:
					{
						if( Diff( cmp2, 2, 6 ) )
						{
							PIXEL00_81( pOut );
							PIXEL01_31( pOut );
							PIXEL02_10( pOut );
							PIXEL03_80( pOut );
							PIXEL12_30( pOut );
							PIXEL13_10( pOut );
						}
						else
						{
							PIXEL00_12( pOut );
							PIXEL01_14( pOut );
							PIXEL02_83( pOut );
							PIXEL03_50( pOut );
							PIXEL12_70( pOut );
							PIXEL13_21( pOut );
						}
						PIXEL10_81( pOut );
						PIXEL11_31( pOut );
						PIXEL20_60( pOut );
						PIXEL21_70( pOut );
						PIXEL22_30( pOut );
						PIXEL23_10( pOut );
						PIXEL30_20( pOut );
						PIXEL31_60( pOut );
						PIXEL32_61( pOut );
						PIXEL33_80( pOut );
						break;
					}
				case 146:
				case 178:
					{
						PIXEL00_80( pOut );
						PIXEL01_10( pOut );
						if( Diff( cmp2, 2, 6 ) )
						{
							PIXEL02_10( pOut );
							PIXEL03_80( pOut );
							PIXEL12_30( pOut );
							PIXEL13_10( pOut );
							PIXEL23_32( pOut );
							PIXEL33_82( pOut );
						}
						else
						{
							PIXEL02_21( pOut );
							PIXEL03_50( pOut );
							PIXEL12_70( pOut );
							PIXEL13_83( pOut );
							PIXEL23_13( pOut );
							PIXEL33_11( pOut );
						}
						PIXEL10_61( pOut );
						PIXEL11_30( pOut );
						PIXEL20_60( pOut );
						PIXEL21_70( pOut );
						PIXEL22_32( pOut );
						PIXEL30_20( pOut );
						PIXEL31_60( pOut );
						PIXEL32_82( pOut );
						break;
					}
				case 84:
				case 85:
					{
						PIXEL00_20( pOut );
						PIXEL01_60( pOut );
						PIXEL02_81( pOut );
						if( Diff( cmp2, 6, 8 ) )
						{
							PIXEL03_81( pOut );
							PIXEL13_31( pOut );
							PIXEL22_30( pOut );
							PIXEL23_10( pOut );
							PIXEL32_10( pOut );
							PIXEL33_80( pOut );
						}
						else
						{
							PIXEL03_12( pOut );
							PIXEL13_14( pOut );
							PIXEL22_70( pOut );
							PIXEL23_83( pOut );
							PIXEL32_21( pOut );
							PIXEL33_50( pOut );
						}
						PIXEL10_60( pOut );
						PIXEL11_70( pOut );
						PIXEL12_31( pOut );
						PIXEL20_61( pOut );
						PIXEL21_30( pOut );
						PIXEL30_80( pOut );
						PIXEL31_10( pOut );
						break;
					}
				case 112:
				case 113:
					{
						PIXEL00_20( pOut );
						PIXEL01_60( pOut );
						PIXEL02_61( pOut );
						PIXEL03_80( pOut );
						PIXEL10_60( pOut );
						PIXEL11_70( pOut );
						PIXEL12_30( pOut );
						PIXEL13_10( pOut );
						PIXEL20_82( pOut );
						PIXEL21_32( pOut );
						if( Diff( cmp2, 6, 8 ) )
						{
							PIXEL22_30( pOut );
							PIXEL23_10( pOut );
							PIXEL30_82( pOut );
							PIXEL31_32( pOut );
							PIXEL32_10( pOut );
							PIXEL33_80( pOut );
						}
						else
						{
							PIXEL22_70( pOut );
							PIXEL23_21( pOut );
							PIXEL30_11( pOut );
							PIXEL31_13( pOut );
							PIXEL32_83( pOut );
							PIXEL33_50( pOut );
						}
						break;
					}
				case 200:
				case 204:
					{
						PIXEL00_80( pOut );
						PIXEL01_61( pOut );
						PIXEL02_60( pOut );
						PIXEL03_20( pOut );
						PIXEL10_10( pOut );
						PIXEL11_30( pOut );
						PIXEL12_70( pOut );
						PIXEL13_60( pOut );
						if( Diff( cmp2, 8, 4 ) )
						{
							PIXEL20_10( pOut );
							PIXEL21_30( pOut );
							PIXEL30_80( pOut );
							PIXEL31_10( pOut );
							PIXEL32_31( pOut );
							PIXEL33_81( pOut );
						}
						else
						{
							PIXEL20_21( pOut );
							PIXEL21_70( pOut );
							PIXEL30_50( pOut );
							PIXEL31_83( pOut );
							PIXEL32_14( pOut );
							PIXEL33_12( pOut );
						}
						PIXEL22_31( pOut );
						PIXEL23_81( pOut );
						break;
					}
				case 73:
				case 77:
					{
						if( Diff( cmp2, 8, 4 ) )
						{
							PIXEL00_82( pOut );
							PIXEL10_32( pOut );
							PIXEL20_10( pOut );
							PIXEL21_30( pOut );
							PIXEL30_80( pOut );
							PIXEL31_10( pOut );
						}
						else
						{
							PIXEL00_11( pOut );
							PIXEL10_13( pOut );
							PIXEL20_83( pOut );
							PIXEL21_70( pOut );
							PIXEL30_50( pOut );
							PIXEL31_21( pOut );
						}
						PIXEL01_82( pOut );
						PIXEL02_60( pOut );
						PIXEL03_20( pOut );
						PIXEL11_32( pOut );
						PIXEL12_70( pOut );
						PIXEL13_60( pOut );
						PIXEL22_30( pOut );
						PIXEL23_61( pOut );
						PIXEL32_10( pOut );
						PIXEL33_80( pOut );
						break;
					}
				case 42:
				case 170:
					{
						if( Diff( cmp2, 4, 2 ) )
						{
							PIXEL00_80( pOut );
							PIXEL01_10( pOut );
							PIXEL10_10( pOut );
							PIXEL11_30( pOut );
							PIXEL20_31( pOut );
							PIXEL30_81( pOut );
						}
						else
						{
							PIXEL00_50( pOut );
							PIXEL01_21( pOut );
							PIXEL10_83( pOut );
							PIXEL11_70( pOut );
							PIXEL20_14( pOut );
							PIXEL30_12( pOut );
						}
						PIXEL02_10( pOut );
						PIXEL03_80( pOut );
						PIXEL12_30( pOut );
						PIXEL13_61( pOut );
						PIXEL21_31( pOut );
						PIXEL22_70( pOut );
						PIXEL23_60( pOut );
						PIXEL31_81( pOut );
						PIXEL32_60( pOut );
						PIXEL33_20( pOut );
						break;
					}
				case 14:
				case 142:
					{
						if( Diff( cmp2, 4, 2 ) )
						{
							PIXEL00_80( pOut );
							PIXEL01_10( pOut );
							PIXEL02_32( pOut );
							PIXEL03_82( pOut );
							PIXEL10_10( pOut );
							PIXEL11_30( pOut );
						}
						else
						{
							PIXEL00_50( pOut );
							PIXEL01_83( pOut );
							PIXEL02_13( pOut );
							PIXEL03_11( pOut );
							PIXEL10_21( pOut );
							PIXEL11_70( pOut );
						}
						PIXEL12_32( pOut );
						PIXEL13_82( pOut );
						PIXEL20_10( pOut );
						PIXEL21_30( pOut );
						PIXEL22_70( pOut );
						PIXEL23_60( pOut );
						PIXEL30_80( pOut );
						PIXEL31_61( pOut );
						PIXEL32_60( pOut );
						PIXEL33_20( pOut );
						break;
					}
				case 67:
					{
						PIXEL00_81( pOut );
						PIXEL01_31( pOut );
						PIXEL02_10( pOut );
						PIXEL03_80( pOut );
						PIXEL10_81( pOut );
						PIXEL11_31( pOut );
						PIXEL12_30( pOut );
						PIXEL13_61( pOut );
						PIXEL20_61( pOut );
						PIXEL21_30( pOut );
						PIXEL22_30( pOut );
						PIXEL23_61( pOut );
						PIXEL30_80( pOut );
						PIXEL31_10( pOut );
						PIXEL32_10( pOut );
						PIXEL33_80( pOut );
						break;
					}
				case 70:
					{
						PIXEL00_80( pOut );
						PIXEL01_10( pOut );
						PIXEL02_32( pOut );
						PIXEL03_82( pOut );
						PIXEL10_61( pOut );
						PIXEL11_30( pOut );
						PIXEL12_32( pOut );
						PIXEL13_82( pOut );
						PIXEL20_61( pOut );
						PIXEL21_30( pOut );
						PIXEL22_30( pOut );
						PIXEL23_61( pOut );
						PIXEL30_80( pOut );
						PIXEL31_10( pOut );
						PIXEL32_10( pOut );
						PIXEL33_80( pOut );
						break;
					}
				case 28:
					{
						PIXEL00_80( pOut );
						PIXEL01_61( pOut );
						PIXEL02_81( pOut );
						PIXEL03_81( pOut );
						PIXEL10_10( pOut );
						PIXEL11_30( pOut );
						PIXEL12_31( pOut );
						PIXEL13_31( pOut );
						PIXEL20_10( pOut );
						PIXEL21_30( pOut );
						PIXEL22_30( pOut );
						PIXEL23_10( pOut );
						PIXEL30_80( pOut );
						PIXEL31_61( pOut );
						PIXEL32_61( pOut );
						PIXEL33_80( pOut );
						break;
					}
				case 152:
					{
						PIXEL00_80( pOut );
						PIXEL01_61( pOut );
						PIXEL02_61( pOut );
						PIXEL03_80( pOut );
						PIXEL10_10( pOut );
						PIXEL11_30( pOut );
						PIXEL12_30( pOut );
						PIXEL13_10( pOut );
						PIXEL20_10( pOut );
						PIXEL21_30( pOut );
						PIXEL22_32( pOut );
						PIXEL23_32( pOut );
						PIXEL30_80( pOut );
						PIXEL31_61( pOut );
						PIXEL32_82( pOut );
						PIXEL33_82( pOut );
						break;
					}
				case 194:
					{
						PIXEL00_80( pOut );
						PIXEL01_10( pOut );
						PIXEL02_10( pOut );
						PIXEL03_80( pOut );
						PIXEL10_61( pOut );
						PIXEL11_30( pOut );
						PIXEL12_30( pOut );
						PIXEL13_61( pOut );
						PIXEL20_61( pOut );
						PIXEL21_30( pOut );
						PIXEL22_31( pOut );
						PIXEL23_81( pOut );
						PIXEL30_80( pOut );
						PIXEL31_10( pOut );
						PIXEL32_31( pOut );
						PIXEL33_81( pOut );
						break;
					}
				case 98:
					{
						PIXEL00_80( pOut );
						PIXEL01_10( pOut );
						PIXEL02_10( pOut );
						PIXEL03_80( pOut );
						PIXEL10_61( pOut );
						PIXEL11_30( pOut );
						PIXEL12_30( pOut );
						PIXEL13_61( pOut );
						PIXEL20_82( pOut );
						PIXEL21_32( pOut );
						PIXEL22_30( pOut );
						PIXEL23_61( pOut );
						PIXEL30_82( pOut );
						PIXEL31_32( pOut );
						PIXEL32_10( pOut );
						PIXEL33_80( pOut );
						break;
					}
				case 56:
					{
						PIXEL00_80( pOut );
						PIXEL01_61( pOut );
						PIXEL02_61( pOut );
						PIXEL03_80( pOut );
						PIXEL10_10( pOut );
						PIXEL11_30( pOut );
						PIXEL12_30( pOut );
						PIXEL13_10( pOut );
						PIXEL20_31( pOut );
						PIXEL21_31( pOut );
						PIXEL22_30( pOut );
						PIXEL23_10( pOut );
						PIXEL30_81( pOut );
						PIXEL31_81( pOut );
						PIXEL32_61( pOut );
						PIXEL33_80( pOut );
						break;
					}
				case 25:
					{
						PIXEL00_82( pOut );
						PIXEL01_82( pOut );
						PIXEL02_61( pOut );
						PIXEL03_80( pOut );
						PIXEL10_32( pOut );
						PIXEL11_32( pOut );
						PIXEL12_30( pOut );
						PIXEL13_10( pOut );
						PIXEL20_10( pOut );
						PIXEL21_30( pOut );
						PIXEL22_30( pOut );
						PIXEL23_10( pOut );
						PIXEL30_80( pOut );
						PIXEL31_61( pOut );
						PIXEL32_61( pOut );
						PIXEL33_80( pOut );
						break;
					}
				case 26:
				case 31:
					{
						if( Diff( cmp2, 4, 2 ) )
						{
							PIXEL00_0( pOut );
							PIXEL01_0( pOut );
							PIXEL10_0( pOut );
						}
						else
						{
							PIXEL00_50( pOut );
							PIXEL01_50( pOut );
							PIXEL10_50( pOut );
						}
						if( Diff( cmp2, 2, 6 ) )
						{
							PIXEL02_0( pOut );
							PIXEL03_0( pOut );
							PIXEL13_0( pOut );
						}
						else
						{
							PIXEL02_50( pOut );
							PIXEL03_50( pOut );
							PIXEL13_50( pOut );
						}
						PIXEL11_0( pOut );
						PIXEL12_0( pOut );
						PIXEL20_10( pOut );
						PIXEL21_30( pOut );
						PIXEL22_30( pOut );
						PIXEL23_10( pOut );
						PIXEL30_80( pOut );
						PIXEL31_61( pOut );
						PIXEL32_61( pOut );
						PIXEL33_80( pOut );
						break;
					}
				case 82:
				case 214:
					{
						PIXEL00_80( pOut );
						PIXEL01_10( pOut );
						if( Diff( cmp2, 2, 6 ) )
						{
							PIXEL02_0( pOut );
							PIXEL03_0( pOut );
							PIXEL13_0( pOut );
						}
						else
						{
							PIXEL02_50( pOut );
							PIXEL03_50( pOut );
							PIXEL13_50( pOut );
						}
						PIXEL10_61( pOut );
						PIXEL11_30( pOut );
						PIXEL12_0( pOut );
						PIXEL20_61( pOut );
						PIXEL21_30( pOut );
						PIXEL22_0( pOut );
						if( Diff( cmp2, 6, 8 ) )
						{
							PIXEL23_0( pOut );
							PIXEL32_0( pOut );
							PIXEL33_0( pOut );
						}
						else
						{
							PIXEL23_50( pOut );
							PIXEL32_50( pOut );
							PIXEL33_50( pOut );
						}
						PIXEL30_80( pOut );
						PIXEL31_10( pOut );
						break;
					}
				case 88:
				case 248:
					{
						PIXEL00_80( pOut );
						PIXEL01_61( pOut );
						PIXEL02_61( pOut );
						PIXEL03_80( pOut );
						PIXEL10_10( pOut );
						PIXEL11_30( pOut );
						PIXEL12_30( pOut );
						PIXEL13_10( pOut );
						if( Diff( cmp2, 8, 4 ) )
						{
							PIXEL20_0( pOut );
							PIXEL30_0( pOut );
							PIXEL31_0( pOut );
						}
						else
						{
							PIXEL20_50( pOut );
							PIXEL30_50( pOut );
							PIXEL31_50( pOut );
						}
						PIXEL21_0( pOut );
						PIXEL22_0( pOut );
						if( Diff( cmp2, 6, 8 ) )
						{
							PIXEL23_0( pOut );
							PIXEL32_0( pOut );
							PIXEL33_0( pOut );
						}
						else
						{
							PIXEL23_50( pOut );
							PIXEL32_50( pOut );
							PIXEL33_50( pOut );
						}
						break;
					}
				case 74:
				case 107:
					{
						if( Diff( cmp2, 4, 2 ) )
						{
							PIXEL00_0( pOut );
							PIXEL01_0( pOut );
							PIXEL10_0( pOut );
						}
						else
						{
							PIXEL00_50( pOut );
							PIXEL01_50( pOut );
							PIXEL10_50( pOut );
						}
						PIXEL02_10( pOut );
						PIXEL03_80( pOut );
						PIXEL11_0( pOut );
						PIXEL12_30( pOut );
						PIXEL13_61( pOut );
						if( Diff( cmp2, 8, 4 ) )
						{
							PIXEL20_0( pOut );
							PIXEL30_0( pOut );
							PIXEL31_0( pOut );
						}
						else
						{
							PIXEL20_50( pOut );
							PIXEL30_50( pOut );
							PIXEL31_50( pOut );
						}
						PIXEL21_0( pOut );
						PIXEL22_30( pOut );
						PIXEL23_61( pOut );
						PIXEL32_10( pOut );
						PIXEL33_80( pOut );
						break;
					}
				case 27:
					{
						if( Diff( cmp2, 4, 2 ) )
						{
							PIXEL00_0( pOut );
							PIXEL01_0( pOut );
							PIXEL10_0( pOut );
						}
						else
						{
							PIXEL00_50( pOut );
							PIXEL01_50( pOut );
							PIXEL10_50( pOut );
						}
						PIXEL02_10( pOut );
						PIXEL03_80( pOut );
						PIXEL11_0( pOut );
						PIXEL12_30( pOut );
						PIXEL13_10( pOut );
						PIXEL20_10( pOut );
						PIXEL21_30( pOut );
						PIXEL22_30( pOut );
						PIXEL23_10( pOut );
						PIXEL30_80( pOut );
						PIXEL31_61( pOut );
						PIXEL32_61( pOut );
						PIXEL33_80( pOut );
						break;
					}
				case 86:
					{
						PIXEL00_80( pOut );
						PIXEL01_10( pOut );
						if( Diff( cmp2, 2, 6 ) )
						{
							PIXEL02_0( pOut );
							PIXEL03_0( pOut );
							PIXEL13_0( pOut );
						}
						else
						{
							PIXEL02_50( pOut );
							PIXEL03_50( pOut );
							PIXEL13_50( pOut );
						}
						PIXEL10_61( pOut );
						PIXEL11_30( pOut );
						PIXEL12_0( pOut );
						PIXEL20_61( pOut );
						PIXEL21_30( pOut );
						PIXEL22_30( pOut );
						PIXEL23_10( pOut );
						PIXEL30_80( pOut );
						PIXEL31_10( pOut );
						PIXEL32_10( pOut );
						PIXEL33_80( pOut );
						break;
					}
				case 216:
					{
						PIXEL00_80( pOut );
						PIXEL01_61( pOut );
						PIXEL02_61( pOut );
						PIXEL03_80( pOut );
						PIXEL10_10( pOut );
						PIXEL11_30( pOut );
						PIXEL12_30( pOut );
						PIXEL13_10( pOut );
						PIXEL20_10( pOut );
						PIXEL21_30( pOut );
						PIXEL22_0( pOut );
						if( Diff( cmp2, 6, 8 ) )
						{
							PIXEL23_0( pOut );
							PIXEL32_0( pOut );
							PIXEL33_0( pOut );
						}
						else
						{
							PIXEL23_50( pOut );
							PIXEL32_50( pOut );
							PIXEL33_50( pOut );
						}
						PIXEL30_80( pOut );
						PIXEL31_10( pOut );
						break;
					}
				case 106:
					{
						PIXEL00_80( pOut );
						PIXEL01_10( pOut );
						PIXEL02_10( pOut );
						PIXEL03_80( pOut );
						PIXEL10_10( pOut );
						PIXEL11_30( pOut );
						PIXEL12_30( pOut );
						PIXEL13_61( pOut );
						if( Diff( cmp2, 8, 4 ) )
						{
							PIXEL20_0( pOut );
							PIXEL30_0( pOut );
							PIXEL31_0( pOut );
						}
						else
						{
							PIXEL20_50( pOut );
							PIXEL30_50( pOut );
							PIXEL31_50( pOut );
						}
						PIXEL21_0( pOut );
						PIXEL22_30( pOut );
						PIXEL23_61( pOut );
						PIXEL32_10( pOut );
						PIXEL33_80( pOut );
						break;
					}
				case 30:
					{
						PIXEL00_80( pOut );
						PIXEL01_10( pOut );
						if( Diff( cmp2, 2, 6 ) )
						{
							PIXEL02_0( pOut );
							PIXEL03_0( pOut );
							PIXEL13_0( pOut );
						}
						else
						{
							PIXEL02_50( pOut );
							PIXEL03_50( pOut );
							PIXEL13_50( pOut );
						}
						PIXEL10_10( pOut );
						PIXEL11_30( pOut );
						PIXEL12_0( pOut );
						PIXEL20_10( pOut );
						PIXEL21_30( pOut );
						PIXEL22_30( pOut );
						PIXEL23_10( pOut );
						PIXEL30_80( pOut );
						PIXEL31_61( pOut );
						PIXEL32_61( pOut );
						PIXEL33_80( pOut );
						break;
					}
				case 210:
					{
						PIXEL00_80( pOut );
						PIXEL01_10( pOut );
						PIXEL02_10( pOut );
						PIXEL03_80( pOut );
						PIXEL10_61( pOut );
						PIXEL11_30( pOut );
						PIXEL12_30( pOut );
						PIXEL13_10( pOut );
						PIXEL20_61( pOut );
						PIXEL21_30( pOut );
						PIXEL22_0( pOut );
						if( Diff( cmp2, 6, 8 ) )
						{
							PIXEL23_0( pOut );
							PIXEL32_0( pOut );
							PIXEL33_0( pOut );
						}
						else
						{
							PIXEL23_50( pOut );
							PIXEL32_50( pOut );
							PIXEL33_50( pOut );
						}
						PIXEL30_80( pOut );
						PIXEL31_10( pOut );
						break;
					}
				case 120:
					{
						PIXEL00_80( pOut );
						PIXEL01_61( pOut );
						PIXEL02_61( pOut );
						PIXEL03_80( pOut );
						PIXEL10_10( pOut );
						PIXEL11_30( pOut );
						PIXEL12_30( pOut );
						PIXEL13_10( pOut );
						if( Diff( cmp2, 8, 4 ) )
						{
							PIXEL20_0( pOut );
							PIXEL30_0( pOut );
							PIXEL31_0( pOut );
						}
						else
						{
							PIXEL20_50( pOut );
							PIXEL30_50( pOut );
							PIXEL31_50( pOut );
						}
						PIXEL21_0( pOut );
						PIXEL22_30( pOut );
						PIXEL23_10( pOut );
						PIXEL32_10( pOut );
						PIXEL33_80( pOut );
						break;
					}
				case 75:
					{
						if( Diff( cmp2, 4, 2 ) )
						{
							PIXEL00_0( pOut );
							PIXEL01_0( pOut );
							PIXEL10_0( pOut );
						}
						else
						{
							PIXEL00_50( pOut );
							PIXEL01_50( pOut );
							PIXEL10_50( pOut );
						}
						PIXEL02_10( pOut );
						PIXEL03_80( pOut );
						PIXEL11_0( pOut );
						PIXEL12_30( pOut );
						PIXEL13_61( pOut );
						PIXEL20_10( pOut );
						PIXEL21_30( pOut );
						PIXEL22_30( pOut );
						PIXEL23_61( pOut );
						PIXEL30_80( pOut );
						PIXEL31_10( pOut );
						PIXEL32_10( pOut );
						PIXEL33_80( pOut );
						break;
					}
				case 29:
					{
						PIXEL00_82( pOut );
						PIXEL01_82( pOut );
						PIXEL02_81( pOut );
						PIXEL03_81( pOut );
						PIXEL10_32( pOut );
						PIXEL11_32( pOut );
						PIXEL12_31( pOut );
						PIXEL13_31( pOut );
						PIXEL20_10( pOut );
						PIXEL21_30( pOut );
						PIXEL22_30( pOut );
						PIXEL23_10( pOut );
						PIXEL30_80( pOut );
						PIXEL31_61( pOut );
						PIXEL32_61( pOut );
						PIXEL33_80( pOut );
						break;
					}
				case 198:
					{
						PIXEL00_80( pOut );
						PIXEL01_10( pOut );
						PIXEL02_32( pOut );
						PIXEL03_82( pOut );
						PIXEL10_61( pOut );
						PIXEL11_30( pOut );
						PIXEL12_32( pOut );
						PIXEL13_82( pOut );
						PIXEL20_61( pOut );
						PIXEL21_30( pOut );
						PIXEL22_31( pOut );
						PIXEL23_81( pOut );
						PIXEL30_80( pOut );
						PIXEL31_10( pOut );
						PIXEL32_31( pOut );
						PIXEL33_81( pOut );
						break;
					}
				case 184:
					{
						PIXEL00_80( pOut );
						PIXEL01_61( pOut );
						PIXEL02_61( pOut );
						PIXEL03_80( pOut );
						PIXEL10_10( pOut );
						PIXEL11_30( pOut );
						PIXEL12_30( pOut );
						PIXEL13_10( pOut );
						PIXEL20_31( pOut );
						PIXEL21_31( pOut );
						PIXEL22_32( pOut );
						PIXEL23_32( pOut );
						PIXEL30_81( pOut );
						PIXEL31_81( pOut );
						PIXEL32_82( pOut );
						PIXEL33_82( pOut );
						break;
					}
				case 99:
					{
						PIXEL00_81( pOut );
						PIXEL01_31( pOut );
						PIXEL02_10( pOut );
						PIXEL03_80( pOut );
						PIXEL10_81( pOut );
						PIXEL11_31( pOut );
						PIXEL12_30( pOut );
						PIXEL13_61( pOut );
						PIXEL20_82( pOut );
						PIXEL21_32( pOut );
						PIXEL22_30( pOut );
						PIXEL23_61( pOut );
						PIXEL30_82( pOut );
						PIXEL31_32( pOut );
						PIXEL32_10( pOut );
						PIXEL33_80( pOut );
						break;
					}
				case 57:
					{
						PIXEL00_82( pOut );
						PIXEL01_82( pOut );
						PIXEL02_61( pOut );
						PIXEL03_80( pOut );
						PIXEL10_32( pOut );
						PIXEL11_32( pOut );
						PIXEL12_30( pOut );
						PIXEL13_10( pOut );
						PIXEL20_31( pOut );
						PIXEL21_31( pOut );
						PIXEL22_30( pOut );
						PIXEL23_10( pOut );
						PIXEL30_81( pOut );
						PIXEL31_81( pOut );
						PIXEL32_61( pOut );
						PIXEL33_80( pOut );
						break;
					}
				case 71:
					{
						PIXEL00_81( pOut );
						PIXEL01_31( pOut );
						PIXEL02_32( pOut );
						PIXEL03_82( pOut );
						PIXEL10_81( pOut );
						PIXEL11_31( pOut );
						PIXEL12_32( pOut );
						PIXEL13_82( pOut );
						PIXEL20_61( pOut );
						PIXEL21_30( pOut );
						PIXEL22_30( pOut );
						PIXEL23_61( pOut );
						PIXEL30_80( pOut );
						PIXEL31_10( pOut );
						PIXEL32_10( pOut );
						PIXEL33_80( pOut );
						break;
					}
				case 156:
					{
						PIXEL00_80( pOut );
						PIXEL01_61( pOut );
						PIXEL02_81( pOut );
						PIXEL03_81( pOut );
						PIXEL10_10( pOut );
						PIXEL11_30( pOut );
						PIXEL12_31( pOut );
						PIXEL13_31( pOut );
						PIXEL20_10( pOut );
						PIXEL21_30( pOut );
						PIXEL22_32( pOut );
						PIXEL23_32( pOut );
						PIXEL30_80( pOut );
						PIXEL31_61( pOut );
						PIXEL32_82( pOut );
						PIXEL33_82( pOut );
						break;
					}
				case 226:
					{
						PIXEL00_80( pOut );
						PIXEL01_10( pOut );
						PIXEL02_10( pOut );
						PIXEL03_80( pOut );
						PIXEL10_61( pOut );
						PIXEL11_30( pOut );
						PIXEL12_30( pOut );
						PIXEL13_61( pOut );
						PIXEL20_82( pOut );
						PIXEL21_32( pOut );
						PIXEL22_31( pOut );
						PIXEL23_81( pOut );
						PIXEL30_82( pOut );
						PIXEL31_32( pOut );
						PIXEL32_31( pOut );
						PIXEL33_81( pOut );
						break;
					}
				case 60:
					{
						PIXEL00_80( pOut );
						PIXEL01_61( pOut );
						PIXEL02_81( pOut );
						PIXEL03_81( pOut );
						PIXEL10_10( pOut );
						PIXEL11_30( pOut );
						PIXEL12_31( pOut );
						PIXEL13_31( pOut );
						PIXEL20_31( pOut );
						PIXEL21_31( pOut );
						PIXEL22_30( pOut );
						PIXEL23_10( pOut );
						PIXEL30_81( pOut );
						PIXEL31_81( pOut );
						PIXEL32_61( pOut );
						PIXEL33_80( pOut );
						break;
					}
				case 195:
					{
						PIXEL00_81( pOut );
						PIXEL01_31( pOut );
						PIXEL02_10( pOut );
						PIXEL03_80( pOut );
						PIXEL10_81( pOut );
						PIXEL11_31( pOut );
						PIXEL12_30( pOut );
						PIXEL13_61( pOut );
						PIXEL20_61( pOut );
						PIXEL21_30( pOut );
						PIXEL22_31( pOut );
						PIXEL23_81( pOut );
						PIXEL30_80( pOut );
						PIXEL31_10( pOut );
						PIXEL32_31( pOut );
						PIXEL33_81( pOut );
						break;
					}
				case 102:
					{
						PIXEL00_80( pOut );
						PIXEL01_10( pOut );
						PIXEL02_32( pOut );
						PIXEL03_82( pOut );
						PIXEL10_61( pOut );
						PIXEL11_30( pOut );
						PIXEL12_32( pOut );
						PIXEL13_82( pOut );
						PIXEL20_82( pOut );
						PIXEL21_32( pOut );
						PIXEL22_30( pOut );
						PIXEL23_61( pOut );
						PIXEL30_82( pOut );
						PIXEL31_32( pOut );
						PIXEL32_10( pOut );
						PIXEL33_80( pOut );
						break;
					}
				case 153:
					{
						PIXEL00_82( pOut );
						PIXEL01_82( pOut );
						PIXEL02_61( pOut );
						PIXEL03_80( pOut );
						PIXEL10_32( pOut );
						PIXEL11_32( pOut );
						PIXEL12_30( pOut );
						PIXEL13_10( pOut );
						PIXEL20_10( pOut );
						PIXEL21_30( pOut );
						PIXEL22_32( pOut );
						PIXEL23_32( pOut );
						PIXEL30_80( pOut );
						PIXEL31_61( pOut );
						PIXEL32_82( pOut );
						PIXEL33_82( pOut );
						break;
					}
				case 58:
					{
						if( Diff( cmp2, 4, 2 ) )
						{
							PIXEL00_80( pOut );
							PIXEL01_10( pOut );
							PIXEL10_10( pOut );
							PIXEL11_30( pOut );
						}
						else
						{
							PIXEL00_20( pOut );
							PIXEL01_12( pOut );
							PIXEL10_11( pOut );
							PIXEL11_0( pOut );
						}
						if( Diff( cmp2, 2, 6 ) )
						{
							PIXEL02_10( pOut );
							PIXEL03_80( pOut );
							PIXEL12_30( pOut );
							PIXEL13_10( pOut );
						}
						else
						{
							PIXEL02_11( pOut );
							PIXEL03_20( pOut );
							PIXEL12_0( pOut );
							PIXEL13_12( pOut );
						}
						PIXEL20_31( pOut );
						PIXEL21_31( pOut );
						PIXEL22_30( pOut );
						PIXEL23_10( pOut );
						PIXEL30_81( pOut );
						PIXEL31_81( pOut );
						PIXEL32_61( pOut );
						PIXEL33_80( pOut );
						break;
					}
				case 83:
					{
						PIXEL00_81( pOut );
						PIXEL01_31( pOut );
						if( Diff( cmp2, 2, 6 ) )
						{
							PIXEL02_10( pOut );
							PIXEL03_80( pOut );
							PIXEL12_30( pOut );
							PIXEL13_10( pOut );
						}
						else
						{
							PIXEL02_11( pOut );
							PIXEL03_20( pOut );
							PIXEL12_0( pOut );
							PIXEL13_12( pOut );
						}
						PIXEL10_81( pOut );
						PIXEL11_31( pOut );
						PIXEL20_61( pOut );
						PIXEL21_30( pOut );
						if( Diff( cmp2, 6, 8 ) )
						{
							PIXEL22_30( pOut );
							PIXEL23_10( pOut );
							PIXEL32_10( pOut );
							PIXEL33_80( pOut );
						}
						else
						{
							PIXEL22_0( pOut );
							PIXEL23_11( pOut );
							PIXEL32_12( pOut );
							PIXEL33_20( pOut );
						}
						PIXEL30_80( pOut );
						PIXEL31_10( pOut );
						break;
					}
				case 92:
					{
						PIXEL00_80( pOut );
						PIXEL01_61( pOut );
						PIXEL02_81( pOut );
						PIXEL03_81( pOut );
						PIXEL10_10( pOut );
						PIXEL11_30( pOut );
						PIXEL12_31( pOut );
						PIXEL13_31( pOut );
						if( Diff( cmp2, 8, 4 ) )
						{
							PIXEL20_10( pOut );
							PIXEL21_30( pOut );
							PIXEL30_80( pOut );
							PIXEL31_10( pOut );
						}
						else
						{
							PIXEL20_12( pOut );
							PIXEL21_0( pOut );
							PIXEL30_20( pOut );
							PIXEL31_11( pOut );
						}
						if( Diff( cmp2, 6, 8 ) )
						{
							PIXEL22_30( pOut );
							PIXEL23_10( pOut );
							PIXEL32_10( pOut );
							PIXEL33_80( pOut );
						}
						else
						{
							PIXEL22_0( pOut );
							PIXEL23_11( pOut );
							PIXEL32_12( pOut );
							PIXEL33_20( pOut );
						}
						break;
					}
				case 202:
					{
						if( Diff( cmp2, 4, 2 ) )
						{
							PIXEL00_80( pOut );
							PIXEL01_10( pOut );
							PIXEL10_10( pOut );
							PIXEL11_30( pOut );
						}
						else
						{
							PIXEL00_20( pOut );
							PIXEL01_12( pOut );
							PIXEL10_11( pOut );
							PIXEL11_0( pOut );
						}
						PIXEL02_10( pOut );
						PIXEL03_80( pOut );
						PIXEL12_30( pOut );
						PIXEL13_61( pOut );
						if( Diff( cmp2, 8, 4 ) )
						{
							PIXEL20_10( pOut );
							PIXEL21_30( pOut );
							PIXEL30_80( pOut );
							PIXEL31_10( pOut );
						}
						else
						{
							PIXEL20_12( pOut );
							PIXEL21_0( pOut );
							PIXEL30_20( pOut );
							PIXEL31_11( pOut );
						}
						PIXEL22_31( pOut );
						PIXEL23_81( pOut );
						PIXEL32_31( pOut );
						PIXEL33_81( pOut );
						break;
					}
				case 78:
					{
						if( Diff( cmp2, 4, 2 ) )
						{
							PIXEL00_80( pOut );
							PIXEL01_10( pOut );
							PIXEL10_10( pOut );
							PIXEL11_30( pOut );
						}
						else
						{
							PIXEL00_20( pOut );
							PIXEL01_12( pOut );
							PIXEL10_11( pOut );
							PIXEL11_0( pOut );
						}
						PIXEL02_32( pOut );
						PIXEL03_82( pOut );
						PIXEL12_32( pOut );
						PIXEL13_82( pOut );
						if( Diff( cmp2, 8, 4 ) )
						{
							PIXEL20_10( pOut );
							PIXEL21_30( pOut );
							PIXEL30_80( pOut );
							PIXEL31_10( pOut );
						}
						else
						{
							PIXEL20_12( pOut );
							PIXEL21_0( pOut );
							PIXEL30_20( pOut );
							PIXEL31_11( pOut );
						}
						PIXEL22_30( pOut );
						PIXEL23_61( pOut );
						PIXEL32_10( pOut );
						PIXEL33_80( pOut );
						break;
					}
				case 154:
					{
						if( Diff( cmp2, 4, 2 ) )
						{
							PIXEL00_80( pOut );
							PIXEL01_10( pOut );
							PIXEL10_10( pOut );
							PIXEL11_30( pOut );
						}
						else
						{
							PIXEL00_20( pOut );
							PIXEL01_12( pOut );
							PIXEL10_11( pOut );
							PIXEL11_0( pOut );
						}
						if( Diff( cmp2, 2, 6 ) )
						{
							PIXEL02_10( pOut );
							PIXEL03_80( pOut );
							PIXEL12_30( pOut );
							PIXEL13_10( pOut );
						}
						else
						{
							PIXEL02_11( pOut );
							PIXEL03_20( pOut );
							PIXEL12_0( pOut );
							PIXEL13_12( pOut );
						}
						PIXEL20_10( pOut );
						PIXEL21_30( pOut );
						PIXEL22_32( pOut );
						PIXEL23_32( pOut );
						PIXEL30_80( pOut );
						PIXEL31_61( pOut );
						PIXEL32_82( pOut );
						PIXEL33_82( pOut );
						break;
					}
				case 114:
					{
						PIXEL00_80( pOut );
						PIXEL01_10( pOut );
						if( Diff( cmp2, 2, 6 ) )
						{
							PIXEL02_10( pOut );
							PIXEL03_80( pOut );
							PIXEL12_30( pOut );
							PIXEL13_10( pOut );
						}
						else
						{
							PIXEL02_11( pOut );
							PIXEL03_20( pOut );
							PIXEL12_0( pOut );
							PIXEL13_12( pOut );
						}
						PIXEL10_61( pOut );
						PIXEL11_30( pOut );
						PIXEL20_82( pOut );
						PIXEL21_32( pOut );
						if( Diff( cmp2, 6, 8 ) )
						{
							PIXEL22_30( pOut );
							PIXEL23_10( pOut );
							PIXEL32_10( pOut );
							PIXEL33_80( pOut );
						}
						else
						{
							PIXEL22_0( pOut );
							PIXEL23_11( pOut );
							PIXEL32_12( pOut );
							PIXEL33_20( pOut );
						}
						PIXEL30_82( pOut );
						PIXEL31_32( pOut );
						break;
					}
				case 89:
					{
						PIXEL00_82( pOut );
						PIXEL01_82( pOut );
						PIXEL02_61( pOut );
						PIXEL03_80( pOut );
						PIXEL10_32( pOut );
						PIXEL11_32( pOut );
						PIXEL12_30( pOut );
						PIXEL13_10( pOut );
						if( Diff( cmp2, 8, 4 ) )
						{
							PIXEL20_10( pOut );
							PIXEL21_30( pOut );
							PIXEL30_80( pOut );
							PIXEL31_10( pOut );
						}
						else
						{
							PIXEL20_12( pOut );
							PIXEL21_0( pOut );
							PIXEL30_20( pOut );
							PIXEL31_11( pOut );
						}
						if( Diff( cmp2, 6, 8 ) )
						{
							PIXEL22_30( pOut );
							PIXEL23_10( pOut );
							PIXEL32_10( pOut );
							PIXEL33_80( pOut );
						}
						else
						{
							PIXEL22_0( pOut );
							PIXEL23_11( pOut );
							PIXEL32_12( pOut );
							PIXEL33_20( pOut );
						}
						break;
					}
				case 90:
					{
						if( Diff( cmp2, 4, 2 ) )
						{
							PIXEL00_80( pOut );
							PIXEL01_10( pOut );
							PIXEL10_10( pOut );
							PIXEL11_30( pOut );
						}
						else
						{
							PIXEL00_20( pOut );
							PIXEL01_12( pOut );
							PIXEL10_11( pOut );
							PIXEL11_0( pOut );
						}
						if( Diff( cmp2, 2, 6 ) )
						{
							PIXEL02_10( pOut );
							PIXEL03_80( pOut );
							PIXEL12_30( pOut );
							PIXEL13_10( pOut );
						}
						else
						{
							PIXEL02_11( pOut );
							PIXEL03_20( pOut );
							PIXEL12_0( pOut );
							PIXEL13_12( pOut );
						}
						if( Diff( cmp2, 8, 4 ) )
						{
							PIXEL20_10( pOut );
							PIXEL21_30( pOut );
							PIXEL30_80( pOut );
							PIXEL31_10( pOut );
						}
						else
						{
							PIXEL20_12( pOut );
							PIXEL21_0( pOut );
							PIXEL30_20( pOut );
							PIXEL31_11( pOut );
						}
						if( Diff( cmp2, 6, 8 ) )
						{
							PIXEL22_30( pOut );
							PIXEL23_10( pOut );
							PIXEL32_10( pOut );
							PIXEL33_80( pOut );
						}
						else
						{
							PIXEL22_0( pOut );
							PIXEL23_11( pOut );
							PIXEL32_12( pOut );
							PIXEL33_20( pOut );
						}
						break;
					}
				case 55:
				case 23:
					{
						if( Diff( cmp2, 2, 6 ) )
						{
							PIXEL00_81( pOut );
							PIXEL01_31( pOut );
							PIXEL02_0( pOut );
							PIXEL03_0( pOut );
							PIXEL12_0( pOut );
							PIXEL13_0( pOut );
						}
						else
						{
							PIXEL00_12( pOut );
							PIXEL01_14( pOut );
							PIXEL02_83( pOut );
							PIXEL03_50( pOut );
							PIXEL12_70( pOut );
							PIXEL13_21( pOut );
						}
						PIXEL10_81( pOut );
						PIXEL11_31( pOut );
						PIXEL20_60( pOut );
						PIXEL21_70( pOut );
						PIXEL22_30( pOut );
						PIXEL23_10( pOut );
						PIXEL30_20( pOut );
						PIXEL31_60( pOut );
						PIXEL32_61( pOut );
						PIXEL33_80( pOut );
						break;
					}
				case 182:
				case 150:
					{
						PIXEL00_80( pOut );
						PIXEL01_10( pOut );
						if( Diff( cmp2, 2, 6 ) )
						{
							PIXEL02_0( pOut );
							PIXEL03_0( pOut );
							PIXEL12_0( pOut );
							PIXEL13_0( pOut );
							PIXEL23_32( pOut );
							PIXEL33_82( pOut );
						}
						else
						{
							PIXEL02_21( pOut );
							PIXEL03_50( pOut );
							PIXEL12_70( pOut );
							PIXEL13_83( pOut );
							PIXEL23_13( pOut );
							PIXEL33_11( pOut );
						}
						PIXEL10_61( pOut );
						PIXEL11_30( pOut );
						PIXEL20_60( pOut );
						PIXEL21_70( pOut );
						PIXEL22_32( pOut );
						PIXEL30_20( pOut );
						PIXEL31_60( pOut );
						PIXEL32_82( pOut );
						break;
					}
				case 213:
				case 212:
					{
						PIXEL00_20( pOut );
						PIXEL01_60( pOut );
						PIXEL02_81( pOut );
						if( Diff( cmp2, 6, 8 ) )
						{
							PIXEL03_81( pOut );
							PIXEL13_31( pOut );
							PIXEL22_0( pOut );
							PIXEL23_0( pOut );
							PIXEL32_0( pOut );
							PIXEL33_0( pOut );
						}
						else
						{
							PIXEL03_12( pOut );
							PIXEL13_14( pOut );
							PIXEL22_70( pOut );
							PIXEL23_83( pOut );
							PIXEL32_21( pOut );
							PIXEL33_50( pOut );
						}
						PIXEL10_60( pOut );
						PIXEL11_70( pOut );
						PIXEL12_31( pOut );
						PIXEL20_61( pOut );
						PIXEL21_30( pOut );
						PIXEL30_80( pOut );
						PIXEL31_10( pOut );
						break;
					}
				case 241:
				case 240:
					{
						PIXEL00_20( pOut );
						PIXEL01_60( pOut );
						PIXEL02_61( pOut );
						PIXEL03_80( pOut );
						PIXEL10_60( pOut );
						PIXEL11_70( pOut );
						PIXEL12_30( pOut );
						PIXEL13_10( pOut );
						PIXEL20_82( pOut );
						PIXEL21_32( pOut );
						if( Diff( cmp2, 6, 8 ) )
						{
							PIXEL22_0( pOut );
							PIXEL23_0( pOut );
							PIXEL30_82( pOut );
							PIXEL31_32( pOut );
							PIXEL32_0( pOut );
							PIXEL33_0( pOut );
						}
						else
						{
							PIXEL22_70( pOut );
							PIXEL23_21( pOut );
							PIXEL30_11( pOut );
							PIXEL31_13( pOut );
							PIXEL32_83( pOut );
							PIXEL33_50( pOut );
						}
						break;
					}
				case 236:
				case 232:
					{
						PIXEL00_80( pOut );
						PIXEL01_61( pOut );
						PIXEL02_60( pOut );
						PIXEL03_20( pOut );
						PIXEL10_10( pOut );
						PIXEL11_30( pOut );
						PIXEL12_70( pOut );
						PIXEL13_60( pOut );
						if( Diff( cmp2, 8, 4 ) )
						{
							PIXEL20_0( pOut );
							PIXEL21_0( pOut );
							PIXEL30_0( pOut );
							PIXEL31_0( pOut );
							PIXEL32_31( pOut );
							PIXEL33_81( pOut );
						}
						else
						{
							PIXEL20_21( pOut );
							PIXEL21_70( pOut );
							PIXEL30_50( pOut );
							PIXEL31_83( pOut );
							PIXEL32_14( pOut );
							PIXEL33_12( pOut );
						}
						PIXEL22_31( pOut );
						PIXEL23_81( pOut );
						break;
					}
				case 109:
				case 105:
					{
						if( Diff( cmp2, 8, 4 ) )
						{
							PIXEL00_82( pOut );
							PIXEL10_32( pOut );
							PIXEL20_0( pOut );
							PIXEL21_0( pOut );
							PIXEL30_0( pOut );
							PIXEL31_0( pOut );
						}
						else
						{
							PIXEL00_11( pOut );
							PIXEL10_13( pOut );
							PIXEL20_83( pOut );
							PIXEL21_70( pOut );
							PIXEL30_50( pOut );
							PIXEL31_21( pOut );
						}
						PIXEL01_82( pOut );
						PIXEL02_60( pOut );
						PIXEL03_20( pOut );
						PIXEL11_32( pOut );
						PIXEL12_70( pOut );
						PIXEL13_60( pOut );
						PIXEL22_30( pOut );
						PIXEL23_61( pOut );
						PIXEL32_10( pOut );
						PIXEL33_80( pOut );
						break;
					}
				case 171:
				case 43:
					{
						if( Diff( cmp2, 4, 2 ) )
						{
							PIXEL00_0( pOut );
							PIXEL01_0( pOut );
							PIXEL10_0( pOut );
							PIXEL11_0( pOut );
							PIXEL20_31( pOut );
							PIXEL30_81( pOut );
						}
						else
						{
							PIXEL00_50( pOut );
							PIXEL01_21( pOut );
							PIXEL10_83( pOut );
							PIXEL11_70( pOut );
							PIXEL20_14( pOut );
							PIXEL30_12( pOut );
						}
						PIXEL02_10( pOut );
						PIXEL03_80( pOut );
						PIXEL12_30( pOut );
						PIXEL13_61( pOut );
						PIXEL21_31( pOut );
						PIXEL22_70( pOut );
						PIXEL23_60( pOut );
						PIXEL31_81( pOut );
						PIXEL32_60( pOut );
						PIXEL33_20( pOut );
						break;
					}
				case 143:
				case 15:
					{
						if( Diff( cmp2, 4, 2 ) )
						{
							PIXEL00_0( pOut );
							PIXEL01_0( pOut );
							PIXEL02_32( pOut );
							PIXEL03_82( pOut );
							PIXEL10_0( pOut );
							PIXEL11_0( pOut );
						}
						else
						{
							PIXEL00_50( pOut );
							PIXEL01_83( pOut );
							PIXEL02_13( pOut );
							PIXEL03_11( pOut );
							PIXEL10_21( pOut );
							PIXEL11_70( pOut );
						}
						PIXEL12_32( pOut );
						PIXEL13_82( pOut );
						PIXEL20_10( pOut );
						PIXEL21_30( pOut );
						PIXEL22_70( pOut );
						PIXEL23_60( pOut );
						PIXEL30_80( pOut );
						PIXEL31_61( pOut );
						PIXEL32_60( pOut );
						PIXEL33_20( pOut );
						break;
					}
				case 124:
					{
						PIXEL00_80( pOut );
						PIXEL01_61( pOut );
						PIXEL02_81( pOut );
						PIXEL03_81( pOut );
						PIXEL10_10( pOut );
						PIXEL11_30( pOut );
						PIXEL12_31( pOut );
						PIXEL13_31( pOut );
						if( Diff( cmp2, 8, 4 ) )
						{
							PIXEL20_0( pOut );
							PIXEL30_0( pOut );
							PIXEL31_0( pOut );
						}
						else
						{
							PIXEL20_50( pOut );
							PIXEL30_50( pOut );
							PIXEL31_50( pOut );
						}
						PIXEL21_0( pOut );
						PIXEL22_30( pOut );
						PIXEL23_10( pOut );
						PIXEL32_10( pOut );
						PIXEL33_80( pOut );
						break;
					}
				case 203:
					{
						if( Diff( cmp2, 4, 2 ) )
						{
							PIXEL00_0( pOut );
							PIXEL01_0( pOut );
							PIXEL10_0( pOut );
						}
						else
						{
							PIXEL00_50( pOut );
							PIXEL01_50( pOut );
							PIXEL10_50( pOut );
						}
						PIXEL02_10( pOut );
						PIXEL03_80( pOut );
						PIXEL11_0( pOut );
						PIXEL12_30( pOut );
						PIXEL13_61( pOut );
						PIXEL20_10( pOut );
						PIXEL21_30( pOut );
						PIXEL22_31( pOut );
						PIXEL23_81( pOut );
						PIXEL30_80( pOut );
						PIXEL31_10( pOut );
						PIXEL32_31( pOut );
						PIXEL33_81( pOut );
						break;
					}
				case 62:
					{
						PIXEL00_80( pOut );
						PIXEL01_10( pOut );
						if( Diff( cmp2, 2, 6 ) )
						{
							PIXEL02_0( pOut );
							PIXEL03_0( pOut );
							PIXEL13_0( pOut );
						}
						else
						{
							PIXEL02_50( pOut );
							PIXEL03_50( pOut );
							PIXEL13_50( pOut );
						}
						PIXEL10_10( pOut );
						PIXEL11_30( pOut );
						PIXEL12_0( pOut );
						PIXEL20_31( pOut );
						PIXEL21_31( pOut );
						PIXEL22_30( pOut );
						PIXEL23_10( pOut );
						PIXEL30_81( pOut );
						PIXEL31_81( pOut );
						PIXEL32_61( pOut );
						PIXEL33_80( pOut );
						break;
					}
				case 211:
					{
						PIXEL00_81( pOut );
						PIXEL01_31( pOut );
						PIXEL02_10( pOut );
						PIXEL03_80( pOut );
						PIXEL10_81( pOut );
						PIXEL11_31( pOut );
						PIXEL12_30( pOut );
						PIXEL13_10( pOut );
						PIXEL20_61( pOut );
						PIXEL21_30( pOut );
						PIXEL22_0( pOut );
						if( Diff( cmp2, 6, 8 ) )
						{
							PIXEL23_0( pOut );
							PIXEL32_0( pOut );
							PIXEL33_0( pOut );
						}
						else
						{
							PIXEL23_50( pOut );
							PIXEL32_50( pOut );
							PIXEL33_50( pOut );
						}
						PIXEL30_80( pOut );
						PIXEL31_10( pOut );
						break;
					}
				case 118:
					{
						PIXEL00_80( pOut );
						PIXEL01_10( pOut );
						if( Diff( cmp2, 2, 6 ) )
						{
							PIXEL02_0( pOut );
							PIXEL03_0( pOut );
							PIXEL13_0( pOut );
						}
						else
						{
							PIXEL02_50( pOut );
							PIXEL03_50( pOut );
							PIXEL13_50( pOut );
						}
						PIXEL10_61( pOut );
						PIXEL11_30( pOut );
						PIXEL12_0( pOut );
						PIXEL20_82( pOut );
						PIXEL21_32( pOut );
						PIXEL22_30( pOut );
						PIXEL23_10( pOut );
						PIXEL30_82( pOut );
						PIXEL31_32( pOut );
						PIXEL32_10( pOut );
						PIXEL33_80( pOut );
						break;
					}
				case 217:
					{
						PIXEL00_82( pOut );
						PIXEL01_82( pOut );
						PIXEL02_61( pOut );
						PIXEL03_80( pOut );
						PIXEL10_32( pOut );
						PIXEL11_32( pOut );
						PIXEL12_30( pOut );
						PIXEL13_10( pOut );
						PIXEL20_10( pOut );
						PIXEL21_30( pOut );
						PIXEL22_0( pOut );
						if( Diff( cmp2, 6, 8 ) )
						{
							PIXEL23_0( pOut );
							PIXEL32_0( pOut );
							PIXEL33_0( pOut );
						}
						else
						{
							PIXEL23_50( pOut );
							PIXEL32_50( pOut );
							PIXEL33_50( pOut );
						}
						PIXEL30_80( pOut );
						PIXEL31_10( pOut );
						break;
					}
				case 110:
					{
						PIXEL00_80( pOut );
						PIXEL01_10( pOut );
						PIXEL02_32( pOut );
						PIXEL03_82( pOut );
						PIXEL10_10( pOut );
						PIXEL11_30( pOut );
						PIXEL12_32( pOut );
						PIXEL13_82( pOut );
						if( Diff( cmp2, 8, 4 ) )
						{
							PIXEL20_0( pOut );
							PIXEL30_0( pOut );
							PIXEL31_0( pOut );
						}
						else
						{
							PIXEL20_50( pOut );
							PIXEL30_50( pOut );
							PIXEL31_50( pOut );
						}
						PIXEL21_0( pOut );
						PIXEL22_30( pOut );
						PIXEL23_61( pOut );
						PIXEL32_10( pOut );
						PIXEL33_80( pOut );
						break;
					}
				case 155:
					{
						if( Diff( cmp2, 4, 2 ) )
						{
							PIXEL00_0( pOut );
							PIXEL01_0( pOut );
							PIXEL10_0( pOut );
						}
						else
						{
							PIXEL00_50( pOut );
							PIXEL01_50( pOut );
							PIXEL10_50( pOut );
						}
						PIXEL02_10( pOut );
						PIXEL03_80( pOut );
						PIXEL11_0( pOut );
						PIXEL12_30( pOut );
						PIXEL13_10( pOut );
						PIXEL20_10( pOut );
						PIXEL21_30( pOut );
						PIXEL22_32( pOut );
						PIXEL23_32( pOut );
						PIXEL30_80( pOut );
						PIXEL31_61( pOut );
						PIXEL32_82( pOut );
						PIXEL33_82( pOut );
						break;
					}
				case 188:
					{
						PIXEL00_80( pOut );
						PIXEL01_61( pOut );
						PIXEL02_81( pOut );
						PIXEL03_81( pOut );
						PIXEL10_10( pOut );
						PIXEL11_30( pOut );
						PIXEL12_31( pOut );
						PIXEL13_31( pOut );
						PIXEL20_31( pOut );
						PIXEL21_31( pOut );
						PIXEL22_32( pOut );
						PIXEL23_32( pOut );
						PIXEL30_81( pOut );
						PIXEL31_81( pOut );
						PIXEL32_82( pOut );
						PIXEL33_82( pOut );
						break;
					}
				case 185:
					{
						PIXEL00_82( pOut );
						PIXEL01_82( pOut );
						PIXEL02_61( pOut );
						PIXEL03_80( pOut );
						PIXEL10_32( pOut );
						PIXEL11_32( pOut );
						PIXEL12_30( pOut );
						PIXEL13_10( pOut );
						PIXEL20_31( pOut );
						PIXEL21_31( pOut );
						PIXEL22_32( pOut );
						PIXEL23_32( pOut );
						PIXEL30_81( pOut );
						PIXEL31_81( pOut );
						PIXEL32_82( pOut );
						PIXEL33_82( pOut );
						break;
					}
				case 61:
					{
						PIXEL00_82( pOut );
						PIXEL01_82( pOut );
						PIXEL02_81( pOut );
						PIXEL03_81( pOut );
						PIXEL10_32( pOut );
						PIXEL11_32( pOut );
						PIXEL12_31( pOut );
						PIXEL13_31( pOut );
						PIXEL20_31( pOut );
						PIXEL21_31( pOut );
						PIXEL22_30( pOut );
						PIXEL23_10( pOut );
						PIXEL30_81( pOut );
						PIXEL31_81( pOut );
						PIXEL32_61( pOut );
						PIXEL33_80( pOut );
						break;
					}
				case 157:
					{
						PIXEL00_82( pOut );
						PIXEL01_82( pOut );
						PIXEL02_81( pOut );
						PIXEL03_81( pOut );
						PIXEL10_32( pOut );
						PIXEL11_32( pOut );
						PIXEL12_31( pOut );
						PIXEL13_31( pOut );
						PIXEL20_10( pOut );
						PIXEL21_30( pOut );
						PIXEL22_32( pOut );
						PIXEL23_32( pOut );
						PIXEL30_80( pOut );
						PIXEL31_61( pOut );
						PIXEL32_82( pOut );
						PIXEL33_82( pOut );
						break;
					}
				case 103:
					{
						PIXEL00_81( pOut );
						PIXEL01_31( pOut );
						PIXEL02_32( pOut );
						PIXEL03_82( pOut );
						PIXEL10_81( pOut );
						PIXEL11_31( pOut );
						PIXEL12_32( pOut );
						PIXEL13_82( pOut );
						PIXEL20_82( pOut );
						PIXEL21_32( pOut );
						PIXEL22_30( pOut );
						PIXEL23_61( pOut );
						PIXEL30_82( pOut );
						PIXEL31_32( pOut );
						PIXEL32_10( pOut );
						PIXEL33_80( pOut );
						break;
					}
				case 227:
					{
						PIXEL00_81( pOut );
						PIXEL01_31( pOut );
						PIXEL02_10( pOut );
						PIXEL03_80( pOut );
						PIXEL10_81( pOut );
						PIXEL11_31( pOut );
						PIXEL12_30( pOut );
						PIXEL13_61( pOut );
						PIXEL20_82( pOut );
						PIXEL21_32( pOut );
						PIXEL22_31( pOut );
						PIXEL23_81( pOut );
						PIXEL30_82( pOut );
						PIXEL31_32( pOut );
						PIXEL32_31( pOut );
						PIXEL33_81( pOut );
						break;
					}
				case 230:
					{
						PIXEL00_80( pOut );
						PIXEL01_10( pOut );
						PIXEL02_32( pOut );
						PIXEL03_82( pOut );
						PIXEL10_61( pOut );
						PIXEL11_30( pOut );
						PIXEL12_32( pOut );
						PIXEL13_82( pOut );
						PIXEL20_82( pOut );
						PIXEL21_32( pOut );
						PIXEL22_31( pOut );
						PIXEL23_81( pOut );
						PIXEL30_82( pOut );
						PIXEL31_32( pOut );
						PIXEL32_31( pOut );
						PIXEL33_81( pOut );
						break;
					}
				case 199:
					{
						PIXEL00_81( pOut );
						PIXEL01_31( pOut );
						PIXEL02_32( pOut );
						PIXEL03_82( pOut );
						PIXEL10_81( pOut );
						PIXEL11_31( pOut );
						PIXEL12_32( pOut );
						PIXEL13_82( pOut );
						PIXEL20_61( pOut );
						PIXEL21_30( pOut );
						PIXEL22_31( pOut );
						PIXEL23_81( pOut );
						PIXEL30_80( pOut );
						PIXEL31_10( pOut );
						PIXEL32_31( pOut );
						PIXEL33_81( pOut );
						break;
					}
				case 220:
					{
						PIXEL00_80( pOut );
						PIXEL01_61( pOut );
						PIXEL02_81( pOut );
						PIXEL03_81( pOut );
						PIXEL10_10( pOut );
						PIXEL11_30( pOut );
						PIXEL12_31( pOut );
						PIXEL13_31( pOut );
						if( Diff( cmp2, 8, 4 ) )
						{
							PIXEL20_10( pOut );
							PIXEL21_30( pOut );
							PIXEL30_80( pOut );
							PIXEL31_10( pOut );
						}
						else
						{
							PIXEL20_12( pOut );
							PIXEL21_0( pOut );
							PIXEL30_20( pOut );
							PIXEL31_11( pOut );
						}
						PIXEL22_0( pOut );
						if( Diff( cmp2, 6, 8 ) )
						{
							PIXEL23_0( pOut );
							PIXEL32_0( pOut );
							PIXEL33_0( pOut );
						}
						else
						{
							PIXEL23_50( pOut );
							PIXEL32_50( pOut );
							PIXEL33_50( pOut );
						}
						break;
					}
				case 158:
					{
						if( Diff( cmp2, 4, 2 ) )
						{
							PIXEL00_80( pOut );
							PIXEL01_10( pOut );
							PIXEL10_10( pOut );
							PIXEL11_30( pOut );
						}
						else
						{
							PIXEL00_20( pOut );
							PIXEL01_12( pOut );
							PIXEL10_11( pOut );
							PIXEL11_0( pOut );
						}
						if( Diff( cmp2, 2, 6 ) )
						{
							PIXEL02_0( pOut );
							PIXEL03_0( pOut );
							PIXEL13_0( pOut );
						}
						else
						{
							PIXEL02_50( pOut );
							PIXEL03_50( pOut );
							PIXEL13_50( pOut );
						}
						PIXEL12_0( pOut );
						PIXEL20_10( pOut );
						PIXEL21_30( pOut );
						PIXEL22_32( pOut );
						PIXEL23_32( pOut );
						PIXEL30_80( pOut );
						PIXEL31_61( pOut );
						PIXEL32_82( pOut );
						PIXEL33_82( pOut );
						break;
					}
				case 234:
					{
						if( Diff( cmp2, 4, 2 ) )
						{
							PIXEL00_80( pOut );
							PIXEL01_10( pOut );
							PIXEL10_10( pOut );
							PIXEL11_30( pOut );
						}
						else
						{
							PIXEL00_20( pOut );
							PIXEL01_12( pOut );
							PIXEL10_11( pOut );
							PIXEL11_0( pOut );
						}
						PIXEL02_10( pOut );
						PIXEL03_80( pOut );
						PIXEL12_30( pOut );
						PIXEL13_61( pOut );
						if( Diff( cmp2, 8, 4 ) )
						{
							PIXEL20_0( pOut );
							PIXEL30_0( pOut );
							PIXEL31_0( pOut );
						}
						else
						{
							PIXEL20_50( pOut );
							PIXEL30_50( pOut );
							PIXEL31_50( pOut );
						}
						PIXEL21_0( pOut );
						PIXEL22_31( pOut );
						PIXEL23_81( pOut );
						PIXEL32_31( pOut );
						PIXEL33_81( pOut );
						break;
					}
				case 242:
					{
						PIXEL00_80( pOut );
						PIXEL01_10( pOut );
						if( Diff( cmp2, 2, 6 ) )
						{
							PIXEL02_10( pOut );
							PIXEL03_80( pOut );
							PIXEL12_30( pOut );
							PIXEL13_10( pOut );
						}
						else
						{
							PIXEL02_11( pOut );
							PIXEL03_20( pOut );
							PIXEL12_0( pOut );
							PIXEL13_12( pOut );
						}
						PIXEL10_61( pOut );
						PIXEL11_30( pOut );
						PIXEL20_82( pOut );
						PIXEL21_32( pOut );
						PIXEL22_0( pOut );
						if( Diff( cmp2, 6, 8 ) )
						{
							PIXEL23_0( pOut );
							PIXEL32_0( pOut );
							PIXEL33_0( pOut );
						}
						else
						{
							PIXEL23_50( pOut );
							PIXEL32_50( pOut );
							PIXEL33_50( pOut );
						}
						PIXEL30_82( pOut );
						PIXEL31_32( pOut );
						break;
					}
				case 59:
					{
						if( Diff( cmp2, 4, 2 ) )
						{
							PIXEL00_0( pOut );
							PIXEL01_0( pOut );
							PIXEL10_0( pOut );
						}
						else
						{
							PIXEL00_50( pOut );
							PIXEL01_50( pOut );
							PIXEL10_50( pOut );
						}
						if( Diff( cmp2, 2, 6 ) )
						{
							PIXEL02_10( pOut );
							PIXEL03_80( pOut );
							PIXEL12_30( pOut );
							PIXEL13_10( pOut );
						}
						else
						{
							PIXEL02_11( pOut );
							PIXEL03_20( pOut );
							PIXEL12_0( pOut );
							PIXEL13_12( pOut );
						}
						PIXEL11_0( pOut );
						PIXEL20_31( pOut );
						PIXEL21_31( pOut );
						PIXEL22_30( pOut );
						PIXEL23_10( pOut );
						PIXEL30_81( pOut );
						PIXEL31_81( pOut );
						PIXEL32_61( pOut );
						PIXEL33_80( pOut );
						break;
					}
				case 121:
					{
						PIXEL00_82( pOut );
						PIXEL01_82( pOut );
						PIXEL02_61( pOut );
						PIXEL03_80( pOut );
						PIXEL10_32( pOut );
						PIXEL11_32( pOut );
						PIXEL12_30( pOut );
						PIXEL13_10( pOut );
						if( Diff( cmp2, 8, 4 ) )
						{
							PIXEL20_0( pOut );
							PIXEL30_0( pOut );
							PIXEL31_0( pOut );
						}
						else
						{
							PIXEL20_50( pOut );
							PIXEL30_50( pOut );
							PIXEL31_50( pOut );
						}
						PIXEL21_0( pOut );
						if( Diff( cmp2, 6, 8 ) )
						{
							PIXEL22_30( pOut );
							PIXEL23_10( pOut );
							PIXEL32_10( pOut );
							PIXEL33_80( pOut );
						}
						else
						{
							PIXEL22_0( pOut );
							PIXEL23_11( pOut );
							PIXEL32_12( pOut );
							PIXEL33_20( pOut );
						}
						break;
					}
				case 87:
					{
						PIXEL00_81( pOut );
						PIXEL01_31( pOut );
						if( Diff( cmp2, 2, 6 ) )
						{
							PIXEL02_0( pOut );
							PIXEL03_0( pOut );
							PIXEL13_0( pOut );
						}
						else
						{
							PIXEL02_50( pOut );
							PIXEL03_50( pOut );
							PIXEL13_50( pOut );
						}
						PIXEL10_81( pOut );
						PIXEL11_31( pOut );
						PIXEL12_0( pOut );
						PIXEL20_61( pOut );
						PIXEL21_30( pOut );
						if( Diff( cmp2, 6, 8 ) )
						{
							PIXEL22_30( pOut );
							PIXEL23_10( pOut );
							PIXEL32_10( pOut );
							PIXEL33_80( pOut );
						}
						else
						{
							PIXEL22_0( pOut );
							PIXEL23_11( pOut );
							PIXEL32_12( pOut );
							PIXEL33_20( pOut );
						}
						PIXEL30_80( pOut );
						PIXEL31_10( pOut );
						break;
					}
				case 79:
					{
						if( Diff( cmp2, 4, 2 ) )
						{
							PIXEL00_0( pOut );
							PIXEL01_0( pOut );
							PIXEL10_0( pOut );
						}
						else
						{
							PIXEL00_50( pOut );
							PIXEL01_50( pOut );
							PIXEL10_50( pOut );
						}
						PIXEL02_32( pOut );
						PIXEL03_82( pOut );
						PIXEL11_0( pOut );
						PIXEL12_32( pOut );
						PIXEL13_82( pOut );
						if( Diff( cmp2, 8, 4 ) )
						{
							PIXEL20_10( pOut );
							PIXEL21_30( pOut );
							PIXEL30_80( pOut );
							PIXEL31_10( pOut );
						}
						else
						{
							PIXEL20_12( pOut );
							PIXEL21_0( pOut );
							PIXEL30_20( pOut );
							PIXEL31_11( pOut );
						}
						PIXEL22_30( pOut );
						PIXEL23_61( pOut );
						PIXEL32_10( pOut );
						PIXEL33_80( pOut );
						break;
					}
				case 122:
					{
						if( Diff( cmp2, 4, 2 ) )
						{
							PIXEL00_80( pOut );
							PIXEL01_10( pOut );
							PIXEL10_10( pOut );
							PIXEL11_30( pOut );
						}
						else
						{
							PIXEL00_20( pOut );
							PIXEL01_12( pOut );
							PIXEL10_11( pOut );
							PIXEL11_0( pOut );
						}
						if( Diff( cmp2, 2, 6 ) )
						{
							PIXEL02_10( pOut );
							PIXEL03_80( pOut );
							PIXEL12_30( pOut );
							PIXEL13_10( pOut );
						}
						else
						{
							PIXEL02_11( pOut );
							PIXEL03_20( pOut );
							PIXEL12_0( pOut );
							PIXEL13_12( pOut );
						}
						if( Diff( cmp2, 8, 4 ) )
						{
							PIXEL20_0( pOut );
							PIXEL30_0( pOut );
							PIXEL31_0( pOut );
						}
						else
						{
							PIXEL20_50( pOut );
							PIXEL30_50( pOut );
							PIXEL31_50( pOut );
						}
						PIXEL21_0( pOut );
						if( Diff( cmp2, 6, 8 ) )
						{
							PIXEL22_30( pOut );
							PIXEL23_10( pOut );
							PIXEL32_10( pOut );
							PIXEL33_80( pOut );
						}
						else
						{
							PIXEL22_0( pOut );
							PIXEL23_11( pOut );
							PIXEL32_12( pOut );
							PIXEL33_20( pOut );
						}
						break;
					}
				case 94:
					{
						if( Diff( cmp2, 4, 2 ) )
						{
							PIXEL00_80( pOut );
							PIXEL01_10( pOut );
							PIXEL10_10( pOut );
							PIXEL11_30( pOut );
						}
						else
						{
							PIXEL00_20( pOut );
							PIXEL01_12( pOut );
							PIXEL10_11( pOut );
							PIXEL11_0( pOut );
						}
						if( Diff( cmp2, 2, 6 ) )
						{
							PIXEL02_0( pOut );
							PIXEL03_0( pOut );
							PIXEL13_0( pOut );
						}
						else
						{
							PIXEL02_50( pOut );
							PIXEL03_50( pOut );
							PIXEL13_50( pOut );
						}
						PIXEL12_0( pOut );
						if( Diff( cmp2, 8, 4 ) )
						{
							PIXEL20_10( pOut );
							PIXEL21_30( pOut );
							PIXEL30_80( pOut );
							PIXEL31_10( pOut );
						}
						else
						{
							PIXEL20_12( pOut );
							PIXEL21_0( pOut );
							PIXEL30_20( pOut );
							PIXEL31_11( pOut );
						}
						if( Diff( cmp2, 6, 8 ) )
						{
							PIXEL22_30( pOut );
							PIXEL23_10( pOut );
							PIXEL32_10( pOut );
							PIXEL33_80( pOut );
						}
						else
						{
							PIXEL22_0( pOut );
							PIXEL23_11( pOut );
							PIXEL32_12( pOut );
							PIXEL33_20( pOut );
						}
						break;
					}
				case 218:
					{
						if( Diff( cmp2, 4, 2 ) )
						{
							PIXEL00_80( pOut );
							PIXEL01_10( pOut );
							PIXEL10_10( pOut );
							PIXEL11_30( pOut );
						}
						else
						{
							PIXEL00_20( pOut );
							PIXEL01_12( pOut );
							PIXEL10_11( pOut );
							PIXEL11_0( pOut );
						}
						if( Diff( cmp2, 2, 6 ) )
						{
							PIXEL02_10( pOut );
							PIXEL03_80( pOut );
							PIXEL12_30( pOut );
							PIXEL13_10( pOut );
						}
						else
						{
							PIXEL02_11( pOut );
							PIXEL03_20( pOut );
							PIXEL12_0( pOut );
							PIXEL13_12( pOut );
						}
						if( Diff( cmp2, 8, 4 ) )
						{
							PIXEL20_10( pOut );
							PIXEL21_30( pOut );
							PIXEL30_80( pOut );
							PIXEL31_10( pOut );
						}
						else
						{
							PIXEL20_12( pOut );
							PIXEL21_0( pOut );
							PIXEL30_20( pOut );
							PIXEL31_11( pOut );
						}
						PIXEL22_0( pOut );
						if( Diff( cmp2, 6, 8 ) )
						{
							PIXEL23_0( pOut );
							PIXEL32_0( pOut );
							PIXEL33_0( pOut );
						}
						else
						{
							PIXEL23_50( pOut );
							PIXEL32_50( pOut );
							PIXEL33_50( pOut );
						}
						break;
					}
				case 91:
					{
						if( Diff( cmp2, 4, 2 ) )
						{
							PIXEL00_0( pOut );
							PIXEL01_0( pOut );
							PIXEL10_0( pOut );
						}
						else
						{
							PIXEL00_50( pOut );
							PIXEL01_50( pOut );
							PIXEL10_50( pOut );
						}
						if( Diff( cmp2, 2, 6 ) )
						{
							PIXEL02_10( pOut );
							PIXEL03_80( pOut );
							PIXEL12_30( pOut );
							PIXEL13_10( pOut );
						}
						else
						{
							PIXEL02_11( pOut );
							PIXEL03_20( pOut );
							PIXEL12_0( pOut );
							PIXEL13_12( pOut );
						}
						PIXEL11_0( pOut );
						if( Diff( cmp2, 8, 4 ) )
						{
							PIXEL20_10( pOut );
							PIXEL21_30( pOut );
							PIXEL30_80( pOut );
							PIXEL31_10( pOut );
						}
						else
						{
							PIXEL20_12( pOut );
							PIXEL21_0( pOut );
							PIXEL30_20( pOut );
							PIXEL31_11( pOut );
						}
						if( Diff( cmp2, 6, 8 ) )
						{
							PIXEL22_30( pOut );
							PIXEL23_10( pOut );
							PIXEL32_10( pOut );
							PIXEL33_80( pOut );
						}
						else
						{
							PIXEL22_0( pOut );
							PIXEL23_11( pOut );
							PIXEL32_12( pOut );
							PIXEL33_20( pOut );
						}
						break;
					}
				case 229:
					{
						PIXEL00_20( pOut );
						PIXEL01_60( pOut );
						PIXEL02_60( pOut );
						PIXEL03_20( pOut );
						PIXEL10_60( pOut );
						PIXEL11_70( pOut );
						PIXEL12_70( pOut );
						PIXEL13_60( pOut );
						PIXEL20_82( pOut );
						PIXEL21_32( pOut );
						PIXEL22_31( pOut );
						PIXEL23_81( pOut );
						PIXEL30_82( pOut );
						PIXEL31_32( pOut );
						PIXEL32_31( pOut );
						PIXEL33_81( pOut );
						break;
					}
				case 167:
					{
						PIXEL00_81( pOut );
						PIXEL01_31( pOut );
						PIXEL02_32( pOut );
						PIXEL03_82( pOut );
						PIXEL10_81( pOut );
						PIXEL11_31( pOut );
						PIXEL12_32( pOut );
						PIXEL13_82( pOut );
						PIXEL20_60( pOut );
						PIXEL21_70( pOut );
						PIXEL22_70( pOut );
						PIXEL23_60( pOut );
						PIXEL30_20( pOut );
						PIXEL31_60( pOut );
						PIXEL32_60( pOut );
						PIXEL33_20( pOut );
						break;
					}
				case 173:
					{
						PIXEL00_82( pOut );
						PIXEL01_82( pOut );
						PIXEL02_60( pOut );
						PIXEL03_20( pOut );
						PIXEL10_32( pOut );
						PIXEL11_32( pOut );
						PIXEL12_70( pOut );
						PIXEL13_60( pOut );
						PIXEL20_31( pOut );
						PIXEL21_31( pOut );
						PIXEL22_70( pOut );
						PIXEL23_60( pOut );
						PIXEL30_81( pOut );
						PIXEL31_81( pOut );
						PIXEL32_60( pOut );
						PIXEL33_20( pOut );
						break;
					}
				case 181:
					{
						PIXEL00_20( pOut );
						PIXEL01_60( pOut );
						PIXEL02_81( pOut );
						PIXEL03_81( pOut );
						PIXEL10_60( pOut );
						PIXEL11_70( pOut );
						PIXEL12_31( pOut );
						PIXEL13_31( pOut );
						PIXEL20_60( pOut );
						PIXEL21_70( pOut );
						PIXEL22_32( pOut );
						PIXEL23_32( pOut );
						PIXEL30_20( pOut );
						PIXEL31_60( pOut );
						PIXEL32_82( pOut );
						PIXEL33_82( pOut );
						break;
					}
				case 186:
					{
						if( Diff( cmp2, 4, 2 ) )
						{
							PIXEL00_80( pOut );
							PIXEL01_10( pOut );
							PIXEL10_10( pOut );
							PIXEL11_30( pOut );
						}
						else
						{
							PIXEL00_20( pOut );
							PIXEL01_12( pOut );
							PIXEL10_11( pOut );
							PIXEL11_0( pOut );
						}
						if( Diff( cmp2, 2, 6 ) )
						{
							PIXEL02_10( pOut );
							PIXEL03_80( pOut );
							PIXEL12_30( pOut );
							PIXEL13_10( pOut );
						}
						else
						{
							PIXEL02_11( pOut );
							PIXEL03_20( pOut );
							PIXEL12_0( pOut );
							PIXEL13_12( pOut );
						}
						PIXEL20_31( pOut );
						PIXEL21_31( pOut );
						PIXEL22_32( pOut );
						PIXEL23_32( pOut );
						PIXEL30_81( pOut );
						PIXEL31_81( pOut );
						PIXEL32_82( pOut );
						PIXEL33_82( pOut );
						break;
					}
				case 115:
					{
						PIXEL00_81( pOut );
						PIXEL01_31( pOut );
						if( Diff( cmp2, 2, 6 ) )
						{
							PIXEL02_10( pOut );
							PIXEL03_80( pOut );
							PIXEL12_30( pOut );
							PIXEL13_10( pOut );
						}
						else
						{
							PIXEL02_11( pOut );
							PIXEL03_20( pOut );
							PIXEL12_0( pOut );
							PIXEL13_12( pOut );
						}
						PIXEL10_81( pOut );
						PIXEL11_31( pOut );
						PIXEL20_82( pOut );
						PIXEL21_32( pOut );
						if( Diff( cmp2, 6, 8 ) )
						{
							PIXEL22_30( pOut );
							PIXEL23_10( pOut );
							PIXEL32_10( pOut );
							PIXEL33_80( pOut );
						}
						else
						{
							PIXEL22_0( pOut );
							PIXEL23_11( pOut );
							PIXEL32_12( pOut );
							PIXEL33_20( pOut );
						}
						PIXEL30_82( pOut );
						PIXEL31_32( pOut );
						break;
					}
				case 93:
					{
						PIXEL00_82( pOut );
						PIXEL01_82( pOut );
						PIXEL02_81( pOut );
						PIXEL03_81( pOut );
						PIXEL10_32( pOut );
						PIXEL11_32( pOut );
						PIXEL12_31( pOut );
						PIXEL13_31( pOut );
						if( Diff( cmp2, 8, 4 ) )
						{
							PIXEL20_10( pOut );
							PIXEL21_30( pOut );
							PIXEL30_80( pOut );
							PIXEL31_10( pOut );
						}
						else
						{
							PIXEL20_12( pOut );
							PIXEL21_0( pOut );
							PIXEL30_20( pOut );
							PIXEL31_11( pOut );
						}
						if( Diff( cmp2, 6, 8 ) )
						{
							PIXEL22_30( pOut );
							PIXEL23_10( pOut );
							PIXEL32_10( pOut );
							PIXEL33_80( pOut );
						}
						else
						{
							PIXEL22_0( pOut );
							PIXEL23_11( pOut );
							PIXEL32_12( pOut );
							PIXEL33_20( pOut );
						}
						break;
					}
				case 206:
					{
						if( Diff( cmp2, 4, 2 ) )
						{
							PIXEL00_80( pOut );
							PIXEL01_10( pOut );
							PIXEL10_10( pOut );
							PIXEL11_30( pOut );
						}
						else
						{
							PIXEL00_20( pOut );
							PIXEL01_12( pOut );
							PIXEL10_11( pOut );
							PIXEL11_0( pOut );
						}
						PIXEL02_32( pOut );
						PIXEL03_82( pOut );
						PIXEL12_32( pOut );
						PIXEL13_82( pOut );
						if( Diff( cmp2, 8, 4 ) )
						{
							PIXEL20_10( pOut );
							PIXEL21_30( pOut );
							PIXEL30_80( pOut );
							PIXEL31_10( pOut );
						}
						else
						{
							PIXEL20_12( pOut );
							PIXEL21_0( pOut );
							PIXEL30_20( pOut );
							PIXEL31_11( pOut );
						}
						PIXEL22_31( pOut );
						PIXEL23_81( pOut );
						PIXEL32_31( pOut );
						PIXEL33_81( pOut );
						break;
					}
				case 205:
				case 201:
					{
						PIXEL00_82( pOut );
						PIXEL01_82( pOut );
						PIXEL02_60( pOut );
						PIXEL03_20( pOut );
						PIXEL10_32( pOut );
						PIXEL11_32( pOut );
						PIXEL12_70( pOut );
						PIXEL13_60( pOut );
						if( Diff( cmp2, 8, 4 ) )
						{
							PIXEL20_10( pOut );
							PIXEL21_30( pOut );
							PIXEL30_80( pOut );
							PIXEL31_10( pOut );
						}
						else
						{
							PIXEL20_12( pOut );
							PIXEL21_0( pOut );
							PIXEL30_20( pOut );
							PIXEL31_11( pOut );
						}
						PIXEL22_31( pOut );
						PIXEL23_81( pOut );
						PIXEL32_31( pOut );
						PIXEL33_81( pOut );
						break;
					}
				case 174:
				case 46:
					{
						if( Diff( cmp2, 4, 2 ) )
						{
							PIXEL00_80( pOut );
							PIXEL01_10( pOut );
							PIXEL10_10( pOut );
							PIXEL11_30( pOut );
						}
						else
						{
							PIXEL00_20( pOut );
							PIXEL01_12( pOut );
							PIXEL10_11( pOut );
							PIXEL11_0( pOut );
						}
						PIXEL02_32( pOut );
						PIXEL03_82( pOut );
						PIXEL12_32( pOut );
						PIXEL13_82( pOut );
						PIXEL20_31( pOut );
						PIXEL21_31( pOut );
						PIXEL22_70( pOut );
						PIXEL23_60( pOut );
						PIXEL30_81( pOut );
						PIXEL31_81( pOut );
						PIXEL32_60( pOut );
						PIXEL33_20( pOut );
						break;
					}
				case 179:
				case 147:
					{
						PIXEL00_81( pOut );
						PIXEL01_31( pOut );
						if( Diff( cmp2, 2, 6 ) )
						{
							PIXEL02_10( pOut );
							PIXEL03_80( pOut );
							PIXEL12_30( pOut );
							PIXEL13_10( pOut );
						}
						else
						{
							PIXEL02_11( pOut );
							PIXEL03_20( pOut );
							PIXEL12_0( pOut );
							PIXEL13_12( pOut );
						}
						PIXEL10_81( pOut );
						PIXEL11_31( pOut );
						PIXEL20_60( pOut );
						PIXEL21_70( pOut );
						PIXEL22_32( pOut );
						PIXEL23_32( pOut );
						PIXEL30_20( pOut );
						PIXEL31_60( pOut );
						PIXEL32_82( pOut );
						PIXEL33_82( pOut );
						break;
					}
				case 117:
				case 116:
					{
						PIXEL00_20( pOut );
						PIXEL01_60( pOut );
						PIXEL02_81( pOut );
						PIXEL03_81( pOut );
						PIXEL10_60( pOut );
						PIXEL11_70( pOut );
						PIXEL12_31( pOut );
						PIXEL13_31( pOut );
						PIXEL20_82( pOut );
						PIXEL21_32( pOut );
						if( Diff( cmp2, 6, 8 ) )
						{
							PIXEL22_30( pOut );
							PIXEL23_10( pOut );
							PIXEL32_10( pOut );
							PIXEL33_80( pOut );
						}
						else
						{
							PIXEL22_0( pOut );
							PIXEL23_11( pOut );
							PIXEL32_12( pOut );
							PIXEL33_20( pOut );
						}
						PIXEL30_82( pOut );
						PIXEL31_32( pOut );
						break;
					}
				case 189:
					{
						PIXEL00_82( pOut );
						PIXEL01_82( pOut );
						PIXEL02_81( pOut );
						PIXEL03_81( pOut );
						PIXEL10_32( pOut );
						PIXEL11_32( pOut );
						PIXEL12_31( pOut );
						PIXEL13_31( pOut );
						PIXEL20_31( pOut );
						PIXEL21_31( pOut );
						PIXEL22_32( pOut );
						PIXEL23_32( pOut );
						PIXEL30_81( pOut );
						PIXEL31_81( pOut );
						PIXEL32_82( pOut );
						PIXEL33_82( pOut );
						break;
					}
				case 231:
					{
						PIXEL00_81( pOut );
						PIXEL01_31( pOut );
						PIXEL02_32( pOut );
						PIXEL03_82( pOut );
						PIXEL10_81( pOut );
						PIXEL11_31( pOut );
						PIXEL12_32( pOut );
						PIXEL13_82( pOut );
						PIXEL20_82( pOut );
						PIXEL21_32( pOut );
						PIXEL22_31( pOut );
						PIXEL23_81( pOut );
						PIXEL30_82( pOut );
						PIXEL31_32( pOut );
						PIXEL32_31( pOut );
						PIXEL33_81( pOut );
						break;
					}
				case 126:
					{
						PIXEL00_80( pOut );
						PIXEL01_10( pOut );
						if( Diff( cmp2, 2, 6 ) )
						{
							PIXEL02_0( pOut );
							PIXEL03_0( pOut );
							PIXEL13_0( pOut );
						}
						else
						{
							PIXEL02_50( pOut );
							PIXEL03_50( pOut );
							PIXEL13_50( pOut );
						}
						PIXEL10_10( pOut );
						PIXEL11_30( pOut );
						PIXEL12_0( pOut );
						if( Diff( cmp2, 8, 4 ) )
						{
							PIXEL20_0( pOut );
							PIXEL30_0( pOut );
							PIXEL31_0( pOut );
						}
						else
						{
							PIXEL20_50( pOut );
							PIXEL30_50( pOut );
							PIXEL31_50( pOut );
						}
						PIXEL21_0( pOut );
						PIXEL22_30( pOut );
						PIXEL23_10( pOut );
						PIXEL32_10( pOut );
						PIXEL33_80( pOut );
						break;
					}
				case 219:
					{
						if( Diff( cmp2, 4, 2 ) )
						{
							PIXEL00_0( pOut );
							PIXEL01_0( pOut );
							PIXEL10_0( pOut );
						}
						else
						{
							PIXEL00_50( pOut );
							PIXEL01_50( pOut );
							PIXEL10_50( pOut );
						}
						PIXEL02_10( pOut );
						PIXEL03_80( pOut );
						PIXEL11_0( pOut );
						PIXEL12_30( pOut );
						PIXEL13_10( pOut );
						PIXEL20_10( pOut );
						PIXEL21_30( pOut );
						PIXEL22_0( pOut );
						if( Diff( cmp2, 6, 8 ) )
						{
							PIXEL23_0( pOut );
							PIXEL32_0( pOut );
							PIXEL33_0( pOut );
						}
						else
						{
							PIXEL23_50( pOut );
							PIXEL32_50( pOut );
							PIXEL33_50( pOut );
						}
						PIXEL30_80( pOut );
						PIXEL31_10( pOut );
						break;
					}
				case 125:
					{
						if( Diff( cmp2, 8, 4 ) )
						{
							PIXEL00_82( pOut );
							PIXEL10_32( pOut );
							PIXEL20_0( pOut );
							PIXEL21_0( pOut );
							PIXEL30_0( pOut );
							PIXEL31_0( pOut );
						}
						else
						{
							PIXEL00_11( pOut );
							PIXEL10_13( pOut );
							PIXEL20_83( pOut );
							PIXEL21_70( pOut );
							PIXEL30_50( pOut );
							PIXEL31_21( pOut );
						}
						PIXEL01_82( pOut );
						PIXEL02_81( pOut );
						PIXEL03_81( pOut );
						PIXEL11_32( pOut );
						PIXEL12_31( pOut );
						PIXEL13_31( pOut );
						PIXEL22_30( pOut );
						PIXEL23_10( pOut );
						PIXEL32_10( pOut );
						PIXEL33_80( pOut );
						break;
					}
				case 221:
					{
						PIXEL00_82( pOut );
						PIXEL01_82( pOut );
						PIXEL02_81( pOut );
						if( Diff( cmp2, 6, 8 ) )
						{
							PIXEL03_81( pOut );
							PIXEL13_31( pOut );
							PIXEL22_0( pOut );
							PIXEL23_0( pOut );
							PIXEL32_0( pOut );
							PIXEL33_0( pOut );
						}
						else
						{
							PIXEL03_12( pOut );
							PIXEL13_14( pOut );
							PIXEL22_70( pOut );
							PIXEL23_83( pOut );
							PIXEL32_21( pOut );
							PIXEL33_50( pOut );
						}
						PIXEL10_32( pOut );
						PIXEL11_32( pOut );
						PIXEL12_31( pOut );
						PIXEL20_10( pOut );
						PIXEL21_30( pOut );
						PIXEL30_80( pOut );
						PIXEL31_10( pOut );
						break;
					}
				case 207:
					{
						if( Diff( cmp2, 4, 2 ) )
						{
							PIXEL00_0( pOut );
							PIXEL01_0( pOut );
							PIXEL02_32( pOut );
							PIXEL03_82( pOut );
							PIXEL10_0( pOut );
							PIXEL11_0( pOut );
						}
						else
						{
							PIXEL00_50( pOut );
							PIXEL01_83( pOut );
							PIXEL02_13( pOut );
							PIXEL03_11( pOut );
							PIXEL10_21( pOut );
							PIXEL11_70( pOut );
						}
						PIXEL12_32( pOut );
						PIXEL13_82( pOut );
						PIXEL20_10( pOut );
						PIXEL21_30( pOut );
						PIXEL22_31( pOut );
						PIXEL23_81( pOut );
						PIXEL30_80( pOut );
						PIXEL31_10( pOut );
						PIXEL32_31( pOut );
						PIXEL33_81( pOut );
						break;
					}
				case 238:
					{
						PIXEL00_80( pOut );
						PIXEL01_10( pOut );
						PIXEL02_32( pOut );
						PIXEL03_82( pOut );
						PIXEL10_10( pOut );
						PIXEL11_30( pOut );
						PIXEL12_32( pOut );
						PIXEL13_82( pOut );
						if( Diff( cmp2, 8, 4 ) )
						{
							PIXEL20_0( pOut );
							PIXEL21_0( pOut );
							PIXEL30_0( pOut );
							PIXEL31_0( pOut );
							PIXEL32_31( pOut );
							PIXEL33_81( pOut );
						}
						else
						{
							PIXEL20_21( pOut );
							PIXEL21_70( pOut );
							PIXEL30_50( pOut );
							PIXEL31_83( pOut );
							PIXEL32_14( pOut );
							PIXEL33_12( pOut );
						}
						PIXEL22_31( pOut );
						PIXEL23_81( pOut );
						break;
					}
				case 190:
					{
						PIXEL00_80( pOut );
						PIXEL01_10( pOut );
						if( Diff( cmp2, 2, 6 ) )
						{
							PIXEL02_0( pOut );
							PIXEL03_0( pOut );
							PIXEL12_0( pOut );
							PIXEL13_0( pOut );
							PIXEL23_32( pOut );
							PIXEL33_82( pOut );
						}
						else
						{
							PIXEL02_21( pOut );
							PIXEL03_50( pOut );
							PIXEL12_70( pOut );
							PIXEL13_83( pOut );
							PIXEL23_13( pOut );
							PIXEL33_11( pOut );
						}
						PIXEL10_10( pOut );
						PIXEL11_30( pOut );
						PIXEL20_31( pOut );
						PIXEL21_31( pOut );
						PIXEL22_32( pOut );
						PIXEL30_81( pOut );
						PIXEL31_81( pOut );
						PIXEL32_82( pOut );
						break;
					}
				case 187:
					{
						if( Diff( cmp2, 4, 2 ) )
						{
							PIXEL00_0( pOut );
							PIXEL01_0( pOut );
							PIXEL10_0( pOut );
							PIXEL11_0( pOut );
							PIXEL20_31( pOut );
							PIXEL30_81( pOut );
						}
						else
						{
							PIXEL00_50( pOut );
							PIXEL01_21( pOut );
							PIXEL10_83( pOut );
							PIXEL11_70( pOut );
							PIXEL20_14( pOut );
							PIXEL30_12( pOut );
						}
						PIXEL02_10( pOut );
						PIXEL03_80( pOut );
						PIXEL12_30( pOut );
						PIXEL13_10( pOut );
						PIXEL21_31( pOut );
						PIXEL22_32( pOut );
						PIXEL23_32( pOut );
						PIXEL31_81( pOut );
						PIXEL32_82( pOut );
						PIXEL33_82( pOut );
						break;
					}
				case 243:
					{
						PIXEL00_81( pOut );
						PIXEL01_31( pOut );
						PIXEL02_10( pOut );
						PIXEL03_80( pOut );
						PIXEL10_81( pOut );
						PIXEL11_31( pOut );
						PIXEL12_30( pOut );
						PIXEL13_10( pOut );
						PIXEL20_82( pOut );
						PIXEL21_32( pOut );
						if( Diff( cmp2, 6, 8 ) )
						{
							PIXEL22_0( pOut );
							PIXEL23_0( pOut );
							PIXEL30_82( pOut );
							PIXEL31_32( pOut );
							PIXEL32_0( pOut );
							PIXEL33_0( pOut );
						}
						else
						{
							PIXEL22_70( pOut );
							PIXEL23_21( pOut );
							PIXEL30_11( pOut );
							PIXEL31_13( pOut );
							PIXEL32_83( pOut );
							PIXEL33_50( pOut );
						}
						break;
					}
				case 119:
					{
						if( Diff( cmp2, 2, 6 ) )
						{
							PIXEL00_81( pOut );
							PIXEL01_31( pOut );
							PIXEL02_0( pOut );
							PIXEL03_0( pOut );
							PIXEL12_0( pOut );
							PIXEL13_0( pOut );
						}
						else
						{
							PIXEL00_12( pOut );
							PIXEL01_14( pOut );
							PIXEL02_83( pOut );
							PIXEL03_50( pOut );
							PIXEL12_70( pOut );
							PIXEL13_21( pOut );
						}
						PIXEL10_81( pOut );
						PIXEL11_31( pOut );
						PIXEL20_82( pOut );
						PIXEL21_32( pOut );
						PIXEL22_30( pOut );
						PIXEL23_10( pOut );
						PIXEL30_82( pOut );
						PIXEL31_32( pOut );
						PIXEL32_10( pOut );
						PIXEL33_80( pOut );
						break;
					}
				case 237:
				case 233:
					{
						PIXEL00_82( pOut );
						PIXEL01_82( pOut );
						PIXEL02_60( pOut );
						PIXEL03_20( pOut );
						PIXEL10_32( pOut );
						PIXEL11_32( pOut );
						PIXEL12_70( pOut );
						PIXEL13_60( pOut );
						PIXEL20_0( pOut );
						PIXEL21_0( pOut );
						PIXEL22_31( pOut );
						PIXEL23_81( pOut );
						if( Diff( cmp2, 8, 4 ) )
						{
							PIXEL30_0( pOut );
						}
						else
						{
							PIXEL30_20( pOut );
						}
						PIXEL31_0( pOut );
						PIXEL32_31( pOut );
						PIXEL33_81( pOut );
						break;
					}
				case 175:
				case 47:
					{
						if( Diff( cmp2, 4, 2 ) )
						{
							PIXEL00_0( pOut );
						}
						else
						{
							PIXEL00_20( pOut );
						}
						PIXEL01_0( pOut );
						PIXEL02_32( pOut );
						PIXEL03_82( pOut );
						PIXEL10_0( pOut );
						PIXEL11_0( pOut );
						PIXEL12_32( pOut );
						PIXEL13_82( pOut );
						PIXEL20_31( pOut );
						PIXEL21_31( pOut );
						PIXEL22_70( pOut );
						PIXEL23_60( pOut );
						PIXEL30_81( pOut );
						PIXEL31_81( pOut );
						PIXEL32_60( pOut );
						PIXEL33_20( pOut );
						break;
					}
				case 183:
				case 151:
					{
						PIXEL00_81( pOut );
						PIXEL01_31( pOut );
						PIXEL02_0( pOut );
						if( Diff( cmp2, 2, 6 ) )
						{
							PIXEL03_0( pOut );
						}
						else
						{
							PIXEL03_20( pOut );
						}
						PIXEL10_81( pOut );
						PIXEL11_31( pOut );
						PIXEL12_0( pOut );
						PIXEL13_0( pOut );
						PIXEL20_60( pOut );
						PIXEL21_70( pOut );
						PIXEL22_32( pOut );
						PIXEL23_32( pOut );
						PIXEL30_20( pOut );
						PIXEL31_60( pOut );
						PIXEL32_82( pOut );
						PIXEL33_82( pOut );
						break;
					}
				case 245:
				case 244:
					{
						PIXEL00_20( pOut );
						PIXEL01_60( pOut );
						PIXEL02_81( pOut );
						PIXEL03_81( pOut );
						PIXEL10_60( pOut );
						PIXEL11_70( pOut );
						PIXEL12_31( pOut );
						PIXEL13_31( pOut );
						PIXEL20_82( pOut );
						PIXEL21_32( pOut );
						PIXEL22_0( pOut );
						PIXEL23_0( pOut );
						PIXEL30_82( pOut );
						PIXEL31_32( pOut );
						PIXEL32_0( pOut );
						if( Diff( cmp2, 6, 8 ) )
						{
							PIXEL33_0( pOut );
						}
						else
						{
							PIXEL33_20( pOut );
						}
						break;
					}
				case 250:
					{
						PIXEL00_80( pOut );
						PIXEL01_10( pOut );
						PIXEL02_10( pOut );
						PIXEL03_80( pOut );
						PIXEL10_10( pOut );
						PIXEL11_30( pOut );
						PIXEL12_30( pOut );
						PIXEL13_10( pOut );
						if( Diff( cmp2, 8, 4 ) )
						{
							PIXEL20_0( pOut );
							PIXEL30_0( pOut );
							PIXEL31_0( pOut );
						}
						else
						{
							PIXEL20_50( pOut );
							PIXEL30_50( pOut );
							PIXEL31_50( pOut );
						}
						PIXEL21_0( pOut );
						PIXEL22_0( pOut );
						if( Diff( cmp2, 6, 8 ) )
						{
							PIXEL23_0( pOut );
							PIXEL32_0( pOut );
							PIXEL33_0( pOut );
						}
						else
						{
							PIXEL23_50( pOut );
							PIXEL32_50( pOut );
							PIXEL33_50( pOut );
						}
						break;
					}
				case 123:
					{
						if( Diff( cmp2, 4, 2 ) )
						{
							PIXEL00_0( pOut );
							PIXEL01_0( pOut );
							PIXEL10_0( pOut );
						}
						else
						{
							PIXEL00_50( pOut );
							PIXEL01_50( pOut );
							PIXEL10_50( pOut );
						}
						PIXEL02_10( pOut );
						PIXEL03_80( pOut );
						PIXEL11_0( pOut );
						PIXEL12_30( pOut );
						PIXEL13_10( pOut );
						if( Diff( cmp2, 8, 4 ) )
						{
							PIXEL20_0( pOut );
							PIXEL30_0( pOut );
							PIXEL31_0( pOut );
						}
						else
						{
							PIXEL20_50( pOut );
							PIXEL30_50( pOut );
							PIXEL31_50( pOut );
						}
						PIXEL21_0( pOut );
						PIXEL22_30( pOut );
						PIXEL23_10( pOut );
						PIXEL32_10( pOut );
						PIXEL33_80( pOut );
						break;
					}
				case 95:
					{
						if( Diff( cmp2, 4, 2 ) )
						{
							PIXEL00_0( pOut );
							PIXEL01_0( pOut );
							PIXEL10_0( pOut );
						}
						else
						{
							PIXEL00_50( pOut );
							PIXEL01_50( pOut );
							PIXEL10_50( pOut );
						}
						if( Diff( cmp2, 2, 6 ) )
						{
							PIXEL02_0( pOut );
							PIXEL03_0( pOut );
							PIXEL13_0( pOut );
						}
						else
						{
							PIXEL02_50( pOut );
							PIXEL03_50( pOut );
							PIXEL13_50( pOut );
						}
						PIXEL11_0( pOut );
						PIXEL12_0( pOut );
						PIXEL20_10( pOut );
						PIXEL21_30( pOut );
						PIXEL22_30( pOut );
						PIXEL23_10( pOut );
						PIXEL30_80( pOut );
						PIXEL31_10( pOut );
						PIXEL32_10( pOut );
						PIXEL33_80( pOut );
						break;
					}
				case 222:
					{
						PIXEL00_80( pOut );
						PIXEL01_10( pOut );
						if( Diff( cmp2, 2, 6 ) )
						{
							PIXEL02_0( pOut );
							PIXEL03_0( pOut );
							PIXEL13_0( pOut );
						}
						else
						{
							PIXEL02_50( pOut );
							PIXEL03_50( pOut );
							PIXEL13_50( pOut );
						}
						PIXEL10_10( pOut );
						PIXEL11_30( pOut );
						PIXEL12_0( pOut );
						PIXEL20_10( pOut );
						PIXEL21_30( pOut );
						PIXEL22_0( pOut );
						if( Diff( cmp2, 6, 8 ) )
						{
							PIXEL23_0( pOut );
							PIXEL32_0( pOut );
							PIXEL33_0( pOut );
						}
						else
						{
							PIXEL23_50( pOut );
							PIXEL32_50( pOut );
							PIXEL33_50( pOut );
						}
						PIXEL30_80( pOut );
						PIXEL31_10( pOut );
						break;
					}
				case 252:
					{
						PIXEL00_80( pOut );
						PIXEL01_61( pOut );
						PIXEL02_81( pOut );
						PIXEL03_81( pOut );
						PIXEL10_10( pOut );
						PIXEL11_30( pOut );
						PIXEL12_31( pOut );
						PIXEL13_31( pOut );
						if( Diff( cmp2, 8, 4 ) )
						{
							PIXEL20_0( pOut );
							PIXEL30_0( pOut );
							PIXEL31_0( pOut );
						}
						else
						{
							PIXEL20_50( pOut );
							PIXEL30_50( pOut );
							PIXEL31_50( pOut );
						}
						PIXEL21_0( pOut );
						PIXEL22_0( pOut );
						PIXEL23_0( pOut );
						PIXEL32_0( pOut );
						if( Diff( cmp2, 6, 8 ) )
						{
							PIXEL33_0( pOut );
						}
						else
						{
							PIXEL33_20( pOut );
						}
						break;
					}
				case 249:
					{
						PIXEL00_82( pOut );
						PIXEL01_82( pOut );
						PIXEL02_61( pOut );
						PIXEL03_80( pOut );
						PIXEL10_32( pOut );
						PIXEL11_32( pOut );
						PIXEL12_30( pOut );
						PIXEL13_10( pOut );
						PIXEL20_0( pOut );
						PIXEL21_0( pOut );
						PIXEL22_0( pOut );
						if( Diff( cmp2, 6, 8 ) )
						{
							PIXEL23_0( pOut );
							PIXEL32_0( pOut );
							PIXEL33_0( pOut );
						}
						else
						{
							PIXEL23_50( pOut );
							PIXEL32_50( pOut );
							PIXEL33_50( pOut );
						}
						if( Diff( cmp2, 8, 4 ) )
						{
							PIXEL30_0( pOut );
						}
						else
						{
							PIXEL30_20( pOut );
						}
						PIXEL31_0( pOut );
						break;
					}
				case 235:
					{
						if( Diff( cmp2, 4, 2 ) )
						{
							PIXEL00_0( pOut );
							PIXEL01_0( pOut );
							PIXEL10_0( pOut );
						}
						else
						{
							PIXEL00_50( pOut );
							PIXEL01_50( pOut );
							PIXEL10_50( pOut );
						}
						PIXEL02_10( pOut );
						PIXEL03_80( pOut );
						PIXEL11_0( pOut );
						PIXEL12_30( pOut );
						PIXEL13_61( pOut );
						PIXEL20_0( pOut );
						PIXEL21_0( pOut );
						PIXEL22_31( pOut );
						PIXEL23_81( pOut );
						if( Diff( cmp2, 8, 4 ) )
						{
							PIXEL30_0( pOut );
						}
						else
						{
							PIXEL30_20( pOut );
						}
						PIXEL31_0( pOut );
						PIXEL32_31( pOut );
						PIXEL33_81( pOut );
						break;
					}
				case 111:
					{
						if( Diff( cmp2, 4, 2 ) )
						{
							PIXEL00_0( pOut );
						}
						else
						{
							PIXEL00_20( pOut );
						}
						PIXEL01_0( pOut );
						PIXEL02_32( pOut );
						PIXEL03_82( pOut );
						PIXEL10_0( pOut );
						PIXEL11_0( pOut );
						PIXEL12_32( pOut );
						PIXEL13_82( pOut );
						if( Diff( cmp2, 8, 4 ) )
						{
							PIXEL20_0( pOut );
							PIXEL30_0( pOut );
							PIXEL31_0( pOut );
						}
						else
						{
							PIXEL20_50( pOut );
							PIXEL30_50( pOut );
							PIXEL31_50( pOut );
						}
						PIXEL21_0( pOut );
						PIXEL22_30( pOut );
						PIXEL23_61( pOut );
						PIXEL32_10( pOut );
						PIXEL33_80( pOut );
						break;
					}
				case 63:
					{
						if( Diff( cmp2, 4, 2 ) )
						{
							PIXEL00_0( pOut );
						}
						else
						{
							PIXEL00_20( pOut );
						}
						PIXEL01_0( pOut );
						if( Diff( cmp2, 2, 6 ) )
						{
							PIXEL02_0( pOut );
							PIXEL03_0( pOut );
							PIXEL13_0( pOut );
						}
						else
						{
							PIXEL02_50( pOut );
							PIXEL03_50( pOut );
							PIXEL13_50( pOut );
						}
						PIXEL10_0( pOut );
						PIXEL11_0( pOut );
						PIXEL12_0( pOut );
						PIXEL20_31( pOut );
						PIXEL21_31( pOut );
						PIXEL22_30( pOut );
						PIXEL23_10( pOut );
						PIXEL30_81( pOut );
						PIXEL31_81( pOut );
						PIXEL32_61( pOut );
						PIXEL33_80( pOut );
						break;
					}
				case 159:
					{
						if( Diff( cmp2, 4, 2 ) )
						{
							PIXEL00_0( pOut );
							PIXEL01_0( pOut );
							PIXEL10_0( pOut );
						}
						else
						{
							PIXEL00_50( pOut );
							PIXEL01_50( pOut );
							PIXEL10_50( pOut );
						}
						PIXEL02_0( pOut );
						if( Diff( cmp2, 2, 6 ) )
						{
							PIXEL03_0( pOut );
						}
						else
						{
							PIXEL03_20( pOut );
						}
						PIXEL11_0( pOut );
						PIXEL12_0( pOut );
						PIXEL13_0( pOut );
						PIXEL20_10( pOut );
						PIXEL21_30( pOut );
						PIXEL22_32( pOut );
						PIXEL23_32( pOut );
						PIXEL30_80( pOut );
						PIXEL31_61( pOut );
						PIXEL32_82( pOut );
						PIXEL33_82( pOut );
						break;
					}
				case 215:
					{
						PIXEL00_81( pOut );
						PIXEL01_31( pOut );
						PIXEL02_0( pOut );
						if( Diff( cmp2, 2, 6 ) )
						{
							PIXEL03_0( pOut );
						}
						else
						{
							PIXEL03_20( pOut );
						}
						PIXEL10_81( pOut );
						PIXEL11_31( pOut );
						PIXEL12_0( pOut );
						PIXEL13_0( pOut );
						PIXEL20_61( pOut );
						PIXEL21_30( pOut );
						PIXEL22_0( pOut );
						if( Diff( cmp2, 6, 8 ) )
						{
							PIXEL23_0( pOut );
							PIXEL32_0( pOut );
							PIXEL33_0( pOut );
						}
						else
						{
							PIXEL23_50( pOut );
							PIXEL32_50( pOut );
							PIXEL33_50( pOut );
						}
						PIXEL30_80( pOut );
						PIXEL31_10( pOut );
						break;
					}
				case 246:
					{
						PIXEL00_80( pOut );
						PIXEL01_10( pOut );
						if( Diff( cmp2, 2, 6 ) )
						{
							PIXEL02_0( pOut );
							PIXEL03_0( pOut );
							PIXEL13_0( pOut );
						}
						else
						{
							PIXEL02_50( pOut );
							PIXEL03_50( pOut );
							PIXEL13_50( pOut );
						}
						PIXEL10_61( pOut );
						PIXEL11_30( pOut );
						PIXEL12_0( pOut );
						PIXEL20_82( pOut );
						PIXEL21_32( pOut );
						PIXEL22_0( pOut );
						PIXEL23_0( pOut );
						PIXEL30_82( pOut );
						PIXEL31_32( pOut );
						PIXEL32_0( pOut );
						if( Diff( cmp2, 6, 8 ) )
						{
							PIXEL33_0( pOut );
						}
						else
						{
							PIXEL33_20( pOut );
						}
						break;
					}
				case 254:
					{
						PIXEL00_80( pOut );
						PIXEL01_10( pOut );
						if( Diff( cmp2, 2, 6 ) )
						{
							PIXEL02_0( pOut );
							PIXEL03_0( pOut );
							PIXEL13_0( pOut );
						}
						else
						{
							PIXEL02_50( pOut );
							PIXEL03_50( pOut );
							PIXEL13_50( pOut );
						}
						PIXEL10_10( pOut );
						PIXEL11_30( pOut );
						PIXEL12_0( pOut );
						if( Diff( cmp2, 8, 4 ) )
						{
							PIXEL20_0( pOut );
							PIXEL30_0( pOut );
							PIXEL31_0( pOut );
						}
						else
						{
							PIXEL20_50( pOut );
							PIXEL30_50( pOut );
							PIXEL31_50( pOut );
						}
						PIXEL21_0( pOut );
						PIXEL22_0( pOut );
						PIXEL23_0( pOut );
						PIXEL32_0( pOut );
						if( Diff( cmp2, 6, 8 ) )
						{
							PIXEL33_0( pOut );
						}
						else
						{
							PIXEL33_20( pOut );
						}
						break;
					}
				case 253:
					{
						PIXEL00_82( pOut );
						PIXEL01_82( pOut );
						PIXEL02_81( pOut );
						PIXEL03_81( pOut );
						PIXEL10_32( pOut );
						PIXEL11_32( pOut );
						PIXEL12_31( pOut );
						PIXEL13_31( pOut );
						PIXEL20_0( pOut );
						PIXEL21_0( pOut );
						PIXEL22_0( pOut );
						PIXEL23_0( pOut );
						if( Diff( cmp2, 8, 4 ) )
						{
							PIXEL30_0( pOut );
						}
						else
						{
							PIXEL30_20( pOut );
						}
						PIXEL31_0( pOut );
						PIXEL32_0( pOut );
						if( Diff( cmp2, 6, 8 ) )
						{
							PIXEL33_0( pOut );
						}
						else
						{
							PIXEL33_20( pOut );
						}
						break;
					}
				case 251:
					{
						if( Diff( cmp2, 4, 2 ) )
						{
							PIXEL00_0( pOut );
							PIXEL01_0( pOut );
							PIXEL10_0( pOut );
						}
						else
						{
							PIXEL00_50( pOut );
							PIXEL01_50( pOut );
							PIXEL10_50( pOut );
						}
						PIXEL02_10( pOut );
						PIXEL03_80( pOut );
						PIXEL11_0( pOut );
						PIXEL12_30( pOut );
						PIXEL13_10( pOut );
						PIXEL20_0( pOut );
						PIXEL21_0( pOut );
						PIXEL22_0( pOut );
						if( Diff( cmp2, 6, 8 ) )
						{
							PIXEL23_0( pOut );
							PIXEL32_0( pOut );
							PIXEL33_0( pOut );
						}
						else
						{
							PIXEL23_50( pOut );
							PIXEL32_50( pOut );
							PIXEL33_50( pOut );
						}
						if( Diff( cmp2, 8, 4 ) )
						{
							PIXEL30_0( pOut );
						}
						else
						{
							PIXEL30_20( pOut );
						}
						PIXEL31_0( pOut );
						break;
					}
				case 239:
					{
						if( Diff( cmp2, 4, 2 ) )
						{
							PIXEL00_0( pOut );
						}
						else
						{
							PIXEL00_20( pOut );
						}
						PIXEL01_0( pOut );
						PIXEL02_32( pOut );
						PIXEL03_82( pOut );
						PIXEL10_0( pOut );
						PIXEL11_0( pOut );
						PIXEL12_32( pOut );
						PIXEL13_82( pOut );
						PIXEL20_0( pOut );
						PIXEL21_0( pOut );
						PIXEL22_31( pOut );
						PIXEL23_81( pOut );
						if( Diff( cmp2, 8, 4 ) )
						{
							PIXEL30_0( pOut );
						}
						else
						{
							PIXEL30_20( pOut );
						}
						PIXEL31_0( pOut );
						PIXEL32_31( pOut );
						PIXEL33_81( pOut );
						break;
					}
				case 127:
					{
						if( Diff( cmp2, 4, 2 ) )
						{
							PIXEL00_0( pOut );
						}
						else
						{
							PIXEL00_20( pOut );
						}
						PIXEL01_0( pOut );
						if( Diff( cmp2, 2, 6 ) )
						{
							PIXEL02_0( pOut );
							PIXEL03_0( pOut );
							PIXEL13_0( pOut );
						}
						else
						{
							PIXEL02_50( pOut );
							PIXEL03_50( pOut );
							PIXEL13_50( pOut );
						}
						PIXEL10_0( pOut );
						PIXEL11_0( pOut );
						PIXEL12_0( pOut );
						if( Diff( cmp2, 8, 4 ) )
						{
							PIXEL20_0( pOut );
							PIXEL30_0( pOut );
							PIXEL31_0( pOut );
						}
						else
						{
							PIXEL20_50( pOut );
							PIXEL30_50( pOut );
							PIXEL31_50( pOut );
						}
						PIXEL21_0( pOut );
						PIXEL22_30( pOut );
						PIXEL23_10( pOut );
						PIXEL32_10( pOut );
						PIXEL33_80( pOut );
						break;
					}
				case 191:
					{
						if( Diff( cmp2, 4, 2 ) )
						{
							PIXEL00_0( pOut );
						}
						else
						{
							PIXEL00_20( pOut );
						}
						PIXEL01_0( pOut );
						PIXEL02_0( pOut );
						if( Diff( cmp2, 2, 6 ) )
						{
							PIXEL03_0( pOut );
						}
						else
						{
							PIXEL03_20( pOut );
						}
						PIXEL10_0( pOut );
						PIXEL11_0( pOut );
						PIXEL12_0( pOut );
						PIXEL13_0( pOut );
						PIXEL20_31( pOut );
						PIXEL21_31( pOut );
						PIXEL22_32( pOut );
						PIXEL23_32( pOut );
						PIXEL30_81( pOut );
						PIXEL31_81( pOut );
						PIXEL32_82( pOut );
						PIXEL33_82( pOut );
						break;
					}
				case 223:
					{
						if( Diff( cmp2, 4, 2 ) )
						{
							PIXEL00_0( pOut );
							PIXEL01_0( pOut );
							PIXEL10_0( pOut );
						}
						else
						{
							PIXEL00_50( pOut );
							PIXEL01_50( pOut );
							PIXEL10_50( pOut );
						}
						PIXEL02_0( pOut );
						if( Diff( cmp2, 2, 6 ) )
						{
							PIXEL03_0( pOut );
						}
						else
						{
							PIXEL03_20( pOut );
						}
						PIXEL11_0( pOut );
						PIXEL12_0( pOut );
						PIXEL13_0( pOut );
						PIXEL20_10( pOut );
						PIXEL21_30( pOut );
						PIXEL22_0( pOut );
						if( Diff( cmp2, 6, 8 ) )
						{
							PIXEL23_0( pOut );
							PIXEL32_0( pOut );
							PIXEL33_0( pOut );
						}
						else
						{
							PIXEL23_50( pOut );
							PIXEL32_50( pOut );
							PIXEL33_50( pOut );
						}
						PIXEL30_80( pOut );
						PIXEL31_10( pOut );
						break;
					}
				case 247:
					{
						PIXEL00_81( pOut );
						PIXEL01_31( pOut );
						PIXEL02_0( pOut );
						if( Diff( cmp2, 2, 6 ) )
						{
							PIXEL03_0( pOut );
						}
						else
						{
							PIXEL03_20( pOut );
						}
						PIXEL10_81( pOut );
						PIXEL11_31( pOut );
						PIXEL12_0( pOut );
						PIXEL13_0( pOut );
						PIXEL20_82( pOut );
						PIXEL21_32( pOut );
						PIXEL22_0( pOut );
						PIXEL23_0( pOut );
						PIXEL30_82( pOut );
						PIXEL31_32( pOut );
						PIXEL32_0( pOut );
						if( Diff( cmp2, 6, 8 ) )
						{
							PIXEL33_0( pOut );
						}
						else
						{
							PIXEL33_20( pOut );
						}
						break;
					}
				case 255:
					{
						if( Diff( cmp2, 4, 2 ) )
						{
							PIXEL00_0( pOut );
						}
						else
						{
							PIXEL00_20( pOut );
						}
						PIXEL01_0( pOut );
						PIXEL02_0( pOut );
						if( Diff( cmp2, 2, 6 ) )
						{
							PIXEL03_0( pOut );
						}
						else
						{
							PIXEL03_20( pOut );
						}
						PIXEL10_0( pOut );
						PIXEL11_0( pOut );
						PIXEL12_0( pOut );
						PIXEL13_0( pOut );
						PIXEL20_0( pOut );
						PIXEL21_0( pOut );
						PIXEL22_0( pOut );
						PIXEL23_0( pOut );
						if( Diff( cmp2, 8, 4 ) )
						{
							PIXEL30_0( pOut );
						}
						else
						{
							PIXEL30_20( pOut );
						}
						PIXEL31_0( pOut );
						PIXEL32_0( pOut );
						if( Diff( cmp2, 6, 8 ) )
						{
							PIXEL33_0( pOut );
						}
						else
						{
							PIXEL33_20( pOut );
						}
						break;
					}
				}
			}
			
			public static readonly FilterCaseDelegate FilterCase;
			static unsafe hq4x()
			{
				FilterCase = InternalFilterCase;
			}
		}
	}
}
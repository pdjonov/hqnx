/******************************************************************************
	hqNx - GPU-gased (XNA) implementation of hq2x, hq3x, and hq4x
	Copyright (C) 2009 Philip Djonov

	****
		This file contains functions *very* closely based on the demo
		implementation found in the original hqNx project, released under
		the GNU GPL 2.1 and originally copyrighted:
			Copyright (C) 2003 MaxSt ( maxst@hiend3d.com )
 
		http://www.hiend3d.com/hq2x.html
		http://www.hiend3d.com/hq3x.html
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
		private static unsafe void Interp0( float* pc, int t1 )
		{
			pc[t1] = 1;
		}

		private static unsafe void Interp1( float* pc, int t1, int t2 )
		{
			//*((int*)pc) = (c1*3+c2) >> 2;

			pc[t1] = 3;
			pc[t2] = 1;
		}

		private static unsafe void Interp2( float* pc, int t1, int t2, int t3 )
		{
			//*((int*)pc) = (c1*2+c2+c3) >> 2;

			pc[t1] = 2;
			pc[t2] = 1;
			pc[t3] = 1;
		}

		private static unsafe void Interp3( float* pc, int t1, int t2 )
		{
			//*((int*)pc) = (c1*7+c2)/8;

			pc[t1] = 7;
			pc[t2] = 1;
		}

		private static unsafe void Interp4( float* pc, int t1, int t2, int t3 )
		{
			//*((int*)pc) = (c1*2+(c2+c3)*7)/16;

			pc[t1] = 1;
			pc[t2] = 7;
			pc[t3] = 7;
		}

		private static unsafe void Interp5( float* pc, int t1, int t2 )
		{
			//*((int*)pc) = (c1 + c2) >> 1;

			pc[t1] = 1;
			pc[t2] = 1;
		}

		private static unsafe void Interp6( float* pc, int t1, int t2, int t3 )
		{
			//*((int*)pc) = (c1*5+c2*2+c3)/8;

			pc[t1] = 5;
			pc[t2] = 2;
			pc[t3] = 1;
		}

		private static unsafe void Interp7( float* pc, int t1, int t2, int t3 )
		{
			//*((int*)pc) = (c1*6+c2+c3)/8;

			pc[t1] = 6;
			pc[t2] = 1;
			pc[t3] = 1;
		}

		private static unsafe void Interp8( float* pc, int t1, int t2 )
		{
			//*((int*)pc) = (c1*5+c2*3)/8;

			pc[t1] = 5;
			pc[t2] = 3;
		}

		private static unsafe void Interp9( float* pc, int t1, int t2, int t3 )
		{
			//*((int*)pc) = (c1*2+(c2+c3)*3)/8;

			pc[t1] = 2;
			pc[t2] = 3;
			pc[t3] = 3;
		}

		private static unsafe void Interp10( float* pc, int t1, int t2, int t3 )
		{
			//*((int*)pc) = (c1*14+c2+c3)/16;

			pc[t1] = 14;
			pc[t2] = 1;
			pc[t3] = 1;
		}

		private static bool Diff( int cmp2, int t1, int t2 )
		{
			if( t1 > t2 )
			{
				int tmp = t1;
				t1 = t2;
				t2 = tmp;
			}

			int bit;
			if( t1 == 2 && t2 == 4 )
				bit = 0;
			else if( t1 == 2 && t2 == 6 )
				bit = 1;
			else if( t1 == 4 && t2 == 8 )
				bit = 2;
			else if( t1 == 6 && t2 == 8 )
				bit = 3;
			else throw new Exception();

			return (cmp2 & (1 << bit)) != 0;
		}

		private unsafe delegate void FilterCaseDelegate( float* pOut, int pattern, int cmp2 );

		private static unsafe void NormalizeKernel( float* pKern )
		{
			float sum = 0;
			
			for( int i = 1; i < 10; i++ )
				sum += pKern[i];

			if( sum == 0 )
			{
				pKern[5] = 1.0F;
				return;
			}

			float scalar = 1.0F / sum;

			for( int i = 1; i < 10; i++ )
				pKern[i] *= scalar;
		}

		private static unsafe void ConvertKernel( Color* pOut, float* pKern )
		{
			pOut[0] = new Color( pKern[1], pKern[2], pKern[3], pKern[4] );
			pOut[1] = new Color( pKern[6], pKern[7], pKern[8], pKern[9] );
		}

		/// <summary>
		/// 
		/// </summary>
		/// <remarks>
		/// This really, *really*, needs to be preprocessed into a loaded blob.
		/// </remarks>
		private unsafe void InitLutNx( Texture3D lut, int n, FilterCaseDelegate filterCase )
		{
			float[] lutbits = new float[10 * n * n * 16 * 256];
			Color[] lutValues = new Color[2 * n * n * 16 * 256];

			fixed( float *pLutBits = lutbits )
			fixed( Color *pLutValueBits = lutValues )
			{
				float *pLut = pLutBits;
				Color *pValues = pLutValueBits;

				for( int pattern = 0; pattern < 256; pattern++ )
				{
					for( int cmp2 = 0; cmp2 < 16; cmp2++ )
					{
						filterCase( pLut, pattern, cmp2 );

						for( int tap = 0; tap < n * n; tap++ )
						{
							NormalizeKernel( pLut );
							ConvertKernel( pValues, pLut );

							pLut += 10;
							pValues += 2;
						}
					}
				}
			}

			lut.SetData( lutValues );
		}

	}
}
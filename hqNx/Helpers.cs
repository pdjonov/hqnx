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

namespace hqNx
{

	internal static class Helpers
	{

		public static void DisposeAndNull<T>( ref T obj )
			where T : class
		{
			IDisposable disp = obj as IDisposable;

			obj = null;

			if( disp != null )
				disp.Dispose();
		}

		public static int RoundUpToPow2( int num )
		{
			for( int i = 31; i >= 0; i-- )
			{
				if( (num & (1 << i)) != 0 )
					return i << 1;
			}

			return 1;
		}

		public static bool IsPow2( int num )
		{
			return (num & (num - 1)) == 0;
		}

		public static byte FloatToByte( float num )
		{
			if( num <= 0 )
				return 0;
			if( num >= 1 )
				return 0xFF;

			return (byte)(num * 0xFF);
		}

	}

}
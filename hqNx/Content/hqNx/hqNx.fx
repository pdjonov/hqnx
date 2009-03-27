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

float2 ImageDims;
float2 Taps[9];

texture Lut2x, Lut3x, Lut4x;

sampler ImageSampler : register( s0 ) = sampler_state
{
	AddressU = Clamp;
	AddressV = Clamp;
	MinFilter = Point;
	MagFilter = Point;
	MipFilter = None;
};

sampler PatternSampler : register( s1 ) = sampler_state
{
	AddressU = Clamp;
	AddressV = Clamp;
	MinFilter = Point;
	MagFilter = Point;
	MipFilter = None;
};

sampler LutSampler : register( s2 ) = sampler_state
{
	AddressU = Clamp;
	AddressV = Clamp;
	AddressW = Clamp;
	MinFilter = Point;
	MagFilter = Point;
	MipFilter = None;
};

float4 quad_2d_vs(
	float2 iPos : POSITION,
	float2 iTc : TEXCOORD0,
	
	out float2 oTc : TEXCOORD0
	) : POSITION
{
	oTc = iTc;
	return float4( iPos, 0, 1 );
}

#if 1

/*
	This could probably be done right in RGB space without it
	being a *terrible* quality hit... It would certainly save
	a *ton* of math instructions.
	
	(see the #else)
*/

float3 rgb_to_yuv( float3 rgb )
{
	return float3(
		dot( rgb, 0.25 ),
		0.5 + (rgb.r - rgb.b) * 0.25,
		0.5 + dot( rgb, float3( -0.125, 0.25, -0.125 ) )
	);
}

float3 Threshold = float3( 0.1875, 0.02734375, 0.0234375 );

bool diff( float3 a, float3 b )
{
	float3 yuvA = rgb_to_yuv( a );
	float3 yuvB = rgb_to_yuv( b );
	
	float3 d = abs( yuvA - yuvB );
	return any( step( Threshold, d ) );
}

#else

float3 Threshold = float3( 0.2, 0.1, 0.2 );

bool diff( float3 a, float3 b )
{
	float3 d = abs( a - b );
	return any( step( Threshold, d ) );
}

#endif

float4 pattern_ps( float2 iTc : TEXCOORD0 ) : COLOR
{
	float3 t1 = tex2D( ImageSampler, iTc + Taps[0] );
	float3 t2 = tex2D( ImageSampler, iTc + Taps[1] );
	float3 t3 = tex2D( ImageSampler, iTc + Taps[2] );
	float3 t4 = tex2D( ImageSampler, iTc + Taps[3] );
	float3 t5 = tex2D( ImageSampler, iTc );
	float3 t6 = tex2D( ImageSampler, iTc + Taps[5] );
	float3 t7 = tex2D( ImageSampler, iTc + Taps[6] );
	float3 t8 = tex2D( ImageSampler, iTc + Taps[7] );
	float3 t9 = tex2D( ImageSampler, iTc + Taps[8] );

	float2 pattern = 0;

	//the main 8-bit pattern (the switch-case)
	if( diff( t1, t5 ) )
		pattern.y += 1;
	if( diff( t2, t5 ) )
		pattern.y += 2;
	if( diff( t3, t5 ) )
		pattern.y += 4;
	if( diff( t4, t5 ) )
		pattern.y += 8;
	if( diff( t6, t5 ) )
		pattern.y += 16;
	if( diff( t7, t5 ) )
		pattern.y += 32;
	if( diff( t8, t5 ) )
		pattern.y += 64;
	if( diff( t9, t5 ) )
		pattern.y += 128;

	//the four outer diagonals (ifs inside cases)
	if( diff( t2, t4 ) )
		pattern.x += 1;
	if( diff( t2, t6 ) )
		pattern.x += 2;
	if( diff( t4, t8 ) )
		pattern.x += 4;
	if( diff( t6, t8 ) )
		pattern.x += 8;

	//scale it into pixel space
	pattern /= float2( 15, 255 );

	return float4( pattern, 0, 0 );
}

float4 hqNx_ps( float2 iTc : TEXCOORD0, uniform const int N ) : COLOR
{
	float2 bigTc = iTc * ImageDims;
	
	//get the pixel coordinate
	float2 imgTc = (floor( bigTc ) + 0.5) / ImageDims;

	//and the sub-pixel coordinate
	float2 subTc = frac( bigTc );

	//linearize the sub-pixel coordinate
	float2 subTcStep = floor( subTc * N );
	float subCoord = (subTcStep.y * N + subTcStep.x) * 2;

	//fetch the pattern bits
	float2 pattern = tex2D( PatternSampler, imgTc );

	//fetch the color samples to blend
	float3 t1 = tex2D( ImageSampler, imgTc + Taps[0] );
	float3 t2 = tex2D( ImageSampler, imgTc + Taps[1] );
	float3 t3 = tex2D( ImageSampler, imgTc + Taps[2] );
	float3 t4 = tex2D( ImageSampler, imgTc + Taps[3] );
	float3 t5 = tex2D( ImageSampler, imgTc );
	float3 t6 = tex2D( ImageSampler, imgTc + Taps[5] );
	float3 t7 = tex2D( ImageSampler, imgTc + Taps[6] );
	float3 t8 = tex2D( ImageSampler, imgTc + Taps[7] );
	float3 t9 = tex2D( ImageSampler, imgTc + Taps[8] );

	/*
		Fetch the blend factors. Using the pattern values computed in the
		patterning pass, we can resolve both the switch-case and the embedded
		ifs by addressing the correct slice and row. Column values contain the
		nine blend weights, stored as pairs of 4-byte colors (the ninth weight
		is inferred as blend weights must sum to unity).
	*/

	float4 b1234 = tex3D( LutSampler, float3( (subCoord + 0.5) / (N * N * 2), pattern ) );
	float4 b6789 = tex3D( LutSampler, float3( (subCoord + 1.5) / (N * N * 2), pattern ) );
	float b5 = 1 - dot( b1234 + b6789, 1 );

	//blend
	float3 cl =
		t1 * b1234.x +
		t2 * b1234.y +
		t3 * b1234.z +
		t4 * b1234.w +
		t5 * b5 +
		t6 * b6789.x +
		t7 * b6789.y +
		t8 * b6789.z +
		t9 * b6789.w;

	return float4( cl, 1 );
}

technique hqNx
{
    pass ExtractPattern
    {
		AlphaBlendEnable = False;
		AlphaTestEnable = False;
		ZEnable = False;
    
        VertexShader = compile vs_1_1 quad_2d_vs();
        PixelShader = compile ps_2_a pattern_ps();
    }
    
    pass hq2x
    {
		AlphaBlendEnable = False;
		AlphaTestEnable = False;
		ZEnable = False;
    
		Texture[2] = <Lut2x>;
		
		VertexShader = compile vs_1_1 quad_2d_vs();
		PixelShader = compile ps_2_0 hqNx_ps( 2 );		
    }

	pass hq3x
    {
		AlphaBlendEnable = False;
		AlphaTestEnable = False;
		ZEnable = False;
    
		Texture[2] = <Lut3x>;
		
		VertexShader = compile vs_1_1 quad_2d_vs();
		PixelShader = compile ps_2_0 hqNx_ps( 3 );
    }
    
    pass hq4x
    {
		AlphaBlendEnable = False;
		AlphaTestEnable = False;
		ZEnable = False;
    
		Texture[2] = <Lut4x>;
		
		VertexShader = compile vs_1_1 quad_2d_vs();
		PixelShader = compile ps_2_0 hqNx_ps( 4 );
    }
}

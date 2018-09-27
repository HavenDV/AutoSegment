// ImageFormatConversions.cpp : Image format conversions
//

#include "stdafx.h"

#include "ImageProcessing.h"

#include <math.h>

//------------------------------------------------------------------------------

namespace ImageProcessing
{

	//--------------------------------------------------------------------------------
	// ### Image format conversion functions ###

	ERRORS IMAGEPROCESSING_API ConvertToGrayScale_8bpp(
		int width, int height,
		void* ptr1, int stride1,
		void* ptr2, int stride2)
	{
		// The input pixelformat must be one of the followings:
		// - 1bpp lineart
		// - 8bpp grayscale (palette is assumed to be linear grayscale - not yet checked)
		// - 16bpp grayscale
		// - 24bpp color
		// - 32bpp color with transparency info (REM: transparency is not taken into consideration)

		// The output pixelformat is always 8bpp, the memory must be allocated by the calling  environment

		// The conversion is not psychovisual (the components of input pixel vectors
		// are averaged with the same weight)
		BYTE LUT[] = {   0,   0,   0,   1,   1,   1,   2,   2,   2,   3,   3,   3,   4,   4,   4,   5,   5,   5,   6,   6,   6,   7,   7,   7,   8,   8,   8,   9,   9,   9,
						10,  10,  10,  11,  11,  11,  12,  12,  12,  13,  13,  13,  14,  14,  14,  15,  15,  15,  16,  16,  16,  17,  17,  17,  18,  18,  18,  19,  19,  19,
						20,  20,  20,  21,  21,  21,  22,  22,  22,  23,  23,  23,  24,  24,  24,  25,  25,  25,  26,  26,  26,  27,  27,  27,  28,  28,  28,  29,  29,  29,
						30,  30,  30,  31,  31,  31,  32,  32,  32,  33,  33,  33,  34,  34,  34,  35,  35,  35,  36,  36,  36,  37,  37,  37,  38,  38,  38,  39,  39,  39,
						40,  40,  40,  41,  41,  41,  42,  42,  42,  43,  43,  43,  44,  44,  44,  45,  45,  45,  46,  46,  46,  47,  47,  47,  48,  48,  48,  49,  49,  49,
						50,  50,  50,  51,  51,  51,  52,  52,  52,  53,  53,  53,  54,  54,  54,  55,  55,  55,  56,  56,  56,  57,  57,  57,  58,  58,  58,  59,  59,  59,
						60,  60,  60,  61,  61,  61,  62,  62,  62,  63,  63,  63,  64,  64,  64,  65,  65,  65,  66,  66,  66,  67,  67,  67,  68,  68,  68,  69,  69,  69,
						70,  70,  70,  71,  71,  71,  72,  72,  72,  73,  73,  73,  74,  74,  74,  75,  75,  75,  76,  76,  76,  77,  77,  77,  78,  78,  78,  79,  79,  79,
						80,  80,  80,  81,  81,  81,  82,  82,  82,  83,  83,  83,  84,  84,  84,  85,  85,  85,  86,  86,  86,  87,  87,  87,  88,  88,  88,  89,  89,  89,
						90,  90,  90,  91,  91,  91,  92,  92,  92,  93,  93,  93,  94,  94,  94,  95,  95,  95,  96,  96,  96,  97,  97,  97,  98,  98,  98,  99,  99,  99,
					   100, 100, 100, 101, 101, 101, 102, 102, 102, 103, 103, 103, 104, 104, 104, 105, 105, 105, 106, 106, 106, 107, 107, 107, 108, 108, 108, 109, 109, 109,
					   110, 110, 110, 111, 111, 111, 112, 112, 112, 113, 113, 113, 114, 114, 114, 115, 115, 115, 116, 116, 116, 117, 117, 117, 118, 118, 118, 119, 119, 119,
					   120, 120, 120, 121, 121, 121, 122, 122, 122, 123, 123, 123, 124, 124, 124, 125, 125, 125, 126, 126, 126, 127, 127, 127, 128, 128, 128, 129, 129, 129,
					   130, 130, 130, 131, 131, 131, 132, 132, 132, 133, 133, 133, 134, 134, 134, 135, 135, 135, 136, 136, 136, 137, 137, 137, 138, 138, 138, 139, 139, 139,
					   140, 140, 140, 141, 141, 141, 142, 142, 142, 143, 143, 143, 144, 144, 144, 145, 145, 145, 146, 146, 146, 147, 147, 147, 148, 148, 148, 149, 149, 149,
					   150, 150, 150, 151, 151, 151, 152, 152, 152, 153, 153, 153, 154, 154, 154, 155, 155, 155, 156, 156, 156, 157, 157, 157, 158, 158, 158, 159, 159, 159,
					   160, 160, 160, 161, 161, 161, 162, 162, 162, 163, 163, 163, 164, 164, 164, 165, 165, 165, 166, 166, 166, 167, 167, 167, 168, 168, 168, 169, 169, 169,
					   170, 170, 170, 171, 171, 171, 172, 172, 172, 173, 173, 173, 174, 174, 174, 175, 175, 175, 176, 176, 176, 177, 177, 177, 178, 178, 178, 179, 179, 179,
					   180, 180, 180, 181, 181, 181, 182, 182, 182, 183, 183, 183, 184, 184, 184, 185, 185, 185, 186, 186, 186, 187, 187, 187, 188, 188, 188, 189, 189, 189,
					   190, 190, 190, 191, 191, 191, 192, 192, 192, 193, 193, 193, 194, 194, 194, 195, 195, 195, 196, 196, 196, 197, 197, 197, 198, 198, 198, 199, 199, 199,
					   200, 200, 200, 201, 201, 201, 202, 202, 202, 203, 203, 203, 204, 204, 204, 205, 205, 205, 206, 206, 206, 207, 207, 207, 208, 208, 208, 209, 209, 209,
					   210, 210, 210, 211, 211, 211, 212, 212, 212, 213, 213, 213, 214, 214, 214, 215, 215, 215, 216, 216, 216, 217, 217, 217, 218, 218, 218, 219, 219, 219,
					   220, 220, 220, 221, 221, 221, 222, 222, 222, 223, 223, 223, 224, 224, 224, 225, 225, 225, 226, 226, 226, 227, 227, 227, 228, 228, 228, 229, 229, 229,
					   230, 230, 230, 231, 231, 231, 232, 232, 232, 233, 233, 233, 234, 234, 234, 235, 235, 235, 236, 236, 236, 237, 237, 237, 238, 238, 238, 239, 239, 239,
					   240, 240, 240, 241, 241, 241, 242, 242, 242, 243, 243, 243, 244, 244, 244, 245, 245, 245, 246, 246, 246, 247, 247, 247, 248, 248, 248, 249, 249, 249,
					   250, 250, 250, 251, 251, 251, 252, 252, 252, 253, 253, 253, 254, 254, 254, 255, 255, 255 };

		// 1. check the parameters
		if ( width<=0 || height <= 0 || stride2<width )
			return ERR_IMAGESIZE;			// image size error
		if ( ptr1==NULL || ptr2 == NULL)
			return ERR_IMAGEPOINTER;		// image pointer error

		memset(ptr2, 0, height*stride2);	// clear the output any way

		unsigned char* ptr1_0 = (unsigned char*)ptr1;
		unsigned char* ptr2_0 = (unsigned char*)ptr2;
		unsigned char* ptr1_c;
		unsigned char* ptr2_c;

		// 2. decide the dimension of input pixel vector
		int inputpixeldimension;
		if ( stride1>=5*width)
			return ERR_IMAGEFORMAT;		// image format error
		if (stride1 >= 4 * width)
			inputpixeldimension = 4;	// 32bpp (assumed:ARGB)
		else if (stride1 >= 3 * width)
			inputpixeldimension = 3;	// 24bpp (assumed: RGB)
		else if (stride1 >= 2 * width)
			inputpixeldimension = 2;	// 16bpp (assumed: grayscale)
		else if (stride1 >= width)
			inputpixeldimension = 1;	// 8bpp (assumed: grayscale with linear palette)
		else if (stride1 >= width / 8)
			inputpixeldimension = 0;	// 1bpp
		else
			return ERR_IMAGEFORMAT;		// image format error

		// 3. Expand the 1bpp lineart image to grayscale with {0,255} pixel values
		if (inputpixeldimension == 0)
		{
			int width1 = width/8;
			for (int jj = 0; jj < height; jj++)
			{
				ptr1_c = ptr1_0;
				ptr2_c = ptr2_0;
				for (int ii = 0; ii < width1; ii++)
				{
					BYTE data = *ptr1_c++;
					/*
					// replaced by wired logic - for speed up the conversion
					if (data & 128) ptr2_c[0] = 255;
					if (data & 64) ptr2_c[1] = 255;
					if (data & 32) ptr2_c[2] = 255;
					if (data & 16) ptr2_c[3] = 255;
					if (data & 8) ptr2_c[4] = 255;
					if (data & 4) ptr2_c[5] = 255;
					if (data & 2) ptr2_c[6] = 255;
					if (data & 1)  ptr2_c[7] = 255;*/
					switch (data)
					{
					case 0:
						break;
					case 1:
						ptr2_c[7] = 255; break;
					case 2:
						ptr2_c[6] = 255; break;
					case 3:
						ptr2_c[6] = ptr2_c[7] = 255; break;
					case 4:
						ptr2_c[5] = 255; break;
					case 5:
						ptr2_c[5] = ptr2_c[7] = 255; break;
					case 6:
						ptr2_c[5] = ptr2_c[6] = 255; break;
					case 7:
						ptr2_c[5] = ptr2_c[6] = ptr2_c[7] = 255; break;
					case 8:
						ptr2_c[4] = 255; break;
					case 9:
						ptr2_c[4] = ptr2_c[7] = 255; break;
					case 10:
						ptr2_c[4] = ptr2_c[6] = 255; break;
					case 11:
						ptr2_c[4] = ptr2_c[6] = ptr2_c[7] = 255; break;
					case 12:
						ptr2_c[4] = ptr2_c[5] = 255; break;
					case 13:
						ptr2_c[4] = ptr2_c[5] = ptr2_c[7] = 255; break;
					case 14:
						ptr2_c[4] = ptr2_c[5] = ptr2_c[6] = 255; break;
					case 15:
						ptr2_c[4] = ptr2_c[5] = ptr2_c[6] = ptr2_c[7] = 255; break;
					case 16:
						ptr2_c[3] = 255; break;
					case 17:
						ptr2_c[3] = ptr2_c[7] = 255; break;
					case 18:
						ptr2_c[3] = ptr2_c[6] = 255; break;
					case 19:
						ptr2_c[3] = ptr2_c[6] = ptr2_c[7] = 255; break;
					case 20:
						ptr2_c[3] = ptr2_c[5] = 255; break;
					case 21:
						ptr2_c[3] = ptr2_c[5] = ptr2_c[7] = 255; break;
					case 22:
						ptr2_c[3] = ptr2_c[5] = ptr2_c[6] = 255; break;
					case 23:
						ptr2_c[3] = ptr2_c[5] = ptr2_c[6] = ptr2_c[7] = 255; break;
					case 24:
						ptr2_c[3] = ptr2_c[4] = 255; break;
					case 25:
						ptr2_c[3] = ptr2_c[4] = ptr2_c[7] = 255; break;
					case 26:
						ptr2_c[3] = ptr2_c[4] = ptr2_c[6] = 255; break;
					case 27:
						ptr2_c[3] = ptr2_c[4] = ptr2_c[6] = ptr2_c[7] = 255; break;
					case 28:
						ptr2_c[3] = ptr2_c[4] = ptr2_c[5] = 255; break;
					case 29:
						ptr2_c[3] = ptr2_c[4] = ptr2_c[5] = ptr2_c[7] = 255; break;
					case 30:
						ptr2_c[3] = ptr2_c[4] = ptr2_c[5] = ptr2_c[6] = 255; break;
					case 31:
						ptr2_c[3] = ptr2_c[4] = ptr2_c[5] = ptr2_c[6] = ptr2_c[7] = 255; break;
					case 32:
						ptr2_c[2] = 255; break;
					case 33:
						ptr2_c[2] = ptr2_c[7] = 255; break;
					case 34:
						ptr2_c[2] = ptr2_c[6] = 255; break;
					case 35:
						ptr2_c[2] = ptr2_c[6] = ptr2_c[7] = 255; break;
					case 36:
						ptr2_c[2] = ptr2_c[5] = 255; break;
					case 37:
						ptr2_c[2] = ptr2_c[5] = ptr2_c[7] = 255; break;
					case 38:
						ptr2_c[2] = ptr2_c[5] = ptr2_c[6] = 255; break;
					case 39:
						ptr2_c[2] = ptr2_c[5] = ptr2_c[6] = ptr2_c[7] = 255; break;
					case 40:
						ptr2_c[2] = ptr2_c[4] = 255; break;
					case 41:
						ptr2_c[2] = ptr2_c[4] = ptr2_c[7] = 255; break;
					case 42:
						ptr2_c[2] = ptr2_c[4] = ptr2_c[6] = 255; break;
					case 43:
						ptr2_c[2] = ptr2_c[4] = ptr2_c[6] = ptr2_c[7] = 255; break;
					case 44:
						ptr2_c[2] = ptr2_c[4] = ptr2_c[5] = 255; break;
					case 45:
						ptr2_c[2] = ptr2_c[4] = ptr2_c[5] = ptr2_c[7] = 255; break;
					case 46:
						ptr2_c[2] = ptr2_c[4] = ptr2_c[5] = ptr2_c[6] = 255; break;
					case 47:
						ptr2_c[2] = ptr2_c[4] = ptr2_c[5] = ptr2_c[6] = ptr2_c[7] = 255; break;
					case 48:
						ptr2_c[2] = ptr2_c[3] = 255; break;
					case 49:
						ptr2_c[2] = ptr2_c[3] = ptr2_c[7] = 255; break;
					case 50:
						ptr2_c[2] = ptr2_c[3] = ptr2_c[6] = 255; break;
					case 51:
						ptr2_c[2] = ptr2_c[3] = ptr2_c[6] = ptr2_c[7] = 255; break;
					case 52:
						ptr2_c[2] = ptr2_c[3] = ptr2_c[5] = 255; break;
					case 53:
						ptr2_c[2] = ptr2_c[3] = ptr2_c[5] = ptr2_c[7] = 255; break;
					case 54:
						ptr2_c[2] = ptr2_c[3] = ptr2_c[5] = ptr2_c[6] = 255; break;
					case 55:
						ptr2_c[2] = ptr2_c[3] = ptr2_c[5] = ptr2_c[6] = ptr2_c[7] = 255; break;
					case 56:
						ptr2_c[2] = ptr2_c[3] = ptr2_c[4] = 255; break;
					case 57:
						ptr2_c[2] = ptr2_c[3] = ptr2_c[4] = ptr2_c[7] = 255; break;
					case 58:
						ptr2_c[2] = ptr2_c[3] = ptr2_c[4] = ptr2_c[6] = 255; break;
					case 59:
						ptr2_c[2] = ptr2_c[3] = ptr2_c[4] = ptr2_c[6] = ptr2_c[7] = 255; break;
					case 60:
						ptr2_c[2] = ptr2_c[3] = ptr2_c[4] = ptr2_c[5] = 255; break;
					case 61:
						ptr2_c[2] = ptr2_c[3] = ptr2_c[4] = ptr2_c[5] = ptr2_c[7] = 255; break;
					case 62:
						ptr2_c[2] = ptr2_c[3] = ptr2_c[4] = ptr2_c[5] = ptr2_c[6] = 255; break;
					case 63:
						ptr2_c[2] = ptr2_c[3] = ptr2_c[4] = ptr2_c[5] = ptr2_c[6] = ptr2_c[7] = 255; break;
					case 64:
						ptr2_c[1] = 255; break;
					case 65:
						ptr2_c[1] = ptr2_c[7] = 255; break;
					case 66:
						ptr2_c[1] = ptr2_c[6] = 255; break;
					case 67:
						ptr2_c[1] = ptr2_c[6] = ptr2_c[7] = 255; break;
					case 68:
						ptr2_c[1] = ptr2_c[5] = 255; break;
					case 69:
						ptr2_c[1] = ptr2_c[5] = ptr2_c[7] = 255; break;
					case 70:
						ptr2_c[1] = ptr2_c[5] = ptr2_c[6] = 255; break;
					case 71:
						ptr2_c[1] = ptr2_c[5] = ptr2_c[6] = ptr2_c[7] = 255; break;
					case 72:
						ptr2_c[1] = ptr2_c[4] = 255; break;
					case 73:
						ptr2_c[1] = ptr2_c[4] = ptr2_c[7] = 255; break;
					case 74:
						ptr2_c[1] = ptr2_c[4] = ptr2_c[6] = 255; break;
					case 75:
						ptr2_c[1] = ptr2_c[4] = ptr2_c[6] = ptr2_c[7] = 255; break;
					case 76:
						ptr2_c[1] = ptr2_c[4] = ptr2_c[5] = 255; break;
					case 77:
						ptr2_c[1] = ptr2_c[4] = ptr2_c[5] = ptr2_c[7] = 255; break;
					case 78:
						ptr2_c[1] = ptr2_c[4] = ptr2_c[5] = ptr2_c[6] = 255; break;
					case 79:
						ptr2_c[1] = ptr2_c[4] = ptr2_c[5] = ptr2_c[6] = ptr2_c[7] = 255; break;
					case 80:
						ptr2_c[1] = ptr2_c[3] = 255; break;
					case 81:
						ptr2_c[1] = ptr2_c[3] = ptr2_c[7] = 255; break;
					case 82:
						ptr2_c[1] = ptr2_c[3] = ptr2_c[6] = 255; break;
					case 83:
						ptr2_c[1] = ptr2_c[3] = ptr2_c[6] = ptr2_c[7] = 255; break;
					case 84:
						ptr2_c[1] = ptr2_c[3] = ptr2_c[5] = 255; break;
					case 85:
						ptr2_c[1] = ptr2_c[3] = ptr2_c[5] = ptr2_c[7] = 255; break;
					case 86:
						ptr2_c[1] = ptr2_c[3] = ptr2_c[5] = ptr2_c[6] = 255; break;
					case 87:
						ptr2_c[1] = ptr2_c[3] = ptr2_c[5] = ptr2_c[6] = ptr2_c[7] = 255; break;
					case 88:
						ptr2_c[1] = ptr2_c[3] = ptr2_c[4] = 255; break;
					case 89:
						ptr2_c[1] = ptr2_c[3] = ptr2_c[4] = ptr2_c[7] = 255; break;
					case 90:
						ptr2_c[1] = ptr2_c[3] = ptr2_c[4] = ptr2_c[6] = 255; break;
					case 91:
						ptr2_c[1] = ptr2_c[3] = ptr2_c[4] = ptr2_c[6] = ptr2_c[7] = 255; break;
					case 92:
						ptr2_c[1] = ptr2_c[3] = ptr2_c[4] = ptr2_c[5] = 255; break;
					case 93:
						ptr2_c[1] = ptr2_c[3] = ptr2_c[4] = ptr2_c[5] = ptr2_c[7] = 255; break;
					case 94:
						ptr2_c[1] = ptr2_c[3] = ptr2_c[4] = ptr2_c[5] = ptr2_c[6] = 255; break;
					case 95:
						ptr2_c[1] = ptr2_c[3] = ptr2_c[4] = ptr2_c[5] = ptr2_c[6] = ptr2_c[7] = 255; break;
					case 96:
						ptr2_c[1] = ptr2_c[2] = 255; break;
					case 97:
						ptr2_c[1] = ptr2_c[2] = ptr2_c[7] = 255; break;
					case 98:
						ptr2_c[1] = ptr2_c[2] = ptr2_c[6] = 255; break;
					case 99:
						ptr2_c[1] = ptr2_c[2] = ptr2_c[6] = ptr2_c[7] = 255; break;
					case 100:
						ptr2_c[1] = ptr2_c[2] = ptr2_c[5] = 255; break;
					case 101:
						ptr2_c[1] = ptr2_c[2] = ptr2_c[5] = ptr2_c[7] = 255; break;
					case 102:
						ptr2_c[1] = ptr2_c[2] = ptr2_c[5] = ptr2_c[6] = 255; break;
					case 103:
						ptr2_c[1] = ptr2_c[2] = ptr2_c[5] = ptr2_c[6] = ptr2_c[7] = 255; break;
					case 104:
						ptr2_c[1] = ptr2_c[2] = ptr2_c[4] = 255; break;
					case 105:
						ptr2_c[1] = ptr2_c[2] = ptr2_c[4] = ptr2_c[7] = 255; break;
					case 106:
						ptr2_c[1] = ptr2_c[2] = ptr2_c[4] = ptr2_c[6] = 255; break;
					case 107:
						ptr2_c[1] = ptr2_c[2] = ptr2_c[4] = ptr2_c[6] = ptr2_c[7] = 255; break;
					case 108:
						ptr2_c[1] = ptr2_c[2] = ptr2_c[4] = ptr2_c[5] = 255; break;
					case 109:
						ptr2_c[1] = ptr2_c[2] = ptr2_c[4] = ptr2_c[5] = ptr2_c[7] = 255; break;
					case 110:
						ptr2_c[1] = ptr2_c[2] = ptr2_c[4] = ptr2_c[5] = ptr2_c[6] = 255; break;
					case 111:
						ptr2_c[1] = ptr2_c[2] = ptr2_c[4] = ptr2_c[5] = ptr2_c[6] = ptr2_c[7] = 255; break;
					case 112:
						ptr2_c[1] = ptr2_c[2] = ptr2_c[3] = 255; break;
					case 113:
						ptr2_c[1] = ptr2_c[2] = ptr2_c[3] = ptr2_c[7] = 255; break;
					case 114:
						ptr2_c[1] = ptr2_c[2] = ptr2_c[3] = ptr2_c[6] = 255; break;
					case 115:
						ptr2_c[1] = ptr2_c[2] = ptr2_c[3] = ptr2_c[6] = ptr2_c[7] = 255; break;
					case 116:
						ptr2_c[1] = ptr2_c[2] = ptr2_c[3] = ptr2_c[5] = 255; break;
					case 117:
						ptr2_c[1] = ptr2_c[2] = ptr2_c[3] = ptr2_c[5] = ptr2_c[7] = 255; break;
					case 118:
						ptr2_c[1] = ptr2_c[2] = ptr2_c[3] = ptr2_c[5] = ptr2_c[6] = 255; break;
					case 119:
						ptr2_c[1] = ptr2_c[2] = ptr2_c[3] = ptr2_c[5] = ptr2_c[6] = ptr2_c[7] = 255; break;
					case 120:
						ptr2_c[1] = ptr2_c[2] = ptr2_c[3] = ptr2_c[4] = 255; break;
					case 121:
						ptr2_c[1] = ptr2_c[2] = ptr2_c[3] = ptr2_c[4] = ptr2_c[7] = 255; break;
					case 122:
						ptr2_c[1] = ptr2_c[2] = ptr2_c[3] = ptr2_c[4] = ptr2_c[6] = 255; break;
					case 123:
						ptr2_c[1] = ptr2_c[2] = ptr2_c[3] = ptr2_c[4] = ptr2_c[6] = ptr2_c[7] = 255; break;
					case 124:
						ptr2_c[1] = ptr2_c[2] = ptr2_c[3] = ptr2_c[4] = ptr2_c[5] = 255; break;
					case 125:
						ptr2_c[1] = ptr2_c[2] = ptr2_c[3] = ptr2_c[4] = ptr2_c[5] = ptr2_c[7] = 255; break;
					case 126:
						ptr2_c[1] = ptr2_c[2] = ptr2_c[3] = ptr2_c[4] = ptr2_c[5] = ptr2_c[6] = 255; break;
					case 127:
						ptr2_c[1] = ptr2_c[2] = ptr2_c[3] = ptr2_c[4] = ptr2_c[5] = ptr2_c[6] = ptr2_c[7] = 255; break;
					case 128:
						ptr2_c[0] = 255; break;
					case 129:
						ptr2_c[0] = ptr2_c[7] = 255; break;
					case 130:
						ptr2_c[0] = ptr2_c[6] = 255; break;
					case 131:
						ptr2_c[0] = ptr2_c[6] = ptr2_c[7] = 255; break;
					case 132:
						ptr2_c[0] = ptr2_c[5] = 255; break;
					case 133:
						ptr2_c[0] = ptr2_c[5] = ptr2_c[7] = 255; break;
					case 134:
						ptr2_c[0] = ptr2_c[5] = ptr2_c[6] = 255; break;
					case 135:
						ptr2_c[0] = ptr2_c[5] = ptr2_c[6] = ptr2_c[7] = 255; break;
					case 136:
						ptr2_c[0] = ptr2_c[4] = 255; break;
					case 137:
						ptr2_c[0] = ptr2_c[4] = ptr2_c[7] = 255; break;
					case 138:
						ptr2_c[0] = ptr2_c[4] = ptr2_c[6] = 255; break;
					case 139:
						ptr2_c[0] = ptr2_c[4] = ptr2_c[6] = ptr2_c[7] = 255; break;
					case 140:
						ptr2_c[0] = ptr2_c[4] = ptr2_c[5] = 255; break;
					case 141:
						ptr2_c[0] = ptr2_c[4] = ptr2_c[5] = ptr2_c[7] = 255; break;
					case 142:
						ptr2_c[0] = ptr2_c[4] = ptr2_c[5] = ptr2_c[6] = 255; break;
					case 143:
						ptr2_c[0] = ptr2_c[4] = ptr2_c[5] = ptr2_c[6] = ptr2_c[7] = 255; break;
					case 144:
						ptr2_c[0] = ptr2_c[3] = 255; break;
					case 145:
						ptr2_c[0] = ptr2_c[3] = ptr2_c[7] = 255; break;
					case 146:
						ptr2_c[0] = ptr2_c[3] = ptr2_c[6] = 255; break;
					case 147:
						ptr2_c[0] = ptr2_c[3] = ptr2_c[6] = ptr2_c[7] = 255; break;
					case 148:
						ptr2_c[0] = ptr2_c[3] = ptr2_c[5] = 255; break;
					case 149:
						ptr2_c[0] = ptr2_c[3] = ptr2_c[5] = ptr2_c[7] = 255; break;
					case 150:
						ptr2_c[0] = ptr2_c[3] = ptr2_c[5] = ptr2_c[6] = 255; break;
					case 151:
						ptr2_c[0] = ptr2_c[3] = ptr2_c[5] = ptr2_c[6] = ptr2_c[7] = 255; break;
					case 152:
						ptr2_c[0] = ptr2_c[3] = ptr2_c[4] = 255; break;
					case 153:
						ptr2_c[0] = ptr2_c[3] = ptr2_c[4] = ptr2_c[7] = 255; break;
					case 154:
						ptr2_c[0] = ptr2_c[3] = ptr2_c[4] = ptr2_c[6] = 255; break;
					case 155:
						ptr2_c[0] = ptr2_c[3] = ptr2_c[4] = ptr2_c[6] = ptr2_c[7] = 255; break;
					case 156:
						ptr2_c[0] = ptr2_c[3] = ptr2_c[4] = ptr2_c[5] = 255; break;
					case 157:
						ptr2_c[0] = ptr2_c[3] = ptr2_c[4] = ptr2_c[5] = ptr2_c[7] = 255; break;
					case 158:
						ptr2_c[0] = ptr2_c[3] = ptr2_c[4] = ptr2_c[5] = ptr2_c[6] = 255; break;
					case 159:
						ptr2_c[0] = ptr2_c[3] = ptr2_c[4] = ptr2_c[5] = ptr2_c[6] = ptr2_c[7] = 255; break;
					case 160:
						ptr2_c[0] = ptr2_c[2] = 255; break;
					case 161:
						ptr2_c[0] = ptr2_c[2] = ptr2_c[7] = 255; break;
					case 162:
						ptr2_c[0] = ptr2_c[2] = ptr2_c[6] = 255; break;
					case 163:
						ptr2_c[0] = ptr2_c[2] = ptr2_c[6] = ptr2_c[7] = 255; break;
					case 164:
						ptr2_c[0] = ptr2_c[2] = ptr2_c[5] = 255; break;
					case 165:
						ptr2_c[0] = ptr2_c[2] = ptr2_c[5] = ptr2_c[7] = 255; break;
					case 166:
						ptr2_c[0] = ptr2_c[2] = ptr2_c[5] = ptr2_c[6] = 255; break;
					case 167:
						ptr2_c[0] = ptr2_c[2] = ptr2_c[5] = ptr2_c[6] = ptr2_c[7] = 255; break;
					case 168:
						ptr2_c[0] = ptr2_c[2] = ptr2_c[4] = 255; break;
					case 169:
						ptr2_c[0] = ptr2_c[2] = ptr2_c[4] = ptr2_c[7] = 255; break;
					case 170:
						ptr2_c[0] = ptr2_c[2] = ptr2_c[4] = ptr2_c[6] = 255; break;
					case 171:
						ptr2_c[0] = ptr2_c[2] = ptr2_c[4] = ptr2_c[6] = ptr2_c[7] = 255; break;
					case 172:
						ptr2_c[0] = ptr2_c[2] = ptr2_c[4] = ptr2_c[5] = 255; break;
					case 173:
						ptr2_c[0] = ptr2_c[2] = ptr2_c[4] = ptr2_c[5] = ptr2_c[7] = 255; break;
					case 174:
						ptr2_c[0] = ptr2_c[2] = ptr2_c[4] = ptr2_c[5] = ptr2_c[6] = 255; break;
					case 175:
						ptr2_c[0] = ptr2_c[2] = ptr2_c[4] = ptr2_c[5] = ptr2_c[6] = ptr2_c[7] = 255; break;
					case 176:
						ptr2_c[0] = ptr2_c[2] = ptr2_c[3] = 255; break;
					case 177:
						ptr2_c[0] = ptr2_c[2] = ptr2_c[3] = ptr2_c[7] = 255; break;
					case 178:
						ptr2_c[0] = ptr2_c[2] = ptr2_c[3] = ptr2_c[6] = 255; break;
					case 179:
						ptr2_c[0] = ptr2_c[2] = ptr2_c[3] = ptr2_c[6] = ptr2_c[7] = 255; break;
					case 180:
						ptr2_c[0] = ptr2_c[2] = ptr2_c[3] = ptr2_c[5] = 255; break;
					case 181:
						ptr2_c[0] = ptr2_c[2] = ptr2_c[3] = ptr2_c[5] = ptr2_c[7] = 255; break;
					case 182:
						ptr2_c[0] = ptr2_c[2] = ptr2_c[3] = ptr2_c[5] = ptr2_c[6] = 255; break;
					case 183:
						ptr2_c[0] = ptr2_c[2] = ptr2_c[3] = ptr2_c[5] = ptr2_c[6] = ptr2_c[7] = 255; break;
					case 184:
						ptr2_c[0] = ptr2_c[2] = ptr2_c[3] = ptr2_c[4] = 255; break;
					case 185:
						ptr2_c[0] = ptr2_c[2] = ptr2_c[3] = ptr2_c[4] = ptr2_c[7] = 255; break;
					case 186:
						ptr2_c[0] = ptr2_c[2] = ptr2_c[3] = ptr2_c[4] = ptr2_c[6] = 255; break;
					case 187:
						ptr2_c[0] = ptr2_c[2] = ptr2_c[3] = ptr2_c[4] = ptr2_c[6] = ptr2_c[7] = 255; break;
					case 188:
						ptr2_c[0] = ptr2_c[2] = ptr2_c[3] = ptr2_c[4] = ptr2_c[5] = 255; break;
					case 189:
						ptr2_c[0] = ptr2_c[2] = ptr2_c[3] = ptr2_c[4] = ptr2_c[5] = ptr2_c[7] = 255; break;
					case 190:
						ptr2_c[0] = ptr2_c[2] = ptr2_c[3] = ptr2_c[4] = ptr2_c[5] = ptr2_c[6] = 255; break;
					case 191:
						ptr2_c[0] = ptr2_c[2] = ptr2_c[3] = ptr2_c[4] = ptr2_c[5] = ptr2_c[6] = ptr2_c[7] = 255; break;
					case 192:
						ptr2_c[0] = ptr2_c[1] = 255; break;
					case 193:
						ptr2_c[0] = ptr2_c[1] = ptr2_c[7] = 255; break;
					case 194:
						ptr2_c[0] = ptr2_c[1] = ptr2_c[6] = 255; break;
					case 195:
						ptr2_c[0] = ptr2_c[1] = ptr2_c[6] = ptr2_c[7] = 255; break;
					case 196:
						ptr2_c[0] = ptr2_c[1] = ptr2_c[5] = 255; break;
					case 197:
						ptr2_c[0] = ptr2_c[1] = ptr2_c[5] = ptr2_c[7] = 255; break;
					case 198:
						ptr2_c[0] = ptr2_c[1] = ptr2_c[5] = ptr2_c[6] = 255; break;
					case 199:
						ptr2_c[0] = ptr2_c[1] = ptr2_c[5] = ptr2_c[6] = ptr2_c[7] = 255; break;
					case 200:
						ptr2_c[0] = ptr2_c[1] = ptr2_c[4] = 255; break;
					case 201:
						ptr2_c[0] = ptr2_c[1] = ptr2_c[4] = ptr2_c[7] = 255; break;
					case 202:
						ptr2_c[0] = ptr2_c[1] = ptr2_c[4] = ptr2_c[6] = 255; break;
					case 203:
						ptr2_c[0] = ptr2_c[1] = ptr2_c[4] = ptr2_c[6] = ptr2_c[7] = 255; break;
					case 204:
						ptr2_c[0] = ptr2_c[1] = ptr2_c[4] = ptr2_c[5] = 255; break;
					case 205:
						ptr2_c[0] = ptr2_c[1] = ptr2_c[4] = ptr2_c[5] = ptr2_c[7] = 255; break;
					case 206:
						ptr2_c[0] = ptr2_c[1] = ptr2_c[4] = ptr2_c[5] = ptr2_c[6] = 255; break;
					case 207:
						ptr2_c[0] = ptr2_c[1] = ptr2_c[4] = ptr2_c[5] = ptr2_c[6] = ptr2_c[7] = 255; break;
					case 208:
						ptr2_c[0] = ptr2_c[1] = ptr2_c[3] = 255; break;
					case 209:
						ptr2_c[0] = ptr2_c[1] = ptr2_c[3] = ptr2_c[7] = 255; break;
					case 210:
						ptr2_c[0] = ptr2_c[1] = ptr2_c[3] = ptr2_c[6] = 255; break;
					case 211:
						ptr2_c[0] = ptr2_c[1] = ptr2_c[3] = ptr2_c[6] = ptr2_c[7] = 255; break;
					case 212:
						ptr2_c[0] = ptr2_c[1] = ptr2_c[3] = ptr2_c[5] = 255; break;
					case 213:
						ptr2_c[0] = ptr2_c[1] = ptr2_c[5] = ptr2_c[7] = 255; break;
					case 214:
						ptr2_c[0] = ptr2_c[1] = ptr2_c[3] = ptr2_c[5] = ptr2_c[6] = 255; break;
					case 215:
						ptr2_c[0] = ptr2_c[1] = ptr2_c[3] = ptr2_c[5] = ptr2_c[6] = ptr2_c[7] = 255; break;
					case 216:
						ptr2_c[0] = ptr2_c[1] = ptr2_c[3] = ptr2_c[4] = 255; break;
					case 217:
						ptr2_c[0] = ptr2_c[1] = ptr2_c[3] = ptr2_c[4] = ptr2_c[7] = 255; break;
					case 218:
						ptr2_c[0] = ptr2_c[1] = ptr2_c[3] = ptr2_c[4] = ptr2_c[6] = 255; break;
					case 219:
						ptr2_c[0] = ptr2_c[1] = ptr2_c[3] = ptr2_c[4] = ptr2_c[6] = ptr2_c[7] = 255; break;
					case 220:
						ptr2_c[0] = ptr2_c[1] = ptr2_c[3] = ptr2_c[4] = ptr2_c[5] = 255; break;
					case 221:
						ptr2_c[0] = ptr2_c[1] = ptr2_c[3] = ptr2_c[4] = ptr2_c[5] = ptr2_c[7] = 255; break;
					case 222:
						ptr2_c[0] = ptr2_c[1] = ptr2_c[3] = ptr2_c[4] = ptr2_c[5] = ptr2_c[6] = 255; break;
					case 223:
						ptr2_c[0] = ptr2_c[1] = ptr2_c[3] = ptr2_c[4] = ptr2_c[5] = ptr2_c[6] = ptr2_c[7] = 255; break;
					case 224:
						ptr2_c[0] = ptr2_c[1] = ptr2_c[2] = 255; break;
					case 225:
						ptr2_c[0] = ptr2_c[1] = ptr2_c[2] = ptr2_c[7] = 255; break;
					case 226:
						ptr2_c[0] = ptr2_c[1] = ptr2_c[2] = ptr2_c[6] = 255; break;
					case 227:
						ptr2_c[0] = ptr2_c[1] = ptr2_c[2] = ptr2_c[6] = ptr2_c[7] = 255; break;
					case 228:
						ptr2_c[0] = ptr2_c[1] = ptr2_c[2] = ptr2_c[5] = 255; break;
					case 229:
						ptr2_c[0] = ptr2_c[1] = ptr2_c[2] = ptr2_c[5] = ptr2_c[7] = 255; break;
					case 230:
						ptr2_c[0] = ptr2_c[1] = ptr2_c[2] = ptr2_c[5] = ptr2_c[6] = 255; break;
					case 231:
						ptr2_c[0] = ptr2_c[1] = ptr2_c[2] = ptr2_c[5] = ptr2_c[6] = ptr2_c[7] = 255; break;
					case 232:
						ptr2_c[0] = ptr2_c[1] = ptr2_c[2] = ptr2_c[4] = 255; break;
					case 233:
						ptr2_c[0] = ptr2_c[1] = ptr2_c[2] = ptr2_c[4] = ptr2_c[7] = 255; break;
					case 234:
						ptr2_c[0] = ptr2_c[1] = ptr2_c[2] = ptr2_c[4] = ptr2_c[6] = 255; break;
					case 235:
						ptr2_c[0] = ptr2_c[1] = ptr2_c[2] = ptr2_c[4] = ptr2_c[6] = ptr2_c[7] = 255; break;
					case 236:
						ptr2_c[0] = ptr2_c[1] = ptr2_c[2] = ptr2_c[4] = ptr2_c[5] = 255; break;
					case 237:
						ptr2_c[0] = ptr2_c[1] = ptr2_c[2] = ptr2_c[4] = ptr2_c[5] = ptr2_c[7] = 255; break;
					case 238:
						ptr2_c[0] = ptr2_c[1] = ptr2_c[2] = ptr2_c[4] = ptr2_c[5] = ptr2_c[6] = 255; break;
					case 239:
						ptr2_c[0] = ptr2_c[1] = ptr2_c[2] = ptr2_c[4] = ptr2_c[5] = ptr2_c[6] = ptr2_c[7] = 255; break;
					case 240:
						ptr2_c[0] = ptr2_c[1] = ptr2_c[2] = ptr2_c[3] = 255; break;
					case 241:
						ptr2_c[0] = ptr2_c[1] = ptr2_c[2] = ptr2_c[3] = ptr2_c[7] = 255; break;
					case 242:
						ptr2_c[0] = ptr2_c[1] = ptr2_c[2] = ptr2_c[3] = ptr2_c[6] = 255; break;
					case 243:
						ptr2_c[0] = ptr2_c[1] = ptr2_c[2] = ptr2_c[3] = ptr2_c[6] = ptr2_c[7] = 255; break;
					case 244:
						ptr2_c[0] = ptr2_c[1] = ptr2_c[2] = ptr2_c[3] = ptr2_c[5] = 255; break;
					case 245:
						ptr2_c[0] = ptr2_c[1] = ptr2_c[2] = ptr2_c[3] = ptr2_c[5] = ptr2_c[7] = 255; break;
					case 246:
						ptr2_c[0] = ptr2_c[1] = ptr2_c[2] = ptr2_c[3] = ptr2_c[5] = ptr2_c[6] = 255; break;
					case 247:
						ptr2_c[0] = ptr2_c[1] = ptr2_c[2] = ptr2_c[3] = ptr2_c[5] = ptr2_c[6] = ptr2_c[7] = 255; break;
					case 248:
						ptr2_c[0] = ptr2_c[1] = ptr2_c[2] = ptr2_c[3] = ptr2_c[4] = 255; break;
					case 249:
						ptr2_c[0] = ptr2_c[1] = ptr2_c[2] = ptr2_c[3] = ptr2_c[4] = ptr2_c[7] = 255; break;
					case 250:
						ptr2_c[0] = ptr2_c[1] = ptr2_c[2] = ptr2_c[3] = ptr2_c[4] = ptr2_c[6] = 255; break;
					case 251:
						ptr2_c[0] = ptr2_c[1] = ptr2_c[2] = ptr2_c[3] = ptr2_c[4] = ptr2_c[6] = ptr2_c[7] = 255; break;
					case 252:
						ptr2_c[0] = ptr2_c[1] = ptr2_c[2] = ptr2_c[3] = ptr2_c[4] = ptr2_c[5] = 255; break;
					case 253:
						ptr2_c[0] = ptr2_c[1] = ptr2_c[2] = ptr2_c[3] = ptr2_c[4] = ptr2_c[5] = ptr2_c[7] = 255; break;
					case 254:
						ptr2_c[0] = ptr2_c[1] = ptr2_c[2] = ptr2_c[3] = ptr2_c[4] = ptr2_c[5] = ptr2_c[6] = 255; break;
					case 255:
						ptr2_c[0] = ptr2_c[1] = ptr2_c[2] = ptr2_c[3] = ptr2_c[4] = ptr2_c[5] = ptr2_c[6] = ptr2_c[7] = 255; break;
					}
					ptr2_c += 8;
				}
				ptr1_0 += stride1;
				ptr2_0 += stride2;
			}
			return ERRNO;
		}

		// 4. Apply special (fast) solution, if both image is 8bpp 
		if ( inputpixeldimension==1)
		{
			if ( stride1==stride2)
			{
				// (...also their stride is equal to each other)
				memcpy(ptr2_0, ptr1_0, height*stride1);
				return ERRNO;
			} else
			{
				// (their stride values are different)
				for ( int jj=0; jj<height; jj++)
				{
					memcpy(ptr2_0, ptr1_0, width);
					ptr1_0 += stride1;
					ptr2_0 += stride2;
				}
				return ERRNO;
			}
		}

		// 5. create the grayscale output image (as averaging the components of
		//    input pixel value components)
		//    REM: no physchovisual averaging applied
		for ( int jj=0; jj<height; jj++)
		{
			ptr1_c = ptr1_0;
			ptr2_c = ptr2_0;
			if ( inputpixeldimension==2 )
			{
				// 16bpp grayscale
				for ( int ii=0; ii<width; ii++)
				{
					unsigned int value = (unsigned int)*ptr1_c++;
					value += ((unsigned int)*ptr1_c++)<<8;
					value >>=8;	// downscaling: {0...65535} >>> {0...255}
					*ptr2_c++ = (unsigned char)value;
				}

			} else if ( inputpixeldimension==3 )
			{
				// 24bpp RGB
				// REM: no psychovisual weighting)
				for ( int ii=0; ii<width; ii++)
				{
					unsigned short sum = 0;
					unsigned char average = 0;
					sum += (unsigned short)*ptr1_c++;
					sum += (unsigned short)*ptr1_c++;
					sum += (unsigned short)*ptr1_c++;
					average = LUT[sum];	// (unsigned char)(sum / 3);
					*ptr2_c++ = average;
				}

			} else if ( inputpixeldimension==4 )
			{
				// 32bpp ARGB
				// REM: no psychovisual weighting)
				for ( int ii=0; ii<width; ii++)
				{
					unsigned short sum = 0;
					unsigned char average = 0;
					sum += (unsigned short)*ptr1_c++;
					sum += (unsigned short)*ptr1_c++;
					sum += (unsigned short)*ptr1_c++;
					average = LUT[sum];	// (unsigned char)(sum/3);
					*ptr2_c++ = average;
					ptr1_c++;	// transparency info not used
				}
			} else
				return ERR_IMAGEFORMAT;		// image format error

			ptr1_0 += stride1;
			ptr2_0 += stride2;
		} 
		return ERRNO;
	}


	ERRORS IMAGEPROCESSING_API ConvertToRGB_24bpp(
		int width, int height,
		void* ptr1, int stride1,
		void* ptr2, int stride2)
	{
		// The input pixelformat must be one of the followings:
		// - 1bpp lineart
		// - 8bpp grayscale (palette is assumed to be linear grayscale - not yet checked)
		// - 16bpp grayscale
		// - 24bpp color
		// - 32bpp color with transparency info (REM: transparency is not taken into consideration)

		// The output pixelformat is always 24bpp, the memory must be allocated by the calling  environment

		// 1. check the parameters
		if (width <= 0 || height <= 0 || stride2<3*width)
			return ERR_IMAGESIZE;			// image size error
		if (ptr1 == NULL || ptr2 == NULL)
			return ERR_IMAGEPOINTER;		// image pointer error

		memset(ptr2, 0, height*stride2);	// clear the output any way

		unsigned char* ptr1_0 = (unsigned char*)ptr1;
		unsigned char* ptr2_0 = (unsigned char*)ptr2;
		unsigned char* ptr1_c;
		unsigned char* ptr2_c;

		// 2. decide the dimension of input pixel vector
		int inputpixeldimension;
		if (stride1 >= 5 * width)
			return ERR_IMAGEFORMAT;		// image format error
		if (stride1 >= 4 * width)
			inputpixeldimension = 4;	// 32bpp (assumed:ARGB)
		else if (stride1 >= 3 * width)
			inputpixeldimension = 3;	// 24bpp (assumed: RGB)
		else if (stride1 >= 2 * width)
			inputpixeldimension = 2;	// 16bpp (assumed: grayscale)
		else if (stride1 >= width)
			inputpixeldimension = 1;	// 8bpp (assumed: grayscale with linear palette)
		else if (stride1 >= width / 8)
			inputpixeldimension = 0;	// 1bpp
		else
			return ERR_IMAGEFORMAT;		// image format error

		// 3. execute the conversion
		if (inputpixeldimension == 0)
		{
			// 1bpp lineart
			int width1 = width / 8;
			for (int jj = 0; jj < height; jj++)
			{
				ptr1_c = ptr1_0;
				ptr2_c = ptr2_0;
				for (int ii = 0; ii < width1; ii++)
				{
					BYTE data = *ptr1_c++;
					// replaced by wired logic - for speed up the conversion
					if (data & 128) { ptr2_c[0] = 255;  ptr2_c[1] = 255;  ptr2_c[2] = 255; }
					if (data & 64)  { ptr2_c[3] = 255;  ptr2_c[4] = 255;  ptr2_c[5] = 255; }
					if (data & 32)  { ptr2_c[6] = 255;  ptr2_c[7] = 255;  ptr2_c[8] = 255; }
					if (data & 16)  { ptr2_c[9] = 255;  ptr2_c[10] = 255;  ptr2_c[11] = 255; }
					if (data & 8)   { ptr2_c[12] = 255; ptr2_c[13] = 255;  ptr2_c[14] = 255; }
					if (data & 4)   { ptr2_c[15] = 255;  ptr2_c[16] = 255;  ptr2_c[17] = 255; }
					if (data & 2)   { ptr2_c[18] = 255;  ptr2_c[19] = 255;  ptr2_c[20] = 255; }
					if (data & 1)   { ptr2_c[21] = 255;  ptr2_c[22] = 255;  ptr2_c[23] = 255; }
					ptr2_c += 24;
				}
				ptr1_0 += stride1;
				ptr2_0 += stride2;
			}
			return ERRNO;
		}

		if (inputpixeldimension == 1)
		{
			// 8bpp grayscale
			for (int jj = 0; jj < height; jj++)
			{
				ptr1_c = ptr1_0;
				ptr2_c = ptr2_0;
				for (int ii = 0; ii < width; ii++)
				{
					BYTE value = *ptr1_c++;
					*ptr2_c++ = value;
					*ptr2_c++ = value;
					*ptr2_c++ = value;
				}
				ptr1_0 += stride1;
				ptr2_0 += stride2;
			}
			return ERRNO;
		}

		if (inputpixeldimension == 2)
		{
			// 16bpp grayscale
			for (int jj = 0; jj < height; jj++)
			{
				ptr1_c = ptr1_0;
				ptr2_c = ptr2_0;
				for (int ii = 0; ii < width; ii++)
				{
					unsigned int value = (unsigned int)*ptr1_c++;
					value += ((unsigned int)*ptr1_c++) << 8;
					value >>= 8;	// downscaling: {0...65535} >>> {0...255}
					*ptr2_c++ = (unsigned char)value;
					*ptr2_c++ = (unsigned char)value;
					*ptr2_c++ = (unsigned char)value;
				}
				ptr1_0 += stride1;
				ptr2_0 += stride2;
			}
			return ERRNO;
		}

		if (inputpixeldimension == 3)
		{
			// 24bpp RGB
			for (int jj = 0; jj < height; jj++)
			{
				ptr1_c = ptr1_0;
				ptr2_c = ptr2_0;
				for (int ii = 0; ii < width; ii++)
				{
					BYTE value = *ptr1_c++;
					*ptr2_c++ = value;
					value = *ptr1_c++;
					*ptr2_c++ = value;
					value = *ptr1_c++;
					*ptr2_c++ = value;
				}
				ptr1_0 += stride1;
				ptr2_0 += stride2;
			}
			return ERRNO;
		}

		if (inputpixeldimension == 4)
		{
			// 32bpp ARGB
			for (int jj = 0; jj < height; jj++)
			{
				ptr1_c = ptr1_0;
				ptr2_c = ptr2_0;
				for (int ii = 0; ii < width; ii++)
				{
					BYTE value = *ptr1_c++;
					*ptr2_c++ = value;
					value = *ptr1_c++;
					*ptr2_c++ = value;
					value = *ptr1_c++;
					*ptr2_c++ = value;
					ptr1_c++;

				}
				ptr1_0 += stride1;
				ptr2_0 += stride2;
			}
			return ERRNO;
		}

		return ERR_IMAGEFORMAT;		// image format error
	}


}



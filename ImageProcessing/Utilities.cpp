// Utilities.cpp : Misc. bitmap-related utilities
//

#include "stdafx.h"

#include "ImageProcessing.h"

#include <math.h>

//------------------------------------------------------------------------------

namespace ImageProcessing
{

	//--------------------------------------------------------------------------------
	// ### Misc. utilities ###


	// clean the bitmap (set every pixel values to {0,0,0}
	ERRORS IMAGEPROCESSING_API CleanBitmap_24bpp(
		int width, int height,
		void* ptr, int stride)
	{
		// 1. check the parameters
		if (width <= 0 || height <= 0 || stride<width)
			return ERR_IMAGESIZE;			// image size error
		if (ptr == NULL)
			return ERR_IMAGEPOINTER;		// image pointer error

		// 2. check the format (must be 24bpp)
		if (stride < 3 * width || stride >= 4 * width)
			return ERR_IMAGEFORMAT;			// bad pixelformat

		// 3. execute the cleaning
		unsigned char* ptr_0 = (unsigned char*)ptr;
		//for (int jj = 0; jj < height; jj++)
		//{
		//	for (int ii = 0; ii < stride; ii++)
		//		ptr_0[ii+jj*stride] = 0;
		//}
		memset(ptr_0, 0, height*stride);


		return ERRNO;
	}

	ERRORS IMAGEPROCESSING_API DownscaleBitmapAvg(
		int width1, int height1,
		void* ptr1, int stride1,
		int DOWNSCALINGFACTOR,
		void* ptr2, int stride2)
	{
		// The input pixelformat must be one of the followings:
		// - 1bpp lineart
		// - 8bpp grayscale (palette is assumed to be linear grayscale - not yet checked)
		// - 16bpp grayscale
		// - 24bpp color
		// - 32bpp color with transparency info (REM: transparency is not taken into consideration)
		// The output pixelformat will be the same as the input pixelformat, the memory must be allocated by the calling  environment
		// The output pixel values are the average computed in the [DOWNSCALINGFACTOR*DOWNSCALINGFACTOR] size input kernel

		// 1. check the parameters
		if (width1 <= 0 || height1 <= 0)
			return ERR_IMAGESIZE;			// image size error
		if (ptr1 == NULL || ptr2 == NULL)
			return ERR_IMAGEPOINTER;		// image pointer error
		if (DOWNSCALINGFACTOR <= 0)
			return ERR_PARAMETER;			// parameter error

		int down_width = width1 / DOWNSCALINGFACTOR;
		int down_height = height1 / DOWNSCALINGFACTOR;
		memset(ptr2, 0, down_height*stride2);	// clear the output any way

		unsigned char* ptr1_0 = (unsigned char*)ptr1;
		unsigned char* ptr2_0 = (unsigned char*)ptr2;
		unsigned char* ptr1_c;
		unsigned char* ptr2_c;

		// 2. decide the dimension of input pixel vector
		int inputpixeldimension;
		if (stride1 >= 5 * width1)
			return ERR_IMAGEFORMAT;		// image format error
		if (stride1 >= 4 * width1)
			inputpixeldimension = 4;	// 32bpp (assumed:ARGB)
		else if (stride1 >= 3 * width1)
			inputpixeldimension = 3;	// 24bpp (assumed: RGB)
		else if (stride1 >= 2 * width1)
			inputpixeldimension = 2;	// 16bpp (assumed: grayscale)
		else if (stride1 >= width1)
			inputpixeldimension = 1;	// 8bpp (assumed: grayscale with linear palette)
		else if (stride1 >= width1 / 8)
			inputpixeldimension = 0;	// 1bpp
		else
			return ERR_IMAGEFORMAT;		// image format error

		unsigned int divider = DOWNSCALINGFACTOR*DOWNSCALINGFACTOR;
		unsigned int sum1, sum2, sum3, sum4;
		int ii, jj, i, j;
		switch (inputpixeldimension)
		{
		case 0:							// 1bpp
			// REM: presently only the DOWNSCALINGFACTOR==4 case is implemented
			if (DOWNSCALINGFACTOR != 4)
				return ERRNO;
			for (jj = 0; jj < down_height; jj++)
			{
				ptr1_c = ptr1_0;
				ptr2_c = ptr2_0;
				BYTE output = 0;
				BYTE mask = 128;
				bool bUpperHalf = true;
				for (ii = 0; ii < down_width; ii++)
				{
					sum1 = 0;
					for (j = 0; j < DOWNSCALINGFACTOR; j++)
					{
						BYTE input = ptr1_c[ii/2 + j * stride1];
						input = bUpperHalf ? input >> 4 : input & 15;
						switch (input)
						{
						case 0:
						default:
							break;
						case 1:
						case 2:
						case 4:
						case 8:
							sum1++;
							break;
						case 3:
						case 5:
						case 6:
						case 9:
						case 10:
						case 12:
							sum1 += 2;
							break;
						case 7:
						case 11:
						case 13:
						case 14:
							sum1 += 2;
							break;
						case 15:
							sum1 += 4;
							break;
						}

					}
					if (sum1 >= divider / 2)
						output = output | mask;
					mask /= 2;
					if (mask == 0)
					{
						*ptr2_c++ = output;
						output = 0;
						mask = 128;
					}
					bUpperHalf = !bUpperHalf;
				}
				ptr1_0 += DOWNSCALINGFACTOR*stride1;
				ptr2_0 += stride2;
			}
			break;
		case 1:							// 8bpp (assumed: grayscale with linear palette)
			for (jj = 0; jj < down_height; jj++)
			{
				ptr1_c = ptr1_0;
				ptr2_c = ptr2_0;
				for (ii = 0; ii < down_width; ii++)
				{
					sum1 = 0;
					for (j = 0; j < DOWNSCALINGFACTOR; j++)
					{
						for (i = 0; i < DOWNSCALINGFACTOR; i++)
						{
							sum1 += (unsigned int)ptr1_c[i + ii * DOWNSCALINGFACTOR + j * stride1];
						}
					}
					sum1 /= divider;	*ptr2_c++ = (BYTE)sum1;
				}
				ptr1_0 += DOWNSCALINGFACTOR*stride1;
				ptr2_0 += stride2;
			}
			break;
		case 2:							// 16bpp (assumed: grayscale)
			for (jj = 0; jj < down_height; jj++)
			{
				ptr1_c = ptr1_0;
				ptr2_c = ptr2_0;
				for (ii = 0; ii < down_width; ii++)
				{
					sum1 = 0; sum2 = 0; sum3 = 0;
					for (j = 0; j < DOWNSCALINGFACTOR; j++)
					{
						for (i = 0; i < DOWNSCALINGFACTOR; i++)
						{
							sum1 += (unsigned int)ptr1_c[2 * (i + ii * DOWNSCALINGFACTOR) + j * stride1];
							sum1 += (unsigned int)ptr1_c[2 * (i + ii * DOWNSCALINGFACTOR) + 1 + j * stride1] << 8;
						}
					}
					sum1 /= divider;
					BYTE value = (BYTE)(sum1 >> 8);
					*ptr2_c++ = value;
					value = (BYTE)(sum1 & 255);
					*ptr2_c++ = value;
				}
				ptr1_0 += DOWNSCALINGFACTOR*stride1;
				ptr2_0 += stride2;
			}
			break;
		case 3:							// 24bpp (assumed: RGB)
			for (jj = 0; jj < down_height; jj++)
			{
				ptr1_c = ptr1_0;
				ptr2_c = ptr2_0;
				for (ii = 0; ii < down_width; ii++)
				{
					sum1 = 0; sum2 = 0; sum3 = 0;
					for (j = 0; j < DOWNSCALINGFACTOR; j++)
					{
						for (i = 0; i < DOWNSCALINGFACTOR; i++)
						{
							sum1 += (unsigned int)ptr1_c[3 * (i + ii * DOWNSCALINGFACTOR) + j * stride1];
							sum2 += (unsigned int)ptr1_c[3 * (i + ii * DOWNSCALINGFACTOR) + 1 + j * stride1];
							sum3 += (unsigned int)ptr1_c[3 * (i + ii * DOWNSCALINGFACTOR) + 2 + j * stride1];
						}
					}
					sum1 /= divider;	*ptr2_c++ = (BYTE)sum1;
					sum2 /= divider;	*ptr2_c++ = (BYTE)sum2;
					sum3 /= divider;	*ptr2_c++ = (BYTE)sum3;
				}
				ptr1_0 += DOWNSCALINGFACTOR*stride1;
				ptr2_0 += stride2;
			}
			break;
		case 4:							// 32bpp (assumed:ARGB)
			for (jj = 0; jj < down_height; jj++)
			{
				ptr1_c = ptr1_0;
				ptr2_c = ptr2_0;
				for (ii = 0; ii < down_width; ii++)
				{
					sum1 = 0; sum2 = 0; sum3 = 0; sum4 = 0;
					for (j = 0; j < DOWNSCALINGFACTOR; j++)
					{
						for (i = 0; i < DOWNSCALINGFACTOR; i++)
						{
							sum1 += (unsigned int)ptr1_c[4 * (i + ii * DOWNSCALINGFACTOR) + j * stride1];
							sum2 += (unsigned int)ptr1_c[4 * (i + ii * DOWNSCALINGFACTOR) + 1 + j * stride1];
							sum3 += (unsigned int)ptr1_c[4 * (i + ii * DOWNSCALINGFACTOR) + 2 + j * stride1];
							sum4 += (unsigned int)ptr1_c[4 * (i + ii * DOWNSCALINGFACTOR) + 3 + j * stride1];
						}
					}
					sum1 /= divider;	*ptr2_c++ = (BYTE)sum1;
					sum2 /= divider;	*ptr2_c++ = (BYTE)sum2;
					sum3 /= divider;	*ptr2_c++ = (BYTE)sum3;
					sum4 /= divider;	*ptr2_c++ = (BYTE)sum4;
				}
				ptr1_0 += DOWNSCALINGFACTOR*stride1;
				ptr2_0 += stride2;
			}
			break;
		}

		return ERRNO;
	}

	ERRORS IMAGEPROCESSING_API DownscaleBitmap(
		int width1, int height1,
		void* ptr1, int stride1,
		int DOWNSCALINGFACTOR,
		void* ptr2, int stride2)
	{
		// The input pixelformat must be one of the followings:
		// - 1bpp lineart
		// - 8bpp grayscale (palette is assumed to be linear grayscale - not yet checked)
		// - 16bpp grayscale
		// - 24bpp color
		// - 32bpp color with transparency info (REM: transparency is not taken into consideration)
		// The output pixelformat will be the same as the input pixelformat, the memory must be allocated by the calling  environment
		// The output pixel value is equal to the left-top pixel of [DOWNSCALINGFACTOR*DOWNSCALINGFACTOR] size input kernel

		// 1. check the parameters
		if (width1 <= 0 || height1 <= 0)
			return ERR_IMAGESIZE;			// image size error
		if (ptr1 == NULL || ptr2 == NULL)
			return ERR_IMAGEPOINTER;		// image pointer error
		if (DOWNSCALINGFACTOR <= 0)
			return ERR_PARAMETER;			// parameter error

		int down_width = width1 / DOWNSCALINGFACTOR;
		int down_height = height1 / DOWNSCALINGFACTOR;
		memset(ptr2, 0, down_height*stride2);	// clear the output any way

		unsigned char* ptr1_0 = (unsigned char*)ptr1;
		unsigned char* ptr2_0 = (unsigned char*)ptr2;
		unsigned char* ptr1_c;
		unsigned char* ptr2_c;

		// 2. decide the dimension of input pixel vector
		int inputpixeldimension;
		if (stride1 >= 5 * width1)
			return ERR_IMAGEFORMAT;		// image format error
		if (stride1 >= 4 * width1)
			inputpixeldimension = 4;	// 32bpp (assumed:ARGB)
		else if (stride1 >= 3 * width1)
			inputpixeldimension = 3;	// 24bpp (assumed: RGB)
		else if (stride1 >= 2 * width1)
			inputpixeldimension = 2;	// 16bpp (assumed: grayscale)
		else if (stride1 >= width1)
			inputpixeldimension = 1;	// 8bpp (assumed: grayscale with linear palette)
		else if (stride1 >= width1 / 8)
			inputpixeldimension = 0;	// 1bpp
		else
			return ERR_IMAGEFORMAT;		// image format error

		unsigned int divider = DOWNSCALINGFACTOR*DOWNSCALINGFACTOR;
		unsigned int sum1;
		BYTE value;
		int ii, jj, j;
		switch (inputpixeldimension)
		{
		case 0:							// 1bpp
										// REM: presently only the DOWNSCALINGFACTOR==4 case is implemented
			if (DOWNSCALINGFACTOR != 4)
				return ERRNO;
			for (jj = 0; jj < down_height; jj++)
			{
				ptr1_c = ptr1_0;
				ptr2_c = ptr2_0;
				BYTE output = 0;
				BYTE mask = 128;
				bool bUpperHalf = true;
				for (ii = 0; ii < down_width; ii++)
				{
					sum1 = 0;
					for (j = 0; j < DOWNSCALINGFACTOR; j++)
					{
						BYTE input = ptr1_c[ii / 2 + j * stride1];
						input = bUpperHalf ? input >> 4 : input & 15;
						switch (input)
						{
						case 0:
						default:
							break;
						case 1:
						case 2:
						case 4:
						case 8:
							sum1++;
							break;
						case 3:
						case 5:
						case 6:
						case 9:
						case 10:
						case 12:
							sum1 += 2;
							break;
						case 7:
						case 11:
						case 13:
						case 14:
							sum1 += 2;
							break;
						case 15:
							sum1 += 4;
							break;
						}

					}
					if (sum1 >= divider / 2)
						output = output | mask;
					mask /= 2;
					if (mask == 0)
					{
						*ptr2_c++ = output;
						output = 0;
						mask = 128;
					}
					bUpperHalf = !bUpperHalf;
				}
				ptr1_0 += DOWNSCALINGFACTOR*stride1;
				ptr2_0 += stride2;
			}
			break;
		case 1:							// 8bpp (assumed: grayscale with linear palette)
			for (jj = 0; jj < down_height; jj++)
			{
				ptr1_c = ptr1_0;
				ptr2_c = ptr2_0;
				for (ii = 0; ii < down_width; ii++)
				{
					value = ptr1_c[ii * DOWNSCALINGFACTOR];
					*ptr2_c++ = value;
				}
				ptr1_0 += DOWNSCALINGFACTOR*stride1;
				ptr2_0 += stride2;
			}
			break;
		case 2:							// 16bpp (assumed: grayscale)
			for (jj = 0; jj < down_height; jj++)
			{
				ptr1_c = ptr1_0;
				ptr2_c = ptr2_0;
				for (ii = 0; ii < down_width; ii++)
				{
					sum1 = (unsigned int)ptr1_c[2 * ii * DOWNSCALINGFACTOR];
					sum1 += (unsigned int)ptr1_c[2 * ii * DOWNSCALINGFACTOR + 1] << 8;
					value = (BYTE)(sum1 >> 8);
					*ptr2_c++ = value;
					value = (BYTE)(sum1 & 255);
					*ptr2_c++ = value;
				}
				ptr1_0 += DOWNSCALINGFACTOR*stride1;
				ptr2_0 += stride2;
			}
			break;
		case 3:							// 24bpp (assumed: RGB)
			for (jj = 0; jj < down_height; jj++)
			{
				ptr1_c = ptr1_0;
				ptr2_c = ptr2_0;
				for (ii = 0; ii < down_width; ii++)
				{
					value = ptr1_c[3 * (ii * DOWNSCALINGFACTOR)];
					*ptr2_c++ = value;
					value = ptr1_c[3 * (ii * DOWNSCALINGFACTOR) + 1];
					*ptr2_c++ = value;
					value = ptr1_c[3 * (ii * DOWNSCALINGFACTOR) + 2];
					*ptr2_c++ = value;
				}
				ptr1_0 += DOWNSCALINGFACTOR*stride1;
				ptr2_0 += stride2;
			}
			break;
		case 4:							// 32bpp (assumed:ARGB)
			for (jj = 0; jj < down_height; jj++)
			{
				ptr1_c = ptr1_0;
				ptr2_c = ptr2_0;
				for (ii = 0; ii < down_width; ii++)
				{
					value = ptr1_c[4 * (ii * DOWNSCALINGFACTOR)];
					*ptr2_c++ = value;
					value = ptr1_c[4 * (ii * DOWNSCALINGFACTOR) + 1];
					*ptr2_c++ = value;
					value = ptr1_c[4 * (ii * DOWNSCALINGFACTOR) + 2];
					*ptr2_c++ = value;
					value = ptr1_c[4 * (ii * DOWNSCALINGFACTOR) + 3];
					*ptr2_c++ = value;
				}
				ptr1_0 += DOWNSCALINGFACTOR*stride1;
				ptr2_0 += stride2;
			}
			break;
		}

		return ERRNO;
	}
	//--------------------------------------------------------------------------------


}



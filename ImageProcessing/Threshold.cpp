// Threshold.cpp : creating bi-level image by thresholding (0:object, 255:background)
//

#include "stdafx.h"

#include "ImageProcessing.h"

#include <math.h>

//------------------------------------------------------------------------------

namespace ImageProcessing
{
	//--------------------------------------------------------------------------------
	// ### Thresholding ###

	// Writes the thresholded values to the output, the input is untouched
	ERRORS IMAGEPROCESSING_API Threshold_8bpp(
		int width, int height,
		void* ptr1, int stride1,
		void* ptr2, int stride2,
		int threshold)
	{
		// 1. check the parameters
		if ( width<=0 || height <= 0 || stride1<width || stride2<width )
			return ERR_IMAGESIZE;			// image size error
		if ( ptr1==NULL || ptr2 == NULL)
			return ERR_IMAGEPOINTER;		// image pointer error
		if ( threshold<0 || threshold>255)
			return ERR_PARAMETER;			// parameter error

		// 3. execute the thresholding
		unsigned char* ptr1_0 = (unsigned char*)ptr1;
		unsigned char* ptr2_0 = (unsigned char*)ptr2;
		unsigned char* ptr1_c;
		unsigned char* ptr2_c;
		unsigned char pixel_value;
		for ( int jj=0; jj<height; jj++ )
		{
			ptr1_c = ptr1_0;
			ptr2_c = ptr2_0;
			for ( int ii=0; ii<width; ii++ )
			{
				pixel_value = *ptr1_c++;
				*ptr2_c++ = pixel_value>threshold ? 255:0;
			}
			ptr1_0 += stride1;
			ptr2_0 += stride2;
		}
		return ERRNO;
	}


	//--------------------------------------------------------------------------------

}



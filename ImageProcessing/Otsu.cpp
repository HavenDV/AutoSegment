// Otsu.cpp : Otsu thresholding
//

#include "stdafx.h"

#include "ImageProcessing.h"

#include <math.h>

//------------------------------------------------------------------------------

namespace ImageProcessing
{
	//--------------------------------------------------------------------------------
	// ### Otsu thresholding ###

	ERRORS IMAGEPROCESSING_API Otsu_8bpp(
		int width, int height,
		void* ptr1, int stride1,
		void* ptr2, int stride2,
		int destroyedband)
	{

		// 1. check the parameters
		if ( width<=0 || height <= 0 || stride1<width || stride2<width )
			return ERR_IMAGESIZE;			// image size error
		if ( ptr1==NULL || ptr2 == NULL)
			return ERR_IMAGEPOINTER;		// image pointer error

		unsigned char* ptr1_0 = (unsigned char*)ptr1;
		unsigned char* ptr2_0 = (unsigned char*)ptr2;
		unsigned char* ptr1_c;
		unsigned char* ptr2_c;
		int img_size = width*height;

		// 2. get the histogram
		double* pHist = NULL;
		try
		{
			pHist = new double[256];
		}
		catch(...)
		{
			return ERR_MEMORY;		// memory allocation error
		}
		memset(pHist, 0, 256*sizeof(double));
		for ( int jj=0; jj<height; jj++)
		{
			ptr1_c = ptr1_0;
			for ( int ii=0; ii<width; ii++)
			{
				pHist[*ptr1_c++] += 1.0;
			}
			ptr1_0 += stride1;
		}

		// 3. get rid of smallest and highest pixel values (under and over-exposured images)
		int img_size_d = img_size;
		for (int ii = 0; ii < destroyedband; ii++)
		{
			img_size_d -= (int)pHist[ii];
			pHist[ii] = 0;
		}
		for (int ii = (256 - destroyedband); ii <256 ; ii++)
		{
			img_size_d -= (int)pHist[ii];
			pHist[ii] = 0;
		}

		// 4. normalize the histogram
		for ( int ii=0; ii<256; ii++)
			pHist[ii]/=img_size_d;

		// 5. produce the bilevel output
		double ut = 0;
		for ( int ii=0; ii<256; ii++)
			ut += ii*pHist[ii];

		int max_k = 0;
		int max_sigma_k = 0;
		for ( int k=0; k<256; ++k)
		{
			double wk = 0;
			for ( int ii=0; ii<=k; ++ii)
				wk += pHist[ii];
			double uk = 0;
			for ( int ii=0; ii<=k; ++ii)
				uk += ii*pHist[ii];
			double sigma_k = 0;
			if ( wk !=0 && wk != -1)
				sigma_k = ((ut*wk - uk)*(ut*wk - uk))/(wk*(1-wk));
			if ( sigma_k>max_sigma_k)
			{
				max_k = k;
				max_sigma_k = (int)sigma_k;
			}
		}

		// 6. write it to output bitmap
		ptr1_0 = (unsigned char*)ptr1;
		for ( int jj=0; jj<height; jj++)
		{
			ptr1_c = ptr1_0;
			ptr2_c = ptr2_0;
			for ( int ii=0; ii<width; ii++)
			{
				unsigned char pixelvalue = *ptr1_c++;
				*ptr2_c++ = pixelvalue>max_k ? 255:0;
			}
			ptr1_0 += stride1;
			ptr2_0 += stride2;
		}

		// get rid of allocated memory
		delete [] pHist;
		return ERRNO;
	}

}


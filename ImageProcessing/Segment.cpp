// Segment.cpp : creating a segmented image where all disjunct spots have different pixel values
//		 (serial number of spot)
//

#include "stdafx.h"

#include "ImageProcessing.h"

#include <math.h>

//------------------------------------------------------------------------------

namespace ImageProcessing
{

	//--------------------------------------------------------------------------------
	// forward declarations

	int GetCurrentSpotSN(int* features, int FEATURES0ARRAYLENGTH);

	//--------------------------------------------------------------------------------
	// ### Segmenting ###

	ERRORS IMAGEPROCESSING_API SegmentImage_8bpp(
		int width, int height,
		void* ptr1, int stride1,
		int* Segmented, int* features,
		int FEATURES0ARRAYLENGTH)
	{
		// 1. check the parameters
		if ( width<=0 || height <= 0 || stride1<width )
			return ERR_IMAGESIZE;			// image size error
		if ( ptr1==NULL)
			return ERR_IMAGEPOINTER;		// image pointer error
		if (stride1 >= 2 * width)
			return ERR_IMAGEFORMAT;			// image format error (input must be 8bpp)
		if (Segmented == NULL || features == NULL)
			return ERR_PARAMETER;			// parameter error


		// 2. init. the features vector
		memset(features, 0, FEATURES0ARRAYLENGTH * sizeof(int));
		unsigned int currentSpotSN = 0;
		unsigned char* ptr1_0 = (unsigned char*)ptr1;


		// 3. init. the output (segmented) image
		memset(Segmented, 0, width*height*sizeof(int));

		// 4. execute the segmentation
		for (int jj = 0; jj < height; jj++)
		{
			for (int ii = 0; ii < width; ii++)
			{
				if (ptr1_0[ii+jj*stride1] == 255)
					continue;						// background (white)

				// 1. decide SN for the current pixel (before analysing the merge options)
				if (ii == 0 || (ptr1_0[ii - 1 + jj*stride1] == 255))
				{
					currentSpotSN = GetCurrentSpotSN(features, FEATURES0ARRAYLENGTH);
					if (currentSpotSN == -1)
						return ERRNO;	// overloaded

					features[6 * currentSpotSN + 2] = features[6 * currentSpotSN + 4] = ii;
					features[6 * currentSpotSN + 3] = features[6 * currentSpotSN + 5] = jj;
				}
				else
				{
					currentSpotSN = Segmented[ii - 1 + jj*width];
					if (ii > features[6 * currentSpotSN + 4])
						features[6 * currentSpotSN + 4] = ii;	// only right side corner's x-coord. can be increased
				}
				Segmented[ii + jj*width] = currentSpotSN;
				features[6 * currentSpotSN + 1] += 1;

				// 2. decide, if it is necessary to merge?
				int MergeSpotSN = -1;
				if (jj > 0 && (ptr1_0[ii + (jj - 1)*stride1] != 255) && (Segmented[ii + (jj-1)*width] != currentSpotSN))
					MergeSpotSN = Segmented[ii + (jj - 1)*width];
				else if (jj > 0 && (ptr1_0[ii + 1 + (jj - 1)*stride1] != 255) && (Segmented[ii + 1 + (jj - 1)*width] != currentSpotSN))
					MergeSpotSN = Segmented[ii + 1 + (jj - 1)*width];
				if (MergeSpotSN != -1)
				{
					// 3. merge - keeping the smaller ID
					if ((int)currentSpotSN < MergeSpotSN)
					{
						int TmpSN = currentSpotSN;
						currentSpotSN = MergeSpotSN;
						MergeSpotSN = TmpSN;
					}

					for (int jjjj = features[6 * currentSpotSN + 3]; jjjj <= features[6 * currentSpotSN + 5]; jjjj++)
					{
						for (int iiii = features[6 * currentSpotSN + 2]; iiii <= features[6 * currentSpotSN + 4]; iiii++)
						{
							if (Segmented[iiii + jjjj*width] == currentSpotSN)
								Segmented[iiii + jjjj*width] = MergeSpotSN;
						}
					}


					features[6 * MergeSpotSN + 1] += features[6 * currentSpotSN + 1];
					if (features[6 * currentSpotSN + 2] < features[6 * MergeSpotSN + 2])
						features[6 * MergeSpotSN + 2] = features[6 * currentSpotSN + 2];
					if (features[6 * currentSpotSN + 3] < features[6 * MergeSpotSN + 3])
						features[6 * MergeSpotSN + 3] = features[6 * currentSpotSN + 3];
					if (features[6 * currentSpotSN + 4] > features[6 * MergeSpotSN + 4])
						features[6 * MergeSpotSN + 4] = features[6 * currentSpotSN + 4];
					if (features[6 * currentSpotSN + 5] > features[6 * MergeSpotSN + 5])
						features[6 * MergeSpotSN + 5] = features[6 * currentSpotSN + 5];

					for (int kk = 0; kk < 6; kk++)
						features[6 * currentSpotSN + kk] = 0;

				}
			}
		}

		return ERRNO;
	}

	int GetCurrentSpotSN(int* features, int FEATURES0ARRAYLENGTH)
	{
		int CurrentSpotSN = 1;	// start at index=1

		// look for the first free index
		bool bFound = false;
		int maxSN = FEATURES0ARRAYLENGTH / 6;
		while (!bFound && (CurrentSpotSN < maxSN))
		{
			if (features[6 * CurrentSpotSN + 1] == 0)
				bFound = true;
			else
				CurrentSpotSN++;
		}
		return bFound ? CurrentSpotSN : -1;
	}


	//--------------------------------------------------------------------------------

}



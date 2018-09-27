// FeatureExtraction.cpp : extract features for selected spot (object)
//

#include "stdafx.h"

#include "ImageProcessing.h"

#include <math.h>

//------------------------------------------------------------------------------

namespace ImageProcessing
{

	//--------------------------------------------------------------------------------
	// forward declarations

	enum DIRECTION						// directions for the contour following
	{
		DIR1 = 1,						// to the right
		DIR2,							// to the right-bottom
		DIR3,							// to the bottom
		DIR4,							// to the left-bottom
		DIR5,							// to the left
		DIR6,							// to the left-top
		DIR7,							// to the top
		DIR8							// to the right-top
	};

	void ExtractSpotContour(
		int width, int height,
		int* Segmented, int* features,
		int spotSN, double* Contour_ptr);

	double GetPerimeterOfSpotContour(double* contour_ptr);

	//--------------------------------------------------------------------------------
	// ### Feature extraction ###

	// get the necessary features from original (RGB) input image, basic features and segmented spot,
	// and store them to the Teachers array, beginning at 'newitem_index'
	ERRORS IMAGEPROCESSING_API GetFeaturesOfSelectedTeacher(
		int width, int height,
		void* ptr1, int stride1,
		int* Segmented, int* features0,
		int spotSN, double* Teachers,
		int newitem_index)
	{
		// 1. check the parameters
		if ( width<=0 || height <= 0 || stride1<width )
			return ERR_IMAGESIZE;			// image size error
		if ( ptr1==NULL)
			return ERR_IMAGEPOINTER;		// image pointer error
		if (Segmented == NULL || features0 == NULL)
			return ERR_PARAMETER;			// parameter error

		// decide the pixelformat of original input image
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
		//else if (stride1 >= width / 8)
		//	inputpixeldimension = 0;	// 1bpp	REM: not allowed
		else
			return ERR_IMAGEFORMAT;		// image format error
		unsigned char* ptr1_0 = (unsigned char*)ptr1;


		// 3. The following features will be extracted to teachers[newitem_index] variable length block:
		//    [0]:    serial number of "name" identifying the object category (already filled)
		//    [1]:    x-coordinate of "centre" of representing spot
		//    [2]:    y-coordinate of "centre" of representing spot
		//    [3]:    average of R components of spot's pixel values
		//    [4]:    average of G components of spot's pixel values
		//    [5]:    average of B components of spot's pixel values
		//    [6]:    grayscale value (average of R, G and B components)
		//    [7]:    area (number of pixels inside the spot)
		//    [8]:    min. 'diameter' in pixels
		//    [9]:    max. 'diameter' in pixels
		//    [10]:   aspect ratio = min_d/max_d
		//    [11]:   circularity = area/(perimeter*perimeter)
		//    [12]:   number of vertex positions for clockwise contour description    (=N1)
		//    [13]:   ... N1 pieces of pairs: x and y coordinates of vertex position (in image's coordinate system)
		double centre_x = 0.0;
		double centre_y = 0.0;
		double avg_R = 0.0;
		double avg_G = 0.0;
		double avg_B = 0.0;
		double avg_I = 0.0;
		double area = features0[6* spotSN + 1];
		double min_diameter = 100000.0;	// init. with impossible
		double max_diameter = 0.0;
		double aspect_ratio = 0.0;
		double circularity = 0.0;
		double pixel_value;
		unsigned int value32;
		for (int jjjj = features0[6 * spotSN + 3]; jjjj <= features0[6 * spotSN + 5]; jjjj++)
		{
			for (int iiii = features0[6 * spotSN + 2]; iiii <= features0[6 * spotSN + 4]; iiii++)
			{
				if (Segmented[iiii + jjjj*width] != spotSN)
					continue;	// within the container rectangle, but not belongign to the current spot
				centre_x += (double)iiii;
				centre_y += (double)jjjj;
				switch (inputpixeldimension)
				{
				case 1:			// 8bpp (assumed: grayscale with linear palette)
					pixel_value = (double)ptr1_0[iiii + jjjj*stride1];
					avg_R += pixel_value;
					avg_G += pixel_value;
					avg_B += pixel_value;
					break;
				case 2:			// 16bpp (assumed: grayscale, without palette)
					value32 = (unsigned int)ptr1_0[2*iiii + jjjj*stride1];
					value32 += (unsigned int)(ptr1_0[2 * iiii + 1 + jjjj*stride1] << 8);
					value32 >>= 8;	// downscaling: {0...65535} >>> {0...255}
					pixel_value = (double)value32;
					avg_R += pixel_value;
					avg_G += pixel_value;
					avg_B += pixel_value;
					break;
				case 3:			// 24bpp (assumed: RGB)
					avg_B += (double)(ptr1_0[3 * iiii + 0 + jjjj*stride1]);
					avg_G += (double)(ptr1_0[3 * iiii + 1 + jjjj*stride1]);
					avg_R += (double)(ptr1_0[3 * iiii + 2 + jjjj*stride1]);
					break;
				case 4:			// 32bpp (assumed: ARGB)
					avg_B += (double)(ptr1_0[4 * iiii + 0 + jjjj*stride1]);
					avg_G += (double)(ptr1_0[4 * iiii + 1 + jjjj*stride1]);
					avg_R += (double)(ptr1_0[4 * iiii + 2 + jjjj*stride1]);
					break;
				}
			}
		}
		centre_x /= area;
		centre_y /= area;
		avg_R /= area;
		avg_G /= area;
		avg_B /= area;
		avg_I = (avg_R + avg_G + avg_B) / 3.0;

		// decide (roughly) the min. and max. diameters
		// - decide the horizontal/vertical min/max diameters
		double SpotWidth = features0[6 * spotSN + 4] - features0[6 * spotSN + 2] + 1;
		double SpotHeight = features0[6 * spotSN + 5] - features0[6 * spotSN + 3] + 1;
		if (SpotWidth < SpotHeight)
		{
			min_diameter = SpotWidth;
			max_diameter = SpotHeight;
		}
		else
		{
			min_diameter = SpotHeight;
			max_diameter = SpotWidth;
		}
		// - decide the diagonal min/max sizes - and get the overall min/max values
		double DiagonalWidth = 1;
		// TODO: decide it rotation-invariant manner

		// decide the aspect ratio
		aspect_ratio = max_diameter == 0.0 ? 0.0 : min_diameter / max_diameter;

		// decide the contour description
		double* Contour_ptr = Teachers + newitem_index + 12;	// pointer to the vertex-position counter, followed by series of positions
		ExtractSpotContour( width, height, Segmented, features0, spotSN, Contour_ptr);
		double perimeter = GetPerimeterOfSpotContour(Contour_ptr);

		// decide the circularity
		double PI = 3.1415926535897932384626433832;
		circularity = (perimeter==0.0) ? 0.0 : (4.0 * PI *area / perimeter / perimeter);

		// write the extracted features to the Teacher array
		// REM: all features are normalized - pixel value components by /255.0, etc.
		Teachers[newitem_index + 1] = centre_x;
		Teachers[newitem_index + 2] = centre_y;
		Teachers[newitem_index + 3] = avg_R/255.0;
		Teachers[newitem_index + 4] = avg_G / 255.0;
		Teachers[newitem_index + 5] = avg_B / 255.0;
		Teachers[newitem_index + 6] = avg_I / 255.0;
		Teachers[newitem_index + 7] = area/400.0;
		Teachers[newitem_index + 8] = min_diameter/20.0;
		Teachers[newitem_index + 9] = max_diameter/20.0;
		Teachers[newitem_index + 10] = aspect_ratio;
		Teachers[newitem_index + 11] = circularity;

		return ERRNO;
	}


	// get the necessary features from original (RGB) input image, basic features and segmented spot,
	// and store the NBOFFEATURES value to the Feature_ptr vector
	ERRORS IMAGEPROCESSING_API GetFeaturesOfSelectedSpot(
		int width, int height,
		void* ptr1, int stride1,
		int* Segmented, int* features0,
		int spotSN, double* Feature_ptr)
	{
		// 1. check the parameters
		if (width <= 0 || height <= 0 || stride1<width)
			return ERR_IMAGESIZE;			// image size error
		if (ptr1 == NULL)
			return ERR_IMAGEPOINTER;		// image pointer error
		if (Segmented == NULL || features0 == NULL)
			return ERR_PARAMETER;			// parameter error

											// decide the pixelformat of original input image
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
										//else if (stride1 >= width / 8)
										//	inputpixeldimension = 0;	// 1bpp	REM: not allowed
		else
			return ERR_IMAGEFORMAT;		// image format error
		unsigned char* ptr1_0 = (unsigned char*)ptr1;


		// 3. The following features will be extracted to Feature_ptr variable length block:
		//    [0]:    average of R components of spot's pixel values
		//    [1]:    average of G components of spot's pixel values
		//    [2]:    average of B components of spot's pixel values
		//    [3]:    grayscale value (average of R, G and B components)
		//    [4]:    area (number of pixels inside the spot)
		//    [5]:    min. 'diameter' in pixels
		//    [6]:    max. 'diameter' in pixels
		//    [7]:   aspect ratio = min_d/max_d
		//    [8]:   circularity = area/(perimeter*perimeter)
		//    [9]:   number of vertex positions for clockwise contour description    (=N1)
		//    [10]: ...  N1 pieces of pairs: x and y coordinates of vertex position (in image's coordinate system)
		double avg_R = 0.0;
		double avg_G = 0.0;
		double avg_B = 0.0;
		double avg_I = 0.0;
		double area = features0[6 * spotSN + 1];
		double min_diameter = 100000.0;	// init. with impossible
		double max_diameter = 0.0;
		double aspect_ratio = 0.0;
		double circularity = 0.0;
		double pixel_value;
		unsigned int value32;
		for (int jjjj = features0[6 * spotSN + 3]; jjjj <= features0[6 * spotSN + 5]; jjjj++)
		{
			for (int iiii = features0[6 * spotSN + 2]; iiii <= features0[6 * spotSN + 4]; iiii++)
			{
				if (Segmented[iiii + jjjj*width] != spotSN)
					continue;	// within the container rectangle, but not belongign to the current spot
				switch (inputpixeldimension)
				{
				case 1:			// 8bpp (assumed: grayscale with linear palette)
					pixel_value = (double)ptr1_0[iiii + jjjj*stride1];
					avg_R += pixel_value;
					avg_G += pixel_value;
					avg_B += pixel_value;
					break;
				case 2:			// 16bpp (assumed: grayscale, without palette)
					value32 = (unsigned int)ptr1_0[2 * iiii + jjjj*stride1];
					value32 += (unsigned int)(ptr1_0[2 * iiii + 1 + jjjj*stride1] << 8);
					value32 >>= 8;	// downscaling: {0...65535} >>> {0...255}
					pixel_value = (double)value32;
					avg_R += pixel_value;
					avg_G += pixel_value;
					avg_B += pixel_value;
					break;
				case 3:			// 24bpp (assumed: RGB)
					avg_B += (double)(ptr1_0[3 * iiii + 0 + jjjj*stride1]);
					avg_G += (double)(ptr1_0[3 * iiii + 1 + jjjj*stride1]);
					avg_R += (double)(ptr1_0[3 * iiii + 2 + jjjj*stride1]);
					break;
				case 4:			// 32bpp (assumed: ARGB)
					avg_B += (double)(ptr1_0[4 * iiii + 0 + jjjj*stride1]);
					avg_G += (double)(ptr1_0[4 * iiii + 1 + jjjj*stride1]);
					avg_R += (double)(ptr1_0[4 * iiii + 2 + jjjj*stride1]);
					break;
				}
			}
		}
		avg_R /= area;
		avg_G /= area;
		avg_B /= area;
		avg_I = (avg_R + avg_G + avg_B) / 3.0;

		// decide (roughly) the min. and max. diameters
		// - decide the horizontal/vertical min/max diameters
		double SpotWidth = features0[6 * spotSN + 4] - features0[6 * spotSN + 2] + 1;
		double SpotHeight = features0[6 * spotSN + 5] - features0[6 * spotSN + 3] + 1;
		if (SpotWidth < SpotHeight)
		{
			min_diameter = SpotWidth;
			max_diameter = SpotHeight;
		}
		else
		{
			min_diameter = SpotHeight;
			max_diameter = SpotWidth;
		}
		// - decide the diagonal min/max sizes - and get the overall min/max values
		double DiagonalWidth = 1;
		// TODO: decide it rotation-invariant manner

		// decide the aspect ratio
		aspect_ratio = max_diameter == 0.0 ? 0.0 : min_diameter / max_diameter;

		// decide the contour description
		double* Contour_ptr = Feature_ptr + 9;	// pointer to the vertex-position counter, followed by series of positions
		ExtractSpotContour(width, height, Segmented, features0, spotSN, Contour_ptr);
		double perimeter = GetPerimeterOfSpotContour(Contour_ptr);

		// decide the circularity
		double PI = 3.1415926535897932384626433832;
		circularity = (perimeter == 0.0) ? 0.0 : (4.0 * PI *area / perimeter / perimeter);

		// write the extracted features to the Feature_ptr vector
		// REM: all features are normalized - pixel value components by /255.0, etc.
		Feature_ptr[0] = avg_R / 255.0;
		Feature_ptr[1] = avg_G / 255.0;
		Feature_ptr[2] = avg_B / 255.0;
		Feature_ptr[3] = avg_I / 255.0;
		Feature_ptr[4] = area / 400.0;
		Feature_ptr[5] = min_diameter / 20.0;
		Feature_ptr[6] = max_diameter / 20.0;
		Feature_ptr[7] = aspect_ratio;
		Feature_ptr[8] = circularity;

		return ERRNO;
	}

	//--------------------------------------------------------------------------------

	void ExtractSpotContour(
		int width, int height,
		int* Segmented, int* features,
		int spotSN, double* Contour_ptr)
	{
		Contour_ptr[0] = 0;
		int nbofvertexpositions = 0;

		// 1. decide and store the starting position
		int starting_x = features[6 * spotSN + 2];	// start at the left-top corner
		int starting_y = features[6 * spotSN + 3];
		bool bFound = false;
		while (!bFound && starting_x < features[6 * spotSN + 4])
		{
			if (Segmented[starting_x + starting_y*width] == spotSN)
				bFound = true;
			else
				starting_x++;
		}
		Contour_ptr[1 + 2 * nbofvertexpositions] = starting_x;
		Contour_ptr[1 + 2 * nbofvertexpositions + 1] = starting_y;

		// 2. set the starting direction
		DIRECTION currentdir = DIR8;						// to the right-top

		// 3. follow the spot's border - in clockwise manner -, till closing
		bool bClosed = false;
		bool bContinuationFound = true;
		int current_x = starting_x;
		int current_y = starting_y;
		int new_x, new_y;
		int TryCounter = 0;
		while (!bClosed && TryCounter <= 7)
		{
			bContinuationFound = false;
			TryCounter++;
			switch (currentdir)
			{
			case DIR1:
			default:
				new_x = current_x + 1;	new_y = current_y;
				if (new_x >= width)
				{
					currentdir = DIR2;
					continue;
				}
				if ( Segmented[new_x + new_y*width]==spotSN)
				{
					bContinuationFound = true;
					currentdir = DIR6;
				}
				else
					currentdir = DIR2;
				break;
			case DIR2:
				new_x = current_x + 1;	new_y = current_y + 1;
				if (new_x >= width || new_y >= height)
				{
					currentdir = DIR3;
					continue;
				}
				if (Segmented[new_x + new_y*width] == spotSN)
				{
					bContinuationFound = true;
					currentdir = DIR7;
				}
				else
					currentdir = DIR3;
				break;
			case DIR3:
				new_x = current_x;	new_y = current_y + 1;
				if (new_y >= height)
				{
					currentdir = DIR4;
					continue;
				}
				if (Segmented[new_x + new_y*width] == spotSN)
				{
					bContinuationFound = true;
					currentdir = DIR8;
				}
				else
					currentdir = DIR4;
				break;
			case DIR4:
				new_x = current_x - 1;	new_y = current_y + 1;
				if (new_x < 0 || new_y >= height)
				{
					currentdir = DIR5;
					continue;
				}
				if (Segmented[new_x + new_y*width] == spotSN)
				{
					bContinuationFound = true;
					currentdir = DIR1;
				}
				else
					currentdir = DIR5;
				break;
			case DIR5:
				new_x = current_x - 1;	new_y = current_y;
				if (new_x < 0)
				{
					currentdir = DIR6;
					continue;
				}
				if (Segmented[new_x + new_y*width] == spotSN)
				{
					bContinuationFound = true;
					currentdir = DIR2;
				}
				else
					currentdir = DIR6;
				break;
			case DIR6:
				new_x = current_x - 1;	new_y = current_y - 1;
				if (new_x < 0 || new_y < 0)
				{
					currentdir = DIR7;
					continue;
				}
				if (Segmented[new_x + new_y*width] == spotSN)
				{
					bContinuationFound = true;
					currentdir = DIR3;
				}
				else
					currentdir = DIR7;
				break;
			case DIR7:
				new_x = current_x;	new_y = current_y - 1;
				if (new_y < 0)
				{
					currentdir = DIR8;
					continue;
				}
				if (Segmented[new_x + new_y*width] == spotSN)
				{
					bContinuationFound = true;
					currentdir = DIR4;
				}
				else
					currentdir = DIR8;
				break;
			case DIR8:
				new_x = current_x + 1;	new_y = current_y - 1;
				if (new_x >= width || new_y < 0)
				{
					currentdir = DIR1;
					continue;
				}
				if (Segmented[new_x + new_y*width] == spotSN)
				{
					bContinuationFound = true;
					currentdir = DIR5;
				}
				else
					currentdir = DIR1;
				break;
			}

			if (bContinuationFound)
			{
				TryCounter = 0;
				if ((starting_x == new_x) && (starting_y == new_y))
					bClosed = true;
				else
				{
					// store the new position
					current_x = new_x;
					current_y = new_y;
					Contour_ptr[1 + 2 * nbofvertexpositions] = current_x;
					Contour_ptr[1 + 2 * nbofvertexpositions + 1] = current_y;
					nbofvertexpositions++;

				}
			}
		}

		///TODO: make the storage more effective:
		//        - detect the straight line segments, and
		//        - represent them with their end-positions


		Contour_ptr[0] = (nbofvertexpositions-1);

	}

	double GetPerimeterOfSpotContour(double* contour_ptr)
	{
		double perimeter = 0;

		int NbofVertexPositions = (int)contour_ptr[0];
		for (int ii = 0; ii < NbofVertexPositions; ii++)
		{
			int x1 = (int)contour_ptr[1 + 2 * ii];
			int y1 = (int)contour_ptr[1 + 2 * ii + 1];
			int x2 = ii==(NbofVertexPositions-1) ? (int)contour_ptr[1] : (int)contour_ptr[1 + 2 * (ii+1)];
			int y2 = ii == (NbofVertexPositions - 1) ? (int)contour_ptr[1 + 1] : (int)contour_ptr[1 + 2 * (ii+1) + 1];
			double xdiff = (double)(x2 - x1);
			double ydiff = (double)(y2 - y1);
			perimeter += sqrt(xdiff*xdiff + ydiff*ydiff);
		}

		return perimeter;
	}
}



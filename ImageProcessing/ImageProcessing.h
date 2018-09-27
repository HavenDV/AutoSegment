#include "ImageProcessingDefs.h"

#include "Errors.h"

#include <vector>

namespace ImageProcessing
{
	using namespace std;


	//--------------------------------------------------------------------------------
		// ### Common features of image processing functions ###
		//
		// Image format
		//	- All function works with color images (24bpp: 3x8 bits/pixels, in RGB colorspace) or
		//                            grayscale images (8bpp) - with version "_24bpp" or "_8bpp"
		//  - Paletted images are assumed:  grayscale, with linear palette
		//  - The pixels are stored row-continuously, but at the end of rows padding bytes
		//    can be presented - that do not hold image information. This is the reason of using
		//    'stride' parameter (that's value is necessary for working row-continuously)
		//	- Due to Microsoft's packing policy, the color channels are stored in 'BGR' order
		//
		// Parameters
		//	- the images are given by four parameters:
		//         - void* ptr;					points to the first pixel's first color data
		//         - int width;					number of pixels in a row
		//         - int height;				number of pixels in a column
		//         - int stride;				length of a row in bytes (containing the padding
		//										bytes at the end of rows)
		//	- in case of functions having both input and output image, their size is equal (except
		//	  the resizing function), so the 'width' and 'height' parameters are common for
		//	  both images. (The stride parameters can differ - so they are given separately.)
		//	- all images and histogram vectors must be allocated (and deallocated after usage)
		//	  by the calling environment
		//	- The ordering of parameters:
		//			- input image (with common size)
		//			- output image
		//			- additional parameters
		//
		// Returning value
		//	- the functions return with error code, that are listed in errors.h file.
		//	  (ERRNO = 0 means: successful operation)
		//
		// Remark: Calling convention
		// - The function's names and calling convention are similar used in Intel's IPPI package

	//--------------------------------------------------------------------------------
	// ### format conversion ###

	ERRORS IMAGEPROCESSING_API ConvertToGrayScale_8bpp(
		int width, int height,
		void* ptr1, int stride1,
		void* ptr2, int stride2);

	ERRORS IMAGEPROCESSING_API ConvertToRGB_24bpp(
		int width, int height,
		void* ptr1, int stride1,
		void* ptr2, int stride2);

	//--------------------------------------------------------------------------------
	// ### misc. utilities ###

	ERRORS IMAGEPROCESSING_API CleanBitmap_24bpp(
		int width, int height,
		void* ptr, int stride);

	ERRORS IMAGEPROCESSING_API DownscaleBitmap(
		int width1, int height1,
		void* ptr1, int stride1,
		int DOWNSCALINGFACTOR,
		void* ptr2, int stride2);

	ERRORS IMAGEPROCESSING_API DownscaleBitmapAvg(
		int width1, int height1,
		void* ptr1, int stride1,
		int DOWNSCALINGFACTOR,
		void* ptr2, int stride2);

	//---------------------------------------------------------------------------------
	// ### thresholding ###

	ERRORS IMAGEPROCESSING_API Threshold_8bpp(
		int width, int height,
		void* ptr1, int stride1,
		void* ptr2, int stride2,
		int threshold);

	ERRORS IMAGEPROCESSING_API Otsu_8bpp(
		int width, int height,
		void* ptr1, int stride1,
		void* ptr2, int stride2,
		int destroyedband);

	//---------------------------------------------------------------------------------
	// ### Segmenting ###

	ERRORS IMAGEPROCESSING_API SegmentImage_8bpp(
		int width, int height,
		void* ptr1, int stride1,
		int* Segmented, int* features,
		int FEATURES0ARRAYLENGTH);

	//---------------------------------------------------------------------------------
	// ### Feature extraction ###

	ERRORS IMAGEPROCESSING_API GetFeaturesOfSelectedTeacher(
		int width, int height,
		void* ptr1, int stride1,
		int* Segmented, int* features0,
		int spotSN, double* Teachers,
		int newitem_index);

	ERRORS IMAGEPROCESSING_API GetFeaturesOfSelectedSpot(
		int width, int height,
		void* ptr1, int stride1,
		int* Segmented, int* features0,
		int spotSN, double* Feature_ptr);

	//-------------------------------------------------------------------------------------------
	// ### Neural network ###

	ERRORS IMAGEPROCESSING_API InitNeuralNetwork(
		double* weights_ptr, int nbOfUnits_1, int nbOfUnits_2, int nbOfUnits_3);

	int IMAGEPROCESSING_API TeachNeuralNetwork(
		double* Features_ptr, double* weights_ptr,
		int nbOfUnits_1, int nbOfUnits_2, int nbOfUnits_3,
		double factor, int bExpectedDecision,
		double* Decisions_ptr);

	int IMAGEPROCESSING_API UseNeuralNetwork(
		double* Features_ptr, double* weights_ptr,
		int nbOfUnits_1, int nbOfUnits_2, int nbOfUnits_3,
		double* Decisions_ptr);

	ERRORS IMAGEPROCESSING_API RecognizeWithNN(
		int width, int height,
		void* ptr_input, int stride_input,
		void* ptr_result, int stride_result,
		int NBOFFEATURES, bool* UsedFeatures_ptr, int* Types_ptr,
		BYTE OurColor_R, BYTE OurColor_G, BYTE OurColor_B,
		int nbOfUnits_1, int nbOfUnits_2, int nbOfUnits_3,
		double* Weights_ptr, double confidence,
		int* Segmented_ptr, int* Features0_ptr, int FEATURES0ARRAYLENGTH);

	//---------------------------------------------------------------------------------
	// ### Errors ###

	void IMAGEPROCESSING_API Get_ErrorString(
		char* str, int strlen, ERRORS errcode );

}


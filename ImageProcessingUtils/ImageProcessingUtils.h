// ImageProcessingUtils.h : main header file for the ImageProcessingUtils DLL
//

#pragma once

#include "ImageProcessing.h"

#include <vcclr.h>

namespace ImageProcessingUtils{
	using namespace System;
	using namespace System::Data;
	using namespace System::Drawing;
	using namespace System::Drawing::Imaging;

	public ref class CImageProcessingUtils
	{
	public:

	//--------------------------------------------------------------------------------
	// ### image format conversion ###

	// ConvertToGrayScale_8bpp
	// Purpose: convert the input bitmap to 8bpp grayscale format bitmap
	//
	// REM: the input and output bitmaps are the same size
	static int ConvertToGrayScale_8bpp(Bitmap^ bmp_input, Bitmap^ bmp_output)
	{
		System::Drawing::Rectangle rect =
			System::Drawing::Rectangle(0, 0, bmp_input->Width, bmp_input->Height);
		BitmapData^ data_input =
			bmp_input->LockBits(rect, System::Drawing::Imaging::ImageLockMode::ReadOnly,
			bmp_input->PixelFormat);
		BitmapData^ data_output =
			bmp_output->LockBits(rect, System::Drawing::Imaging::ImageLockMode::ReadWrite,
			bmp_output->PixelFormat);
		ImageProcessing::ERRORS err = ImageProcessing::ERRNO;

		try
		{
			err = ImageProcessing::ConvertToGrayScale_8bpp(
				data_input->Width, data_input->Height,
				data_input->Scan0.ToPointer(), data_input->Stride,
				data_output->Scan0.ToPointer(), data_output->Stride);
			err = ImageProcessing::ERRNO;
		}
		finally
		{
			bmp_input->UnlockBits(data_input);
			bmp_output->UnlockBits(data_output);
		}
		return (int)err;
	}

	// ConvertToRGB_24bpp
	// Purpose: convert the input bitmap to 24bpp (RGB) bitmap
	//
	// REM: the input and output bitmaps are the same size
	static int ConvertToRGB_24bpp(Bitmap^ bmp_input, Bitmap^ bmp_output)
	{
		System::Drawing::Rectangle rect =
			System::Drawing::Rectangle(0, 0, bmp_input->Width, bmp_input->Height);
		BitmapData^ data_input =
			bmp_input->LockBits(rect, System::Drawing::Imaging::ImageLockMode::ReadOnly,
				bmp_input->PixelFormat);
		BitmapData^ data_output =
			bmp_output->LockBits(rect, System::Drawing::Imaging::ImageLockMode::ReadWrite,
				bmp_output->PixelFormat);
		ImageProcessing::ERRORS err = ImageProcessing::ERRNO;

		try
		{
			err = ImageProcessing::ConvertToRGB_24bpp(
				data_input->Width, data_input->Height,
				data_input->Scan0.ToPointer(), data_input->Stride,
				data_output->Scan0.ToPointer(), data_output->Stride);
			err = ImageProcessing::ERRNO;
		}
		finally
		{
			bmp_input->UnlockBits(data_input);
			bmp_output->UnlockBits(data_output);
		}
		return (int)err;
	}

	//--------------------------------------------------------------------------------
	// ### misc. utilities ###

	// Clean the 24bpp bitmap's pixels
	static int CleanBitmap_24bpp(Bitmap^ bmp)
	{
		ImageProcessing::ERRORS err = ImageProcessing::ERRNO;

		if (bmp == nullptr)
			return  (int)err;

		System::Drawing::Rectangle rect =
			System::Drawing::Rectangle(0, 0, bmp->Width, bmp->Height);
		BitmapData^ data_input =
			bmp->LockBits(rect, System::Drawing::Imaging::ImageLockMode::ReadOnly,
			bmp->PixelFormat);

		try
		{
			err = ImageProcessing::CleanBitmap_24bpp(
				data_input->Width, data_input->Height,
				data_input->Scan0.ToPointer(), data_input->Stride);
			err = ImageProcessing::ERRNO;
		}
		finally
		{
			bmp->UnlockBits(data_input);
		}

		return (int)err;
	}

	// Downscale the input bitmap, with DOWNSCALINGFACTOR factor, keeping the original pixelformat
	static int DownscaleBitmap(Bitmap^ bmp_input, int DOWNSCALINGFACTOR, Bitmap^ bmp_output)
	{
		System::Drawing::Rectangle rect1 =
			System::Drawing::Rectangle(0, 0, bmp_input->Width, bmp_input->Height);
		BitmapData^ data_input =
			bmp_input->LockBits(rect1, System::Drawing::Imaging::ImageLockMode::ReadOnly,
				bmp_input->PixelFormat);
		System::Drawing::Rectangle rect2 =
			System::Drawing::Rectangle(0, 0, bmp_output->Width, bmp_output->Height);
		BitmapData^ data_output =
			bmp_output->LockBits(rect2, System::Drawing::Imaging::ImageLockMode::ReadWrite,
				bmp_output->PixelFormat);
		ImageProcessing::ERRORS err = ImageProcessing::ERRNO;

		try
		{
			err = ImageProcessing::DownscaleBitmap(
				data_input->Width, data_input->Height,
				data_input->Scan0.ToPointer(), data_input->Stride,
				DOWNSCALINGFACTOR,
				data_output->Scan0.ToPointer(), data_output->Stride);
			err = ImageProcessing::ERRNO;
		}
		finally
		{
			bmp_input->UnlockBits(data_input);
			bmp_output->UnlockBits(data_output);
		}
		return (int)err;
	}

	// Downscale the input bitmap, with DOWNSCALINGFACTOR factor, keeping the original pixelformat
	// The output pixel values are computed as averaging within [DOWNSCALINGFACTOR*DOWNSCALINGFACTOR] size input kernels
	static int DownscaleBitmapAvg(Bitmap^ bmp_input, int DOWNSCALINGFACTOR, Bitmap^ bmp_output)
	{
		System::Drawing::Rectangle rect1 =
			System::Drawing::Rectangle(0, 0, bmp_input->Width, bmp_input->Height);
		BitmapData^ data_input =
			bmp_input->LockBits(rect1, System::Drawing::Imaging::ImageLockMode::ReadOnly,
				bmp_input->PixelFormat);
		System::Drawing::Rectangle rect2 =
			System::Drawing::Rectangle(0, 0, bmp_output->Width, bmp_output->Height);
		BitmapData^ data_output =
			bmp_output->LockBits(rect2, System::Drawing::Imaging::ImageLockMode::ReadWrite,
				bmp_output->PixelFormat);
		ImageProcessing::ERRORS err = ImageProcessing::ERRNO;

		try
		{
			err = ImageProcessing::DownscaleBitmapAvg(
				data_input->Width, data_input->Height,
				data_input->Scan0.ToPointer(), data_input->Stride,
				DOWNSCALINGFACTOR,
				data_output->Scan0.ToPointer(), data_output->Stride);
			err = ImageProcessing::ERRNO;
		}
		finally
		{
			bmp_input->UnlockBits(data_input);
			bmp_output->UnlockBits(data_output);
		}
		return (int)err;
	}

	//----------------------------------------------------------------------------------
	// ### Thresholding ###

	// Threshold_8bpp
	// Purpose: Prepare the input image (e.g. for morphological operations),
	//			by thresholding. The resulted pixel values:
	//			0: below the threshold value
	//			255: over the threshold value
	// Parameter:
	//		- 'threshold'  the threshold value 0<=threshold<=255
	static int Threshold_8bpp(Bitmap^ bmp_input, Bitmap^ bmp_output, int threshold)
	{
		System::Drawing::Rectangle rect =
			System::Drawing::Rectangle(0, 0, bmp_input->Width, bmp_input->Height);
		BitmapData^ data_input =
			bmp_input->LockBits(rect, System::Drawing::Imaging::ImageLockMode::ReadOnly,
				bmp_input->PixelFormat);
		BitmapData^ data_output =
			bmp_output->LockBits(rect, System::Drawing::Imaging::ImageLockMode::ReadWrite,
				bmp_output->PixelFormat);
		ImageProcessing::ERRORS err = ImageProcessing::ERRNO;

		try
		{
			err = ImageProcessing::Threshold_8bpp(
				data_input->Width, data_input->Height,
				data_input->Scan0.ToPointer(), data_input->Stride,
				data_output->Scan0.ToPointer(), data_output->Stride,
				threshold);
			err = ImageProcessing::ERRNO;
		}
		finally
		{
			bmp_input->UnlockBits(data_input);
			bmp_output->UnlockBits(data_output);
		}
		return (int)err;
	}

	// Otsu_8bpp
	// Purpose: Decide the optimal thresholding value and produce bilevel image:
	//			0: below the threshold value
	//			255: over the threshold value
	//
	static int Otsu_8bpp(Bitmap^ bmp_input, Bitmap^ bmp_output, int destroyedband)
	{
		System::Drawing::Rectangle rect =
			System::Drawing::Rectangle(0, 0, bmp_input->Width, bmp_input->Height);
		BitmapData^ data_input =
			bmp_input->LockBits(rect, System::Drawing::Imaging::ImageLockMode::ReadOnly,
				bmp_input->PixelFormat);
		BitmapData^ data_output =
			bmp_output->LockBits(rect, System::Drawing::Imaging::ImageLockMode::ReadWrite,
				bmp_output->PixelFormat);
		ImageProcessing::ERRORS err = ImageProcessing::ERRNO;

		try
		{
			err = ImageProcessing::Otsu_8bpp(
				data_input->Width, data_input->Height,
				data_input->Scan0.ToPointer(), data_input->Stride,
				data_output->Scan0.ToPointer(), data_output->Stride,
				destroyedband);
			err = ImageProcessing::ERRNO;
		}
		finally
		{
			bmp_input->UnlockBits(data_input);
			bmp_output->UnlockBits(data_output);
		}
		return (int)err;
	}

	//-------------------------------------------------------------------------------------------
	// ### Segmenting ###

	static int SegmentImage_8bpp(Bitmap^ Preprocessed_Bitmap, array<int>^ Segmented, array<int>^ Features0,
		int FEATURES0ARRAYLENGTH)
	{
		System::Drawing::Rectangle rect =
			System::Drawing::Rectangle(0, 0, Preprocessed_Bitmap->Width, Preprocessed_Bitmap->Height);
		BitmapData^ data_input =
			Preprocessed_Bitmap->LockBits(rect, System::Drawing::Imaging::ImageLockMode::ReadOnly,
				Preprocessed_Bitmap->PixelFormat);

		pin_ptr<int> Segmented_ptr = &Segmented[0];
		pin_ptr<int> Features_ptr = &Features0[0];

		ImageProcessing::ERRORS err = ImageProcessing::ERRNO;

		try
		{
			err = ImageProcessing::SegmentImage_8bpp(data_input->Width, data_input->Height,
				data_input->Scan0.ToPointer(), data_input->Stride,
				Segmented_ptr, Features_ptr, FEATURES0ARRAYLENGTH);
			err = ImageProcessing::ERRNO;

		}
		finally
		{
			Preprocessed_Bitmap->UnlockBits(data_input);
		}

		return (int)err;
	}

	//-------------------------------------------------------------------------------------------
	// ### Feature extraction ###

	static int GetFeaturesOfSelectedTeacher(Bitmap^ bmp_input, array<int>^ Segmented, array<int>^ Features0,
		int spotSN, array<double>^ Teachers, int newitem_index)
	{
		System::Drawing::Rectangle rect =
			System::Drawing::Rectangle(0, 0, bmp_input->Width, bmp_input->Height);
		BitmapData^ data_input =
			bmp_input->LockBits(rect, System::Drawing::Imaging::ImageLockMode::ReadOnly,
				bmp_input->PixelFormat);

		pin_ptr<int> Segmented_ptr = &Segmented[0];
		pin_ptr<int> Features_ptr = &Features0[0];
		pin_ptr<double> Teachers_ptr = &Teachers[0];

		ImageProcessing::ERRORS err = ImageProcessing::ERRNO;

		try
		{
			err = ImageProcessing::GetFeaturesOfSelectedTeacher(data_input->Width, data_input->Height,
				data_input->Scan0.ToPointer(), data_input->Stride,
				Segmented_ptr, Features_ptr, spotSN,
				Teachers_ptr, newitem_index);
			err = ImageProcessing::ERRNO;

		}
		finally
		{
			bmp_input->UnlockBits(data_input);
		}

		return (int)err;
	}

	//-------------------------------------------------------------------------------------------
	// ### Neural network ###

	static int InitNeuralNetwork(array<double>^ Weights,
		int nbOfUnits_1, int nbOfUnits_2, int nbOfUnits_3)
	{
		pin_ptr<double> Weights_ptr = &Weights[0];

		ImageProcessing::ERRORS err = ImageProcessing::ERRNO;

		try
		{
			err = ImageProcessing::InitNeuralNetwork(
				Weights_ptr, nbOfUnits_1, nbOfUnits_2, nbOfUnits_3);
			err = ImageProcessing::ERRNO;
		}
		finally
		{
			// nothing to do ...
		}

		return (int)err;
	}

	static int TeachNeuralNetwork(array<double>^ Features, array<double>^ Weights,
		int nbOfUnits_1, int nbOfUnits_2, int nbOfUnits_3,
		double factor, int bExpectedDecision, array<double>^ Decisions)
	{
		pin_ptr<double> Features_ptr = &Features[0];
		pin_ptr<double> Weights_ptr = &Weights[0];
		pin_ptr<double> Decisions_ptr = &Decisions[0];

		int decision = 0;

		try
		{
			decision = ImageProcessing::TeachNeuralNetwork(Features_ptr, Weights_ptr,
				nbOfUnits_1, nbOfUnits_2, nbOfUnits_3,
				factor, bExpectedDecision, Decisions_ptr);
			decision = 0;
		}
		finally
		{
			// nothing to do ...
		}

		return decision;
	}

	static int UseNeuralNetwork(array<double>^ Features, array<double>^ Weights,
		int nbOfUnits_1, int nbOfUnits_2, int nbOfUnits_3, array<double>^ Decisions)
	{
		pin_ptr<double> Features_ptr = &Features[0];
		pin_ptr<double> Weights_ptr = &Weights[0];
		pin_ptr<double> Decisions_ptr = &Decisions[0];

		int decision = 0;

		try
		{
			decision = ImageProcessing::UseNeuralNetwork(Features_ptr, Weights_ptr,
				nbOfUnits_1, nbOfUnits_2, nbOfUnits_3, Decisions_ptr);
			decision = 0;
		}
		finally
		{
			// nothing to do ...
		}

		return decision;
	}

	// decide for all segmented spots of input image, if it is similar to one of the teached types
	// if YES, set the proper pixel on result bitmap to 'OWN' or 'OTHER'
	static int RecognizeWithNN(Bitmap^ bmp_input, Bitmap^ bmp_result,
		int NBOFFEATURES, array<bool>^ UsedFeatures, array<int>^ Types,
		BYTE OurColor_R, BYTE OurColor_G, BYTE OurColor_B,
		int nbOfUnits_1, int nbOfUnits_2, int nbOfUnits_3,
		array<double>^ Weights, double confidence,
		array<int>^ Segmented, array<int>^ Features0, int FEATURES0ARRAYLENGTH)
	{
		System::Drawing::Rectangle rect =
			System::Drawing::Rectangle(0, 0, bmp_input->Width, bmp_input->Height);
		BitmapData^ data_input =
			bmp_input->LockBits(rect, System::Drawing::Imaging::ImageLockMode::ReadOnly,
				bmp_input->PixelFormat);
		BitmapData^ data_result =
			bmp_result->LockBits(rect, System::Drawing::Imaging::ImageLockMode::ReadWrite,
				bmp_result->PixelFormat);

		pin_ptr<bool> UsedFeatures_ptr = &UsedFeatures[0];
		pin_ptr<int> Types_ptr = &Types[0];
		pin_ptr<double> Weights_ptr = &Weights[0];
		pin_ptr<int> Segmented_ptr = &Segmented[0];
		pin_ptr<int> Features0_ptr = &Features0[0];

		ImageProcessing::ERRORS err = ImageProcessing::ERRNO;

		try
		{
			err = ImageProcessing::RecognizeWithNN(
				data_input->Width, data_input->Height,
				data_input->Scan0.ToPointer(), data_input->Stride,
				data_result->Scan0.ToPointer(), data_result->Stride,
				NBOFFEATURES, UsedFeatures_ptr, Types_ptr,
				OurColor_R, OurColor_G, OurColor_B,
				nbOfUnits_1, nbOfUnits_2, nbOfUnits_3,
				Weights_ptr, confidence,
				Segmented_ptr, Features0_ptr, FEATURES0ARRAYLENGTH);
			err = ImageProcessing::ERRNO;
		}
		finally
		{
			bmp_input->UnlockBits(data_input);
			bmp_result->UnlockBits(data_result);
		}

		return (int)err;
	}

	//-------------------------------------------------------------------------------------------
	};

}


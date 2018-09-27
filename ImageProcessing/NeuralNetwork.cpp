// Neuralnetwork.cpp : using the neural network
//

#include "stdafx.h"

#include "ImageProcessing.h"

#include <math.h>

//------------------------------------------------------------------------------

namespace ImageProcessing
{

	//--------------------------------------------------------------------------------
	// ### Using neural network ###

	int IMAGEPROCESSING_API UseNeuralNetwork(
			double* features_ptr, double* weights_ptr,
			int nbOfUnits_1, int nbOfUnits_2, int nbOfUnits_3,
			double* decisions_ptr)
	{
		double b1 = 0.35;	// bias - only for test
		double b2 = 0.6;

		int decision = 0;

		// apply the weights, according to the 3-layers neural network
		double* layer_1_ptr = new double[nbOfUnits_1];
		memset(layer_1_ptr, 0, nbOfUnits_1*sizeof(double));
		double* layer_2_ptr = new double[nbOfUnits_2];
		memset(layer_2_ptr, 0, nbOfUnits_2*sizeof(double));
		double* layer_3_ptr = new double[nbOfUnits_3];
		memset(layer_3_ptr, 0, nbOfUnits_3 * sizeof(double));

		// Input layer
		for ( int ii=0; ii<nbOfUnits_1; ii++)
		{
			layer_1_ptr[ii] = weights_ptr[ii]* features_ptr[ii];
		}

		// Hidden layer
		for ( int ii=0; ii<nbOfUnits_2; ii++)
		{
			int index1 = (ii+1)*nbOfUnits_1;
			for ( int jj=0; jj<nbOfUnits_1; jj++ )
			{
				layer_2_ptr[ii] += weights_ptr[index1+jj]*layer_1_ptr[jj];
			}
			layer_2_ptr[ii] += b1;
			// apply the Sigmoid sqashing (activation function):
			// Out = 1/(1+e^(-In))
			layer_2_ptr[ii] = 1.0/(1.0 + exp(-layer_2_ptr[ii]));
		}

		// Output layer
		for (int ii = 0; ii<nbOfUnits_3; ii++)
		{
			int index2 = (nbOfUnits_2 + 1)*nbOfUnits_1 + ii*nbOfUnits_2;
			for (int jj = 0; jj<nbOfUnits_2; jj++)
			{
				layer_3_ptr[ii] += weights_ptr[index2 + jj] * layer_2_ptr[jj];
			}
			layer_3_ptr[ii] += b2;
			// apply the Sigmoid sqashing (activation function):
			// Out = 1/(1+e^(-In))
			decisions_ptr[ii] = 1.0 / (1.0 + exp(-layer_3_ptr[ii]));
		}

		// decide the max. output (index of max. element of decisions vector)
		double maxvalue = 0.0;
		for (int ii = 0; ii < nbOfUnits_3; ii++)
		{
			if (decisions_ptr[ii] > maxvalue)
			{
				maxvalue = decisions_ptr[ii];
				decision = ii;
			}
		}

		delete [] layer_1_ptr;
		delete [] layer_2_ptr;
		delete [] layer_3_ptr;

		return decision;	// REM: the correspondent weight is: decisions_ptr[decision]
	}

	ERRORS IMAGEPROCESSING_API RecognizeWithNN(
		int width, int height,
		void* ptr_input, int stride_input,
		void* ptr_result, int stride_result,
		int NBOFFEATURES, bool* UsedFeatures_ptr, int* Types_ptr,
		BYTE OurColor_R, BYTE OurColor_G, BYTE OurColor_B,
		int nbOfUnits_1, int nbOfUnits_2, int nbOfUnits_3,
		double* Weights_ptr, double confidence,
		int* Segmented_ptr, int* Features0_ptr, int FEATURES0ARRAYLENGTH)
	{
		int MINAREA = 10;	// lower limit for analysing a spot, in order to filter out the too small objects

		// 1. check the parameters
		if (width <= 0 || height <= 0 || stride_input<3 * width || stride_result<3 * width)
			return ERR_IMAGESIZE;			// image size error
		if (ptr_input == NULL || ptr_result == NULL)
			return ERR_IMAGEPOINTER;		// image pointer error
		if (NBOFFEATURES == 0 || Types_ptr == NULL || Weights_ptr == NULL)
			return ERR_PARAMETER;			// parameter error
		if (nbOfUnits_1 == 0 || nbOfUnits_2 == 0 || nbOfUnits_3 == 0)
			return ERR_PARAMETER;			// parameter error

		int step_input = 0;
		if (stride_input >= 4 * width)
			step_input = 4;
		else  if (stride_input >= 3 * width)
			step_input = 3;
		else return ERR_IMAGEFORMAT;		// image format error

		// init. the result image
		unsigned char* ptr_input_0 = (unsigned char*)ptr_input;
		unsigned char* ptr_result_0 = (unsigned char*)ptr_result;
		memset(ptr_result_0, 0, height*stride_result);

		double* features_ptr = new double[100000];	// NBOFFEATURES + COUNTEROFVETREXPOSITIONS + list of vertex positions
		double* decisions_ptr = new double[nbOfUnits_3];

		// process the segmented spots one after another
		int index = 1;
		while (6 * index < FEATURES0ARRAYLENGTH && Features0_ptr[6 * index + 1]>0)
		{
			features_ptr[4] = Features0_ptr[6 * index + 1];		// area
			if (Features0_ptr[6 * index + 1] <= MINAREA)
			{
				index++;
				continue;						// get rid of too small objects
			}

			// extract all the features belonging to the index-th spot
			memset(features_ptr, 0, 100000 * sizeof(double));
			GetFeaturesOfSelectedSpot(
				width, height,
				ptr_input, stride_input,
				Segmented_ptr, Features0_ptr,
				index, features_ptr);

			// keep only the used features
			for (int kk = 0; kk < NBOFFEATURES; kk++)
			{
				if (UsedFeatures_ptr[kk] == false)
					features_ptr[kk] = 0.0;
			}

			// get the decision from neural network
			memset(decisions_ptr, 0, nbOfUnits_3 * sizeof(double));
			int decision = UseNeuralNetwork(features_ptr, Weights_ptr,
				nbOfUnits_1, nbOfUnits_2, nbOfUnits_3, decisions_ptr);
			if (decisions_ptr[decision] < confidence)
			{
				index++;
				continue;	// the recognition is not acceptable, keep it as 'not our object'
			}
			if (Types_ptr[decision] == 0)
			{
				index++;
				continue;	// recognized as 'not our object'
			}

			// it is our object, draw the spot to the result image
			for (int jjjj = Features0_ptr[6 * index + 3]; jjjj <= Features0_ptr[6 * index + 5]; jjjj++)
			{
				for (int iiii = Features0_ptr[6 * index + 2]; iiii <= Features0_ptr[6 * index + 4]; iiii++)
				{
					if (Segmented_ptr[iiii + jjjj*width] != index)
						continue;	// within the container rectangle, but not belongign to the current spot
					ptr_result_0[iiii*step_input + 0 + jjjj*stride_result] = OurColor_B;
					ptr_result_0[iiii*step_input + 1 + jjjj*stride_result] = OurColor_G;
					ptr_result_0[iiii*step_input + 2 + jjjj*stride_result] = OurColor_R;
				}
			}

			index++;
		}

		delete[] features_ptr;
		delete[] decisions_ptr;

		return ERRNO;
	}

	//--------------------------------------------------------------------------------

}



// Teacher.cpp : functions for teaching the neural network
//

#include "stdafx.h"

#include "ImageProcessing.h"

#include <math.h>

//------------------------------------------------------------------------------

namespace ImageProcessing
{

	//--------------------------------------------------------------------------------
	// ### Teaching, neural network ###

	// REM: the number of Units in layers:
	//		nbOfUnits_1:		in INPUT layer
	//		nbOfUnits_2:		in HIDDEN layer
	//		nbOfUnits_3:		in OUTPUT layer
	ERRORS IMAGEPROCESSING_API InitNeuralNetwork(
			double* weights_ptr, int nbOfUnits_1, int nbOfUnits_2, int nbOfUnits_3 )
	{
		double range_min = -1.0;
		double range_max = 1.0;
		// Init the weights of NN with small random numbers, in {-1,+1} interval
		int nbofWeights = nbOfUnits_1 +	
						  nbOfUnits_1*nbOfUnits_2 +
						  nbOfUnits_2*nbOfUnits_3;

		// set all weights of INPUT layer to 1.0 (now: constants)
		for ( int ii=0; ii<nbOfUnits_1; ii++ )
			weights_ptr[ii] = 1.0;

		// set the other layers to small random value
		for ( int ii=nbOfUnits_1; ii<nbofWeights; ii++ )
		{
			double newvalue = (double)rand() / (RAND_MAX + 1) * (range_max - range_min)+
							range_min;

			weights_ptr[ii] = newvalue;
		}

		return ERRNO;
	}

	int IMAGEPROCESSING_API TeachNeuralNetwork(
			double* features_ptr, double* weights_ptr,
			int nbOfUnits_1, int nbOfUnits_2, int nbOfUnits_3,
			double factor, int ExpectedDecision,
			double* Decisions_ptr)
	{
		double b1 = 0.35;	// bias - only for test
		double b2 = 0.6;

		double* Target_ptr = new double[nbOfUnits_3];
		for (int ii = 0; ii < nbOfUnits_3; ii++)
		{
			Target_ptr[ii] = (ExpectedDecision == ii + 1) ? 1.0 : 0.0;	// expected decision
		}

		// apply the weights, according to the 3-layers neural network
		int decision = 0;
		double* layer_1_ptr = new double[nbOfUnits_1];
		memset(layer_1_ptr, 0, nbOfUnits_1 * sizeof(double));
		double* layer_2_ptr = new double[nbOfUnits_2];
		memset(layer_2_ptr, 0, nbOfUnits_2 * sizeof(double));
		double* layer_3_ptr = new double[nbOfUnits_3];
		memset(layer_3_ptr, 0, nbOfUnits_3 * sizeof(double));

		// store the input weights for using at back propagation
		int nbofweights = nbOfUnits_1 + nbOfUnits_1*nbOfUnits_2 + nbOfUnits_2*nbOfUnits_3;
		double* tmp_weights_ptr = new double[nbofweights];
		memcpy(tmp_weights_ptr, weights_ptr, nbofweights * sizeof(double));

		// I. Get the decisions from NN, with current weights
		// Input layer
		for (int ii = 0; ii<nbOfUnits_1; ii++)
		{
			layer_1_ptr[ii] = weights_ptr[ii] * features_ptr[ii];
		}

		// Hidden layer
		for (int ii = 0; ii<nbOfUnits_2; ii++)
		{
			int index1 = (ii + 1)*nbOfUnits_1;
			for (int jj = 0; jj<nbOfUnits_1; jj++)
			{
				layer_2_ptr[ii] += weights_ptr[index1 + jj] * layer_1_ptr[jj];
			}
			layer_2_ptr[ii] += b1;
			// apply the Sigmoid sqashing (activation function):
			// Out = 1/(1+e^(-In))
			layer_2_ptr[ii] = 1.0 / (1.0 + exp(-layer_2_ptr[ii]));
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
			layer_3_ptr[ii] = 1.0 / (1.0 + exp(-layer_3_ptr[ii]));
		}

		// II. backward pass
		// Change the weights for output layer
		for (int jj = 0; jj<nbOfUnits_2; jj++)
		{
			for (int mm = 0; mm < nbOfUnits_3; mm++)
			{
				double currentdecision = layer_3_ptr[mm];
				double delta3 = currentdecision*(1.0 - currentdecision)*(Target_ptr[mm] - layer_3_ptr[mm]);
				int index3 = (nbOfUnits_2 + 1)*nbOfUnits_1 + mm*nbOfUnits_2;
				weights_ptr[index3 + jj] += factor*delta3*layer_2_ptr[jj];
			}
		}

		for (int jj = 0; jj < nbOfUnits_2; jj++)
		{
			for (int kk = 0; kk < nbOfUnits_1; kk++)
			{
				double value = 0.0;
				for (int mm = 0; mm < nbOfUnits_3; mm++)
				{
					int index4 = (nbOfUnits_2 + 1)*nbOfUnits_1 + mm*nbOfUnits_2 + jj;
					value += -layer_3_ptr[mm] * (1.0 - layer_3_ptr[mm])*(Target_ptr[mm] - layer_3_ptr[mm])*tmp_weights_ptr[index4];
				}
				value *= layer_2_ptr[jj] * (1.0 - layer_2_ptr[jj])*layer_1_ptr[kk];
				weights_ptr[(jj + 1)*nbOfUnits_1 + kk] -= factor*value;
			}
		}

		// get the decision with the corrected NN 
		// Input layer
		memset(layer_1_ptr, 0, nbOfUnits_1 * sizeof(double));
		memset(layer_2_ptr, 0, nbOfUnits_2 * sizeof(double));
		memset(layer_3_ptr, 0, nbOfUnits_3 * sizeof(double));
		for (int ii = 0; ii<nbOfUnits_1; ii++)
		{
			layer_1_ptr[ii] = weights_ptr[ii] * features_ptr[ii];
		}

		// Hidden layer
		for (int ii = 0; ii<nbOfUnits_2; ii++)
		{
			int index1 = (ii + 1)*nbOfUnits_1;
			for (int jj = 0; jj<nbOfUnits_1; jj++)
			{
				layer_2_ptr[ii] += weights_ptr[index1 + jj] * layer_1_ptr[jj];
			}
			layer_2_ptr[ii] += b1;
			// apply the Sigmoid sqashing (activation function):
			// Out = 1/(1+e^(-In))
			layer_2_ptr[ii] = 1.0 / (1.0 + exp(-layer_2_ptr[ii]));
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
			layer_3_ptr[ii] = 1.0 / (1.0 + exp(-layer_3_ptr[ii]));
		}

		// decide the max. output (index of max. element of decisions vector)
		double maxvalue = 0.0;
		for (int ii = 0; ii < nbOfUnits_3; ii++)
		{
			Decisions_ptr[ii] = layer_3_ptr[ii];
			if (Decisions_ptr[ii] > maxvalue)
			{
				maxvalue = Decisions_ptr[ii];
				decision = ii;
			}
		}

		delete[] layer_1_ptr;
		delete[] layer_2_ptr;
		delete[] layer_3_ptr;
		delete[] Target_ptr;
		delete[] tmp_weights_ptr;

		return decision;	// REM: the correspondent weight is: decisions_ptr[decision]

	}

	//--------------------------------------------------------------------------------

}



// Errors.cpp : return with error string


#include "stdafx.h"

#include "ImageProcessing.h" 

#include <string.h>

namespace ImageProcessing
{
	void IMAGEPROCESSING_API Get_ErrorString(
		char* str, int strlen, ERRORS errcode )
	{
		switch ( errcode )
		{
		case ERRNO:
			strcpy_s(str, strlen, "No error");
			break;
		case ERR_IMAGESIZE:
			strcpy_s(str, strlen, "Image size error");
			break;
		case ERR_IMAGEPOINTER:
			strcpy_s(str, strlen, "Image pointer error");
			break;
		case ERR_IMAGEFORMAT:
			strcpy_s(str, strlen, "Image format error");
			break;
		case ERR_HISTPOINTER:
			strcpy_s(str, strlen, "Histogram pointer error");
			break;
		case ERR_MEMORY:
			strcpy_s(str, strlen, "Memory allocation error");
			break;
		case ERR_PARAMETER:
			strcpy_s(str, strlen, "Parameter error");
			break;
		default:
			strcpy_s(str, strlen, "Undefined error");
			break;
		}
	}

}
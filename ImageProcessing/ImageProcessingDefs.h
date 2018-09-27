#pragma once
#ifdef IMAGEPROCESSING_EXPORTS
#define IMAGEPROCESSING_API __declspec(dllexport)
#else
//#ifndef IMAGEPROCESSING_LIB
#define IMAGEPROCESSING_API __declspec(dllimport)
//#else
//#define IMAGEPROCESSING_API
//#endif
#endif

// Error codes

namespace ImageProcessing
{
	// error code definitions
	enum ERRORS
	{
		ERRNO = 0,				// no error
		ERR_IMAGESIZE,			// image size error
		ERR_IMAGEPOINTER,		// image pointer error
		ERR_IMAGEFORMAT,		// image format error
		ERR_HISTPOINTER,		// histogram pointer error
		ERR_MEMORY,				// memory allocation error
		ERR_PARAMETER,			// (general) parameter error
		ERR_NOT_OUR_IMAGE,		// "Not Our Image" (it's structure is not acceptable)
		ERR_NOT_GOOD_ENOUGH		// "Not Good Enough" (the necessary details are not identifyable)
	};
}
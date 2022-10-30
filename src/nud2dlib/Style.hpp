#pragma once

#include "Context.hpp"

extern "C"
{
	D2DLIB_API HANDLE CreateStrokeStyle(HANDLE ctx, FLOAT* dashes, UINT dashCount, FLOAT dashOffset,
		D2D1_CAP_STYLE startCap = D2D1_CAP_STYLE::D2D1_CAP_STYLE_FLAT, D2D1_CAP_STYLE endCap = D2D1_CAP_STYLE::D2D1_CAP_STYLE_FLAT);
}

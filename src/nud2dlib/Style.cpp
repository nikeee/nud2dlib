#include "Style.hpp"

D2DLIB_API HANDLE CreateStrokeStyle(HANDLE ctx, FLOAT* dashes, UINT dashCount, FLOAT dashOffset,
	D2D1_CAP_STYLE startCap, D2D1_CAP_STYLE endCap)
{
	RetrieveContext(ctx);

	ID2D1StrokeStyle* strokeStyle = NULL;

	HRESULT hr = context->factory->CreateStrokeStyle(
		D2D1::StrokeStyleProperties(
			D2D1_CAP_STYLE_FLAT,
			D2D1_CAP_STYLE_FLAT,
			D2D1_CAP_STYLE_ROUND,
			D2D1_LINE_JOIN_MITER,
			10.0f,
			D2D1_DASH_STYLE_CUSTOM,
			0.0f),
		dashes,
		dashCount,
		&strokeStyle
	);

	if (!SUCCEEDED(hr)) {
		return NULL;
	}

	return (HANDLE)strokeStyle;
}

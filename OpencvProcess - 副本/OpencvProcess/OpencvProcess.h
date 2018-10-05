#pragma once
#include "stdafx.h"
extern "C" __declspec(dllexport) void read(BYTE *dd, int height, int wheight);
extern "C" __declspec(dllexport) float calibration(BYTE *dd, int height, int wheight, int cornerw, int cornerh, float length);
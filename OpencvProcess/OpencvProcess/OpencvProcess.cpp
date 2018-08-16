// OpencvProcess.cpp: 定义 DLL 应用程序的导出函数。
//

#include "stdafx.h"
#include <opencv2/opencv.hpp>
#include <opencv2/imgproc/imgproc.hpp>
#include <iostream>
#include <vector>
#include <string>

//-----------------------------------【命名空间声明部分】---------------------------------------
//		描写叙述：包括程序所使用的命名空间
//----------------------------------------------------------------------------------------------- 
using namespace cv;
using namespace std;

extern "C" __declspec(dllexport) void read(BYTE *dd,int height,int wheight)
{
	Mat test =Mat(wheight,height,CV_8UC1,dd);
	//Mat test= imread("test.bmp");
	imwrite("test2.bmp", test);
	return;
}
// OpencvProcess.cpp: 定义 DLL 应用程序的导出函数。
//

#include "stdafx.h"
#include <opencv2/opencv.hpp>
#include <opencv2/imgproc/imgproc.hpp>
#include <iostream>
#include <vector>
#include <string>
#include<Math.h>

//-----------------------------------【命名空间声明部分】---------------------------------------
//		描写叙述：包括程序所使用的命名空间
//----------------------------------------------------------------------------------------------- 
using namespace cv;
using namespace std;

extern "C" __declspec(dllexport) void read(BYTE *dd, int height, int wheight)
{
	Mat test = Mat(wheight, height, CV_8UC1, dd);
	//Mat test= imread("test.bmp");
	imwrite("test2.bmp", test);
	return;
}
extern "C" __declspec(dllexport) float calibration(BYTE *dd,int height,int wheight,int cornerw,int cornerh,double length)
{
	Mat srcImage =Mat(wheight,height,CV_8UC1,dd);
	Size boardSize(cornerw, cornerh);//棋盘格大小
	double worldLength = length;//世界距离20mm
	double pixPerMm;
	vector<Point2f> cornerBuff;
	threshold(srcImage, srcImage, 100, 255, CV_THRESH_BINARY_INV);
	bool found = findChessboardCorners(srcImage, boardSize, cornerBuff);
	if (found)
	{
		Mat srcImage1;
		cvtColor(srcImage, srcImage1, CV_RGB2GRAY);
		cornerSubPix(srcImage1, cornerBuff, Size(5, 5), Size(-1, -1), TermCriteria(CV_TERMCRIT_EPS + CV_TERMCRIT_ITER, 30, 0.1));
		Point2f pa = cornerBuff[cornerBuff.size() / 2];
		Point2f pb = cornerBuff[cornerBuff.size() / 2 + 1];
		Point2f pc = cornerBuff[cornerBuff.size() / 2 + 2];
		double L1 = sqrt(pa.y - pb.y)*(pa.y - pb.y) + (pa.x - pb.x)*(pa.x - pb.x);
		double L2 = sqrt(pc.y - pb.y)*(pc.y - pb.y) + (pc.x - pb.x)*(pc.x - pb.x);
		double L;
		if (L1 > L2)
			L = L2;
		else
			L = L1;
		pixPerMm = L / worldLength;
		//printf("%f", pixPerMm);


		return pixPerMm;
	}
	else
	{
		return -1;
	}
	
	//imwrite("test2.bmp", test);
	//return;
}
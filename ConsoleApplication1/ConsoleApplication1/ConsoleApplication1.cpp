// ConsoleApplication1.cpp: 定义控制台应用程序的入口点。
//
#include "stdafx.h"
#include <opencv2/opencv.hpp>
#include <opencv2/imgproc/imgproc.hpp>
#include <iostream>
#include <vector>

using namespace cv;
using namespace std;
void unevenLightCompensate(Mat &image, int blockSize)
{
	if (image.channels() == 3) cvtColor(image, image, 7);
	//double average = mean(image)[0];
	int rows_new = ceil(double(image.rows) / double(blockSize));
	int cols_new = ceil(double(image.cols) / double(blockSize));
	Mat blockImage;
	blockImage = Mat::zeros(rows_new, cols_new, CV_32FC1);
	double temaver = 0;
	for (int i = 0; i < rows_new; i++)
	{
		for (int j = 0; j < cols_new; j++)
		{
			int rowmin = i * blockSize;
			int rowmax = (i + 1)*blockSize;
			if (rowmax > image.rows) rowmax = image.rows;
			int colmin = j * blockSize;
			int colmax = (j + 1)*blockSize;
			if (colmax > image.cols) colmax = image.cols;
			Mat imageROI = image(Range(rowmin, rowmax), Range(colmin, colmax));
			temaver = mean(imageROI)[0];
			blockImage.at<float>(i, j) = temaver;
		}
	}
	blockImage = blockImage -temaver;
	Mat blockImage2;
	resize(blockImage, blockImage2, image.size(), (0, 0), (0, 0), INTER_CUBIC);
	Mat image2;
	image.convertTo(image2, CV_32FC1);
	Mat dst = image2 - blockImage2;
	dst.convertTo(image, CV_8UC1);
}

void RemoveSmallRegion(Mat &Src, Mat &Dst, int AreaLimit, int NeihborMode)
{
	int RemoveCount = 0;
	//新建一幅标签图像初始化为0像素点，为了记录每个像素点检验状态的标签，0代表未检查，1代表正在检查,2代表检查不合格（需要反转颜色），3代表检查合格或不需检查 
	//初始化的图像全部为0，未检查
	Mat PointLabel = Mat::zeros(Src.size(), CV_8UC1);
	//去除小连通区域的白色点

	//cout << "去除小连通域.";
	for (int i = 0; i < Src.rows; i++)
	{
		for (int j = 0; j < Src.cols; j++)
		{
			if (Src.at<uchar>(i, j) < 10)
			{
				PointLabel.at<uchar>(i, j) = 3;//将背景黑色点标记为合格，像素为3
			}
		}
	}




	vector<Point2i>NeihborPos;//将邻域压进容器
	NeihborPos.push_back(Point2i(-1, 0));
	NeihborPos.push_back(Point2i(1, 0));
	NeihborPos.push_back(Point2i(0, -1));
	NeihborPos.push_back(Point2i(0, 1));
	if (NeihborMode == 1)
	{
		//cout << "Neighbor mode: 8邻域." << endl;
		NeihborPos.push_back(Point2i(-1, -1));
		NeihborPos.push_back(Point2i(-1, 1));
		NeihborPos.push_back(Point2i(1, -1));
		NeihborPos.push_back(Point2i(1, 1));
	}
	//else cout << "Neighbor mode: 4邻域." << endl;
	int NeihborCount = 4 + 4 * NeihborMode;
	int CurrX = 0, CurrY = 0;
	//开始检测
	for (int i = 0; i < Src.rows; i++)
	{
		for (int j = 0; j < Src.cols; j++)
		{
			if (PointLabel.at<uchar>(i, j) == 0)//标签图像像素点为0，表示还未检查的不合格点
			{   //开始检查
				vector<Point2i>GrowBuffer;//记录检查像素点的个数
				GrowBuffer.push_back(Point2i(j, i));
				PointLabel.at<uchar>(i, j) = 1;//标记为正在检查
				int CheckResult = 0;


				for (int z = 0; z < GrowBuffer.size(); z++)
				{
					for (int q = 0; q < NeihborCount; q++)
					{
						CurrX = GrowBuffer.at(z).x + NeihborPos.at(q).x;
						CurrY = GrowBuffer.at(z).y + NeihborPos.at(q).y;
						if (CurrX >= 0 && CurrX < Src.cols&&CurrY >= 0 && CurrY < Src.rows)  //防止越界  
						{
							if (PointLabel.at<uchar>(CurrY, CurrX) == 0)
							{
								GrowBuffer.push_back(Point2i(CurrX, CurrY));  //邻域点加入buffer  
								PointLabel.at<uchar>(CurrY, CurrX) = 1;           //更新邻域点的检查标签，避免重复检查  
							}
						}
					}
				}
				if (GrowBuffer.size() > AreaLimit) //判断结果（是否超出限定的大小），1为未超出，2为超出  
					CheckResult = 2;
				else
				{
					CheckResult = 1;
					RemoveCount++;//记录有多少区域被去除
				}


				for (int z = 0; z < GrowBuffer.size(); z++)
				{
					CurrX = GrowBuffer.at(z).x;
					CurrY = GrowBuffer.at(z).y;
					PointLabel.at<uchar>(CurrY, CurrX) += CheckResult;//标记不合格的像素点，像素值为2
				}
				//********结束该点处的检查**********  


			}
		}


	}

	//CheckMode = 255 * (1 - CheckMode);
	//开始反转面积过小的区域  
	for (int i = 0; i < Src.rows; ++i)
	{
		for (int j = 0; j < Src.cols; ++j)
		{
			if (PointLabel.at<uchar>(i, j) == 2)
			{
				Dst.at<uchar>(i, j) = 0;
			}
			else if (PointLabel.at<uchar>(i, j) == 3)
			{
				Dst.at<uchar>(i, j) = Src.at<uchar>(i, j);

			}
		}
	}
}
	bool IsLongContour(vector<Point> &contour, int maxLength)
	{
		if (contour.size() >= maxLength)
			return true;
		return false;
	}

	bool IsAreaContour(vector<Point> &contour, double maxArea)
	{
		Point2f center;
		float radius;
		minEnclosingCircle(contour, center, radius);
		//circle(src, center, radius, Scalar(0, 255, 255), 2);
		if (radius >= maxArea)
			return true;
		return false;
	}

	bool IsBlackPoint(Mat &src, Point possiblePoint, int maxGray)
	{
		for (int i = -2; i <= 2; i++)
		{
			for (int j = -2; j <= 2; j++)
			{
				if (src.at<uchar>(Point(possiblePoint.x + i, possiblePoint.y + j)) <= maxGray)
					return true;
			}
		}
		return false;
	}
	bool IsBlackAreaContour(Mat &src, vector<Point> &contour, int maxGray)
	{
		for (int i = 0; i < contour.size(); i += (contour.size() - 1) / 3)
		{
			if (IsBlackPoint(src, contour[i], maxGray))
				return true;
		}
		return false;
	}
	vector<vector<Point>> SelectContours(Mat &src, vector<vector<Point>> &contours, double maxArea, int maxLength, int maxGray)
	{
		vector<vector<Point>> selectedContours;
		for (int i = 0; i < contours.size(); i++)
		{
			if (IsAreaContour(contours[i], maxArea) && IsBlackAreaContour(src, contours[i], maxGray))//选取长度符合且领域是黑的轮廓
			{
				selectedContours.push_back(contours[i]);
			}
			else if (IsLongContour(contours[i], maxLength) && IsBlackAreaContour(src, contours[i], maxGray))//选取面积符合且领域是黑的轮廓
			{
				selectedContours.push_back(contours[i]);
			}
		}
		return selectedContours;
	}

	void Gradient(Mat& InputArray_gray, Mat& OutputArray)
{
	//转换为灰度图像
	//Mat InputArray_gray(InputArray.size().height, InputArray.size().width, CV_32F);
	//cvtColor(InputArray, InputArray_gray, CV_RGB2GRAY);
	// 创建X、Y方向梯度图像变量
	Mat grad_x, grad_y;
	Mat abs_grad_x, abs_grad_y;// 梯度绝对值
							   // X方向梯度 并取绝对值
	Sobel(InputArray_gray, grad_x, InputArray_gray.depth(), 1, 0);
	convertScaleAbs(grad_x, abs_grad_x);
	// Y方向梯度 并取绝对值
	Sobel(InputArray_gray, grad_y, InputArray_gray.depth(), 0, 1);
	convertScaleAbs(grad_y, abs_grad_y);
	//magnitude(abs_grad_x, abs_grad_y, OutputArray);
	//计算梯度值的平方
	//pow(abs_grad_x, 2.0f, abs_grad_x);
	//pow(abs_grad_y, 2.0f, abs_grad_y);
	//OutputArray = abs_grad_x + abs_grad_y;
	add(abs_grad_x, abs_grad_y, OutputArray, noArray(), CV_8UC1);
	//显示梯度平方
	//imshow("Gradient", OutputArray);
	//waitKey(0);
}

/**************/
/*梯度阈值分割*/
/**************/
void GradientThreshold(Mat src, Mat &outArray)
{
	Mat gradientDst, thresholdDst;
	Gradient(src, gradientDst);
	Scalar mean;
	Scalar dev;
	meanStdDev(gradientDst, mean, dev);
	float m = mean.val[0];//均值
	float s = dev.val[0];//方差
	float thres = m + 3 * s;
	threshold(src, outArray, thres, 255, THRESH_BINARY_INV);
}

void SingleCutCircle(Mat &src, Point center, int radius)
{
	for (int x = 0; x < src.cols; x++)
	{
		for (int y = 0; y < src.rows; y++)
		{
			int temp = ((x - center.x) * (x - center.x) + (y - center.y) * (y - center.y));
			if (temp >(radius * radius))
			{
				src.at<uchar>(Point(x, y)) = 0;
			}
		}
	}
}

/*****************/
/*获取soble边缘图*/
/*****************/
void GetSobel(Mat src, Mat &outPutArray, Point center, int radius)
{
	Mat src_gray;
	int scale = 1;
	int delta = 0;
	int ddepth = CV_8UC1;
	equalizeHist(src, src);
	//cvNormalize(src, src, 1, 0, CV_MINMAX, 0);
	GaussianBlur(src, src_gray, Size(3, 3), 0, 0, BORDER_DEFAULT);
	//Mat element = getStructuringElement(MORPH_CROSS, Size(7, 7)); //第一个参数MORPH_RECT表示矩形的卷积核，当然还可以选择椭圆形的、交叉型的																   //腐蚀操作

	//erode(src, src, element);
	//dilate(src, src, element);
	/// 转换为灰度图
	//cvtColor(src, src_gray, CV_RGB2GRAY);
	/// 创建 grad_x 和 grad_y 矩阵
	Mat grad_x, grad_y, grad_45, grad_135;
	Mat abs_grad_x, abs_grad_y, abs_grad_45, abs_grad_135;
	//求x方向梯度
	Sobel(src_gray, grad_x, ddepth, 1, 0, 3, scale, delta, BORDER_DEFAULT);
	convertScaleAbs(grad_x, abs_grad_x);

	/// 求Y方向梯度
	Sobel(src_gray, grad_y, ddepth, 0, 1, 3, scale, delta, BORDER_DEFAULT);
	convertScaleAbs(grad_y, abs_grad_y);

	/// 求45方向梯度
	Sobel(src_gray, grad_45, ddepth, 0,2, 3, scale, delta, BORDER_DEFAULT);
	convertScaleAbs(grad_45, abs_grad_45);

	/// 求135方向梯度
	Sobel(src_gray, grad_135, ddepth, 1, 1, 3, scale, delta, BORDER_DEFAULT);
	convertScaleAbs(grad_135, abs_grad_135);
	/// 合并梯度(近似)
	Mat out1, out2;
	add(abs_grad_x, abs_grad_y, out1, noArray(), CV_8UC1);
	add(abs_grad_45, abs_grad_135, out2, noArray(), CV_8UC1);

	outPutArray = (out1 + out2) / 2;
	SingleCutCircle(outPutArray, center, radius - 15);
	//Mat splitPart,splitROI;
	//int PartNumber = 30;
	//for (int xIndex = 0; xIndex + outPutArray.cols / PartNumber <= outPutArray.cols;)
	//{
	//	for (int yIndex = 0; yIndex + outPutArray.rows / PartNumber <= outPutArray.rows;)
	//	{			
	//		splitROI = outPutArray(Rect(xIndex, yIndex, outPutArray.cols /PartNumber, outPutArray.rows / PartNumber));
	//		splitPart = splitROI;
	//		threshold(splitPart, splitPart, 0, 255, THRESH_BINARY | THRESH_OTSU);
	//		
	//		splitPart.copyTo(splitROI, splitPart);
	//		yIndex += outPutArray.rows / PartNumber;
	//	}
	//	xIndex += outPutArray.cols / PartNumber;
	//}
	threshold(outPutArray, outPutArray, 0, 255, THRESH_BINARY |THRESH_OTSU);
	//adaptiveThreshold(outPutArray, outPutArray, 255, ADAPTIVE_THRESH_GAUSSIAN_C, THRESH_BINARY, 43, 0);
}



void CutCircle(Mat &src, Point center, int radius)
{
	for (int x = 0; x < src.cols; x++)
	{
		for (int y = 0; y < src.rows; y++)
		{
			int temp = ((x - center.x) * (x - center.x) + (y - center.y) * (y - center.y));
			if (temp > (radius * radius))
			{ 
				src.at<Vec3b>(Point(x, y))[0] = 0;
				src.at<Vec3b>(Point(x, y))[1] = 0;
				src.at<Vec3b>(Point(x, y))[2] = 0;
			}
		}
	 }
}





void GetContours(vector<vector<Point>> &contours, Mat src, Point center, int radius)
{
	vector<Vec4i> hierarchy;
	Mat graDst, sobleDst;
	cvtColor(src, src, CV_BGR2GRAY, 1);
	//threshold(src, src, 150, 255, THRESH_BINARY_INV);



	GradientThreshold(src, graDst);
	imwrite("gradient.bmp", graDst);
	GetSobel(src, sobleDst, center, radius);
	imwrite("soble.bmp", sobleDst);
	src = graDst + sobleDst;
	imwrite("dst.bmp", src);


	

	SingleCutCircle(src, center, radius - 40);
	imwrite("dst.bmp", src);
	//Mat element = getStructuringElement(MORPH_CROSS, Size(7, 7));
	//erode(src, src, element);
	//imwrite("erode.bmp", src);
	//dilate(src, src, element);
	RemoveSmallRegion(src, src, 190, 0);
	imwrite("remove.bmp", src);
	//Canny(src, src, 9, 3, 3);
	//imwrite("canny.bmp",src);
	findContours(src, contours, hierarchy, CV_RETR_LIST, CHAIN_APPROX_NONE);
}


void DrawContours(vector<vector<Point>> &contours, Mat &src)
{
	drawContours(src, contours, -1, Scalar(0, 255, 0), 1, INTER_LINEAR);
	//drawContours(src, contours, -1, Scalar(0, 255, 0), 1, 8);

	Point2f center; float radius;
	for (int i = 0; i < contours.size(); i++)
	{
		minEnclosingCircle(contours[i], center, radius);
		circle(src, center, radius, Scalar(0, 255, 255), 2);
	}
	Point2f rect[4];
	vector<Rect> boundRect(contours.size());  //定义外接矩形集合
	vector<RotatedRect> box(contours.size()); //定义最小外接矩形集合
	for (int i = 0; i<contours.size(); i++)
	{
		box[i] = minAreaRect(Mat(contours[i]));  //计算每个轮廓最小外接矩形
												 // circle(src, Point(box[i].center.x, box[i].center.y), 5, Scalar(0, 255, 0), -1, 8);  //绘制最小外接矩形的中心点
		box[i].points(rect);  //把最小外接矩形四个端点复制给rect数组
							  // rectangle(src, Point(boundRect[i].x, boundRect[i].y), Point(boundRect[i].x + boundRect[i].width, boundRect[i].y + boundRect[i].height), Scalar(0, 255, 0), 2, 8);
		for (int j = 0; j<4; j++)
		{
			line(src, rect[j], rect[(j + 1) % 4], Scalar(0, 0, 255), 2, 8);  //绘制最小外接矩形每条边
		}
	}
}

int main()
{//【1】加载原始图和Mat变量定义   
	//project文件夹下应该有一张名为1.jpg的素材图
	Mat srcImage;
	//暂时变量和目标图的定义
	//char str[20];
	//for (int i = 0; i <= 0; i++) 
	//{

	//sprintf(str, "sp%d_600.bmp", i);
	srcImage = imread("16灰尘.jpg");
	//} 
	Mat midImage;
	vector<Vec3f> circles;
	//char strOut[20];
	//【2】显示原始图
	//imshow("【原始图】", srcImage);
	//for (int n = 0; n < srcImage.size(); n++)
	//{
	//【3】转为灰度图，进行图像平滑
	cvtColor(srcImage, midImage, CV_BGR2GRAY, 1);//转化边缘检測后的图为灰度图
	GaussianBlur(midImage, midImage, Size(9, 9), 2, 2);

	//【4】进行霍夫圆变换
	//Canny(midImage, cannyImage, 1, 7);
	HoughCircles(midImage, circles, CV_HOUGH_GRADIENT, 1.5, 180, 200, 100, 0, 0);
	Mat srcROICircle;
	//【5】依次在图中绘制出圆
	Point center;
	int radius;
	for (size_t i = 0; i < circles.size(); i++)
	{
		center = Point(cvRound(circles[i][0]), cvRound(circles[i][1]));
		radius = cvRound(circles[i][2]);
		//绘制圆心
		//circle(srcImage[n], center, 5, Scalar(0, 255, 0), -1, 8, 0);
		CutCircle(srcImage, center, radius);
		//绘制圆轮廓
		//circle(srcImage[n], center, radius, Scalar(155, 50, 255), 3, 8, 0);
		srcROICircle = srcImage(Rect(center.x - radius, center.y - radius, radius * 2, radius * 2));

	}
	vector<vector<Point>> contours, selectedContours;
	GetContours(contours, srcROICircle, Point(radius, radius), radius);
	selectedContours = SelectContours(srcROICircle, contours, 12, 9999985, 250);
	DrawContours(selectedContours, srcROICircle);
	//【6】显示效果图  
	//imshow("【效果图】", srcImage);
	//sprintf(strOut, "houghCircle%d.bmp", n);
	imwrite("houghCircle0.bmp", srcROICircle);
	waitKey(0);
    return 0;
}


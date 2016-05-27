#include<stdio.h>
#include<stdlib.h>
#include<string.h>
#include"gtest/gtest.h"

#define xfree(x) if(x) {free(x); x = NULL;}

#ifdef _DEBUG
#ifdef _WIN64
#ifdef _UNICODE
#pragma comment(lib, "lib/VS2015/x64/Unicode/Debug/gtestd.lib")
#else
#pragma comment(lib, "lib/VS2015/x64/MBCS/Debug/gtestd.lib")
#endif
#else
#ifdef _UNICODE
#pragma comment(lib, "lib/VS2015/x86/Unicode/Debug/gtestd.lib")
#else
#pragma comment(lib, "lib/VS2015/x86/MBCS/Debug/gtestd.lib")
#endif
#endif
#else
#ifdef _WIN64
#ifdef _UNICODE
#pragma comment(lib, "lib/VS2015/x64/Unicode/Release/gtest.lib")
#else
#pragma comment(lib, "lib/VS2015/x64/MBCS/Release/gtest.lib")
#endif
#else
#ifdef _UNICODE
#pragma comment(lib, "lib/VS2015/x86/Unicode/Release/gtest.lib")
#else
#pragma comment(lib, "lib/VS2015/x86/MBCS/Release/gtest.lib")
#endif
#endif
#endif

int min3(int a, int b, int c) {
	if (a < b) {
		if (a < c)
			return a;
		else
			return c;
	}
	else {
		if (b < c)
			return b;
		else
			return c;
	}
}

int compute_distance(char *strA, int len_a, char *strB, int len_b, int **temp) {
	int i, j;

	for (i = 1; i <= len_a; i++) {
		temp[i][0] = i;
	}

	for (j = 1; j <= len_b; j++) {
		temp[0][j] = j;
	}

	temp[0][0] = 0;

	for (i = 1; i <= len_a; i++) {
		for (j = 1; j <= len_b; j++) {
			if (strA[i - 1] == strB[j - 1]) {
				temp[i][j] = temp[i - 1][j - 1];
			}
			else {
				temp[i][j] = min3(temp[i - 1][j], temp[i][j - 1], temp[i - 1][j - 1]) + 1;
			}
		}
	}
	return temp[len_a][len_b];
}


int edit_distance(const char* strA, const char* strB)
{
	int i, j;
	int result;
	int len_a = strlen(strA);
	int len_b = strlen(strB);

	/* malloc */
	int **temp = (int**)malloc(sizeof(int*) * (len_a + 1));
	for (i = 0; i < len_a + 1; ++i)
	{
		temp[i] = (int*)malloc(sizeof(int) * (len_b + 1));
		memset(temp[i], 0, sizeof(int) * (len_b + 1));
	}

	for (i = 1; i <= len_a; i++) 
	{
		for (j = 1; j <= len_b; j++) 
		{
			if (strA[i - 1] == strB[j - 1]) 
			{
				temp[i][j] = temp[i - 1][j - 1];
			}
			else 
			{
				temp[i][j] = min3(temp[i - 1][j], temp[i][j - 1], temp[i - 1][j - 1]) + 1;
			}
		}
	}
	result = temp[len_a][len_b];

	/* free */
	for (j = 0; j < len_a; ++j)
	{
		xfree(temp[j]);
	}
	xfree(temp);
	return result;
}

double compute_similarity(const char* src, const char* dst)
{
	int max = strlen(src) + strlen(dst);
	return (double)(max - edit_distance(src, dst)) / max;
}

TEST(compute_distance, compute)
{
	char a[] = "efsdfdabcdefgaabcdefgaabcdefgaabcdefgasfabcdefgefsdfdabcdefgaabcdefgaabcdefgaabcdefgasfabcdefg";
	char b[] = "efsdfdabcdefgaabcdefgaaefsdfdabcdefgaabcdefgaabcdefgaabcdefgasfabcdabcdefggaabcdefgasfabcdefg";
	int len_a = strlen(a);
	int len_b = strlen(b);

	int **temp = (int**)malloc(sizeof(int*) * (len_a + 1));
	for (int i = 0; i < len_a + 1; i++) {
		temp[i] = (int*)malloc(sizeof(int) * (len_b + 1));
		memset(temp[i], 0, sizeof(int) * (len_b + 1));
	}
	int distance = compute_distance(a, len_a, b, len_b, temp);
	EXPECT_EQ(21, distance);
}

TEST(edit_distance, compute)
{
	EXPECT_EQ(2, edit_distance("ab¸ò", "ab"));
}

TEST(compute_similarity, compute)
{
	EXPECT_DOUBLE_EQ(0.8, compute_similarity("ÄãºÃ", "ÄãÃÇºÃ"));
}

int main(int argc, char* argv[])
{
	testing::InitGoogleTest(&argc, argv);
	auto val = RUN_ALL_TESTS();
	system("pause");
	return val;
}
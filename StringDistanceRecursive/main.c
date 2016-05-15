#include<stdio.h>
#include<stdlib.h>
#include<string.h>

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
int compute_distance(char *strA, int pABegin, int pAEnd, char *strB, int pBBegin, int pBEnd, int **temp) {
	int a, b, c;
	if (pABegin > pAEnd) {
		if (pBBegin > pBEnd) {
			return 0;
		}
		else {
			return pBEnd - pBBegin + 1;
		}
	}

	if (pBBegin > pBEnd) {
		if (pABegin > pAEnd) {
			return 0;
		}
		else {
			return pAEnd - pABegin + 1;
		}
	}

	if (strA[pABegin] == strB[pBBegin]) {
		if (temp[pABegin + 1][pBBegin + 1] != 0) {
			a = temp[pABegin + 1][pBBegin + 1];
		}
		else {
			a = compute_distance(strA, pABegin + 1, pAEnd, strB, pBBegin + 1, pBEnd, temp);
		}
		return a;
	}
	else {
		if (temp[pABegin + 1][pBBegin + 1] != 0) {
			a = temp[pABegin + 1][pBBegin + 1];
		}
		else {
			a = compute_distance(strA, pABegin + 1, pAEnd, strB, pBBegin + 1, pBEnd, temp);
			temp[pABegin + 1][pBBegin + 1] = a;
		}

		if (temp[pABegin + 1][pBBegin] != 0) {
			b = temp[pABegin + 1][pBBegin];
		}
		else {
			b = compute_distance(strA, pABegin + 1, pAEnd, strB, pBBegin, pBEnd, temp);
			temp[pABegin + 1][pBBegin] = b;
		}

		if (temp[pABegin][pBBegin + 1] != 0) {
			c = temp[pABegin][pBBegin + 1];
		}
		else {
			c = compute_distance(strA, pABegin, pAEnd, strB, pBBegin + 1, pBEnd, temp);
			temp[pABegin][pBBegin + 1] = c;
		}

		return min3(a, b, c) + 1;
	}

}

void main() {
	char a[] = "efsdfdabcdefgaabcdefgaabcdefgaabcdefgasfabcdefgefsdfdabcdefgaabcdefgaabcdefgaabcdefgasfabcdefg";
	char b[] = "efsdfdabcdefgaabcdefgaaefsdfdabcdefgaabcdefgaabcdefgaabcdefgasfabcdabcdefggaabcdefgasfabcdefg";
	int len_a = strlen(a);
	int len_b = strlen(b);

	int **temp = (int**)malloc(sizeof(int*) * (len_a + 1));
	for (int i = 0; i < len_a + 1; i++) {
		temp[i] = (int*)malloc(sizeof(int) * (len_b + 1));
		memset(temp[i], 0, sizeof(int) * (len_b + 1));
	}
	memset(temp, 0, sizeof(temp));
	int distance = compute_distance(a, 0, len_a - 1, b, 0, len_b - 1, temp);
	printf("%d\n", distance);
	system("pause");
}
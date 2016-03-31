#include <iostream>
#include <vector>
#include <numeric>
#include <algorithm>
#include <chrono>
#include <functional>

using namespace std;

typedef struct TESTNODE
{
	wchar_t* key;
	int value;
} TestNode;

int main()
{
	TestNode x = { x.key = L"A", x.value = 115 };
	TestNode y = { y.key = L"B", y.value = 95 };
	TestNode z = { z.key = L"C", z.value = 125 };
	vector<TestNode> vecNodes = { x, y, z };
	vector<int> vecInts;
	for (int i = 0; i < 1000; ++i)
	{
		vecInts.push_back(rand());
	}
	clock_t start = clock();
	sort(vecInts.begin(), vecInts.end(), greater<int>());
	int sum = accumulate(vecNodes.begin(), vecNodes.end(), 0,
		[](int s, TestNode t) -> int {return s + t.value; });
	sort(vecNodes.begin(), vecNodes.end(), [](TestNode a, TestNode b) -> bool { return a.value < b.value; });
	clock_t end = clock();
	double timeSpan = double(end - start) / CLOCKS_PER_SEC;
	for (vector<TestNode>::const_iterator cItr = vecNodes.begin(); cItr != vecNodes.end(); ++cItr)
	{
		cout << cItr->key << ", " << cItr->value << endl;
	}
	cout << sum << endl;
	cout << timeSpan << "s" << endl;
	system("pause");
	return 0;
}
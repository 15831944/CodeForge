#include <iostream>
#include <vector>
#include <numeric>
#include <algorithm>
#include <chrono>
#include <functional>
#include <unordered_map>
#include <string>

using namespace std;

typedef struct TESTNODE
{
	char* key;
	int value;
} TestNode;

int main()
{
	TestNode x = { x.key = "AA", x.value = 115 };
	TestNode y = { y.key = "BB", y.value = 95 };
	TestNode z = { z.key = "CC", z.value = 125 };
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

	unordered_map<string, int> filemap;
	filemap.insert(make_pair("acad.lsp", 1));
	filemap.insert(make_pair("acaddoc.lsp", 1));
	filemap.insert(make_pair("acad.fas", 1));
	filemap.insert(make_pair("acaddoc.fas", 1));
	filemap.insert(make_pair("acad.vlx", 1));
	filemap.insert(make_pair("acaddoc.vlx", 1));
	filemap.insert(make_pair("dwgrun.bat", 1));
	filemap.insert(make_pair("acad.mnl", 2));

	unordered_map<string, int>::iterator itr;
	if ((itr = filemap.find("acad.lsp")) != filemap.end())
	{
		if (itr->second == 1)
		{
			cout << itr->first << ":" << itr->second << endl;
		}
	}
	cout << hash_value("acad.lsp") << endl;
	cout << hash_value("acaddoc.lsp") << endl;
	system("pause");
	return 0;
}
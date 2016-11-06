#include <windows.h>
#include <stdio.h>

// Both __stdcall and __cdecl are OK for BqqZip.dll, and __cdecl is the default one in C++.
//typedef int(__cdecl *ZIPPROC)(wchar_t*, LPCWSTR);
//typedef int(__cdecl *UNZIPPROC)(wchar_t*, LPCWSTR);
typedef int(__stdcall *ZIPPROC)(LPCWSTR, LPCWSTR);
typedef int(__stdcall *UNZIPPROC)(LPCWSTR, LPCWSTR);
//typedef int(*ZIPPROC)(wchar_t*, LPCWSTR);
//typedef int(*UNZIPPROC)(wchar_t*, LPCWSTR);

int main()
{
	HINSTANCE hInstLib;
	ZIPPROC Zip;
	UNZIPPROC UnZip;
	BOOL bFreeResult;
	char szBasePath[MAX_PATH];
	ZeroMemory(szBasePath, MAX_PATH);
	GetCurrentDirectory(MAX_PATH, szBasePath);
	printf("Current path: %s\n", szBasePath);
	// Get a handle to the DLL module.
	hInstLib = LoadLibrary(TEXT("BqqZip.dll"));

	// If the handle is valid, try to get the function address.
	if (NULL != hInstLib)
	{
		Zip = (ZIPPROC)GetProcAddress(hInstLib, "Zip");
		UnZip = (UNZIPPROC)GetProcAddress(hInstLib, "UnZip");

		if (NULL != Zip)
		{			
			printf("Zip the first file: %d\n", Zip(L"C:\\Users\\Lenovo\\Documents\\Visual Studio 2015\\Projects\\VC\\CodeForge\\Release\\1ÕýÃæ3.jpg",
				L"C:\\Users\\Lenovo\\Documents\\Visual Studio 2015\\Projects\\VC\\CodeForge\\Release\\first.zip"));
			printf("Zip the second file: %d\n", Zip(L"C:\\Users\\Lenovo\\Documents\\Visual Studio 2015\\Projects\\VC\\CodeForge\\Release\\2±³Ãæ2.jpg", 
				L"C:\\Users\\Lenovo\\Documents\\Visual Studio 2015\\Projects\\VC\\CodeForge\\Release\\second.zip"));
		} 
		else
		{
			printf("Can not call the Zip function.\n");
		}

		if (NULL != UnZip)
		{
			printf("Unzip the first file: %d\n", UnZip(L"C:\\Users\\Lenovo\\Documents\\Visual Studio 2015\\Projects\\VC\\CodeForge\\Release\\first.zip", 
				L"C:\\Users\\Lenovo\\Documents\\Visual Studio 2015\\Projects\\VC\\CodeForge\\Release\\first.jpg"));
			printf("Unzip the second file: %d\n", UnZip(L"C:\\Users\\Lenovo\\Documents\\Visual Studio 2015\\Projects\\VC\\CodeForge\\Release\\second.zip", 
				L"C:\\Users\\Lenovo\\Documents\\Visual Studio 2015\\Projects\\VC\\CodeForge\\Release\\second.jpg"));
		} 
		else
		{
			printf("Can not call the Unzip function.\n");
		}

		// Free the DLL module.
		bFreeResult = FreeLibrary(hInstLib);
		printf("Free the library: %d\n", bFreeResult);
	}
	else
	{
		printf("Can not load the library.\n");
	}
	system("pause");
	return 0;
}
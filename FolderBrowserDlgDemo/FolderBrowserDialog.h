#pragma once

#include <Windows.h>

class FolderBrowserDialog
{
public:
	int Flags;
	HWND Owner;
	TCHAR* SelectedPath;
	TCHAR* Title;

	FolderBrowserDialog();
	~FolderBrowserDialog();
	bool ShowDialog();
};


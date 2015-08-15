#include "stdafx.h"
#include "FolderBrowserDialog.h"
#include <ShlObj.h>

#pragma comment(lib, "Shell32.lib")


FolderBrowserDialog::FolderBrowserDialog()
{
	this->Flags = BIF_USENEWUI;
	this->Owner = NULL;
	this->Title = NULL;
}


FolderBrowserDialog::~FolderBrowserDialog()
{
}


bool FolderBrowserDialog::ShowDialog()
{
	BROWSEINFO browserInfo;
	memset(&browserInfo, 0, sizeof(browserInfo));

	browserInfo.hwndOwner = this->Owner;
	browserInfo.lpszTitle = this->Title;
	browserInfo.ulFlags = this->Flags;

	OleInitialize(NULL);
	LPITEMIDLIST lpItemIdList = SHBrowseForFolder(&browserInfo);

	if (NULL == lpItemIdList)
	{
		return false;
	}

	TCHAR *buffer = new TCHAR[MAX_PATH];

	if (!SHGetPathFromIDList(lpItemIdList, buffer))
	{
		CoTaskMemFree(lpItemIdList);
		return false;
	}
	this->SelectedPath = buffer;

	CoTaskMemFree(lpItemIdList);
	OleUninitialize();

	return true;
}

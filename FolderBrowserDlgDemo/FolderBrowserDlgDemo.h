
// FolderBrowserDlgDemo.h : PROJECT_NAME Ӧ�ó������ͷ�ļ�
//

#pragma once

#ifndef __AFXWIN_H__
	#error "�ڰ������ļ�֮ǰ������stdafx.h�������� PCH �ļ�"
#endif

#include "resource.h"		// ������


// CFolderBrowserDlgDemoApp: 
// �йش����ʵ�֣������ FolderBrowserDlgDemo.cpp
//

class CFolderBrowserDlgDemoApp : public CWinApp
{
public:
	CFolderBrowserDlgDemoApp();

// ��д
public:
	virtual BOOL InitInstance();

// ʵ��

	DECLARE_MESSAGE_MAP()
};

extern CFolderBrowserDlgDemoApp theApp;
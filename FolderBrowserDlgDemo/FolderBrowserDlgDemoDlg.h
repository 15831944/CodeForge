
// FolderBrowserDlgDemoDlg.h : ͷ�ļ�
//

#pragma once
#include "afxwin.h"
#include "afxbutton.h"


// CFolderBrowserDlgDemoDlg �Ի���
class CFolderBrowserDlgDemoDlg : public CDialogEx
{
// ����
public:
	CFolderBrowserDlgDemoDlg(CWnd* pParent = NULL);	// ��׼���캯��

// �Ի�������
#ifdef AFX_DESIGN_TIME
	enum { IDD = IDD_FOLDERBROWSERDLGDEMO_DIALOG };
#endif

	protected:
	virtual void DoDataExchange(CDataExchange* pDX);	// DDX/DDV ֧��


// ʵ��
protected:
	HICON m_hIcon;

	// ���ɵ���Ϣӳ�亯��
	virtual BOOL OnInitDialog();
	afx_msg void OnSysCommand(UINT nID, LPARAM lParam);
	afx_msg void OnPaint();
	afx_msg HCURSOR OnQueryDragIcon();
	DECLARE_MESSAGE_MAP()
private:
	CEdit m_txtFolder;
	CEdit m_txtFolderPicker;
	CMFCButton m_mfcBtnFolder;
	CMFCButton m_mfcBtnFolderPicker;
public:
	afx_msg void OnBnClickedMfcbuttonFolder();
	afx_msg void OnBnClickedMfcbuttonFolderpicker();
};


// FolderBrowserDlgDemoDlg.cpp : ʵ���ļ�
//

#include "stdafx.h"
#include "FolderBrowserDlgDemo.h"
#include "FolderBrowserDlgDemoDlg.h"
#include "afxdialogex.h"
#include "FolderBrowserDialog.h"

#ifdef _DEBUG
#define new DEBUG_NEW
#endif


// ����Ӧ�ó��򡰹��ڡ��˵���� CAboutDlg �Ի���

class CAboutDlg : public CDialogEx
{
public:
	CAboutDlg();

// �Ի�������
#ifdef AFX_DESIGN_TIME
	enum { IDD = IDD_ABOUTBOX };
#endif

	protected:
	virtual void DoDataExchange(CDataExchange* pDX);    // DDX/DDV ֧��

// ʵ��
protected:
	DECLARE_MESSAGE_MAP()
};

CAboutDlg::CAboutDlg() : CDialogEx(IDD_ABOUTBOX)
{
}

void CAboutDlg::DoDataExchange(CDataExchange* pDX)
{
	CDialogEx::DoDataExchange(pDX);
}

BEGIN_MESSAGE_MAP(CAboutDlg, CDialogEx)
END_MESSAGE_MAP()


// CFolderBrowserDlgDemoDlg �Ի���



CFolderBrowserDlgDemoDlg::CFolderBrowserDlgDemoDlg(CWnd* pParent /*=NULL*/)
	: CDialogEx(IDD_FOLDERBROWSERDLGDEMO_DIALOG, pParent)
{
	m_hIcon = AfxGetApp()->LoadIcon(IDR_MAINFRAME);
}

void CFolderBrowserDlgDemoDlg::DoDataExchange(CDataExchange* pDX)
{
	CDialogEx::DoDataExchange(pDX);
	DDX_Control(pDX, IDC_EDIT_FOLDER, m_txtFolder);
	DDX_Control(pDX, IDC_EDIT_FOLDERPICKER, m_txtFolderPicker);
	DDX_Control(pDX, IDC_MFCBUTTON_FOLDER, m_mfcBtnFolder);
	DDX_Control(pDX, IDC_MFCBUTTON_FOLDERPICKER, m_mfcBtnFolderPicker);
}

BEGIN_MESSAGE_MAP(CFolderBrowserDlgDemoDlg, CDialogEx)
	ON_WM_SYSCOMMAND()
	ON_WM_PAINT()
	ON_WM_QUERYDRAGICON()
	ON_BN_CLICKED(IDC_MFCBUTTON_FOLDER, &CFolderBrowserDlgDemoDlg::OnBnClickedMfcbuttonFolder)
	ON_BN_CLICKED(IDC_MFCBUTTON_FOLDERPICKER, &CFolderBrowserDlgDemoDlg::OnBnClickedMfcbuttonFolderpicker)
END_MESSAGE_MAP()


// CFolderBrowserDlgDemoDlg ��Ϣ�������

BOOL CFolderBrowserDlgDemoDlg::OnInitDialog()
{
	CDialogEx::OnInitDialog();

	// ��������...���˵�����ӵ�ϵͳ�˵��С�

	// IDM_ABOUTBOX ������ϵͳ���Χ�ڡ�
	ASSERT((IDM_ABOUTBOX & 0xFFF0) == IDM_ABOUTBOX);
	ASSERT(IDM_ABOUTBOX < 0xF000);

	CMenu* pSysMenu = GetSystemMenu(FALSE);
	if (pSysMenu != NULL)
	{
		BOOL bNameValid;
		CString strAboutMenu;
		bNameValid = strAboutMenu.LoadString(IDS_ABOUTBOX);
		ASSERT(bNameValid);
		if (!strAboutMenu.IsEmpty())
		{
			pSysMenu->AppendMenu(MF_SEPARATOR);
			pSysMenu->AppendMenu(MF_STRING, IDM_ABOUTBOX, strAboutMenu);
		}
	}

	// ���ô˶Ի����ͼ�ꡣ  ��Ӧ�ó��������ڲ��ǶԻ���ʱ����ܽ��Զ�
	//  ִ�д˲���
	SetIcon(m_hIcon, TRUE);			// ���ô�ͼ��
	SetIcon(m_hIcon, FALSE);		// ����Сͼ��

	// TODO: �ڴ���Ӷ���ĳ�ʼ������
	CString strFolderSelect;
	strFolderSelect.LoadString(IDS_FOLDER);

	this->m_mfcBtnFolder.SetWindowText(strFolderSelect);
	this->m_mfcBtnFolderPicker.SetWindowText(strFolderSelect);

	return TRUE;  // ���ǽ��������õ��ؼ������򷵻� TRUE
}

void CFolderBrowserDlgDemoDlg::OnSysCommand(UINT nID, LPARAM lParam)
{
	if ((nID & 0xFFF0) == IDM_ABOUTBOX)
	{
		CAboutDlg dlgAbout;
		dlgAbout.DoModal();
	}
	else
	{
		CDialogEx::OnSysCommand(nID, lParam);
	}
}

// �����Ի��������С����ť������Ҫ����Ĵ���
//  �����Ƹ�ͼ�ꡣ  ����ʹ���ĵ�/��ͼģ�͵� MFC Ӧ�ó���
//  �⽫�ɿ���Զ���ɡ�

void CFolderBrowserDlgDemoDlg::OnPaint()
{
	if (IsIconic())
	{
		CPaintDC dc(this); // ���ڻ��Ƶ��豸������

		SendMessage(WM_ICONERASEBKGND, reinterpret_cast<WPARAM>(dc.GetSafeHdc()), 0);

		// ʹͼ���ڹ����������о���
		int cxIcon = GetSystemMetrics(SM_CXICON);
		int cyIcon = GetSystemMetrics(SM_CYICON);
		CRect rect;
		GetClientRect(&rect);
		int x = (rect.Width() - cxIcon + 1) / 2;
		int y = (rect.Height() - cyIcon + 1) / 2;

		// ����ͼ��
		dc.DrawIcon(x, y, m_hIcon);
	}
	else
	{
		CDialogEx::OnPaint();
	}
}

//���û��϶���С������ʱϵͳ���ô˺���ȡ�ù��
//��ʾ��
HCURSOR CFolderBrowserDlgDemoDlg::OnQueryDragIcon()
{
	return static_cast<HCURSOR>(m_hIcon);
}



void CFolderBrowserDlgDemoDlg::OnBnClickedMfcbuttonFolder()
{
	// TODO: �ڴ���ӿؼ�֪ͨ����������
	FolderBrowserDialog folderBrowserDialog{};
	folderBrowserDialog.Title = _T("ѡ���ļ���");
	if (folderBrowserDialog.ShowDialog())
	{
		m_txtFolder.SetWindowText(folderBrowserDialog.SelectedPath);
	}
}


void CFolderBrowserDlgDemoDlg::OnBnClickedMfcbuttonFolderpicker()
{
	// TODO: �ڴ���ӿؼ�֪ͨ����������
	CFolderPickerDialog folderPickerDlg{};
	
	if (IDOK == folderPickerDlg.DoModal())
	{
		m_txtFolderPicker.SetWindowText(folderPickerDlg.GetFolderPath());
	}
	
}

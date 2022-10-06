
// MediaStreamingDemoDlg.cpp : implementation file
//

#include "stdafx.h"
#include "MediaStreamingDemo.h"
#include "MediaStreamingDemoDlg.h"
#include "afxdialogex.h"

#include "DirectShowHelper.h"

#include <sstream>

#ifdef _DEBUG
#define new DEBUG_NEW
#endif


// CAboutDlg dialog used for App About

class CAboutDlg : public CDialogEx
{
public:
	CAboutDlg();

// Dialog Data
	enum { IDD = IDD_ABOUTBOX };

	protected:
	virtual void DoDataExchange(CDataExchange* pDX);    // DDX/DDV support

// Implementation
protected:
	DECLARE_MESSAGE_MAP()
};

CAboutDlg::CAboutDlg() : CDialogEx(CAboutDlg::IDD)
{
}

void CAboutDlg::DoDataExchange(CDataExchange* pDX)
{
	CDialogEx::DoDataExchange(pDX);
}

BEGIN_MESSAGE_MAP(CAboutDlg, CDialogEx)
END_MESSAGE_MAP()


// CMediaStreamingDemoDlg dialog




CMediaStreamingDemoDlg::CMediaStreamingDemoDlg(CWnd* pParent /*=NULL*/)
	: CDialogEx(CMediaStreamingDemoDlg::IDD, pParent)
	, mVideoFPS(25)
	, mState(CaptureState::Stop)
{
	m_hIcon = AfxGetApp()->LoadIcon(IDR_MAINFRAME);
	m_iMP4VideoBitrate = 4600000;
	m_iMP4FrameRate = 25;
	m_iMP4AudioChannels =2;

	#ifndef _WIN64
		m_iMP4Width = 720;
		m_iMP4Height = 480;
	#else
		m_iMP4Width = 1980;
		m_iMP4Height = 1080;
	#endif

	m_strMP4Title = _T("My Title");
	m_strMP4Author = _T("My Author");
	m_strMP4Album = _T("My Album");
	m_strMP4Comment = _T("My Comment");
}

void CMediaStreamingDemoDlg::DoDataExchange(CDataExchange* pDX)
{
	CDialogEx::DoDataExchange(pDX);
	DDX_Control(pDX, IDC_VIDEO_DEVICE_COMBO, uiVideoDevices);
	DDX_Control(pDX, IDC_VIDEO_SIZE_COMBO, uiVideoSizes);
	DDX_Text(pDX, IDC_EDIT_FPS, mVideoFPS);
	DDX_Control(pDX, IDC_COMBO_AUDIO_DEVICE, uiAudioDevices);;
	DDX_Text(pDX, IDC_EDITMP4VIDEOBITRATE, m_iMP4VideoBitrate);
	DDX_Text(pDX, IDC_EDITMP4FRAMERATE, m_iMP4FrameRate);
	DDX_Control(pDX, IDC_COMBOMP4AUDIOBITRATE, m_CboMP4AudioBitrate);
	DDX_Text(pDX, IDC_EDITMP4AUDIOCHANNEL, m_iMP4AudioChannels);
	DDX_Text(pDX, IDC_EDITMP4WIDTH, m_iMP4Width);
	DDX_Text(pDX, IDC_EDITMP4HEIGHT, m_iMP4Height);
	DDX_Control(pDX, IDC_CHECKMP4ASPECTRATIO, m_ChkMP4AspectRatio);
	DDX_Control(pDX, IDC_COMBOMP4AUDIOSAMPLERATE, m_CboMP4AudioSampleRate);
	DDX_Text(pDX, IDC_EDITMP4TITLE, m_strMP4Title);
	DDX_Text(pDX, IDC_EDITMP4AUTHOR, m_strMP4Author);
	DDX_Text(pDX, IDC_EDITMP4ALBUM, m_strMP4Album);
	DDX_Text(pDX, IDC_EDITMP4COMMENT, m_strMP4Comment);
	DDX_Control(pDX, IDC_CBOGPU, m_CboGPU);
	DDX_Control(pDX, IDC_CBONVIDAPRESET, m_CboNVIDAPreset);
}

BEGIN_MESSAGE_MAP(CMediaStreamingDemoDlg, CDialogEx)
	ON_WM_SYSCOMMAND()
	ON_WM_PAINT()
	ON_WM_QUERYDRAGICON()
	ON_BN_CLICKED(IDC_START_BUTTON, &CMediaStreamingDemoDlg::OnBnClickedStartButton)
	ON_BN_CLICKED(IDC_STOP_BUTTON, &CMediaStreamingDemoDlg::OnBnClickedStopButton)
	ON_CBN_SELCHANGE(IDC_VIDEO_DEVICE_COMBO, &CMediaStreamingDemoDlg::OnCbnSelchangeVideoDeviceCombo)
	ON_CBN_SELCHANGE(IDC_VIDEO_SIZE_COMBO, &CMediaStreamingDemoDlg::OnCbnSelchangeVideoSizeCombo)

	ON_CBN_SELCHANGE(IDC_CBOGPU, &CMediaStreamingDemoDlg::OnSelchangeCbogpu)
	ON_BN_CLICKED(IDC_BTNDETECTGPU, &CMediaStreamingDemoDlg::OnBnClickedBtndetectgpu)
END_MESSAGE_MAP()


// CMediaStreamingDemoDlg message handlers

BOOL CMediaStreamingDemoDlg::OnInitDialog()
{
	CDialogEx::OnInitDialog();

	// Add "About..." menu item to system menu.

	// IDM_ABOUTBOX must be in the system command range.
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

	// Set the icon for this dialog.  The framework does this automatically
	//  when the application's main window is not a dialog
	SetIcon(m_hIcon, TRUE);			// Set big icon
	SetIcon(m_hIcon, FALSE);		// Set small icon

	CRect rc;
	GetDlgItem(IDC_VIDEO_PLACEHOLDER)->GetWindowRect(rc);
	ScreenToClient(rc);

	mVideoWnd.Create(NULL, NULL, WS_CHILD | WS_VISIBLE, rc, this, IDC_VIDEO_PLACEHOLDER);
	//mPreviewWnd = &mVideoWnd;

	DirectShowHelper dsHelper;
	std::vector<std::wstring> devices;
	dsHelper.enumerateDevices(CLSID_VideoInputDeviceCategory, devices);

	int i;
	for (i = 0; i < devices.size(); i++) {
		uiVideoDevices.AddString(devices[i].c_str());
	}

	if (uiVideoDevices.GetCount()) {
		uiVideoDevices.SetCurSel(0);
		CString curCam;
		uiVideoDevices.GetLBText(uiVideoDevices.GetCurSel(), curCam);
		updateResolutions(curCam.GetBuffer());
	}

	devices.clear();
	dsHelper.enumerateDevices(CLSID_AudioInputDeviceCategory, devices);

	for (i = 0; i < devices.size(); i++) {
		uiAudioDevices.AddString(devices[i].c_str());
	}

	if (uiAudioDevices.GetCount()) {
		uiAudioDevices.SetCurSel(0);
	}



	m_CboMP4AudioBitrate.AddString(L"96000");
	m_CboMP4AudioBitrate.AddString(L"128000");
	m_CboMP4AudioBitrate.AddString(L"160000");
	m_CboMP4AudioBitrate.AddString(L"192000");
	m_CboMP4AudioBitrate.SetCurSel(0);

	m_CboMP4AudioSampleRate.AddString(L"48000");
	m_CboMP4AudioSampleRate.AddString(L"44100");

	m_CboMP4AudioSampleRate.SetCurSel(1);

	#ifndef _WIN64
		SetWindowText(L"Video Capture to MP4 Sample for (x86)");
	#else
		SetWindowText(L"Video Capture to MP4 Sample for (x64)");

	#endif


	this->m_CboGPU.AddString(L"None");
	this->m_CboGPU.AddString(L"Nvida");
	this->m_CboGPU.AddString(L"AMD");
	this->m_CboGPU.AddString(L"Intel");
	this->m_CboGPU.SetCurSel(0);

	m_CboNVIDAPreset.AddString(L"slow");
	m_CboNVIDAPreset.AddString(L"medium");
	m_CboNVIDAPreset.AddString(L"fast");
	m_CboNVIDAPreset.AddString(L"high performance");
	m_CboNVIDAPreset.AddString(L"high quality");
	m_CboNVIDAPreset.AddString(L"bluray disk");
	m_CboNVIDAPreset.AddString(L"low latency");
	m_CboNVIDAPreset.AddString(L"low latency high quality");
	m_CboNVIDAPreset.AddString(L"low latency high performance");
	m_CboNVIDAPreset.AddString(L"lossless");
	m_CboNVIDAPreset.AddString(L"lossless high performance");
	m_CboNVIDAPreset.SetCurSel(3);


	return TRUE;  // return TRUE  unless you set the focus to a control
}

void CMediaStreamingDemoDlg::OnSysCommand(UINT nID, LPARAM lParam)
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

// If you add a minimize button to your dialog, you will need the code below
//  to draw the icon.  For MFC applications using the document/view model,
//  this is automatically done for you by the framework.

void CMediaStreamingDemoDlg::OnPaint()
{
	if (IsIconic())
	{
		CPaintDC dc(this); // device context for painting

		SendMessage(WM_ICONERASEBKGND, reinterpret_cast<WPARAM>(dc.GetSafeHdc()), 0);

		// Center icon in client rectangle
		int cxIcon = GetSystemMetrics(SM_CXICON);
		int cyIcon = GetSystemMetrics(SM_CYICON);
		CRect rect;
		GetClientRect(&rect);
		int x = (rect.Width() - cxIcon + 1) / 2;
		int y = (rect.Height() - cyIcon + 1) / 2;

		// Draw the icon
		dc.DrawIcon(x, y, m_hIcon);
	}
	else
	{
		CDialogEx::OnPaint();
	}
}

// The system calls this function to obtain the cursor to display while the user drags
//  the minimized window.
HCURSOR CMediaStreamingDemoDlg::OnQueryDragIcon()
{
	return static_cast<HCURSOR>(m_hIcon);
}

bool CMediaStreamingDemoDlg::hasSize(DWORD size)
{
	for (int i = 0; i < uiVideoSizes.GetCount(); i++) {
		if (uiVideoSizes.GetItemData(i) == size)
			return true;
	}

	return false;
}

void CMediaStreamingDemoDlg::updateResolutions(const WCHAR* device)
{
	DirectShowHelper dsHelper;
	std::vector<AM_MEDIA_TYPE*> mediaTypes;

	uiVideoSizes.ResetContent();

	dsHelper.enumerateMediaTypes(device, CLSID_VideoInputDeviceCategory, mediaTypes);
	for (int i = 0; i < mediaTypes.size(); i++) {
		AM_MEDIA_TYPE* mt = mediaTypes[i];
		if (mt) {
			if (mt->majortype == MEDIATYPE_Video /*&& mt->subtype == MEDIASUBTYPE_RGB24*/ && mt->cbFormat == sizeof(VIDEOINFOHEADER)) {
				VIDEOINFOHEADER* vih = (VIDEOINFOHEADER*) mt->pbFormat;
				if (vih) {
					std::wstringstream ss;
					ss << vih->bmiHeader.biWidth << "x" << vih->bmiHeader.biHeight;
					std::wstring size = ss.str();

					DWORD data = vih->bmiHeader.biWidth;
					data <<= 16;
					data |= vih->bmiHeader.biHeight;

					if (!hasSize(data)) {
						uiVideoSizes.AddString(size.c_str());
						uiVideoSizes.SetItemData(uiVideoSizes.GetCount() - 1, (DWORD_PTR)data);
					}
				}
			}
			dsHelper.deleteMediaType(mt);
		}
	}

	if (uiVideoSizes.GetCount()) {
		uiVideoSizes.SetCurSel(0);
		OnCbnSelchangeVideoSizeCombo();
	}
}

void CMediaStreamingDemoDlg::updateCaptureUI()
{
	CString text;
	switch (mState) {
	case CaptureState::Stop:
		text = L"Start";
		break;
	case CaptureState::Start:
		text = L"Pause";
		break;
	case CaptureState::Pause:
		text = L"Resume";
		break;
	}

	GetDlgItem(IDC_START_BUTTON)->SetWindowText(text);
}

void CMediaStreamingDemoDlg::OnBnClickedStartButton()
{
	if (mState == CaptureState::Stop) {
		UpdateData();

	
		CString curCam;
		uiVideoDevices.GetLBText(uiVideoDevices.GetCurSel(), curCam);
		mMediaCapture.setVideoDevice(curCam.GetBuffer());
		mMediaCapture.setWidth(mWidth);
		mMediaCapture.setHeight(mHeight);
		mMediaCapture.setFramerate(mVideoFPS);
		mMediaCapture.setDisplayWindow(mVideoWnd.m_hWnd);
	
		// audio
		CString curMic;
		uiAudioDevices.GetLBText(uiAudioDevices.GetCurSel(), curMic);
		mMediaCapture.setAudioDevice(curMic.GetBuffer());
		mMediaCapture.setMP4MetaData(this->m_strMP4Title,this->m_strMP4Author,this->m_strMP4Album,this->m_strMP4Comment);


		CString strFilter = L"MP4 File (*.mp4)|*.mp4||";	
		CFileDialog dlg(FALSE,L"",L".mp4",OFN_FILEMUSTEXIST,strFilter,this);
		if(dlg.DoModal()==IDCANCEL)
			return;

		mMediaCapture.setGPU(m_CboGPU.GetCurSel());

		mMediaCapture.setFileName(dlg.GetPathName().GetBuffer());
	
		CString strAudioBitrate;
		m_CboMP4AudioBitrate.GetLBText(m_CboMP4AudioBitrate.GetCurSel(),strAudioBitrate);
		
		CString strAudioSampleRate;
		m_CboMP4AudioSampleRate.GetLBText(m_CboMP4AudioSampleRate.GetCurSel(),strAudioSampleRate);
		

		mMediaCapture.setMP4FileSetting(this->m_iMP4Width,this->m_iMP4Height,this->m_iMP4VideoBitrate, _ttoi(strAudioBitrate),this->m_iMP4FrameRate, _ttoi(strAudioSampleRate),this->m_iMP4AudioChannels,this->m_ChkMP4AspectRatio.GetCheck());
		if (mMediaCapture.start())
			mState = CaptureState::Start;
		else
			AfxMessageBox(mMediaCapture.getLastError().c_str());

		updateCaptureUI();
	}
	else if (mState == CaptureState::Start) {

		
		mMediaCapture.pause();
		mState = CaptureState::Pause;
		updateCaptureUI();
	}
	else if (mState == CaptureState::Pause) {
		mMediaCapture.resume();
		mState = CaptureState::Start;
		updateCaptureUI();
	}
}


void CMediaStreamingDemoDlg::OnBnClickedStopButton()
{
	mMediaCapture.stop();
	mState = CaptureState::Stop;
	updateCaptureUI();
}


void CMediaStreamingDemoDlg::OnCbnSelchangeVideoDeviceCombo()
{
	CString curCam;
	uiVideoDevices.GetLBText(uiVideoDevices.GetCurSel(), curCam);
	updateResolutions(curCam.GetBuffer());
}


void CMediaStreamingDemoDlg::OnCbnSelchangeVideoSizeCombo()
{
	if (uiVideoSizes.GetCurSel() >= 0) {
		DWORD data = (DWORD)uiVideoSizes.GetItemData(uiVideoSizes.GetCurSel());
		mWidth = (data >> 16) & 0xFFFF;
		mHeight = data & 0xFFFF;
	}

	CRect rc;

	GetDlgItem(IDC_VIDEO_PLACEHOLDER)->GetWindowRect(rc);

	if (rc.Width() > mWidth && rc.Height() > mHeight) {
		CRect rc2(0, 0, mWidth, mHeight);
		rc2.OffsetRect(rc.left + (rc.Width() - rc2.Width()) / 2, rc.top + (rc.Height() - rc2.Height()) / 2);
		rc = rc2;
	}
/*	else {
		double ratio;
		int dx = mWidth - rc.Width();
		int dy = mHeight - rc.Height();
		if (dx > dy) {
			ratio = mWidth / (double)mHeight;
			rc.DeflateRect(0, (rc.Height() - (rc.Height() / ratio)) / 2);
		}
		else {
			ratio = mHeight / (double)mWidth;
			rc.DeflateRect((rc.Width() - (rc.Width() / ratio)) / 2, 0);
		}
	}
*/
	ScreenToClient(rc);
	mVideoWnd.MoveWindow(rc);
}




void CMediaStreamingDemoDlg::OnSelchangeCbogpu()
{
	// TODO: Add your control notification handler code here

	if(m_CboGPU.GetCurSel()==1)
		this->m_CboNVIDAPreset.EnableWindow(true);
	else
		this->m_CboNVIDAPreset.EnableWindow(false);


}


void CMediaStreamingDemoDlg::OnBnClickedBtndetectgpu()
{
	// TODO: Add your control notification handler code here
   this->m_CboGPU.SetCurSel(mMediaCapture.DetectGPUIsInstalled());

   OnSelchangeCbogpu();

}

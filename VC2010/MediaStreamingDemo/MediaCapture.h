#pragma once

#include <dshow.h>
#include <string>


class MediaCapture
{
public:
	MediaCapture(void);
	~MediaCapture(void);

public:
	// devices
	void setVideoDevice(std::wstring deviceName);
	std::wstring getVideoDevice();
	void setAudioDevice(std::wstring deviceName);
	std::wstring getAudioDevice();
	short DetectGPUIsInstalled();

	// output file
	void setFileName(std::wstring fileName);
	std::wstring getFileName();

	// video display
	void setDisplayWindow(HWND wnd);
	
	// capture parameters
	void setWidth(int width);
	int getWidth();
	void setHeight(int height);
	int getHeight();
	void setFramerate(int rate);
	int getFramerate();
	// codec parameters
	void setVideoBitrate(int bitrate);
	int getVideoBitrate();
	void setVideoQuality(int q);
	int getVideoQuality();

	// operations
	bool start();
	void pause();
	void resume();
	void stop();

	const std::wstring getLastError();
	void setMP4FileSetting(int iWidth, int iHeight, int iVideoBitrate, int iAudioBitrate, int iFrameRate, long iAudioSamplerate, int iAudiochannel, bool bAspectRatio);
	void setMP4MetaData(CString strTitle, CString strAuthor, CString strAlbum, CString strComment);
	void setGPU(int index);




private:
	void cleanup();
	void setClippingWindow();
	
private:
	std::wstring mVideoDevice;
	std::wstring mAudioDevice;
	std::wstring mFileName;
	HWND mDisplayWindow;
	int mWidth;
	int mHeight;
	int mFramerate;
	int mVideoBitrate;
	int mVideoQuality;
	int mGPUIndex;
	int mNVIDAPreset;

	std::wstring mLastError;

	// dshow stuff
	DWORD mGraphReg;
	CComPtr<IGraphBuilder>		mGraphBuilder;
	CComPtr<IMediaControl>		mMediaCtrl;
	CComPtr<IMediaEvent>		mMediaEvent;
	CComPtr<IBaseFilter>		mVideoSource;
	CComPtr<IBaseFilter>		mAudioSource;
	CComPtr<IBaseFilter>		mVideoRenderer;
	CComPtr<IBaseFilter>		mAudioRenderer;
	
	CComPtr<IBaseFilter>		mMP4EncoderFilter;

	CComPtr<IBaseFilter>		mTeeFilter;


	CString m_strMyMP4Title;
	CString m_strMyMP4Author;
	CString m_strMyMP4Copyright;
	CString m_strMyMP4Comment;
	CString m_strMyMP4Album;
	

	bool m_bMp4AspectRatio;
	short m_iMp4Width;
	short m_iMp4Height;
	short m_iMp4Audiochannel;

	short m_iMp4FrameRate;
	long m_iMp4Videobitrate;
	long m_iMp4Audiobitrate;
	long m_iMp4AudioSamplerate;
	short m_iMp4H264Profile;
	short m_iMp4VideoBitMode;

private:
	void setLastError(WCHAR* text);
};


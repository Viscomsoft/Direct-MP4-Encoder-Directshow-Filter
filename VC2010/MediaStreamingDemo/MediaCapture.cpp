#include "StdAfx.h"
#include "MediaCapture.h"

//#include <streams.h>

#include "DirectShowHelper.h"
#include <vector>
#include <sstream>

#include <InitGuid.h>

#include "directmp4encoderuids.h"


#ifndef __D3DRM_H__
#define __D3DRM_H__
#endif

#include <d3d9.h>
#include <vmr9.h>
#include <wmsdkidl.h>




DEFINE_GUID(CLSID_ViscomsoftDirectMP4Encoder, 
0x966c8832, 0x5916, 0x453b, 0xbd, 0x6e, 0x2f, 0x35, 0xa1, 0xe1, 0xd, 0x99);


// {D57140A2-DFB8-4f47-8362-84BC0CD5CBA9}
DEFINE_GUID(IID_IDirectMP4EncoderConfig, 
0xd57140a2, 0xdfb8, 0x4f47, 0x83, 0x62, 0x84, 0xbc, 0xc, 0xd5, 0xcb, 0xa9);


//#include <qedit.h>

using namespace std;

const LONGLONG MILLISECONDS = (1000);            // 10 ^ 3
const LONGLONG NANOSECONDS = (1000000000);       // 10 ^ 9
const LONGLONG UNITS = (NANOSECONDS / 100);      // 10 ^ 7

MediaCapture::MediaCapture(void)
	: mFramerate(30)
	, mVideoBitrate(1000000)
	, mVideoQuality(4)
{
	m_iMp4Audiochannel=2;
	m_iMp4Width=1280;
	m_iMp4Height=720;
	m_iMp4FrameRate=25;
	m_iMp4Videobitrate = 4600000;
	m_iMp4Audiobitrate = 96000;
	m_iMp4AudioSamplerate = 48000;
	m_iMp4H264Profile = 0;

}

MediaCapture::~MediaCapture(void)
{
	stop();
}

short MediaCapture::DetectGPUIsInstalled()
{
	HRESULT hr;

	if(mMP4EncoderFilter)
	{
		mMP4EncoderFilter.Release();
		mMP4EncoderFilter=NULL;
	}

	CComPtr<IBaseFilter> pEncoder;

	hr=mMP4EncoderFilter.CoCreateInstance(CLSID_ViscomsoftDirectMP4Encoder);


	if (FAILED(hr))
		return 0;

	CComQIPtr<IMP4Manager, &IID_IDirectMP4EncoderConfig> pProfile(mMP4EncoderFilter);
	
	//pProfile->SetLicenseKey(123);

	ULONG iGPU;
	pProfile->DetectGPU(&iGPU);


	if(mMP4EncoderFilter)
	{
		mMP4EncoderFilter.Release();
		mMP4EncoderFilter=NULL;
	}


	return iGPU;



}

bool MediaCapture::start()
{
	DirectShowHelper dsHelper;
	HRESULT hr;

	do {
		CComPtr<IPin> inPin;
		CComPtr<IPin> outPin;
		CComPtr<IVMRFilterConfig> filterConfig;
		CComPtr<IVMRWindowlessControl> windowlessCtrl;
		
		CRect rc;

		// create GraphBuilder
		hr = dsHelper.createGraphBuilder(&mGraphBuilder);
		if (FAILED(hr)) {
			setLastError(L"ERROR. GraphBuilder");
			break;
		}

		// add graph builder to ROT
#ifdef _DEBUG
		hr = dsHelper.AddToRot(mGraphBuilder, &mGraphReg);
		if (FAILED(hr)) {
			setLastError(L"ERROR. Adding ROT");
			break;
		}
#endif
		// media control
		hr = mGraphBuilder->QueryInterface(IID_IMediaControl, (void**)&mMediaCtrl);
		if (FAILED(hr)) {
			setLastError(L"ERROR. MediaControl");
			break;
		}

		// media event
		hr = mGraphBuilder->QueryInterface(IID_IMediaEvent, (void**)&mMediaEvent);
		if (FAILED(hr)) {
			setLastError(L"ERROR. MediaEvent");
			break;
		}

		if (!mVideoDevice.empty()) {
			// VideoSource: create video source filter
			hr = dsHelper.createFilter(mVideoDevice.c_str(), CLSID_VideoInputDeviceCategory, &mVideoSource);
			if (FAILED(hr)) {
				setLastError(L"ERROR. Webcam");
				break;
			}

			// VideoSource: set media format
			std::vector<AM_MEDIA_TYPE*> mediaTypes;
			dsHelper.enumerateMediaTypes(mVideoDevice.c_str(), CLSID_VideoInputDeviceCategory, mediaTypes);
			for (int i = 0; i < mediaTypes.size(); i++) {
				AM_MEDIA_TYPE* mt = mediaTypes[i];
				if (mt) {
					
					if (mt->majortype == MEDIATYPE_Video 
						&& mt->cbFormat == sizeof(VIDEOINFOHEADER)) 
					{

						
						VIDEOINFOHEADER* vih = (VIDEOINFOHEADER*) mt->pbFormat;
					
						if (vih && vih->bmiHeader.biWidth == mWidth && vih->bmiHeader.biHeight == mHeight) {
							if (mFramerate > 0)
								vih->AvgTimePerFrame = UNITS / mFramerate;
						
								dsHelper.setFilterFormat(mt, mVideoSource);
								break;
							
							
						}
					}
				}
				dsHelper.deleteMediaType(mt);
			}

			//if (mSelectedVideoFormat == VideoFormat::NONE) {
				//setLastError(L"ERROR. Video format");
				//break;
			//}

			// VideoSource: add filter to graph
			hr = mGraphBuilder->AddFilter(mVideoSource, L"Video Source");
			if (FAILED(hr)) {
				setLastError(L"ERROR. Add video capture source");
				break;
			}

			// VideoRenderer: create renderer filter
			hr = dsHelper.createFilter(CLSID_VideoMixingRenderer, &mVideoRenderer);
			//hr = dsHelper.createFilter(CLSID_VideoMixingRenderer9, &mVideoRenderer);
			if (FAILED(hr)) {
				setLastError(L"ERROR. VideoRenderer@1");
				break;
			}

			// VideoRenderer: add renderer to graph
			hr = mGraphBuilder->AddFilter(mVideoRenderer, L"Video Renderer");
			if (FAILED(hr)) {
				setLastError(L"ERROR. VideoRenderer@2");
				break;
			}

			// VideoRenderer: get filter config interface
			hr = mVideoRenderer->QueryInterface(IID_IVMRFilterConfig, (void**)&filterConfig);
			if (FAILED(hr)) {
				setLastError(L"ERROR. VideoRenderer@3");
				break;
			}

			// VideoRenderer: set rendering mode
			hr = filterConfig->SetRenderingMode(VMRMode_Windowless);
			if (FAILED(hr)) {
				setLastError(L"ERROR. VideoRenderer@4");
				break;
			}

			filterConfig->SetRenderingPrefs(RenderPrefs_ForceOffscreen| RenderPrefs_AllowOffscreen );


			filterConfig.Release();
			setClippingWindow();
		}

		if (!mAudioDevice.empty()) {
			// AudioSource: create audio source filter
			hr = dsHelper.createFilter(mAudioDevice.c_str(), CLSID_AudioInputDeviceCategory, &mAudioSource);
			if (FAILED(hr)) {
				setLastError(L"ERROR. Microphone");
				break;
			}

			// AudioSource: set media format
			/*
			std::vector<AM_MEDIA_TYPE*> mediaTypes;
			dsHelper.enumerateMediaTypes(mAudioDevice.c_str(), CLSID_AudioInputDeviceCategory, mediaTypes);
			for (int i = 0; i < mediaTypes.size(); i++) {
				AM_MEDIA_TYPE* mt = mediaTypes[i];
				if (mt) {
					if (mt->majortype == MEDIATYPE_Audio && 
						(mt->subtype == MEDIASUBTYPE_PCM) && 
						mt->cbFormat == sizeof(WAVEFORMATEX)) {
						WAVEFORMATEX* wfx = (WAVEFORMATEX*) mt->pbFormat;
						if (wfx && 
							wfx->wFormatTag == WAVE_FORMAT_PCM && 
							wfx->nSamplesPerSec == getSamplingFrequency() &&
							wfx->nChannels == 2 &&
							wfx->wBitsPerSample == 16) {
							dsHelper.setFilterFormat(mt, mAudioSource);
						}
					}
				}
				dsHelper.deleteMediaType(mt);
			}
			*/

			// AudioSource: add filter to graph
			hr = mGraphBuilder->AddFilter(mAudioSource, L"Audio Source");
			if (FAILED(hr)) {
				setLastError(L"ERROR. Add audio capture source");
				break;
			}
		}

		// MediaStreaming: create media streaming filter

		

		hr = dsHelper.createFilter(CLSID_ViscomsoftDirectMP4Encoder, &mMP4EncoderFilter);
		
		

		hr = mGraphBuilder->AddFilter(mMP4EncoderFilter, L"Direct MP4 Encoder");

		

	

	CComQIPtr<IFileSinkFilter> pSink(mMP4EncoderFilter);
	if(!pSink)
	{
		AfxMessageBox(_T("Failed to set file name"));
		return hr;
	}
	pSink->SetFileName(mFileName.c_str(), NULL);



		CComQIPtr<IMP4Manager, &IID_IDirectMP4EncoderConfig> pProfile(mMP4EncoderFilter);
		//pProfile->SetLicenseKey(123);
	
		
	
		CString strPreset=L"faster";

		int m_iMP4H264Preset=1;

		
		if(mGPUIndex==1)
		{
			if(mNVIDAPreset==0)
				strPreset="slow";
			else if(mNVIDAPreset==1)
				strPreset="medium";
			else if(mNVIDAPreset==2)
				strPreset="fast";
			else if(mNVIDAPreset==3)
				strPreset="hp";
			else if(mNVIDAPreset==4)
				strPreset="hq";
			else if(mNVIDAPreset==5)
				strPreset="bd";
			else if(mNVIDAPreset==6)
				strPreset="ll";
			else if(mNVIDAPreset==7)
				strPreset="llhq";
			else if(mNVIDAPreset==8)
				strPreset="llhp";
			else if(mNVIDAPreset==9)
				strPreset="lossless";
			else if(mNVIDAPreset==10)
				strPreset="losslesshp";


		}
		else
			strPreset=L"superfast";
		
		pProfile->SetH264Preset(strPreset.AllocSysString());

		pProfile->SetHwCodec(mGPUIndex);


		

		int m_iMP4AudioChannels=2;
	

		

		

			
		pProfile->SetMp4AspectRatio(m_bMp4AspectRatio);

	
		pProfile->SetMp4VideoBitrate(m_iMp4Videobitrate);

		
		pProfile->SetMp4AudioBitrate(m_iMp4Audiobitrate);
		pProfile->SetMp4AudioSampleRate(m_iMp4AudioSamplerate);
		pProfile->SetMp4AudioChannel(m_iMp4Audiochannel);
		
		pProfile->SetMp4FrameRate(m_iMp4FrameRate);
		
	
		pProfile->SetMp4Resolution(m_iMp4Width,m_iMp4Height);



		pProfile->SetTitle(m_strMyMP4Title.AllocSysString());
		pProfile->SetAlbum(m_strMyMP4Album.AllocSysString());
		pProfile->SetAuthor(m_strMyMP4Author.AllocSysString());
		pProfile->SetComment(m_strMyMP4Comment.AllocSysString());
		pProfile->SetCopyright(m_strMyMP4Comment.AllocSysString());

		
		// Tee: create tee filter
		hr = dsHelper.createFilter(CLSID_SmartTee, &mTeeFilter);
		if (FAILED(hr)) {
			setLastError(L"ERROR. TeeFilter");
			break;
		}

		// Tee: add tee filter to graph
		hr = mGraphBuilder->AddFilter(mTeeFilter, L"Tee");
		if (FAILED(hr)) {
			setLastError(L"ERROR. TeeFilter@2");
			break;
		}

		// Connection: connect video source -> tee
		hr = dsHelper.connectFilters(mGraphBuilder, mVideoSource, mTeeFilter);
		if (FAILED(hr)) {
			setLastError(L"ERROR. Connect video source -> tee");
			break;
		}

		// Connection: connect tee -> mediastreaming
		hr = dsHelper.connectFilters(mGraphBuilder, mTeeFilter, mMP4EncoderFilter);
		if (FAILED(hr)) {
			setLastError(L"ERROR. Connect tee -> mp4encoder");
			break;
		}

		// Connection: connect tee -> video renderer
		hr = dsHelper.connectFilters(mGraphBuilder, mTeeFilter, mVideoRenderer);
		if (FAILED(hr)) {
			setLastError(L"ERROR. Connect tee -> video renderer");
			break;
		}

		// Connection: connect audio source -> mediastreaming
		hr = dsHelper.connectFilters(mGraphBuilder, mAudioSource,mMP4EncoderFilter);
		if (FAILED(hr)) {
			setLastError(L"ERROR. Connect audio source -> mp4encoder");
			break;
		}


		// run graph
		hr = mMediaCtrl->Run();
		if (FAILED(hr)) {
			setLastError(L"ERROR. Start streaming filter");
			break;
		}

		return true;
	} while (0);

	cleanup();
	return false;
}

void MediaCapture::pause()
{
	HRESULT hr;

	CComQIPtr<IMediaControl,&IID_IMediaControl> pMediaControl(mGraphBuilder);
	hr = pMediaControl->Pause();

	
}

void MediaCapture::resume()
{
	HRESULT hr;

	CComQIPtr<IMediaControl,&IID_IMediaControl> pMediaControl(mGraphBuilder);
	hr = pMediaControl->Run();

}

void MediaCapture::stop()
{

	if (mMediaCtrl.p)
		mMediaCtrl->Stop();
	cleanup();
}

void MediaCapture::cleanup()
{
	if (mGraphReg != 0) {
		DirectShowHelper ds;
		ds.RemoveFromRot(mGraphReg);
		mGraphReg = 0;
	}

	mGraphBuilder.Release();
	mMediaCtrl.Release();
	mMediaEvent.Release();
	mVideoSource.Release();
	mAudioSource.Release();
	mVideoRenderer.Release();
	mMP4EncoderFilter.Release();
	mTeeFilter.Release();
}

void MediaCapture::setFileName(wstring fileName)
{
	mFileName = fileName;
}
void MediaCapture::setMP4MetaData(CString strTitle, CString strAuthor, CString strAlbum, CString strComment)
{

	m_strMyMP4Title = strTitle;
	m_strMyMP4Author = strAuthor;
	m_strMyMP4Comment =strComment;
	m_strMyMP4Album = strAlbum;;
	

}
void MediaCapture::setMP4FileSetting(int iWidth, int iHeight, int iVideoBitrate, int iAudioBitrate, int iFrameRate, long iAudioSamplerate, int iAudiochannel, bool bAspectRatio)
{

	m_iMp4Width =iWidth;
	m_iMp4Height =iHeight;

	m_bMp4AspectRatio = bAspectRatio;
	m_iMp4Audiochannel = iAudiochannel;
	m_iMp4AudioSamplerate =  iAudioSamplerate;
	m_iMp4FrameRate = iFrameRate;
	m_iMp4Videobitrate = iVideoBitrate;
	m_iMp4Audiobitrate = iAudioBitrate;

}


wstring MediaCapture::getFileName()
{
	return mFileName;
}

void MediaCapture::setDisplayWindow(HWND wnd)
{
	mDisplayWindow = wnd;
}

void MediaCapture::setVideoDevice(wstring deviceName)
{
	mVideoDevice = deviceName;
}

wstring MediaCapture::getVideoDevice()
{
	return mVideoDevice;
}

void MediaCapture::setWidth(int width)
{
	mWidth = width;
}

int MediaCapture::getWidth()
{
	return mWidth;
}

void MediaCapture::setHeight(int height)
{
	mHeight = height;
}

int MediaCapture::getHeight()
{
	return mHeight;
}

void MediaCapture::setFramerate(int rate)
{
	mFramerate = rate;
}

int MediaCapture::getFramerate()
{
	return mFramerate;
}

void MediaCapture::setVideoBitrate(int bitrate)
{
	mVideoBitrate = bitrate;
}

void MediaCapture::setGPU(int index)
{
	mGPUIndex= index;
}

int MediaCapture::getVideoBitrate()
{
	return mVideoBitrate;
}


void MediaCapture::setVideoQuality(int q)
{
	mVideoQuality = q;
}

int MediaCapture::getVideoQuality()
{
	return mVideoQuality;
}

void MediaCapture::setAudioDevice(wstring deviceName)
{
	mAudioDevice = deviceName;
}

wstring MediaCapture::getAudioDevice()
{
	return mAudioDevice;
}

void MediaCapture::setLastError(WCHAR* text)
{
	mLastError = text;
}

const wstring MediaCapture::getLastError()
{
	return mLastError;
}

void MediaCapture::setClippingWindow()
{
	CComPtr<IVMRWindowlessControl> windowlessCtrl;
	RECT rc;
	HRESULT hr;

	if (mVideoRenderer) {
		hr = mVideoRenderer->QueryInterface(IID_IVMRWindowlessControl, (void**)&windowlessCtrl);
		if (FAILED(hr)) {
			setLastError(L"ERROR. VideoRenderer@5");
			return;
		}
	}

	if (windowlessCtrl) {
		windowlessCtrl->SetVideoClippingWindow(mDisplayWindow);
		GetClientRect(mDisplayWindow, &rc);
		windowlessCtrl->SetVideoPosition(0, &rc);

		windowlessCtrl->SetAspectRatioMode(VMR_ARMODE_LETTER_BOX);

		windowlessCtrl.Release();
	}
}


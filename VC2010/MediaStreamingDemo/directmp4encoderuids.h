//------------------------------------------------------------------------------
//
// Desc: CLSIDs used by the Direct MP4 Encoder filter.
//------------------------------------------------------------------------------
#pragma once



#include <windows.h>


EXTERN_C const CLSID CLSID_ViscomsoftDirectMP4Encoder;
EXTERN_C const CLSID IID_IDirectMP4EncoderConfig;



class IMP4Manager : public IUnknown
{
public:
	STDMETHOD(SetMp4VideoBitrate)(ULONG nVideoBitrate)=0;
	STDMETHOD(SetMp4AudioBitrate)(ULONG nAudioBitrate)=0;
	STDMETHOD(SetMp4AudioSampleRate)(ULONG nAudioSample)=0;
	STDMETHOD(SetMp4AudioChannel)(ULONG nAudiochannel)=0;
	STDMETHOD(SetMp4FrameRate)(ULONG nFrameRate)=0;
	STDMETHOD(SetMp4Resolution)(ULONG nWidth,ULONG nHeight)=0;
	STDMETHOD(SetMp4AspectRatio)(bool bEnableAspectRatio)=0;

	STDMETHOD(SetTitle)(BSTR bstr)=0;
	STDMETHOD(SetAlbum)(BSTR bstr)=0;
	STDMETHOD(SetAuthor)(BSTR bstr)=0;
	STDMETHOD(SetComment)(BSTR bstr)=0;
	STDMETHOD(SetCopyright)(BSTR bstr)=0;
	STDMETHOD(SetLicenseKey)(ULONG nIndex)=0;
	
	// Hardware codec
	//  0 None
	//  1 Nvidia
	//  2 AMD
	//  3 Intel
	STDMETHOD(SetHwCodec)(ULONG nHwCodec) = 0;
	STDMETHOD(DetectGPU)(ULONG* nGPU) = 0;


	// Detect GPU
	//   returns:
	//   0 None
	//   1 Nvidia
	//   2 AMD
	//   3 Intel


	// H.264 presets: ultrafast, superfast, veryfast, faster, fast, medium, slow, slower, veryslow, placebo
	STDMETHOD(SetH264Preset)(BSTR bstr)=0;

	// H.264 profiles: baseline, main, high
	STDMETHOD(SetH264Profile)(BSTR bstr)=0;

	
	
};

extern HRESULT E_INVALID_FILE_FORMAT;
extern HRESULT E_INVALID_FRAMERATE;
extern HRESULT E_INVALID_FRAME_SIZE;
extern HRESULT E_INVALID_QVBR;
extern HRESULT E_UNKNOWN_VIDEO_CODEC;
extern HRESULT E_UNKNOWN_AUDIO_CODEC;
extern HRESULT E_INVALID_OUTPUT_FORMAT;
extern HRESULT E_VIDEO_PARAM_WRONG;
extern HRESULT E_AUDIO_PARAM_WRONG;
extern HRESULT E_INVALID_ENCODER_SETTINGS;
extern HRESULT E_COULD_NOT_ALLOC_AUDIO_STREAM;
extern HRESULT E_COULD_NOT_ALLOC_VIDEO_STREAM;
extern HRESULT E_COULD_NOT_FIND_VIDEO_CODEC;
extern HRESULT E_UNSUPPORTED_CODEC;
extern HRESULT E_ERROR_OPEN_CODEC;
extern HRESULT E_COULD_NOT_WRITE_HEADER;
extern HRESULT E_VIDEO_ENCODING_ABORTED;
extern HRESULT E_VIDEO_SCALER_INIT_ERROR;
extern HRESULT E_INVALID_OUTPUT_RESOLUTION;
extern HRESULT E_INVALID_INPUT_RESOLUTION;
extern HRESULT E_AUDIO_CODEC_INIT_FAILED;
extern HRESULT E_VIDEO_CODEC_INIT_FAILED;

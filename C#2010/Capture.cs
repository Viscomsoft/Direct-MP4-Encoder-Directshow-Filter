
using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using DirectShowLib;
using System.Drawing;

namespace WindowsFormsApplication1
{
	/// <summary> Summary description for MainForm. </summary>
    internal class Capture : IDisposable
	{
        #region Member variables

        /// <summary> graph builder interface. </summary>
		private IFilterGraph2 m_FilterGraph = null;
        IMediaControl m_mediaCtrl = null;

        /// <summary> Set by async routine when it captures an image </summary>
        private bool m_bRunning = false;
        public string m_strURL;
//#if DEBUG
        DsROTEntry m_rot = null;
        //#endif

        #endregion

        [Guid("D57140A2-DFB8-4f47-8362-84BC0CD5CBA9"),
        InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        interface IDirectMP4EncoderConfig
        {
            void SetMp4VideoBitrate([In] int nVideoBitrate);
            void SetMp4AudioBitrate([In] int nAudioBitrate);
            void SetMp4AudioSampleRate([In] int nAudioSample);
            void SetMp4AudioChannel([In] int nAudiochannel);
            void SetMp4FrameRate([In] int nFrameRate);
            void SetMp4Resolution([In] int nWidth, [In] int nHeight);
            void SetMp4AspectRatio([In] bool bEnableAspectRatio);
            void SetTitle([In] IntPtr bstr);
            void SetAlbum([In] IntPtr bstr);
            void SetAuthor([In] IntPtr bstr);
            void SetComment([In] IntPtr bstr);
            void SetCopyright([In] IntPtr bstr);
            void setLicenseKey([In] int nVal);
            void SetHwCodec([In] int nHwCodec);
            void DetectGPU([In] ref int nGPU);
            void SetH264Preset([In] IntPtr bstr);
          
        }


        

        /// <summary> release everything. </summary>
        public void Dispose()
        {
            GC.SuppressFinalize(this);
            CloseInterfaces();
        }

        ~Capture()
        {
            Dispose();
        }
       
        /// <summary>
        /// Create capture object
        /// </summary>
        /// <param name="iDeviceNum">Zero based index of capture device</param>
        public Capture(int iDeviceNum, int iAudioDeviceNum, IntPtr windowHandle, Rectangle videoWndRect, int iWidth, int iHeight, int iMP4Width, int iMP4Height, int iMP4VideoBitrate, int iMP4FrameRate, int iMP4AudioChannels, int iMP4AudioSampleRate, int iMP4AudioBitrate, string strMP4Title, string strMP4Album, string strMP4Author, string strMP4Comment, bool bAspectRatio, string strMP4Filename, int iGPUIndex, int iNVIDAPreset)
        {
            DsDevice [] capDevices;
            DsDevice[] capAudioDevices;

            
              // Get the collection of video devices
            capDevices = DsDevice.GetDevicesOfCat( FilterCategory.VideoInputDevice );

            if (iDeviceNum + 1 > capDevices.Length)
            {
                throw new Exception("No video capture devices found at that index!");
            }

            capAudioDevices = DsDevice.GetDevicesOfCat(FilterCategory.AudioInputDevice);

            if (iAudioDeviceNum + 1 > capAudioDevices.Length)
            {
                throw new Exception("No audio capture devices found at that index!");
            }

            try
            {
                // Set up the capture graph
                SetupGraph(capDevices[iDeviceNum], capAudioDevices[iAudioDeviceNum], windowHandle, videoWndRect, iWidth, iHeight, iMP4Width, iMP4Height, iMP4VideoBitrate, iMP4FrameRate, iMP4AudioChannels, iMP4AudioSampleRate, iMP4AudioBitrate, strMP4Title, strMP4Album, strMP4Author, strMP4Comment, bAspectRatio, strMP4Filename,iGPUIndex, iNVIDAPreset);

                m_bRunning = false;
            }
            catch
            {
                Dispose();
                throw;
            }
        }


        // Start the capture graph
        public void Start()
        {
            if (!m_bRunning)
            {
                int hr = m_mediaCtrl.Run();
                //Marshal.ThrowExceptionForHR( hr );

                m_bRunning = true;
            }
        }

        // Pause the capture graph.
        // Running the graph takes up a lot of resources.  Pause it when it
        // isn't needed.
        public void Stop()
        {
            if (m_bRunning)
            {
                IMediaControl mediaCtrl = m_FilterGraph as IMediaControl;

                int hr = mediaCtrl.Stop();
          
                m_bRunning = false;
            }
        }

        /// <summary> build the capture graph. </summary>
        /// 
        private bool SetVideoCaptureParameters(ICaptureGraphBuilder2 capGraph, IBaseFilter captureFilter, Guid mediaSubType, int iWidth, int iHeight)
        {
            /* The stream config interface */
            object streamConfig;

            /* Get the stream's configuration interface */
            int hr = capGraph.FindInterface(PinCategory.Capture,
                                            MediaType.Video,
                                            captureFilter,
                                            typeof(IAMStreamConfig).GUID,
                                            out streamConfig);

            DsError.ThrowExceptionForHR(hr);

            var videoStreamConfig = streamConfig as IAMStreamConfig;

            /* If QueryInterface fails... */
            if (videoStreamConfig == null)
            {
                throw new Exception("Failed to get IAMStreamConfig");
            }

            ///* Make the VIDEOINFOHEADER 'readable' */
            var videoInfo = new VideoInfoHeader();

            int iCount = 0, iSize = 0;
            videoStreamConfig.GetNumberOfCapabilities(out iCount, out iSize);

            IntPtr TaskMemPointer = Marshal.AllocCoTaskMem(iSize);


            AMMediaType pmtConfig = null;
            for (int iFormat = 0; iFormat < iCount; iFormat++)
            {
                IntPtr ptr = IntPtr.Zero;

                videoStreamConfig.GetStreamCaps(iFormat, out pmtConfig, TaskMemPointer);

                videoInfo = (VideoInfoHeader)Marshal.PtrToStructure(pmtConfig.formatPtr, typeof(VideoInfoHeader));

                if (videoInfo.BmiHeader.Width == iWidth && videoInfo.BmiHeader.Height == iHeight)
                {

                    ///* Setup the VIDEOINFOHEADER with the parameters we want */
                    //videoInfo.AvgTimePerFrame = (1000000000 /100) / 25;

                    if (mediaSubType != Guid.Empty)
                    {
                        int fourCC = 0;
                        byte[] b = mediaSubType.ToByteArray();
                        fourCC = b[0];
                        fourCC |= b[1] << 8;
                        fourCC |= b[2] << 16;
                        fourCC |= b[3] << 24;

                        videoInfo.BmiHeader.Compression = fourCC;
                 
                    }

                    /* Copy the data back to unmanaged memory */
                    Marshal.StructureToPtr(videoInfo, pmtConfig.formatPtr, true);

                    hr = videoStreamConfig.SetFormat(pmtConfig);
                    break;
                }

            }

            /* Free memory */
            Marshal.FreeCoTaskMem(TaskMemPointer);
            DsUtils.FreeAMMediaType(pmtConfig);

            if (hr < 0)
                return false;

            return true;
        }
      
        private void SetupGraph(DsDevice dev, DsDevice audiodev, IntPtr windowHandle, Rectangle videoWndRect, int iWidth, int iHeight, int iMP4Width, int iMP4Height, int iMP4VideoBitrate, int iMP4FrameRate, int iMP4AudioChannels, int iMP4AudioSampleRate, int iMP4AudioBitrate, string strMP4Title, string strMP4Album, string strMP4Author, string strMP4Comment, bool bAspectRatio, string strMP4Filename, int iGPUIndex, int iNVIDAPreset)
		{
            int hr;
            
            IBaseFilter capFilter = null;
            IBaseFilter audiocapFilter = null;
            ICaptureGraphBuilder2 capGraph = null;
            IBaseFilter mp4EncoderFilter = null;

            IBaseFilter vmr9Filter = null;
            IBaseFilter mTeeFilter = null;

            // Get the graphbuilder object
            m_FilterGraph = (IFilterGraph2)new FilterGraph();

//#if DEBUG
            m_rot = new DsROTEntry( m_FilterGraph );

        

//#endif

            try
            {
                // Get the ICaptureGraphBuilder2
                capGraph = (ICaptureGraphBuilder2) new CaptureGraphBuilder2();

                // Start building the graph
                hr = capGraph.SetFiltergraph( m_FilterGraph );
                Marshal.ThrowExceptionForHR( hr );

                // Add the capture device to the graph
                hr = m_FilterGraph.AddSourceFilterForMoniker(dev.Mon, null, dev.Name, out capFilter);
                Marshal.ThrowExceptionForHR( hr );

                hr = m_FilterGraph.AddSourceFilterForMoniker(audiodev.Mon, null, audiodev.Name, out audiocapFilter);
                Marshal.ThrowExceptionForHR(hr);


                SetVideoCaptureParameters(capGraph, capFilter, Guid.Empty, iWidth, iHeight);

                //Insert MP4 encoder
              
                Guid guid = new Guid("966C8832-5916-453B-BD6E-2F35A1E10D99");
                Type comtype = Type.GetTypeFromCLSID(guid);
                mp4EncoderFilter = (IBaseFilter)Activator.CreateInstance(comtype);


                m_FilterGraph.AddFilter(mp4EncoderFilter, "Direct MP4 Encoder");

                IDirectMP4EncoderConfig myMP4FilterConfig = (IDirectMP4EncoderConfig)mp4EncoderFilter;
            
               

                 myMP4FilterConfig.SetHwCodec(iGPUIndex);

                  
                string strPreset="";

                if (iGPUIndex == 1)
                {
                    if (iNVIDAPreset == 0)
                        strPreset = "slow";
                    else if (iNVIDAPreset == 1)
                        strPreset = "medium";
                    else if (iNVIDAPreset == 2)
                        strPreset = "fast";
                    else if (iNVIDAPreset == 3)
                        strPreset = "hp";
                    else if (iNVIDAPreset == 4)
                        strPreset = "hq";
                    else if (iNVIDAPreset == 5)
                        strPreset = "bd";
                    else if (iNVIDAPreset == 6)
                        strPreset = "ll";
                    else if (iNVIDAPreset == 7)
                        strPreset = "llhq";
                    else if (iNVIDAPreset == 8)
                        strPreset = "llhp";
                    else if (iNVIDAPreset == 9)
                        strPreset = "lossless";
                    else if (iNVIDAPreset == 10)
                        strPreset = "losslesshp";

                }
                else
                {
                    strPreset="superfast";

                }

                IntPtr str1 = Marshal.StringToBSTR(strPreset);
                myMP4FilterConfig.SetH264Preset(str1);

                IntPtr strMyTitle = Marshal.StringToBSTR(strMP4Title);
                myMP4FilterConfig.SetTitle( strMyTitle);
              
                myMP4FilterConfig.SetMp4Resolution(iMP4Width, iMP4Height);
                myMP4FilterConfig.SetMp4VideoBitrate(iMP4VideoBitrate);
                myMP4FilterConfig.SetMp4FrameRate(iMP4FrameRate);
                myMP4FilterConfig.SetMp4AudioBitrate(iMP4AudioBitrate);
                myMP4FilterConfig.SetMp4AudioChannel(iMP4AudioChannels);
                myMP4FilterConfig.SetMp4AudioSampleRate(iMP4AudioSampleRate);
                myMP4FilterConfig.SetMp4AspectRatio(bAspectRatio);

                Marshal.FreeBSTR(str1);
               Marshal.FreeBSTR(strMyTitle);

             
                IFileSinkFilter sinkFilter = (IFileSinkFilter)mp4EncoderFilter;
                sinkFilter.SetFileName(strMP4Filename, null);

                //vmr9Filter = (IBaseFilter)new  VideoMixingRenderer9();
                vmr9Filter = (IBaseFilter)new VideoMixingRenderer();

                m_FilterGraph.AddFilter(vmr9Filter, "Video Renderer");

               // IVMRFilterConfig9 FilterConfig = (IVMRFilterConfig9)vmr9Filter;
               // FilterConfig.SetRenderingMode(VMR9Mode.Windowless);

                 IVMRFilterConfig FilterConfig = (IVMRFilterConfig)vmr9Filter;
                 FilterConfig.SetRenderingMode(VMRMode.Windowless);
                 FilterConfig.SetRenderingPrefs(VMRRenderPrefs.ForceOffscreen | VMRRenderPrefs.AllowOffscreen);

                 IVMRWindowlessControl windowlessCtrl = (IVMRWindowlessControl)vmr9Filter;
                   windowlessCtrl.SetVideoClippingWindow(windowHandle);
                  windowlessCtrl.SetVideoPosition(null, DsRect.FromRectangle(videoWndRect));
                  windowlessCtrl.SetAspectRatioMode(VMRAspectRatioMode.LetterBox);

             //   IVMRWindowlessControl9 windowlessCtrl = (IVMRWindowlessControl9)vmr9Filter;
             //   windowlessCtrl.SetVideoClippingWindow(windowHandle);
              //  windowlessCtrl.SetVideoPosition(null, DsRect.FromRectangle(videoWndRect));
              //  windowlessCtrl.SetAspectRatioMode(VMR9AspectRatioMode.LetterBox);


               mTeeFilter = (IBaseFilter)new SmartTee();
                m_FilterGraph.AddFilter(mTeeFilter, "Tee");


                    hr = capGraph.RenderStream(PinCategory.Preview, MediaType.Video, capFilter, null, mTeeFilter);
                hr = capGraph.RenderStream(null,MediaType.Video, mTeeFilter, null, vmr9Filter);


                hr = capGraph.RenderStream(PinCategory.Capture, MediaType.Video, capFilter, null, mp4EncoderFilter);



                hr = capGraph.RenderStream(PinCategory.Capture, MediaType.Audio, audiocapFilter, null, mp4EncoderFilter);



             Marshal.ThrowExceptionForHR( hr );

                
                m_mediaCtrl = m_FilterGraph as IMediaControl;
            }
            finally
            {
                if (capFilter != null)
                {
                    Marshal.ReleaseComObject(capFilter);
                    capFilter = null;
                }

                if (audiocapFilter != null)
                {
                    Marshal.ReleaseComObject(audiocapFilter);
                    audiocapFilter = null;

                }

                if (mp4EncoderFilter != null)
                {
                    Marshal.ReleaseComObject(mp4EncoderFilter);
                    mp4EncoderFilter = null;

                }
             
                if (capGraph != null)
                {
                    Marshal.ReleaseComObject(capGraph);
                    capGraph = null;
                }
            }
        }

       

        /// <summary> Shut down capture </summary>
		private void CloseInterfaces()
		{
            int hr;

            try
			{
				if( m_mediaCtrl != null )
				{
                    // Stop the graph
                    hr = m_mediaCtrl.Stop();
                    m_bRunning = false;
				}
			}
			catch
			{
			}

//#if DEBUG
            // Remove graph from the ROT
            if ( m_rot != null )
            {
                m_rot.Dispose();
                m_rot = null;
            }
//#endif

            if (m_FilterGraph != null)
            {
                Marshal.ReleaseComObject(m_FilterGraph);
                m_FilterGraph = null;
            }
        }
    }
}

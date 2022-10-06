using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DirectShowLib;
using System.Runtime.InteropServices;

namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

            cboGPU.Items.Add("None");
            cboGPU.Items.Add("Nvida");
            cboGPU.Items.Add("AMD");
            cboGPU.Items.Add("Intel");
            cboGPU.SelectedIndex = 0;

         
            cboNVIDAPreset.Items.Add("slow");
	        cboNVIDAPreset.Items.Add("medium");
    	    cboNVIDAPreset.Items.Add("fast");
	        cboNVIDAPreset.Items.Add("high performance");
    	    cboNVIDAPreset.Items.Add("high quality");
	        cboNVIDAPreset.Items.Add("bluray disk");
	        cboNVIDAPreset.Items.Add("low latency");
	        cboNVIDAPreset.Items.Add("low latency high quality");
	        cboNVIDAPreset.Items.Add("low latency high performance");
	        cboNVIDAPreset.Items.Add("lossless");
            cboNVIDAPreset.Items.Add("lossless high performance");
            cboNVIDAPreset.SelectedIndex = 3;

            //enumerate Video Input filters and add them to comboBox1
            foreach (DsDevice ds in DsDevice.GetDevicesOfCat(FilterCategory.VideoInputDevice))
            {
                cboVideoDevice.Items.Add(ds.Name);

            }

            if (cboVideoDevice.Items.Count > 0)
                cboVideoDevice.SelectedIndex = 0;

            foreach (DsDevice ds in DsDevice.GetDevicesOfCat(FilterCategory.AudioInputDevice))
            {
                cboAudioDevice.Items.Add(ds.Name);
            }

            if (cboAudioDevice.Items.Count > 0)
                cboAudioDevice.SelectedIndex = 0;


            

            cboaudiosample.Items.Add("48000");
            cboaudiosample.Items.Add("44100");
            cboaudiosample.SelectedIndex = 0;

            cboaudiobitrate.Items.Add("96000");
            cboaudiobitrate.Items.Add("128000");
            cboaudiobitrate.Items.Add("1600000");
            cboaudiobitrate.Items.Add("1920000");
            cboaudiobitrate.SelectedIndex = 0;

        }
        Capture cam = null;

        private void GetAllAvailableResolution(DsDevice vidDev)
        {
            try
            {
                int hr, bitCount = 0;

                IBaseFilter sourceFilter = null;

                var m_FilterGraph2 = new FilterGraph() as IFilterGraph2;
                hr = m_FilterGraph2.AddSourceFilterForMoniker(vidDev.Mon, null, vidDev.Name, out sourceFilter);
                var pRaw2 = DsFindPin.ByCategory(sourceFilter, PinCategory.Capture, 0);
                //var AvailableResolutions = new List<string>();

                VideoInfoHeader v = new VideoInfoHeader();
                IEnumMediaTypes mediaTypeEnum;
                hr = pRaw2.EnumMediaTypes(out mediaTypeEnum);

                AMMediaType[] mediaTypes = new AMMediaType[1];
                IntPtr fetched = IntPtr.Zero;
                hr = mediaTypeEnum.Next(1, mediaTypes, fetched);

                while (fetched != null && mediaTypes[0] != null)
                {
                    Marshal.PtrToStructure(mediaTypes[0].formatPtr, v);
                    if (v.BmiHeader.Size != 0 && v.BmiHeader.BitCount != 0)
                    {
                        if (v.BmiHeader.BitCount > bitCount)
                        {
                            //AvailableResolutions.Clear();
                            cboVideoFormat.Items.Clear();
                            bitCount = v.BmiHeader.BitCount;
                        }

                        int iResult = cboVideoFormat.FindString(v.BmiHeader.Width + "x" + v.BmiHeader.Height);
                        if (iResult == -1)
                            cboVideoFormat.Items.Add(v.BmiHeader.Width + "x" + v.BmiHeader.Height);
                    }
                    hr = mediaTypeEnum.Next(1, mediaTypes, fetched);
                }
                //return AvailableResolutions;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                //   return new List<string>();
            }
        }
      
        private void button1_Click(object sender, EventArgs e)
        {
            int iwidth = cboVideoFormat.Text.IndexOf("x");

            if (iwidth == -1)
            {
                MessageBox.Show("Video Format Error");
                return;
            }
            string strWidth = cboVideoFormat.Text.Substring(0, iwidth);

            string strHeight = cboVideoFormat.Text.Substring(iwidth + 1);

            Cursor.Current = Cursors.WaitCursor;

            this.saveFileDialog1.Filter = "MP4 File (*.mp4)|*.mp4";
            
                if (cam == null)
                {
                    if (this.saveFileDialog1.ShowDialog(this) == DialogResult.OK)
                    {
                    cam = new Capture(cboVideoDevice.SelectedIndex, cboAudioDevice.SelectedIndex, pictureBox1.Handle, pictureBox1.ClientRectangle, Int32.Parse(strWidth), Int32.Parse(strHeight), Int32.Parse(txtWidth.Text), Int32.Parse(txtHeight.Text),
                       Int32.Parse(txtvideobitrate.Text), Int32.Parse(txtFrameRate.Text), Int32.Parse(txtAudioChannels.Text), Int32.Parse(cboaudiosample.Text), Int32.Parse(cboaudiobitrate.Text), txtTitle.Text, txtAlbum.Text, txtAuthor.Text, txtComment.Text, chkaspectratio.Checked,saveFileDialog1.FileName,cboGPU.SelectedIndex,cboNVIDAPreset.SelectedIndex);
                    cam.Start();
                    button1.Text = "Stop";
                    }
                }
                else
                {
                    button1.Text = "Start";


                    // Pause the recording
                    cam.Stop();

                    // Close it down
                    cam.Dispose();
                    cam = null;
                }
            

            Cursor.Current = Cursors.Default;
        }

        private void cboVideoDevice_SelectedIndexChanged(object sender, EventArgs e)
        {
            cboVideoFormat.Items.Clear();
            int videodeviceindex = cboVideoDevice.SelectedIndex;

            if (videodeviceindex != -1)
            {
                int i = 0;

                foreach (DsDevice ds in DsDevice.GetDevicesOfCat(FilterCategory.VideoInputDevice))
                {

                    if (i == videodeviceindex)
                    {
                        GetAllAvailableResolution(ds);
                        if (cboVideoFormat.Items.Count > 0)
                            cboVideoFormat.SelectedIndex = 0;
                    }
                    i++;
                }


            }
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (cam != null)
            {
                cam.Stop();

                // Close it down
                cam.Dispose();
                cam = null;
            }
        }

        private void cboGPU_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboGPU.SelectedIndex == 1)
                cboNVIDAPreset.Enabled = true;
            else
                cboNVIDAPreset.Enabled = false;
     
        }

        private void btnDetectGPU_Click(object sender, EventArgs e)
        {

            cboGPU.SelectedIndex = DetectGPUIsInstalled();

            //int a = Capture. DetectGPUIsInstalled();
        }

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

        public int DetectGPUIsInstalled()
        {
            IBaseFilter mp4EncoderFilterTmp = null;
            Guid guid = new Guid("966C8832-5916-453B-BD6E-2F35A1E10D99");
            Type comtype = Type.GetTypeFromCLSID(guid);
            mp4EncoderFilterTmp = (IBaseFilter)Activator.CreateInstance(comtype);

            IDirectMP4EncoderConfig myMP4FilterConfigTmp = (IDirectMP4EncoderConfig)mp4EncoderFilterTmp;

            int iGPU = 0;
            myMP4FilterConfigTmp.DetectGPU(ref iGPU);

            if (mp4EncoderFilterTmp != null)
            {
                Marshal.ReleaseComObject(mp4EncoderFilterTmp);
                mp4EncoderFilterTmp = null;

            }

            return iGPU;

        }
       
    }
}

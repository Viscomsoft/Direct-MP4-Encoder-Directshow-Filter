namespace WindowsFormsApplication1
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.cboaudiobitrate = new System.Windows.Forms.ComboBox();
            this.label8 = new System.Windows.Forms.Label();
            this.cboaudiosample = new System.Windows.Forms.ComboBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.txtvideobitrate = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.cboVideoFormat = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.cboAudioDevice = new System.Windows.Forms.ComboBox();
            this.cboVideoDevice = new System.Windows.Forms.ComboBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.button1 = new System.Windows.Forms.Button();
            this.txtFrameRate = new System.Windows.Forms.TextBox();
            this.txtAudioChannels = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.txtWidth = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.txtHeight = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.chkaspectratio = new System.Windows.Forms.CheckBox();
            this.label12 = new System.Windows.Forms.Label();
            this.txtTitle = new System.Windows.Forms.TextBox();
            this.label13 = new System.Windows.Forms.Label();
            this.txtAlbum = new System.Windows.Forms.TextBox();
            this.txtComment = new System.Windows.Forms.TextBox();
            this.label14 = new System.Windows.Forms.Label();
            this.txtAuthor = new System.Windows.Forms.TextBox();
            this.label15 = new System.Windows.Forms.Label();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.label1 = new System.Windows.Forms.Label();
            this.cboGPU = new System.Windows.Forms.ComboBox();
            this.btnDetectGPU = new System.Windows.Forms.Button();
            this.label16 = new System.Windows.Forms.Label();
            this.cboNVIDAPreset = new System.Windows.Forms.ComboBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // cboaudiobitrate
            // 
            this.cboaudiobitrate.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboaudiobitrate.FormattingEnabled = true;
            this.cboaudiobitrate.Location = new System.Drawing.Point(93, 172);
            this.cboaudiobitrate.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.cboaudiobitrate.Name = "cboaudiobitrate";
            this.cboaudiobitrate.Size = new System.Drawing.Size(93, 20);
            this.cboaudiobitrate.TabIndex = 37;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(6, 172);
            this.label8.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(83, 12);
            this.label8.TabIndex = 36;
            this.label8.Text = "Audio bitrate";
            // 
            // cboaudiosample
            // 
            this.cboaudiosample.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboaudiosample.FormattingEnabled = true;
            this.cboaudiosample.Location = new System.Drawing.Point(272, 172);
            this.cboaudiosample.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.cboaudiosample.Name = "cboaudiosample";
            this.cboaudiosample.Size = new System.Drawing.Size(80, 20);
            this.cboaudiosample.TabIndex = 35;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(194, 172);
            this.label7.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(77, 12);
            this.label7.TabIndex = 34;
            this.label7.Text = "Audio Sample";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(178, 143);
            this.label6.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(65, 12);
            this.label6.TabIndex = 32;
            this.label6.Text = "Frame Rate";
            // 
            // txtvideobitrate
            // 
            this.txtvideobitrate.Location = new System.Drawing.Point(96, 140);
            this.txtvideobitrate.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.txtvideobitrate.Name = "txtvideobitrate";
            this.txtvideobitrate.Size = new System.Drawing.Size(66, 21);
            this.txtvideobitrate.TabIndex = 31;
            this.txtvideobitrate.Text = "4600000";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(6, 140);
            this.label5.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(83, 12);
            this.label5.TabIndex = 30;
            this.label5.Text = "Video Bitrate";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 106);
            this.label4.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(77, 12);
            this.label4.TabIndex = 29;
            this.label4.Text = "Audio Device";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 81);
            this.label3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(77, 12);
            this.label3.TabIndex = 28;
            this.label3.Text = "Video Fomrat";
            // 
            // cboVideoFormat
            // 
            this.cboVideoFormat.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboVideoFormat.FormattingEnabled = true;
            this.cboVideoFormat.Location = new System.Drawing.Point(96, 78);
            this.cboVideoFormat.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.cboVideoFormat.Name = "cboVideoFormat";
            this.cboVideoFormat.Size = new System.Drawing.Size(219, 20);
            this.cboVideoFormat.TabIndex = 27;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 37);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(77, 12);
            this.label2.TabIndex = 26;
            this.label2.Text = "Video Device";
            // 
            // cboAudioDevice
            // 
            this.cboAudioDevice.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboAudioDevice.FormattingEnabled = true;
            this.cboAudioDevice.Location = new System.Drawing.Point(96, 108);
            this.cboAudioDevice.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.cboAudioDevice.Name = "cboAudioDevice";
            this.cboAudioDevice.Size = new System.Drawing.Size(219, 20);
            this.cboAudioDevice.TabIndex = 25;
            // 
            // cboVideoDevice
            // 
            this.cboVideoDevice.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboVideoDevice.FormattingEnabled = true;
            this.cboVideoDevice.Location = new System.Drawing.Point(96, 37);
            this.cboVideoDevice.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.cboVideoDevice.Name = "cboVideoDevice";
            this.cboVideoDevice.Size = new System.Drawing.Size(219, 20);
            this.cboVideoDevice.TabIndex = 24;
            this.cboVideoDevice.SelectedIndexChanged += new System.EventHandler(this.cboVideoDevice_SelectedIndexChanged);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Location = new System.Drawing.Point(390, 11);
            this.pictureBox1.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(490, 377);
            this.pictureBox1.TabIndex = 23;
            this.pictureBox1.TabStop = false;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(14, 387);
            this.button1.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(146, 54);
            this.button1.TabIndex = 20;
            this.button1.Text = "Start";
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // txtFrameRate
            // 
            this.txtFrameRate.Location = new System.Drawing.Point(268, 140);
            this.txtFrameRate.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.txtFrameRate.Name = "txtFrameRate";
            this.txtFrameRate.Size = new System.Drawing.Size(44, 21);
            this.txtFrameRate.TabIndex = 38;
            this.txtFrameRate.Text = "25";
            // 
            // txtAudioChannels
            // 
            this.txtAudioChannels.Location = new System.Drawing.Point(100, 207);
            this.txtAudioChannels.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.txtAudioChannels.Name = "txtAudioChannels";
            this.txtAudioChannels.Size = new System.Drawing.Size(44, 21);
            this.txtAudioChannels.TabIndex = 40;
            this.txtAudioChannels.Text = "2";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(10, 210);
            this.label9.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(89, 12);
            this.label9.TabIndex = 39;
            this.label9.Text = "Audio Channels";
            // 
            // txtWidth
            // 
            this.txtWidth.Location = new System.Drawing.Point(101, 236);
            this.txtWidth.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.txtWidth.Name = "txtWidth";
            this.txtWidth.Size = new System.Drawing.Size(44, 21);
            this.txtWidth.TabIndex = 42;
            this.txtWidth.Text = "720";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(11, 239);
            this.label10.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(35, 12);
            this.label10.TabIndex = 41;
            this.label10.Text = "Width";
            // 
            // txtHeight
            // 
            this.txtHeight.Location = new System.Drawing.Point(284, 236);
            this.txtHeight.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.txtHeight.Name = "txtHeight";
            this.txtHeight.Size = new System.Drawing.Size(44, 21);
            this.txtHeight.TabIndex = 44;
            this.txtHeight.Text = "480";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(194, 238);
            this.label11.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(41, 12);
            this.label11.TabIndex = 43;
            this.label11.Text = "Height";
            // 
            // chkaspectratio
            // 
            this.chkaspectratio.AutoSize = true;
            this.chkaspectratio.Location = new System.Drawing.Point(194, 204);
            this.chkaspectratio.Name = "chkaspectratio";
            this.chkaspectratio.Size = new System.Drawing.Size(162, 16);
            this.chkaspectratio.TabIndex = 45;
            this.chkaspectratio.Text = "Enable MP4 Aspect Ratio";
            this.chkaspectratio.UseVisualStyleBackColor = true;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(10, 319);
            this.label12.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(35, 12);
            this.label12.TabIndex = 46;
            this.label12.Text = "Title";
            // 
            // txtTitle
            // 
            this.txtTitle.Location = new System.Drawing.Point(101, 319);
            this.txtTitle.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.txtTitle.Name = "txtTitle";
            this.txtTitle.Size = new System.Drawing.Size(85, 21);
            this.txtTitle.TabIndex = 47;
            this.txtTitle.Text = "my Title";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(194, 323);
            this.label13.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(35, 12);
            this.label13.TabIndex = 48;
            this.label13.Text = "Album";
            // 
            // txtAlbum
            // 
            this.txtAlbum.Location = new System.Drawing.Point(243, 319);
            this.txtAlbum.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.txtAlbum.Name = "txtAlbum";
            this.txtAlbum.Size = new System.Drawing.Size(85, 21);
            this.txtAlbum.TabIndex = 49;
            this.txtAlbum.Text = "my Album";
            // 
            // txtComment
            // 
            this.txtComment.Location = new System.Drawing.Point(244, 346);
            this.txtComment.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.txtComment.Name = "txtComment";
            this.txtComment.Size = new System.Drawing.Size(85, 21);
            this.txtComment.TabIndex = 53;
            this.txtComment.Text = "My Comment";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(195, 350);
            this.label14.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(47, 12);
            this.label14.TabIndex = 52;
            this.label14.Text = "Comment";
            // 
            // txtAuthor
            // 
            this.txtAuthor.Location = new System.Drawing.Point(102, 346);
            this.txtAuthor.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.txtAuthor.Name = "txtAuthor";
            this.txtAuthor.Size = new System.Drawing.Size(85, 21);
            this.txtAuthor.TabIndex = 51;
            this.txtAuthor.Text = "My Author";
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(11, 346);
            this.label15.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(41, 12);
            this.label15.TabIndex = 50;
            this.label15.Text = "Author";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(11, 269);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(101, 12);
            this.label1.TabIndex = 54;
            this.label1.Text = "GPU Acceleration";
            // 
            // cboGPU
            // 
            this.cboGPU.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboGPU.FormattingEnabled = true;
            this.cboGPU.Location = new System.Drawing.Point(124, 268);
            this.cboGPU.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.cboGPU.Name = "cboGPU";
            this.cboGPU.Size = new System.Drawing.Size(81, 20);
            this.cboGPU.TabIndex = 55;
            this.cboGPU.SelectedIndexChanged += new System.EventHandler(this.cboGPU_SelectedIndexChanged);
            // 
            // btnDetectGPU
            // 
            this.btnDetectGPU.Location = new System.Drawing.Point(223, 266);
            this.btnDetectGPU.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.btnDetectGPU.Name = "btnDetectGPU";
            this.btnDetectGPU.Size = new System.Drawing.Size(152, 19);
            this.btnDetectGPU.TabIndex = 56;
            this.btnDetectGPU.Text = "Detect GPU is installed";
            this.btnDetectGPU.UseVisualStyleBackColor = true;
            this.btnDetectGPU.Click += new System.EventHandler(this.btnDetectGPU_Click);
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(12, 294);
            this.label16.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(77, 12);
            this.label16.TabIndex = 57;
            this.label16.Text = "NVIDA Preset";
            // 
            // cboNVIDAPreset
            // 
            this.cboNVIDAPreset.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboNVIDAPreset.Enabled = false;
            this.cboNVIDAPreset.FormattingEnabled = true;
            this.cboNVIDAPreset.Location = new System.Drawing.Point(124, 292);
            this.cboNVIDAPreset.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.cboNVIDAPreset.Name = "cboNVIDAPreset";
            this.cboNVIDAPreset.Size = new System.Drawing.Size(124, 20);
            this.cboNVIDAPreset.TabIndex = 58;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(945, 486);
            this.Controls.Add(this.cboNVIDAPreset);
            this.Controls.Add(this.label16);
            this.Controls.Add(this.btnDetectGPU);
            this.Controls.Add(this.cboGPU);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtComment);
            this.Controls.Add(this.label14);
            this.Controls.Add(this.txtAuthor);
            this.Controls.Add(this.label15);
            this.Controls.Add(this.txtAlbum);
            this.Controls.Add(this.label13);
            this.Controls.Add(this.txtTitle);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.chkaspectratio);
            this.Controls.Add(this.txtHeight);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.txtWidth);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.txtAudioChannels);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.txtFrameRate);
            this.Controls.Add(this.cboaudiobitrate);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.cboaudiosample);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.txtvideobitrate);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.cboVideoFormat);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.cboAudioDevice);
            this.Controls.Add(this.cboVideoDevice);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.button1);
            this.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.Name = "Form1";
            this.Text = "Video Capture to MP4 file sample";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Form1_FormClosed);
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox cboaudiobitrate;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.ComboBox cboaudiosample;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtvideobitrate;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox cboVideoFormat;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cboAudioDevice;
        private System.Windows.Forms.ComboBox cboVideoDevice;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox txtFrameRate;
        private System.Windows.Forms.TextBox txtAudioChannels;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox txtWidth;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox txtHeight;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.CheckBox chkaspectratio;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.TextBox txtTitle;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.TextBox txtAlbum;
        private System.Windows.Forms.TextBox txtComment;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.TextBox txtAuthor;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cboGPU;
        private System.Windows.Forms.Button btnDetectGPU;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.ComboBox cboNVIDAPreset;
    }
}


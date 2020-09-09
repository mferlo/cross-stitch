namespace Stitcher
{
    partial class Form
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.zoomSlider = new System.Windows.Forms.TrackBar();
            this.loadButton = new System.Windows.Forms.Button();
            this.canvas = new System.Windows.Forms.PictureBox();
            this.zoomLabel = new System.Windows.Forms.Label();
            this.colorListBox = new System.Windows.Forms.ListBox();
            this.widthRuler = new System.Windows.Forms.PictureBox();
            this.dimensionsLabel = new System.Windows.Forms.Label();
            this.heightRuler = new System.Windows.Forms.PictureBox();
            this.fileName = new System.Windows.Forms.Label();
            this.actualSize = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.zoomSlider)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.canvas)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.widthRuler)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.heightRuler)).BeginInit();
            this.SuspendLayout();
            // 
            // zoomSlider
            // 
            this.zoomSlider.Location = new System.Drawing.Point(12, 11);
            this.zoomSlider.Maximum = 3;
            this.zoomSlider.Name = "zoomSlider";
            this.zoomSlider.Size = new System.Drawing.Size(104, 45);
            this.zoomSlider.TabIndex = 0;
            this.zoomSlider.ValueChanged += new System.EventHandler(this.zoomSlider_ValueChanged);
            // 
            // loadButton
            // 
            this.loadButton.Location = new System.Drawing.Point(163, 11);
            this.loadButton.Name = "loadButton";
            this.loadButton.Size = new System.Drawing.Size(75, 21);
            this.loadButton.TabIndex = 1;
            this.loadButton.Text = "Load";
            this.loadButton.UseVisualStyleBackColor = true;
            this.loadButton.Click += new System.EventHandler(this.loadButton_Click);
            // 
            // canvas
            // 
            this.canvas.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.canvas.Location = new System.Drawing.Point(183, 58);
            this.canvas.Name = "canvas";
            this.canvas.Size = new System.Drawing.Size(886, 609);
            this.canvas.TabIndex = 2;
            this.canvas.TabStop = false;
            // 
            // zoomLabel
            // 
            this.zoomLabel.AutoSize = true;
            this.zoomLabel.Location = new System.Drawing.Point(122, 14);
            this.zoomLabel.Name = "zoomLabel";
            this.zoomLabel.Size = new System.Drawing.Size(35, 14);
            this.zoomLabel.TabIndex = 3;
            this.zoomLabel.Text = "100%";
            // 
            // colorListBox
            // 
            this.colorListBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.colorListBox.FormattingEnabled = true;
            this.colorListBox.ItemHeight = 14;
            this.colorListBox.Location = new System.Drawing.Point(12, 38);
            this.colorListBox.Name = "colorListBox";
            this.colorListBox.Size = new System.Drawing.Size(145, 620);
            this.colorListBox.TabIndex = 4;
            this.colorListBox.SelectedIndexChanged += new System.EventHandler(this.colorListBox_SelectedIndexChanged);
            // 
            // widthRuler
            // 
            this.widthRuler.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.widthRuler.Location = new System.Drawing.Point(183, 38);
            this.widthRuler.Name = "widthRuler";
            this.widthRuler.Size = new System.Drawing.Size(886, 14);
            this.widthRuler.TabIndex = 5;
            this.widthRuler.TabStop = false;
            // 
            // dimensionsLabel
            // 
            this.dimensionsLabel.AutoSize = true;
            this.dimensionsLabel.Location = new System.Drawing.Point(244, 14);
            this.dimensionsLabel.Name = "dimensionsLabel";
            this.dimensionsLabel.Size = new System.Drawing.Size(28, 14);
            this.dimensionsLabel.TabIndex = 6;
            this.dimensionsLabel.Text = "WxH";
            // 
            // heightRuler
            // 
            this.heightRuler.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.heightRuler.Location = new System.Drawing.Point(163, 58);
            this.heightRuler.Name = "heightRuler";
            this.heightRuler.Size = new System.Drawing.Size(14, 609);
            this.heightRuler.TabIndex = 7;
            this.heightRuler.TabStop = false;
            // 
            // fileName
            // 
            this.fileName.AutoSize = true;
            this.fileName.Location = new System.Drawing.Point(359, 14);
            this.fileName.Name = "fileName";
            this.fileName.Size = new System.Drawing.Size(63, 14);
            this.fileName.TabIndex = 8;
            this.fileName.Text = "fileName";
            // 
            // actualSize
            // 
            this.actualSize.Location = new System.Drawing.Point(977, 9);
            this.actualSize.Name = "actualSize";
            this.actualSize.Size = new System.Drawing.Size(92, 23);
            this.actualSize.TabIndex = 9;
            this.actualSize.Text = "Actual Size";
            this.actualSize.UseVisualStyleBackColor = true;
            this.actualSize.Click += new System.EventHandler(this.actualSize_Click);
            // 
            // Form
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1101, 679);
            this.Controls.Add(this.actualSize);
            this.Controls.Add(this.fileName);
            this.Controls.Add(this.heightRuler);
            this.Controls.Add(this.dimensionsLabel);
            this.Controls.Add(this.widthRuler);
            this.Controls.Add(this.colorListBox);
            this.Controls.Add(this.zoomLabel);
            this.Controls.Add(this.canvas);
            this.Controls.Add(this.loadButton);
            this.Controls.Add(this.zoomSlider);
            this.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.Name = "Form";
            this.Text = "Stitcher";
            ((System.ComponentModel.ISupportInitialize)(this.zoomSlider)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.canvas)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.widthRuler)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.heightRuler)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private void ZoomSlider_ValueChanged(object sender, System.EventArgs e)
        {
            throw new System.NotImplementedException();
        }

        #endregion

        private System.Windows.Forms.TrackBar zoomSlider;
        private System.Windows.Forms.Button loadButton;
        private System.Windows.Forms.PictureBox canvas;
        private System.Windows.Forms.Label zoomLabel;
        private System.Windows.Forms.ListBox colorListBox;
        private System.Windows.Forms.PictureBox widthRuler;
        private System.Windows.Forms.Label dimensionsLabel;
        private System.Windows.Forms.PictureBox heightRuler;
        private System.Windows.Forms.Label fileName;
        private System.Windows.Forms.Button actualSize;
    }
}


﻿namespace Stitcher
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
            this.fileNameLabel = new System.Windows.Forms.Label();
            this.actualSizeButton = new System.Windows.Forms.Button();
            this.setBackgroundButton = new System.Windows.Forms.Button();
            this.backgroundColor = new System.Windows.Forms.PictureBox();
            this.printButton = new System.Windows.Forms.Button();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.zoomSlider)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.canvas)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.widthRuler)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.heightRuler)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.backgroundColor)).BeginInit();
            this.SuspendLayout();
            // 
            // zoomSlider
            // 
            this.zoomSlider.Location = new System.Drawing.Point(12, 11);
            this.zoomSlider.Maximum = 3;
            this.zoomSlider.Name = "zoomSlider";
            this.zoomSlider.Size = new System.Drawing.Size(50, 45);
            this.zoomSlider.TabIndex = 0;
            this.zoomSlider.ValueChanged += new System.EventHandler(this.zoomSlider_ValueChanged);
            // 
            // loadButton
            // 
            this.loadButton.Location = new System.Drawing.Point(163, 10);
            this.loadButton.Name = "loadButton";
            this.loadButton.Size = new System.Drawing.Size(75, 22);
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
            this.zoomLabel.Location = new System.Drawing.Point(59, 14);
            this.zoomLabel.Name = "zoomLabel";
            this.zoomLabel.Size = new System.Drawing.Size(35, 14);
            this.zoomLabel.TabIndex = 3;
            this.zoomLabel.Text = "100%";
            // 
            // colorListBox
            // 
            this.colorListBox.FormattingEnabled = true;
            this.colorListBox.ItemHeight = 14;
            this.colorListBox.Location = new System.Drawing.Point(12, 58);
            this.colorListBox.Name = "colorListBox";
            this.colorListBox.Size = new System.Drawing.Size(145, 158);
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
            this.dimensionsLabel.Location = new System.Drawing.Point(325, 14);
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
            // fileNameLabel
            // 
            this.fileNameLabel.AutoSize = true;
            this.fileNameLabel.Location = new System.Drawing.Point(471, 14);
            this.fileNameLabel.Name = "fileNameLabel";
            this.fileNameLabel.Size = new System.Drawing.Size(63, 14);
            this.fileNameLabel.TabIndex = 8;
            this.fileNameLabel.Text = "fileName";
            // 
            // actualSizeButton
            // 
            this.actualSizeButton.Location = new System.Drawing.Point(977, 10);
            this.actualSizeButton.Name = "actualSizeButton";
            this.actualSizeButton.Size = new System.Drawing.Size(92, 22);
            this.actualSizeButton.TabIndex = 9;
            this.actualSizeButton.Text = "Actual Size";
            this.actualSizeButton.UseVisualStyleBackColor = true;
            this.actualSizeButton.Click += new System.EventHandler(this.actualSize_Click);
            // 
            // setBackgroundButton
            // 
            this.setBackgroundButton.Location = new System.Drawing.Point(12, 222);
            this.setBackgroundButton.Name = "setBackgroundButton";
            this.setBackgroundButton.Size = new System.Drawing.Size(117, 23);
            this.setBackgroundButton.TabIndex = 10;
            this.setBackgroundButton.Text = "Set Background";
            this.setBackgroundButton.UseVisualStyleBackColor = true;
            this.setBackgroundButton.Click += new System.EventHandler(this.setBackgroundButton_Click);
            // 
            // backgroundColor
            // 
            this.backgroundColor.Location = new System.Drawing.Point(135, 222);
            this.backgroundColor.Name = "backgroundColor";
            this.backgroundColor.Size = new System.Drawing.Size(23, 23);
            this.backgroundColor.TabIndex = 11;
            this.backgroundColor.TabStop = false;
            // 
            // printButton
            // 
            this.printButton.Location = new System.Drawing.Point(244, 10);
            this.printButton.Name = "printButton";
            this.printButton.Size = new System.Drawing.Size(75, 22);
            this.printButton.TabIndex = 12;
            this.printButton.Text = "Print";
            this.printButton.UseVisualStyleBackColor = true;
            this.printButton.Click += new System.EventHandler(this.printButton_Click);
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Location = new System.Drawing.Point(100, 13);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(54, 18);
            this.checkBox1.TabIndex = 13;
            this.checkBox1.Text = "Grid";
            this.checkBox1.UseVisualStyleBackColor = true;
            this.checkBox1.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // Form
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1101, 679);
            this.Controls.Add(this.checkBox1);
            this.Controls.Add(this.printButton);
            this.Controls.Add(this.backgroundColor);
            this.Controls.Add(this.setBackgroundButton);
            this.Controls.Add(this.actualSizeButton);
            this.Controls.Add(this.fileNameLabel);
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
            ((System.ComponentModel.ISupportInitialize)(this.backgroundColor)).EndInit();
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
        private System.Windows.Forms.Label fileNameLabel;
        private System.Windows.Forms.Button actualSizeButton;
        private System.Windows.Forms.Button setBackgroundButton;
        private System.Windows.Forms.PictureBox backgroundColor;
        private System.Windows.Forms.Button printButton;
        private System.Windows.Forms.CheckBox checkBox1;
    }
}


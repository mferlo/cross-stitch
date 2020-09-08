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
            ((System.ComponentModel.ISupportInitialize)(this.zoomSlider)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.canvas)).BeginInit();
            this.SuspendLayout();
            // 
            // zoomSlider
            // 
            this.zoomSlider.Location = new System.Drawing.Point(12, 12);
            this.zoomSlider.Maximum = 3;
            this.zoomSlider.Name = "zoomSlider";
            this.zoomSlider.Size = new System.Drawing.Size(104, 45);
            this.zoomSlider.TabIndex = 0;
            this.zoomSlider.ValueChanged += new System.EventHandler(this.zoomSlider_ValueChanged);
            // 
            // loadButton
            // 
            this.loadButton.Location = new System.Drawing.Point(163, 12);
            this.loadButton.Name = "loadButton";
            this.loadButton.Size = new System.Drawing.Size(75, 23);
            this.loadButton.TabIndex = 1;
            this.loadButton.Text = "button1";
            this.loadButton.UseVisualStyleBackColor = true;
            this.loadButton.Click += new System.EventHandler(this.loadButton_Click);
            // 
            // canvas
            // 
            this.canvas.Location = new System.Drawing.Point(163, 41);
            this.canvas.Name = "canvas";
            this.canvas.Size = new System.Drawing.Size(926, 675);
            this.canvas.TabIndex = 2;
            this.canvas.TabStop = false;
            // 
            // zoomLabel
            // 
            this.zoomLabel.AutoSize = true;
            this.zoomLabel.Location = new System.Drawing.Point(122, 12);
            this.zoomLabel.Name = "zoomLabel";
            this.zoomLabel.Size = new System.Drawing.Size(35, 15);
            this.zoomLabel.TabIndex = 3;
            this.zoomLabel.Text = "100%";
            // 
            // colorListBox
            // 
            this.colorListBox.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.colorListBox.FormattingEnabled = true;
            this.colorListBox.ItemHeight = 14;
            this.colorListBox.Location = new System.Drawing.Point(12, 41);
            this.colorListBox.Name = "colorListBox";
            this.colorListBox.Size = new System.Drawing.Size(145, 592);
            this.colorListBox.TabIndex = 4;
            // 
            // Form
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1101, 728);
            this.Controls.Add(this.colorListBox);
            this.Controls.Add(this.zoomLabel);
            this.Controls.Add(this.canvas);
            this.Controls.Add(this.loadButton);
            this.Controls.Add(this.zoomSlider);
            this.Name = "Form";
            this.Text = "Stitcher";
            ((System.ComponentModel.ISupportInitialize)(this.zoomSlider)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.canvas)).EndInit();
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
    }
}


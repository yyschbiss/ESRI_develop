namespace DXApplication1
{
    partial class ImageViewer
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ImageViewer));
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.ShowPicBtn = new System.Windows.Forms.Button();
            this.LastPicBtn = new System.Windows.Forms.Button();
            this.NextPicBtn = new System.Windows.Forms.Button();
            this.ZoomInBtn = new System.Windows.Forms.Button();
            this.ZoomOutBtn = new System.Windows.Forms.Button();
            this.AutoSizeBtn = new System.Windows.Forms.Button();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.flowLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 1.072386F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 98.92761F));
            this.tableLayoutPanel1.Controls.Add(this.pictureBox1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.flowLayoutPanel1, 1, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 90F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(997, 591);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // pictureBox1
            // 
            this.pictureBox1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.tableLayoutPanel1.SetColumnSpan(this.pictureBox1, 2);
            this.pictureBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBox1.Location = new System.Drawing.Point(3, 3);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(991, 525);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Controls.Add(this.ShowPicBtn);
            this.flowLayoutPanel1.Controls.Add(this.LastPicBtn);
            this.flowLayoutPanel1.Controls.Add(this.NextPicBtn);
            this.flowLayoutPanel1.Controls.Add(this.ZoomInBtn);
            this.flowLayoutPanel1.Controls.Add(this.ZoomOutBtn);
            this.flowLayoutPanel1.Controls.Add(this.AutoSizeBtn);
            this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(13, 534);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(981, 54);
            this.flowLayoutPanel1.TabIndex = 1;
            // 
            // ShowPicBtn
            // 
            this.ShowPicBtn.AutoSize = true;
            this.ShowPicBtn.Location = new System.Drawing.Point(3, 3);
            this.ShowPicBtn.Name = "ShowPicBtn";
            this.ShowPicBtn.Size = new System.Drawing.Size(75, 23);
            this.ShowPicBtn.TabIndex = 0;
            this.ShowPicBtn.Text = "显示图片";
            this.ShowPicBtn.UseVisualStyleBackColor = true;
            this.ShowPicBtn.Click += new System.EventHandler(this.ShowPicBtn_Click);
            // 
            // LastPicBtn
            // 
            this.LastPicBtn.AutoSize = true;
            this.LastPicBtn.Location = new System.Drawing.Point(84, 3);
            this.LastPicBtn.Name = "LastPicBtn";
            this.LastPicBtn.Size = new System.Drawing.Size(75, 23);
            this.LastPicBtn.TabIndex = 1;
            this.LastPicBtn.Text = "上一张";
            this.LastPicBtn.UseVisualStyleBackColor = true;
            this.LastPicBtn.Click += new System.EventHandler(this.LastPicBtn_Click);
            // 
            // NextPicBtn
            // 
            this.NextPicBtn.AutoSize = true;
            this.NextPicBtn.Location = new System.Drawing.Point(165, 3);
            this.NextPicBtn.Name = "NextPicBtn";
            this.NextPicBtn.Size = new System.Drawing.Size(75, 23);
            this.NextPicBtn.TabIndex = 2;
            this.NextPicBtn.Text = "下一张";
            this.NextPicBtn.UseVisualStyleBackColor = true;
            this.NextPicBtn.Click += new System.EventHandler(this.NextPicBtn_Click);
            // 
            // ZoomInBtn
            // 
            this.ZoomInBtn.AutoSize = true;
            this.ZoomInBtn.Location = new System.Drawing.Point(246, 3);
            this.ZoomInBtn.Name = "ZoomInBtn";
            this.ZoomInBtn.Size = new System.Drawing.Size(75, 23);
            this.ZoomInBtn.TabIndex = 3;
            this.ZoomInBtn.Text = "放大";
            this.ZoomInBtn.UseVisualStyleBackColor = true;
            this.ZoomInBtn.Click += new System.EventHandler(this.ZoomInBtn_Click);
            // 
            // ZoomOutBtn
            // 
            this.ZoomOutBtn.AutoSize = true;
            this.ZoomOutBtn.Location = new System.Drawing.Point(327, 3);
            this.ZoomOutBtn.Name = "ZoomOutBtn";
            this.ZoomOutBtn.Size = new System.Drawing.Size(75, 23);
            this.ZoomOutBtn.TabIndex = 4;
            this.ZoomOutBtn.Text = "缩小";
            this.ZoomOutBtn.UseVisualStyleBackColor = true;
            this.ZoomOutBtn.Click += new System.EventHandler(this.ZoomOutBtn_Click);
            // 
            // AutoSizeBtn
            // 
            this.AutoSizeBtn.Location = new System.Drawing.Point(408, 3);
            this.AutoSizeBtn.Name = "AutoSizeBtn";
            this.AutoSizeBtn.Size = new System.Drawing.Size(121, 23);
            this.AutoSizeBtn.TabIndex = 5;
            this.AutoSizeBtn.Text = "缩放至屏幕大小";
            this.AutoSizeBtn.UseVisualStyleBackColor = true;
            this.AutoSizeBtn.Click += new System.EventHandler(this.AutoSizeBtn_Click);
            // 
            // ImageViewer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(997, 591);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "ImageViewer";
            this.Text = "ImageViewer";
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.flowLayoutPanel1.ResumeLayout(false);
            this.flowLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.Button ShowPicBtn;
        private System.Windows.Forms.Button LastPicBtn;
        private System.Windows.Forms.Button NextPicBtn;
        private System.Windows.Forms.Button ZoomInBtn;
        private System.Windows.Forms.Button ZoomOutBtn;
        private System.Windows.Forms.Button AutoSizeBtn;
    }
}
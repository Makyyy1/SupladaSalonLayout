namespace SupladaSalonLayout
{
    partial class ViewReadyForQueue
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ViewReadyForQueue));
            this.lblTodayDate = new System.Windows.Forms.Label();
            this.dataReadyForQueue = new System.Windows.Forms.DataGridView();
            this.pcClose = new System.Windows.Forms.PictureBox();
            this.label1 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dataReadyForQueue)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pcClose)).BeginInit();
            this.SuspendLayout();
            // 
            // lblTodayDate
            // 
            this.lblTodayDate.AutoSize = true;
            this.lblTodayDate.Font = new System.Drawing.Font("Calibri", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTodayDate.Location = new System.Drawing.Point(12, 60);
            this.lblTodayDate.Name = "lblTodayDate";
            this.lblTodayDate.Size = new System.Drawing.Size(200, 27);
            this.lblTodayDate.TabIndex = 0;
            this.lblTodayDate.Text = "Today's Date: ";
            // 
            // dataReadyForQueue
            // 
            this.dataReadyForQueue.AllowUserToAddRows = false;
            this.dataReadyForQueue.AllowUserToDeleteRows = false;
            this.dataReadyForQueue.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataReadyForQueue.BackgroundColor = System.Drawing.Color.White;
            this.dataReadyForQueue.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataReadyForQueue.Location = new System.Drawing.Point(12, 100);
            this.dataReadyForQueue.Name = "dataReadyForQueue";
            this.dataReadyForQueue.ReadOnly = true;
            this.dataReadyForQueue.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataReadyForQueue.Size = new System.Drawing.Size(1200, 600);
            this.dataReadyForQueue.TabIndex = 1;
            // 
            // pcClose
            // 
            this.pcClose.Image = ((System.Drawing.Image)(resources.GetObject("pcClose.Image")));
            this.pcClose.Location = new System.Drawing.Point(1190, 12);
            this.pcClose.Name = "pcClose";
            this.pcClose.Size = new System.Drawing.Size(29, 32);
            this.pcClose.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pcClose.TabIndex = 2;
            this.pcClose.TabStop = false;
            this.pcClose.Click += new System.EventHandler(this.pcClose_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Calibri", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(12, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(300, 39);
            this.label1.TabIndex = 3;
            this.label1.Text = "Ready for Queue";
            // 
            // ViewReadyForQueue
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(237)))), ((int)(((byte)(212)))));
            this.ClientSize = new System.Drawing.Size(1230, 720);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.pcClose);
            this.Controls.Add(this.dataReadyForQueue);
            this.Controls.Add(this.lblTodayDate);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "ViewReadyForQueue";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "View Ready for Queue";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ViewReadyForQueue_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.dataReadyForQueue)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pcClose)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblTodayDate;
        private System.Windows.Forms.DataGridView dataReadyForQueue;
        private System.Windows.Forms.PictureBox pcClose;
        private System.Windows.Forms.Label label1;
    }
}


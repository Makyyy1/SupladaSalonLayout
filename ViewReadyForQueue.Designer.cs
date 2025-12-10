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
            this.btnAddSelectedQueue = new System.Windows.Forms.Button();
            this.btnSelectAll = new System.Windows.Forms.Button();
            this.lblSelected = new System.Windows.Forms.Label();
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
            this.lblTodayDate.Size = new System.Drawing.Size(140, 27);
            this.lblTodayDate.TabIndex = 0;
            this.lblTodayDate.Text = "Today\'s Date: ";
            // 
            // dataReadyForQueue
            // 
            this.dataReadyForQueue.AllowUserToAddRows = false;
            this.dataReadyForQueue.AllowUserToDeleteRows = false;
            this.dataReadyForQueue.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataReadyForQueue.BackgroundColor = System.Drawing.Color.White;
            this.dataReadyForQueue.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataReadyForQueue.Location = new System.Drawing.Point(12, 138);
            this.dataReadyForQueue.Name = "dataReadyForQueue";
            this.dataReadyForQueue.ReadOnly = false;
            this.dataReadyForQueue.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataReadyForQueue.Size = new System.Drawing.Size(1200, 600);
            this.dataReadyForQueue.TabIndex = 1;
            this.dataReadyForQueue.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataReadyForQueue_CellValueChanged);
            this.dataReadyForQueue.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataReadyForQueue_CellClick);
            this.dataReadyForQueue.CurrentCellDirtyStateChanged += new System.EventHandler(this.dataReadyForQueue_CurrentCellDirtyStateChanged);
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
            this.label1.Size = new System.Drawing.Size(239, 39);
            this.label1.TabIndex = 3;
            this.label1.Text = "Ready for Queue";
            // 
            // btnAddSelectedQueue
            // 
            this.btnAddSelectedQueue.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(164)))), ((int)(((byte)(244)))), ((int)(((byte)(207)))));
            this.btnAddSelectedQueue.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnAddSelectedQueue.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAddSelectedQueue.Location = new System.Drawing.Point(1028, 91);
            this.btnAddSelectedQueue.Name = "btnAddSelectedQueue";
            this.btnAddSelectedQueue.Size = new System.Drawing.Size(184, 41);
            this.btnAddSelectedQueue.TabIndex = 42;
            this.btnAddSelectedQueue.Text = "Add Selected to Queue";
            this.btnAddSelectedQueue.UseVisualStyleBackColor = false;
            this.btnAddSelectedQueue.Click += new System.EventHandler(this.btnAddSelectedQueue_Click);
            // 
            // btnSelectAll
            // 
            this.btnSelectAll.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(164)))), ((int)(((byte)(244)))), ((int)(((byte)(207)))));
            this.btnSelectAll.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnSelectAll.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSelectAll.Location = new System.Drawing.Point(12, 99);
            this.btnSelectAll.Name = "btnSelectAll";
            this.btnSelectAll.Size = new System.Drawing.Size(99, 33);
            this.btnSelectAll.TabIndex = 43;
            this.btnSelectAll.Text = "Select All";
            this.btnSelectAll.UseVisualStyleBackColor = false;
            this.btnSelectAll.Click += new System.EventHandler(this.btnSelectAll_Click);
            // 
            // lblSelected
            // 
            this.lblSelected.AutoSize = true;
            this.lblSelected.Font = new System.Drawing.Font("Verdana", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSelected.Location = new System.Drawing.Point(117, 106);
            this.lblSelected.Name = "lblSelected";
            this.lblSelected.Size = new System.Drawing.Size(21, 18);
            this.lblSelected.TabIndex = 44;
            this.lblSelected.Text = ": ";
            // 
            // ViewReadyForQueue
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(237)))), ((int)(((byte)(212)))));
            this.ClientSize = new System.Drawing.Size(1229, 752);
            this.Controls.Add(this.lblSelected);
            this.Controls.Add(this.btnSelectAll);
            this.Controls.Add(this.btnAddSelectedQueue);
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
        private System.Windows.Forms.Button btnAddSelectedQueue;
        private System.Windows.Forms.Button btnSelectAll;
        private System.Windows.Forms.Label lblSelected;
    }
}


namespace SupladaSalonLayout
{
    partial class AdminCreateAppointment
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AdminCreateAppointment));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle19 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle20 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle21 = new System.Windows.Forms.DataGridViewCellStyle();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.pcClose = new System.Windows.Forms.PictureBox();
            this.btnAddQueue = new System.Windows.Forms.Button();
            this.dataAppointments = new System.Windows.Forms.DataGridView();
            this.btnSoloService = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pcClose)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataAppointments)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Calibri", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(11, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(321, 39);
            this.label1.TabIndex = 2;
            this.label1.Text = "Manage Appointments";
            // 
            // label2
            // 
            this.label2.Font = new System.Drawing.Font("Calibri", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(253)))), ((int)(((byte)(159)))), ((int)(((byte)(43)))));
            this.label2.Location = new System.Drawing.Point(2, 18);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(1624, 39);
            this.label2.TabIndex = 4;
            this.label2.Text = "_________________________________________________________________________________" +
    "_____________________";
            // 
            // pcClose
            // 
            this.pcClose.Image = ((System.Drawing.Image)(resources.GetObject("pcClose.Image")));
            this.pcClose.Location = new System.Drawing.Point(1597, 9);
            this.pcClose.Name = "pcClose";
            this.pcClose.Size = new System.Drawing.Size(29, 32);
            this.pcClose.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pcClose.TabIndex = 26;
            this.pcClose.TabStop = false;
            this.pcClose.Click += new System.EventHandler(this.pcClose_Click);
            // 
            // btnAddQueue
            // 
            this.btnAddQueue.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(164)))), ((int)(((byte)(244)))), ((int)(((byte)(207)))));
            this.btnAddQueue.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnAddQueue.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAddQueue.Location = new System.Drawing.Point(1358, 128);
            this.btnAddQueue.Name = "btnAddQueue";
            this.btnAddQueue.Size = new System.Drawing.Size(199, 41);
            this.btnAddQueue.TabIndex = 33;
            this.btnAddQueue.Text = "Ready for Queue";
            this.btnAddQueue.UseVisualStyleBackColor = false;
            this.btnAddQueue.Click += new System.EventHandler(this.btnAddQueue_Click);
            // 
            // dataAppointments
            // 
            this.dataAppointments.AllowUserToAddRows = false;
            this.dataAppointments.AllowUserToDeleteRows = false;
            dataGridViewCellStyle19.BackColor = System.Drawing.SystemColors.Window;
            this.dataAppointments.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle19;
            this.dataAppointments.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataAppointments.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.dataAppointments.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(250)))), ((int)(((byte)(252)))));
            this.dataAppointments.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            dataGridViewCellStyle20.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle20.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(214)))), ((int)(((byte)(167)))));
            dataGridViewCellStyle20.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle20.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle20.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(214)))), ((int)(((byte)(167)))));
            dataGridViewCellStyle20.SelectionForeColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle20.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataAppointments.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle20;
            this.dataAppointments.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataAppointments.Cursor = System.Windows.Forms.Cursors.Hand;
            dataGridViewCellStyle21.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle21.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle21.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle21.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle21.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(55)))), ((int)(((byte)(188)))), ((int)(((byte)(125)))));
            dataGridViewCellStyle21.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle21.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dataAppointments.DefaultCellStyle = dataGridViewCellStyle21;
            this.dataAppointments.EnableHeadersVisualStyles = false;
            this.dataAppointments.Location = new System.Drawing.Point(9, 70);
            this.dataAppointments.Name = "dataAppointments";
            this.dataAppointments.ReadOnly = true;
            this.dataAppointments.RowHeadersVisible = false;
            this.dataAppointments.RowHeadersWidth = 100;
            this.dataAppointments.RowTemplate.Height = 40;
            this.dataAppointments.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataAppointments.Size = new System.Drawing.Size(1311, 867);
            this.dataAppointments.TabIndex = 38;
            this.dataAppointments.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataAppointments_CellClick);
            // 
            // btnSoloService
            // 
            this.btnSoloService.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(164)))), ((int)(((byte)(244)))), ((int)(((byte)(207)))));
            this.btnSoloService.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnSoloService.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSoloService.Location = new System.Drawing.Point(1358, 70);
            this.btnSoloService.Name = "btnSoloService";
            this.btnSoloService.Size = new System.Drawing.Size(199, 41);
            this.btnSoloService.TabIndex = 40;
            this.btnSoloService.Text = "Create Appointment";
            this.btnSoloService.UseVisualStyleBackColor = false;
            this.btnSoloService.Click += new System.EventHandler(this.btnSoloService_Click_1);
            // 
            // AdminCreateAppointment
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(237)))), ((int)(((byte)(212)))));
            this.ClientSize = new System.Drawing.Size(1638, 949);
            this.Controls.Add(this.btnSoloService);
            this.Controls.Add(this.dataAppointments);
            this.Controls.Add(this.btnAddQueue);
            this.Controls.Add(this.pcClose);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label2);
            this.Name = "AdminCreateAppointment";
            this.Text = "AdminCreateAppointment";
            ((System.ComponentModel.ISupportInitialize)(this.pcClose)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataAppointments)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.PictureBox pcClose;
        private System.Windows.Forms.Button btnAddQueue;
        private System.Windows.Forms.DataGridView dataAppointments;
        private System.Windows.Forms.Button btnSoloService;
    }
}
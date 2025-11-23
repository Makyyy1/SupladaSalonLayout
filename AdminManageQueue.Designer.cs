namespace SupladaSalonLayout
{
    partial class AdminManageQueue
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AdminManageQueue));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle8 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle9 = new System.Windows.Forms.DataGridViewCellStyle();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.pcClose = new System.Windows.Forms.PictureBox();
            this.btnCancelAppointment = new System.Windows.Forms.Button();
            this.dataQueue = new System.Windows.Forms.DataGridView();
            this.AppointmentDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.AppointmentTime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Customerfirstname = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.customerLastName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CustomerContact = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Status = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.btnReadyBilling = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pcClose)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataQueue)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Calibri", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(220, 39);
            this.label1.TabIndex = 3;
            this.label1.Text = "Manage Queue";
            // 
            // label2
            // 
            this.label2.Font = new System.Drawing.Font("Calibri", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(253)))), ((int)(((byte)(159)))), ((int)(((byte)(43)))));
            this.label2.Location = new System.Drawing.Point(-4, 21);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(1630, 39);
            this.label2.TabIndex = 5;
            this.label2.Text = "_________________________________________________________________________________" +
    "____________________";
            // 
            // pcClose
            // 
            this.pcClose.Image = ((System.Drawing.Image)(resources.GetObject("pcClose.Image")));
            this.pcClose.Location = new System.Drawing.Point(1597, 9);
            this.pcClose.Name = "pcClose";
            this.pcClose.Size = new System.Drawing.Size(29, 32);
            this.pcClose.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pcClose.TabIndex = 27;
            this.pcClose.TabStop = false;
            this.pcClose.Click += new System.EventHandler(this.pcClose_Click);
            // 
            // btnCancelAppointment
            // 
            this.btnCancelAppointment.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(162)))), ((int)(((byte)(162)))));
            this.btnCancelAppointment.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnCancelAppointment.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCancelAppointment.Location = new System.Drawing.Point(1388, 128);
            this.btnCancelAppointment.Name = "btnCancelAppointment";
            this.btnCancelAppointment.Size = new System.Drawing.Size(238, 38);
            this.btnCancelAppointment.TabIndex = 33;
            this.btnCancelAppointment.Text = "Cancel Appointment";
            this.btnCancelAppointment.UseVisualStyleBackColor = false;
            this.btnCancelAppointment.Click += new System.EventHandler(this.btnCancelAppointment_Click);
            // 
            // dataQueue
            // 
            this.dataQueue.AllowUserToAddRows = false;
            this.dataQueue.AllowUserToDeleteRows = false;
            dataGridViewCellStyle7.BackColor = System.Drawing.SystemColors.Window;
            this.dataQueue.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle7;
            this.dataQueue.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataQueue.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.dataQueue.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(250)))), ((int)(((byte)(252)))));
            this.dataQueue.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            dataGridViewCellStyle8.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle8.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(214)))), ((int)(((byte)(167)))));
            dataGridViewCellStyle8.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle8.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle8.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(214)))), ((int)(((byte)(167)))));
            dataGridViewCellStyle8.SelectionForeColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle8.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataQueue.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle8;
            this.dataQueue.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataQueue.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.AppointmentDate,
            this.AppointmentTime,
            this.Customerfirstname,
            this.customerLastName,
            this.CustomerContact,
            this.Status});
            this.dataQueue.Cursor = System.Windows.Forms.Cursors.Hand;
            dataGridViewCellStyle9.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle9.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle9.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle9.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle9.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(55)))), ((int)(((byte)(188)))), ((int)(((byte)(125)))));
            dataGridViewCellStyle9.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle9.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dataQueue.DefaultCellStyle = dataGridViewCellStyle9;
            this.dataQueue.EnableHeadersVisualStyles = false;
            this.dataQueue.Location = new System.Drawing.Point(3, 63);
            this.dataQueue.Name = "dataQueue";
            this.dataQueue.ReadOnly = true;
            this.dataQueue.RowHeadersVisible = false;
            this.dataQueue.RowHeadersWidth = 100;
            this.dataQueue.RowTemplate.Height = 40;
            this.dataQueue.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataQueue.Size = new System.Drawing.Size(1379, 874);
            this.dataQueue.TabIndex = 39;
            // 
            // AppointmentDate
            // 
            this.AppointmentDate.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.AppointmentDate.DataPropertyName = "AppointmentDate";
            this.AppointmentDate.HeaderText = "Appointment Date";
            this.AppointmentDate.Name = "AppointmentDate";
            this.AppointmentDate.ReadOnly = true;
            // 
            // AppointmentTime
            // 
            this.AppointmentTime.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.AppointmentTime.DataPropertyName = "AppointmentTime";
            this.AppointmentTime.HeaderText = "Appointment Time";
            this.AppointmentTime.Name = "AppointmentTime";
            this.AppointmentTime.ReadOnly = true;
            // 
            // Customerfirstname
            // 
            this.Customerfirstname.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Customerfirstname.DataPropertyName = "CustomerFirstName";
            this.Customerfirstname.HeaderText = "First Name";
            this.Customerfirstname.Name = "Customerfirstname";
            this.Customerfirstname.ReadOnly = true;
            // 
            // customerLastName
            // 
            this.customerLastName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.customerLastName.DataPropertyName = "CustomerLastName";
            this.customerLastName.HeaderText = "Last Name";
            this.customerLastName.Name = "customerLastName";
            this.customerLastName.ReadOnly = true;
            // 
            // CustomerContact
            // 
            this.CustomerContact.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.CustomerContact.DataPropertyName = "CustomerContact";
            this.CustomerContact.HeaderText = "Contact";
            this.CustomerContact.Name = "CustomerContact";
            this.CustomerContact.ReadOnly = true;
            this.CustomerContact.Width = 87;
            // 
            // Status
            // 
            this.Status.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.Status.DataPropertyName = "Status";
            this.Status.HeaderText = "Status";
            this.Status.Name = "Status";
            this.Status.ReadOnly = true;
            this.Status.Width = 77;
            // 
            // btnReadyBilling
            // 
            this.btnReadyBilling.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(164)))), ((int)(((byte)(244)))), ((int)(((byte)(207)))));
            this.btnReadyBilling.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnReadyBilling.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnReadyBilling.Location = new System.Drawing.Point(1388, 81);
            this.btnReadyBilling.Name = "btnReadyBilling";
            this.btnReadyBilling.Size = new System.Drawing.Size(238, 41);
            this.btnReadyBilling.TabIndex = 40;
            this.btnReadyBilling.Text = "Ready for Billing";
            this.btnReadyBilling.UseVisualStyleBackColor = false;
            this.btnReadyBilling.Click += new System.EventHandler(this.btnReadyBilling_Click);
            // 
            // AdminManageQueue
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(237)))), ((int)(((byte)(212)))));
            this.ClientSize = new System.Drawing.Size(1638, 949);
            this.Controls.Add(this.btnReadyBilling);
            this.Controls.Add(this.dataQueue);
            this.Controls.Add(this.btnCancelAppointment);
            this.Controls.Add(this.pcClose);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label2);
            this.Name = "AdminManageQueue";
            this.Text = "AdminManageQueue";
            this.Load += new System.EventHandler(this.AdminManageQueue_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pcClose)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataQueue)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.PictureBox pcClose;
        private System.Windows.Forms.Button btnCancelAppointment;
        private System.Windows.Forms.DataGridView dataQueue;
        private System.Windows.Forms.DataGridViewTextBoxColumn AppointmentDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn AppointmentTime;
        private System.Windows.Forms.DataGridViewTextBoxColumn Customerfirstname;
        private System.Windows.Forms.DataGridViewTextBoxColumn customerLastName;
        private System.Windows.Forms.DataGridViewTextBoxColumn CustomerContact;
        private System.Windows.Forms.DataGridViewTextBoxColumn Status;
        private System.Windows.Forms.Button btnReadyBilling;
    }
}
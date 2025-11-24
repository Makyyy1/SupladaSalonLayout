namespace SupladaSalonLayout
{
    partial class CashierHomeDashboard
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CashierHomeDashboard));
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnReports = new System.Windows.Forms.Button();
            this.btnLogout = new System.Windows.Forms.Button();
            this.btnQueue = new System.Windows.Forms.Button();
            this.btnAppointments = new System.Windows.Forms.Button();
            this.btnPayments = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.realTime = new System.Windows.Forms.Timer(this.components);
            this.panelMainForm = new System.Windows.Forms.Panel();
            this.panel3 = new System.Windows.Forms.Panel();
            this.panel4 = new System.Windows.Forms.Panel();
            this.lblTime = new System.Windows.Forms.Label();
            this.panel5 = new System.Windows.Forms.Panel();
            this.lblDate = new System.Windows.Forms.Label();
            this.panel7 = new System.Windows.Forms.Panel();
            this.lblCustomerQueue = new System.Windows.Forms.Label();
            this.pcCustomerQueue = new System.Windows.Forms.PictureBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.panel6 = new System.Windows.Forms.Panel();
            this.lblTodayAppointments = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.pcCustomerToday = new System.Windows.Forms.PictureBox();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.panelMainForm.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panel4.SuspendLayout();
            this.panel5.SuspendLayout();
            this.panel7.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pcCustomerQueue)).BeginInit();
            this.panel6.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pcCustomerToday)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.AutoScroll = true;
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(253)))), ((int)(((byte)(159)))), ((int)(((byte)(43)))));
            this.panel1.Controls.Add(this.btnReports);
            this.panel1.Controls.Add(this.btnLogout);
            this.panel1.Controls.Add(this.btnQueue);
            this.panel1.Controls.Add(this.btnAppointments);
            this.panel1.Controls.Add(this.btnPayments);
            this.panel1.Controls.Add(this.panel2);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(250, 1041);
            this.panel1.TabIndex = 0;
            // 
            // btnReports
            // 
            this.btnReports.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(253)))), ((int)(((byte)(159)))), ((int)(((byte)(43)))));
            this.btnReports.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnReports.FlatAppearance.BorderSize = 0;
            this.btnReports.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnReports.Font = new System.Drawing.Font("Verdana", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnReports.Location = new System.Drawing.Point(0, 257);
            this.btnReports.Name = "btnReports";
            this.btnReports.Padding = new System.Windows.Forms.Padding(10, 0, 0, 0);
            this.btnReports.Size = new System.Drawing.Size(250, 45);
            this.btnReports.TabIndex = 8;
            this.btnReports.Text = "Reports";
            this.btnReports.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnReports.UseVisualStyleBackColor = false;
            this.btnReports.Click += new System.EventHandler(this.btnReports_Click);
            // 
            // btnLogout
            // 
            this.btnLogout.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this.btnLogout.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(135)))), ((int)(((byte)(46)))));
            this.btnLogout.FlatAppearance.BorderSize = 0;
            this.btnLogout.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLogout.Font = new System.Drawing.Font("Verdana", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnLogout.Location = new System.Drawing.Point(0, 964);
            this.btnLogout.Name = "btnLogout";
            this.btnLogout.Size = new System.Drawing.Size(250, 45);
            this.btnLogout.TabIndex = 7;
            this.btnLogout.Text = "Logout";
            this.btnLogout.UseVisualStyleBackColor = false;
            this.btnLogout.Click += new System.EventHandler(this.btnLogout_Click);
            // 
            // btnQueue
            // 
            this.btnQueue.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(253)))), ((int)(((byte)(159)))), ((int)(((byte)(43)))));
            this.btnQueue.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnQueue.FlatAppearance.BorderSize = 0;
            this.btnQueue.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnQueue.Font = new System.Drawing.Font("Verdana", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnQueue.Location = new System.Drawing.Point(0, 212);
            this.btnQueue.Name = "btnQueue";
            this.btnQueue.Padding = new System.Windows.Forms.Padding(10, 0, 0, 0);
            this.btnQueue.Size = new System.Drawing.Size(250, 45);
            this.btnQueue.TabIndex = 3;
            this.btnQueue.Text = "Manage Queue";
            this.btnQueue.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnQueue.UseVisualStyleBackColor = false;
            this.btnQueue.Click += new System.EventHandler(this.btnQueue_Click);
            // 
            // btnAppointments
            // 
            this.btnAppointments.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(253)))), ((int)(((byte)(159)))), ((int)(((byte)(43)))));
            this.btnAppointments.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnAppointments.FlatAppearance.BorderSize = 0;
            this.btnAppointments.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnAppointments.Font = new System.Drawing.Font("Verdana", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAppointments.Location = new System.Drawing.Point(0, 167);
            this.btnAppointments.Name = "btnAppointments";
            this.btnAppointments.Padding = new System.Windows.Forms.Padding(10, 0, 0, 0);
            this.btnAppointments.Size = new System.Drawing.Size(250, 45);
            this.btnAppointments.TabIndex = 2;
            this.btnAppointments.Text = "Appointments";
            this.btnAppointments.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnAppointments.UseVisualStyleBackColor = false;
            this.btnAppointments.Click += new System.EventHandler(this.btnAppointments_Click);
            // 
            // btnPayments
            // 
            this.btnPayments.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(253)))), ((int)(((byte)(159)))), ((int)(((byte)(43)))));
            this.btnPayments.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnPayments.FlatAppearance.BorderSize = 0;
            this.btnPayments.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnPayments.Font = new System.Drawing.Font("Verdana", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnPayments.Location = new System.Drawing.Point(0, 122);
            this.btnPayments.Name = "btnPayments";
            this.btnPayments.Padding = new System.Windows.Forms.Padding(10, 0, 0, 0);
            this.btnPayments.Size = new System.Drawing.Size(250, 45);
            this.btnPayments.TabIndex = 1;
            this.btnPayments.Text = "Payments";
            this.btnPayments.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnPayments.UseVisualStyleBackColor = false;
            this.btnPayments.Click += new System.EventHandler(this.btnPayments_Click);
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(253)))), ((int)(((byte)(159)))), ((int)(((byte)(43)))));
            this.panel2.Controls.Add(this.pictureBox1);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(250, 122);
            this.panel2.TabIndex = 0;
            // 
            // pictureBox1
            // 
            this.pictureBox1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(59, 3);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(140, 116);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // realTime
            // 
            this.realTime.Interval = 1000;
            this.realTime.Tick += new System.EventHandler(this.realTime_Tick);
            // 
            // panelMainForm
            // 
            this.panelMainForm.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(237)))), ((int)(((byte)(212)))));
            this.panelMainForm.Controls.Add(this.panel3);
            this.panelMainForm.Controls.Add(this.panel7);
            this.panelMainForm.Controls.Add(this.label1);
            this.panelMainForm.Controls.Add(this.panel6);
            this.panelMainForm.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelMainForm.Location = new System.Drawing.Point(250, 0);
            this.panelMainForm.Name = "panelMainForm";
            this.panelMainForm.Size = new System.Drawing.Size(1654, 1041);
            this.panelMainForm.TabIndex = 2;
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(253)))), ((int)(((byte)(159)))), ((int)(((byte)(43)))));
            this.panel3.Controls.Add(this.panel4);
            this.panel3.Controls.Add(this.panel5);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel3.Location = new System.Drawing.Point(0, 0);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(1654, 53);
            this.panel3.TabIndex = 7;
            // 
            // panel4
            // 
            this.panel4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.panel4.Controls.Add(this.lblTime);
            this.panel4.Font = new System.Drawing.Font("Cascadia Code", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.panel4.Location = new System.Drawing.Point(1551, 26);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(88, 24);
            this.panel4.TabIndex = 5;
            // 
            // lblTime
            // 
            this.lblTime.AutoSize = true;
            this.lblTime.Dock = System.Windows.Forms.DockStyle.Left;
            this.lblTime.Font = new System.Drawing.Font("Calibri", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTime.Location = new System.Drawing.Point(0, 0);
            this.lblTime.Name = "lblTime";
            this.lblTime.Size = new System.Drawing.Size(49, 23);
            this.lblTime.TabIndex = 2;
            this.lblTime.Text = "Time";
            // 
            // panel5
            // 
            this.panel5.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.panel5.Controls.Add(this.lblDate);
            this.panel5.Font = new System.Drawing.Font("Cascadia Code", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.panel5.Location = new System.Drawing.Point(1401, 3);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(240, 24);
            this.panel5.TabIndex = 4;
            // 
            // lblDate
            // 
            this.lblDate.AutoSize = true;
            this.lblDate.Dock = System.Windows.Forms.DockStyle.Left;
            this.lblDate.Font = new System.Drawing.Font("Calibri", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDate.Location = new System.Drawing.Point(0, 0);
            this.lblDate.Name = "lblDate";
            this.lblDate.Size = new System.Drawing.Size(48, 23);
            this.lblDate.TabIndex = 2;
            this.lblDate.Text = "Date";
            // 
            // panel7
            // 
            this.panel7.BackColor = System.Drawing.SystemColors.Window;
            this.panel7.Controls.Add(this.lblCustomerQueue);
            this.panel7.Controls.Add(this.pcCustomerQueue);
            this.panel7.Controls.Add(this.label4);
            this.panel7.Location = new System.Drawing.Point(841, 123);
            this.panel7.Name = "panel7";
            this.panel7.Size = new System.Drawing.Size(801, 472);
            this.panel7.TabIndex = 6;
            // 
            // lblCustomerQueue
            // 
            this.lblCustomerQueue.AutoSize = true;
            this.lblCustomerQueue.Cursor = System.Windows.Forms.Cursors.Hand;
            this.lblCustomerQueue.Font = new System.Drawing.Font("Calibri", 72F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCustomerQueue.Location = new System.Drawing.Point(390, 168);
            this.lblCustomerQueue.Name = "lblCustomerQueue";
            this.lblCustomerQueue.Size = new System.Drawing.Size(99, 117);
            this.lblCustomerQueue.TabIndex = 6;
            this.lblCustomerQueue.Text = "0";
            this.lblCustomerQueue.Click += new System.EventHandler(this.lblCustomerQueue_Click);
            // 
            // pcCustomerQueue
            // 
            this.pcCustomerQueue.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pcCustomerQueue.Image = ((System.Drawing.Image)(resources.GetObject("pcCustomerQueue.Image")));
            this.pcCustomerQueue.Location = new System.Drawing.Point(43, 123);
            this.pcCustomerQueue.Name = "pcCustomerQueue";
            this.pcCustomerQueue.Size = new System.Drawing.Size(223, 212);
            this.pcCustomerQueue.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pcCustomerQueue.TabIndex = 5;
            this.pcCustomerQueue.TabStop = false;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Calibri", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(306, 4);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(240, 39);
            this.label4.TabIndex = 5;
            this.label4.Text = "Customer Queue";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Calibri", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(6, 68);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(254, 39);
            this.label1.TabIndex = 2;
            this.label1.Text = "Welcome, Cashier";
            // 
            // panel6
            // 
            this.panel6.BackColor = System.Drawing.SystemColors.Window;
            this.panel6.Controls.Add(this.lblTodayAppointments);
            this.panel6.Controls.Add(this.label3);
            this.panel6.Controls.Add(this.pcCustomerToday);
            this.panel6.Location = new System.Drawing.Point(9, 123);
            this.panel6.Name = "panel6";
            this.panel6.Size = new System.Drawing.Size(801, 472);
            this.panel6.TabIndex = 5;
            // 
            // lblTodayAppointments
            // 
            this.lblTodayAppointments.AutoSize = true;
            this.lblTodayAppointments.Cursor = System.Windows.Forms.Cursors.Hand;
            this.lblTodayAppointments.Font = new System.Drawing.Font("Calibri", 72F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTodayAppointments.Location = new System.Drawing.Point(399, 159);
            this.lblTodayAppointments.Name = "lblTodayAppointments";
            this.lblTodayAppointments.Size = new System.Drawing.Size(99, 117);
            this.lblTodayAppointments.TabIndex = 5;
            this.lblTodayAppointments.Text = "0";
            this.lblTodayAppointments.Click += new System.EventHandler(this.lblTodayAppointments_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Calibri", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(219, 4);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(412, 39);
            this.label3.TabIndex = 5;
            this.label3.Text = "Customer Appointment Today";
            // 
            // pcCustomerToday
            // 
            this.pcCustomerToday.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pcCustomerToday.Image = ((System.Drawing.Image)(resources.GetObject("pcCustomerToday.Image")));
            this.pcCustomerToday.Location = new System.Drawing.Point(43, 123);
            this.pcCustomerToday.Name = "pcCustomerToday";
            this.pcCustomerToday.Size = new System.Drawing.Size(223, 212);
            this.pcCustomerToday.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pcCustomerToday.TabIndex = 5;
            this.pcCustomerToday.TabStop = false;
            // 
            // CashierHomeDashboard
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1904, 1041);
            this.Controls.Add(this.panelMainForm);
            this.Controls.Add(this.panel1);
            this.Name = "CashierHomeDashboard";
            this.Text = "Cashier Dashboard";
            this.Load += new System.EventHandler(this.CashierHomeDashboard_Load);
            this.panel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.panelMainForm.ResumeLayout(false);
            this.panelMainForm.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.panel4.ResumeLayout(false);
            this.panel4.PerformLayout();
            this.panel5.ResumeLayout(false);
            this.panel5.PerformLayout();
            this.panel7.ResumeLayout(false);
            this.panel7.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pcCustomerQueue)).EndInit();
            this.panel6.ResumeLayout(false);
            this.panel6.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pcCustomerToday)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnLogout;
        private System.Windows.Forms.Button btnQueue;
        private System.Windows.Forms.Button btnAppointments;
        private System.Windows.Forms.Button btnPayments;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Timer realTime;
        private System.Windows.Forms.Panel panelMainForm;
        private System.Windows.Forms.Panel panel7;
        private System.Windows.Forms.Label lblCustomerQueue;
        private System.Windows.Forms.PictureBox pcCustomerQueue;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panel6;
        private System.Windows.Forms.Label lblTodayAppointments;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Label lblTime;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.Label lblDate;
        private System.Windows.Forms.Button btnReports;
        private System.Windows.Forms.PictureBox pcCustomerToday;
    }
}


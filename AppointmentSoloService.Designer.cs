namespace SupladaSalonLayout
{
    partial class AppointmentSoloService
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
            this.txtCustomerLastName = new System.Windows.Forms.TextBox();
            this.label12 = new System.Windows.Forms.Label();
            this.TimePicker = new System.Windows.Forms.DateTimePicker();
            this.datePicker = new System.Windows.Forms.DateTimePicker();
            this.label11 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.txtCustomerContact = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtCustomerFirstName = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.cbServiceType = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.btnCreateAppointment = new System.Windows.Forms.Button();
            this.clServices = new System.Windows.Forms.CheckedListBox();
            this.label2 = new System.Windows.Forms.Label();
            this.btnCancel = new System.Windows.Forms.Button();
            this.dataSchedule = new System.Windows.Forms.DataGridView();
            this.dataAddServices = new System.Windows.Forms.DataGridView();
            this.label6 = new System.Windows.Forms.Label();
            this.btnAdd = new System.Windows.Forms.Button();
            this.btnRemove = new System.Windows.Forms.Button();
            this.dateOccupiedSched = new System.Windows.Forms.DateTimePicker();
            this.label7 = new System.Windows.Forms.Label();
            this.lblTotalServices = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dataSchedule)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataAddServices)).BeginInit();
            this.SuspendLayout();
            // 
            // txtCustomerLastName
            // 
            this.txtCustomerLastName.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtCustomerLastName.Location = new System.Drawing.Point(176, 114);
            this.txtCustomerLastName.Name = "txtCustomerLastName";
            this.txtCustomerLastName.Size = new System.Drawing.Size(259, 27);
            this.txtCustomerLastName.TabIndex = 47;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label12.Location = new System.Drawing.Point(17, 117);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(153, 19);
            this.label12.TabIndex = 46;
            this.label12.Text = "Customer Last Name:";
            // 
            // TimePicker
            // 
            this.TimePicker.CalendarFont = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TimePicker.CustomFormat = "h:mm tt";
            this.TimePicker.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TimePicker.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.TimePicker.Location = new System.Drawing.Point(178, 270);
            this.TimePicker.Name = "TimePicker";
            this.TimePicker.ShowUpDown = true;
            this.TimePicker.Size = new System.Drawing.Size(121, 27);
            this.TimePicker.TabIndex = 45;
            this.TimePicker.Value = new System.DateTime(2025, 10, 16, 10, 21, 0, 0);
            this.TimePicker.ValueChanged += new System.EventHandler(this.TimePicker_ValueChanged);
            // 
            // datePicker
            // 
            this.datePicker.CalendarFont = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.datePicker.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.datePicker.Location = new System.Drawing.Point(178, 215);
            this.datePicker.MinDate = new System.DateTime(2025, 10, 16, 0, 0, 0, 0);
            this.datePicker.Name = "datePicker";
            this.datePicker.Size = new System.Drawing.Size(257, 27);
            this.datePicker.TabIndex = 44;
            this.datePicker.Value = new System.DateTime(2025, 10, 16, 0, 0, 0, 0);
            this.datePicker.ValueChanged += new System.EventHandler(this.datePicker_ValueChanged);
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label11.Location = new System.Drawing.Point(28, 276);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(142, 19);
            this.label11.TabIndex = 43;
            this.label11.Text = "Appointment Time:";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label10.Location = new System.Drawing.Point(29, 221);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(141, 19);
            this.label10.TabIndex = 42;
            this.label10.Text = "Appointment Date:";
            // 
            // txtCustomerContact
            // 
            this.txtCustomerContact.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtCustomerContact.Location = new System.Drawing.Point(176, 160);
            this.txtCustomerContact.Name = "txtCustomerContact";
            this.txtCustomerContact.Size = new System.Drawing.Size(259, 27);
            this.txtCustomerContact.TabIndex = 41;
            this.txtCustomerContact.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtCustomerContact_KeyPress);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(44, 163);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(126, 19);
            this.label3.TabIndex = 40;
            this.label3.Text = "Contact Number:";
            // 
            // txtCustomerFirstName
            // 
            this.txtCustomerFirstName.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtCustomerFirstName.Location = new System.Drawing.Point(176, 70);
            this.txtCustomerFirstName.Name = "txtCustomerFirstName";
            this.txtCustomerFirstName.Size = new System.Drawing.Size(259, 27);
            this.txtCustomerFirstName.TabIndex = 39;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(15, 73);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(155, 19);
            this.label4.TabIndex = 38;
            this.label4.Text = "Customer First Name:";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Font = new System.Drawing.Font("Calibri", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label13.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(253)))), ((int)(((byte)(159)))), ((int)(((byte)(43)))));
            this.label13.Location = new System.Drawing.Point(12, 18);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(545, 39);
            this.label13.TabIndex = 48;
            this.label13.Text = "_________________________________";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Calibri", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(287, 39);
            this.label1.TabIndex = 52;
            this.label1.Text = "Create Appointment";
            // 
            // cbServiceType
            // 
            this.cbServiceType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbServiceType.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbServiceType.FormattingEnabled = true;
            this.cbServiceType.Location = new System.Drawing.Point(176, 336);
            this.cbServiceType.Name = "cbServiceType";
            this.cbServiceType.Size = new System.Drawing.Size(304, 27);
            this.cbServiceType.TabIndex = 53;
            this.cbServiceType.SelectedIndexChanged += new System.EventHandler(this.cbServiceType_SelectedIndexChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(53, 339);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(117, 19);
            this.label5.TabIndex = 55;
            this.label5.Text = "Type of Service:";
            // 
            // btnCreateAppointment
            // 
            this.btnCreateAppointment.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(164)))), ((int)(((byte)(244)))), ((int)(((byte)(207)))));
            this.btnCreateAppointment.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnCreateAppointment.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCreateAppointment.Location = new System.Drawing.Point(503, 451);
            this.btnCreateAppointment.Name = "btnCreateAppointment";
            this.btnCreateAppointment.Size = new System.Drawing.Size(192, 38);
            this.btnCreateAppointment.TabIndex = 56;
            this.btnCreateAppointment.Text = "Create Appointment";
            this.btnCreateAppointment.UseVisualStyleBackColor = false;
            this.btnCreateAppointment.Click += new System.EventHandler(this.btnCreateAppointment_Click);
            // 
            // clServices
            // 
            this.clServices.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.clServices.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.clServices.FormattingEnabled = true;
            this.clServices.Location = new System.Drawing.Point(176, 380);
            this.clServices.Name = "clServices";
            this.clServices.Size = new System.Drawing.Size(304, 167);
            this.clServices.TabIndex = 57;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(63, 383);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(107, 19);
            this.label2.TabIndex = 58;
            this.label2.Text = "Select Service:";
            // 
            // btnCancel
            // 
            this.btnCancel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(162)))), ((int)(((byte)(162)))));
            this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnCancel.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCancel.Location = new System.Drawing.Point(710, 452);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(196, 37);
            this.btnCancel.TabIndex = 59;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = false;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // dataSchedule
            // 
            this.dataSchedule.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(250)))), ((int)(((byte)(252)))));
            this.dataSchedule.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.dataSchedule.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataSchedule.Location = new System.Drawing.Point(969, 95);
            this.dataSchedule.Name = "dataSchedule";
            this.dataSchedule.Size = new System.Drawing.Size(506, 498);
            this.dataSchedule.TabIndex = 61;
            // 
            // dataAddServices
            // 
            this.dataAddServices.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(250)))), ((int)(((byte)(252)))));
            this.dataAddServices.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.dataAddServices.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataAddServices.Location = new System.Drawing.Point(503, 95);
            this.dataAddServices.Name = "dataAddServices";
            this.dataAddServices.Size = new System.Drawing.Size(403, 293);
            this.dataAddServices.TabIndex = 62;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Bold);
            this.label6.Location = new System.Drawing.Point(965, 73);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(139, 19);
            this.label6.TabIndex = 63;
            this.label6.Text = "Occupied Schedule";
            // 
            // btnAdd
            // 
            this.btnAdd.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(164)))), ((int)(((byte)(244)))), ((int)(((byte)(207)))));
            this.btnAdd.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnAdd.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAdd.Location = new System.Drawing.Point(176, 555);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(112, 38);
            this.btnAdd.TabIndex = 64;
            this.btnAdd.Text = "Add";
            this.btnAdd.UseVisualStyleBackColor = false;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // btnRemove
            // 
            this.btnRemove.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(162)))), ((int)(((byte)(162)))));
            this.btnRemove.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnRemove.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnRemove.Location = new System.Drawing.Point(294, 556);
            this.btnRemove.Name = "btnRemove";
            this.btnRemove.Size = new System.Drawing.Size(116, 37);
            this.btnRemove.TabIndex = 65;
            this.btnRemove.Text = "Remove";
            this.btnRemove.UseVisualStyleBackColor = false;
            this.btnRemove.Click += new System.EventHandler(this.btnRemove_Click);
            // 
            // dateOccupiedSched
            // 
            this.dateOccupiedSched.CalendarFont = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dateOccupiedSched.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dateOccupiedSched.Location = new System.Drawing.Point(1110, 65);
            this.dateOccupiedSched.MinDate = new System.DateTime(2025, 10, 16, 0, 0, 0, 0);
            this.dateOccupiedSched.Name = "dateOccupiedSched";
            this.dateOccupiedSched.Size = new System.Drawing.Size(257, 27);
            this.dateOccupiedSched.TabIndex = 66;
            this.dateOccupiedSched.Value = new System.DateTime(2025, 10, 16, 0, 0, 0, 0);
            this.dateOccupiedSched.ValueChanged += new System.EventHandler(this.dateOccupiedSched_ValueChanged);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(499, 73);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(126, 19);
            this.label7.TabIndex = 67;
            this.label7.Text = "Selected Services";
            // 
            // lblTotalServices
            // 
            this.lblTotalServices.AutoSize = true;
            this.lblTotalServices.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTotalServices.Location = new System.Drawing.Point(747, 400);
            this.lblTotalServices.Name = "lblTotalServices";
            this.lblTotalServices.Size = new System.Drawing.Size(47, 19);
            this.lblTotalServices.TabIndex = 68;
            this.lblTotalServices.Text = "Total:";
            // 
            // AppointmentSoloService
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoValidate = System.Windows.Forms.AutoValidate.EnablePreventFocusChange;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(237)))), ((int)(((byte)(212)))));
            this.ClientSize = new System.Drawing.Size(1487, 617);
            this.ControlBox = false;
            this.Controls.Add(this.lblTotalServices);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.dateOccupiedSched);
            this.Controls.Add(this.btnRemove);
            this.Controls.Add(this.btnAdd);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.dataAddServices);
            this.Controls.Add(this.dataSchedule);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.clServices);
            this.Controls.Add(this.btnCreateAppointment);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.cbServiceType);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtCustomerLastName);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.TimePicker);
            this.Controls.Add(this.datePicker);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.txtCustomerContact);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtCustomerFirstName);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label13);
            this.Name = "AppointmentSoloService";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Load += new System.EventHandler(this.AppointmentSoloService_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataSchedule)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataAddServices)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtCustomerLastName;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.DateTimePicker TimePicker;
        private System.Windows.Forms.DateTimePicker datePicker;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox txtCustomerContact;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtCustomerFirstName;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cbServiceType;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button btnCreateAppointment;
        private System.Windows.Forms.CheckedListBox clServices;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.DataGridView dataSchedule;
        private System.Windows.Forms.DataGridView dataAddServices;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.Button btnRemove;
        private System.Windows.Forms.DateTimePicker dateOccupiedSched;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label lblTotalServices;
    }
}
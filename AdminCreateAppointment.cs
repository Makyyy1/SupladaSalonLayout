using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SupladaSalonLayout
{
    public partial class AdminCreateAppointment : Form
    {

        private int selectedAppointmentID = -1;
        private int currentUserID = -1;

        public AdminCreateAppointment()
        {
            InitializeComponent();
            LoadAppointments();
        }

        public AdminCreateAppointment(int userID)
        {
            InitializeComponent();
            currentUserID = userID;
            LoadAppointments();
        }

        public int GetCurrentUserID()
        {
            return currentUserID;
        }

        private void RefreshDashboard()
        {
            foreach (Form form in Application.OpenForms)
            {
                if (form is AdminHomeDashboard)
                {
                    ((AdminHomeDashboard)form).RefreshCounts();
                    break;
                }
                else if (form is CashierHomeDashboard)
                {
                    ((CashierHomeDashboard)form).RefreshCounts();
                    break;
                }
            }
        }


        private bool HasServiceNameColumn()
        {
            // For now, assume Service Name column exists since we updated the database
            // This avoids multiple database connections that might cause locking issues
            return true;
        }

        public void LoadAppointments()
        {
            try
            {
                using (SqlConnection connect = new SqlConnection(DB_Salon.connectionString))
                {
                    connect.Open();
                    // Check if Service Name column exists and use appropriate query
                    string query;
                    DataTable data;
                    bool hasServiceNameColumn = HasServiceNameColumn();
                    
                    if (hasServiceNameColumn)
                    {
                        query = @"
                        SELECT
                        a.AppointmentID,
                        a.CustomerFirstName + ' ' + a.CustomerLastName AS CustomerName,
                        a.CustomerFirstName, 
                        a.CustomerLastName, 
                        a.CustomerContact,
                        a.[Service Name] AS Services,
                        a.AppointmentDate, 
                        CASE 
                            WHEN a.AppointmentTime IS NULL OR CONVERT(VARCHAR(8), a.AppointmentTime, 108) = '00:00:00' 
                            THEN '' 
                            ELSE LTRIM(RIGHT(CONVERT(VARCHAR(20), CAST('1900-01-01 ' + CONVERT(VARCHAR(8), a.AppointmentTime, 108) AS DATETIME), 100), 7))
                        END AS AppointmentTime,  
                        a.Status 
                        FROM Appointments a 
                        WHERE a.Status = 'Confirmed' AND a.UserID = @UserID
                        ORDER BY a.AppointmentID ASC";
                    }
                    else
                    {
                        // Fallback to Service1 if Service Name doesn't exist
                        query = @"
                        SELECT
                        a.AppointmentID,
                        a.CustomerFirstName + ' ' + a.CustomerLastName AS CustomerName,
                        a.CustomerFirstName, 
                        a.CustomerLastName, 
                        a.CustomerContact,
                        a.Service1 AS Services,
                        a.AppointmentDate, 
                        CASE 
                            WHEN a.AppointmentTime IS NULL OR CONVERT(VARCHAR(8), a.AppointmentTime, 108) = '00:00:00' 
                            THEN '' 
                            ELSE LTRIM(RIGHT(CONVERT(VARCHAR(20), CAST('1900-01-01 ' + CONVERT(VARCHAR(8), a.AppointmentTime, 108) AS DATETIME), 100), 7))
                        END AS AppointmentTime,  
                        a.Status 
                        FROM Appointments a 
                        WHERE a.Status = 'Confirmed' AND a.UserID = @UserID
                        ORDER BY a.AppointmentID ASC";
                    }

                    SqlCommand cmd = new SqlCommand(query, connect);
                    cmd.Parameters.AddWithValue("@UserID", currentUserID);
                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    data = new DataTable();
                    adapter.Fill(data);

                    // Clear existing columns
                    dataAppointments.DataSource = null;
                    dataAppointments.Columns.Clear();

                    // Set the data source
                    dataAppointments.DataSource = data;

                    // Hide columns that are not needed for display
                    dataAppointments.Columns["AppointmentID"].Visible = false;
                    dataAppointments.Columns["CustomerFirstName"].Visible = false;
                    dataAppointments.Columns["CustomerLastName"].Visible = false;

                    // Add button columns
                    DataGridViewButtonColumn rescheduleBtn = new DataGridViewButtonColumn();
                    rescheduleBtn.Name = "Reschedule";
                    rescheduleBtn.HeaderText = "Reschedule";
                    rescheduleBtn.Text = "Reschedule";
                    rescheduleBtn.UseColumnTextForButtonValue = true;
                    dataAppointments.Columns.Add(rescheduleBtn);

                    DataGridViewButtonColumn cancelBtn = new DataGridViewButtonColumn();
                    cancelBtn.Name = "Cancel";
                    cancelBtn.HeaderText = "Cancel";
                    cancelBtn.Text = "Cancel";
                    cancelBtn.UseColumnTextForButtonValue = true;
                    dataAppointments.Columns.Add(cancelBtn);

                    // Set column order - MUST be done in REVERSE order or set all at once
                    dataAppointments.Columns["Cancel"].DisplayIndex = 7;
                    dataAppointments.Columns["Reschedule"].DisplayIndex = 6;
                    dataAppointments.Columns["Status"].DisplayIndex = 5;
                    dataAppointments.Columns["Services"].DisplayIndex = 4;
                    dataAppointments.Columns["Services"].HeaderText = "Services";
                    dataAppointments.Columns["CustomerContact"].DisplayIndex = 3;
                    dataAppointments.Columns["CustomerName"].DisplayIndex = 2;
                    dataAppointments.Columns["AppointmentTime"].DisplayIndex = 1;
                    dataAppointments.Columns["AppointmentDate"].DisplayIndex = 0;

                    // Apply button styles
                    ApplyButtonStylesToAllRows();
                } 
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading appointments: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /*
        private void ClearFields()
        {
            txtCustomerFirstName.Clear();
            txtCustomerLastName.Clear();
            txtCustomerContact.Clear();
            datePicker.Value = DateTime.Now;
            dateTimePicker.Value = DateTime.Now;

            for (int i = 0; i < checkListServices.Items.Count; i++)
            {
                checkListServices.SetItemChecked(i, false);
            }
        }

        private bool ValidateInputs()
        {
            if (string.IsNullOrWhiteSpace(txtCustomerFirstName.Text))
            {
                MessageBox.Show("Customer first name is required.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtCustomerFirstName.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtCustomerLastName.Text))
            {
                MessageBox.Show("Customer last name is required.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtCustomerLastName.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtCustomerContact.Text))
            {
                MessageBox.Show("Contact number is required.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtCustomerContact.Focus();
                return false;
            }

            if (txtCustomerContact.Text.Length != 11 || !txtCustomerContact.Text.All(char.IsDigit))
            {
                MessageBox.Show("Contact number must be exactly 11 digits.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtCustomerContact.Focus();
                return false;
            }

            int checkedCount = checkListServices.CheckedItems.Count;
            if (checkedCount == 0 || checkedCount > 2)
            {
                MessageBox.Show("Please select 1 or 2 services.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            return true;
        }*/

        private void btnAddQueue_Click(object sender, EventArgs e)
        {
            if (selectedAppointmentID == -1)
            {
                MessageBox.Show("Please select an appointment first.", "No Selection", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                using (SqlConnection connect = new SqlConnection(DB_Salon.connectionString))
                {
                    connect.Open();

                    string updateQuery = "UPDATE [dbo].[Appointments] SET [Status] = @Status WHERE [AppointmentID] = @AppointmentID";
                    SqlCommand cmd = new SqlCommand(updateQuery, connect);
                    cmd.Parameters.AddWithValue("@Status", "On going");
                    cmd.Parameters.AddWithValue("@AppointmentID", selectedAppointmentID);

                    int rowsAffected = cmd.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Appointment moved to queue successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        LoadAppointments();
                        RefreshDashboard();

                        selectedAppointmentID = -1;

                        foreach (Form form in Application.OpenForms)
                        {
                            if (form is AdminManageQueue)
                            {
                                ((AdminManageQueue)form).LoadQueue();
                                break;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error moving appointment to queue: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnViewQueue_Click(object sender, EventArgs e)
        {
            AdminManageQueue queueForm = new AdminManageQueue();
            queueForm.ShowDialog();
        }

        private void AdminCreateAppointment_Load(object sender, EventArgs e)
        {
            
        }

        private void btnSoloService_Click(object sender, EventArgs e)
        {
            AppointmentSoloService solo = new AppointmentSoloService(this);
            solo.ShowDialog();
        }

        private void AddActionButtons()
        {
            // Remove existing button columns if they exist to ensure clean state
            if (dataAppointments.Columns.Contains("Reschedule"))
            {
                dataAppointments.Columns.Remove("Reschedule");
            }
            if (dataAppointments.Columns.Contains("Cancel"))
            {
                dataAppointments.Columns.Remove("Cancel");
            }

            // Add Reschedule button column
            DataGridViewButtonColumn rescheduleColumn = new DataGridViewButtonColumn();
            rescheduleColumn.Name = "Reschedule";
            rescheduleColumn.HeaderText = "Reschedule";
            rescheduleColumn.Text = "Reschedule";
            rescheduleColumn.UseColumnTextForButtonValue = true;
            rescheduleColumn.Width = 100;
            rescheduleColumn.FlatStyle = FlatStyle.Flat;
            DataGridViewCellStyle rescheduleStyle = new DataGridViewCellStyle();
            rescheduleStyle.BackColor = Color.FromArgb(255, 193, 7);
            rescheduleStyle.ForeColor = Color.White;
            rescheduleStyle.SelectionBackColor = Color.FromArgb(255, 193, 7);
            rescheduleStyle.SelectionForeColor = Color.White;
            rescheduleStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            rescheduleColumn.DefaultCellStyle = rescheduleStyle;
            dataAppointments.Columns.Add(rescheduleColumn);

            // Add Cancel button column
            DataGridViewButtonColumn cancelColumn = new DataGridViewButtonColumn();
            cancelColumn.Name = "Cancel";
            cancelColumn.HeaderText = "Cancel";
            cancelColumn.Text = "Cancel";
            cancelColumn.UseColumnTextForButtonValue = true;
            cancelColumn.Width = 100;
            cancelColumn.FlatStyle = FlatStyle.Flat;
            DataGridViewCellStyle cancelStyle = new DataGridViewCellStyle();
            cancelStyle.BackColor = Color.FromArgb(220, 53, 69);
            cancelStyle.ForeColor = Color.White;
            cancelStyle.SelectionBackColor = Color.FromArgb(220, 53, 69);
            cancelStyle.SelectionForeColor = Color.White;
            cancelStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            cancelColumn.DefaultCellStyle = cancelStyle;
            dataAppointments.Columns.Add(cancelColumn);
        }

        private void ApplyButtonStylesToAllRows()
        {
            // Ensure all button cells have the correct styles
            foreach (DataGridViewRow row in dataAppointments.Rows)
            {
                if (row.Cells["Reschedule"] != null)
                {
                    row.Cells["Reschedule"].Style.BackColor = Color.FromArgb(255, 193, 7);
                    row.Cells["Reschedule"].Style.ForeColor = Color.White;
                    row.Cells["Reschedule"].Style.SelectionBackColor = Color.FromArgb(255, 193, 7);
                    row.Cells["Reschedule"].Style.SelectionForeColor = Color.White;
                    row.Cells["Reschedule"].Value = "Reschedule";
                }
                if (row.Cells["Cancel"] != null)
                {
                    row.Cells["Cancel"].Style.BackColor = Color.FromArgb(220, 53, 69);
                    row.Cells["Cancel"].Style.ForeColor = Color.White;
                    row.Cells["Cancel"].Style.SelectionBackColor = Color.FromArgb(220, 53, 69);
                    row.Cells["Cancel"].Style.SelectionForeColor = Color.White;
                    row.Cells["Cancel"].Value = "Cancel";
                }
            }
        }

        private void ReorderColumns()
        {
            if (dataAppointments.Columns.Count == 0) return;

            // Temporarily disable auto-size to prevent flickering
            dataAppointments.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;

            // Define the desired column order with their display indices
            int index = 0;

            if (dataAppointments.Columns.Contains("AppointmentDate"))
                dataAppointments.Columns["AppointmentDate"].DisplayIndex = index++;

            if (dataAppointments.Columns.Contains("AppointmentTime"))
                dataAppointments.Columns["AppointmentTime"].DisplayIndex = index++;

            if (dataAppointments.Columns.Contains("CustomerName"))
                dataAppointments.Columns["CustomerName"].DisplayIndex = index++;

            if (dataAppointments.Columns.Contains("CustomerContact"))
                dataAppointments.Columns["CustomerContact"].DisplayIndex = index++;

            if (dataAppointments.Columns.Contains("Service1"))
                dataAppointments.Columns["Service1"].DisplayIndex = index++;

            if (dataAppointments.Columns.Contains("Service2"))
                dataAppointments.Columns["Service2"].DisplayIndex = index++;

            if (dataAppointments.Columns.Contains("Status"))
                dataAppointments.Columns["Status"].DisplayIndex = index++;

            if (dataAppointments.Columns.Contains("Reschedule"))
                dataAppointments.Columns["Reschedule"].DisplayIndex = index++;

            if (dataAppointments.Columns.Contains("Cancel"))
                dataAppointments.Columns["Cancel"].DisplayIndex = index++;

            // Re-enable auto-size if needed
            dataAppointments.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }

        private void dataAppointments_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                DataGridViewRow row = dataAppointments.Rows[e.RowIndex];
                string columnName = dataAppointments.Columns[e.ColumnIndex].Name;

                if (row.Cells["AppointmentID"].Value != null)
                {
                    selectedAppointmentID = Convert.ToInt32(row.Cells["AppointmentID"].Value);
                }

                if (columnName == "Reschedule")
                {
                    RescheduleAppointment(selectedAppointmentID, row);
                }
                else if (columnName == "Cancel")
                {
                    CancelAppointment(selectedAppointmentID, row);
                }
            }
        }

        private void RescheduleAppointment(int appointmentID, DataGridViewRow row)
        {
            if (appointmentID == -1)
            {
                MessageBox.Show("Please select an appointment first.", "No Selection", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                DateTime currentDate = DateTime.Today;
                TimeSpan currentTime = new TimeSpan(9, 0, 0);

                using (SqlConnection connect = new SqlConnection(DB_Salon.connectionString))
                {
                    connect.Open();
                    string getQuery = "SELECT AppointmentDate, AppointmentTime FROM Appointments WHERE AppointmentID = @AppointmentID";
                    using (SqlCommand getCmd = new SqlCommand(getQuery, connect))
                    {
                        getCmd.Parameters.AddWithValue("@AppointmentID", appointmentID);
                        using (SqlDataReader reader = getCmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                if (reader["AppointmentDate"] != DBNull.Value)
                                {
                                    currentDate = Convert.ToDateTime(reader["AppointmentDate"]);
                                }
                                if (reader["AppointmentTime"] != DBNull.Value)
                                {
                                    object timeValue = reader["AppointmentTime"];
                                    if (timeValue is TimeSpan)
                                    {
                                        currentTime = (TimeSpan)timeValue;
                                    }
                                    else
                                    {
                                        string timeString = timeValue.ToString();
                                        if (!string.IsNullOrEmpty(timeString) && timeString != "00:00:00")
                                        {
                                            if (TimeSpan.TryParse(timeString, out TimeSpan parsedTime))
                                            {
                                                currentTime = parsedTime;
                                            }
                                            else if (DateTime.TryParseExact(timeString, new[] { "HH:mm", "HH:mm:ss", "h:mm tt", "hh:mm tt" }, 
                                                null, System.Globalization.DateTimeStyles.None, out DateTime timeAsDateTime))
                                            {
                                                currentTime = timeAsDateTime.TimeOfDay;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }

                string customerName = row.Cells["CustomerName"].Value?.ToString() ?? 
                    $"{row.Cells["CustomerFirstName"].Value} {row.Cells["CustomerLastName"].Value}";

                // Open reschedule form
                using (AppointmentReschedule rescheduleForm = new AppointmentReschedule(appointmentID, customerName, currentDate, currentTime))
                {
                    if (rescheduleForm.ShowDialog(this) == DialogResult.OK)
                    {
                        LoadAppointments();
                        RefreshDashboard();
                        selectedAppointmentID = -1;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error rescheduling appointment: " + ex.Message, "Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CancelAppointment(int appointmentID, DataGridViewRow row)
        {
            if (appointmentID == -1)
            {
                MessageBox.Show("Please select an appointment first.", "No Selection", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                string customerName = row.Cells["CustomerName"].Value?.ToString() ?? 
                    $"{row.Cells["CustomerFirstName"].Value} {row.Cells["CustomerLastName"].Value}";

                DialogResult confirm = MessageBox.Show(
                    $"Are you sure you want to cancel the appointment for {customerName}?",
                    "Confirm Cancellation",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (confirm == DialogResult.Yes)
                {
                    using (SqlConnection connect = new SqlConnection(DB_Salon.connectionString))
                    {
                        connect.Open();
                        string updateQuery = "UPDATE Appointments SET Status = 'Cancelled' WHERE AppointmentID = @AppointmentID";

                        using (SqlCommand cmd = new SqlCommand(updateQuery, connect))
                        {
                            cmd.Parameters.AddWithValue("@AppointmentID", appointmentID);
                            int rowsAffected = cmd.ExecuteNonQuery();

                            if (rowsAffected > 0)
                            {
                                MessageBox.Show("Appointment cancelled successfully.", "Success",
                                    MessageBoxButtons.OK, MessageBoxIcon.Information);

                                LoadAppointments();
                                RefreshDashboard();
                                selectedAppointmentID = -1;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error cancelling appointment: " + ex.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void pcClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSoloService_Click_1(object sender, EventArgs e)
        {
            AppointmentSoloService crtService = new AppointmentSoloService(this);
            crtService.Show();
        }
    }
}

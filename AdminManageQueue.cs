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
    public partial class AdminManageQueue : Form
    {
        private int currentUserID = -1;

        public AdminManageQueue()
        {
            InitializeComponent();
        }

        public AdminManageQueue(int userID)
        {
            InitializeComponent();
            currentUserID = userID;
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

        public void LoadQueue()
        {
            try
            {
                using (SqlConnection connect = new SqlConnection(DB_Salon.connectionString))
                {
                    connect.Open();
                    string query = @"
                                    SELECT
                                    a.AppointmentID,
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
                                WHERE a.Status = 'On going' AND a.UserID = @UserID
                                ORDER BY a.AppointmentDate DESC, a.AppointmentTime DESC";
                    SqlCommand cmd = new SqlCommand(query, connect);
                    cmd.Parameters.AddWithValue("@UserID", currentUserID);
                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    DataTable data = new DataTable();
                    adapter.Fill(data);

                    dataQueue.DataSource = data;

                    if (dataQueue.Columns.Contains("AppointmentID"))
                    {
                        dataQueue.Columns["AppointmentID"].Visible = false;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading queue: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void AdminManageQueue_Load(object sender, EventArgs e)
        {
            LoadQueue();
        }

        private void pcClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnCancelAppointment_Click(object sender, EventArgs e)
        {
            try
            {
                if (dataQueue.SelectedRows.Count == 0)
                {
                    MessageBox.Show("Please select an appointment to cancel.", "No Selection",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                DataGridViewRow selectedRow = dataQueue.SelectedRows[0];

                int appointmentId = Convert.ToInt32(selectedRow.Cells["AppointmentID"].Value);

                string customerName = $"{selectedRow.Cells["CustomerFirstName"].Value} {selectedRow.Cells["CustomerLastName"].Value}";

                DialogResult result = MessageBox.Show(
                    $"Are you sure you want to cancel the appointment for {customerName}?",
                    "Confirm Cancellation",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    using (SqlConnection connect = new SqlConnection(DB_Salon.connectionString))
                    {
                        connect.Open();
                        string updateQuery = "UPDATE Appointments SET Status = 'Cancelled' WHERE AppointmentID = @AppointmentID";

                        using (SqlCommand cmd = new SqlCommand(updateQuery, connect))
                        {
                            cmd.Parameters.AddWithValue("@AppointmentID", appointmentId);
                            int rowsAffected = cmd.ExecuteNonQuery();

                            if (rowsAffected > 0)
                            {
                                MessageBox.Show("Appointment cancelled successfully.", "Success",
                                    MessageBoxButtons.OK, MessageBoxIcon.Information);

                                LoadQueue();
                                RefreshDashboard();
                            }
                            else
                            {
                                MessageBox.Show("Failed to cancel appointment.", "Error",
                                    MessageBoxButtons.OK, MessageBoxIcon.Error);
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

        private void btnReadyBilling_Click(object sender, EventArgs e)
        {
            if (dataQueue.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select an appointment to mark as Ready for Billing.", 
                    "No Selection", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                DataGridViewRow selectedRow = dataQueue.SelectedRows[0];
                int appointmentId = Convert.ToInt32(selectedRow.Cells["AppointmentID"].Value);
                string customerName = $"{selectedRow.Cells["CustomerFirstName"].Value} {selectedRow.Cells["CustomerLastName"].Value}";

                DialogResult result = MessageBox.Show(
                    $"Mark {customerName}'s appointment as Ready for Billing?",
                    "Confirm Billing",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    using (SqlConnection connect = new SqlConnection(DB_Salon.connectionString))
                    {
                        connect.Open();
                        string updateQuery = "UPDATE Appointments SET Status = 'Ready for Billing' WHERE AppointmentID = @AppointmentID";

                        using (SqlCommand cmd = new SqlCommand(updateQuery, connect))
                        {
                            cmd.Parameters.AddWithValue("@AppointmentID", appointmentId);
                            int rowsAffected = cmd.ExecuteNonQuery();

                            if (rowsAffected > 0)
                            {
                                LoadQueue();
                                RefreshDashboard();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error processing billing: " + ex.Message, 
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}

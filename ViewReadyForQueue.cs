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
    public partial class ViewReadyForQueue : Form
    {
        private int currentUserID = -1;
        private Timer dateTimer;

        public ViewReadyForQueue(int userID)
        {
            InitializeComponent();
            currentUserID = userID;
            InitializeDateLabel();
            LoadTodayAppointments();
        }

        private void InitializeDateLabel()
        {
            // Update date label with current date
            lblTodayDate.Text = $"Today's Date: {DateTime.Now:MMMM dd, yyyy}";
            
            // Set up timer to update date every minute (optional, for real-time updates)
            dateTimer = new Timer();
            dateTimer.Interval = 60000; // 1 minute
            dateTimer.Tick += DateTimer_Tick;
            dateTimer.Start();
        }

        private void DateTimer_Tick(object sender, EventArgs e)
        {
            lblTodayDate.Text = $"Today's Date: {DateTime.Now:MMMM dd, yyyy}";
        }

        private void LoadTodayAppointments()
        {
            try
            {
                using (SqlConnection connect = new SqlConnection(DB_Salon.connectionString))
                {
                    connect.Open();
                    string query = @"
                        SELECT
                            a.AppointmentID,
                            a.CustomerFirstName + ' ' + a.CustomerLastName AS CustomerName,
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
                        WHERE a.Status = 'Confirmed' 
                        AND a.UserID = @UserID
                        AND CAST(a.AppointmentDate AS DATE) = CAST(GETDATE() AS DATE)
                        ORDER BY a.AppointmentTime ASC";

                    SqlCommand cmd = new SqlCommand(query, connect);
                    cmd.Parameters.AddWithValue("@UserID", currentUserID);
                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    DataTable data = new DataTable();
                    adapter.Fill(data);

                    dataReadyForQueue.DataSource = data;

                    if (dataReadyForQueue.Columns.Contains("AppointmentID"))
                    {
                        dataReadyForQueue.Columns["AppointmentID"].Visible = false;
                    }

                    // Set column headers and formatting
                    if (dataReadyForQueue.Columns.Contains("CustomerName"))
                    {
                        dataReadyForQueue.Columns["CustomerName"].HeaderText = "Customer Name";
                        dataReadyForQueue.Columns["CustomerName"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                    }

                    if (dataReadyForQueue.Columns.Contains("CustomerContact"))
                    {
                        dataReadyForQueue.Columns["CustomerContact"].HeaderText = "Contact";
                        dataReadyForQueue.Columns["CustomerContact"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                    }

                    if (dataReadyForQueue.Columns.Contains("Services"))
                    {
                        dataReadyForQueue.Columns["Services"].HeaderText = "Services";
                        dataReadyForQueue.Columns["Services"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                    }

                    if (dataReadyForQueue.Columns.Contains("AppointmentDate"))
                    {
                        dataReadyForQueue.Columns["AppointmentDate"].HeaderText = "Date";
                        dataReadyForQueue.Columns["AppointmentDate"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                    }

                    if (dataReadyForQueue.Columns.Contains("AppointmentTime"))
                    {
                        dataReadyForQueue.Columns["AppointmentTime"].HeaderText = "Time";
                        dataReadyForQueue.Columns["AppointmentTime"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                    }

                    if (dataReadyForQueue.Columns.Contains("Status"))
                    {
                        dataReadyForQueue.Columns["Status"].HeaderText = "Status";
                        dataReadyForQueue.Columns["Status"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading appointments: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void pcClose_Click(object sender, EventArgs e)
        {
            if (dateTimer != null)
            {
                dateTimer.Stop();
                dateTimer.Dispose();
            }
            this.Close();
        }

        private void ViewReadyForQueue_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (dateTimer != null)
            {
                dateTimer.Stop();
                dateTimer.Dispose();
            }
        }
    }
}


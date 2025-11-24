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
    public partial class CashierHomeDashboard : Form
    {
        private string currentUsername;
        private string currentRole = "Cashier";
        private int currentUserID = -1;

        public CashierHomeDashboard(string username, int userID)
        {
            InitializeComponent();
            currentUsername = username;
            currentUserID = userID;
            customDesign();
            UpdateWelcomeMessage();
        }

        public int GetCurrentUserID()
        {
            return currentUserID;
        }

        private void customDesign()
        {
            // Hide submenus if any
        }

        private void hideSub()
        {
            // Hide submenus if any
        }

        private void LoadDashboardCounts()
        {
            LoadAppointmentCount();
            LoadQueueCount();
        }

        private void LoadAppointmentCount()
        {
            try
            {
                if (lblTodayAppointments == null)
                {
                    return;
                }

                using (SqlConnection conn = new SqlConnection(DB_Salon.connectionString))
                {
                    conn.Open();

                    string query = @"SELECT COUNT(*) 
                                   FROM Appointments 
                                   WHERE Status = 'Confirmed' AND UserID = @UserID";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@UserID", currentUserID);
                        object result = cmd.ExecuteScalar();
                        int count = result != null && result != DBNull.Value ? Convert.ToInt32(result) : 0;
                        lblTodayAppointments.Text = count.ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                if (lblTodayAppointments != null)
                {
                    lblTodayAppointments.Text = "0";
                }
                System.Diagnostics.Debug.WriteLine("Error loading appointment count: " + ex.Message);
            }
        }

        private void LoadQueueCount()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(DB_Salon.connectionString))
                {
                    conn.Open();

                    string query = @"SELECT COUNT(*) 
                                   FROM Appointments 
                                   WHERE Status = 'Ready for Billing' AND UserID = @UserID";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@UserID", currentUserID);
                        object result = cmd.ExecuteScalar();
                        int count = result != null && result != DBNull.Value ? Convert.ToInt32(result) : 0;
                        lblCustomerQueue.Text = count.ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                if (lblCustomerQueue != null)
                {
                    lblCustomerQueue.Text = "0";
                }
                System.Diagnostics.Debug.WriteLine("Error loading queue count: " + ex.Message);
            }
        }

        public void RefreshCounts()
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new Action(() => { LoadDashboardCounts(); }));
            }
            else
            {
                LoadDashboardCounts();
            }
        }

        private void UpdateWelcomeMessage()
        {
            if (this.Controls.Find("label1", true).Length > 0)
            {
                Label welcomeLabel = (Label)this.Controls.Find("label1", true)[0];
                welcomeLabel.Text = $"Welcome, {currentRole} {currentUsername}";
            }
        }

        private Form activeForm = null;

        private void openChildForm(Form childForm)
        {
            if (activeForm != null)
                activeForm.Close();
            activeForm = childForm;
            childForm.TopLevel = false;
            childForm.FormBorderStyle = FormBorderStyle.None;
            childForm.Dock = DockStyle.Fill;
            panelMainForm.Controls.Add(childForm);
            panelMainForm.Tag = childForm;
            childForm.BringToFront();
            childForm.Show();
        }

        private void CashierHomeDashboard_Load(object sender, EventArgs e)
        {
            realTime.Start();
            lblDate.Text = DateTime.Now.ToLongDateString();
            lblTime.Text = DateTime.Now.ToString("h:mm tt");

            UpdateWelcomeMessage();
            
            if (lblTodayAppointments != null && lblCustomerQueue != null)
            {
                LoadDashboardCounts();
            }
        }

        private void realTime_Tick(object sender, EventArgs e)
        {
            lblTime.Text = DateTime.Now.ToString("h:mm tt");
            realTime.Start();
        }

        private void btnPayments_Click(object sender, EventArgs e)
        {
            openChildForm(new AdminPayments(currentUserID));
        }

        private void btnAppointments_Click(object sender, EventArgs e)
        {
            openChildForm(new AdminCreateAppointment(currentUserID));
        }

        private void btnQueue_Click(object sender, EventArgs e)
        {
            openChildForm(new AdminManageQueue(currentUserID));
        }

        private void btnReports_Click(object sender, EventArgs e)
        {
            openChildForm(new AdminReports(currentUserID));
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            DialogResult logout = MessageBox.Show("Are you sure you want to logout?", "Confirmation", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
            if (logout == DialogResult.OK)
            {
                this.Close();
                LoginForm login = new LoginForm();
                login.Show();
            }
            if (logout == DialogResult.Cancel)
            {
                this.Focus();
            }
        }

        private void lblTodayAppointments_Click(object sender, EventArgs e)
        {
            openChildForm(new AdminCreateAppointment(currentUserID));
        }

        private void lblCustomerQueue_Click(object sender, EventArgs e)
        {
            openChildForm(new AdminManageQueue(currentUserID));
        }
    }
}


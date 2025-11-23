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
    public partial class AdminHomeDashboard : Form
    {
        private string currentUsername = "Admin";
        private string currentRole = "Admin";
        private int currentUserID = -1;

        public AdminHomeDashboard()
        {
            InitializeComponent();
            customDesign();
            SetupClickableAreas();
        }

        public AdminHomeDashboard(string username, string role, int userID) : this()
        {
            currentUsername = username;
            currentRole = role;
            currentUserID = userID;
            UpdateWelcomeMessage();
        }

        public int GetCurrentUserID()
        {
            return currentUserID;
        }

        private void customDesign()
        {
            panelSubmenu.Visible = false;
            panelAppointmentSubmenu.Visible = false;
        }

        private void hideSub()
        {
            panelSubmenu.Visible = false;
            panelAppointmentSubmenu.Visible = false;

            /*if (panelSubmenu.Visible == true)
                panelSubmenu.Visible = false;

            if (panelAppointmentSubmenu.Visible == true)
                panelAppointmentSubmenu.Visible = false;
            */
        }

        private void showSub(Panel subMenu)
        {
            if (subMenu.Visible == true)
            {
                //hideSub();
                subMenu.Visible = false;
            }
            else
                hideSub();
                subMenu.Visible = true;
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

                    // Count appointments with Status = 'Confirmed' for current user
                    string query = @"SELECT COUNT(*) 
                                   FROM Appointments 
                                   WHERE Status = 'Confirmed' AND UserID = @UserID";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@UserID", currentUserID);
                        object result = cmd.ExecuteScalar();
                        int count = result != null && result != DBNull.Value ? Convert.ToInt32(result) : 0;

                        // Update label directly
                        lblTodayAppointments.Text = count.ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                // Set to 0 on error
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
                                   WHERE Status = 'On going' AND UserID = @UserID";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@UserID", currentUserID);
                        object result = cmd.ExecuteScalar();
                        int count = result != null && result != DBNull.Value ? Convert.ToInt32(result) : 0;

                        // Update label on UI thread
                        if (this.InvokeRequired)
                        {
                            this.Invoke(new Action(() => { lblCustomerQueue.Text = count.ToString(); }));
                        }
                        else
                        {
                            lblCustomerQueue.Text = count.ToString();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Set to 0 on error and log
                if (this.InvokeRequired)
                {
                    this.Invoke(new Action(() => { lblCustomerQueue.Text = "0"; }));
                }
                else
                {
                    lblCustomerQueue.Text = "0";
                }
                System.Diagnostics.Debug.WriteLine("Error loading queue count: " + ex.Message);
            }
        }

        public void RefreshCounts()
        {
            System.Diagnostics.Debug.WriteLine("RefreshCounts() called");
            // Ensure we're on the UI thread when refreshing
            if (this.InvokeRequired)
            {
                this.Invoke(new Action(() => { LoadDashboardCounts(); }));
            }
            else
            {
                LoadDashboardCounts();
            }
        }

        // Test method - you can call this from a button click or anywhere to test
        public void TestAppointmentCount()
        {
            System.Diagnostics.Debug.WriteLine("=== TEST: Manual appointment count test ===");
            
            // First test: Can we update the label at all?
            if (lblTodayAppointments != null)
            {
                lblTodayAppointments.Text = "TEST";
                System.Diagnostics.Debug.WriteLine("TEST: Label set to 'TEST' - check if it appears on screen");
                System.Threading.Thread.Sleep(1000);
            }
            
            LoadAppointmentCount();
        }

        private void SetupClickableAreas()
        {
            if (pcCustomerToday != null)
            {
                pcCustomerToday.Cursor = Cursors.Hand;
            }

            if (pcCustomerQueue != null)
            {
                pcCustomerQueue.Cursor = Cursors.Hand;
            }

            // Make the appointment count label clickable
            if (lblTodayAppointments != null)
            {
                lblTodayAppointments.Cursor = Cursors.Hand;
                lblTodayAppointments.Click += lblTodayAppointments_Click;
            }

            // Make the queue count label clickable
            if (lblCustomerQueue != null)
            {
                lblCustomerQueue.Cursor = Cursors.Hand;
                lblCustomerQueue.Click += lblCustomerQueue_Click;
            }
        }

        private void AdminHomeDashboard_Load(object sender, EventArgs e)
        {
            realTime.Start();
            lblDate.Text = DateTime.Now.ToLongDateString();
            lblTime.Text = DateTime.Now.ToString("h:mm tt");

            UpdateWelcomeMessage();
            
            // Ensure labels are initialized before loading counts
            if (lblTodayAppointments != null && lblCustomerQueue != null)
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

        private void realTime_Tick(object sender, EventArgs e)
        {
            lblTime.Text = DateTime.Now.ToString("h:mm tt");
            realTime.Start();
        }

        private void btnFileMaintenance_Click(object sender, EventArgs e)
        {
            showSub(panelSubmenu);
        }

        private void btnAppointment_Click(object sender, EventArgs e)
        {
            showSub(panelAppointmentSubmenu);
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

        private void btnServices_Click(object sender, EventArgs e)
        {
            openChildForm(new FMAdminServices());
        }

        private void btnCreateAppointment_Click(object sender, EventArgs e)
        {
            openChildForm(new AdminCreateAppointment(currentUserID));
        }

        private void btnManageQueue_Click(object sender, EventArgs e)
        {
            openChildForm(new AdminManageQueue(currentUserID));
        }

        private void btnProducts_Click(object sender, EventArgs e)
        {
            openChildForm(new FMAdminProducts());
        }

        private void btnTechnicians_Click(object sender, EventArgs e)
        {
            openChildForm(new FMAdminTechnicians());
        }

        private void btnPayments_Click(object sender, EventArgs e)
        {
            openChildForm(new AdminPayments(currentUserID));
            hideSub();
        }

        private void btnReports_Click(object sender, EventArgs e)
        {
            openChildForm(new AdminReports(currentUserID));
            hideSub();
        }

        private void btnDiscounts_Click(object sender, EventArgs e)
        {
            openChildForm(new AdminDiscounts());
            
        }

        private void btnUsers_Click(object sender, EventArgs e)
        {
            openChildForm(new AdminManageUsers());
            hideSub();
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

        private void pcCustomerToday_Click(object sender, EventArgs e)
        {
            openChildForm(new AdminCreateAppointment(currentUserID));
            hideSub();

            LoadAppointmentCount();
        }

        private void pcCustomerQueue_Click(object sender, EventArgs e)
        {
            openChildForm(new AdminManageQueue(currentUserID));
            hideSub();

            LoadQueueCount();
        }

        private void lblTodayAppointments_Click(object sender, EventArgs e)
        {
            openChildForm(new AdminCreateAppointment(currentUserID));
            hideSub();
        }

        private void lblCustomerQueue_Click(object sender, EventArgs e)
        {
            openChildForm(new AdminManageQueue(currentUserID));
            hideSub();
        }
    }
}

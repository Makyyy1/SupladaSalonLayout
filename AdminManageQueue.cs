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
        private string currentFilter = "All"; // Default filter

        public AdminManageQueue()
        {
            InitializeComponent();
        }

        public AdminManageQueue(int userID)
        {
            InitializeComponent();
            currentUserID = userID;
        }

        public void RefreshDashboard()
        {
            // Refresh both AdminHomeDashboard and CashierHomeDashboard if they are open
            // Also refresh ViewReadyForQueue if it's open
            foreach (Form form in Application.OpenForms)
            {
                if (form is AdminHomeDashboard)
                {
                    ((AdminHomeDashboard)form).RefreshCounts();
                }
                else if (form is CashierHomeDashboard)
                {
                    ((CashierHomeDashboard)form).RefreshCounts();
                }
                else if (form is ViewReadyForQueue)
                {
                    // Refresh ViewReadyForQueue form to show updated appointments
                    ((ViewReadyForQueue)form).ReloadAppointments();
                }
            }
        }

        private void OpenChildFormInDashboard(Form childForm)
        {
            try
            {
                // Find the parent dashboard
                foreach (Form form in Application.OpenForms)
                {
                    if (form is AdminHomeDashboard)
                    {
                        // Use reflection to call the private openChildForm method
                        System.Reflection.MethodInfo method = form.GetType().GetMethod("openChildForm",
                            System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                        if (method != null)
                        {
                            method.Invoke(form, new object[] { childForm });
                        }
                        break;
                    }
                    else if (form is CashierHomeDashboard)
                    {
                        // Use reflection to call the private openChildForm method
                        System.Reflection.MethodInfo method = form.GetType().GetMethod("openChildForm",
                            System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                        if (method != null)
                        {
                            method.Invoke(form, new object[] { childForm });
                        }
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error opening form: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void LoadQueue(string filter = "All")
        {
            try
            {
                using (SqlConnection connect = new SqlConnection(DB_Salon.connectionString))
                {
                    connect.Open();
                    
                    // Build WHERE clause based on filter
                    string whereClause = "";
                    switch (filter)
                    {
                        case "On Queue":
                            whereClause = "WHERE a.Status = 'On Queue'";
                            break;
                        case "On Going":
                            whereClause = "WHERE a.Status = 'On going'";
                            break;
                        case "All":
                        default:
                            whereClause = "WHERE a.Status IN ('On going', 'On Queue')";
                            break;
                    }
                    
                    string query = $@"
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
                                {whereClause}
                                ORDER BY a.AppointmentDate DESC, a.AppointmentTime DESC";
                    SqlCommand cmd = new SqlCommand(query, connect);
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
        
        private void UpdateFilterButtons(string activeFilter)
        {
            // Reset all button styles
            btnAll.BackColor = SystemColors.Control;
            btnOnQueue.BackColor = SystemColors.Control;
            btnOnGoing.BackColor = SystemColors.Control;
            
            // Highlight active filter button
            switch (activeFilter)
            {
                case "All":
                    btnAll.BackColor = Color.LightBlue;
                    break;
                case "On Queue":
                    btnOnQueue.BackColor = Color.LightBlue;
                    break;
                case "On Going":
                    btnOnGoing.BackColor = Color.LightBlue;
                    break;
            }
        }

        private void AdminManageQueue_Load(object sender, EventArgs e)
        {
            currentFilter = "All";
            UpdateFilterButtons("All");
            LoadQueue("All");
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
                    MessageBox.Show("Please select an appointment to move back.", "No Selection",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                DataGridViewRow selectedRow = dataQueue.SelectedRows[0];

                int appointmentId = Convert.ToInt32(selectedRow.Cells["AppointmentID"].Value);

                string customerName = $"{selectedRow.Cells["CustomerFirstName"].Value} {selectedRow.Cells["CustomerLastName"].Value}";

                DialogResult result = MessageBox.Show(
                    $"Remove {customerName}'s from Queue?",
                    "Confirm Move",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    using (SqlConnection connect = new SqlConnection(DB_Salon.connectionString))
                    {
                        connect.Open();
                        string updateQuery = "UPDATE Appointments SET Status = 'Confirmed' WHERE AppointmentID = @AppointmentID";

                        using (SqlCommand cmd = new SqlCommand(updateQuery, connect))
                        {
                            cmd.Parameters.AddWithValue("@AppointmentID", appointmentId);
                            int rowsAffected = cmd.ExecuteNonQuery();

                            if (rowsAffected > 0)
                            {
                                MessageBox.Show("Appointment moved back to Appointments successfully.", "Success",
                                    MessageBoxButtons.OK, MessageBoxIcon.Information);

                                LoadQueue(currentFilter);
                                RefreshDashboard();

                                // Refresh AdminCreateAppointment form if it's open
                                foreach (Form form in Application.OpenForms)
                                {
                                    if (form is AdminCreateAppointment)
                                    {
                                        ((AdminCreateAppointment)form).LoadAppointments();
                                        break;
                                    }
                                }
                            }
                            else
                            {
                                MessageBox.Show("Failed to move appointment.", "Error",
                                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error moving appointment: " + ex.Message, "Error",
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
                string currentStatus = selectedRow.Cells["Status"].Value?.ToString() ?? "";

                // Only allow "On going" status appointments to be ready for billing
                if (!currentStatus.Equals("On going", StringComparison.OrdinalIgnoreCase))
                {
                    MessageBox.Show("Only appointments with 'On going' status can be marked as Ready for Billing.\n\n" +
                        $"Current status: {currentStatus}", 
                        "Invalid Status", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                int appointmentId = Convert.ToInt32(selectedRow.Cells["AppointmentID"].Value);
                string firstName = selectedRow.Cells["CustomerFirstName"].Value?.ToString();
                string lastName = selectedRow.Cells["CustomerLastName"].Value?.ToString();
                string contact = selectedRow.Cells["CustomerContact"].Value?.ToString();
                DateTime appointmentDate = Convert.ToDateTime(selectedRow.Cells["AppointmentDate"].Value);
                string appointmentTimeStr = selectedRow.Cells["AppointmentTime"].Value?.ToString();
                string services = selectedRow.Cells["Services"].Value?.ToString();

                string customerName = $"{firstName} {lastName}";

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
                                LoadQueue(currentFilter);
                                RefreshDashboard();
                                
                                // Open AdminPayments form as child form in the dashboard panel
                                AdminPayments paymentsForm = new AdminPayments(currentUserID, appointmentId);
                                OpenChildFormInDashboard(paymentsForm);
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

        private void btnCancel_Click(object sender, EventArgs e)
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

                                LoadQueue(currentFilter);
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

        private void btnScheduledAppointments_Click(object sender, EventArgs e)
        {
            try
            {
                ViewReadyForQueue viewForm = new ViewReadyForQueue(currentUserID);
                viewForm.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error opening Ready for Queue view: " + ex.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnAddQueue_Click(object sender, EventArgs e)
        {
            try
            {
                WalkInQueue walkInForm = new WalkInQueue(this, currentUserID);
                walkInForm.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error opening Walk-In Queue form: " + ex.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnAll_Click(object sender, EventArgs e)
        {
            currentFilter = "All";
            UpdateFilterButtons("All");
            LoadQueue("All");
        }

        private void btnOnQueue_Click(object sender, EventArgs e)
        {
            currentFilter = "On Queue";
            UpdateFilterButtons("On Queue");
            LoadQueue("On Queue");
        }

        private void btnOnGoing_Click(object sender, EventArgs e)
        {
            currentFilter = "On Going";
            UpdateFilterButtons("On Going");
            LoadQueue("On Going");
        }

        private void dataQueue_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                int rowIndex = dataQueue.HitTest(e.X, e.Y).RowIndex;
                if (rowIndex >= 0)
                {
                    // Select the row that was right-clicked
                    if (!dataQueue.Rows[rowIndex].Selected)
                    {
                        dataQueue.ClearSelection();
                        dataQueue.Rows[rowIndex].Selected = true;
                    }
                    
                    // Show context menu
                    contextMenuQueue.Show(dataQueue, e.Location);
                }
            }
        }

        private void menuItemChangeToOnGoing_Click(object sender, EventArgs e)
        {
            try
            {
                if (dataQueue.SelectedRows.Count == 0)
                {
                    MessageBox.Show("Please select an appointment to change status.", "No Selection",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                DataGridViewRow selectedRow = dataQueue.SelectedRows[0];
                int appointmentId = Convert.ToInt32(selectedRow.Cells["AppointmentID"].Value);
                string currentStatus = selectedRow.Cells["Status"].Value?.ToString() ?? "";

                // Check if already "On going"
                if (currentStatus.Equals("On going", StringComparison.OrdinalIgnoreCase))
                {
                    MessageBox.Show("This appointment is already set to 'On going' status.", "Already Set",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                string customerName = $"{selectedRow.Cells["CustomerFirstName"].Value} {selectedRow.Cells["CustomerLastName"].Value}";

                DialogResult result = MessageBox.Show(
                    $"Change status to 'On going' for {customerName}?",
                    "Confirm Status Change",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    using (SqlConnection connect = new SqlConnection(DB_Salon.connectionString))
                    {
                        connect.Open();
                        string updateQuery = "UPDATE Appointments SET Status = 'On going' WHERE AppointmentID = @AppointmentID";

                        using (SqlCommand cmd = new SqlCommand(updateQuery, connect))
                        {
                            cmd.Parameters.AddWithValue("@AppointmentID", appointmentId);
                            int rowsAffected = cmd.ExecuteNonQuery();

                            if (rowsAffected > 0)
                            {
                                MessageBox.Show("Status changed to 'On going' successfully.", "Success",
                                    MessageBoxButtons.OK, MessageBoxIcon.Information);

                                LoadQueue(currentFilter);
                                RefreshDashboard();
                            }
                            else
                            {
                                MessageBox.Show("Failed to change status.", "Error",
                                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error changing status: " + ex.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void menuItemChangeToOnQueue_Click(object sender, EventArgs e)
        {
            try
            {
                if (dataQueue.SelectedRows.Count == 0)
                {
                    MessageBox.Show("Please select an appointment to change status.", "No Selection",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                DataGridViewRow selectedRow = dataQueue.SelectedRows[0];
                int appointmentId = Convert.ToInt32(selectedRow.Cells["AppointmentID"].Value);
                string currentStatus = selectedRow.Cells["Status"].Value?.ToString() ?? "";

                // Check if already "On Queue"
                if (currentStatus.Equals("On Queue", StringComparison.OrdinalIgnoreCase))
                {
                    MessageBox.Show("This appointment is already set to 'On Queue' status.", "Already Set",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                string customerName = $"{selectedRow.Cells["CustomerFirstName"].Value} {selectedRow.Cells["CustomerLastName"].Value}";

                DialogResult result = MessageBox.Show(
                    $"Change status to 'On Queue' for {customerName}?",
                    "Confirm Status Change",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    using (SqlConnection connect = new SqlConnection(DB_Salon.connectionString))
                    {
                        connect.Open();
                        string updateQuery = "UPDATE Appointments SET Status = 'On Queue' WHERE AppointmentID = @AppointmentID";

                        using (SqlCommand cmd = new SqlCommand(updateQuery, connect))
                        {
                            cmd.Parameters.AddWithValue("@AppointmentID", appointmentId);
                            int rowsAffected = cmd.ExecuteNonQuery();

                            if (rowsAffected > 0)
                            {
                                MessageBox.Show("Status changed to 'On Queue' successfully.", "Success",
                                    MessageBoxButtons.OK, MessageBoxIcon.Information);

                                LoadQueue(currentFilter);
                                RefreshDashboard();
                            }
                            else
                            {
                                MessageBox.Show("Failed to change status.", "Error",
                                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error changing status: " + ex.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}

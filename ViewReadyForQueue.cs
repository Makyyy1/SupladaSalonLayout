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
        private bool isSelectingAll = false;

        public ViewReadyForQueue(int userID)
        {
            InitializeComponent();
            currentUserID = userID;
            InitializeDateLabel();
            WireUpEventHandlers();
            LoadTodayAppointments();
            UpdateSelectedCount();
        }

        // Public method to reload appointments (can be called from other forms)
        public void ReloadAppointments()
        {
            LoadTodayAppointments();
            UpdateSelectedCount();
        }

        private void WireUpEventHandlers()
        {
            btnSelectAll.Click += btnSelectAll_Click;
            btnAddSelectedQueue.Click += btnAddSelectedQueue_Click;
            dataReadyForQueue.CellValueChanged += dataReadyForQueue_CellValueChanged;
            dataReadyForQueue.CellClick += dataReadyForQueue_CellClick;
            dataReadyForQueue.CurrentCellDirtyStateChanged += dataReadyForQueue_CurrentCellDirtyStateChanged;
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
                        AND CAST(a.AppointmentDate AS DATE) = CAST(GETDATE() AS DATE)
                        ORDER BY a.AppointmentTime ASC";

                    SqlCommand cmd = new SqlCommand(query, connect);
                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    DataTable data = new DataTable();
                    adapter.Fill(data);

                    // Set DataSource first
                    dataReadyForQueue.DataSource = data;

                    // Remove checkbox column if it exists (to avoid duplicates)
                    if (dataReadyForQueue.Columns.Contains("Select"))
                    {
                        dataReadyForQueue.Columns.Remove("Select");
                    }

                    // Add checkbox column after DataSource is set
                    DataGridViewCheckBoxColumn checkBoxColumn = new DataGridViewCheckBoxColumn();
                    checkBoxColumn.Name = "Select";
                    checkBoxColumn.HeaderText = "Select";
                    checkBoxColumn.Width = 60;
                    checkBoxColumn.ReadOnly = false;
                    checkBoxColumn.FalseValue = false;
                    checkBoxColumn.TrueValue = true;
                    checkBoxColumn.ThreeState = false;
                    dataReadyForQueue.Columns.Insert(0, checkBoxColumn);

                    // Ensure checkbox column is in first position and editable
                    if (dataReadyForQueue.Columns.Contains("Select"))
                    {
                        DataGridViewCheckBoxColumn selectCol = dataReadyForQueue.Columns["Select"] as DataGridViewCheckBoxColumn;
                        if (selectCol != null)
                        {
                            selectCol.DisplayIndex = 0;
                            selectCol.ReadOnly = false;
                            selectCol.Width = 60;
                            selectCol.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
                            selectCol.FalseValue = false;
                            selectCol.TrueValue = true;
                            selectCol.ThreeState = false;
                        }
                    }
                    
                    // Make other columns read-only except the checkbox
                    foreach (DataGridViewColumn col in dataReadyForQueue.Columns)
                    {
                        if (col.Name != "Select")
                        {
                            col.ReadOnly = true;
                        }
                    }

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

                    // Initialize all checkboxes to false
                    dataReadyForQueue.SuspendLayout();
                    foreach (DataGridViewRow row in dataReadyForQueue.Rows)
                    {
                        if (!row.IsNewRow && row.Cells["Select"] != null)
                        {
                            DataGridViewCheckBoxCell checkCell = row.Cells["Select"] as DataGridViewCheckBoxCell;
                            if (checkCell != null)
                            {
                                checkCell.Value = false;
                                checkCell.FalseValue = false;
                                checkCell.TrueValue = true;
                            }
                            else
                            {
                                row.Cells["Select"].Value = false;
                            }
                        }
                    }
                    dataReadyForQueue.ResumeLayout(true);
                    
                    // Force refresh
                    dataReadyForQueue.Invalidate();
                    dataReadyForQueue.Refresh();

                    UpdateSelectedCount();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading appointments: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void UpdateSelectedCount()
        {
            if (dataReadyForQueue.Rows.Count == 0)
            {
                lblSelected.Text = "0 of 0 selected";
                return;
            }

            int selectedCount = 0;
            int totalCount = 0;
            
            foreach (DataGridViewRow row in dataReadyForQueue.Rows)
            {
                if (!row.IsNewRow)
                {
                    totalCount++;
                    
                    if (row.Cells["Select"] != null && row.Cells["Select"].Value != null)
                    {
                        try
                        {
                            bool isChecked = row.Cells["Select"].Value is bool ? 
                                            (bool)row.Cells["Select"].Value : 
                                            Convert.ToBoolean(row.Cells["Select"].Value);
                            
                            if (isChecked)
                            {
                                selectedCount++;
                            }
                        }
                        catch
                        {
                            // If conversion fails, treat as unchecked
                        }
                    }
                }
            }

            lblSelected.Text = $"{selectedCount} of {totalCount} selected";
        }

        private void btnSelectAll_Click(object sender, EventArgs e)
        {
            if (dataReadyForQueue.Rows.Count == 0)
            {
                return;
            }

            // Temporarily remove event handlers to prevent multiple firings
            dataReadyForQueue.CellValueChanged -= dataReadyForQueue_CellValueChanged;
            dataReadyForQueue.CurrentCellDirtyStateChanged -= dataReadyForQueue_CurrentCellDirtyStateChanged;
            dataReadyForQueue.CellClick -= dataReadyForQueue_CellClick;

            isSelectingAll = true;

            try
            {
                // Commit any pending edits first
                if (dataReadyForQueue.IsCurrentCellDirty)
                {
                    dataReadyForQueue.CommitEdit(DataGridViewDataErrorContexts.Commit);
                }
                dataReadyForQueue.EndEdit();

                // Clear selection to avoid conflicts
                dataReadyForQueue.CurrentCell = null;

                // Select all checkboxes
                foreach (DataGridViewRow row in dataReadyForQueue.Rows)
                {
                    if (!row.IsNewRow && row.Cells["Select"] != null)
                    {
                        row.Cells["Select"].Value = true;
                    }
                }

                // Commit changes to data source
                dataReadyForQueue.EndEdit();

                // Refresh the grid
                dataReadyForQueue.Refresh();

                // Allow UI to update
                Application.DoEvents();

                // Update the count display
                UpdateSelectedCount();
            }
            finally
            {
                isSelectingAll = false;

                // Re-attach event handlers
                dataReadyForQueue.CellValueChanged += dataReadyForQueue_CellValueChanged;
                dataReadyForQueue.CurrentCellDirtyStateChanged += dataReadyForQueue_CurrentCellDirtyStateChanged;
                dataReadyForQueue.CellClick += dataReadyForQueue_CellClick;
            }
        }

        private void dataReadyForQueue_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            // Update count when checkbox value changes
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0 && 
                dataReadyForQueue.Columns[e.ColumnIndex].Name == "Select")
            {
                if (!isSelectingAll)
                {
                    UpdateSelectedCount();
                }
            }
        }

        private void dataReadyForQueue_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            // Handle checkbox clicks - commit the edit immediately
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0 && 
                dataReadyForQueue.Columns[e.ColumnIndex].Name == "Select")
            {
                dataReadyForQueue.CommitEdit(DataGridViewDataErrorContexts.Commit);
                // UpdateSelectedCount will be called by CellValueChanged
            }
        }

        private void dataReadyForQueue_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            // Commit checkbox edits immediately when the cell becomes dirty
            if (dataReadyForQueue.CurrentCell != null && 
                dataReadyForQueue.CurrentCell.OwningColumn.Name == "Select" &&
                dataReadyForQueue.IsCurrentCellDirty)
            {
                dataReadyForQueue.CommitEdit(DataGridViewDataErrorContexts.Commit);
            }
        }

        private void btnAddSelectedQueue_Click(object sender, EventArgs e)
        {
            try
            {
                // Commit any pending edits first
                if (dataReadyForQueue.IsCurrentCellDirty)
                {
                    dataReadyForQueue.CommitEdit(DataGridViewDataErrorContexts.Commit);
                }

                List<int> selectedAppointmentIDs = new List<int>();

                // Collect all selected appointment IDs
                foreach (DataGridViewRow row in dataReadyForQueue.Rows)
                {
                    if (!row.IsNewRow && row.Cells["Select"] != null && row.Cells["Select"].Value != null)
                    {
                        bool isChecked = false;
                        try
                        {
                            if (row.Cells["Select"].Value is bool)
                            {
                                isChecked = (bool)row.Cells["Select"].Value;
                            }
                            else
                            {
                                isChecked = Convert.ToBoolean(row.Cells["Select"].Value);
                            }
                        }
                        catch
                        {
                            isChecked = false;
                        }

                        if (isChecked)
                        {
                            if (row.Cells["AppointmentID"] != null && row.Cells["AppointmentID"].Value != null)
                            {
                                int appointmentId = Convert.ToInt32(row.Cells["AppointmentID"].Value);
                                selectedAppointmentIDs.Add(appointmentId);
                            }
                        }
                    }
                }

                if (selectedAppointmentIDs.Count == 0)
                {
                    MessageBox.Show("Please select at least one appointment to add to queue.", "No Selection",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                DialogResult result = MessageBox.Show(
                    $"Add {selectedAppointmentIDs.Count} appointment(s) to queue?",
                    "Confirm Add to Queue",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    using (SqlConnection connect = new SqlConnection(DB_Salon.connectionString))
                    {
                        connect.Open();
                        
                        int successCount = 0;
                        foreach (int appointmentId in selectedAppointmentIDs)
                        {
                            string updateQuery = "UPDATE Appointments SET Status = 'On Queue' WHERE AppointmentID = @AppointmentID AND Status = 'Confirmed'";
                            
                            using (SqlCommand cmd = new SqlCommand(updateQuery, connect))
                            {
                                cmd.Parameters.AddWithValue("@AppointmentID", appointmentId);
                                int rowsAffected = cmd.ExecuteNonQuery();
                                
                                if (rowsAffected > 0)
                                {
                                    successCount++;
                                }
                            }
                        }

                        if (successCount > 0)
                        {
                            MessageBox.Show($"{successCount} appointment(s) added to queue successfully.", "Success",
                                MessageBoxButtons.OK, MessageBoxIcon.Information);

                            // Refresh AdminManageQueue if it's open
                            foreach (Form form in Application.OpenForms)
                            {
                                if (form is AdminManageQueue)
                                {
                                    ((AdminManageQueue)form).LoadQueue("All");
                                    ((AdminManageQueue)form).RefreshDashboard();
                                    break;
                                }
                            }

                            // Refresh both AdminHomeDashboard and CashierHomeDashboard
                            RefreshDashboard();

                            // Reload the appointments list
                            LoadTodayAppointments();
                        }
                        else
                        {
                            MessageBox.Show("No appointments were added to queue. They may have already been processed.", "No Changes",
                                MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error adding appointments to queue: " + ex.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void RefreshDashboard()
        {
            // Refresh both AdminHomeDashboard and CashierHomeDashboard if they are open
            try
            {
                foreach (Form form in Application.OpenForms)
                {
                    if (form is AdminHomeDashboard)
                    {
                        AdminHomeDashboard adminDash = form as AdminHomeDashboard;
                        if (adminDash != null)
                        {
                            if (adminDash.InvokeRequired)
                            {
                                adminDash.Invoke(new Action(() => adminDash.RefreshCounts()));
                            }
                            else
                            {
                                adminDash.RefreshCounts();
                            }
                        }
                    }
                    else if (form is CashierHomeDashboard)
                    {
                        CashierHomeDashboard cashierDash = form as CashierHomeDashboard;
                        if (cashierDash != null)
                        {
                            if (cashierDash.InvokeRequired)
                            {
                                cashierDash.Invoke(new Action(() => cashierDash.RefreshCounts()));
                            }
                            else
                            {
                                cashierDash.RefreshCounts();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Error refreshing dashboard: " + ex.Message);
            }
        }

        private void pcClose_Click(object sender, EventArgs e)
        {
            // Refresh dashboard before closing
            RefreshDashboard();
            
            if (dateTimer != null)
            {
                dateTimer.Stop();
                dateTimer.Dispose();
            }
            this.Close();
        }

        private void ViewReadyForQueue_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Refresh dashboard when form is closing
            RefreshDashboard();
            
            if (dateTimer != null)
            {
                dateTimer.Stop();
                dateTimer.Dispose();
            }
        }
    }
}


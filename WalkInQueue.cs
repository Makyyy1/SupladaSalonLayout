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
    public partial class WalkInQueue : Form
    {
        private AdminManageQueue parentForm;
        private DateTime initialTime;
        private DateTime initialDate;
        private int currentUserID = -1;

        private class ServiceItem
        {
            public int ServiceID { get; set; }
            public string ServiceName { get; set; }
            public decimal ServicePrice { get; set; }
            public int CategoryID { get; set; }

            public override string ToString()
            {
                return $"{ServiceName} - ₱{ServicePrice:F2}";
            }
        }

        private DataTable selectedServicesTable;

        public WalkInQueue(AdminManageQueue parent, int userID)
        {
            InitializeComponent();
            parentForm = parent;
            currentUserID = userID;
        }

        private void WalkInQueue_Load(object sender, EventArgs e)
        {
            try
            {
                using (SqlConnection connect = new SqlConnection(DB_Salon.connectionString))
                {
                    connect.Open();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Database connection failed: " + ex.Message, "Connection Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            LoadCategories();
            SetupTimePicker();
            SetupDatePicker();
            InitializeSelectedServicesGrid();
            
            // Update date and time to current system time on load
            TimePicker.Value = DateTime.Now;
            datePicker.Value = DateTime.Today;
        }

        private void LoadCategories()
        {
            try
            {
                using (SqlConnection connect = new SqlConnection(DB_Salon.connectionString))
                {
                    connect.Open();
                    string query = "SELECT CategoryID, CategoryName FROM Categories ORDER BY CategoryName";

                    using (SqlCommand cmd = new SqlCommand(query, connect))
                    {
                        SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                        DataTable data = new DataTable();
                        adapter.Fill(data);

                        cbServiceType.DataSource = data;
                        cbServiceType.DisplayMember = "CategoryName";
                        cbServiceType.ValueMember = "CategoryID";
                        cbServiceType.SelectedIndex = -1;
                    }
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Failed to load categories.", "Load Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void cbServiceType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbServiceType.SelectedIndex != -1 && cbServiceType.SelectedValue != null)
            {
                if (cbServiceType.SelectedValue is int)
                {
                    LoadServicesByCategory((int)cbServiceType.SelectedValue);
                }
                else
                {
                    if (int.TryParse(cbServiceType.SelectedValue.ToString(), out int serviceTypeId))
                    {
                        LoadServicesByCategory(serviceTypeId);
                    }
                }
            }
        }

        private void LoadServicesByCategory(int categoryID)
        {
            try
            {
                using (SqlConnection connect = new SqlConnection(DB_Salon.connectionString))
                {
                    connect.Open();
                    string query = "SELECT ServiceID, ServiceName, ServicePrice, CategoryID FROM Services WHERE CategoryID = @CategoryID ORDER BY ServiceName";

                    using (SqlCommand cmd = new SqlCommand(query, connect))
                    {
                        cmd.Parameters.AddWithValue("@CategoryID", categoryID);

                        clServices.Items.Clear();
                        SqlDataReader reader = cmd.ExecuteReader();

                        while (reader.Read())
                        {
                            clServices.Items.Add(new ServiceItem
                            {
                                ServiceID = Convert.ToInt32(reader["ServiceID"]),
                                ServiceName = reader["ServiceName"].ToString(),
                                ServicePrice = Convert.ToDecimal(reader["ServicePrice"]),
                                CategoryID = Convert.ToInt32(reader["CategoryID"])
                            });
                        }
                    }
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Failed to load services.", "Load Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void SetupTimePicker()
        {
            TimePicker.Format = DateTimePickerFormat.Custom;
            TimePicker.CustomFormat = "h:mm tt";
            TimePicker.ShowUpDown = true;
            TimePicker.Enabled = false; // Disable time picker - use real-time only

            // Set to current system time
            TimePicker.Value = DateTime.Now;
            initialTime = TimePicker.Value;
        }

        private void SetupDatePicker()
        {
            datePicker.MinDate = DateTime.Today;
            datePicker.MaxDate = DateTime.Today; // Lock to today only
            datePicker.Value = DateTime.Today;
            datePicker.Enabled = false; // Disable date picker - use real-time only
            initialDate = DateTime.Today;
        }

        private void InitializeSelectedServicesGrid()
        {
            selectedServicesTable = new DataTable();
            selectedServicesTable.Columns.Add("ServiceID", typeof(int));
            selectedServicesTable.Columns.Add("ServiceName", typeof(string));
            selectedServicesTable.Columns.Add("ServicePrice", typeof(decimal));

            dataAddServices.DataSource = selectedServicesTable;

            dataAddServices.Columns["ServiceID"].Visible = false;

            dataAddServices.Columns["ServiceName"].HeaderText = "Service";
            dataAddServices.Columns["ServiceName"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dataAddServices.Columns["ServiceName"].FillWeight = 70;

            dataAddServices.Columns["ServicePrice"].HeaderText = "Price (₱)";
            dataAddServices.Columns["ServicePrice"].DefaultCellStyle.Format = "N2";
            dataAddServices.Columns["ServicePrice"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dataAddServices.Columns["ServicePrice"].FillWeight = 30;
            dataAddServices.Columns["ServicePrice"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            dataAddServices.AllowUserToAddRows = false;
            dataAddServices.AllowUserToDeleteRows = false;
            dataAddServices.ReadOnly = true;
            dataAddServices.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataAddServices.MultiSelect = false;
            dataAddServices.RowHeadersVisible = false;

            UpdateTotalPrice();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (clServices.CheckedItems.Count == 0)
            {
                MessageBox.Show("Please select at least one service to add.", "No Service Selected", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            foreach (var item in clServices.CheckedItems)
            {
                if (item is ServiceItem service)
                {
                    DataRow[] existingRows = selectedServicesTable.Select($"ServiceID = {service.ServiceID}");
                    if (existingRows.Length == 0)
                    {
                        selectedServicesTable.Rows.Add(service.ServiceID, service.ServiceName, service.ServicePrice);
                    }
                }
            }

            UpdateTotalPrice();
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            if (dataAddServices.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select a service to remove.", "No Service Selected", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            foreach (DataGridViewRow row in dataAddServices.SelectedRows)
            {
                if (!row.IsNewRow)
                {
                    dataAddServices.Rows.Remove(row);
                }
            }

            UpdateTotalPrice();
        }

        private void UpdateTotalPrice()
        {
            decimal total = 0;
            foreach (DataRow row in selectedServicesTable.Rows)
            {
                total += Convert.ToDecimal(row["ServicePrice"]);
            }
            lblTotalServices.Text = $"Total: ₱{total:N2}";
        }

        private bool ValidateInputs()
        {
            if (cbServiceType.SelectedIndex == -1)
            {
                MessageBox.Show("Please select a category.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cbServiceType.Focus();
                return false;
            }

            if (selectedServicesTable.Rows.Count == 0)
            {
                MessageBox.Show("Please add at least one service.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

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

            // Contact is optional, so we don't validate it

            // Date and time are automatically set to current system date/time
            // No validation needed as they are read-only

            return true;
        }

        private void btnAddToQueue_Click(object sender, EventArgs e)
        {
            if (!ValidateInputs())
            {
                return;
            }

            try
            {
                using (SqlConnection connect = new SqlConnection(DB_Salon.connectionString))
                {
                    connect.Open();

                    // Concatenate all selected services into a single string
                    List<string> serviceNames = new List<string>();
                    foreach (DataRow row in selectedServicesTable.Rows)
                    {
                        serviceNames.Add(row["ServiceName"].ToString());
                    }
                    string concatenatedServices = string.Join(", ", serviceNames);

                    // Get the first service ID for the ServiceID field
                    int firstServiceID = Convert.ToInt32(selectedServicesTable.Rows[0]["ServiceID"]);

                    // Insert appointment with Status = 'On Queue' (directly to queue)
                    string query = @"INSERT INTO Appointments 
                        (CustomerFirstName, CustomerLastName, CustomerContact, ServiceID, [Service Name], 
                         AppointmentDate, AppointmentTime, Status, UserID) 
                        VALUES 
                        (@CustomerFirstName, @CustomerLastName, @Contact, 
                         @ServiceID, @ServiceName, @AppointmentDate, 
                         @AppointmentTime, @Status, @UserID)";

                    using (SqlCommand cmd = new SqlCommand(query, connect))
                    {
                        cmd.Parameters.AddWithValue("@CustomerFirstName", txtCustomerFirstName.Text.Trim());
                        cmd.Parameters.AddWithValue("@CustomerLastName", txtCustomerLastName.Text.Trim());
                        cmd.Parameters.AddWithValue("@Contact", string.IsNullOrWhiteSpace(txtCustomerContact.Text) ? (object)DBNull.Value : txtCustomerContact.Text.Trim());
                        cmd.Parameters.AddWithValue("@ServiceID", firstServiceID);
                        cmd.Parameters.AddWithValue("@ServiceName", concatenatedServices);
                        // Use current system date and time for walk-in customers
                        DateTime currentDateTime = DateTime.Now;
                        cmd.Parameters.AddWithValue("@AppointmentDate", currentDateTime.Date);
                        cmd.Parameters.AddWithValue("@AppointmentTime", currentDateTime.TimeOfDay);
                        cmd.Parameters.AddWithValue("@Status", "On Queue");
                        cmd.Parameters.AddWithValue("@UserID", currentUserID > 0 ? currentUserID : (object)DBNull.Value);

                        cmd.ExecuteNonQuery();
                    }
                }

                MessageBox.Show("Walk-in customer added to queue successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // Refresh the parent form's queue
                if (parentForm != null)
                {
                    parentForm.LoadQueue();
                    parentForm.RefreshDashboard();
                }

                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Unable to add customer to queue. Please try again.\n\nError: " + ex.Message,
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void txtCustomerContact_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void TimePicker_ValueChanged(object sender, EventArgs e)
        {
            // Time picker is disabled - no action needed
            // Time is always set to current system time
        }

        private void datePicker_ValueChanged(object sender, EventArgs e)
        {
            // Date picker is disabled - no action needed
            // Date is always set to current system date
        }
    }
}

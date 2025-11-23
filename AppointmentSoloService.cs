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
    public partial class AppointmentSoloService : Form
    {

        private AdminCreateAppointment parentForm;
        private DateTime initialTime;
        private DateTime initialDate;

        public AppointmentSoloService(AdminCreateAppointment parent)
        {
            InitializeComponent();
            parentForm = parent;
            datePicker.ValueChanged += datePicker_ValueChanged;
            
            // Add cell formatting event for time column
            dataSchedule.CellFormatting += dataSchedule_CellFormatting;
        }

        private void AppointmentSoloService_Load(object sender, EventArgs e)
        {
            // Test database connection first
            try
            {
                using (SqlConnection connect = new SqlConnection(DB_Salon.connectionString))
                {
                    connect.Open();
                    // Connection successful
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
            
            // Set dateOccupiedSched to current date
            dateOccupiedSched.Value = DateTime.Today;
            
            LoadOccupiedSchedule();
        }

        private void RefreshDashboard()
        {
            // Find the open AdminHomeDashboard or CashierHomeDashboard and refresh it
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

            DateTime now = DateTime.Now;
            int currentHour = now.Hour;

            // If current time is before 9 AM, set to 9:00 AM
            if (currentHour < 9)
            {
                TimePicker.Value = DateTime.Today.AddHours(9);
            }
            // If current time is after 7 PM, set to 9:00 AM next day
            else if (currentHour >= 19)
            {
                TimePicker.Value = DateTime.Today.AddDays(1).AddHours(9);
            }
            // Otherwise use current time
            else
            {
                TimePicker.Value = now;
            }

            initialTime = TimePicker.Value;
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

        private void LoadOccupiedSchedule()
        {
            try
            {
                using (SqlConnection connect = new SqlConnection(DB_Salon.connectionString))
                {
                    connect.Open();
                    
                    // Check if Service Name column exists and use appropriate query
                    string query;
                    bool hasServiceNameColumn = HasServiceNameColumn();
                    
                    if (hasServiceNameColumn)
                    {
                        query = @"SELECT AppointmentID, CustomerFirstName + ' ' + CustomerLastName as CustomerName, 
                                       AppointmentDate, 
                                       CAST(AppointmentTime AS TIME) as AppointmentTime, [Service Name] as AvailedServices, Status 
                                       FROM Appointments 
                                       WHERE Status = 'Confirmed' 
                                       AND CONVERT(DATE, AppointmentDate) = CONVERT(DATE, @CurrentDate)
                                       ORDER BY AppointmentTime";
                    }
                    else
                    {
                        // Fallback to Service1 if Service Name doesn't exist
                        query = @"SELECT AppointmentID, CustomerFirstName + ' ' + CustomerLastName as CustomerName, 
                                       AppointmentDate, 
                                       CAST(AppointmentTime AS TIME) as AppointmentTime, Service1 as AvailedServices, Status 
                                       FROM Appointments 
                                       WHERE Status = 'Confirmed' 
                                       AND CONVERT(DATE, AppointmentDate) = CONVERT(DATE, @CurrentDate)
                                       ORDER BY AppointmentTime";
                    }

                    using (SqlCommand cmd = new SqlCommand(query, connect))
                    {
                        cmd.Parameters.AddWithValue("@CurrentDate", dateOccupiedSched.Value.Date);
                        
                        SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                        DataTable data = new DataTable();
                        adapter.Fill(data);
                        dataSchedule.DataSource = data;

                        dataSchedule.Columns["AppointmentID"].Visible = false;

                        dataSchedule.Columns["CustomerName"].HeaderText = "Customer";
                        dataSchedule.Columns["CustomerName"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                        dataSchedule.Columns["CustomerName"].FillWeight = 25;
                        dataSchedule.Columns["CustomerName"].Width = 150;

                        dataSchedule.Columns["AppointmentDate"].HeaderText = "Date";
                        dataSchedule.Columns["AppointmentDate"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                        dataSchedule.Columns["AppointmentDate"].FillWeight = 15;
                        dataSchedule.Columns["AppointmentDate"].Width = 100;
                        dataSchedule.Columns["AppointmentDate"].DefaultCellStyle.Format = "MM/dd/yyyy";
                        dataSchedule.Columns["AppointmentDate"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;


                        dataSchedule.Columns["AppointmentTime"].HeaderText = "Time";
                        dataSchedule.Columns["AppointmentTime"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                        dataSchedule.Columns["AppointmentTime"].FillWeight = 15;
                        dataSchedule.Columns["AppointmentTime"].Width = 100;
                        dataSchedule.Columns["AppointmentTime"].DefaultCellStyle.Format = "t";
                        dataSchedule.Columns["AppointmentTime"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

                        // Format time column to 12-hour format
                        if (dataSchedule.Columns["AppointmentTime"] != null)
                        {
                            dataSchedule.Columns["AppointmentTime"].DefaultCellStyle.Format = "t";
                        }
                        
                        // Handle column name based on what's available
                        if (data.Columns.Contains("AvailedServices"))
                        {
                            dataSchedule.Columns["AvailedServices"].HeaderText = "Services";
                            dataSchedule.Columns["AvailedServices"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                            dataSchedule.Columns["AvailedServices"].FillWeight = 30;
                            dataSchedule.Columns["AvailedServices"].Width = 200;
                        }
                        
                        dataSchedule.Columns["Status"].HeaderText = "Status";
                        dataSchedule.Columns["Status"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                        dataSchedule.Columns["Status"].FillWeight = 15;
                        dataSchedule.Columns["Status"].Width = 100;
                        dataSchedule.Columns["Status"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

                        dataSchedule.AllowUserToAddRows = false;
                        dataSchedule.AllowUserToDeleteRows = false;
                        dataSchedule.ReadOnly = true;
                        dataSchedule.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                        dataSchedule.MultiSelect = false;
                        dataSchedule.RowHeadersVisible = false;

                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to load occupied schedule.\n\nError: " + ex.Message, "Load Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
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
                    // Check if service already exists in the grid
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

        private void dateOccupiedSched_ValueChanged(object sender, EventArgs e)
        {
            FilterScheduleByDate();
        }

        private bool HasServiceNameColumn()
        {
            // For now, assume Service Name column exists since we updated the database
            // This avoids multiple database connections that might cause locking issues
            return true;
        }

        private void FilterScheduleByDate()
        {
            try
            {
                using (SqlConnection connect = new SqlConnection(DB_Salon.connectionString))
                {
                    connect.Open();
                    
                    // Check if Service Name column exists and use appropriate query
                    string query;
                    bool hasServiceNameColumn = HasServiceNameColumn();
                    
                    if (hasServiceNameColumn)
                    {
                        query = @"SELECT AppointmentID, CustomerFirstName + ' ' + CustomerLastName as CustomerName, 
                                       AppointmentDate, 
                                       CAST(AppointmentTime AS TIME) as AppointmentTime, [Service Name] as AvailedServices, Status 
                                       FROM Appointments 
                                       WHERE Status = 'Confirmed' 
                                       AND CONVERT(DATE, AppointmentDate) = CONVERT(DATE, @AppointmentDate)
                                       ORDER BY AppointmentTime";
                    }
                    else
                    {
                        // Fallback to Service1 if Service Name doesn't exist
                        query = @"SELECT AppointmentID, CustomerFirstName + ' ' + CustomerLastName as CustomerName, 
                                       AppointmentDate, 
                                       CAST(AppointmentTime AS TIME) as AppointmentTime, Service1 as AvailedServices, Status 
                                       FROM Appointments 
                                       WHERE Status = 'Confirmed' 
                                       AND CONVERT(DATE, AppointmentDate) = CONVERT(DATE, @AppointmentDate)
                                       ORDER BY AppointmentTime";
                    }

                    using (SqlCommand cmd = new SqlCommand(query, connect))
                    {
                        cmd.Parameters.AddWithValue("@AppointmentDate", dateOccupiedSched.Value.Date);
                        
                        SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                        DataTable data = new DataTable();
                        adapter.Fill(data);

                        dataSchedule.DataSource = data;
                        dataSchedule.Columns["AppointmentID"].Visible = false;
                        dataSchedule.Columns["CustomerName"].HeaderText = "Customer";
                        dataSchedule.Columns["AppointmentDate"].HeaderText = "Date";
                        dataSchedule.Columns["AppointmentTime"].HeaderText = "Time";
                        
                        // Format time column to 12-hour format
                        if (dataSchedule.Columns["AppointmentTime"] != null)
                        {
                            dataSchedule.Columns["AppointmentTime"].DefaultCellStyle.Format = "t";
                        }
                        
                        // Handle column name based on what's available
                        if (data.Columns.Contains("AvailedServices"))
                        {
                            dataSchedule.Columns["AvailedServices"].HeaderText = "Services";
                        }
                        
                        dataSchedule.Columns["Status"].HeaderText = "Status";
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to filter schedule.\n\nError: " + ex.Message, "Filter Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private bool HasAppointmentConflict()
        {
            try
            {
                using (SqlConnection connect = new SqlConnection(DB_Salon.connectionString))
                {
                    connect.Open();
                    string query = @"SELECT COUNT(*) FROM Appointments 
                                   WHERE Status NOT IN ('Cancelled') 
                                   AND AppointmentDate = @AppointmentDate 
                                   AND CONVERT(varchar(8), AppointmentTime, 108) = CONVERT(varchar(8), @AppointmentTime, 108)";

                    using (SqlCommand cmd = new SqlCommand(query, connect))
                    {
                        cmd.Parameters.AddWithValue("@AppointmentDate", datePicker.Value.Date);
                        cmd.Parameters.AddWithValue("@AppointmentTime", TimePicker.Value.TimeOfDay);

                        int count = Convert.ToInt32(cmd.ExecuteScalar());
                        return count > 0;
                    }
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        private void SetupDatePicker()
        {
            datePicker.MinDate = DateTime.Today;
            datePicker.Value = DateTime.Today;
            initialDate = DateTime.Today;
        }

        private void TimePicker_ValueChanged(object sender, EventArgs e)
        {
            DateTime selectedTime = TimePicker.Value;
            DateTime selectedDate = datePicker.Value.Date;
            DateTime now = DateTime.Now;
            int hour = selectedTime.Hour;

            bool isToday = selectedDate == DateTime.Today;

            // Business hours validation (9 AM to 7 PM)
            if (hour < 9)
            {
                TimePicker.Value = new DateTime(
                    selectedTime.Year,
                    selectedTime.Month,
                    selectedTime.Day,
                    9, 0, 0);
                MessageBox.Show("Appointment time must be between 9:00 AM and 7:00 PM.",
                    "Invalid Time", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            else if (hour >= 19)
            {
                TimePicker.Value = new DateTime(
                    selectedTime.Year,
                    selectedTime.Month,
                    selectedTime.Day,
                    19, 0, 0);
                MessageBox.Show("Appointment time must be between 9:00 AM and 7:00 PM.",
                    "Invalid Time", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // If today, prevent selecting past times
            if (isToday)
            {
                TimeSpan selectedTimeOfDay = selectedTime.TimeOfDay;
                TimeSpan currentTimeOfDay = now.TimeOfDay;

                if (selectedTimeOfDay < currentTimeOfDay)
                {
                    // Set to current time rounded up to next 15-minute interval
                    int currentMinute = now.Minute;
                    int roundedMinute = ((currentMinute + 14) / 15) * 15;

                    DateTime validTime;
                    if (roundedMinute >= 60)
                    {
                        validTime = now.AddHours(1).AddMinutes(-now.Minute);
                    }
                    else
                    {
                        validTime = new DateTime(now.Year, now.Month, now.Day, now.Hour, roundedMinute, 0);
                    }

                    // Check if rounded time is still within business hours
                    if (validTime.Hour >= 19)
                    {
                        MessageBox.Show("No more appointments available for today. Please select a future date.",
                            "Invalid Time", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        datePicker.Value = DateTime.Today.AddDays(1);
                        TimePicker.Value = DateTime.Today.AddDays(1).AddHours(9);
                        return;
                    }

                    TimePicker.Value = validTime;
                    MessageBox.Show("Cannot select a past time for today's appointment. Time has been adjusted to the nearest available slot.",
                        "Past Time Selected", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
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

            if (string.IsNullOrWhiteSpace(txtCustomerContact.Text))
            {
                MessageBox.Show("Contact number is required.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtCustomerContact.Focus();
                return false;
            }

            if (txtCustomerContact.Text.Length != 11)
            {
                MessageBox.Show("Contact number must be exactly 11 digits.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtCustomerContact.Focus();
                return false;
            }

            int hour = TimePicker.Value.Hour;
            if (hour < 9 || hour >= 19)
            {
                MessageBox.Show("Appointment time must be between 9:00 AM and 7:00 PM.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                TimePicker.Focus();
                return false;
            }

            if (datePicker.Value.Date < DateTime.Today)
            {
                MessageBox.Show("Appointment date cannot be in the past.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                datePicker.Focus();
                return false;
            }

            DateTime appointmentDateTime = datePicker.Value.Date.Add(TimePicker.Value.TimeOfDay);
            if (appointmentDateTime < DateTime.Now)
            {
                MessageBox.Show("Appointment time cannot be in the past.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                TimePicker.Focus();
                return false;
            }

            if (HasAppointmentConflict())
            {
                MessageBox.Show("This appointment slot is already occupied. Please choose a different date or time.", "Schedule Conflict", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            return true;
        }


        private void btnCreateAppointment_Click(object sender, EventArgs e)
        {
            if (!ValidateInputs())
            {
                return;
            }

            // Check for appointment conflicts
            if (HasAppointmentConflict())
            {
                MessageBox.Show("The selected time slot is already occupied. Please choose a different time.", "Time Conflict", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                using (SqlConnection connect = new SqlConnection(DB_Salon.connectionString))
                {
                    connect.Open();

                    int userID = parentForm != null ? parentForm.GetCurrentUserID() : -1;
                    if (userID == -1)
                    {
                        MessageBox.Show("Unable to determine user. Please try again.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    // Concatenate all selected services into a single string
                    List<string> serviceNames = new List<string>();
                    foreach (DataRow row in selectedServicesTable.Rows)
                    {
                        serviceNames.Add(row["ServiceName"].ToString());
                    }
                    string concatenatedServices = string.Join(", ", serviceNames);

                    // Get the first service ID for the ServiceID field (as required by database)
                    int firstServiceID = Convert.ToInt32(selectedServicesTable.Rows[0]["ServiceID"]);

                    // Check if Service Name column exists and use appropriate query
                    string query;
                    bool hasServiceNameColumn = HasServiceNameColumn();
                    
                    if (hasServiceNameColumn)
                    {
                        query = @"INSERT INTO Appointments 
                        (CustomerFirstName, CustomerLastName, CustomerContact, ServiceID, [Service Name], 
                         AppointmentDate, AppointmentTime, Status, UserID) 
                        VALUES 
                        (@CustomerFirstName, @CustomerLastName, @Contact, 
                         @ServiceID, @ServiceName, @AppointmentDate, 
                         @AppointmentTime, @Status, @UserID)";
                    }
                    else
                    {
                        // Fallback to Service1 if Service Name doesn't exist
                        query = @"INSERT INTO Appointments 
                        (CustomerFirstName, CustomerLastName, CustomerContact, ServiceID, Service1, 
                         AppointmentDate, AppointmentTime, Status, UserID) 
                        VALUES 
                        (@CustomerFirstName, @CustomerLastName, @Contact, 
                         @ServiceID, @Service1, @AppointmentDate, 
                         @AppointmentTime, @Status, @UserID)";
                    }

                    using (SqlCommand cmd = new SqlCommand(query, connect))
                    {
                        cmd.Parameters.AddWithValue("@CustomerFirstName", txtCustomerFirstName.Text.Trim());
                        cmd.Parameters.AddWithValue("@CustomerLastName", txtCustomerLastName.Text.Trim());
                        cmd.Parameters.AddWithValue("@Contact", txtCustomerContact.Text.Trim());
                        cmd.Parameters.AddWithValue("@ServiceID", firstServiceID);
                        
                        // Use appropriate parameter name based on query
                        if (hasServiceNameColumn)
                        {
                            cmd.Parameters.AddWithValue("@ServiceName", concatenatedServices);
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue("@Service1", concatenatedServices);
                        }
                        cmd.Parameters.AddWithValue("@AppointmentDate", datePicker.Value.Date);
                        cmd.Parameters.AddWithValue("@AppointmentTime", TimePicker.Value.TimeOfDay);
                        cmd.Parameters.AddWithValue("@Status", "Confirmed");
                        cmd.Parameters.AddWithValue("@UserID", userID);

                        cmd.ExecuteNonQuery();
                    }
                }

                MessageBox.Show("Appointment created successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                parentForm?.LoadAppointments();
                RefreshDashboard();
                LoadOccupiedSchedule();

                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Unable to create appointment. Please try again.\n\nError: " + ex.Message,
                    "Create Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void txtCustomerContact_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private bool HasAppointmentChanges()
        {
            bool categorySelected = cbServiceType.SelectedIndex != -1;

            bool serviceSelected = selectedServicesTable.Rows.Count > 0;

            bool firstNameEntered = !string.IsNullOrWhiteSpace(txtCustomerFirstName.Text);

            bool lastNameEntered = !string.IsNullOrWhiteSpace(txtCustomerLastName.Text);

            bool contactEntered = !string.IsNullOrWhiteSpace(txtCustomerContact.Text);

            bool dateChanged = datePicker.Value.Date != initialDate.Date;

            bool timeChanged = TimePicker.Value.TimeOfDay != initialTime.TimeOfDay;

            return categorySelected || serviceSelected || firstNameEntered ||
                   lastNameEntered || contactEntered || dateChanged || timeChanged;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            bool hasChanges = HasAppointmentChanges();
            if (hasChanges)
            {
                DialogResult result = MessageBox.Show(
                    "You have unsaved changes. Are you sure you want to cancel?", "Confirm Cancel", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    this.Close();
                }
            }
            else
            {
                this.Close();
            }
        }

        private void datePicker_ValueChanged(object sender, EventArgs e)
        {
            DateTime selectedDate = datePicker.Value.Date;
            DateTime now = DateTime.Now;

            // If today is selected, ensure time is not in the past
            if (selectedDate == DateTime.Today)
            {
                TimeSpan currentTime = now.TimeOfDay;
                TimeSpan selectedTime = TimePicker.Value.TimeOfDay;

                // If selected time is in the past, update to current time
                if (selectedTime < currentTime)
                {
                    if (now.Hour >= 19)
                    {
                        MessageBox.Show("No more appointments available for today. Selecting tomorrow.",
                            "Too Late", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        datePicker.Value = DateTime.Today.AddDays(1);
                        TimePicker.Value = DateTime.Today.AddDays(1).AddHours(9);
                    }
                    else if (now.Hour < 9)
                    {
                        TimePicker.Value = DateTime.Today.AddHours(9);
                    }
                    else
                    {
                        TimePicker.Value = now;
                    }
                }
            }
            else if (selectedDate > DateTime.Today)
            {
                // Future date selected, allow any time from 9 AM
                if (TimePicker.Value.Hour < 9)
                {
                    TimePicker.Value = selectedDate.AddHours(9);
                }
            }
        }

        private void dataSchedule_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            // Format the AppointmentTime column
            if (e.ColumnIndex == dataSchedule.Columns["AppointmentTime"].Index && e.Value != null)
            {
                if (e.Value is DateTime timeValue)
                {
                    e.Value = timeValue.ToString("h:mm tt");
                    e.FormattingApplied = true;
                }
                else if (DateTime.TryParse(e.Value.ToString(), out DateTime parsedTime))
                {
                    e.Value = parsedTime.ToString("h:mm tt");
                    e.FormattingApplied = true;
                }
            }
        }
    }
}

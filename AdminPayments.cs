using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextFont = iTextSharp.text.Font;
using iTextBaseColor = iTextSharp.text.BaseColor;
using Microsoft.Win32;

namespace SupladaSalonLayout
{
    public partial class AdminPayments : Form
    {
        private int currentAppointmentId;
        private int selectedAppointmentID = 0;
        private decimal service1Price = 0;
        private decimal service2Price = 0;
        private decimal productPrice = 0;
        private decimal discountAmount = 0;
        private int selectedPaymentModeID = 0;
        private int currentUserID = -1;
        private DataTable servicesTable;
        private DataTable productsTable;
        private string currentUsername = "Unknown";
        private string currentRole = "Unknown";

        public AdminPayments()
        {
            InitializeComponent();
            datePicker.MinDate = DateTime.Today;
            datePicker.Value = DateTime.Today;
            TimePicker.Value = DateTime.Now;
            InitializeDataGridViews();
            WireUpEventHandlers();
            LoadCategories();
            LoadProducts();
            LoadDiscounts();
            LoadPaymentModes();
            LoadTechnicians();
            LoadCurrentUserDetails();
        }

        public AdminPayments(int userID, int appointmentId = 0)
        {
            InitializeComponent();
            currentUserID = userID;
            selectedAppointmentID = appointmentId;
            
            // Set datePicker MinDate to prevent selecting past dates
            datePicker.MinDate = DateTime.Today;
            
            // Set datePicker and TimePicker to current system date/time if no appointment is being loaded
            if (appointmentId == 0)
            {
                datePicker.Value = DateTime.Today;
                TimePicker.Value = DateTime.Now;
            }
            
            InitializeDataGridViews();
            WireUpEventHandlers();
            LoadCategories();
            LoadProducts();
            LoadDiscounts();
            LoadPaymentModes();
            LoadTechnicians();
            LoadCurrentUserDetails();
            
            if (appointmentId > 0)
            {
                LoadAppointmentDetails(appointmentId);
            }
        }

        private void WireUpEventHandlers()
        {
            btnAddProduct.Click += btnAddProduct_Click;
            btnAddServices.Click += btnAddServices_Click;
            btnProceed.Click += btnProceed_Click;
            btnCancelPayment.Click += btnCancelPayment_Click;
            btnRemoveService.Click += btnRemoveService_Click;
            btnRemoveProducts.Click += btnRemoveProducts_Click;
            cbServiceCategory.SelectedIndexChanged += cbServiceCategory_SelectedIndexChanged;
            cbDiscounts.SelectedIndexChanged += cbDiscounts_SelectedIndexChanged;
            cbPayment.SelectedIndexChanged += cbPayment_SelectedIndexChanged;
            dataAvailedProducts.CellValueChanged += dataAvailedProducts_CellValueChanged;
            txtDiscountPrice.TextChanged += txtDiscountPrice_TextChanged;
            txtDiscountPrice.KeyPress += txtDiscountPrice_KeyPress;
            txtDiscountPrice.Leave += txtDiscountPrice_Leave;
        }

        private void LoadCurrentUserDetails()
        {
            if (currentUserID <= 0)
            {
                return;
            }

            try
            {
                using (SqlConnection conn = new SqlConnection(DB_Salon.connectionString))
                {
                    conn.Open();
                    string query = "SELECT Username, Role FROM Users WHERE UserID = @UserID";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@UserID", currentUserID);
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                currentUsername = reader["Username"]?.ToString() ?? currentUsername;
                                currentRole = reader["Role"]?.ToString() ?? currentRole;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading user details: " + ex.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void InitializeDataGridViews()
        {
            // Initialize Services DataGridView
            servicesTable = new DataTable();
            servicesTable.Columns.Add("ServiceName", typeof(string));
            servicesTable.Columns.Add("ServicePrice", typeof(decimal));
            dataAvailedServices.DataSource = servicesTable;
            dataAvailedServices.Columns["ServiceName"].HeaderText = "Service Name";
            dataAvailedServices.Columns["ServiceName"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dataAvailedServices.Columns["ServicePrice"].HeaderText = "Price";
            dataAvailedServices.Columns["ServicePrice"].DefaultCellStyle.Format = "N2";
            dataAvailedServices.Columns["ServicePrice"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dataAvailedServices.AllowUserToAddRows = false;
            dataAvailedServices.ReadOnly = true;

            // Initialize Products DataGridView
            productsTable = new DataTable();
            productsTable.Columns.Add("ProductName", typeof(string));
            productsTable.Columns.Add("ProductPrice", typeof(decimal));
            productsTable.Columns.Add("Quantity", typeof(int));
            dataAvailedProducts.DataSource = productsTable;
            dataAvailedProducts.Columns["ProductName"].HeaderText = "Product Name";
            dataAvailedProducts.Columns["ProductName"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dataAvailedProducts.Columns["ProductPrice"].HeaderText = "Unit Price";
            dataAvailedProducts.Columns["ProductPrice"].DefaultCellStyle.Format = "N2";
            dataAvailedProducts.Columns["ProductPrice"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dataAvailedProducts.Columns["ProductPrice"].ReadOnly = true;
            dataAvailedProducts.Columns["Quantity"].HeaderText = "Quantity";
            dataAvailedProducts.Columns["Quantity"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataAvailedProducts.Columns["Quantity"].ReadOnly = false;
            dataAvailedProducts.AllowUserToAddRows = false;
        }

        private void LoadPaymentModes()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(DB_Salon.connectionString))
                {
                    conn.Open();
                    string query = "SELECT PaymentModeID, PaymentModeName FROM PaymentModes"; 

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            DataTable dt = new DataTable();
                            dt.Load(reader);

                            DataRow emptyRow = dt.NewRow();
                            emptyRow["PaymentModeID"] = 0;
                            emptyRow["PaymentModeName"] = "-- Select Payment Mode --";
                            dt.Rows.InsertAt(emptyRow, 0);

                            cbPayment.DisplayMember = "PaymentModeName";
                            cbPayment.ValueMember = "PaymentModeID";
                            cbPayment.DataSource = dt;
                            cbPayment.Tag = dt; // Store DataTable to access PaymentModeName
                            
                            // Initialize Reference # as disabled
                            txtReferenceNumber.Enabled = false;
                            txtReferenceNumber.Clear();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading payment modes: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadTechnicians()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(DB_Salon.connectionString))
                {
                    conn.Open();
                    string query = "SELECT TechnicianID, TechnicianName FROM Technicians ORDER BY TechnicianName";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            DataTable dt = new DataTable();
                            dt.Load(reader);

                            DataRow emptyRow = dt.NewRow();
                            emptyRow["TechnicianID"] = 0;
                            emptyRow["TechnicianName"] = "-- Select Technician --";
                            dt.Rows.InsertAt(emptyRow, 0);

                            cbTechnicians.DisplayMember = "TechnicianName";
                            cbTechnicians.ValueMember = "TechnicianID";
                            cbTechnicians.DataSource = dt;
                            cbTechnicians.SelectedIndex = 0;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading technicians: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void pcClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void LoadAppointmentDetails(int appointmentId)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(DB_Salon.connectionString))
                {
                    conn.Open();
                    string query = @"SELECT 
                                    a.CustomerFirstName,
                                    a.CustomerLastName,
                                    a.CustomerContact,
                                    a.AppointmentDate,
                                    a.AppointmentTime,
                                    a.[Service Name] as Services,
                                    a.Status,
                                    a.TechnicianID
                                FROM Appointments a
                                WHERE a.AppointmentID = @AppointmentID";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@AppointmentID", appointmentId);
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                // Update textboxes with customer information
                                txtCustomerFirstName.Text = reader["CustomerFirstName"].ToString();
                                txtCustomerLastName.Text = reader["CustomerLastName"].ToString();
                                txtContactNumber.Text = reader["CustomerContact"].ToString();
                                
                                // Set appointment date and time
                                if (DateTime.TryParse(reader["AppointmentDate"].ToString(), out DateTime apptDate))
                                {
                                    // If appointment date is in the past, set to today; otherwise use appointment date
                                    if (apptDate < DateTime.Today)
                                    {
                                        datePicker.Value = DateTime.Today;
                                    }
                                    else
                                {
                                    datePicker.Value = apptDate;
                                    }
                                }
                                
                                // Handle appointment time
                                object timeValue = reader["AppointmentTime"];
                                if (timeValue != DBNull.Value)
                                {
                                    if (timeValue is TimeSpan ts)
                                    {
                                        TimePicker.Value = DateTime.Today.Add(ts);
                                    }
                                    else if (TimeSpan.TryParse(timeValue.ToString(), out TimeSpan parsedTime))
                                    {
                                        TimePicker.Value = DateTime.Today.Add(parsedTime);
                                    }
                                }
                                
                                // Load services into DataGridView
                                string services = reader["Services"].ToString();
                                if (!string.IsNullOrEmpty(services))
                                {
                                    LoadServicesToGrid(services);
                                }

                                // Load technician if assigned
                                if (reader["TechnicianID"] != DBNull.Value)
                                {
                                    int technicianID = Convert.ToInt32(reader["TechnicianID"]);
                                    // Set the selected technician in the combobox
                                    foreach (object item in cbTechnicians.Items)
                                    {
                                        if (cbTechnicians.Items.IndexOf(item) > 0) // Skip the first empty item
                                        {
                                            DataRowView rowView = item as DataRowView;
                                            if (rowView != null && Convert.ToInt32(rowView["TechnicianID"]) == technicianID)
                                            {
                                                cbTechnicians.SelectedItem = item;
                                                break;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading appointment details: " + ex.Message, 
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadServicesToGrid(string servicesString)
        {
            try
            {
                servicesTable.Rows.Clear();
                
                if (string.IsNullOrWhiteSpace(servicesString))
                    return;

                // Split services by comma
                string[] serviceNames = servicesString.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                
                using (SqlConnection conn = new SqlConnection(DB_Salon.connectionString))
                {
                    conn.Open();
                    
                    foreach (string serviceName in serviceNames)
                    {
                        string trimmedServiceName = serviceName.Trim();
                        if (string.IsNullOrWhiteSpace(trimmedServiceName))
                            continue;

                        // Get service price from Services table
                        string priceQuery = "SELECT ServicePrice FROM Services WHERE ServiceName = @ServiceName";
                        using (SqlCommand priceCmd = new SqlCommand(priceQuery, conn))
                        {
                            priceCmd.Parameters.AddWithValue("@ServiceName", trimmedServiceName);
                            object priceResult = priceCmd.ExecuteScalar();
                            
                            decimal servicePrice = 0;
                            if (priceResult != null && priceResult != DBNull.Value)
                            {
                                servicePrice = Convert.ToDecimal(priceResult);
                            }

                            // Add to DataTable
                            servicesTable.Rows.Add(trimmedServiceName, servicePrice);
                        }
                    }
                }
                
                CalculateTotals();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading services: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadCategories()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(DB_Salon.connectionString))
                {
                    conn.Open();
                    string query = "SELECT CategoryID, CategoryName FROM Categories ORDER BY CategoryName";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            DataTable dt = new DataTable();
                            dt.Load(reader);

                            // Add empty row at the beginning
                            DataRow emptyRow = dt.NewRow();
                            emptyRow["CategoryID"] = 0;
                            emptyRow["CategoryName"] = "-- Select Category --";
                            dt.Rows.InsertAt(emptyRow, 0);

                            cbServiceCategory.DisplayMember = "CategoryName";
                            cbServiceCategory.ValueMember = "CategoryID";
                            cbServiceCategory.DataSource = dt;
                            cbServiceCategory.SelectedIndex = 0;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading categories: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadServicesByCategory(int categoryID)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(DB_Salon.connectionString))
                {
                    conn.Open();
                    string query = "SELECT ServiceID, ServiceName, ServicePrice FROM Services WHERE CategoryID = @CategoryID ORDER BY ServiceName";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@CategoryID", categoryID);
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            DataTable dt = new DataTable();
                            dt.Load(reader);

                            // Add empty row at the beginning
                            DataRow emptyRow = dt.NewRow();
                            emptyRow["ServiceID"] = 0;
                            emptyRow["ServiceName"] = "-- Select Service --";
                            emptyRow["ServicePrice"] = 0;
                            dt.Rows.InsertAt(emptyRow, 0);

                            cbServices.DisplayMember = "ServiceName";
                            cbServices.ValueMember = "ServiceID";
                            cbServices.DataSource = dt;
                            cbServices.Tag = dt; // Store DataTable to access ServicePrice
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading services: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void cbServiceCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbServiceCategory.SelectedValue != null && cbServiceCategory.SelectedIndex > 0)
            {
                int categoryID = Convert.ToInt32(cbServiceCategory.SelectedValue);
                LoadServicesByCategory(categoryID);
            }
            else
            {
                // Clear services combobox
                cbServices.DataSource = null;
                cbServices.Items.Clear();
            }
        }

        private void btnAddServices_Click(object sender, EventArgs e)
        {
            try
            {
                if (cbServices.SelectedIndex <= 0 || cbServices.SelectedValue == null)
                {
                    MessageBox.Show("Please select a service to add.", "No Selection",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                int serviceID = Convert.ToInt32(cbServices.SelectedValue);

                // Get service details
                using (SqlConnection conn = new SqlConnection(DB_Salon.connectionString))
                {
                    conn.Open();
                    string query = "SELECT ServiceName, ServicePrice FROM Services WHERE ServiceID = @ServiceID";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@ServiceID", serviceID);
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                string serviceName = reader["ServiceName"].ToString();
                                decimal servicePrice = Convert.ToDecimal(reader["ServicePrice"]);

                                // Check if service already exists
                                bool exists = false;
                                foreach (DataRow row in servicesTable.Rows)
                                {
                                    if (row["ServiceName"].ToString() == serviceName)
                                    {
                                        exists = true;
                                        break;
                                    }
                                }

                                if (!exists)
                                {
                                    servicesTable.Rows.Add(serviceName, servicePrice);
                                    CalculateTotals();
                                    
                                    // Keep the selections so user can add more services
                                    // Only reset the service selection
                                    cbServices.SelectedIndex = 0;
                                }
                                else
                                {
                                    MessageBox.Show("This service is already added.", "Duplicate Service",
                                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error adding service: " + ex.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadProducts()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(DB_Salon.connectionString))
                {
                    conn.Open();
                    string query = "SELECT ProductID, ProductName, ProductPrice FROM Products";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            DataTable dt = new DataTable();
                            dt.Load(reader);

                            // Add empty row at the beginning
                            DataRow emptyRow = dt.NewRow();
                            emptyRow["ProductID"] = 0;
                            emptyRow["ProductName"] = "-- Select Add-on --";
                            emptyRow["ProductPrice"] = 0;
                            dt.Rows.InsertAt(emptyRow, 0);

                            cbProducts.DisplayMember = "ProductName";
                            cbProducts.ValueMember = "ProductID";
                            cbProducts.DataSource = dt;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading products: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadDiscounts()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(DB_Salon.connectionString))
                {
                    conn.Open();
                    string query = "SELECT DiscountID, DiscountName, DiscountAmount FROM Discounts";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            DataTable dt = new DataTable();
                            dt.Load(reader);

                            // Add empty row at the beginning
                            DataRow emptyRow = dt.NewRow();
                            emptyRow["DiscountID"] = 0;
                            emptyRow["DiscountName"] = "-- Select Discount --";
                            emptyRow["DiscountAmount"] = 0;
                            dt.Rows.InsertAt(emptyRow, 0);

                            cbDiscounts.DisplayMember = "DiscountName";
                            cbDiscounts.ValueMember = "DiscountID";
                            cbDiscounts.DataSource = dt;
                            cbDiscounts.Tag = dt; // Store DataTable to access DiscountAmount
                            
                            // Initialize discount textbox
                            txtDiscountPrice.Text = "0.00";
                            discountAmount = 0;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading discounts: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void cbDiscounts_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbDiscounts.SelectedValue != null && cbDiscounts.SelectedIndex > 0)
            {
                int discountID = Convert.ToInt32(cbDiscounts.SelectedValue);
                
                // Get discount amount from stored DataTable
                if (cbDiscounts.Tag is DataTable dt)
                {
                    DataRow[] rows = dt.Select($"DiscountID = {discountID}");
                    if (rows.Length > 0)
                    {
                        discountAmount = Convert.ToDecimal(rows[0]["DiscountAmount"]);
                        txtDiscountPrice.Text = discountAmount.ToString("N2");
            }
            else
            {
                discountAmount = 0;
                        txtDiscountPrice.Text = "0.00";
                    }
                }
                else
                {
                    discountAmount = 0;
                    txtDiscountPrice.Text = "0.00";
                }
            }
            else
            {
                discountAmount = 0;
                txtDiscountPrice.Text = "0.00";
            }

            CalculateTotals();
        }

        private void txtDiscountPrice_TextChanged(object sender, EventArgs e)
        {
            if (decimal.TryParse(txtDiscountPrice.Text, out decimal parsedDiscount))
            {
                if (parsedDiscount < 0)
                {
                    MessageBox.Show("Discount price cannot be negative.", "Invalid Discount",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtDiscountPrice.Text = "0.00";
                    discountAmount = 0;
                    lblDiscountPrice.Text = "0.00";
                    CalculateTotals();
                    return;
                }
                
                discountAmount = parsedDiscount;
                lblDiscountPrice.Text = discountAmount.ToString("N2");
                CalculateTotals();
            }
            else if (string.IsNullOrWhiteSpace(txtDiscountPrice.Text))
            {
                discountAmount = 0;
                lblDiscountPrice.Text = "0.00";
                CalculateTotals();
            }
        }

        private void txtDiscountPrice_Leave(object sender, EventArgs e)
        {
            // Validate that discount price is not zero when a discount is selected
            if (cbDiscounts.SelectedIndex > 0)
            {
                if (string.IsNullOrWhiteSpace(txtDiscountPrice.Text) || 
                    !decimal.TryParse(txtDiscountPrice.Text, out decimal discountValue) || 
                    discountValue <= 0)
                {
                    MessageBox.Show("Discount price must be greater than zero when a discount is selected.", 
                        "Invalid Discount", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtDiscountPrice.Focus();
                    
                    // Reset to the discount amount from the combobox if available
                    if (cbDiscounts.Tag is DataTable dt && cbDiscounts.SelectedIndex > 0)
                    {
                        int discountID = Convert.ToInt32(cbDiscounts.SelectedValue);
                        DataRow[] rows = dt.Select($"DiscountID = {discountID}");
                        if (rows.Length > 0)
                        {
                            decimal originalDiscount = Convert.ToDecimal(rows[0]["DiscountAmount"]);
                            if (originalDiscount > 0)
                            {
                                txtDiscountPrice.Text = originalDiscount.ToString("N2");
                                discountAmount = originalDiscount;
                                lblDiscountPrice.Text = discountAmount.ToString("N2");
                                CalculateTotals();
                            }
                        }
                    }
                }
            }
        }

        private void txtDiscountPrice_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Allow only numbers, decimal point, and backspace
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && e.KeyChar != '.')
            {
                e.Handled = true;
            }

            // Allow only one decimal point
            if (e.KeyChar == '.' && (sender as TextBox).Text.IndexOf('.') > -1)
            {
                e.Handled = true;
            }
        }

        private void CalculateTotals()
        {
            try
            {
                // Calculate total services
                decimal totalServices = 0;
                foreach (DataRow row in servicesTable.Rows)
                {
                    if (row["ServicePrice"] != DBNull.Value)
                    {
                        totalServices += Convert.ToDecimal(row["ServicePrice"]);
                    }
                }
                lblTotalService.Text = totalServices.ToString("N2");

                // Calculate total products (price * quantity)
                decimal totalProducts = 0;
                foreach (DataRow row in productsTable.Rows)
                {
                    if (row["ProductPrice"] != DBNull.Value && row["Quantity"] != DBNull.Value)
                    {
                        decimal unitPrice = Convert.ToDecimal(row["ProductPrice"]);
                        int quantity = Convert.ToInt32(row["Quantity"]);
                        totalProducts += unitPrice * quantity;
                    }
                }
                lblTotalProducts.Text = totalProducts.ToString("N2");

                // Calculate subtotal (services + products)
                decimal subtotal = totalServices + totalProducts;
                lblSubtotal.Text = subtotal.ToString("N2");

                // Display discount amount
                lblDiscountPrice.Text = discountAmount.ToString("N2");

                // Calculate total (subtotal - discount)
                decimal total = subtotal - discountAmount;
                if (total < 0) total = 0; // Prevent negative total
                lblTotal.Text = total.ToString("N2");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error calculating totals: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            cbDiscounts.SelectedIndex = 0;
            discountAmount = 0;
            txtDiscountPrice.Text = "0.00";
            CalculateTotals();
        }

        private void btnAddProduct_Click(object sender, EventArgs e)
        {
            try
            {
                if (cbProducts.SelectedIndex <= 0)
                {
                    MessageBox.Show("Please select a product to add.", "No Selection", 
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                int productID = Convert.ToInt32(cbProducts.SelectedValue);
                
                // Check if product already exists in the grid
                foreach (DataRow row in productsTable.Rows)
                {
                    // We need to get product name from the combobox
                    // For now, let's check by getting it from the database
                }

                // Get product details
                using (SqlConnection conn = new SqlConnection(DB_Salon.connectionString))
                {
                    conn.Open();
                    string query = "SELECT ProductName, ProductPrice FROM Products WHERE ProductID = @ProductID";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@ProductID", productID);
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                string productName = reader["ProductName"].ToString();
                                decimal productPrice = Convert.ToDecimal(reader["ProductPrice"]);

                                // Check if product already exists
                                bool exists = false;
                                foreach (DataRow row in productsTable.Rows)
                                {
                                    if (row["ProductName"].ToString() == productName)
                                    {
                                        exists = true;
                                        break;
                                    }
                                }

                                if (!exists)
                                {
                                    int quantity = (int)numericQuantity.Value;
                                    if (quantity <= 0)
                                    {
                                        MessageBox.Show("Please enter a valid quantity (greater than 0).", "Invalid Quantity",
                                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                        return;
                                    }
                                    productsTable.Rows.Add(productName, productPrice, quantity);
                                    CalculateTotals();
                                    cbProducts.SelectedIndex = 0; // Reset selection
                                    numericQuantity.Value = 1; // Reset quantity to 1
                                }
                                else
                                {
                                    MessageBox.Show("This product is already added.", "Duplicate Product", 
                                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error adding product: " + ex.Message, "Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnProceed_Click(object sender, EventArgs e)
        {
            try
            {
                if (!ValidatePaymentInputs())
                {
                    return;
                }

                if (!EnsureAppointmentForBilling())
                {
                    return;
                }

                // Ask if user wants to print FIRST, before saving
                DialogResult printChoice = MessageBox.Show("Do you want to print the sales summary?",
                    "Print Sales Summary", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                DateTime paymentDate = DateTime.Now;
                string reportsDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "SalesReports");
                Directory.CreateDirectory(reportsDirectory);

                string defaultFileName = $"SalesSummary_{paymentDate:yyyyMMdd}.pdf";

                string pdfPath;
                using (SaveFileDialog saveDialog = new SaveFileDialog()
                {
                    Title = "Save Sales Summary",
                    Filter = "PDF Files (*.pdf)|*.pdf",
                    FileName = defaultFileName,
                    InitialDirectory = reportsDirectory
                })
                {
                    if (saveDialog.ShowDialog() != DialogResult.OK)
                    {
                        return;
                    }
                    pdfPath = saveDialog.FileName;
                }

                GenerateSalesSummaryPdf(pdfPath, paymentDate);
                SaveTransactionRecord(pdfPath, paymentDate);
                UpdateAppointmentStatus("Completed");

                MessageBox.Show("Sales summary saved successfully.", "Success",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);

                // If user chose to print, open the PDF file
                if (printChoice == DialogResult.Yes)
                {
                    try
                    {
                        System.Diagnostics.Process.Start(pdfPath);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Unable to open PDF file: " + ex.Message, "Error",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }

                ReturnToManageQueue();
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error generating sales summary: " + ex.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnCancelPayment_Click(object sender, EventArgs e)
        {
            try
            {
                if (selectedAppointmentID > 0)
                {
                    UpdateAppointmentStatus("On going");
                    // Return to Manage Queue if we came from there
                    ReturnToManageQueue();
                    this.Close();
                    return;
                }

                DialogResult confirm = MessageBox.Show(
                    "Cancel this payment entry?",
                    "Confirm Cancel",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (confirm == DialogResult.Yes)
                {
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error cancelling payment: " + ex.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void cbPayment_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbPayment.SelectedValue != null && cbPayment.SelectedIndex > 0)
            {
                selectedPaymentModeID = Convert.ToInt32(cbPayment.SelectedValue);
                
                // Get payment mode name from stored DataTable
                if (cbPayment.Tag is DataTable dt)
                {
                    DataRow[] rows = dt.Select($"PaymentModeID = {selectedPaymentModeID}");
                    if (rows.Length > 0)
                    {
                        string paymentModeName = rows[0]["PaymentModeName"].ToString();
                        
                        // Enable Reference # only for GCash, disable for Cash
                        if (paymentModeName.Equals("GCash", StringComparison.OrdinalIgnoreCase))
                        {
                            txtReferenceNumber.Enabled = true;
                        }
                        else if (paymentModeName.Equals("Cash", StringComparison.OrdinalIgnoreCase))
                        {
                            txtReferenceNumber.Enabled = false;
                            txtReferenceNumber.Clear();
                        }
                        else
                        {
                            // For other payment modes, default to disabled
                            txtReferenceNumber.Enabled = false;
                            txtReferenceNumber.Clear();
                        }
                    }
                }
            }
            else
            {
                selectedPaymentModeID = 0;
                txtReferenceNumber.Enabled = false;
                txtReferenceNumber.Clear();
            }
        }

        private bool ValidatePaymentInputs()
        {
            if (servicesTable.Rows.Count == 0)
            {
                MessageBox.Show("Please add at least one service before proceeding.",
                    "Missing Service", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtCustomerFirstName.Text) ||
                string.IsNullOrWhiteSpace(txtCustomerLastName.Text))
            {
                MessageBox.Show("Customer name is required.", "Missing Information",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (cbPayment.SelectedIndex <= 0)
            {
                MessageBox.Show("Please select a mode of payment.", "Missing Information",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (cbTechnicians.SelectedIndex <= 0)
            {
                MessageBox.Show("Please select a technician.", "Missing Information",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (cbPayment.Text.Equals("GCash", StringComparison.OrdinalIgnoreCase) &&
                string.IsNullOrWhiteSpace(txtReferenceNumber.Text))
            {
                MessageBox.Show("Please enter the GCash reference number.", "Missing Information",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            // Validate discount price - if a discount is selected, it cannot be zero
            if (cbDiscounts.SelectedIndex > 0)
            {
                if (string.IsNullOrWhiteSpace(txtDiscountPrice.Text) || 
                    !decimal.TryParse(txtDiscountPrice.Text, out decimal discountValue) || 
                    discountValue <= 0)
                {
                    MessageBox.Show("Please enter a valid discount amount greater than zero.", "Invalid Discount",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtDiscountPrice.Focus();
                    return false;
                }
            }

            return true;
        }

        private void GenerateSalesSummaryPdf(string filePath, DateTime paymentDate)
        {
            using (FileStream stream = new FileStream(filePath, FileMode.Create))
            {
                Document document = new Document(PageSize.A4, 36f, 36f, 36f, 36f);
                PdfWriter.GetInstance(document, stream);
                document.Open();

                iTextFont titleFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 18);
                iTextFont sectionFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 14);
                iTextFont normalFont = FontFactory.GetFont(FontFactory.HELVETICA, 11);

                Paragraph title = new Paragraph("Sales Summary", titleFont)
                {
                    Alignment = Element.ALIGN_CENTER,
                    SpacingAfter = 15f
                };
                document.Add(title);

                document.Add(new Paragraph($"Date: {paymentDate:MMMM dd, yyyy hh:mm tt}", normalFont));
                document.Add(new Paragraph($"Processed By: {currentRole} {currentUsername}", normalFont));
                string technicianName = cbTechnicians.SelectedIndex > 0 ? cbTechnicians.Text : "None";
                document.Add(new Paragraph($"Technician: {technicianName}", normalFont));
                document.Add(new Paragraph($"Gcash Reference #: {txtReferenceNumber.Text}", normalFont));
                document.Add(Chunk.NEWLINE);

                document.Add(new Paragraph("Customer Information", sectionFont));
                document.Add(new Paragraph($"Name: {txtCustomerFirstName.Text} {txtCustomerLastName.Text}", normalFont));
                document.Add(new Paragraph($"Contact: {txtContactNumber.Text}", normalFont));
                document.Add(new Paragraph($"Appointment Date: {datePicker.Value:MMMM dd, yyyy}", normalFont));
                document.Add(new Paragraph($"Appointment Time: {TimePicker.Value:hh:mm tt}", normalFont));
                document.Add(Chunk.NEWLINE);

                document.Add(new Paragraph("Availed Services", sectionFont));
                PdfPTable servicesPdfTable = new PdfPTable(2);
                servicesPdfTable.WidthPercentage = 100;
                servicesPdfTable.SetWidths(new float[] { 70f, 30f });
                servicesPdfTable.HorizontalAlignment = Element.ALIGN_LEFT;

                servicesPdfTable.AddCell(GetHeaderCell("Service"));
                servicesPdfTable.AddCell(GetHeaderCell("Price"));

                decimal totalServices = 0;
                foreach (DataRow row in servicesTable.Rows)
                {
                    string serviceName = row["ServiceName"].ToString();
                    decimal price = Convert.ToDecimal(row["ServicePrice"]);
                    totalServices += price;
                    servicesPdfTable.AddCell(GetBodyCell(serviceName));
                    servicesPdfTable.AddCell(GetBodyCell(price.ToString("N2"), Element.ALIGN_RIGHT));
                }
                document.Add(servicesPdfTable);
                document.Add(Chunk.NEWLINE);

                document.Add(new Paragraph("Availed Products", sectionFont));
                PdfPTable productsPdfTable = new PdfPTable(4);
                productsPdfTable.WidthPercentage = 100;
                productsPdfTable.SetWidths(new float[] { 40f, 20f, 20f, 20f });
                productsPdfTable.HorizontalAlignment = Element.ALIGN_LEFT;
                productsPdfTable.AddCell(GetHeaderCell("Product"));
                productsPdfTable.AddCell(GetHeaderCell("Unit Price"));
                productsPdfTable.AddCell(GetHeaderCell("Quantity"));
                productsPdfTable.AddCell(GetHeaderCell("Total"));

                decimal totalProducts = 0;
                if (productsTable.Rows.Count > 0)
                {
                    foreach (DataRow row in productsTable.Rows)
                    {
                        string productName = row["ProductName"].ToString();
                        decimal unitPrice = Convert.ToDecimal(row["ProductPrice"]);
                        int quantity = Convert.ToInt32(row["Quantity"]);
                        decimal rowTotal = unitPrice * quantity;
                        totalProducts += rowTotal;
                        productsPdfTable.AddCell(GetBodyCell(productName));
                        productsPdfTable.AddCell(GetBodyCell(unitPrice.ToString("N2"), Element.ALIGN_RIGHT));
                        productsPdfTable.AddCell(GetBodyCell(quantity.ToString(), Element.ALIGN_CENTER));
                        productsPdfTable.AddCell(GetBodyCell(rowTotal.ToString("N2"), Element.ALIGN_RIGHT));
                    }
                }
                else
                {
                    PdfPCell noProductCell = new PdfPCell(new Phrase("No add-on products availed.", normalFont))
                    {
                        Colspan = 4,
                        HorizontalAlignment = Element.ALIGN_LEFT,
                        Padding = 5f
                    };
                    productsPdfTable.AddCell(noProductCell);
                }

                document.Add(productsPdfTable);
                document.Add(Chunk.NEWLINE);

                document.Add(new Paragraph("Totals", sectionFont));
                PdfPTable totalsTable = new PdfPTable(2);
                totalsTable.WidthPercentage = 70;
                totalsTable.HorizontalAlignment = Element.ALIGN_LEFT;
                totalsTable.SetWidths(new float[] { 50f, 50f });

                totalsTable.AddCell(GetBodyCell("Total Services:", Element.ALIGN_LEFT));
                totalsTable.AddCell(GetBodyCell(totalServices.ToString("N2"), Element.ALIGN_RIGHT));

                totalsTable.AddCell(GetBodyCell("Total Products:", Element.ALIGN_LEFT));
                totalsTable.AddCell(GetBodyCell(totalProducts.ToString("N2"), Element.ALIGN_RIGHT));

                totalsTable.AddCell(GetBodyCell("Discount:", Element.ALIGN_LEFT));
                totalsTable.AddCell(GetBodyCell(discountAmount.ToString("N2"), Element.ALIGN_RIGHT));

                decimal subtotal = totalServices + totalProducts;
                decimal grandTotal = subtotal - discountAmount;
                if (grandTotal < 0) grandTotal = 0;

                totalsTable.AddCell(GetBodyCell("Grand Total:", Element.ALIGN_LEFT));
                totalsTable.AddCell(GetBodyCell(grandTotal.ToString("N2"), Element.ALIGN_RIGHT));

                document.Add(totalsTable);
                document.Add(Chunk.NEWLINE);

                document.Add(new Paragraph($"Mode of Payment: {cbPayment.Text}", normalFont));
                document.Close();
            }
        }

        private PdfPCell GetHeaderCell(string text)
        {
            iTextFont headerFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 12);
            return new PdfPCell(new Phrase(text, headerFont))
            {
                BackgroundColor = new iTextBaseColor(240, 240, 240),
                Padding = 5f,
                HorizontalAlignment = Element.ALIGN_LEFT
            };
        }

        private PdfPCell GetBodyCell(string text, int alignment = Element.ALIGN_LEFT)
        {
            iTextFont bodyFont = FontFactory.GetFont(FontFactory.HELVETICA, 11);
            PdfPCell cell = new PdfPCell(new Phrase(text, bodyFont))
            {
                Padding = 5f,
                HorizontalAlignment = alignment
            };
            return cell;
        }

        private void SaveTransactionRecord(string pdfPath, DateTime paymentDate)
        {
            string serviceList = string.Join(", ", servicesTable.AsEnumerable().Select(r => r["ServiceName"].ToString()));
            decimal totalServices = servicesTable.AsEnumerable().Sum(r => Convert.ToDecimal(r["ServicePrice"]));

            string productList = productsTable.Rows.Count > 0
                ? string.Join(", ", productsTable.AsEnumerable().Select(r => 
                    $"{r["ProductName"]} (Qty: {r["Quantity"]})"))
                : "None";
            decimal totalProducts = productsTable.AsEnumerable().Sum(r => 
                Convert.ToDecimal(r["ProductPrice"]) * Convert.ToInt32(r["Quantity"]));

            string discountName = cbDiscounts.SelectedIndex > 0 ? cbDiscounts.Text : "None";
            decimal subtotal = totalServices + totalProducts;
            decimal totalAmount = subtotal - discountAmount;
            if (totalAmount < 0) totalAmount = 0;

            using (SqlConnection conn = new SqlConnection(DB_Salon.connectionString))
            {
                conn.Open();
                string technicianName = cbTechnicians.SelectedIndex > 0 ? cbTechnicians.Text : null;
                string insertQuery = @"
                    INSERT INTO Transactions 
                    (AppointmentID, PaymentModeID, CustomerFirstName, CustomerLastName, CustomerContact, 
                     AppointmentDate, AppointmentTime, Service1, Service1Price, Service2, Service2Price, 
                     ProductName, ProductPrice, DiscountType, DiscountAmount, Subtotal, Total, TransactionDate, UserID, ReferenceNumber, ReportFilePath, TechnicianName)
                    VALUES
                    (@AppointmentID, @PaymentModeID, @CustomerFirstName, @CustomerLastName, @CustomerContact,
                     @AppointmentDate, @AppointmentTime, @Service1, @Service1Price, @Service2, @Service2Price,
                     @ProductName, @ProductPrice, @DiscountType, @DiscountAmount, @Subtotal, @Total, @TransactionDate, @UserID, @ReferenceNumber, @ReportFilePath, @TechnicianName)";

                using (SqlCommand cmd = new SqlCommand(insertQuery, conn))
                {
                    cmd.Parameters.AddWithValue("@AppointmentID", selectedAppointmentID);
                    cmd.Parameters.AddWithValue("@PaymentModeID", selectedPaymentModeID);
                    cmd.Parameters.AddWithValue("@CustomerFirstName", txtCustomerFirstName.Text);
                    cmd.Parameters.AddWithValue("@CustomerLastName", txtCustomerLastName.Text);
                    cmd.Parameters.AddWithValue("@CustomerContact", txtContactNumber.Text);
                    cmd.Parameters.AddWithValue("@AppointmentDate", datePicker.Value.Date);
                    cmd.Parameters.AddWithValue("@AppointmentTime", TimePicker.Value.TimeOfDay);
                    cmd.Parameters.AddWithValue("@Service1", serviceList);
                    cmd.Parameters.AddWithValue("@Service1Price", totalServices);
                    cmd.Parameters.AddWithValue("@Service2", DBNull.Value);
                    cmd.Parameters.AddWithValue("@Service2Price", 0);
                    cmd.Parameters.AddWithValue("@ProductName", productList);
                    cmd.Parameters.AddWithValue("@ProductPrice", totalProducts);
                    cmd.Parameters.AddWithValue("@DiscountType", discountName);
                    cmd.Parameters.AddWithValue("@DiscountAmount", discountAmount);
                    cmd.Parameters.AddWithValue("@Subtotal", subtotal);
                    cmd.Parameters.AddWithValue("@Total", totalAmount);
                    cmd.Parameters.AddWithValue("@TransactionDate", paymentDate);
                    cmd.Parameters.AddWithValue("@UserID", currentUserID);
                    cmd.Parameters.AddWithValue("@ReferenceNumber", txtReferenceNumber.Text);
                    cmd.Parameters.AddWithValue("@ReportFilePath", pdfPath);
                    cmd.Parameters.AddWithValue("@TechnicianName", string.IsNullOrEmpty(technicianName) ? (object)DBNull.Value : technicianName);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        private void PrintPdf(string filePath)
        {
            try
            {
                if (!IsPdfPrintAssociationAvailable())
                {
                    MessageBox.Show("No default application is associated with PDF printing on this machine. " +
                        "Please open the saved file manually and print from your preferred PDF viewer.",
                        "Print Not Available", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                ProcessStartInfo printProcessInfo = new ProcessStartInfo()
                {
                    Verb = "print",
                    CreateNoWindow = true,
                    FileName = filePath,
                    WindowStyle = ProcessWindowStyle.Hidden
                };
                Process printProcess = new Process();
                printProcess.StartInfo = printProcessInfo;
                printProcess.Start();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Unable to send PDF to printer: " + ex.Message, "Print Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private bool IsPdfPrintAssociationAvailable()
        {
            try
            {
                using (RegistryKey pdfKey = Registry.ClassesRoot.OpenSubKey(".pdf"))
                {
                    if (pdfKey == null)
                    {
                        return false;
                    }

                    string className = pdfKey.GetValue(null) as string;
                    if (string.IsNullOrEmpty(className))
                    {
                        return false;
                    }

                    using (RegistryKey commandKey = Registry.ClassesRoot.OpenSubKey(className + "\\shell\\print\\command"))
                    {
                        return commandKey != null;
                    }
                }
            }
            catch
            {
                return false;
            }
        }

        private void UpdateAppointmentStatus(string status)
        {
            if (selectedAppointmentID <= 0)
            {
                return;
            }

            try
            {
                using (SqlConnection conn = new SqlConnection(DB_Salon.connectionString))
                {
                    conn.Open();
                    string query = "UPDATE Appointments SET Status = @Status WHERE AppointmentID = @AppointmentID";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@Status", status);
                        cmd.Parameters.AddWithValue("@AppointmentID", selectedAppointmentID);
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error updating appointment status: " + ex.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ReturnToManageQueue()
        {
            try
            {
                foreach (Form form in Application.OpenForms)
                {
                    if (form is AdminHomeDashboard || form is CashierHomeDashboard)
                    {
                        var method = form.GetType().GetMethod("openChildForm",
                            System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                        if (method != null)
                        {
                            method.Invoke(form, new object[] { new AdminManageQueue(currentUserID) });
                        }
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error returning to Manage Queue: " + ex.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnRemoveService_Click(object sender, EventArgs e)
        {
            try
            {
                if (dataAvailedServices.SelectedRows.Count == 0)
                {
                    MessageBox.Show("Please select a service to remove.", "No Selection",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                DataGridViewRow selectedRow = dataAvailedServices.SelectedRows[0];
                string serviceName = selectedRow.Cells["ServiceName"].Value?.ToString();

                if (!string.IsNullOrEmpty(serviceName))
                {
                    DialogResult result = MessageBox.Show(
                        $"Are you sure you want to remove '{serviceName}'?",
                        "Confirm Removal",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Question);

                    if (result == DialogResult.Yes)
                    {
                        // Remove the row from the DataTable
                        int rowIndex = selectedRow.Index;
                        if (rowIndex >= 0 && rowIndex < servicesTable.Rows.Count)
                        {
                            servicesTable.Rows.RemoveAt(rowIndex);
                        }

                        CalculateTotals();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error removing service: " + ex.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnRemoveProducts_Click(object sender, EventArgs e)
        {
            try
            {
                if (dataAvailedProducts.SelectedRows.Count == 0)
                {
                    MessageBox.Show("Please select a product to remove.", "No Selection",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                DataGridViewRow selectedRow = dataAvailedProducts.SelectedRows[0];
                string productName = selectedRow.Cells["ProductName"].Value?.ToString();

                if (!string.IsNullOrEmpty(productName))
                {
                    DialogResult result = MessageBox.Show(
                        $"Are you sure you want to remove '{productName}'?",
                        "Confirm Removal",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Question);

                    if (result == DialogResult.Yes)
                    {
                        // Remove the row from the DataTable
                        int rowIndex = selectedRow.Index;
                        if (rowIndex >= 0 && rowIndex < productsTable.Rows.Count)
                        {
                            productsTable.Rows.RemoveAt(rowIndex);
                        }

                        CalculateTotals();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error removing product: " + ex.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dataAvailedProducts_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            // If quantity column was changed, recalculate totals
            if (e.ColumnIndex >= 0 && dataAvailedProducts.Columns[e.ColumnIndex].Name == "Quantity")
            {
                try
                {
                    DataGridViewRow row = dataAvailedProducts.Rows[e.RowIndex];
                    if (row.Cells["Quantity"].Value != null)
                    {
                        if (int.TryParse(row.Cells["Quantity"].Value.ToString(), out int quantity))
                        {
                            if (quantity <= 0)
                            {
                                MessageBox.Show("Quantity must be greater than 0.", "Invalid Quantity",
                                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                row.Cells["Quantity"].Value = 1;
                            }
                        }
                    }
                    CalculateTotals();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error updating quantity: " + ex.Message, "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private bool EnsureAppointmentForBilling()
        {
            if (selectedAppointmentID > 0)
            {
                return true;
            }

            try
            {
                selectedAppointmentID = CreateManualAppointmentFromForm();
                return selectedAppointmentID > 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Unable to create appointment record for billing: " + ex.Message,
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        private int CreateManualAppointmentFromForm()
        {
            if (servicesTable.Rows.Count == 0)
            {
                throw new InvalidOperationException("Please add at least one service before proceeding.");
            }

            string serviceList = string.Join(", ", servicesTable.AsEnumerable().Select(r => r["ServiceName"].ToString()));
            string primaryServiceName = servicesTable.Rows[0]["ServiceName"].ToString();
            int serviceId = GetServiceIdByName(primaryServiceName);

            using (SqlConnection conn = new SqlConnection(DB_Salon.connectionString))
            {
                conn.Open();
                string insertQuery = @"INSERT INTO Appointments
                                       (CustomerFirstName, CustomerLastName, CustomerContact, ServiceID, [Service Name],
                                        AppointmentDate, AppointmentTime, Status, UserID)
                                       OUTPUT INSERTED.AppointmentID
                                       VALUES
                                       (@FirstName, @LastName, @Contact, @ServiceID, @ServiceName,
                                        @AppointmentDate, @AppointmentTime, 'Ready for Billing', @UserID)";

                using (SqlCommand cmd = new SqlCommand(insertQuery, conn))
                {
                    cmd.Parameters.AddWithValue("@FirstName", txtCustomerFirstName.Text);
                    cmd.Parameters.AddWithValue("@LastName", txtCustomerLastName.Text);
                    cmd.Parameters.AddWithValue("@Contact", txtContactNumber.Text);
                    cmd.Parameters.AddWithValue("@ServiceID", serviceId);
                    cmd.Parameters.AddWithValue("@ServiceName", serviceList);
                    cmd.Parameters.AddWithValue("@AppointmentDate", datePicker.Value.Date);
                    cmd.Parameters.AddWithValue("@AppointmentTime", TimePicker.Value.TimeOfDay);
                    cmd.Parameters.AddWithValue("@UserID", currentUserID > 0 ? currentUserID : (object)DBNull.Value);

                    object result = cmd.ExecuteScalar();
                    if (result != null && result != DBNull.Value)
                    {
                        return Convert.ToInt32(result);
                    }
                }
            }

            throw new Exception("Failed to create appointment record.");
        }

        private int GetServiceIdByName(string serviceName)
        {
            if (string.IsNullOrWhiteSpace(serviceName))
            {
                return GetDefaultServiceId();
            }

            using (SqlConnection conn = new SqlConnection(DB_Salon.connectionString))
            {
                conn.Open();
                string query = "SELECT TOP 1 ServiceID FROM Services WHERE ServiceName = @ServiceName";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@ServiceName", serviceName);
                    object result = cmd.ExecuteScalar();
                    if (result != null && result != DBNull.Value)
                    {
                        return Convert.ToInt32(result);
                    }
                }
            }

            return GetDefaultServiceId();
        }

        private int GetDefaultServiceId()
        {
            using (SqlConnection conn = new SqlConnection(DB_Salon.connectionString))
            {
                conn.Open();
                string query = "SELECT TOP 1 ServiceID FROM Services ORDER BY ServiceID";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    object result = cmd.ExecuteScalar();
                    if (result != null && result != DBNull.Value)
                    {
                        return Convert.ToInt32(result);
                    }
                }
            }

            throw new Exception("No services are defined in the system.");
        }
    }
}

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

        public AdminPayments()
        {
            InitializeComponent();
            InitializeDataGridViews();
            WireUpEventHandlers();
            LoadCategories();
            LoadProducts();
            LoadDiscounts();
            LoadPaymentModes();
        }

        public AdminPayments(int userID, int appointmentId = 0)
        {
            InitializeComponent();
            currentUserID = userID;
            selectedAppointmentID = appointmentId;
            
            InitializeDataGridViews();
            WireUpEventHandlers();
            LoadCategories();
            LoadProducts();
            LoadDiscounts();
            LoadPaymentModes();
            
            if (appointmentId > 0)
            {
                LoadAppointmentDetails(appointmentId);
            }
        }

        private void WireUpEventHandlers()
        {
            btnAddProduct.Click += btnAddProduct_Click;
            btnAddServices.Click += btnAddServices_Click;
            btnRemoveService.Click += btnRemoveService_Click;
            btnRemoveProducts.Click += btnRemoveProducts_Click;
            cbServiceCategory.SelectedIndexChanged += cbServiceCategory_SelectedIndexChanged;
            cbDiscounts.SelectedIndexChanged += cbDiscounts_SelectedIndexChanged;
            cbPayment.SelectedIndexChanged += cbPayment_SelectedIndexChanged;
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
            dataAvailedProducts.DataSource = productsTable;
            dataAvailedProducts.Columns["ProductName"].HeaderText = "Product Name";
            dataAvailedProducts.Columns["ProductName"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dataAvailedProducts.Columns["ProductPrice"].HeaderText = "Price";
            dataAvailedProducts.Columns["ProductPrice"].DefaultCellStyle.Format = "N2";
            dataAvailedProducts.Columns["ProductPrice"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dataAvailedProducts.AllowUserToAddRows = false;
            dataAvailedProducts.ReadOnly = true;
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
                                    a.Status
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
                                    datePicker.Value = apptDate;
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
                    }
                    else
                    {
                        discountAmount = 0;
                    }
                }
                else
                {
                    discountAmount = 0;
                }
            }
            else
            {
                discountAmount = 0;
            }

            CalculateTotals();
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

                // Calculate total products
                decimal totalProducts = 0;
                foreach (DataRow row in productsTable.Rows)
                {
                    if (row["ProductPrice"] != DBNull.Value)
                    {
                        totalProducts += Convert.ToDecimal(row["ProductPrice"]);
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
                                    productsTable.Rows.Add(productName, productPrice);
                                    CalculateTotals();
                                    cbProducts.SelectedIndex = 0; // Reset selection
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

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
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
    }
}

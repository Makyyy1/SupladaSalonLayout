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
        private int selectedAppointmentID = 0;
        private decimal service1Price = 0;
        private decimal service2Price = 0;
        private decimal productPrice = 0;
        private decimal discountAmount = 0;
        private int selectedPaymentModeID = 0;
        private int currentUserID = -1;

        public AdminPayments()
        {
            InitializeComponent();
            LoadProducts();
            LoadDiscounts();
            LoadPaymentModes();
        }

        public AdminPayments(int userID, int appointmentId = 0)
        {
            InitializeComponent();
            currentUserID = userID;
            selectedAppointmentID = appointmentId;
            
            LoadProducts();
            LoadDiscounts();
            LoadPaymentModes();
            
            if (appointmentId > 0)
            {
                LoadAppointmentDetails(appointmentId);
            }
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
                                
                                // Add services to the list view
                                string services = reader["Services"].ToString();
                                if (!string.IsNullOrEmpty(services))
                                {
                                    //listViewProducts.Items.Clear();
                                    foreach (string service in services.Split(','))
                                    {
                                        if (!string.IsNullOrWhiteSpace(service))
                                        {
                                            //listViewProducts.Items.Add(service.Trim());
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

        /*private void LoadCustomerDetails(int appointmentID)
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
                                        a.Service1,
                                        a.Service2,
                                        s1.ServicePrice as Service1Price,
                                        s2.ServicePrice as Service2Price
                                    FROM Appointments a
                                    LEFT JOIN Services s1 ON a.Service1 = s1.ServiceName
                                    LEFT JOIN Services s2 ON a.Service2 = s2.ServiceName
                                    WHERE a.AppointmentID = @AppointmentID";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@AppointmentID", appointmentID);
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {

                                lblFirstName.Text = reader["CustomerFirstName"].ToString();
                                lblLastName.Text = reader["CustomerLastName"].ToString();
                                lblContactNumber.Text = reader["CustomerContact"].ToString();

                                lblDate.Text = Convert.ToDateTime(reader["AppointmentDate"]).ToString("MMMM dd, yyyy");

                                object timeValue = reader["AppointmentTime"];
                                if (timeValue != DBNull.Value)
                                {
                                    if (timeValue is TimeSpan)
                                    {
                                        TimeSpan ts = (TimeSpan)timeValue;
                                        
                                        DateTime dt = DateTime.Today.Add(ts);
                                        lblTime.Text = dt.ToString("hh:mm tt"); 
                                                                                
                                    }
                                    else
                                    {
                                        TimeSpan ts;
                                        if (TimeSpan.TryParse(timeValue.ToString(), out ts))
                                        {
                                            DateTime dt = DateTime.Today.Add(ts);
                                            lblTime.Text = dt.ToString("hh:mm tt");
                                        }
                                        else
                                        {
                                            lblTime.Text = timeValue.ToString();
                                        }
                                    }
                                }

                                string service1 = reader["Service1"].ToString();
                                string service2 = reader["Service2"].ToString();

                                service1Price = reader["Service1Price"] != DBNull.Value ? Convert.ToDecimal(reader["Service1Price"]) : 0;
                                service2Price = !string.IsNullOrEmpty(service2) && reader["Service2Price"] != DBNull.Value
                                    ? Convert.ToDecimal(reader["Service2Price"]) : 0;

                                lblService1.Text = service1;
                                lblPriceService1.Text = service1Price.ToString("0.00");

                                if (!string.IsNullOrEmpty(service2))
                                {
                                    lblService2.Text = service2;
                                    lblPriceService2.Text = service2Price.ToString("0.00");
                                }
                                else
                                {
                                    lblService2.Text = "";
                                    lblPriceService2.Text = "";
                                }

                                cbProducts.SelectedIndex = 0;
                                cbDiscounts.SelectedIndex = 0;
                                cbPayment.SelectedIndex = 0;
                                productPrice = 0;
                                discountAmount = 0;

                                CalculateTotals();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading customer details: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }*/

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

                            clProducts.DisplayMember = "ProductName";
                            clProducts.ValueMember = "ProductID";
                            clProducts.DataSource = dt;
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
                            cbDiscounts.ValueMember = "DiscountAmount";
                            cbDiscounts.DataSource = dt;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading discounts: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void cbProducts_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (clProducts.SelectedValue != null && int.Parse(clProducts.SelectedValue.ToString()) > 0)
            {
                try
                {
                    using (SqlConnection conn = new SqlConnection(DB_Salon.connectionString))
                    {
                        conn.Open();
                        string query = "SELECT ProductPrice FROM Products WHERE ProductID = @ProductID";

                        using (SqlCommand cmd = new SqlCommand(query, conn))
                        {
                            cmd.Parameters.AddWithValue("@ProductID", clProducts.SelectedValue);
                            object result = cmd.ExecuteScalar();

                            if (result != null)
                            {
                                productPrice = Convert.ToDecimal(result);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error loading product price: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                productPrice = 0;
            }

            CalculateTotals();
        }

        private void cbDiscounts_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbDiscounts.SelectedValue != null && cbDiscounts.SelectedIndex > 0)
            {
                discountAmount = Convert.ToDecimal(cbDiscounts.SelectedValue);
            }
            else
            {
                discountAmount = 0;
            }

            CalculateTotals();
        }

        private void CalculateTotals()
        {
            // Calculate Availed Service Prices
            decimal availedServicePrice = service1Price + service2Price;
            lblTotalService.Text = availedServicePrice.ToString("0.00");

            // Calculate SubTotal (Services + Add-ons)
            decimal subTotal = availedServicePrice + productPrice;
            lblSubtotal.Text = subTotal.ToString("0.00");

            // Calculate Discount Amount
            //decimal discountAmount = subTotal * discountAmount;
            lblDiscountPrice.Text = discountAmount.ToString("0.00");

            // Calculate Total
            decimal total = subTotal - discountAmount;
            lblTotal.Text = total.ToString("0.00");
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            cbDiscounts.SelectedIndex = 0;
            discountAmount = 0;
            CalculateTotals();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
           
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            
        }

        private void ShowReceipt()
        {
            
        }

        private void SaveTransaction()
        {
            
        }

        private void InitializeTransactionsTable()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(DB_Salon.connectionString))
                {
                    conn.Open();
                    // Check if UserID column exists in Transactions table, if not add it
                    string checkColumn = @"IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[Transactions]') AND name = 'UserID')
                                          BEGIN
                                              ALTER TABLE Transactions ADD UserID INT NULL;
                                              -- Update existing records to link to appointments' UserID
                                              UPDATE t SET t.UserID = a.UserID
                                              FROM Transactions t
                                              INNER JOIN Appointments a ON t.AppointmentID = a.AppointmentID;
                                          END";
                    using (SqlCommand cmd = new SqlCommand(checkColumn, conn))
                    {
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Error initializing Transactions table: " + ex.Message);
            }
        }

        private void UpdateAppointmentStatus()
        {
            using (SqlConnection conn = new SqlConnection(DB_Salon.connectionString))
            {
                conn.Open();
                string query = "UPDATE Appointments SET Status = 'Completed' WHERE AppointmentID = @AppointmentID";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@AppointmentID", selectedAppointmentID);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        private void RefreshReportsForm()
        {
            foreach (Form form in Application.OpenForms)
            {
                if (form is AdminReports)
                {
                    ((AdminReports)form).LoadTransactions();
                    break;
                }
            }
        }

        private void cbPayment_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}

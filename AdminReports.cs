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
    public partial class AdminReports : Form
    {
        private int currentUserID = -1;

        public AdminReports()
        {
            InitializeComponent();
            InitializeDatePickers();
            LoadTransactions();
        }

        public AdminReports(int userID)
        {
            InitializeComponent();
            currentUserID = userID;
            InitializeDatePickers();
            LoadTransactions();
        }

        private void InitializeDatePickers()
        {
            dtpStartDate.MinDate = new DateTime(1900, 1, 1);
            dtpStartDate.MaxDate = new DateTime(2100, 12, 31);
            dtpEndDate.MinDate = new DateTime(1900, 1, 1);
            dtpEndDate.MaxDate = new DateTime(2100, 12, 31);

            dtpStartDate.Value = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            dtpEndDate.Value = DateTime.Now;
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

        public void LoadTransactions()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(DB_Salon.connectionString))
                {
                    conn.Open();
                    // Initialize Transactions table to add UserID if needed
                    InitializeTransactionsTable();

                    string query = @"SELECT 
                                        t.TransactionID,
                                        t.CustomerFirstName + ' ' + t.CustomerLastName AS CustomerName,
                                        t.CustomerContact,
                                        t.AppointmentDate,
                                        t.Service1,
                                        t.Service2,
                                        t.ProductName,
                                        t.DiscountType,
                                        ISNULL(pm.PaymentModeName, 'N/A') AS PaymentMode,
                                        t.Total,
                                        t.TransactionDate
                                    FROM Transactions t
                                    LEFT JOIN PaymentModes pm ON t.PaymentModeID = pm.PaymentModeID
                                    LEFT JOIN Appointments a ON t.AppointmentID = a.AppointmentID
                                    WHERE CAST(t.TransactionDate AS DATE) >= @StartDate 
                                    AND CAST(t.TransactionDate AS DATE) <= @EndDate
                                    AND a.UserID = @UserID
                                    ORDER BY t.TransactionDate DESC";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@StartDate", dtpStartDate.Value.Date);
                        cmd.Parameters.AddWithValue("@EndDate", dtpEndDate.Value.Date);

                        SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);

                        dataReports.DataSource = dt;

                        if (dataReports.Columns.Count > 0)
                        {
                            dataReports.Columns["TransactionID"].Visible = false;

                            // Set column headers
                            dataReports.Columns["CustomerName"].HeaderText = "Customer Name";
                            dataReports.Columns["CustomerContact"].HeaderText = "Contact";
                            dataReports.Columns["AppointmentDate"].HeaderText = "Appointment Date";
                            dataReports.Columns["Service1"].HeaderText = "Service 1";
                            dataReports.Columns["Service2"].HeaderText = "Service 2";
                            dataReports.Columns["ProductName"].HeaderText = "Product";
                            dataReports.Columns["DiscountType"].HeaderText = "Discount";
                            dataReports.Columns["PaymentMode"].HeaderText = "Mode of Payment";
                            dataReports.Columns["Total"].HeaderText = "Total";
                            dataReports.Columns["TransactionDate"].HeaderText = "Transaction Date";

                            dataReports.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

                            dataReports.Columns["CustomerName"].FillWeight = 100;
                            dataReports.Columns["CustomerContact"].FillWeight = 80;
                            dataReports.Columns["AppointmentDate"].FillWeight = 80;
                            dataReports.Columns["Service1"].FillWeight = 110;
                            dataReports.Columns["Service2"].FillWeight = 110;
                            dataReports.Columns["ProductName"].FillWeight = 80;
                            dataReports.Columns["DiscountType"].FillWeight = 80;
                            dataReports.Columns["PaymentMode"].FillWeight = 90;
                            dataReports.Columns["Total"].FillWeight = 70;
                            dataReports.Columns["TransactionDate"].FillWeight = 100;

                            dataReports.Columns["Total"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

                            // Format TransactionDate column
                            dataReports.Columns["TransactionDate"].DefaultCellStyle.Format = "MM/dd/yyyy hh:mm tt";

                            // Set additional styling
                            dataReports.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
                            dataReports.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;

                            dataReports.ReadOnly = true;
                            dataReports.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                            dataReports.MultiSelect = false;
                        }

                        // Calculate total earnings
                        CalculateTotalEarnings();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading transactions: " + ex.Message,
                               "Error",
                               MessageBoxButtons.OK,
                               MessageBoxIcon.Error);
            }
        }

        private void CalculateTotalEarnings()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(DB_Salon.connectionString))
                {
                    conn.Open();
                    string query = @"SELECT ISNULL(SUM(Total), 0) 
                                    FROM Transactions
                                    WHERE CAST(TransactionDate AS DATE) >= @StartDate 
                                    AND CAST(TransactionDate AS DATE) <= @EndDate";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@StartDate", dtpStartDate.Value.Date);
                        cmd.Parameters.AddWithValue("@EndDate", dtpEndDate.Value.Date);

                        object result = cmd.ExecuteScalar();
                        decimal totalEarnings = result != null ? Convert.ToDecimal(result) : 0;

                        lblTotalEarnings.Text = $"₱{totalEarnings:N2}";
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error calculating earnings: " + ex.Message,
                               "Error",
                               MessageBoxButtons.OK,
                               MessageBoxIcon.Error);
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (dtpStartDate.Value > dtpEndDate.Value)
            {
                MessageBox.Show("Start date cannot be later than end date.",
                               "Invalid Date Range",
                               MessageBoxButtons.OK,
                               MessageBoxIcon.Warning);
                return;
            }

            LoadTransactions();
        }
    }
}

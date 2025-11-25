using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
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

namespace SupladaSalonLayout
{
    public partial class AdminReports : Form
    {
        private int currentUserID = -1;
        private string currentUserRole = "";

        public AdminReports()
        {
            InitializeComponent();
            InitializeDatePickers();
            LoadUserFilter();
            WireUpEventHandlers();
            LoadTransactions();
        }

        public AdminReports(int userID)
        {
            InitializeComponent();
            currentUserID = userID;
            LoadCurrentUserRole();
            InitializeDatePickers();
            LoadUserFilter();
            WireUpEventHandlers();
            LoadTransactions();
        }

        private void WireUpEventHandlers()
        {
            dataReports.CellDoubleClick += dataReports_CellDoubleClick;
        }

        private void LoadCurrentUserRole()
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
                    string query = "SELECT Role FROM Users WHERE UserID = @UserID";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@UserID", currentUserID);
                        object result = cmd.ExecuteScalar();
                        if (result != null && result != DBNull.Value)
                        {
                            currentUserRole = result.ToString();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Error loading user role: " + ex.Message);
            }
        }

        private void LoadUserFilter()
        {
            cbSortUsers.Items.Clear();
            
            // If user is Cashier, lock to Cashier only
            if (currentUserRole.Equals("Cashier", StringComparison.OrdinalIgnoreCase))
            {
                cbSortUsers.Items.Add("Cashier");
                cbSortUsers.SelectedIndex = 0;
                cbSortUsers.Enabled = false; // Lock the combobox
            }
            else
            {
                // Admin or no user context - show all options
                cbSortUsers.Items.Add("All");
                cbSortUsers.Items.Add("Admin");
                cbSortUsers.Items.Add("Cashier");
                cbSortUsers.SelectedIndex = 0;
                cbSortUsers.Enabled = true;
            }
            
            cbSortUsers.SelectedIndexChanged += cbSortUsers_SelectedIndexChanged;
        }

        private void cbSortUsers_SelectedIndexChanged(object sender, EventArgs e)
        {
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
                    EnsureTransactionsColumn(conn, "UserID", "INT NULL",
                        @"UPDATE t SET t.UserID = a.UserID
                          FROM Transactions t
                          INNER JOIN Appointments a ON t.AppointmentID = a.AppointmentID;");
                    EnsureTransactionsColumn(conn, "ReferenceNumber", "NVARCHAR(100) NULL");
                    EnsureTransactionsColumn(conn, "ReportFilePath", "NVARCHAR(MAX) NULL");
                    EnsureTransactionsColumn(conn, "TechnicianName", "NVARCHAR(100) NULL");
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Error initializing Transactions table: " + ex.Message);
            }
        }

        private void EnsureTransactionsColumn(SqlConnection conn, string columnName, string definition, string postScript = null)
        {
            string query = $@"
                IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[Transactions]') AND name = '{columnName}')
                BEGIN
                    ALTER TABLE Transactions ADD {columnName} {definition};
                    {postScript}
                END";

            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.ExecuteNonQuery();
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

                    string roleFilter = cbSortUsers.SelectedItem?.ToString();
                    string query = @"SELECT 
                                        t.TransactionID,
                                        t.CustomerFirstName + ' ' + t.CustomerLastName AS CustomerName,
                                        t.CustomerContact,
                                        t.AppointmentDate,
                                        t.Service1,
                                        t.ProductName,
                                        t.DiscountType,
                                        ISNULL(pm.PaymentModeName, 'N/A') AS PaymentMode,
                                        ISNULL(NULLIF(t.ReferenceNumber, ''), 'None') AS ReferenceNumber,
                                        ISNULL(NULLIF(t.TechnicianName, ''), 'None') AS TechnicianName,
                                        ISNULL(u.Username, 'Unknown') AS ProcessedBy,
                                        ISNULL(u.Role, 'N/A') AS ProcessedRole,
                                        t.Total,
                                        t.TransactionDate,
                                        t.ReportFilePath
                                    FROM Transactions t
                                    LEFT JOIN PaymentModes pm ON t.PaymentModeID = pm.PaymentModeID
                                    LEFT JOIN Appointments a ON t.AppointmentID = a.AppointmentID
                                    LEFT JOIN Users u ON COALESCE(t.UserID, a.UserID) = u.UserID
                                    WHERE CAST(t.TransactionDate AS DATE) >= @StartDate 
                                    AND CAST(t.TransactionDate AS DATE) <= @EndDate";

                    if (!string.IsNullOrEmpty(roleFilter) && roleFilter != "All")
                    {
                        query += " AND u.Role = @Role";
                    }
                    query += " ORDER BY t.TransactionDate DESC";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@StartDate", dtpStartDate.Value.Date);
                        cmd.Parameters.AddWithValue("@EndDate", dtpEndDate.Value.Date);
                        if (!string.IsNullOrEmpty(roleFilter) && roleFilter != "All")
                        {
                            cmd.Parameters.AddWithValue("@Role", roleFilter);
                        }

                        SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);

                        dataReports.DataSource = dt;

                        if (dataReports.Columns.Count > 0)
                        {
                            dataReports.Columns["TransactionID"].Visible = false;
                            dataReports.Columns["ReportFilePath"].Visible = false;

                            // Set column headers
                            dataReports.Columns["CustomerName"].HeaderText = "Customer Name";
                            dataReports.Columns["CustomerContact"].HeaderText = "Contact";
                            dataReports.Columns["AppointmentDate"].HeaderText = "Appointment Date";
                            dataReports.Columns["Service1"].HeaderText = "Services";
                            dataReports.Columns["ProductName"].HeaderText = "Product";
                            dataReports.Columns["DiscountType"].HeaderText = "Discount";
                            dataReports.Columns["PaymentMode"].HeaderText = "Mode of Payment";
                            dataReports.Columns["ReferenceNumber"].HeaderText = "Reference #";
                            dataReports.Columns["TechnicianName"].HeaderText = "Technician";
                            dataReports.Columns["ProcessedBy"].HeaderText = "Processed By";
                            dataReports.Columns["ProcessedRole"].HeaderText = "Role";
                            dataReports.Columns["Total"].HeaderText = "Total";
                            dataReports.Columns["TransactionDate"].HeaderText = "Transaction Date";

                            dataReports.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

                            dataReports.Columns["CustomerName"].FillWeight = 100;
                            dataReports.Columns["CustomerContact"].FillWeight = 80;
                            dataReports.Columns["AppointmentDate"].FillWeight = 80;
                            dataReports.Columns["Service1"].FillWeight = 130;
                            dataReports.Columns["ProductName"].FillWeight = 80;
                            dataReports.Columns["DiscountType"].FillWeight = 80;
                            dataReports.Columns["PaymentMode"].FillWeight = 90;
                            dataReports.Columns["ReferenceNumber"].FillWeight = 90;
                            dataReports.Columns["TechnicianName"].FillWeight = 100;
                            dataReports.Columns["ProcessedBy"].FillWeight = 90;
                            dataReports.Columns["ProcessedRole"].FillWeight = 70;
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

                        // Calculate totals
                        CalculateTotals();
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

        private void CalculateTotals()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(DB_Salon.connectionString))
                {
                    conn.Open();
                    string roleFilter = cbSortUsers.SelectedItem?.ToString();
                    
                    string query = @"SELECT ISNULL(SUM(t.Total), 0), COUNT(t.TransactionID)
                                    FROM Transactions t
                                    LEFT JOIN Appointments a ON t.AppointmentID = a.AppointmentID
                                    LEFT JOIN Users u ON COALESCE(t.UserID, a.UserID) = u.UserID
                                    WHERE CAST(t.TransactionDate AS DATE) >= @StartDate 
                                    AND CAST(t.TransactionDate AS DATE) <= @EndDate";

                    if (!string.IsNullOrEmpty(roleFilter) && roleFilter != "All")
                    {
                        query += " AND u.Role = @Role";
                    }

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@StartDate", dtpStartDate.Value.Date);
                        cmd.Parameters.AddWithValue("@EndDate", dtpEndDate.Value.Date);
                        if (!string.IsNullOrEmpty(roleFilter) && roleFilter != "All")
                        {
                            cmd.Parameters.AddWithValue("@Role", roleFilter);
                        }

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                decimal totalEarnings = reader.GetDecimal(0);
                                int totalCustomers = reader.GetInt32(1);
                                lblTotalEarnings.Text = $"₱{totalEarnings:N2}";
                                lblTotalCustomers.Text = totalCustomers.ToString();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error calculating totals: " + ex.Message,
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

        private void btnPrintReports_Click(object sender, EventArgs e)
        {
            if (dataReports.Rows.Count == 0)
            {
                MessageBox.Show("No data to print.", "No Data", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                string defaultFileName = $"TransactionReport_{dtpStartDate.Value:yyyyMMdd}_to_{dtpEndDate.Value:yyyyMMdd}.pdf";
                string defaultFolderPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Reports");

                if (!Directory.Exists(defaultFolderPath))
                {
                    Directory.CreateDirectory(defaultFolderPath);
                }

                SaveFileDialog saveFileDialog = new SaveFileDialog
                {
                    Filter = "PDF files (*.pdf)|*.pdf",
                    FileName = defaultFileName,
                    InitialDirectory = defaultFolderPath,
                    Title = "Save Transaction Report PDF"
                };

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    GenerateReportPdf(saveFileDialog.FileName);
                    MessageBox.Show("Report saved successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    DialogResult printResult = MessageBox.Show("Do you want to print the report?", "Print", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (printResult == DialogResult.Yes)
                    {
                        System.Diagnostics.Process.Start(saveFileDialog.FileName);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error generating report: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void GenerateReportPdf(string filePath)
        {
            Document doc = new Document(PageSize.A4.Rotate(), 25, 25, 30, 30);
            PdfWriter.GetInstance(doc, new FileStream(filePath, FileMode.Create));
            doc.Open();

            iTextFont titleFont = new iTextFont(iTextFont.FontFamily.HELVETICA, 18, iTextFont.BOLD);
            iTextFont headerFont = new iTextFont(iTextFont.FontFamily.HELVETICA, 10, iTextFont.BOLD, iTextBaseColor.WHITE);
            iTextFont cellFont = new iTextFont(iTextFont.FontFamily.HELVETICA, 9);
            iTextFont summaryFont = new iTextFont(iTextFont.FontFamily.HELVETICA, 12, iTextFont.BOLD);

            // Title
            Paragraph title = new Paragraph("Transaction Report", titleFont);
            title.Alignment = Element.ALIGN_CENTER;
            doc.Add(title);

            // Date Range
            string roleFilter = cbSortUsers.SelectedItem?.ToString() ?? "All";
            Paragraph dateRange = new Paragraph($"From: {dtpStartDate.Value:MM/dd/yyyy} To: {dtpEndDate.Value:MM/dd/yyyy}  |  Filter: {roleFilter}", cellFont);
            dateRange.Alignment = Element.ALIGN_CENTER;
            dateRange.SpacingAfter = 15;
            doc.Add(dateRange);

            // Table
            PdfPTable table = new PdfPTable(11);
            table.WidthPercentage = 100;
            table.SetWidths(new float[] { 11, 9, 9, 13, 7, 7, 9, 7, 9, 7, 10 });

            string[] headers = { "Customer", "Contact", "Date", "Services", "Product", "Discount", "Payment", "Ref #", "Technician", "Total", "Processed By" };
            foreach (string header in headers)
            {
                PdfPCell cell = new PdfPCell(new Phrase(header, headerFont));
                cell.BackgroundColor = new iTextBaseColor(50, 50, 50);
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                cell.Padding = 5;
                table.AddCell(cell);
            }

            foreach (DataGridViewRow row in dataReports.Rows)
            {
                if (row.IsNewRow) continue;
                table.AddCell(new PdfPCell(new Phrase(row.Cells["CustomerName"].Value?.ToString() ?? "", cellFont)));
                table.AddCell(new PdfPCell(new Phrase(row.Cells["CustomerContact"].Value?.ToString() ?? "", cellFont)));
                table.AddCell(new PdfPCell(new Phrase(row.Cells["AppointmentDate"].Value != null ? Convert.ToDateTime(row.Cells["AppointmentDate"].Value).ToString("MM/dd/yyyy") : "", cellFont)));
                table.AddCell(new PdfPCell(new Phrase(row.Cells["Service1"].Value?.ToString() ?? "", cellFont)));
                table.AddCell(new PdfPCell(new Phrase(row.Cells["ProductName"].Value?.ToString() ?? "", cellFont)));
                table.AddCell(new PdfPCell(new Phrase(row.Cells["DiscountType"].Value?.ToString() ?? "", cellFont)));
                table.AddCell(new PdfPCell(new Phrase(row.Cells["PaymentMode"].Value?.ToString() ?? "", cellFont)));
                table.AddCell(new PdfPCell(new Phrase(row.Cells["ReferenceNumber"].Value?.ToString() ?? "", cellFont)));
                table.AddCell(new PdfPCell(new Phrase(row.Cells["TechnicianName"].Value?.ToString() ?? "None", cellFont)));
                
                PdfPCell totalCell = new PdfPCell(new Phrase(row.Cells["Total"].Value != null ? $"₱{Convert.ToDecimal(row.Cells["Total"].Value):N2}" : "", cellFont));
                totalCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                table.AddCell(totalCell);
                
                table.AddCell(new PdfPCell(new Phrase($"{row.Cells["ProcessedBy"].Value} ({row.Cells["ProcessedRole"].Value})", cellFont)));
            }

            doc.Add(table);

            // Summary
            doc.Add(new Paragraph("\n"));
            Paragraph summary = new Paragraph($"Total Customers: {lblTotalCustomers.Text}     |     Total Earnings: {lblTotalEarnings.Text}", summaryFont);
            summary.Alignment = Element.ALIGN_RIGHT;
            doc.Add(summary);

            doc.Close();
        }

        private void dataReports_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex < 0)
                {
                    return;
                }

                DataGridViewRow selectedRow = dataReports.Rows[e.RowIndex];
                
                if (selectedRow.Cells["ReportFilePath"].Value == null || 
                    selectedRow.Cells["ReportFilePath"].Value == DBNull.Value)
                {
                    MessageBox.Show("No PDF file associated with this transaction.", "No PDF File",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                string pdfPath = selectedRow.Cells["ReportFilePath"].Value.ToString();

                if (string.IsNullOrWhiteSpace(pdfPath))
                {
                    MessageBox.Show("No PDF file path found for this transaction.", "No PDF File",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                if (!File.Exists(pdfPath))
                {
                    MessageBox.Show("The PDF file could not be found at the specified path.\n\n" +
                        $"Path: {pdfPath}", "File Not Found",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Open the PDF file
                System.Diagnostics.Process.Start(pdfPath);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error opening PDF file: " + ex.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}

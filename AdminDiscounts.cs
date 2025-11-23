using iTextSharp.text.pdf;
using iTextSharp.text;
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

namespace SupladaSalonLayout
{
    public partial class AdminDiscounts : Form
    {
        public AdminDiscounts()
        {
            InitializeComponent();
        }

        private void ResetFields()
        {
            txtDiscountName.Clear();
            rtbDiscountDiscription.Clear();
            txtDiscountAmount.Clear();
        }

        private void LoadDiscountsGrid(string searchKeyword = "")
        {

            try
            {
                using (SqlConnection connect = new SqlConnection(DB_Salon.connectionString))
                {
                    connect.Open();
                    string query = @"SELECT DiscountName, DiscountDescription, DiscountAmount FROM Discounts";

                    if (!string.IsNullOrWhiteSpace(searchKeyword))
                        query += " WHERE LEFT(UPPER(DiscountName), 1) = UPPER(@Search)";

                    query += " ORDER BY DiscountName";

                    using (SqlCommand cmd = new SqlCommand(query, connect))
                    {
                        if (!string.IsNullOrWhiteSpace(searchKeyword))
                            cmd.Parameters.AddWithValue("@Search", searchKeyword.Substring(0, 1));

                        SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                        DataTable data = new DataTable();
                        adapter.Fill(data);


                        dataDiscounts.DataSource = data;
                    }
                }
            }
            catch (Exception)
            {
                MessageBox.Show("We’re having trouble loading the product list. " + "Please make sure the product data is available and try again.", "Load Products", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void AdminDiscounts_Load(object sender, EventArgs e)
        {
            LoadDiscountsGrid();
        }

        private void txtSearchDiscounts_TextChanged(object sender, EventArgs e)
        {
            LoadDiscountsGrid(txtSearchDiscounts.Text.Trim());
        }

        private void dataDiscounts_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            //Delete
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                if (e.RowIndex >= 0)
                {

                    if (dataDiscounts.Columns[e.ColumnIndex].HeaderText == "Remove")
                    {
                        string discountName = dataDiscounts.Rows[e.RowIndex].Cells["DiscountName"].Value.ToString();

                        DialogResult confirm = MessageBox.Show($"Are you sure you want to delete '{discountName}'?", "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                        if (confirm == DialogResult.Yes)
                        {
                            try
                            {
                                using (SqlConnection connect = new SqlConnection(DB_Salon.connectionString))
                                {
                                    connect.Open();
                                    string query = "DELETE FROM Discounts WHERE DiscountName = @DiscountName";
                                    using (SqlCommand cmd = new SqlCommand(query, connect))
                                    {
                                        cmd.Parameters.AddWithValue("@DiscountName", discountName);
                                        cmd.ExecuteNonQuery();
                                    }
                                }

                                MessageBox.Show("Discount deleted successfully.", "Delete Discount", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                LoadDiscountsGrid();
                            }
                            catch
                            {
                                MessageBox.Show("Something went wrong while deleting the discount. Please try again.", "Delete Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            }
                        }
                    }
                }

                //Edit
                if (dataDiscounts.Columns[e.ColumnIndex].HeaderText == "Edit")
                {
                    string DiscountName = Convert.ToString(dataDiscounts.Rows[e.RowIndex].Cells["DiscountName"].Value);
                    string DiscountDescription = Convert.ToString(dataDiscounts.Rows[e.RowIndex].Cells["DiscountDescription"].Value);
                    decimal DiscountAmount = Convert.ToDecimal(dataDiscounts.Rows[e.RowIndex].Cells["DiscountAmount"].Value);

                    FMEditDiscounts editProduct = new FMEditDiscounts(DiscountName, DiscountDescription, DiscountAmount);
                    DialogResult result = editProduct.ShowDialog();

                    if (result == DialogResult.OK)
                    {
                        LoadDiscountsGrid();
                    }
                }
            }
        }

        private void btnAddDiscount_Click(object sender, EventArgs e)
        {
            string discountName = txtDiscountName.Text;
            string discountDescription = rtbDiscountDiscription.Text;
            string discountAmount = txtDiscountAmount.Text;

            if (string.IsNullOrWhiteSpace(discountName))
            {
                MessageBox.Show("Discount Name cannot be empty.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(discountDescription))
            {
                MessageBox.Show("Discount Description cannot be empty.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!decimal.TryParse(discountAmount, out decimal price) || price < 0)
            {
                MessageBox.Show("Discount amount must be a number.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                using (SqlConnection connect = new SqlConnection(DB_Salon.connectionString))
                {
                    connect.Open();
                    string query = "INSERT INTO Discounts (DiscountName, DiscountDescription, DiscountAmount) VALUES (@DiscountName, @DiscountDescription, @DiscountAmount)";

                    using (SqlCommand cmd = new SqlCommand(query, connect))
                    {
                        cmd.Parameters.AddWithValue("@DiscountName", txtDiscountName.Text.Trim());
                        cmd.Parameters.AddWithValue("@DiscountDescription", rtbDiscountDiscription.Text.Trim());
                        cmd.Parameters.AddWithValue("@DiscountAmount", decimal.Parse(txtDiscountAmount.Text));
                        cmd.ExecuteNonQuery();
                    }

                    MessageBox.Show("Discount added successfully.", "Edit Discount", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadDiscountsGrid();
                    ResetFields();
                }
            }
            catch
            {
                MessageBox.Show("Unable to add discount.", "Edit Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void pcClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            if (dataDiscounts.Rows.Count > 0)
            {
                SaveFileDialog save = new SaveFileDialog();
                save.Filter = "PDF (*.pdf)|*.pdf";
                save.FileName = "ListOfDiscounts.pdf";
                bool ErrorMessage = false;

                if (save.ShowDialog() == DialogResult.OK)
                {
                    if (File.Exists(save.FileName))
                    {
                        try
                        {
                            File.Delete(save.FileName);
                        }
                        catch (Exception ex)
                        {
                            ErrorMessage = true;
                            MessageBox.Show("Unable to write data in disk: " + ex.Message);
                        }
                    }

                    if (!ErrorMessage)
                    {
                        try
                        {
                            // Count non-image columns
                            int columnCount = 0;
                            foreach (DataGridViewColumn column in dataDiscounts.Columns)
                            {
                                if (!(column is DataGridViewImageColumn))
                                {
                                    columnCount++;
                                }
                            }

                            PdfPTable pTable = new PdfPTable(columnCount);
                            pTable.DefaultCell.Padding = 5;
                            pTable.WidthPercentage = 100;
                            pTable.HorizontalAlignment = Element.ALIGN_LEFT;

                            // Add column headers (skip image columns)
                            foreach (DataGridViewColumn column in dataDiscounts.Columns)
                            {
                                if (!(column is DataGridViewImageColumn))
                                {
                                    PdfPCell cell = new PdfPCell(new Phrase(column.HeaderText));
                                    cell.BackgroundColor = new iTextSharp.text.BaseColor(240, 240, 240);
                                    cell.HorizontalAlignment = Element.ALIGN_CENTER;
                                    cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                                    cell.Padding = 5;
                                    pTable.AddCell(cell);
                                }
                            }

                            // Add rows (skip image columns)
                            foreach (DataGridViewRow row in dataDiscounts.Rows)
                            {
                                if (!row.IsNewRow)
                                {
                                    int columnIndex = 0;
                                    foreach (DataGridViewCell cell in row.Cells)
                                    {
                                        if (!(cell.OwningColumn is DataGridViewImageColumn))
                                        {
                                            string cellValue = cell.Value != null ? cell.Value.ToString() : "";
                                            PdfPCell pdfCell = new PdfPCell(new Phrase(cellValue));
                                            pdfCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                                            pdfCell.Padding = 5;

                                            // Check if this is the Price column (last column)
                                            if (cell.OwningColumn.HeaderText == "Discount Amount")
                                            {
                                                pdfCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                                            }
                                            else
                                            {
                                                pdfCell.HorizontalAlignment = Element.ALIGN_LEFT;
                                            }

                                            pTable.AddCell(pdfCell);
                                            columnIndex++;
                                        }
                                    }
                                }
                            }

                            using (FileStream fileStream = new FileStream(save.FileName, FileMode.Create))
                            {
                                Document docs = new Document(PageSize.A4, 8f, 16f, 16f, 8f);
                                PdfWriter.GetInstance(docs, fileStream);
                                docs.Open();

                                // ADD TITLE HERE
                                iTextSharp.text.Font titleFont = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 18, iTextSharp.text.Font.BOLD);
                                Paragraph title = new Paragraph("List of Discounts", titleFont);
                                title.Alignment = Element.ALIGN_CENTER;
                                title.SpacingAfter = 20f;
                                docs.Add(title);

                                // Add the table
                                docs.Add(pTable);
                                docs.Close();
                                fileStream.Close();
                            }

                            MessageBox.Show("Data exported successfully!", "Info");

                            // AUTOMATICALLY OPEN THE PDF
                            try
                            {
                                System.Diagnostics.Process.Start(save.FileName);
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show("PDF created but couldn't open automatically: " + ex.Message);
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Error while exporting data: " + ex.Message);
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("No record found", "Info");
            }
     }
    }
}

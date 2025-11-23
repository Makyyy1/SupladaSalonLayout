using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Runtime.Remoting.Contexts;
using iTextSharp.text.pdf;
using iTextSharp.text;
using System.IO;

namespace SupladaSalonLayout
{
    public partial class FMAdminProducts : Form
    {
        public FMAdminProducts()
        {
            InitializeComponent();
        }

        private void FMAdminProducts_Load(object sender, EventArgs e)
        {
            LoadProductsGrid();
        }

        private void ResetFields()
        {
            txtProductName.Clear();
            richTxtDescription.Clear();
            txtProductPrice.Clear();
        }

        private void LoadProductsGrid(string searchKeyword = "")
        {

            try
            {
                using (SqlConnection connect = new SqlConnection(DB_Salon.connectionString))
                {
                    connect.Open();
                    string query = @"SELECT ProductName, ProductDescription, ProductPrice FROM Products";

                    if (!string.IsNullOrWhiteSpace(searchKeyword))
                        query += " WHERE LEFT(UPPER(ProductName), 1) = UPPER(@Search)";

                    query += " ORDER BY ProductName";

                    using (SqlCommand cmd = new SqlCommand(query, connect))
                    {
                        if (!string.IsNullOrWhiteSpace(searchKeyword))
                            cmd.Parameters.AddWithValue("@Search", searchKeyword.Substring(0, 1));

                        SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                        DataTable data = new DataTable();
                        adapter.Fill(data);


                        dataProducts.DataSource = data;
                    }
                }
            }
            catch (Exception)
            {
                MessageBox.Show("We’re having trouble loading the product list. " + "Please make sure the product data is available and try again.", "Load Products", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void txtSearchProducts_TextChanged(object sender, EventArgs e)
        {
            LoadProductsGrid(txtSearchProducts.Text.Trim());
        }

        private void dataProducts_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            //Delete
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                if (e.RowIndex >= 0)
                {

                    if (dataProducts.Columns[e.ColumnIndex].HeaderText == "Remove")
                    {
                        string productName = dataProducts.Rows[e.RowIndex].Cells["ProductName"].Value.ToString();

                        DialogResult confirm = MessageBox.Show($"Are you sure you want to delete '{productName}'?", "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                        if (confirm == DialogResult.Yes)
                        {
                            try
                            {
                                using (SqlConnection connect = new SqlConnection(DB_Salon.connectionString))
                                {
                                    connect.Open();
                                    string query = "DELETE FROM Products WHERE ProductName = @ProductName";
                                    using (SqlCommand cmd = new SqlCommand(query, connect))
                                    {
                                        cmd.Parameters.AddWithValue("@ProductName", productName);
                                        cmd.ExecuteNonQuery();
                                    }
                                }

                                MessageBox.Show("Product deleted successfully.", "Delete Product", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                LoadProductsGrid();
                            }
                            catch
                            {
                                MessageBox.Show("Something went wrong while deleting the product. Please try again.", "Delete Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            }
                        }
                    }
                }

                //Edit
                if (dataProducts.Columns[e.ColumnIndex].HeaderText == "Edit")
                {
                    string ProductName = Convert.ToString(dataProducts.Rows[e.RowIndex].Cells["ProductName"].Value);
                    string ProductDescription = Convert.ToString(dataProducts.Rows[e.RowIndex].Cells["ProductDescription"].Value);
                    decimal ProductPrice = Convert.ToDecimal(dataProducts.Rows[e.RowIndex].Cells["ProductPrice"].Value);

                    FMEditProducts editProduct = new FMEditProducts(ProductName, ProductDescription, ProductPrice);
                    DialogResult result = editProduct.ShowDialog();

                    if (result == DialogResult.OK)
                    {
                        LoadProductsGrid();
                    }
                }
            }
        }

        private void btnAddProduct_Click(object sender, EventArgs e)
        {
            string productName = txtProductName.Text;
            string productDescription = richTxtDescription.Text;
            string productPrice = txtProductPrice.Text;

            if (string.IsNullOrWhiteSpace(productName))
            {
                MessageBox.Show("Product Name cannot be empty.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(productDescription))
            {
                MessageBox.Show("Product Description cannot be empty.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!decimal.TryParse(productPrice, out decimal price) || price < 0)
            {
                MessageBox.Show("Product Price must be a number.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                using (SqlConnection connect = new SqlConnection(DB_Salon.connectionString))
                {
                    connect.Open();
                    string query = "INSERT INTO Products (ProductName, ProductDescription, ProductPrice) VALUES (@ProductName, @ProductDescription, @ProductPrice)";

                    using (SqlCommand cmd = new SqlCommand(query, connect))
                    {
                        cmd.Parameters.AddWithValue("@ProductName", txtProductName.Text.Trim());
                        cmd.Parameters.AddWithValue("@ProductDescription", richTxtDescription.Text.Trim());
                        cmd.Parameters.AddWithValue("@ProductPrice", decimal.Parse(txtProductPrice.Text));
                        cmd.ExecuteNonQuery();
                    }

                    MessageBox.Show("Product added successfully.", "Edit Product", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadProductsGrid();
                    ResetFields();
                }
            }
            catch
            {
                MessageBox.Show("Unable to add product.", "Edit Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void pcClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            if (dataProducts.Rows.Count > 0)
            {
                SaveFileDialog save = new SaveFileDialog();
                save.Filter = "PDF (*.pdf)|*.pdf";
                save.FileName = "ListOfProducts.pdf";
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
                            foreach (DataGridViewColumn column in dataProducts.Columns)
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
                            foreach (DataGridViewColumn column in dataProducts.Columns)
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
                            foreach (DataGridViewRow row in dataProducts.Rows)
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
                                            if (cell.OwningColumn.HeaderText == "Price")
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
                                Paragraph title = new Paragraph("List of Products", titleFont);
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
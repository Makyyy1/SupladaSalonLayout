using iTextSharp.text;
using iTextSharp.text.pdf;
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

namespace SupladaSalonLayout
{
    public partial class FMAdminServices : Form
    {

        public FMAdminServices()
        {
            InitializeComponent();
        }

        private void ResetFields()
        {
            txtCategoryName.Clear();
            richTxtDescription.Clear();
        }

        private void FMAdminServices_Load(object sender, EventArgs e)
        {
            LoadServicesGrid();
            LoadFilterCategories();
        }

        private void LoadServicesGrid(string searchKeyword = "", string categoryFilter = "All")
        {
            try
            {
                using (SqlConnection connect = new SqlConnection(DB_Salon.connectionString))
                {
                    connect.Open();
                    string query = @"SELECT c.CategoryName, s.ServiceName, s.ServiceDescription, s.ServicePrice FROM Services s INNER JOIN Categories c ON s.CategoryID = c.CategoryID WHERE 1=1";

                    if (!string.IsNullOrWhiteSpace(searchKeyword))
                        query += " AND LEFT(UPPER(s.ServiceName), 1) = UPPER(@Search)";

                    if (categoryFilter != "All")
                        query += " AND c.CategoryName = @CategoryName";

                    query += " ORDER BY s.ServiceID";

                    using (SqlCommand cmd = new SqlCommand(query, connect))
                    {
                        if (!string.IsNullOrWhiteSpace(searchKeyword))
                            cmd.Parameters.AddWithValue("@Search", searchKeyword.Substring(0, 1));

                        if (categoryFilter != "All")
                            cmd.Parameters.AddWithValue("@CategoryName", categoryFilter);

                        SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                        DataTable data = new DataTable();
                        adapter.Fill(data);


                        dataServices.DataSource = data;
                    }
                }
            }
            catch (Exception)
            {
                MessageBox.Show("We’re having trouble loading the service list. " + "Please make sure the service data is available and try again.", "Load Services", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void txtSearchServices_TextChanged(object sender, EventArgs e)
        {
            string categoryFilter = cbServiceCategory.SelectedItem?.ToString() ?? "All";
            LoadServicesGrid(txtSearchServices.Text.Trim(), categoryFilter);
            
        }

        private void dataServices_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            //Delete
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                if (e.RowIndex >= 0)
                {

                    if (dataServices.Columns[e.ColumnIndex].HeaderText == "Remove")
                    {
                        string serviceName = dataServices.Rows[e.RowIndex].Cells["ServiceName"].Value.ToString();
                        string categoryName = dataServices.Rows[e.RowIndex].Cells["CategoryName"].Value.ToString();

                        DialogResult confirm = MessageBox.Show($"Are you sure you want to delete '{serviceName}'?", "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                        if (confirm == DialogResult.Yes)
                        {
                            try
                            {
                                using (SqlConnection connect = new SqlConnection(DB_Salon.connectionString))
                                {
                                    connect.Open();
                                    string query = "DELETE FROM Services WHERE ServiceName = @ServiceName";
                                    using (SqlCommand cmd = new SqlCommand(query, connect))
                                    {
                                        cmd.Parameters.AddWithValue("@ServiceName", serviceName);
                                        cmd.ExecuteNonQuery();
                                    }
                                }

                                MessageBox.Show("Service deleted successfully.", "Delete Service", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                LoadServicesGrid();
                            }
                            catch
                            {
                                MessageBox.Show("Something went wrong while deleting the service. Please try again.", "Delete Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            }
                        }
                    }
                }
                //Edit
                if (dataServices.Columns[e.ColumnIndex].HeaderText == "Edit")
                {
                    string CategoryName = Convert.ToString(dataServices.Rows[e.RowIndex].Cells["CategoryName"].Value.ToString());
                    string ServiceName = Convert.ToString(dataServices.Rows[e.RowIndex].Cells["ServiceName"].Value.ToString());
                    string ServiceDescription = Convert.ToString(dataServices.Rows[e.RowIndex].Cells["ServiceDescription"].Value.ToString());
                    decimal ServicePrice = Convert.ToDecimal(dataServices.Rows[e.RowIndex].Cells["ServicePrice"].Value);

                    FMEditServices editService = new FMEditServices(CategoryName, ServiceName, ServiceDescription,ServicePrice);
                    DialogResult result = editService.ShowDialog();

                    if (result == DialogResult.OK)
                    {
                        string categoryFilter = cbServiceCategory.SelectedItem?.ToString() ?? "All";
                        LoadServicesGrid(txtSearchServices.Text.Trim(), categoryFilter);
                    }
                }
            }
        }

        private void btnAddCategory_Click(object sender, EventArgs e)
        {
            string categoryName = txtCategoryName.Text;
            string Description = richTxtDescription.Text;

            if (string.IsNullOrWhiteSpace(categoryName))
            {
                MessageBox.Show("Category Name cannot be empty.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(Description))
            {
                MessageBox.Show("Description cannot be empty.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                using (SqlConnection connect = new SqlConnection(DB_Salon.connectionString))
                {
                    connect.Open();
                    string query = "INSERT INTO Categories (CategoryName, Description) VALUES (@CategoryName, @Description)";

                    using (SqlCommand cmd = new SqlCommand(query, connect))
                    {
                        cmd.Parameters.AddWithValue("@CategoryName", txtCategoryName.Text.Trim());
                        cmd.Parameters.AddWithValue("@Description", richTxtDescription.Text.Trim());
                        cmd.ExecuteNonQuery();
                    }

                    MessageBox.Show("New Category added successfully.", "Adding Categories", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadFilterCategories();
                    LoadServicesGrid();
                    ResetFields();
                }
            }
            catch
            {
                MessageBox.Show("Unable to add category.", "Creating Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void LoadFilterCategories()
        {
            try
            {
                using (SqlConnection connect = new SqlConnection(DB_Salon.connectionString))
                {
                    connect.Open();
                    string query = "SELECT CategoryName FROM Categories ORDER BY CategoryName";
                    SqlCommand cmd = new SqlCommand(query, connect);
                    SqlDataReader reader = cmd.ExecuteReader();

                    cbServiceCategory.Items.Clear();
                    cbServiceCategory.Items.Add("All");
                    while (reader.Read())
                    {
                        cbServiceCategory.Items.Add(reader["CategoryName"].ToString());
                    }
                    cbServiceCategory.SelectedIndex = 0;
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Error loading categories");
            }
        }

        private void cbServiceCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            string categoryFilter = cbServiceCategory.SelectedItem?.ToString() ?? "All";
            LoadServicesGrid(txtSearchServices.Text.Trim(), categoryFilter);
        }

        private void btnAddServices_Click(object sender, EventArgs e)
        {
            FMAddServices addService = new FMAddServices();

            DialogResult result = addService.ShowDialog();

            if (result == DialogResult.OK)
            {
                string categoryFilter = cbServiceCategory.SelectedItem?.ToString() ?? "All";
                LoadServicesGrid(txtSearchServices.Text.Trim(), categoryFilter);
            }
        }

        private void btnViewCategories_Click(object sender, EventArgs e)
        {
            FMViewCategories viewCategory = new FMViewCategories();
            viewCategory.ShowDialog();

            LoadFilterCategories();
        }

        private void pcClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            if (dataServices.Rows.Count > 0)
            {
                SaveFileDialog save = new SaveFileDialog();
                save.Filter = "PDF (*.pdf)|*.pdf";
                save.FileName = "Service.pdf";
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
                            foreach (DataGridViewColumn column in dataServices.Columns)
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
                            foreach (DataGridViewColumn column in dataServices.Columns)
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
                            foreach (DataGridViewRow row in dataServices.Rows)
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
                                Paragraph title = new Paragraph("List of Services", titleFont);
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

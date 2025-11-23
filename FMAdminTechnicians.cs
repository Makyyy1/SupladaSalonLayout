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
    public partial class FMAdminTechnicians : Form
    {
        public FMAdminTechnicians()
        {
            InitializeComponent();
            LoadCategories();
        }

        private void LoadTechnicianGrid(string searchKeyword = "", string categoryFilter = "All")
        {
            try
            {
                using (SqlConnection connect = new SqlConnection(DB_Salon.connectionString))
                {
                    connect.Open();
                    string query = @"SELECT t.TechnicianCode, r.RoleName, t.TechnicianName, t.TechnicianNumber, t.TechnicianAddress, t.Gender FROM Technicians t  INNER JOIN TechnicianRoles r ON t.RoleID = r.RoleID  WHERE 1=1";

                    if (!string.IsNullOrWhiteSpace(searchKeyword))
                        query += " AND LEFT(UPPER(t.TechnicianName), 1) = UPPER(@Search)";

                    if (categoryFilter != "All")
                        query += " AND r.RoleName = @RoleName";

                    query += " ORDER BY t.TechnicianCode";

                    using (SqlCommand cmd = new SqlCommand(query, connect))
                    {
                        if (!string.IsNullOrWhiteSpace(searchKeyword))
                            cmd.Parameters.AddWithValue("@Search", searchKeyword.Substring(0, 1));

                        if (categoryFilter != "All")
                            cmd.Parameters.AddWithValue("@RoleName", categoryFilter);

                        SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                        DataTable data = new DataTable();
                        adapter.Fill(data);


                        dataTechnicians.DataSource = data;
                    }
                }
            }
            catch (Exception)
            {
                MessageBox.Show("We’re having trouble loading the technician list. " + "Please make sure the technician data is available and try again.", "Load Technicians", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void FMAdminTechnicians_Load(object sender, EventArgs e)
        {
            LoadTechnicianGrid();
            LoadFilterCategories();
        }

        private void LoadFilterCategories()
        {
            try
            {
                using (SqlConnection connect = new SqlConnection(DB_Salon.connectionString))
                {
                    connect.Open();
                    string query = "SELECT RoleName FROM TechnicianRoles ORDER BY RoleName";
                    SqlCommand cmd = new SqlCommand(query, connect);
                    SqlDataReader reader = cmd.ExecuteReader();

                    cbRoleCategory.Items.Clear();
                    cbRoleCategory.Items.Add("All");
                    while (reader.Read())
                    {
                        cbRoleCategory.Items.Add(reader["RoleName"].ToString());
                    }
                    cbRoleCategory.SelectedIndex = 0;
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Error loading categories");
            }
        }

        private string GetNextTechnicianCode()
        {
            try
            {
                using (SqlConnection connect = new SqlConnection(DB_Salon.connectionString))
                {
                    connect.Open();

                    string query = "SELECT TechnicianCode FROM Technicians ORDER BY TechnicianCode";
                    List<int> existingNumbers = new List<int>();

                    using (SqlCommand cmd = new SqlCommand(query, connect))
                    {
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                string code = reader["TechnicianCode"].ToString();
                                if (code.StartsWith("TECH") && code.Length == 7)
                                {
                                    int number = int.Parse(code.Substring(4));
                                    existingNumbers.Add(number);
                                }
                            }
                        }
                    }

                    int nextNumber = 1;
                    existingNumbers.Sort();

                    foreach (int num in existingNumbers)
                    {
                        if (num == nextNumber)
                        {
                            nextNumber++;
                        }
                        else if (num > nextNumber)
                        {
                            break;
                        }
                    }

                    return "TECH" + nextNumber.ToString("D3");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error generating code: {ex.Message}");
                return "TECH001";
            }
        }

        private void cbRoleCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            string categoryFilter = cbRoleCategory.SelectedItem?.ToString() ?? "All";
            LoadTechnicianGrid(txtSearchTechnicians.Text.Trim(), categoryFilter);
        }

        private void txtSearchTechnicians_TextChanged(object sender, EventArgs e)
        {
            string categoryFilter = cbRoleCategory.SelectedItem?.ToString() ?? "All";
            LoadTechnicianGrid(txtSearchTechnicians.Text.Trim(), categoryFilter);
        }

        private void dataTechnicians_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            //Delete
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                if (e.RowIndex >= 0)
                {

                    if (dataTechnicians.Columns[e.ColumnIndex].HeaderText == "Remove")
                    {
                        string technicianName = dataTechnicians.Rows[e.RowIndex].Cells["TechnicianName"].Value.ToString();

                        DialogResult confirm = MessageBox.Show($"Are you sure you want to delete '{technicianName}'?", "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                        if (confirm == DialogResult.Yes)
                        {
                            try
                            {
                                using (SqlConnection connect = new SqlConnection(DB_Salon.connectionString))
                                {
                                    connect.Open();
                                    string query = "DELETE FROM Technicians WHERE TechnicianName = @TechnicianName";
                                    using (SqlCommand cmd = new SqlCommand(query, connect))
                                    {
                                        cmd.Parameters.AddWithValue("@TechnicianName", technicianName);
                                        cmd.ExecuteNonQuery();
                                    }
                                }

                                MessageBox.Show("Technician deleted successfully.", "Delete Technician", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                LoadTechnicianGrid();
                            }
                            catch
                            {
                                MessageBox.Show("Something went wrong while deleting the technician. Please try again.", "Delete Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            }
                        }
                    }
                }
                //Edit
                if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
                {
                    
                    if (dataTechnicians.Columns[e.ColumnIndex].HeaderText == "Edit")
                    {
                        string techCode = Convert.ToString(dataTechnicians.Rows[e.RowIndex].Cells["TechnicianCode"].Value);
                        string roleName = Convert.ToString(dataTechnicians.Rows[e.RowIndex].Cells["RoleName"].Value);
                        string technicianName = Convert.ToString(dataTechnicians.Rows[e.RowIndex].Cells["TechnicianName"].Value);
                        string technicianNumber = Convert.ToString(dataTechnicians.Rows[e.RowIndex].Cells["TechnicianNumber"].Value);
                        string technicianAddress = Convert.ToString(dataTechnicians.Rows[e.RowIndex].Cells["TechnicianAddress"].Value);
                        string gender = Convert.ToString(dataTechnicians.Rows[e.RowIndex].Cells["Gender"].Value);

                        FMEditTechnicians editTechnician = new FMEditTechnicians(techCode, roleName, technicianName, technicianNumber, technicianAddress, gender);
                        DialogResult result = editTechnician.ShowDialog();

                        if (result == DialogResult.OK)
                        {
                            string categoryFilter = cbRoleCategory.SelectedItem?.ToString() ?? "All";
                            LoadTechnicianGrid(txtSearchTechnicians.Text.Trim(), categoryFilter);
                        }
                    }
                }
            }
        }

        private bool ValidateInputs()
        {
            if (string.IsNullOrWhiteSpace(txtTechName.Text))
            {
                MessageBox.Show("Technician name is required.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (!rbMale.Checked && !rbFemale.Checked)
            {
                MessageBox.Show("Please select a gender.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtTechContact.Text))
            {
                MessageBox.Show("Contact number is required.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtTechContact.Focus();
                return false;
            }

            if (txtTechContact.Text.Length != 11 || !txtTechContact.Text.All(char.IsDigit))
            {
                MessageBox.Show("Contact number must be exactly 11 digits.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtTechContact.Focus();
                return false;
            }

            if (cbTechRole.SelectedValue == null || cbTechRole.SelectedIndex == -1)
            {
                MessageBox.Show("Please select a technician role.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cbTechRole.Focus();
                return false;
            }

            return true;
        }

        private void btnAddTechnician_Click(object sender, EventArgs e)
        {
            if (!ValidateInputs())
            {
                return;
            }

            string gender = "";
            if (rbMale.Checked)
            {
                gender = "Male";
            }
            else if (rbFemale.Checked)
            {
                gender = "Female";
            }

            try
            {

                using (SqlConnection connect = new SqlConnection(DB_Salon.connectionString))
                {
                    connect.Open();
                    string query = @"INSERT INTO Technicians (TechnicianName, Gender, TechnicianNumber, TechnicianAddress, RoleID) 
                            VALUES (@Name, @Gender, @Number, @Address, @RoleID)";

                    using (SqlCommand cmd = new SqlCommand(query, connect))
                    {
                        cmd.Parameters.AddWithValue("@Name", txtTechName.Text.Trim());
                        cmd.Parameters.AddWithValue("@Gender", gender);
                        cmd.Parameters.AddWithValue("@Number", txtTechContact.Text.Trim());
                        cmd.Parameters.AddWithValue("@Address", txtTechAddress.Text.Trim());
                        cmd.Parameters.AddWithValue("@RoleID", cbTechRole.SelectedValue);

                        cmd.ExecuteNonQuery();
                    }
                }

                MessageBox.Show("Technician added successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadFilterCategories();
                LoadTechnicianGrid();
                ResetFields();
                
            }
            catch
            {
                MessageBox.Show("Unable to add category.", "Creating Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void LoadCategories()
        {
            try
            {
                using (SqlConnection connect = new SqlConnection(DB_Salon.connectionString))
                {
                    connect.Open();
                    string query = "SELECT  RoleID, RoleName FROM TechnicianRoles ORDER BY RoleName";
                    SqlCommand cmd = new SqlCommand(query, connect);
                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    DataTable data = new DataTable();
                    adapter.Fill(data);

                    cbTechRole.DataSource = data;
                    cbTechRole.DisplayMember = "RoleName";
                    cbTechRole.ValueMember = "RoleID";
                    cbTechRole.SelectedIndex = -1;

                }
            }
            catch (Exception)
            {
                MessageBox.Show("Error loading technician Role");
            }
        }

        private void ResetFields()
        {
            txtTechName.Clear();
            txtTechContact.Clear();
            txtTechAddress.Clear();

            cbTechRole.SelectedIndex = -1;
            rbMale.Checked = false;
            rbFemale.Checked = false;
        }

        private void btnAddRoles_Click(object sender, EventArgs e)
        {
            FMAddRoles addRole = new FMAddRoles();

            DialogResult result = addRole.ShowDialog();

            if (result == DialogResult.OK)
            {
                LoadCategories();
                LoadFilterCategories();

                string categoryFilter = cbTechRole.SelectedItem?.ToString() ?? "All";
                LoadTechnicianGrid(txtSearchTechnicians.Text.Trim(), categoryFilter);
            }
        }

        private void pcClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            if (dataTechnicians.Rows.Count > 0)
            {
                SaveFileDialog save = new SaveFileDialog();
                save.Filter = "PDF (*.pdf)|*.pdf";
                save.FileName = "ListOfTechnicians.pdf";
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
                            int columnCount = 0;
                            foreach (DataGridViewColumn column in dataTechnicians.Columns)
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

                            foreach (DataGridViewColumn column in dataTechnicians.Columns)
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

                            foreach (DataGridViewRow row in dataTechnicians.Rows)
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

                                iTextSharp.text.Font titleFont = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 18, iTextSharp.text.Font.BOLD);
                                Paragraph title = new Paragraph("List of Technicians", titleFont);
                                title.Alignment = Element.ALIGN_CENTER;
                                title.SpacingAfter = 20f;
                                docs.Add(title);

                                // Add the table
                                docs.Add(pTable);
                                docs.Close();
                                fileStream.Close();
                            }

                            MessageBox.Show("Data exported successfully!", "Info");

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

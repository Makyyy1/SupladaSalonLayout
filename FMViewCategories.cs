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
    public partial class FMViewCategories : Form
    {

        public FMViewCategories()
        {
            InitializeComponent();
        }

        private void FMViewCategories_Load(object sender, EventArgs e)
        {
            LoadCategoriesGrid();
        }

        private void LoadCategoriesGrid(string searchKeyword = "")
        {

            try
            {
                using (SqlConnection connect = new SqlConnection(DB_Salon.connectionString))
                {
                    connect.Open();
                    string query = @"SELECT CategoryName, Description FROM Categories";

                    if (!string.IsNullOrWhiteSpace(searchKeyword))
                        query += " WHERE LEFT(UPPER(CategoryName), 1) = UPPER(@Search)";

                    query += " ORDER BY CategoryName";

                    using (SqlCommand cmd = new SqlCommand(query, connect))
                    {
                        if (!string.IsNullOrWhiteSpace(searchKeyword))
                            cmd.Parameters.AddWithValue("@Search", searchKeyword.Substring(0, 1));

                        SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                        DataTable data = new DataTable();
                        adapter.Fill(data);


                        dataCategories.DataSource = data;
                    }
                }
            }
            catch (Exception)
            {
                MessageBox.Show("We’re having trouble loading the product list. " + "Please make sure the product data is available and try again.", "Load Products", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void txtSearchCategories_TextChanged(object sender, EventArgs e)
        {
            LoadCategoriesGrid(txtSearchCategories.Text.Trim());
        }

        private void dataCategories_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            //Delete
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                if (e.RowIndex >= 0)
                {

                    if (dataCategories.Columns[e.ColumnIndex].HeaderText == "Remove")
                    {
                        string categoryName = dataCategories.Rows[e.RowIndex].Cells["CategoryName"].Value.ToString();
                        string description = dataCategories.Rows[e.RowIndex].Cells["Description"].Value.ToString();

                        DialogResult confirm = MessageBox.Show($"Are you sure you want to delete '{categoryName}'?", "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                        if (confirm == DialogResult.Yes)
                        {
                            try
                            {
                                using (SqlConnection connect = new SqlConnection(DB_Salon.connectionString))
                                {
                                    connect.Open();
                                    string query = "DELETE FROM Categories WHERE CategoryName = @CategoryName";
                                    using (SqlCommand cmd = new SqlCommand(query, connect))
                                    {
                                        cmd.Parameters.AddWithValue("@CategoryName", categoryName);
                                        cmd.ExecuteNonQuery();
                                    }
                                }

                                MessageBox.Show("Product deleted successfully.", "Delete Product", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                LoadCategoriesGrid();

                            }
                            catch
                            {
                                MessageBox.Show("Something went wrong while deleting the product. Please try again.", "Delete Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            }
                        }
                    }
                }
                //Edit
                if (dataCategories.Columns[e.ColumnIndex].HeaderText == "Edit")
                {
                    string CategoryName = Convert.ToString(dataCategories.Rows[e.RowIndex].Cells["CategoryName"].Value.ToString());
                    string description = Convert.ToString(dataCategories.Rows[e.RowIndex].Cells["Description"].Value.ToString());

                    FMEditCategory editCategory = new FMEditCategory(CategoryName, description);
                    DialogResult result = editCategory.ShowDialog();

                    if (result == DialogResult.OK)
                    {
                        LoadCategoriesGrid();
                    }
                }
            }
        }

        private void pcClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}

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
    public partial class FMEditServices : Form
    {

        int originalCategoryID;
        decimal servicePrice;
        string categoryName, serviceName ,serviceDescription;

        private void FMEditServices_Load(object sender, EventArgs e)
        {
            LoadFilterCategories();
            GetOriginalCategoryID();
            txtServiceName.Text = serviceName;
            int categoryIndex = cbCategory.Items.IndexOf(categoryName);
            if (categoryIndex != -1)
            {
                cbCategory.SelectedIndex = categoryIndex;
            }
            else
            {
                cbCategory.Text = categoryName;
            }
            rtbDescription.Text = serviceDescription;
            txtServicePrice.Text = servicePrice.ToString("0.00");
            //serviceName = txtServiceName.Text.Trim();
            //categoryName = cbCategory.SelectedItem != null ? cbCategory.SelectedItem.ToString() : "";
            //decimal.TryParse(txtServicePrice.Text, out servicePrice);
        }

        private void GetOriginalCategoryID()
        {
            try
            {
                using (SqlConnection connect = new SqlConnection(DB_Salon.connectionString))
                {
                    connect.Open();
                    string query = "SELECT CategoryID FROM Categories WHERE CategoryName = @CategoryName";
                    using (SqlCommand cmd = new SqlCommand(query, connect))
                    {
                        cmd.Parameters.AddWithValue("@CategoryName", categoryName);
                        object result = cmd.ExecuteScalar();
                        if (result != null)
                        {
                            originalCategoryID = Convert.ToInt32(result);
                        }
                    }
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Error loading category ID.", "Load Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        public FMEditServices(string CategoryName, string ServiceName, string ServiceDesription, decimal ServicePrice)
        {
            InitializeComponent();
            categoryName = CategoryName;
            serviceName = ServiceName;
            servicePrice = ServicePrice;
            serviceDescription = ServiceDesription;
        }

        private void btnCancelEdit_Click(object sender, EventArgs e)
        {
            bool hasChanges = HasServiceChanges();

            if (hasChanges)
            {
                DialogResult result = MessageBox.Show("You have unsaved changes. Are you sure you want to cancel?", "Confirm Cancel", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    this.DialogResult = DialogResult.Cancel;
                    this.Close();
                }
            }
            else
            {
                this.DialogResult = DialogResult.Cancel;
                this.Close();
            }
        }

        private void btnSaveEdit_Click(object sender, EventArgs e)
        {
            if (!ValidateInputs())
            {
                return;
            }

            if (!HasServiceChanges())
            {
                MessageBox.Show("No changes were made to the service.", "No Changes", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            try
            {
                int newCategoryID = GetCategoryID(cbCategory.Text);

                using (SqlConnection connect = new SqlConnection(DB_Salon.connectionString))
                {
                    connect.Open();
                    string query = @"UPDATE Services SET ServiceName = @NewName, ServiceDescription = @NewDescription, ServicePrice = @Price, CategoryID = @CategoryID WHERE ServiceName = @OriginalName";

                    using (SqlCommand cmd = new SqlCommand(query, connect))
                    {
                        cmd.Parameters.AddWithValue("@NewName", txtServiceName.Text.Trim());
                        cmd.Parameters.AddWithValue("@Price", Convert.ToDecimal(txtServicePrice.Text));
                        cmd.Parameters.AddWithValue("NewDescription", rtbDescription.Text.Trim());
                        cmd.Parameters.AddWithValue("@CategoryID", newCategoryID);
                        cmd.Parameters.AddWithValue("@OriginalName", serviceName);
                        cmd.ExecuteNonQuery();
                    }
                }

                MessageBox.Show("Service updated successfully.", "Edit Service", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch
            {
                MessageBox.Show("Unable to update service. Please check your input and try again.", "Edit Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private int GetCategoryID(string categoryName)
        {
            try
            {
                using (SqlConnection connect = new SqlConnection(DB_Salon.connectionString))
                {
                    connect.Open();
                    string query = "SELECT CategoryID FROM Categories WHERE CategoryName = @CategoryName";
                    using (SqlCommand cmd = new SqlCommand(query, connect))
                    {
                        cmd.Parameters.AddWithValue("@CategoryName", categoryName);
                        object result = cmd.ExecuteScalar();
                        if (result != null)
                        {
                            return Convert.ToInt32(result);
                        }
                    }
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Error getting category ID.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            return originalCategoryID;
        }

        private bool ValidateInputs()
        {
            if (string.IsNullOrWhiteSpace(txtServiceName.Text))
            {
                MessageBox.Show("Service name is required.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtServiceName.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(cbCategory.Text) || cbCategory.Text == "All")
            {
                MessageBox.Show("Please select a valid category.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cbCategory.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(rtbDescription.Text))
            {
                MessageBox.Show("Service description is required.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtServiceName.Focus();
                return false;
            }

            decimal price;
            if (!decimal.TryParse(txtServicePrice.Text, out price))
            {
                MessageBox.Show("Please enter a valid price.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtServicePrice.Focus();
                return false;
            }

            if (price <= 0)
            {
                MessageBox.Show("Price must be greater than zero.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtServicePrice.Focus();
                return false;
            }

            return true;
        }

        private bool HasServiceChanges()
        {
            bool nameChanged = txtServiceName.Text.Trim() != serviceName;
            string currentCategory = cbCategory.SelectedItem != null ? cbCategory.SelectedItem.ToString() : "";
            bool categoryChanged = currentCategory != categoryName;
            bool descriptionChanged = rtbDescription.Text.Trim() != serviceDescription;

            decimal currentPrice = 0;
            decimal.TryParse(txtServicePrice.Text, out currentPrice);
            bool priceChanged = currentPrice != servicePrice;

            return nameChanged || categoryChanged || priceChanged || descriptionChanged;
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

                    cbCategory.Items.Clear();
                    cbCategory.Items.Add("All");
                    while (reader.Read())
                    {
                        cbCategory.Items.Add(reader["CategoryName"].ToString());
                    }
                    //cbCategory.SelectedIndex = 0;
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Error loading categories");
            }
        }
    }
}

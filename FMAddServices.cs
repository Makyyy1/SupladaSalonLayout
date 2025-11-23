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
    public partial class FMAddServices : Form
    {

        string serviceName, categoryName, serviceDescription;
        decimal servicePrice;

        public FMAddServices()
        {
            InitializeComponent();
            LoadCategories();
        }

        private void LoadCategories()
        {
            try
            {
                using (SqlConnection connect = new SqlConnection(DB_Salon.connectionString))
                {
                    connect.Open();
                    string query = "SELECT CategoryID, CategoryName FROM Categories ORDER BY CategoryName";
                    SqlCommand cmd = new SqlCommand(query, connect);
                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    DataTable data = new DataTable();
                    adapter.Fill(data);

                    cbCategoryName.DataSource = data;
                    cbCategoryName.DisplayMember = "CategoryName";
                    cbCategoryName.ValueMember = "CategoryID";
                    cbCategoryName.SelectedIndex = -1;
                   
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Error loading categories");
            }
        }

        private bool ServiceNameExists(string serviceName)
        {
            try
            {
                using (SqlConnection connect = new SqlConnection(DB_Salon.connectionString))
                {
                    connect.Open();
                    string query = "SELECT COUNT(*) FROM Services WHERE LOWER(REPLACE(ServiceName, ' ', '')) = LOWER(REPLACE(@ServiceName, ' ', ''))";
                    using (SqlCommand cmd = new SqlCommand(query, connect))
                    {
                        cmd.Parameters.AddWithValue("@ServiceName", serviceName);
                        int count = (int)cmd.ExecuteScalar();
                        return count > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error checking for duplicate service: " + ex.Message,
                    "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        private bool ValidateInputs()
        {
            if (cbCategoryName.SelectedIndex == -1)
            {
                MessageBox.Show("Please select a category.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cbCategoryName.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtServiceName.Text))
            {
                MessageBox.Show("Service name is required.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtServiceName.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(rtbDescription.Text))
            {
                MessageBox.Show("Service Description is required.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                rtbDescription.Focus();
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

        private bool HasProductChanges()
        {
            bool nameChanged = txtServiceName.Text.Trim() != serviceName;
            string currentCategory = cbCategoryName.SelectedItem != null ? cbCategoryName.SelectedItem.ToString() : "";
            bool categoryChanged = currentCategory != categoryName;
            bool descriptionChanged = rtbDescription.Text.Trim() != serviceDescription;

            decimal currentPrice = 0;
            decimal.TryParse(txtServicePrice.Text, out currentPrice);
            bool priceChanged = currentPrice != servicePrice;

            return nameChanged || categoryChanged || priceChanged || descriptionChanged;
        }

        private void FMAddServices_Load(object sender, EventArgs e)
        {
            serviceName = txtServiceName.Text.Trim();
            categoryName = cbCategoryName.SelectedItem != null ? cbCategoryName.SelectedItem.ToString() : "";
            decimal.TryParse(txtServicePrice.Text, out servicePrice);
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            bool hasChanges = HasProductChanges();

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

        private void btnAddService_Click(object sender, EventArgs e)
        {
            if (!ValidateInputs())
            {
                return;
            }

            if (ServiceNameExists(txtServiceName.Text.Trim()))
            {
                MessageBox.Show("A service with this name already exists. Please use a different name.",
                    "Duplicate Service", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                using (SqlConnection connect = new SqlConnection(DB_Salon.connectionString))
                {
                    connect.Open();
                    string query = "INSERT INTO Services (ServiceName, ServiceDescription,ServicePrice, CategoryID) VALUES (@ServiceName, @ServiceDescription, @ServicePrice, @CategoryID)";

                    using (SqlCommand cmd = new SqlCommand(query, connect))
                    {
                        cmd.Parameters.AddWithValue("@ServiceName", txtServiceName.Text.Trim());
                        cmd.Parameters.AddWithValue("@ServicePrice", Convert.ToDecimal(txtServicePrice.Text));
                        cmd.Parameters.AddWithValue("@ServiceDescription", rtbDescription.Text.Trim());
                        cmd.Parameters.AddWithValue("@CategoryID", cbCategoryName.SelectedValue);
                        cmd.ExecuteNonQuery();
                    }
                }

                MessageBox.Show("Service added successfully.", "Add Service", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception)
            {
                MessageBox.Show("Unable to add service. Please check your input and try again.", "Add Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }
}

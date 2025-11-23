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

namespace SupladaSalonLayout
{
    public partial class FMEditProducts : Form
    {

        decimal productPrice;
        string productName, productDescription;

        private void FMEditProducts_Load(object sender, EventArgs e)
        {
            txtProductName.Text = productName;
            richTxtDescription.Text = productDescription;
            txtProductPrice.Text = productPrice.ToString("0.00");
        }

        private bool HasProductChanges()
        {
            bool nameChanged = txtProductName.Text.Trim() != productName;
            bool descriptionChanged = richTxtDescription.Text.Trim() != productDescription;

            decimal currentPrice = 0;
            decimal.TryParse(txtProductPrice.Text, out currentPrice);
            bool priceChanged = currentPrice != productPrice;

            return nameChanged || descriptionChanged || priceChanged;
        }

        private void btnCancelEdit_Click(object sender, EventArgs e)
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

        private void btnSaveEdit_Click(object sender, EventArgs e)
        {
            try
            {
                using (SqlConnection connect = new SqlConnection(DB_Salon.connectionString))
                {
                    connect.Open();
                    string query = @"UPDATE Products SET ProductName = @Name, ProductDescription = @Desc, ProductPrice = @Price WHERE ProductName = @ProductName";

                    using (SqlCommand cmd = new SqlCommand(query, connect))
                    {
                        cmd.Parameters.AddWithValue("@Name", txtProductName.Text.Trim());
                        cmd.Parameters.AddWithValue("@Desc", richTxtDescription.Text.Trim());
                        cmd.Parameters.AddWithValue("@Price", Convert.ToDecimal(txtProductPrice.Text));
                        cmd.Parameters.AddWithValue("@ProductName", productName);
                        cmd.ExecuteNonQuery();
                    }
                }

                MessageBox.Show("Product updated successfully.", "Edit Product", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch
            {
                MessageBox.Show("Unable to update product. Please check your input and try again.", "Edit Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        public FMEditProducts(string ProductName, string ProductDescription, decimal ProductPrice)
        {
            InitializeComponent();
            productPrice = ProductPrice;
            productName = ProductName;
            productDescription = ProductDescription;
        }
    }
}

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
    public partial class FMEditDiscounts : Form
    {
        decimal discountAmount;
        string discountName, discountDescription;

        private void FMEditDiscounts_Load(object sender, EventArgs e)
        {
            txtDiscountName.Text = discountName;
            rtbDiscountDiscription.Text = discountDescription;
            txtDiscountAmount.Text = discountAmount.ToString("0.00");
        }

        public FMEditDiscounts(string DiscountName, string DiscountDescription, decimal DiscountAmount)
        {
            InitializeComponent();
            discountAmount = DiscountAmount;
            discountName = DiscountName;
            discountDescription = DiscountDescription;
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
                    string query = @"UPDATE Discounts SET DiscountName = @Name, DiscountDescription = @Desc, DiscountAmount = @Amount WHERE DiscountName = @DiscountName";

                    using (SqlCommand cmd = new SqlCommand(query, connect))
                    {
                        cmd.Parameters.AddWithValue("@Name", txtDiscountName.Text.Trim());
                        cmd.Parameters.AddWithValue("@Desc", rtbDiscountDiscription.Text.Trim());
                        cmd.Parameters.AddWithValue("@Amount", Convert.ToDecimal(txtDiscountAmount.Text));
                        cmd.Parameters.AddWithValue("@DiscountName", discountName);
                        cmd.ExecuteNonQuery();
                    }
                }

                MessageBox.Show("Discount updated successfully.", "Discount Product", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch
            {
                MessageBox.Show("Unable to update discount. Please check your input and try again.", "Edit Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private bool HasProductChanges()
        {
            bool nameChanged = txtDiscountName.Text.Trim() != discountName;
            bool descriptionChanged = rtbDiscountDiscription.Text.Trim() != discountDescription;

            decimal currentPrice = 0;
            decimal.TryParse(txtDiscountAmount.Text, out currentPrice);
            bool priceChanged = currentPrice != discountAmount;

            return nameChanged || descriptionChanged || priceChanged;
        }
    }
}

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
    public partial class FMEditCategory : Form
    {

        string categoryName, Description;

        private void FMEditCategory_Load(object sender, EventArgs e)
        {
            txtCategoryName.Text = categoryName;
            richTxtDescription.Text = Description;
        }

        private bool HasCategoryChanges()
        {
            bool nameChanged = txtCategoryName.Text.Trim() != categoryName;
            bool descriptionChanged = richTxtDescription.Text.Trim() != Description;

            return nameChanged || descriptionChanged;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            bool hasChanges = HasCategoryChanges();

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

        private void btnSaveCategory_Click(object sender, EventArgs e)
        {
            try
            {
                using (SqlConnection connect = new SqlConnection(DB_Salon.connectionString))
                {
                    connect.Open();
                    string query = @"UPDATE Categories SET CategoryName = @Name, Description = @Desc WHERE CategoryName = @CategoryName";

                    using (SqlCommand cmd = new SqlCommand(query, connect))
                    {
                        cmd.Parameters.AddWithValue("@Name", txtCategoryName.Text.Trim());
                        cmd.Parameters.AddWithValue("@Desc", richTxtDescription.Text.Trim());
                        cmd.Parameters.AddWithValue("@CategoryName", categoryName);
                        cmd.ExecuteNonQuery();
                    }
                }

                MessageBox.Show("Category updated successfully.", "Edit", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch
            {
                MessageBox.Show("Unable to update category. Please check your input and try again.", "Edit Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        public FMEditCategory(string CategoryName, string description)
        {
            InitializeComponent();
            categoryName = CategoryName;
            Description = description;
        }
    }
}

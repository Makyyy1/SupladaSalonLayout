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
    public partial class FMAddRoles : Form
    {

        string roleName, roleDescription;

        private bool HasProductChanges()
        {
            bool nameChanged = txtRoleName.Text.Trim() != roleName;
            bool descriptionChanged = richTxtDescription.Text.Trim() != roleDescription;


            return nameChanged || descriptionChanged;
        }

        private void ResetFields()
        {
            txtRoleName.Clear();
            richTxtDescription.Clear();
            roleName = "";
            roleDescription = "";
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

        private void FMAddRoles_Load(object sender, EventArgs e)
        {
            roleName = txtRoleName.Text.Trim();
            roleDescription = richTxtDescription.Text.Trim();
            LoadRolesGrid();
        }

        private bool ValidateInputs()
        {

            if (string.IsNullOrWhiteSpace(txtRoleName.Text))
            {
                MessageBox.Show("Role Name is required.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtRoleName.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(richTxtDescription.Text))
            {
                MessageBox.Show("Role Description is required.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                richTxtDescription.Focus();
                return false;
            }

            return true;
        }

        private bool RoleNameExists(string roleName)
        {
            try
            {
                using (SqlConnection connect = new SqlConnection(DB_Salon.connectionString))
                {
                    connect.Open();
                    string query = "SELECT COUNT(*) FROM TechnicianRoles WHERE LOWER(REPLACE(RoleName, ' ', '')) = LOWER(REPLACE(@RoleName, ' ', ''))";
                    using (SqlCommand cmd = new SqlCommand(query, connect))
                    {
                        cmd.Parameters.AddWithValue("@RoleName", roleName);
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

        private void btnSaveRole_Click(object sender, EventArgs e)
        {
            if (!ValidateInputs())
            {
                return;
            }

            if (RoleNameExists(txtRoleName.Text.Trim()))
            {
                MessageBox.Show("A role with this name already exists. Please use a different name.",
                    "Duplicate Role", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                using (SqlConnection connect = new SqlConnection(DB_Salon.connectionString))
                {
                    connect.Open();
                    string query = "INSERT INTO TechnicianRoles (RoleName, RoleDescription) VALUES (@RoleName, @RoleDescription)";

                    using (SqlCommand cmd = new SqlCommand(query, connect))
                    {
                        cmd.Parameters.AddWithValue("@RoleName", txtRoleName.Text.Trim());
                        cmd.Parameters.AddWithValue("@RoleDescription", richTxtDescription.Text.Trim());
                        cmd.ExecuteNonQuery();
                    }
                }

                MessageBox.Show("Role added successfully.", "Add Role", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.DialogResult = DialogResult.OK;
                LoadRolesGrid();
                ResetFields();
            }
            catch (Exception)
            {
                MessageBox.Show("Unable to add role. Please check your input and try again.", "Add Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void LoadRolesGrid()
        {
            try
            {
                using (SqlConnection connect = new SqlConnection(DB_Salon.connectionString))
                {
                    connect.Open();
                    string query = @"SELECT RoleName, RoleDescription FROM TechnicianRoles";

                    query += " ORDER BY RoleName";

                    using (SqlCommand cmd = new SqlCommand(query, connect))
                    {

                        SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                        DataTable data = new DataTable();
                        adapter.Fill(data);


                        dataRoles.DataSource = data;
                    }
                }
            }
            catch (Exception)
            {
                MessageBox.Show("We’re having trouble loading the role list. " + "Please make sure the role data is available and try again.", "Load Services", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void dataRoles_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            //Delete
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                if (dataRoles.Columns[e.ColumnIndex].HeaderText == "Remove")
                {
                    string roleNameToDelete = dataRoles.Rows[e.RowIndex].Cells["RoleNames"].Value.ToString();

                    DialogResult confirm = MessageBox.Show($"Are you sure you want to delete the role '{roleNameToDelete}'?",
                        "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                    if (confirm == DialogResult.Yes)
                    {
                        try
                        {
                            using (SqlConnection connect = new SqlConnection(DB_Salon.connectionString))
                            {
                                connect.Open();
                                string query = "DELETE FROM TechnicianRoles WHERE RoleName = @RoleName";
                                using (SqlCommand cmd = new SqlCommand(query, connect))
                                {
                                    cmd.Parameters.AddWithValue("@RoleName", roleNameToDelete);
                                    cmd.ExecuteNonQuery();
                                }
                            }

                            MessageBox.Show("Role deleted successfully.", "Delete Role",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                            LoadRolesGrid();
                            this.DialogResult = DialogResult.OK;
                        }
                        catch (SqlException ex)
                        {
                            if (ex.Number == 547)
                            {
                                MessageBox.Show("Cannot delete this role because it is currently assigned to one or more technicians. " +
                                    "Please reassign or remove those technicians first.",
                                    "Delete Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            }
                            else
                            {
                                MessageBox.Show("Something went wrong while deleting the role. Please try again.",
                                    "Delete Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            }
                        }
                    }
                }

                if (dataRoles.Columns[e.ColumnIndex].HeaderText == "Edit")
                {
                    string roleNameToEdit = dataRoles.Rows[e.RowIndex].Cells["RoleNames"].Value.ToString();
                    string descriptionToEdit = dataRoles.Rows[e.RowIndex].Cells["RoleDescriptions"].Value.ToString();

                    txtRoleName.Text = roleNameToEdit;
                    richTxtDescription.Text = descriptionToEdit;
                }
            }

        }

        public FMAddRoles()
        {
            InitializeComponent();
        }
    }
}
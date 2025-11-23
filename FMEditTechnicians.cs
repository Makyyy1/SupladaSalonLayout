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
using System.Xml.Linq;

namespace SupladaSalonLayout
{
    public partial class FMEditTechnicians : Form
    {

        private string originalTechnicianCode;
        private string originalTechnicianName;

        private void FMEditTechnicians_Load(object sender, EventArgs e)
        {
        }

        public FMEditTechnicians(string techCode, string roleName, string technicianName, string technicianNumber, string technicianAddress, string gender)
        {
            InitializeComponent();

            originalTechnicianCode = techCode;
            originalTechnicianName = technicianName;

            LoadRoles();
            PopulateFields(roleName, technicianName, technicianNumber, technicianAddress, gender);

        }

        private void LoadRoles()
        {
            try
            {
                using (SqlConnection connect = new SqlConnection(DB_Salon.connectionString))
                {
                    connect.Open();
                    string query = "SELECT RoleID, RoleName FROM TechnicianRoles ORDER BY RoleName";
                    SqlCommand cmd = new SqlCommand(query, connect);
                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    DataTable data = new DataTable();
                    adapter.Fill(data);

                    cbTechRole.DataSource = data;
                    cbTechRole.DisplayMember = "RoleName";
                    cbTechRole.ValueMember = "RoleID";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading technician roles: " + ex.Message, "Load Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void PopulateFields(string roleName, string technicianName, string technicianNumber, string technicianAddress, string gender)
        {
            txtTechName.Text = technicianName;
            txtTechContact.Text = technicianNumber;
            txtTechAddress.Text = technicianAddress;

            if (gender == "Male")
                rbMale.Checked = true;
            else if (gender == "Female")
                rbFemale.Checked = true;

            for (int i = 0; i < cbTechRole.Items.Count; i++)
            {
                DataRowView row = (DataRowView)cbTechRole.Items[i];
                if (row["RoleName"].ToString() == roleName)
                {
                    cbTechRole.SelectedIndex = i;
                    break;
                }
            }
        }

        private bool ValidateInputs()
        {
            if (string.IsNullOrWhiteSpace(txtTechName.Text))
            {
                MessageBox.Show("Technician name is required.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtTechName.Focus();
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

        private void btnSaveEdit_Click(object sender, EventArgs e)
        {
            if (!ValidateInputs())
            {
                return;
            }

            string gender = rbMale.Checked ? "Male" : "Female";

            try
            {
                using (SqlConnection connect = new SqlConnection(DB_Salon.connectionString))
                {
                    connect.Open();
                    string query = @"UPDATE Technicians 
                                   SET TechnicianName = @Name, 
                                       Gender = @Gender, 
                                       TechnicianNumber = @Number, 
                                       TechnicianAddress = @Address, 
                                       RoleID = @RoleID 
                                   WHERE TechnicianCode = @TechCode";

                    using (SqlCommand cmd = new SqlCommand(query, connect))
                    {
                        cmd.Parameters.AddWithValue("@Name", txtTechName.Text.Trim());
                        cmd.Parameters.AddWithValue("@Gender", gender);
                        cmd.Parameters.AddWithValue("@Number", txtTechContact.Text.Trim());
                        cmd.Parameters.AddWithValue("@Address", txtTechAddress.Text.Trim());
                        cmd.Parameters.AddWithValue("@RoleID", cbTechRole.SelectedValue);
                        cmd.Parameters.AddWithValue("@TechCode", originalTechnicianCode);

                        int rowsAffected = cmd.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Technician updated successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            this.DialogResult = DialogResult.OK;
                            this.Close();
                        }
                        else
                        {
                            MessageBox.Show("No technician was updated. Please try again.", "Update Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Unable to update technician: " + ex.Message, "Update Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnCancelEdit_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        
        }
    }
}

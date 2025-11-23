using System;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace SupladaSalonLayout
{
    public partial class AdminAddCashier : Form
    {
        private int editUserID = -1;
        private bool isEditMode = false;

        public AdminAddCashier()
        {
            InitializeComponent();
            isEditMode = false;
            LoadSecurityQuestions();
        }

        public AdminAddCashier(int userID, string username, string fullName, string contactNumber, string address, string securityQuestion, string securityAnswer)
        {
            InitializeComponent();
            isEditMode = true;
            editUserID = userID;
            this.Text = "Edit Cashier User";
            btnSave.Text = "Update User";

            // Populate fields
            txtUsername.Text = username;
            txtFullName.Text = fullName;
            txtContactNumber.Text = contactNumber;
            txtAddress.Text = address;
            txtPassword.Text = ""; // Don't show password
            txtConfirmPassword.Text = "";
            lblPasswordNote.Visible = true;
            lblPasswordNote.Text = "Leave blank to keep current password";

            LoadSecurityQuestions();
            if (!string.IsNullOrEmpty(securityQuestion))
            {
                cmbSecurityQuestion.Text = securityQuestion;
            }
            txtSecurityAnswer.Text = securityAnswer;

            txtUsername.ReadOnly = true; // Username cannot be changed
        }

        private void LoadSecurityQuestions()
        {
            cmbSecurityQuestion.Items.Clear();
            cmbSecurityQuestion.Items.Add("What is your mother's maiden name?");
            cmbSecurityQuestion.Items.Add("What city were you born in?");
            cmbSecurityQuestion.Items.Add("What was the name of your first pet?");
            cmbSecurityQuestion.Items.Add("What was your childhood nickname?");
            cmbSecurityQuestion.Items.Add("What is the name of your favorite teacher?");
            cmbSecurityQuestion.Items.Add("What street did you grow up on?");
            cmbSecurityQuestion.Items.Add("What was your favorite food as a child?");
        }

        private bool ValidateInputs()
        {
            if (string.IsNullOrWhiteSpace(txtUsername.Text))
            {
                MessageBox.Show("Username is required.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtUsername.Focus();
                return false;
            }

            if (!isEditMode && string.IsNullOrWhiteSpace(txtPassword.Text))
            {
                MessageBox.Show("Password is required for new users.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtPassword.Focus();
                return false;
            }

            if (!string.IsNullOrWhiteSpace(txtPassword.Text))
            {
                if (txtPassword.Text.Length < 6)
                {
                    MessageBox.Show("Password must be at least 6 characters long.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtPassword.Focus();
                    return false;
                }

                if (txtPassword.Text != txtConfirmPassword.Text)
                {
                    MessageBox.Show("Passwords do not match.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtConfirmPassword.Focus();
                    return false;
                }
            }

            if (string.IsNullOrWhiteSpace(txtFullName.Text))
            {
                MessageBox.Show("Full name is required.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtFullName.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtContactNumber.Text))
            {
                MessageBox.Show("Contact number is required.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtContactNumber.Focus();
                return false;
            }

            if (txtContactNumber.Text.Length != 11 || !txtContactNumber.Text.All(char.IsDigit))
            {
                MessageBox.Show("Contact number must be exactly 11 digits.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtContactNumber.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtAddress.Text))
            {
                MessageBox.Show("Address is required.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtAddress.Focus();
                return false;
            }

            if (cmbSecurityQuestion.SelectedIndex == -1)
            {
                MessageBox.Show("Please select a security question.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cmbSecurityQuestion.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtSecurityAnswer.Text))
            {
                MessageBox.Show("Security answer is required.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtSecurityAnswer.Focus();
                return false;
            }

            return true;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (!ValidateInputs())
            {
                return;
            }

            try
            {
                using (SqlConnection connect = new SqlConnection(DB_Salon.connectionString))
                {
                    connect.Open();

                    if (isEditMode)
                    {
                        // Update existing user
                        string updateQuery = @"UPDATE Users 
                                            SET FullName = @FullName,
                                                ContactNumber = @ContactNumber,
                                                Address = @Address,
                                                SecurityQuestion = @SecurityQuestion,
                                                SecurityAnswer = @SecurityAnswer";

                        if (!string.IsNullOrWhiteSpace(txtPassword.Text))
                        {
                            updateQuery += ", Password = @Password";
                        }

                        updateQuery += " WHERE UserID = @UserID";

                        using (SqlCommand cmd = new SqlCommand(updateQuery, connect))
                        {
                            cmd.Parameters.AddWithValue("@FullName", txtFullName.Text.Trim());
                            cmd.Parameters.AddWithValue("@ContactNumber", txtContactNumber.Text.Trim());
                            cmd.Parameters.AddWithValue("@Address", txtAddress.Text.Trim());
                            cmd.Parameters.AddWithValue("@SecurityQuestion", cmbSecurityQuestion.Text);
                            cmd.Parameters.AddWithValue("@SecurityAnswer", txtSecurityAnswer.Text.Trim());
                            cmd.Parameters.AddWithValue("@UserID", editUserID);

                            if (!string.IsNullOrWhiteSpace(txtPassword.Text))
                            {
                                cmd.Parameters.AddWithValue("@Password", txtPassword.Text);
                            }

                            int rowsAffected = cmd.ExecuteNonQuery();
                            if (rowsAffected > 0)
                            {
                                MessageBox.Show("User updated successfully!", "Success",
                                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                                this.DialogResult = DialogResult.OK;
                                this.Close();
                            }
                        }
                    }
                    else
                    {
                        // Check if username already exists
                        string checkQuery = "SELECT COUNT(*) FROM Users WHERE Username = @Username";
                        using (SqlCommand checkCmd = new SqlCommand(checkQuery, connect))
                        {
                            checkCmd.Parameters.AddWithValue("@Username", txtUsername.Text.Trim());
                            int count = Convert.ToInt32(checkCmd.ExecuteScalar());
                            if (count > 0)
                            {
                                MessageBox.Show("Username already exists. Please choose a different username.", "Validation Error",
                                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                txtUsername.Focus();
                                return;
                            }
                        }

                        // Insert new user
                        string insertQuery = @"INSERT INTO Users 
                                            (Username, Password, Role, FullName, ContactNumber, Address, SecurityQuestion, SecurityAnswer)
                                            VALUES 
                                            (@Username, @Password, 'Cashier', @FullName, @ContactNumber, @Address, @SecurityQuestion, @SecurityAnswer)";

                        using (SqlCommand cmd = new SqlCommand(insertQuery, connect))
                        {
                            cmd.Parameters.AddWithValue("@Username", txtUsername.Text.Trim());
                            cmd.Parameters.AddWithValue("@Password", txtPassword.Text);
                            cmd.Parameters.AddWithValue("@FullName", txtFullName.Text.Trim());
                            cmd.Parameters.AddWithValue("@ContactNumber", txtContactNumber.Text.Trim());
                            cmd.Parameters.AddWithValue("@Address", txtAddress.Text.Trim());
                            cmd.Parameters.AddWithValue("@SecurityQuestion", cmbSecurityQuestion.Text);
                            cmd.Parameters.AddWithValue("@SecurityAnswer", txtSecurityAnswer.Text.Trim());

                            int rowsAffected = cmd.ExecuteNonQuery();
                            if (rowsAffected > 0)
                            {
                                MessageBox.Show("Cashier user created successfully!", "Success",
                                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                                this.DialogResult = DialogResult.OK;
                                this.Close();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error saving user: " + ex.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}


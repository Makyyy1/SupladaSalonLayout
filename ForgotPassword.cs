using System;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;

namespace SupladaSalonLayout
{
    public partial class ForgotPassword : Form
    {
        public ForgotPassword()
        {
            InitializeComponent();
            LoadSecurityQuestions();
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

        private void btnVerify_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text.Trim();
            string securityQuestion = cmbSecurityQuestion.Text;
            string securityAnswer = txtSecurityAnswer.Text.Trim();

            if (string.IsNullOrWhiteSpace(username))
            {
                MessageBox.Show("Please enter your username.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtUsername.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(securityQuestion))
            {
                MessageBox.Show("Please select a security question.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cmbSecurityQuestion.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(securityAnswer))
            {
                MessageBox.Show("Please enter your security answer.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtSecurityAnswer.Focus();
                return;
            }

            try
            {
                using (SqlConnection connect = new SqlConnection(DB_Salon.connectionString))
                {
                    connect.Open();
                    string query = @"SELECT UserID, SecurityQuestion, SecurityAnswer 
                                   FROM Users 
                                   WHERE Username = @Username 
                                   AND SecurityQuestion = @SecurityQuestion 
                                   AND SecurityAnswer = @SecurityAnswer";

                    using (SqlCommand cmd = new SqlCommand(query, connect))
                    {
                        cmd.Parameters.AddWithValue("@Username", username);
                        cmd.Parameters.AddWithValue("@SecurityQuestion", securityQuestion);
                        cmd.Parameters.AddWithValue("@SecurityAnswer", securityAnswer);

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                int userID = Convert.ToInt32(reader["UserID"]);
                                // Show password reset form
                                pnlResetPassword.Visible = true;
                                pnlSecurityQuestions.Visible = false;
                                this.Height = 350;
                                this.userID = userID;
                            }
                            else
                            {
                                MessageBox.Show("Invalid username, security question, or answer. Please try again.", 
                                    "Verification Failed", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error verifying security information: " + ex.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private int userID = -1;

        private void btnResetPassword_Click(object sender, EventArgs e)
        {
            string newPassword = txtNewPassword.Text;
            string confirmPassword = txtConfirmPassword.Text;

            if (string.IsNullOrWhiteSpace(newPassword))
            {
                MessageBox.Show("Please enter a new password.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtNewPassword.Focus();
                return;
            }

            if (newPassword.Length < 6)
            {
                MessageBox.Show("Password must be at least 6 characters long.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtNewPassword.Focus();
                return;
            }

            if (newPassword != confirmPassword)
            {
                MessageBox.Show("Passwords do not match.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtConfirmPassword.Focus();
                return;
            }

            try
            {
                using (SqlConnection connect = new SqlConnection(DB_Salon.connectionString))
                {
                    connect.Open();
                    string updateQuery = "UPDATE Users SET Password = @Password WHERE UserID = @UserID";

                    using (SqlCommand cmd = new SqlCommand(updateQuery, connect))
                    {
                        cmd.Parameters.AddWithValue("@Password", newPassword);
                        cmd.Parameters.AddWithValue("@UserID", userID);

                        int rowsAffected = cmd.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Password reset successfully! You can now login with your new password.", 
                                "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            this.DialogResult = DialogResult.OK;
                            this.Close();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error resetting password: " + ex.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void txtUsername_TextChanged(object sender, EventArgs e)
        {
            // Load security question for the username
            string username = txtUsername.Text.Trim();
            if (!string.IsNullOrWhiteSpace(username))
            {
                LoadSecurityQuestionForUser(username);
            }
        }

        private void LoadSecurityQuestionForUser(string username)
        {
            try
            {
                using (SqlConnection connect = new SqlConnection(DB_Salon.connectionString))
                {
                    connect.Open();
                    string query = "SELECT SecurityQuestion FROM Users WHERE Username = @Username";
                    using (SqlCommand cmd = new SqlCommand(query, connect))
                    {
                        cmd.Parameters.AddWithValue("@Username", username);
                        object result = cmd.ExecuteScalar();
                        if (result != null && result != DBNull.Value)
                        {
                            cmbSecurityQuestion.Text = result.ToString();
                        }
                    }
                }
            }
            catch
            {
                // Ignore errors
            }
        }
    }
}


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
    public partial class LoginForm : Form
    {
        public LoginForm()
        {
            InitializeComponent();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text;
            string password = txtPassword.Text;

            using (SqlConnection connect = new SqlConnection(DB_Salon.connectionString))
            {
                try
                {
                    connect.Open();
                    string query = "SELECT UserID, Role FROM Users WHERE Username = @user AND Password = @password";
                    SqlCommand cmd = new SqlCommand(query, connect);
                    cmd.Parameters.AddWithValue("@user", username);
                    cmd.Parameters.AddWithValue("@password", password);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            int userID = Convert.ToInt32(reader["UserID"]);
                            string role = reader["Role"].ToString();

                            if (role == "Admin")
                            {
                                MessageBox.Show("Login successful!");
                                AdminHomeDashboard adminForm = new AdminHomeDashboard(username, role, userID);
                                adminForm.Show();
                                this.Hide();
                            }
                            else if (role == "Cashier")
                            {
                                MessageBox.Show("Login successful!");
                                CashierHomeDashboard cashierForm = new CashierHomeDashboard(username, userID);
                                cashierForm.Show();
                                this.Hide();
                            }
                        }
                        else
                        {
                            MessageBox.Show("Invalid Username or Password.", "Login Failed", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            }
            /*
            AdminHomeDashboard OpenDash = new AdminHomeDashboard();
            OpenDash.Show();
            this.Hide();
            */
        }

        private void checkPass_CheckedChanged(object sender, EventArgs e)
        {
            if (checkPass.Checked)
            {
                txtPassword.UseSystemPasswordChar = false;
            }
            else
            {
                txtPassword.UseSystemPasswordChar = true;
            }
        }

        private void lnkForgotPassword_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            using (ForgotPassword forgotForm = new ForgotPassword())
            {
                forgotForm.ShowDialog(this);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}

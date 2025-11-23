using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;

namespace SupladaSalonLayout
{
    public partial class AdminManageUsers : Form
    {
        private int selectedUserID = -1;

        public AdminManageUsers()
        {
            InitializeComponent();
            InitializeDatabase();
            LoadUsers();
        }

        private void InitializeDatabase()
        {
            try
            {
                using (SqlConnection connect = new SqlConnection(DB_Salon.connectionString))
                {
                    connect.Open();
                    
                    // Check and add FullName column
                    string checkFullName = @"IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[Users]') AND name = 'FullName')
                                            BEGIN
                                                ALTER TABLE Users ADD FullName NVARCHAR(100) NULL;
                                            END";
                    
                    // Check and add ContactNumber column
                    string checkContactNumber = @"IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[Users]') AND name = 'ContactNumber')
                                                  BEGIN
                                                      ALTER TABLE Users ADD ContactNumber NVARCHAR(20) NULL;
                                                  END";
                    
                    // Check and add Address column
                    string checkAddress = @"IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[Users]') AND name = 'Address')
                                            BEGIN
                                                ALTER TABLE Users ADD Address NVARCHAR(255) NULL;
                                            END";
                    
                    // Check and add SecurityQuestion column
                    string checkSecurityQuestion = @"IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[Users]') AND name = 'SecurityQuestion')
                                                      BEGIN
                                                          ALTER TABLE Users ADD SecurityQuestion NVARCHAR(255) NULL;
                                                      END";
                    
                    // Check and add SecurityAnswer column
                    string checkSecurityAnswer = @"IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[Users]') AND name = 'SecurityAnswer')
                                                    BEGIN
                                                        ALTER TABLE Users ADD SecurityAnswer NVARCHAR(255) NULL;
                                                    END";
                    
                    using (SqlCommand cmd = new SqlCommand(checkFullName, connect))
                    {
                        cmd.ExecuteNonQuery();
                    }
                    
                    using (SqlCommand cmd = new SqlCommand(checkContactNumber, connect))
                    {
                        cmd.ExecuteNonQuery();
                    }
                    
                    using (SqlCommand cmd = new SqlCommand(checkAddress, connect))
                    {
                        cmd.ExecuteNonQuery();
                    }
                    
                    using (SqlCommand cmd = new SqlCommand(checkSecurityQuestion, connect))
                    {
                        cmd.ExecuteNonQuery();
                    }
                    
                    using (SqlCommand cmd = new SqlCommand(checkSecurityAnswer, connect))
                    {
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error initializing database: " + ex.Message, "Database Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void LoadUsers()
        {
            try
            {
                using (SqlConnection connect = new SqlConnection(DB_Salon.connectionString))
                {
                    connect.Open();
                    string query = @"SELECT 
                                    UserID,
                                    Username,
                                    FullName,
                                    ContactNumber,
                                    Address,
                                    Role
                                    FROM Users
                                    WHERE Role = 'Cashier'
                                    ORDER BY UserID DESC";

                    SqlCommand cmd = new SqlCommand(query, connect);
                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    DataTable data = new DataTable();
                    adapter.Fill(data);

                    dataUsers.DataSource = data;
                    
                    if (dataUsers.Columns.Contains("UserID"))
                    {
                        dataUsers.Columns["UserID"].Visible = false;
                    }

                    // Add Edit and Delete button columns
                    AddActionButtons();
                    ReorderColumns();
                    ApplyButtonStylesToAllRows();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading users: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void AddActionButtons()
        {
            // Remove existing button columns if they exist
            if (dataUsers.Columns.Contains("Edit"))
            {
                dataUsers.Columns.Remove("Edit");
            }
            if (dataUsers.Columns.Contains("Delete"))
            {
                dataUsers.Columns.Remove("Delete");
            }

            // Add Edit button column
            DataGridViewButtonColumn editColumn = new DataGridViewButtonColumn();
            editColumn.Name = "Edit";
            editColumn.HeaderText = "Edit";
            editColumn.Text = "Edit";
            editColumn.UseColumnTextForButtonValue = true;
            editColumn.Width = 80;
            editColumn.FlatStyle = FlatStyle.Flat;
            DataGridViewCellStyle editStyle = new DataGridViewCellStyle();
            editStyle.BackColor = Color.FromArgb(40, 167, 69);
            editStyle.ForeColor = Color.White;
            editStyle.SelectionBackColor = Color.FromArgb(40, 167, 69);
            editStyle.SelectionForeColor = Color.White;
            editStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            editColumn.DefaultCellStyle = editStyle;
            dataUsers.Columns.Add(editColumn);

            // Add Delete button column
            DataGridViewButtonColumn deleteColumn = new DataGridViewButtonColumn();
            deleteColumn.Name = "Delete";
            deleteColumn.HeaderText = "Delete";
            deleteColumn.Text = "Delete";
            deleteColumn.UseColumnTextForButtonValue = true;
            deleteColumn.Width = 80;
            deleteColumn.FlatStyle = FlatStyle.Flat;
            DataGridViewCellStyle deleteStyle = new DataGridViewCellStyle();
            deleteStyle.BackColor = Color.FromArgb(220, 53, 69);
            deleteStyle.ForeColor = Color.White;
            deleteStyle.SelectionBackColor = Color.FromArgb(220, 53, 69);
            deleteStyle.SelectionForeColor = Color.White;
            deleteStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            deleteColumn.DefaultCellStyle = deleteStyle;
            dataUsers.Columns.Add(deleteColumn);
        }

        private void ReorderColumns()
        {
            int displayIndex = 0;
            if (dataUsers.Columns.Contains("Username"))
                dataUsers.Columns["Username"].DisplayIndex = displayIndex++;
            if (dataUsers.Columns.Contains("FullName"))
                dataUsers.Columns["FullName"].DisplayIndex = displayIndex++;
            if (dataUsers.Columns.Contains("ContactNumber"))
                dataUsers.Columns["ContactNumber"].DisplayIndex = displayIndex++;
            if (dataUsers.Columns.Contains("Address"))
                dataUsers.Columns["Address"].DisplayIndex = displayIndex++;
            if (dataUsers.Columns.Contains("Role"))
                dataUsers.Columns["Role"].DisplayIndex = displayIndex++;
            if (dataUsers.Columns.Contains("Edit"))
                dataUsers.Columns["Edit"].DisplayIndex = displayIndex++;
            if (dataUsers.Columns.Contains("Delete"))
                dataUsers.Columns["Delete"].DisplayIndex = displayIndex++;
        }

        private void ApplyButtonStylesToAllRows()
        {
            foreach (DataGridViewRow row in dataUsers.Rows)
            {
                if (row.Cells["Edit"] != null)
                {
                    row.Cells["Edit"].Style.BackColor = Color.FromArgb(40, 167, 69);
                    row.Cells["Edit"].Style.ForeColor = Color.White;
                    row.Cells["Edit"].Style.SelectionBackColor = Color.FromArgb(40, 167, 69);
                    row.Cells["Edit"].Style.SelectionForeColor = Color.White;
                    row.Cells["Edit"].Value = "Edit";
                }
                if (row.Cells["Delete"] != null)
                {
                    row.Cells["Delete"].Style.BackColor = Color.FromArgb(220, 53, 69);
                    row.Cells["Delete"].Style.ForeColor = Color.White;
                    row.Cells["Delete"].Style.SelectionBackColor = Color.FromArgb(220, 53, 69);
                    row.Cells["Delete"].Style.SelectionForeColor = Color.White;
                    row.Cells["Delete"].Value = "Delete";
                }
            }
        }

        private void dataUsers_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                DataGridViewRow row = dataUsers.Rows[e.RowIndex];
                string columnName = dataUsers.Columns[e.ColumnIndex].Name;

                if (row.Cells["UserID"].Value != null)
                {
                    selectedUserID = Convert.ToInt32(row.Cells["UserID"].Value);
                }

                if (columnName == "Edit")
                {
                    EditUser(selectedUserID, row);
                }
                else if (columnName == "Delete")
                {
                    DeleteUser(selectedUserID, row);
                }
            }
        }

        private void EditUser(int userID, DataGridViewRow row)
        {
            if (userID == -1)
            {
                MessageBox.Show("Please select a user first.", "No Selection", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                // Get user details from database
                string username = row.Cells["Username"].Value?.ToString() ?? "";
                string fullName = row.Cells["FullName"].Value?.ToString() ?? "";
                string contactNumber = row.Cells["ContactNumber"].Value?.ToString() ?? "";
                string address = row.Cells["Address"].Value?.ToString() ?? "";
                string securityQuestion = "";
                string securityAnswer = "";

                using (SqlConnection connect = new SqlConnection(DB_Salon.connectionString))
                {
                    connect.Open();
                    string getQuery = "SELECT SecurityQuestion, SecurityAnswer FROM Users WHERE UserID = @UserID";
                    using (SqlCommand getCmd = new SqlCommand(getQuery, connect))
                    {
                        getCmd.Parameters.AddWithValue("@UserID", userID);
                        using (SqlDataReader reader = getCmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                securityQuestion = reader["SecurityQuestion"]?.ToString() ?? "";
                                securityAnswer = reader["SecurityAnswer"]?.ToString() ?? "";
                            }
                        }
                    }
                }

                // Open edit form
                using (AdminAddCashier editForm = new AdminAddCashier(userID, username, fullName, contactNumber, address, securityQuestion, securityAnswer))
                {
                    if (editForm.ShowDialog(this) == DialogResult.OK)
                    {
                        LoadUsers();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading user details: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void DeleteUser(int userID, DataGridViewRow row)
        {
            if (userID == -1)
            {
                MessageBox.Show("Please select a user first.", "No Selection", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                string username = row.Cells["Username"].Value?.ToString() ?? "";

                DialogResult confirm = MessageBox.Show(
                    $"Are you sure you want to delete user '{username}'?",
                    "Confirm Deletion",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (confirm == DialogResult.Yes)
                {
                    using (SqlConnection connect = new SqlConnection(DB_Salon.connectionString))
                    {
                        connect.Open();
                        string deleteQuery = "DELETE FROM Users WHERE UserID = @UserID";

                        using (SqlCommand cmd = new SqlCommand(deleteQuery, connect))
                        {
                            cmd.Parameters.AddWithValue("@UserID", userID);
                            int rowsAffected = cmd.ExecuteNonQuery();

                            if (rowsAffected > 0)
                            {
                                MessageBox.Show("User deleted successfully.", "Success",
                                    MessageBoxButtons.OK, MessageBoxIcon.Information);

                                LoadUsers();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error deleting user: " + ex.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnAddUser_Click(object sender, EventArgs e)
        {
            using (AdminAddCashier addForm = new AdminAddCashier())
            {
                if (addForm.ShowDialog(this) == DialogResult.OK)
                {
                    LoadUsers();
                }
            }
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            FilterUsers();
        }

        private void FilterUsers()
        {
            try
            {
                string searchText = txtSearch.Text.Trim();
                using (SqlConnection connect = new SqlConnection(DB_Salon.connectionString))
                {
                    connect.Open();
                    string query = @"SELECT 
                                    UserID,
                                    Username,
                                    FullName,
                                    ContactNumber,
                                    Address,
                                    Role
                                    FROM Users
                                    WHERE Role = 'Cashier'";

                    if (!string.IsNullOrEmpty(searchText))
                    {
                        query += " AND (Username LIKE @Search OR FullName LIKE @Search OR ContactNumber LIKE @Search)";
                    }

                    query += " ORDER BY UserID DESC";

                    SqlCommand cmd = new SqlCommand(query, connect);
                    if (!string.IsNullOrEmpty(searchText))
                    {
                        cmd.Parameters.AddWithValue("@Search", "%" + searchText + "%");
                    }

                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    DataTable data = new DataTable();
                    adapter.Fill(data);

                    dataUsers.DataSource = data;
                    
                    if (dataUsers.Columns.Contains("UserID"))
                    {
                        dataUsers.Columns["UserID"].Visible = false;
                    }

                    AddActionButtons();
                    ReorderColumns();
                    ApplyButtonStylesToAllRows();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error filtering users: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void pcClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}


using System;
using System.Data.SqlClient;

class Program
{
    static void Main()
    {
        string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\Admin\Desktop\SupladaSalonLayoutLatest\SupladaSalonLayoutLatest\Salon_DB.mdf;Integrated Security=True";

        try
        {
            using (SqlConnection connect = new SqlConnection(connectionString))
            {
                connect.Open();
                Console.WriteLine("Connected to database successfully.");

                // Check if Service Name column already exists
                string checkColumnQuery = @"SELECT COUNT(*) FROM INFORMATION_SCHEMA.COLUMNS
                                           WHERE TABLE_NAME = 'Appointments' AND COLUMN_NAME = 'Service Name'";

                using (SqlCommand checkCmd = new SqlCommand(checkColumnQuery, connect))
                {
                    int columnExists = (int)checkCmd.ExecuteScalar();

                    if (columnExists == 0)
                    {
                        Console.WriteLine("Adding Service Name column...");
                        // Add the new Service Name column
                        string addColumnQuery = "ALTER TABLE Appointments ADD [Service Name] NVARCHAR(MAX) NULL";
                        using (SqlCommand addCmd = new SqlCommand(addColumnQuery, connect))
                        {
                            addCmd.ExecuteNonQuery();
                        }
                        Console.WriteLine("Service Name column added successfully.");

                        // Copy data from Service1 to Service Name if Service1 exists
                        string checkService1Query = @"SELECT COUNT(*) FROM INFORMATION_SCHEMA.COLUMNS
                                                    WHERE TABLE_NAME = 'Appointments' AND COLUMN_NAME = 'Service1'";

                        using (SqlCommand checkService1Cmd = new SqlCommand(checkService1Query, connect))
                        {
                            int service1Exists = (int)checkService1Cmd.ExecuteScalar();

                            if (service1Exists > 0)
                            {
                                Console.WriteLine("Copying data from Service1 to Service Name...");
                                string copyDataQuery = "UPDATE Appointments SET [Service Name] = Service1 WHERE Service1 IS NOT NULL";
                                using (SqlCommand copyCmd = new SqlCommand(copyDataQuery, connect))
                                {
                                    copyCmd.ExecuteNonQuery();
                                }
                                Console.WriteLine("Data copied successfully.");

                                // Drop the old Service1 column
                                Console.WriteLine("Dropping Service1 column...");
                                string dropService1Query = "ALTER TABLE Appointments DROP COLUMN Service1";
                                using (SqlCommand dropCmd = new SqlCommand(dropService1Query, connect))
                                {
                                    dropCmd.ExecuteNonQuery();
                                }
                                Console.WriteLine("Service1 column dropped successfully.");
                            }
                        }

                        // Check and drop Service2 column if it exists
                        string checkService2Query = @"SELECT COUNT(*) FROM INFORMATION_SCHEMA.COLUMNS
                                                    WHERE TABLE_NAME = 'Appointments' AND COLUMN_NAME = 'Service2'";

                        using (SqlCommand checkService2Cmd = new SqlCommand(checkService2Query, connect))
                        {
                            int service2Exists = (int)checkService2Cmd.ExecuteScalar();

                            if (service2Exists > 0)
                            {
                                Console.WriteLine("Dropping Service2 column...");
                                string dropService2Query = "ALTER TABLE Appointments DROP COLUMN Service2";
                                using (SqlCommand dropCmd = new SqlCommand(dropService2Query, connect))
                                {
                                    dropCmd.ExecuteNonQuery();
                                }
                                Console.WriteLine("Service2 column dropped successfully.");
                            }
                        }
                    }
                    else
                    {
                        Console.WriteLine("Service Name column already exists. Skipping database update.");
                    }
                }

                Console.WriteLine("Database schema update completed successfully!");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error updating database: " + ex.Message);
        }

        Console.WriteLine("Press any key to exit...");
        Console.ReadKey();
    }
}

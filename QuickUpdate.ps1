# Simple Database Update Script
Add-Type -AssemblyName System.Data

$connectionString = "Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\Admin\Desktop\SupladaSalonLayoutLatest\SupladaSalonLayoutLatest\Salon_DB.mdf;Integrated Security=True"

try {
    $connection = New-Object System.Data.SqlClient.SqlConnection($connectionString)
    $connection.Open()
    Write-Host "Connected to database successfully."

    # Check if Service Name column already exists
    $checkColumnQuery = "SELECT COUNT(*) FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'Appointments' AND COLUMN_NAME = 'Service Name'"
    $checkCmd = New-Object System.Data.SqlClient.SqlCommand($checkColumnQuery, $connection)
    $columnExists = [int]$checkCmd.ExecuteScalar()
    
    if ($columnExists -eq 0) {
        Write-Host "Adding Service Name column..."
        
        # Add the new Service Name column
        $addColumnQuery = "ALTER TABLE Appointments ADD [Service Name] NVARCHAR(MAX) NULL"
        $addCmd = New-Object System.Data.SqlClient.SqlCommand($addColumnQuery, $connection)
        $addCmd.ExecuteNonQuery()
        Write-Host "Service Name column added successfully."
        
        # Check if Service1 exists and copy data
        $checkService1Query = "SELECT COUNT(*) FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'Appointments' AND COLUMN_NAME = 'Service1'"
        $checkService1Cmd = New-Object System.Data.SqlClient.SqlCommand($checkService1Query, $connection)
        $service1Exists = [int]$checkService1Cmd.ExecuteScalar()
        
        if ($service1Exists -gt 0) {
            Write-Host "Copying data from Service1 to Service Name..."
            $copyDataQuery = "UPDATE Appointments SET [Service Name] = Service1 WHERE Service1 IS NOT NULL"
            $copyCmd = New-Object System.Data.SqlClient.SqlCommand($copyDataQuery, $connection)
            $copyCmd.ExecuteNonQuery()
            Write-Host "Data copied successfully."
            
            # Drop the old Service1 column
            Write-Host "Dropping Service1 column..."
            $dropService1Query = "ALTER TABLE Appointments DROP COLUMN Service1"
            $dropCmd = New-Object System.Data.SqlClient.SqlCommand($dropService1Query, $connection)
            $dropCmd.ExecuteNonQuery()
            Write-Host "Service1 column dropped successfully."
        }
        
        # Check and drop Service2 column if it exists
        $checkService2Query = "SELECT COUNT(*) FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'Appointments' AND COLUMN_NAME = 'Service2'"
        $checkService2Cmd = New-Object System.Data.SqlClient.SqlCommand($checkService2Query, $connection)
        $service2Exists = [int]$checkService2Cmd.ExecuteScalar()
        
        if ($service2Exists -gt 0) {
            Write-Host "Dropping Service2 column..."
            $dropService2Query = "ALTER TABLE Appointments DROP COLUMN Service2"
            $dropCmd = New-Object System.Data.SqlClient.SqlCommand($dropService2Query, $connection)
            $dropCmd.ExecuteNonQuery()
            Write-Host "Service2 column dropped successfully."
        }
    } else {
        Write-Host "Service Name column already exists. Skipping database update."
    }
    
    Write-Host "Database schema update completed successfully!"
} catch {
    Write-Host "Error updating database: $($_.Exception.Message)"
} finally {
    if ($connection.State -eq 'Open') {
        $connection.Close()
    }
}

Write-Host "Database update script finished."

# Script to add 'On Queue' status to Appointments table CHECK constraint
$connectionString = "Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\Admin\Desktop\SupladaSalonLayoutLatest\SupladaSalonLayoutLatest\Salon_DB.mdf;Integrated Security=True"

try {
    $connection = New-Object System.Data.SqlClient.SqlConnection($connectionString)
    $connection.Open()
    Write-Host "Connected to database successfully."

    # Check current constraint
    $checkConstraintQuery = "SELECT name FROM sys.check_constraints WHERE parent_object_id = OBJECT_ID('Appointments') AND name LIKE '%Status%'"
    
    $checkCmd = New-Object System.Data.SqlClient.SqlCommand($checkConstraintQuery, $connection)
    $constraintName = $checkCmd.ExecuteScalar()
    
    if ($constraintName) {
        Write-Host "Found constraint: $constraintName"
        Write-Host "Dropping existing constraint..."
        
        # Drop existing constraint
        $dropConstraintQuery = "ALTER TABLE Appointments DROP CONSTRAINT [$constraintName]"
        $dropCmd = New-Object System.Data.SqlClient.SqlCommand($dropConstraintQuery, $connection)
        $dropCmd.ExecuteNonQuery()
        Write-Host "Constraint dropped successfully."
    }
    
    # Add new constraint with 'On Queue' status
    Write-Host "Adding new constraint with 'On Queue' status..."
    $addConstraintQuery = "ALTER TABLE Appointments ADD CONSTRAINT CK_Appointments_Status CHECK (Status IN ('Pending', 'Confirmed', 'Completed', 'Cancelled', 'Ready for Billing', 'On going', 'On Queue'))"
    
    $addCmd = New-Object System.Data.SqlClient.SqlCommand($addConstraintQuery, $connection)
    $addCmd.ExecuteNonQuery()
    Write-Host "New constraint added successfully with 'On Queue' status."
    
    $connection.Close()
    Write-Host "Database update completed successfully."
}
catch {
    Write-Host "Error: $($_.Exception.Message)"
    if ($connection.State -eq 'Open') {
        $connection.Close()
    }
}

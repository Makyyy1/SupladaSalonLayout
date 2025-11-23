using System;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;

namespace SupladaSalonLayout
{
    public partial class AppointmentReschedule : Form
    {
        private int appointmentID;
        private DateTime currentDate;
        private TimeSpan currentTime;
        private string customerName;

        public AppointmentReschedule(int appointmentID, string customerName, DateTime currentDate, TimeSpan currentTime)
        {
            InitializeComponent();
            this.appointmentID = appointmentID;
            this.customerName = customerName;
            this.currentDate = currentDate;
            this.currentTime = currentTime;

            // Set form title
            this.Text = $"Reschedule Appointment - {customerName}";

            // Initialize date and time pickers
            datePicker.Value = currentDate < DateTime.Today ? DateTime.Today : currentDate;
            timePicker.Value = DateTime.Today.Add(currentTime);
        }

        private void btnConfirm_Click(object sender, EventArgs e)
        {
            DateTime newDate = datePicker.Value.Date;
            TimeSpan newTime = timePicker.Value.TimeOfDay;

            // Validate if any changes were made
            bool dateChanged = newDate.Date != currentDate.Date;
            bool timeChanged = Math.Abs((newTime - currentTime).TotalMinutes) > 0.5; // Allow 30 seconds tolerance for time comparison

            if (!dateChanged && !timeChanged)
            {
                MessageBox.Show("No changes were made to the appointment date or time. Please select a new date or time to reschedule.",
                    "No Changes", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            // Validate business hours
            if (newTime.Hours < 9 || newTime.Hours >= 19)
            {
                MessageBox.Show("Appointment time must be between 9:00 AM and 7:00 PM.",
                    "Invalid Time", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                using (SqlConnection connect = new SqlConnection(DB_Salon.connectionString))
                {
                    connect.Open();
                    string updateQuery = @"UPDATE Appointments 
                                         SET AppointmentDate = @NewDate, 
                                             AppointmentTime = @NewTime 
                                         WHERE AppointmentID = @AppointmentID";

                    using (SqlCommand cmd = new SqlCommand(updateQuery, connect))
                    {
                        cmd.Parameters.AddWithValue("@NewDate", newDate);
                        cmd.Parameters.AddWithValue("@NewTime", newTime);
                        cmd.Parameters.AddWithValue("@AppointmentID", appointmentID);

                        int rowsAffected = cmd.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Appointment rescheduled successfully!", "Success",
                                MessageBoxButtons.OK, MessageBoxIcon.Information);
                            this.DialogResult = DialogResult.OK;
                            this.Close();
                        }
                        else
                        {
                            MessageBox.Show("Failed to reschedule appointment.", "Error",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error rescheduling appointment: " + ex.Message, "Error",
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


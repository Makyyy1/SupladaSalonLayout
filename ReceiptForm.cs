using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SupladaSalonLayout
{
    public partial class ReceiptForm : Form
    {
        public string CustomerName { get; set; }
        public string ContactNumber { get; set; }
        public string AppointmentDate { get; set; }
        public string AppointmentTime { get; set; }
        public string Service1 { get; set; }
        public string Service1Price { get; set; }
        public string Service2 { get; set; }
        public string Service2Price { get; set; }
        public string ProductName { get; set; }
        public string ProductPrice { get; set; }
        public string DiscountType { get; set; }
        public string DiscountAmount { get; set; }
        public string PaymentMode { get; set; }
        public string Subtotal { get; set; }
        public string Total { get; set; }
        public string TransactionDate { get; set; }


        public ReceiptForm()
        {
            InitializeComponent();
        }

        private void ReceiptForm_Load(object sender, EventArgs e)
        {
            DisplayReceipt();
        }

        private void DisplayReceipt()
        {
            lblCustomerName.Text = CustomerName;
            lblContactNumber.Text = ContactNumber;
            lblAppointmentDate.Text = AppointmentDate;
            lblAppointmentTime.Text = AppointmentTime;
            lblService1.Text = Service1;
            lblService1Price.Text = Service1Price;

            if (!string.IsNullOrEmpty(Service2))
            {
                lblService2.Text = Service2;
                lblService2Price.Text = Service2Price;
            }
            else
            {
                lblService2.Visible = false;
                lblService2Price.Visible = false;
            }

            if (!string.IsNullOrEmpty(ProductName))
            {
                lblProduct.Text = ProductName;
                lblProductPrice.Text = ProductPrice;
            }
            else
            {
                lblProduct.Visible = false;
                lblProductPrice.Visible = false;
            }

            if (!string.IsNullOrEmpty(DiscountType))
            {
                lblDiscount.Text = DiscountType;
                lblDiscountAmount.Text = DiscountAmount;
            }
            else
            {
                lblDiscount.Visible = false;
                lblDiscountAmount.Visible = false;
            }

            lblPaymentMode.Text = PaymentMode;
            lblSubtotal.Text = Subtotal;
            lblTotal.Text = Total;
            lblTransactionDate.Text = TransactionDate;
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            PrintReceipt();
        }

        private void pcClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void PrintReceipt()
        {
            try
            {
                PrintDocument printDoc = new PrintDocument();
                printDoc.PrintPage += new PrintPageEventHandler(printDoc_PrintPage);

                PrintDialog printDialog = new PrintDialog();
                printDialog.Document = printDoc;

                if (printDialog.ShowDialog() == DialogResult.OK)
                {
                    printDoc.Print();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error printing receipt: " + ex.Message,
                               "Print Error",
                               MessageBoxButtons.OK,
                               MessageBoxIcon.Error);
            }
        }

        private void printDoc_PrintPage(object sender, PrintPageEventArgs e)
        {
            Font titleFont = new Font("Arial", 16, FontStyle.Bold);
            Font headerFont = new Font("Arial", 12, FontStyle.Bold);
            Font regularFont = new Font("Arial", 10);
            Font totalFont = new Font("Arial", 14, FontStyle.Bold);

            int yPos = 50;
            int leftMargin = 50;

            // Title
            e.Graphics.DrawString("SUPLADA SALON", titleFont, Brushes.Black, leftMargin, yPos);
            yPos += 40;

            e.Graphics.DrawString("RECEIPT", headerFont, Brushes.Black, leftMargin, yPos);
            yPos += 30;

            // Transaction Date
            e.Graphics.DrawString($"Date: {TransactionDate}", regularFont, Brushes.Black, leftMargin, yPos);
            yPos += 40;

            // Customer Details
            e.Graphics.DrawString("CUSTOMER DETAILS", headerFont, Brushes.Black, leftMargin, yPos);
            yPos += 25;
            e.Graphics.DrawString($"Name: {CustomerName}", regularFont, Brushes.Black, leftMargin, yPos);
            yPos += 20;
            e.Graphics.DrawString($"Contact: {ContactNumber}", regularFont, Brushes.Black, leftMargin, yPos);
            yPos += 40;

            // Service Details
            e.Graphics.DrawString("SERVICES", headerFont, Brushes.Black, leftMargin, yPos);
            yPos += 25;
            e.Graphics.DrawString($"{Service1}", regularFont, Brushes.Black, leftMargin, yPos);
            e.Graphics.DrawString($"₱{Service1Price}", regularFont, Brushes.Black, leftMargin + 400, yPos);
            yPos += 20;

            if (!string.IsNullOrEmpty(Service2))
            {
                e.Graphics.DrawString($"{Service2}", regularFont, Brushes.Black, leftMargin, yPos);
                e.Graphics.DrawString($"₱{Service2Price}", regularFont, Brushes.Black, leftMargin + 400, yPos);
                yPos += 20;
            }

            if (!string.IsNullOrEmpty(ProductName))
            {
                yPos += 10;
                e.Graphics.DrawString($"Add-on: {ProductName}", regularFont, Brushes.Black, leftMargin, yPos);
                e.Graphics.DrawString($"₱{ProductPrice}", regularFont, Brushes.Black, leftMargin + 400, yPos);
                yPos += 20;
            }

            yPos += 20;
            e.Graphics.DrawString($"Subtotal:", regularFont, Brushes.Black, leftMargin + 300, yPos);
            e.Graphics.DrawString($"₱{Subtotal}", regularFont, Brushes.Black, leftMargin + 400, yPos);
            yPos += 20;

            if (!string.IsNullOrEmpty(DiscountType))
            {
                e.Graphics.DrawString($"Discount ({DiscountType}):", regularFont, Brushes.Black, leftMargin + 300, yPos);
                e.Graphics.DrawString($"-₱{DiscountAmount}", regularFont, Brushes.Black, leftMargin + 400, yPos);
                yPos += 20;
            }

            yPos += 10;
            e.Graphics.DrawLine(Pens.Black, leftMargin, yPos, leftMargin + 500, yPos);
            yPos += 20;

            e.Graphics.DrawString($"TOTAL:", totalFont, Brushes.Black, leftMargin + 300, yPos);
            e.Graphics.DrawString($"₱{Total}", totalFont, Brushes.Black, leftMargin + 400, yPos);
            yPos += 40;

            e.Graphics.DrawString($"Payment Mode: {PaymentMode}", regularFont, Brushes.Black, leftMargin, yPos);
            yPos += 40;

            e.Graphics.DrawString("Thank you for your business!", regularFont, Brushes.Black, leftMargin + 150, yPos);
        }
    }
}

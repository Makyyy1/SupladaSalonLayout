namespace SupladaSalonLayout
{
    partial class WalkInReceipt
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(WalkInReceipt));
            this.btnPrint = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.pcClose = new System.Windows.Forms.PictureBox();
            this.lblTransactionDate = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.lblProductPrice = new System.Windows.Forms.Label();
            this.lblTotal = new System.Windows.Forms.Label();
            this.label35 = new System.Windows.Forms.Label();
            this.lblDiscountAmount = new System.Windows.Forms.Label();
            this.lblDiscount = new System.Windows.Forms.Label();
            this.lblSubtotal = new System.Windows.Forms.Label();
            this.lblProduct = new System.Windows.Forms.Label();
            this.lblService1 = new System.Windows.Forms.Label();
            this.lblService1Price = new System.Windows.Forms.Label();
            this.label18 = new System.Windows.Forms.Label();
            this.label34 = new System.Windows.Forms.Label();
            this.lblService2 = new System.Windows.Forms.Label();
            this.lblService2Price = new System.Windows.Forms.Label();
            this.lblPaymentMode = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.printDocu = new System.Drawing.Printing.PrintDocument();
            ((System.ComponentModel.ISupportInitialize)(this.pcClose)).BeginInit();
            this.SuspendLayout();
            // 
            // btnPrint
            // 
            this.btnPrint.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(116)))), ((int)(((byte)(212)))), ((int)(((byte)(255)))));
            this.btnPrint.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnPrint.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnPrint.Location = new System.Drawing.Point(352, 457);
            this.btnPrint.Name = "btnPrint";
            this.btnPrint.Size = new System.Drawing.Size(54, 37);
            this.btnPrint.TabIndex = 113;
            this.btnPrint.Text = "Print";
            this.btnPrint.UseVisualStyleBackColor = false;
            this.btnPrint.Click += new System.EventHandler(this.btnPrint_Click);
            // 
            // label3
            // 
            this.label3.Font = new System.Drawing.Font("Calibri", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(253)))), ((int)(((byte)(159)))), ((int)(((byte)(43)))));
            this.label3.Location = new System.Drawing.Point(12, 60);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(469, 56);
            this.label3.TabIndex = 89;
            this.label3.Text = "___________________________________________________________________";
            // 
            // pcClose
            // 
            this.pcClose.Image = ((System.Drawing.Image)(resources.GetObject("pcClose.Image")));
            this.pcClose.Location = new System.Drawing.Point(446, 9);
            this.pcClose.Name = "pcClose";
            this.pcClose.Size = new System.Drawing.Size(29, 32);
            this.pcClose.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pcClose.TabIndex = 116;
            this.pcClose.TabStop = false;
            this.pcClose.Click += new System.EventHandler(this.pcClose_Click);
            // 
            // lblTransactionDate
            // 
            this.lblTransactionDate.AutoSize = true;
            this.lblTransactionDate.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTransactionDate.Location = new System.Drawing.Point(161, 60);
            this.lblTransactionDate.Name = "lblTransactionDate";
            this.lblTransactionDate.Size = new System.Drawing.Size(34, 19);
            this.lblTransactionDate.TabIndex = 115;
            this.lblTransactionDate.Text = "-----";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Calibri", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(143, 9);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(203, 39);
            this.label5.TabIndex = 114;
            this.label5.Text = "Suplada Salon";
            // 
            // lblProductPrice
            // 
            this.lblProductPrice.AutoSize = true;
            this.lblProductPrice.Font = new System.Drawing.Font("Calibri", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblProductPrice.Location = new System.Drawing.Point(380, 208);
            this.lblProductPrice.Name = "lblProductPrice";
            this.lblProductPrice.Size = new System.Drawing.Size(45, 23);
            this.lblProductPrice.TabIndex = 127;
            this.lblProductPrice.Text = "0.00";
            // 
            // lblTotal
            // 
            this.lblTotal.AutoSize = true;
            this.lblTotal.Font = new System.Drawing.Font("Calibri", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTotal.Location = new System.Drawing.Point(380, 405);
            this.lblTotal.Name = "lblTotal";
            this.lblTotal.Size = new System.Drawing.Size(45, 23);
            this.lblTotal.TabIndex = 126;
            this.lblTotal.Text = "0.00";
            // 
            // label35
            // 
            this.label35.AutoSize = true;
            this.label35.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label35.Location = new System.Drawing.Point(317, 408);
            this.label35.Name = "label35";
            this.label35.Size = new System.Drawing.Size(47, 19);
            this.label35.TabIndex = 125;
            this.label35.Text = "Total:";
            // 
            // lblDiscountAmount
            // 
            this.lblDiscountAmount.AutoSize = true;
            this.lblDiscountAmount.Font = new System.Drawing.Font("Calibri", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDiscountAmount.Location = new System.Drawing.Point(381, 321);
            this.lblDiscountAmount.Name = "lblDiscountAmount";
            this.lblDiscountAmount.Size = new System.Drawing.Size(45, 23);
            this.lblDiscountAmount.TabIndex = 123;
            this.lblDiscountAmount.Text = "0.00";
            // 
            // lblDiscount
            // 
            this.lblDiscount.AutoSize = true;
            this.lblDiscount.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDiscount.Location = new System.Drawing.Point(34, 321);
            this.lblDiscount.Name = "lblDiscount";
            this.lblDiscount.Size = new System.Drawing.Size(106, 19);
            this.lblDiscount.TabIndex = 122;
            this.lblDiscount.Text = "Discount Amt:";
            // 
            // lblSubtotal
            // 
            this.lblSubtotal.AutoSize = true;
            this.lblSubtotal.Font = new System.Drawing.Font("Calibri", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSubtotal.Location = new System.Drawing.Point(380, 272);
            this.lblSubtotal.Name = "lblSubtotal";
            this.lblSubtotal.Size = new System.Drawing.Size(45, 23);
            this.lblSubtotal.TabIndex = 121;
            this.lblSubtotal.Text = "0.00";
            // 
            // lblProduct
            // 
            this.lblProduct.AutoSize = true;
            this.lblProduct.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblProduct.Location = new System.Drawing.Point(34, 208);
            this.lblProduct.Name = "lblProduct";
            this.lblProduct.Size = new System.Drawing.Size(74, 19);
            this.lblProduct.TabIndex = 119;
            this.lblProduct.Text = "Products:";
            // 
            // lblService1
            // 
            this.lblService1.AutoSize = true;
            this.lblService1.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblService1.Location = new System.Drawing.Point(34, 116);
            this.lblService1.Name = "lblService1";
            this.lblService1.Size = new System.Drawing.Size(161, 19);
            this.lblService1.TabIndex = 120;
            this.lblService1.Text = "Availed Service Prices:";
            // 
            // lblService1Price
            // 
            this.lblService1Price.AutoSize = true;
            this.lblService1Price.Font = new System.Drawing.Font("Calibri", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblService1Price.Location = new System.Drawing.Point(381, 116);
            this.lblService1Price.Name = "lblService1Price";
            this.lblService1Price.Size = new System.Drawing.Size(45, 23);
            this.lblService1Price.TabIndex = 118;
            this.lblService1Price.Text = "0.00";
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label18.Location = new System.Drawing.Point(34, 272);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(73, 19);
            this.label18.TabIndex = 117;
            this.label18.Text = "SubTotal:";
            // 
            // label34
            // 
            this.label34.Font = new System.Drawing.Font("Calibri", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label34.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(253)))), ((int)(((byte)(159)))), ((int)(((byte)(43)))));
            this.label34.Location = new System.Drawing.Point(31, 340);
            this.label34.Name = "label34";
            this.label34.Size = new System.Drawing.Size(423, 56);
            this.label34.TabIndex = 124;
            this.label34.Text = "___________________________________________________________________";
            // 
            // lblService2
            // 
            this.lblService2.AutoSize = true;
            this.lblService2.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblService2.Location = new System.Drawing.Point(34, 163);
            this.lblService2.Name = "lblService2";
            this.lblService2.Size = new System.Drawing.Size(161, 19);
            this.lblService2.TabIndex = 129;
            this.lblService2.Text = "Availed Service Prices:";
            // 
            // lblService2Price
            // 
            this.lblService2Price.AutoSize = true;
            this.lblService2Price.Font = new System.Drawing.Font("Calibri", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblService2Price.Location = new System.Drawing.Point(381, 163);
            this.lblService2Price.Name = "lblService2Price";
            this.lblService2Price.Size = new System.Drawing.Size(45, 23);
            this.lblService2Price.TabIndex = 128;
            this.lblService2Price.Text = "0.00";
            // 
            // lblPaymentMode
            // 
            this.lblPaymentMode.AutoSize = true;
            this.lblPaymentMode.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPaymentMode.Location = new System.Drawing.Point(134, 442);
            this.lblPaymentMode.Name = "lblPaymentMode";
            this.lblPaymentMode.Size = new System.Drawing.Size(34, 19);
            this.lblPaymentMode.TabIndex = 131;
            this.lblPaymentMode.Text = "-----";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label13.Location = new System.Drawing.Point(89, 409);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(136, 19);
            this.label13.TabIndex = 130;
            this.label13.Text = "Mode of Payment:";
            // 
            // printDocu
            // 
            this.printDocu.PrintPage += new System.Drawing.Printing.PrintPageEventHandler(this.printDocu_PrintPage);
            // 
            // WalkInReceipt
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(487, 542);
            this.ControlBox = false;
            this.Controls.Add(this.lblPaymentMode);
            this.Controls.Add(this.label13);
            this.Controls.Add(this.lblService2);
            this.Controls.Add(this.lblService2Price);
            this.Controls.Add(this.lblProductPrice);
            this.Controls.Add(this.lblTotal);
            this.Controls.Add(this.label35);
            this.Controls.Add(this.lblDiscountAmount);
            this.Controls.Add(this.lblDiscount);
            this.Controls.Add(this.lblSubtotal);
            this.Controls.Add(this.lblProduct);
            this.Controls.Add(this.lblService1);
            this.Controls.Add(this.lblService1Price);
            this.Controls.Add(this.label18);
            this.Controls.Add(this.label34);
            this.Controls.Add(this.pcClose);
            this.Controls.Add(this.lblTransactionDate);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.btnPrint);
            this.Controls.Add(this.label3);
            this.Name = "WalkInReceipt";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Load += new System.EventHandler(this.WalkInReceipt_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pcClose)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnPrint;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.PictureBox pcClose;
        private System.Windows.Forms.Label lblTransactionDate;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label lblProductPrice;
        private System.Windows.Forms.Label lblTotal;
        private System.Windows.Forms.Label label35;
        private System.Windows.Forms.Label lblDiscountAmount;
        private System.Windows.Forms.Label lblDiscount;
        private System.Windows.Forms.Label lblSubtotal;
        private System.Windows.Forms.Label lblProduct;
        private System.Windows.Forms.Label lblService1;
        private System.Windows.Forms.Label lblService1Price;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.Label label34;
        private System.Windows.Forms.Label lblService2;
        private System.Windows.Forms.Label lblService2Price;
        private System.Windows.Forms.Label lblPaymentMode;
        private System.Windows.Forms.Label label13;
        private System.Drawing.Printing.PrintDocument printDocu;
    }
}
namespace POS
{
    partial class Form1
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
            this.components = new System.ComponentModel.Container();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.cartListBox = new System.Windows.Forms.ListBox();
            this.lblTotal = new System.Windows.Forms.Label();
            this.flowPanelItems = new System.Windows.Forms.FlowLayoutPanel();
            this.btnCheckout = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.clear = new System.Windows.Forms.Button();
            this.btnRemoveItem = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(61, 4);
            // 
            // cartListBox
            // 
            this.cartListBox.FormattingEnabled = true;
            this.cartListBox.ItemHeight = 16;
            this.cartListBox.Location = new System.Drawing.Point(1126, 97);
            this.cartListBox.Name = "cartListBox";
            this.cartListBox.Size = new System.Drawing.Size(350, 436);
            this.cartListBox.TabIndex = 10;
            // 
            // lblTotal
            // 
            this.lblTotal.Location = new System.Drawing.Point(1126, 559);
            this.lblTotal.Name = "lblTotal";
            this.lblTotal.Size = new System.Drawing.Size(243, 48);
            this.lblTotal.TabIndex = 11;
            this.lblTotal.Text = "Total: Rs.0";
            this.lblTotal.Click += new System.EventHandler(this.label1_Click);
            // 
            // flowPanelItems
            // 
            this.flowPanelItems.AutoScroll = true;
            this.flowPanelItems.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.flowPanelItems.Location = new System.Drawing.Point(12, 37);
            this.flowPanelItems.Name = "flowPanelItems";
            this.flowPanelItems.Size = new System.Drawing.Size(951, 670);
            this.flowPanelItems.TabIndex = 15;
            // 
            // btnCheckout
            // 
            this.btnCheckout.BackColor = System.Drawing.Color.LawnGreen;
            this.btnCheckout.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnCheckout.Location = new System.Drawing.Point(1129, 598);
            this.btnCheckout.Name = "btnCheckout";
            this.btnCheckout.Size = new System.Drawing.Size(132, 52);
            this.btnCheckout.TabIndex = 16;
            this.btnCheckout.Text = "Checkout";
            this.btnCheckout.UseVisualStyleBackColor = false;
            this.btnCheckout.Click += new System.EventHandler(this.btnCheckout_Click_1);
            // 
            // textBox1
            // 
            this.textBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F);
            this.textBox1.Location = new System.Drawing.Point(1126, 37);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(350, 63);
            this.textBox1.TabIndex = 17;
            this.textBox1.Text = "Cart Items";
            this.textBox1.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.textBox1.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            // 
            // clear
            // 
            this.clear.BackColor = System.Drawing.Color.Red;
            this.clear.Location = new System.Drawing.Point(1364, 598);
            this.clear.Name = "clear";
            this.clear.Size = new System.Drawing.Size(112, 52);
            this.clear.TabIndex = 19;
            this.clear.Text = "Clear";
            this.clear.UseVisualStyleBackColor = false;
            this.clear.Click += new System.EventHandler(this.clear_Click);
            // 
            // btnRemoveItem
            // 
            this.btnRemoveItem.BackColor = System.Drawing.Color.Red;
            this.btnRemoveItem.Location = new System.Drawing.Point(1364, 484);
            this.btnRemoveItem.Name = "btnRemoveItem";
            this.btnRemoveItem.Size = new System.Drawing.Size(93, 33);
            this.btnRemoveItem.TabIndex = 20;
            this.btnRemoveItem.Text = "Remove";
            this.btnRemoveItem.UseVisualStyleBackColor = false;
            this.btnRemoveItem.Click += new System.EventHandler(this.btnRemoveItem_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1601, 878);
            this.Controls.Add(this.btnRemoveItem);
            this.Controls.Add(this.clear);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.btnCheckout);
            this.Controls.Add(this.flowPanelItems);
            this.Controls.Add(this.lblTotal);
            this.Controls.Add(this.cartListBox);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ListBox cartListBox;
        private System.Windows.Forms.Label lblTotal;
        private System.Windows.Forms.FlowLayoutPanel flowPanelItems;
        private System.Windows.Forms.Button btnCheckout;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Button clear;
        private System.Windows.Forms.Button btnRemoveItem;
    }
}

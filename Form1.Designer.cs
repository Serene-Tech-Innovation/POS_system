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
            components = new System.ComponentModel.Container();
            contextMenuStrip1 = new ContextMenuStrip(components);
            cartListBox = new ListBox();
            lblTotal = new Label();
            flowPanelItems = new FlowLayoutPanel();
            btnCheckout = new Button();
            textBox1 = new TextBox();
            clear = new Button();
            btnRemoveItem = new Button();
            txtSearch = new TextBox();
            cmbSort = new ComboBox();
            cmbCategory = new ComboBox();
            cmbSubcategory = new ComboBox();
            numMinPrice = new NumericUpDown();
            numMaxPrice = new NumericUpDown();
            btnFilter = new Button();
            ((System.ComponentModel.ISupportInitialize)numMinPrice).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numMaxPrice).BeginInit();
            SuspendLayout();
            // 
            // contextMenuStrip1
            // 
            contextMenuStrip1.ImageScalingSize = new Size(20, 20);
            contextMenuStrip1.Name = "contextMenuStrip1";
            contextMenuStrip1.Size = new Size(61, 4);
            // 
            // cartListBox
            // 
            cartListBox.FormattingEnabled = true;
            cartListBox.Location = new Point(985, 91);
            cartListBox.Name = "cartListBox";
            cartListBox.Size = new Size(307, 409);
            cartListBox.TabIndex = 10;
            // 
            // lblTotal
            // 
            lblTotal.Location = new Point(985, 524);
            lblTotal.Name = "lblTotal";
            lblTotal.Size = new Size(213, 45);
            lblTotal.TabIndex = 11;
            lblTotal.Text = "Total: Rs.0";
            // 
            // flowPanelItems
            // 
            flowPanelItems.AutoScroll = true;
            flowPanelItems.BackColor = SystemColors.ButtonHighlight;
            flowPanelItems.Location = new Point(12, 91);
            flowPanelItems.Name = "flowPanelItems";
            flowPanelItems.Size = new Size(832, 628);
            flowPanelItems.TabIndex = 15;
            // 
            // btnCheckout
            // 
            btnCheckout.BackColor = Color.LawnGreen;
            btnCheckout.BackgroundImageLayout = ImageLayout.Center;
            btnCheckout.Location = new Point(988, 561);
            btnCheckout.Name = "btnCheckout";
            btnCheckout.Size = new Size(116, 49);
            btnCheckout.TabIndex = 16;
            btnCheckout.Text = "Checkout";
            btnCheckout.UseVisualStyleBackColor = false;
            btnCheckout.Click += btnCheckout_Click;
            // 
            // textBox1
            // 
            textBox1.Font = new Font("Microsoft Sans Serif", 18F);
            textBox1.Location = new Point(985, 35);
            textBox1.Multiline = true;
            textBox1.Name = "textBox1";
            textBox1.Size = new Size(307, 59);
            textBox1.TabIndex = 17;
            textBox1.Text = "Cart Items";
            textBox1.TextAlign = HorizontalAlignment.Center;
            // 
            // clear
            // 
            clear.BackColor = Color.Red;
            clear.Location = new Point(1194, 561);
            clear.Name = "clear";
            clear.Size = new Size(98, 49);
            clear.TabIndex = 19;
            clear.Text = "Clear";
            clear.UseVisualStyleBackColor = false;
            clear.Click += clear_Click;
            // 
            // btnRemoveItem
            // 
            btnRemoveItem.BackColor = Color.Red;
            btnRemoveItem.Location = new Point(1194, 454);
            btnRemoveItem.Name = "btnRemoveItem";
            btnRemoveItem.Size = new Size(81, 31);
            btnRemoveItem.TabIndex = 20;
            btnRemoveItem.Text = "Remove";
            btnRemoveItem.UseVisualStyleBackColor = false;
            btnRemoveItem.Click += btnRemoveItem_Click;
            // 
            // txtSearch
            // 
            txtSearch.Location = new Point(12, 62);
            txtSearch.Name = "txtSearch";
            txtSearch.Size = new Size(100, 23);
            txtSearch.TabIndex = 21;
            // 
            // cmbSort
            // 
            cmbSort.FormattingEnabled = true;
            cmbSort.Items.AddRange(new object[] { "Price Low to High", "Price High to Low", "A-Z", "Z-A" });
            cmbSort.Location = new Point(723, 62);
            cmbSort.Name = "cmbSort";
            cmbSort.Size = new Size(121, 23);
            cmbSort.TabIndex = 22;
            // 
            // cmbCategory
            // 
            cmbCategory.FormattingEnabled = true;
            cmbCategory.Location = new Point(469, 62);
            cmbCategory.Name = "cmbCategory";
            cmbCategory.Size = new Size(121, 23);
            cmbCategory.TabIndex = 23;
            // 
            // cmbSubcategory
            // 
            cmbSubcategory.FormattingEnabled = true;
            cmbSubcategory.Location = new Point(596, 62);
            cmbSubcategory.Name = "cmbSubcategory";
            cmbSubcategory.Size = new Size(121, 23);
            cmbSubcategory.TabIndex = 24;
            // 
            // numMinPrice
            // 
            numMinPrice.Location = new Point(217, 62);
            numMinPrice.Name = "numMinPrice";
            numMinPrice.Size = new Size(120, 23);
            numMinPrice.TabIndex = 25;
            // 
            // numMaxPrice
            // 
            numMaxPrice.Location = new Point(343, 63);
            numMaxPrice.Maximum = new decimal(new int[] { 10000, 0, 0, 0 });
            numMaxPrice.Name = "numMaxPrice";
            numMaxPrice.Size = new Size(120, 23);
            numMaxPrice.TabIndex = 26;
            // 
            // btnFilter
            // 
            btnFilter.Location = new Point(118, 63);
            btnFilter.Name = "btnFilter";
            btnFilter.Size = new Size(75, 23);
            btnFilter.TabIndex = 27;
            btnFilter.Text = "button1";
            btnFilter.UseVisualStyleBackColor = true;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1401, 823);
            Controls.Add(btnFilter);
            Controls.Add(numMaxPrice);
            Controls.Add(numMinPrice);
            Controls.Add(cmbSubcategory);
            Controls.Add(cmbCategory);
            Controls.Add(cmbSort);
            Controls.Add(txtSearch);
            Controls.Add(btnRemoveItem);
            Controls.Add(clear);
            Controls.Add(textBox1);
            Controls.Add(btnCheckout);
            Controls.Add(flowPanelItems);
            Controls.Add(lblTotal);
            Controls.Add(cartListBox);
            Name = "Form1";
            Text = "Form1";
            Load += Form1_Load;
            ((System.ComponentModel.ISupportInitialize)numMinPrice).EndInit();
            ((System.ComponentModel.ISupportInitialize)numMaxPrice).EndInit();
            ResumeLayout(false);
            PerformLayout();

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
        private TextBox txtSearch;
        private ComboBox cmbSort;
        private ComboBox cmbCategory;
        private ComboBox cmbSubcategory;
        private NumericUpDown numMinPrice;
        private NumericUpDown numMaxPrice;
        private Button btnFilter;
    }
}

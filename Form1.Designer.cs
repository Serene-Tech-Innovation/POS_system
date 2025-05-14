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
            cartListBox.Location = new Point(1126, 121);
            cartListBox.Margin = new Padding(3, 4, 3, 4);
            cartListBox.Name = "cartListBox";
            cartListBox.Size = new Size(350, 824);
            cartListBox.TabIndex = 10;
            // 
            // lblTotal
            // 
            lblTotal.Location = new Point(1122, 967);
            lblTotal.Name = "lblTotal";
            lblTotal.Size = new Size(243, 60);
            lblTotal.TabIndex = 11;
            lblTotal.Text = "Total: Rs.0";
            // 
            // flowPanelItems
            // 
            flowPanelItems.AutoScroll = true;
            flowPanelItems.BackColor = SystemColors.ButtonHighlight;
            flowPanelItems.Location = new Point(14, 121);
            flowPanelItems.Margin = new Padding(3, 4, 3, 4);
            flowPanelItems.Name = "flowPanelItems";
            flowPanelItems.Size = new Size(951, 960);
            flowPanelItems.TabIndex = 15;
            // 
            // btnCheckout
            // 
            btnCheckout.BackColor = Color.LawnGreen;
            btnCheckout.BackgroundImageLayout = ImageLayout.Center;
            btnCheckout.Location = new Point(1126, 1016);
            btnCheckout.Margin = new Padding(3, 4, 3, 4);
            btnCheckout.Name = "btnCheckout";
            btnCheckout.Size = new Size(133, 65);
            btnCheckout.TabIndex = 16;
            btnCheckout.Text = "Checkout";
            btnCheckout.UseVisualStyleBackColor = false;
            btnCheckout.Click += btnCheckout_Click;
            // 
            // textBox1
            // 
            textBox1.Font = new Font("Microsoft Sans Serif", 18F);
            textBox1.Location = new Point(1126, 47);
            textBox1.Margin = new Padding(3, 4, 3, 4);
            textBox1.Multiline = true;
            textBox1.Name = "textBox1";
            textBox1.Size = new Size(350, 77);
            textBox1.TabIndex = 17;
            textBox1.Text = "Cart Items";
            textBox1.TextAlign = HorizontalAlignment.Center;
            // 
            // clear
            // 
            clear.BackColor = Color.Red;
            clear.Location = new Point(1361, 1016);
            clear.Margin = new Padding(3, 4, 3, 4);
            clear.Name = "clear";
            clear.Size = new Size(112, 65);
            clear.TabIndex = 19;
            clear.Text = "Clear";
            clear.UseVisualStyleBackColor = false;
            clear.Click += clear_Click;
            // 
            // btnRemoveItem
            // 
            btnRemoveItem.BackColor = Color.Red;
            btnRemoveItem.Location = new Point(1384, 905);
            btnRemoveItem.Margin = new Padding(3, 4, 3, 4);
            btnRemoveItem.Name = "btnRemoveItem";
            btnRemoveItem.Size = new Size(93, 41);
            btnRemoveItem.TabIndex = 20;
            btnRemoveItem.Text = "Remove";
            btnRemoveItem.UseVisualStyleBackColor = false;
            btnRemoveItem.Click += btnRemoveItem_Click;
            // 
            // txtSearch
            // 
            txtSearch.Location = new Point(14, 83);
            txtSearch.Margin = new Padding(3, 4, 3, 4);
            txtSearch.Name = "txtSearch";
            txtSearch.PlaceholderText = "Search";
            txtSearch.Size = new Size(114, 27);
            txtSearch.TabIndex = 21;
            txtSearch.TextChanged += txtSearch_TextChanged;
            // 
            // cmbSort
            // 
            cmbSort.FormattingEnabled = true;
            cmbSort.Items.AddRange(new object[] { "Price Low to High", "Price High to Low", "A-Z", "Z-A" });
            cmbSort.Location = new Point(725, 82);
            cmbSort.Margin = new Padding(3, 4, 3, 4);
            cmbSort.Name = "cmbSort";
            cmbSort.Size = new Size(138, 28);
            cmbSort.TabIndex = 22;
            cmbSort.Text = "Price Range";
            cmbSort.SelectedIndexChanged += cmbSort_SelectedIndexChanged;
            // 
            // cmbCategory
            // 
            cmbCategory.FormattingEnabled = true;
            cmbCategory.Location = new Point(435, 82);
            cmbCategory.Margin = new Padding(3, 4, 3, 4);
            cmbCategory.Name = "cmbCategory";
            cmbCategory.Size = new Size(138, 28);
            cmbCategory.TabIndex = 23;
            cmbCategory.Text = "Main";
            cmbCategory.SelectedIndexChanged += cmbCategory_SelectedIndexChanged;
            // 
            // cmbSubcategory
            // 
            cmbSubcategory.FormattingEnabled = true;
            cmbSubcategory.Location = new Point(580, 82);
            cmbSubcategory.Margin = new Padding(3, 4, 3, 4);
            cmbSubcategory.Name = "cmbSubcategory";
            cmbSubcategory.Size = new Size(138, 28);
            cmbSubcategory.TabIndex = 24;
            cmbSubcategory.Text = "Sub Category";
            cmbSubcategory.SelectedIndexChanged += cmbSubcategory_SelectedIndexChanged;
            // 
            // numMinPrice
            // 
            numMinPrice.Location = new Point(147, 82);
            numMinPrice.Margin = new Padding(3, 4, 3, 4);
            numMinPrice.Maximum = new decimal(new int[] { 10000, 0, 0, 0 });
            numMinPrice.Name = "numMinPrice";
            numMinPrice.Size = new Size(137, 27);
            numMinPrice.TabIndex = 25;
            numMinPrice.ThousandsSeparator = true;
            numMinPrice.ValueChanged += numMinPrice_ValueChanged;
            // 
            // numMaxPrice
            // 
            numMaxPrice.Location = new Point(291, 83);
            numMaxPrice.Margin = new Padding(3, 4, 3, 4);
            numMaxPrice.Maximum = new decimal(new int[] { 10000, 0, 0, 0 });
            numMaxPrice.Name = "numMaxPrice";
            numMaxPrice.Size = new Size(137, 27);
            numMaxPrice.TabIndex = 26;
            numMaxPrice.Value = new decimal(new int[] { 10000, 0, 0, 0 });
            numMaxPrice.ValueChanged += numMaxPrice_ValueChanged;
            // 
            // btnFilter
            // 
            btnFilter.BackColor = Color.FromArgb(255, 128, 0);
            btnFilter.Location = new Point(880, 82);
            btnFilter.Margin = new Padding(3, 4, 3, 4);
            btnFilter.Name = "btnFilter";
            btnFilter.Size = new Size(86, 32);
            btnFilter.TabIndex = 27;
            btnFilter.Text = "Filter";
            btnFilter.UseVisualStyleBackColor = false;
            btnFilter.Click += btnFilter_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1601, 1097);
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
            Margin = new Padding(3, 4, 3, 4);
            Name = "Form1";
            Text = "Form1";
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

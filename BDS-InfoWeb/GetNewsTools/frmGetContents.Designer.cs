namespace GetNewsTools
{
    partial class frmGetNews
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
            this.pnlBrowser = new System.Windows.Forms.Panel();
            this.cboUrls = new System.Windows.Forms.ComboBox();
            this.lblUrls = new System.Windows.Forms.Label();
            this.btnGoTo = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // pnlBrowser
            // 
            this.pnlBrowser.Location = new System.Drawing.Point(13, 57);
            this.pnlBrowser.Margin = new System.Windows.Forms.Padding(4);
            this.pnlBrowser.Name = "pnlBrowser";
            this.pnlBrowser.Size = new System.Drawing.Size(928, 412);
            this.pnlBrowser.TabIndex = 0;
            // 
            // cboUrls
            // 
            this.cboUrls.DisplayMember = "Value";
            this.cboUrls.FormattingEnabled = true;
            this.cboUrls.Location = new System.Drawing.Point(108, 13);
            this.cboUrls.Margin = new System.Windows.Forms.Padding(4);
            this.cboUrls.Name = "cboUrls";
            this.cboUrls.Size = new System.Drawing.Size(696, 24);
            this.cboUrls.TabIndex = 1;
            this.cboUrls.SelectedIndexChanged += new System.EventHandler(this.cboUrls_SelectedIndexChanged);
            // 
            // lblUrls
            // 
            this.lblUrls.AutoSize = true;
            this.lblUrls.Location = new System.Drawing.Point(16, 16);
            this.lblUrls.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblUrls.Name = "lblUrls";
            this.lblUrls.Size = new System.Drawing.Size(84, 16);
            this.lblUrls.TabIndex = 2;
            this.lblUrls.Text = "Source URL:";
            // 
            // btnGoTo
            // 
            this.btnGoTo.BackColor = System.Drawing.SystemColors.HotTrack;
            this.btnGoTo.Location = new System.Drawing.Point(811, 7);
            this.btnGoTo.Name = "btnGoTo";
            this.btnGoTo.Size = new System.Drawing.Size(85, 34);
            this.btnGoTo.TabIndex = 3;
            this.btnGoTo.Text = "Go";
            this.btnGoTo.UseVisualStyleBackColor = false;
            this.btnGoTo.Click += new System.EventHandler(this.btnGoTo_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.Blue;
            this.label1.Location = new System.Drawing.Point(12, 475);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(171, 16);
            this.label1.TabIndex = 4;
            this.label1.Text = "Total Records Inserted:";
            // 
            // frmGetNews
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(948, 497);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnGoTo);
            this.Controls.Add(this.lblUrls);
            this.Controls.Add(this.cboUrls);
            this.Controls.Add(this.pnlBrowser);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmGetNews";
            this.Text = "Reading HTML Contents";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel pnlBrowser;
        private System.Windows.Forms.ComboBox cboUrls;
        private System.Windows.Forms.Label lblUrls;
        private System.Windows.Forms.Button btnGoTo;
        private System.Windows.Forms.Label label1;
    }
}


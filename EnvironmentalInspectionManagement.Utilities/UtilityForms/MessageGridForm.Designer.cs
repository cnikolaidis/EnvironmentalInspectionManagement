namespace EnvironmentalInspectionManagement.Utilities.UtilityForms
{
    partial class MessageGridForm
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
            this.FormDataGrid = new System.Windows.Forms.DataGridView();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.captionLbl = new System.Windows.Forms.Label();
            this.okBtn = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.FormDataGrid)).BeginInit();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // FormDataGrid
            // 
            this.FormDataGrid.AllowUserToAddRows = false;
            this.FormDataGrid.AllowUserToDeleteRows = false;
            this.FormDataGrid.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.FormDataGrid.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.FormDataGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.FormDataGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.FormDataGrid.Location = new System.Drawing.Point(3, 43);
            this.FormDataGrid.Name = "FormDataGrid";
            this.FormDataGrid.ReadOnly = true;
            this.FormDataGrid.RowHeadersVisible = false;
            this.FormDataGrid.Size = new System.Drawing.Size(811, 366);
            this.FormDataGrid.TabIndex = 3;
            this.FormDataGrid.MouseDown += new System.Windows.Forms.MouseEventHandler(this.DataGridViewMouseDown);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.captionLbl, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.FormDataGrid, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.okBtn, 0, 2);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 8.672087F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 80.7588F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 10.2981F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(817, 460);
            this.tableLayoutPanel1.TabIndex = 2;
            // 
            // captionLbl
            // 
            this.captionLbl.AutoSize = true;
            this.captionLbl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.captionLbl.Font = new System.Drawing.Font("Arial", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(161)));
            this.captionLbl.Location = new System.Drawing.Point(3, 0);
            this.captionLbl.Name = "captionLbl";
            this.captionLbl.Size = new System.Drawing.Size(811, 40);
            this.captionLbl.TabIndex = 2;
            this.captionLbl.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // okBtn
            // 
            this.okBtn.Dock = System.Windows.Forms.DockStyle.Fill;
            this.okBtn.Location = new System.Drawing.Point(3, 415);
            this.okBtn.Name = "okBtn";
            this.okBtn.Size = new System.Drawing.Size(811, 42);
            this.okBtn.TabIndex = 4;
            this.okBtn.Text = "OK";
            this.okBtn.UseVisualStyleBackColor = true;
            this.okBtn.Click += new System.EventHandler(this.ButtonClicked);
            // 
            // MessageGridForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(817, 460);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "MessageGridForm";
            this.Text = "MessageGridForm";
            this.Load += new System.EventHandler(this.FormLoad);
            ((System.ComponentModel.ISupportInitialize)(this.FormDataGrid)).EndInit();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView FormDataGrid;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label captionLbl;
        private System.Windows.Forms.Button okBtn;
    }
}
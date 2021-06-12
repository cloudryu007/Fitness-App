namespace Fitness_App.Screens
{
    partial class Workouts
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
            this.headerBorder = new System.Windows.Forms.Panel();
            this.routineCB = new System.Windows.Forms.ComboBox();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.flowPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // headerBorder
            // 
            this.headerBorder.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(37)))), ((int)(((byte)(39)))), ((int)(((byte)(77)))));
            this.headerBorder.Dock = System.Windows.Forms.DockStyle.Top;
            this.headerBorder.Location = new System.Drawing.Point(0, 0);
            this.headerBorder.Name = "headerBorder";
            this.headerBorder.Size = new System.Drawing.Size(782, 2);
            this.headerBorder.TabIndex = 3;
            // 
            // routineCB
            // 
            this.routineCB.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.routineCB.BackColor = System.Drawing.Color.Gainsboro;
            this.routineCB.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.routineCB.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.routineCB.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.routineCB.ForeColor = System.Drawing.Color.Black;
            this.routineCB.FormattingEnabled = true;
            this.routineCB.Location = new System.Drawing.Point(277, 5);
            this.routineCB.Name = "routineCB";
            this.routineCB.Size = new System.Drawing.Size(228, 23);
            this.routineCB.TabIndex = 5;
            this.routineCB.SelectedIndexChanged += new System.EventHandler(this.routineCB_SelectedIndexChanged);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.routineCB, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.flowPanel, 0, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 2);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 7.352941F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 92.64706F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(782, 476);
            this.tableLayoutPanel1.TabIndex = 6;
            // 
            // flowPanel
            // 
            this.flowPanel.AutoScroll = true;
            this.flowPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowPanel.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.flowPanel.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.flowPanel.Location = new System.Drawing.Point(3, 37);
            this.flowPanel.Name = "flowPanel";
            this.flowPanel.Size = new System.Drawing.Size(776, 436);
            this.flowPanel.TabIndex = 6;
            this.flowPanel.WrapContents = false;
            // 
            // Workouts
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(782, 478);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.headerBorder);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "Workouts";
            this.Text = "``";
            this.Load += new System.EventHandler(this.Workouts_Load);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Panel headerBorder;
        private System.Windows.Forms.ComboBox routineCB;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.FlowLayoutPanel flowPanel;
    }
}
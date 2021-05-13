namespace Fitness_App.Screens
{
    partial class Routines
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
            this.dataPanel = new System.Windows.Forms.Panel();
            this.routineLayout = new System.Windows.Forms.FlowLayoutPanel();
            this.addRoutineBtn = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // headerBorder
            // 
            this.headerBorder.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(37)))), ((int)(((byte)(39)))), ((int)(((byte)(77)))));
            this.headerBorder.Dock = System.Windows.Forms.DockStyle.Top;
            this.headerBorder.Location = new System.Drawing.Point(0, 0);
            this.headerBorder.Name = "headerBorder";
            this.headerBorder.Size = new System.Drawing.Size(782, 2);
            this.headerBorder.TabIndex = 2;
            // 
            // dataPanel
            // 
            this.dataPanel.BackColor = System.Drawing.Color.White;
            this.dataPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataPanel.Location = new System.Drawing.Point(146, 2);
            this.dataPanel.Name = "dataPanel";
            this.dataPanel.Size = new System.Drawing.Size(636, 476);
            this.dataPanel.TabIndex = 7;
            // 
            // routineLayout
            // 
            this.routineLayout.AutoScroll = true;
            this.routineLayout.BackColor = System.Drawing.Color.White;
            this.routineLayout.Dock = System.Windows.Forms.DockStyle.Fill;
            this.routineLayout.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.routineLayout.Location = new System.Drawing.Point(0, 0);
            this.routineLayout.Name = "routineLayout";
            this.routineLayout.Size = new System.Drawing.Size(146, 446);
            this.routineLayout.TabIndex = 8;
            this.routineLayout.WrapContents = false;
            // 
            // addRoutineBtn
            // 
            this.addRoutineBtn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(37)))), ((int)(((byte)(39)))), ((int)(((byte)(77)))));
            this.addRoutineBtn.FlatAppearance.BorderSize = 0;
            this.addRoutineBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.addRoutineBtn.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.addRoutineBtn.ForeColor = System.Drawing.Color.White;
            this.addRoutineBtn.Location = new System.Drawing.Point(4, 3);
            this.addRoutineBtn.Name = "addRoutineBtn";
            this.addRoutineBtn.Padding = new System.Windows.Forms.Padding(10, 0, 0, 0);
            this.addRoutineBtn.Size = new System.Drawing.Size(136, 25);
            this.addRoutineBtn.TabIndex = 7;
            this.addRoutineBtn.Text = "Add New Routine";
            this.addRoutineBtn.UseVisualStyleBackColor = false;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.White;
            this.panel1.Controls.Add(this.routineLayout);
            this.panel1.Controls.Add(this.panel2);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel1.Location = new System.Drawing.Point(0, 2);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(146, 476);
            this.panel1.TabIndex = 8;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.addRoutineBtn);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel2.Location = new System.Drawing.Point(0, 446);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(146, 30);
            this.panel2.TabIndex = 0;
            // 
            // Routines
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(782, 478);
            this.Controls.Add(this.dataPanel);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.headerBorder);
            this.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "Routines";
            this.Text = "Routines";
            this.panel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel headerBorder;
        private System.Windows.Forms.Panel dataPanel;
        private System.Windows.Forms.Button addRoutineBtn;
        private System.Windows.Forms.FlowLayoutPanel routineLayout;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
    }
}
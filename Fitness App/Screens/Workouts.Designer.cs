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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle8 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            this.dataPanel = new System.Windows.Forms.TableLayoutPanel();
            this.exLbl = new System.Windows.Forms.Label();
            this.workoutGrid = new System.Windows.Forms.DataGridView();
            this.setColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.repColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.weightColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.goalReps = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.repWeight = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.titlePanel = new System.Windows.Forms.TableLayoutPanel();
            this.routineCB = new System.Windows.Forms.ComboBox();
            this.headerBorder = new System.Windows.Forms.Panel();
            this.exercisePanel = new System.Windows.Forms.FlowLayoutPanel();
            this.centerPanel = new System.Windows.Forms.Panel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.dataPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.workoutGrid)).BeginInit();
            this.titlePanel.SuspendLayout();
            this.centerPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // dataPanel
            // 
            this.dataPanel.ColumnCount = 1;
            this.dataPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.dataPanel.Controls.Add(this.exLbl, 0, 0);
            this.dataPanel.Controls.Add(this.workoutGrid, 0, 1);
            this.dataPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataPanel.Location = new System.Drawing.Point(0, 63);
            this.dataPanel.Name = "dataPanel";
            this.dataPanel.RowCount = 2;
            this.dataPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 12.5F));
            this.dataPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 87.5F));
            this.dataPanel.Size = new System.Drawing.Size(636, 413);
            this.dataPanel.TabIndex = 15;
            // 
            // exLbl
            // 
            this.exLbl.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.exLbl.AutoSize = true;
            this.exLbl.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.exLbl.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(37)))), ((int)(((byte)(39)))), ((int)(((byte)(77)))));
            this.exLbl.Location = new System.Drawing.Point(269, 17);
            this.exLbl.Name = "exLbl";
            this.exLbl.Size = new System.Drawing.Size(97, 17);
            this.exLbl.TabIndex = 4;
            this.exLbl.Text = "Exercise Name";
            // 
            // workoutGrid
            // 
            this.workoutGrid.AllowUserToAddRows = false;
            this.workoutGrid.AllowUserToDeleteRows = false;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(188)))), ((int)(((byte)(220)))), ((int)(((byte)(244)))));
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.ActiveCaption;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.Color.Black;
            this.workoutGrid.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.workoutGrid.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.workoutGrid.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.workoutGrid.BackgroundColor = System.Drawing.Color.White;
            this.workoutGrid.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.workoutGrid.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
            this.workoutGrid.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.ActiveCaption;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.workoutGrid.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.workoutGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.workoutGrid.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.setColumn,
            this.repColumn,
            this.weightColumn,
            this.goalReps,
            this.repWeight});
            this.workoutGrid.Cursor = System.Windows.Forms.Cursors.Default;
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle6.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle6.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle6.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle6.SelectionBackColor = System.Drawing.SystemColors.ActiveCaption;
            dataGridViewCellStyle6.SelectionForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle6.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.workoutGrid.DefaultCellStyle = dataGridViewCellStyle6;
            this.workoutGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.workoutGrid.GridColor = System.Drawing.Color.White;
            this.workoutGrid.Location = new System.Drawing.Point(3, 54);
            this.workoutGrid.Name = "workoutGrid";
            this.workoutGrid.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle7.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle7.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle7.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle7.SelectionBackColor = System.Drawing.SystemColors.ActiveCaption;
            dataGridViewCellStyle7.SelectionForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle7.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.workoutGrid.RowHeadersDefaultCellStyle = dataGridViewCellStyle7;
            dataGridViewCellStyle8.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle8.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle8.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle8.SelectionBackColor = System.Drawing.SystemColors.ActiveCaption;
            dataGridViewCellStyle8.SelectionForeColor = System.Drawing.Color.Black;
            this.workoutGrid.RowsDefaultCellStyle = dataGridViewCellStyle8;
            this.workoutGrid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.workoutGrid.Size = new System.Drawing.Size(630, 356);
            this.workoutGrid.TabIndex = 6;
            this.workoutGrid.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.workoutGrid_CellFormatting);
            this.workoutGrid.CellPainting += new System.Windows.Forms.DataGridViewCellPaintingEventHandler(this.workoutGrid_CellPainting);
            this.workoutGrid.EditingControlShowing += new System.Windows.Forms.DataGridViewEditingControlShowingEventHandler(this.workoutGrid_EditingControlShowing);
            // 
            // setColumn
            // 
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle3.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(37)))), ((int)(((byte)(39)))), ((int)(((byte)(77)))));
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(37)))), ((int)(((byte)(39)))), ((int)(((byte)(77)))));
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.setColumn.DefaultCellStyle = dataGridViewCellStyle3;
            this.setColumn.HeaderText = "Set";
            this.setColumn.Name = "setColumn";
            this.setColumn.ReadOnly = true;
            this.setColumn.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            // 
            // repColumn
            // 
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle4.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle4.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(37)))), ((int)(((byte)(39)))), ((int)(((byte)(77)))));
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(37)))), ((int)(((byte)(39)))), ((int)(((byte)(77)))));
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.repColumn.DefaultCellStyle = dataGridViewCellStyle4;
            this.repColumn.HeaderText = "Reps";
            this.repColumn.Name = "repColumn";
            this.repColumn.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            // 
            // weightColumn
            // 
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle5.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle5.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(37)))), ((int)(((byte)(39)))), ((int)(((byte)(77)))));
            dataGridViewCellStyle5.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(37)))), ((int)(((byte)(39)))), ((int)(((byte)(77)))));
            dataGridViewCellStyle5.SelectionForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle5.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.weightColumn.DefaultCellStyle = dataGridViewCellStyle5;
            this.weightColumn.HeaderText = "Weight";
            this.weightColumn.Name = "weightColumn";
            // 
            // goalReps
            // 
            this.goalReps.HeaderText = "Rep Goal";
            this.goalReps.Name = "goalReps";
            this.goalReps.ReadOnly = true;
            // 
            // repWeight
            // 
            this.repWeight.HeaderText = "Weight Goal";
            this.repWeight.Name = "repWeight";
            this.repWeight.ReadOnly = true;
            // 
            // titlePanel
            // 
            this.titlePanel.BackColor = System.Drawing.Color.White;
            this.titlePanel.ColumnCount = 1;
            this.titlePanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.titlePanel.Controls.Add(this.panel1, 0, 1);
            this.titlePanel.Controls.Add(this.routineCB, 0, 0);
            this.titlePanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.titlePanel.Location = new System.Drawing.Point(0, 0);
            this.titlePanel.Name = "titlePanel";
            this.titlePanel.RowCount = 2;
            this.titlePanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 87.30159F));
            this.titlePanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 12.69841F));
            this.titlePanel.Size = new System.Drawing.Size(636, 63);
            this.titlePanel.TabIndex = 14;
            // 
            // routineCB
            // 
            this.routineCB.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.routineCB.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(37)))), ((int)(((byte)(39)))), ((int)(((byte)(77)))));
            this.routineCB.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.routineCB.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.routineCB.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.routineCB.ForeColor = System.Drawing.Color.White;
            this.routineCB.FormattingEnabled = true;
            this.routineCB.Location = new System.Drawing.Point(218, 16);
            this.routineCB.Name = "routineCB";
            this.routineCB.Size = new System.Drawing.Size(200, 23);
            this.routineCB.TabIndex = 5;
            this.routineCB.SelectedIndexChanged += new System.EventHandler(this.routineCB_SelectedIndexChanged);
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
            // exercisePanel
            // 
            this.exercisePanel.Dock = System.Windows.Forms.DockStyle.Left;
            this.exercisePanel.Location = new System.Drawing.Point(0, 2);
            this.exercisePanel.Name = "exercisePanel";
            this.exercisePanel.Size = new System.Drawing.Size(146, 476);
            this.exercisePanel.TabIndex = 15;
            // 
            // centerPanel
            // 
            this.centerPanel.Controls.Add(this.dataPanel);
            this.centerPanel.Controls.Add(this.titlePanel);
            this.centerPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.centerPanel.Location = new System.Drawing.Point(146, 2);
            this.centerPanel.Name = "centerPanel";
            this.centerPanel.Size = new System.Drawing.Size(636, 476);
            this.centerPanel.TabIndex = 16;
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(37)))), ((int)(((byte)(39)))), ((int)(((byte)(77)))));
            this.panel1.Location = new System.Drawing.Point(3, 58);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(630, 2);
            this.panel1.TabIndex = 6;
            // 
            // Workouts
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(782, 478);
            this.Controls.Add(this.centerPanel);
            this.Controls.Add(this.exercisePanel);
            this.Controls.Add(this.headerBorder);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "Workouts";
            this.Text = "``";
            this.Load += new System.EventHandler(this.Workouts_Load);
            this.dataPanel.ResumeLayout(false);
            this.dataPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.workoutGrid)).EndInit();
            this.titlePanel.ResumeLayout(false);
            this.centerPanel.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.TableLayoutPanel dataPanel;
        private System.Windows.Forms.Label exLbl;
        private System.Windows.Forms.DataGridView workoutGrid;
        private System.Windows.Forms.DataGridViewTextBoxColumn setColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn repColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn weightColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn goalReps;
        private System.Windows.Forms.DataGridViewTextBoxColumn repWeight;
        private System.Windows.Forms.TableLayoutPanel titlePanel;
        private System.Windows.Forms.ComboBox routineCB;
        private System.Windows.Forms.Panel headerBorder;
        private System.Windows.Forms.FlowLayoutPanel exercisePanel;
        private System.Windows.Forms.Panel centerPanel;
        private System.Windows.Forms.Panel panel1;
    }
}
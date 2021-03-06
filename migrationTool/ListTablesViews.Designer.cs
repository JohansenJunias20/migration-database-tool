
namespace migrationTool
{
    partial class ListTablesViews
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
            this.tableCheckedListBox = new System.Windows.Forms.CheckedListBox();
            this.okButton = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            this.viewCheckedListBox = new System.Windows.Forms.CheckedListBox();
            this.selectAllTableCheckBox = new System.Windows.Forms.CheckBox();
            this.selectAllViewCheckBox = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // tableCheckedListBox
            // 
            this.tableCheckedListBox.CheckOnClick = true;
            this.tableCheckedListBox.FormattingEnabled = true;
            this.tableCheckedListBox.Location = new System.Drawing.Point(54, 42);
            this.tableCheckedListBox.Name = "tableCheckedListBox";
            this.tableCheckedListBox.Size = new System.Drawing.Size(268, 259);
            this.tableCheckedListBox.TabIndex = 0;
            // 
            // okButton
            // 
            this.okButton.Location = new System.Drawing.Point(574, 335);
            this.okButton.Name = "okButton";
            this.okButton.Size = new System.Drawing.Size(75, 23);
            this.okButton.TabIndex = 1;
            this.okButton.Text = "OK";
            this.okButton.UseVisualStyleBackColor = true;
            this.okButton.Click += new System.EventHandler(this.okButton_Click);
            // 
            // cancelButton
            // 
            this.cancelButton.Location = new System.Drawing.Point(493, 335);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(75, 23);
            this.cancelButton.TabIndex = 2;
            this.cancelButton.Text = "Cancel";
            this.cancelButton.UseVisualStyleBackColor = true;
            // 
            // viewCheckedListBox
            // 
            this.viewCheckedListBox.CheckOnClick = true;
            this.viewCheckedListBox.FormattingEnabled = true;
            this.viewCheckedListBox.Location = new System.Drawing.Point(381, 42);
            this.viewCheckedListBox.Name = "viewCheckedListBox";
            this.viewCheckedListBox.Size = new System.Drawing.Size(268, 259);
            this.viewCheckedListBox.TabIndex = 3;
            // 
            // selectAllTableCheckBox
            // 
            this.selectAllTableCheckBox.AutoSize = true;
            this.selectAllTableCheckBox.Location = new System.Drawing.Point(135, 19);
            this.selectAllTableCheckBox.Name = "selectAllTableCheckBox";
            this.selectAllTableCheckBox.Size = new System.Drawing.Size(105, 17);
            this.selectAllTableCheckBox.TabIndex = 4;
            this.selectAllTableCheckBox.Text = "Select All Tables";
            this.selectAllTableCheckBox.UseVisualStyleBackColor = true;
            this.selectAllTableCheckBox.CheckedChanged += new System.EventHandler(this.selectAllTableCheckBox_CheckedChanged);
            // 
            // selectAllViewCheckBox
            // 
            this.selectAllViewCheckBox.AutoSize = true;
            this.selectAllViewCheckBox.Location = new System.Drawing.Point(478, 19);
            this.selectAllViewCheckBox.Name = "selectAllViewCheckBox";
            this.selectAllViewCheckBox.Size = new System.Drawing.Size(101, 17);
            this.selectAllViewCheckBox.TabIndex = 5;
            this.selectAllViewCheckBox.Text = "Select All Views";
            this.selectAllViewCheckBox.UseVisualStyleBackColor = true;
            this.selectAllViewCheckBox.CheckedChanged += new System.EventHandler(this.selectAllViewCheckBox_CheckedChanged);
            // 
            // ListTablesViews
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(709, 379);
            this.Controls.Add(this.selectAllViewCheckBox);
            this.Controls.Add(this.selectAllTableCheckBox);
            this.Controls.Add(this.viewCheckedListBox);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.okButton);
            this.Controls.Add(this.tableCheckedListBox);
            this.Name = "ListTablesViews";
            this.Text = "ListTablesViews";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckedListBox tableCheckedListBox;
        private System.Windows.Forms.Button okButton;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.CheckedListBox viewCheckedListBox;
        private System.Windows.Forms.CheckBox selectAllTableCheckBox;
        private System.Windows.Forms.CheckBox selectAllViewCheckBox;
    }
}
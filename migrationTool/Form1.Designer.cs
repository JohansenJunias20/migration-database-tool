
namespace migrationTool
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
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.ipTarget = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.passwordTarget = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.usernameTarget = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.databaseTarget = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.portTarget = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label3 = new System.Windows.Forms.Label();
            this.passwordSource = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.usernameSource = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.pathSource = new System.Windows.Forms.TextBox();
            this.logLabel = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.ignoreExistCheckBox = new System.Windows.Forms.CheckBox();
            this.toolTip2 = new System.Windows.Forms.ToolTip(this.components);
            this.showLog = new System.Windows.Forms.Button();
            this.ignoreDuplicateKey = new System.Windows.Forms.CheckBox();
            this.toolTipDuplicateKey = new System.Windows.Forms.ToolTip(this.components);
            this.forceModeCheckBox = new System.Windows.Forms.CheckBox();
            this.forceModeTip = new System.Windows.Forms.ToolTip(this.components);
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(120, 273);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(532, 23);
            this.progressBar1.TabIndex = 0;
            // 
            // ipTarget
            // 
            this.ipTarget.Location = new System.Drawing.Point(119, 33);
            this.ipTarget.Name = "ipTarget";
            this.ipTarget.Size = new System.Drawing.Size(112, 20);
            this.ipTarget.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(131, 96);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(38, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Target";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.passwordTarget);
            this.groupBox1.Controls.Add(this.label8);
            this.groupBox1.Controls.Add(this.usernameTarget);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.databaseTarget);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.portTarget);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.ipTarget);
            this.groupBox1.Location = new System.Drawing.Point(395, 42);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(340, 213);
            this.groupBox1.TabIndex = 4;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Target";
            // 
            // passwordTarget
            // 
            this.passwordTarget.Location = new System.Drawing.Point(119, 165);
            this.passwordTarget.Name = "passwordTarget";
            this.passwordTarget.PasswordChar = '*';
            this.passwordTarget.Size = new System.Drawing.Size(100, 20);
            this.passwordTarget.TabIndex = 15;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(44, 165);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(53, 13);
            this.label8.TabIndex = 14;
            this.label8.Text = "Password";
            // 
            // usernameTarget
            // 
            this.usernameTarget.Location = new System.Drawing.Point(119, 132);
            this.usernameTarget.Name = "usernameTarget";
            this.usernameTarget.Size = new System.Drawing.Size(100, 20);
            this.usernameTarget.TabIndex = 13;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(42, 133);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(55, 13);
            this.label7.TabIndex = 12;
            this.label7.Text = "Username";
            // 
            // databaseTarget
            // 
            this.databaseTarget.Location = new System.Drawing.Point(119, 99);
            this.databaseTarget.Name = "databaseTarget";
            this.databaseTarget.Size = new System.Drawing.Size(100, 20);
            this.databaseTarget.TabIndex = 11;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(44, 101);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(53, 13);
            this.label6.TabIndex = 10;
            this.label6.Text = "Database";
            // 
            // portTarget
            // 
            this.portTarget.Location = new System.Drawing.Point(119, 66);
            this.portTarget.Name = "portTarget";
            this.portTarget.Size = new System.Drawing.Size(100, 20);
            this.portTarget.TabIndex = 9;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(71, 69);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(26, 13);
            this.label4.TabIndex = 7;
            this.label4.Text = "Port";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(80, 37);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(17, 13);
            this.label5.TabIndex = 8;
            this.label5.Text = "IP";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.passwordSource);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.usernameSource);
            this.groupBox2.Controls.Add(this.label9);
            this.groupBox2.Controls.Add(this.pathSource);
            this.groupBox2.Location = new System.Drawing.Point(50, 42);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(310, 213);
            this.groupBox2.TabIndex = 12;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Source";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(22, 102);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(53, 13);
            this.label3.TabIndex = 12;
            this.label3.Text = "Password";
            // 
            // passwordSource
            // 
            this.passwordSource.Location = new System.Drawing.Point(130, 99);
            this.passwordSource.Name = "passwordSource";
            this.passwordSource.Size = new System.Drawing.Size(100, 20);
            this.passwordSource.TabIndex = 11;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(20, 69);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(55, 13);
            this.label2.TabIndex = 10;
            this.label2.Text = "Username";
            // 
            // usernameSource
            // 
            this.usernameSource.Location = new System.Drawing.Point(130, 66);
            this.usernameSource.Name = "usernameSource";
            this.usernameSource.Size = new System.Drawing.Size(100, 20);
            this.usernameSource.TabIndex = 9;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(46, 36);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(29, 13);
            this.label9.TabIndex = 8;
            this.label9.Text = "Path";
            // 
            // pathSource
            // 
            this.pathSource.Location = new System.Drawing.Point(130, 33);
            this.pathSource.Name = "pathSource";
            this.pathSource.Size = new System.Drawing.Size(174, 20);
            this.pathSource.TabIndex = 1;
            // 
            // logLabel
            // 
            this.logLabel.AutoSize = true;
            this.logLabel.Location = new System.Drawing.Point(122, 302);
            this.logLabel.Name = "logLabel";
            this.logLabel.Size = new System.Drawing.Size(38, 13);
            this.logLabel.TabIndex = 13;
            this.logLabel.Text = "Ready";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(338, 413);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 25);
            this.button1.TabIndex = 14;
            this.button1.Text = "Migrate";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // backgroundWorker1
            // 
            this.backgroundWorker1.WorkerReportsProgress = true;
            this.backgroundWorker1.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker1_DoWork);
            this.backgroundWorker1.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.backgroundWorker1_ProgressChanged);
            // 
            // ignoreExistCheckBox
            // 
            this.ignoreExistCheckBox.AutoSize = true;
            this.ignoreExistCheckBox.Location = new System.Drawing.Point(307, 390);
            this.ignoreExistCheckBox.Name = "ignoreExistCheckBox";
            this.ignoreExistCheckBox.Size = new System.Drawing.Size(149, 17);
            this.ignoreExistCheckBox.TabIndex = 15;
            this.ignoreExistCheckBox.Text = "Ignore Table Already Exist";
            this.ignoreExistCheckBox.UseVisualStyleBackColor = true;
            // 
            // toolTip2
            // 
            this.toolTip2.AutomaticDelay = 100;
            this.toolTip2.AutoPopDelay = 15000;
            this.toolTip2.InitialDelay = 100;
            this.toolTip2.ReshowDelay = 20;
            // 
            // showLog
            // 
            this.showLog.Location = new System.Drawing.Point(577, 302);
            this.showLog.Name = "showLog";
            this.showLog.Size = new System.Drawing.Size(75, 23);
            this.showLog.TabIndex = 16;
            this.showLog.Text = "show log";
            this.showLog.UseVisualStyleBackColor = true;
            this.showLog.Click += new System.EventHandler(this.showLog_Click);
            // 
            // ignoreDuplicateKey
            // 
            this.ignoreDuplicateKey.AutoSize = true;
            this.ignoreDuplicateKey.Location = new System.Drawing.Point(307, 367);
            this.ignoreDuplicateKey.Name = "ignoreDuplicateKey";
            this.ignoreDuplicateKey.Size = new System.Drawing.Size(154, 17);
            this.ignoreDuplicateKey.TabIndex = 17;
            this.ignoreDuplicateKey.Text = "Ignore Insert Duplicate Key";
            this.ignoreDuplicateKey.UseVisualStyleBackColor = true;
            // 
            // forceModeCheckBox
            // 
            this.forceModeCheckBox.AutoSize = true;
            this.forceModeCheckBox.Location = new System.Drawing.Point(338, 344);
            this.forceModeCheckBox.Name = "forceModeCheckBox";
            this.forceModeCheckBox.Size = new System.Drawing.Size(83, 17);
            this.forceModeCheckBox.TabIndex = 18;
            this.forceModeCheckBox.Text = "Force Mode";
            this.forceModeCheckBox.UseVisualStyleBackColor = true;
            this.forceModeCheckBox.Click += new System.EventHandler(this.checkBox1_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.forceModeCheckBox);
            this.Controls.Add(this.ignoreDuplicateKey);
            this.Controls.Add(this.showLog);
            this.Controls.Add(this.ignoreExistCheckBox);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.logLabel);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.progressBar1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.TextBox ipTarget;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox databaseTarget;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox portTarget;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox pathSource;
        private System.Windows.Forms.Label logLabel;
        private System.Windows.Forms.Button button1;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private System.Windows.Forms.TextBox passwordTarget;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox usernameTarget;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox passwordSource;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox usernameSource;
        private System.Windows.Forms.CheckBox ignoreExistCheckBox;
        private System.Windows.Forms.ToolTip toolTip2;
        private System.Windows.Forms.Button showLog;
        private System.Windows.Forms.CheckBox ignoreDuplicateKey;
        private System.Windows.Forms.ToolTip toolTipDuplicateKey;
        private System.Windows.Forms.CheckBox forceModeCheckBox;
        private System.Windows.Forms.ToolTip forceModeTip;
    }
}


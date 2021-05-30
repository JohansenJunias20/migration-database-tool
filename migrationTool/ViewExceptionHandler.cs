using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace migrationTool
{
    public partial class ViewExceptionHandler : Form
    {
        public ViewExceptionHandler(string sql, string error)
        {
            InitializeComponent();
            richTextBox1.Text = sql;
            errorLabel.Text = error;
        }

        private void ViewExceptionHandler_Load(object sender, EventArgs e)
        {

        }

        public string fixedSql;
        public bool isRetry = false;
        public bool isSkip = false;
        private void retryButton_Click(object sender, EventArgs e)
        {
            isRetry = true;
            fixedSql = richTextBox1.Text;
            this.Close();
        }

        private void skipButton_Click(object sender, EventArgs e)
        {
            isSkip = true;
            this.Close();
        }
    }
}

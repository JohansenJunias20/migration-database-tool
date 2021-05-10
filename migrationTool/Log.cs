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
    public partial class Log : Form
    {
        public Log()
        {
            InitializeComponent();
        }

        private void Log_Load(object sender, EventArgs e)
        {

        }
        public void appendLog(string log)
        {
            richTextBox1.AppendText("\n" + log);
        }
        public void changePercentageProgress(int percentage)
        {
            progressBar1.Value = percentage;
        }
    }
}

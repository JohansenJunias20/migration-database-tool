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
    public partial class ListTablesViews : Form
    {
        public ListTablesViews(List<string> tableNames, List<string> viewNames)
        {
            InitializeComponent();
            tableNames.ForEach(tableName =>
            {
                tableCheckedListBox.Items.Add(tableName, false);
            });
            viewNames.ForEach(viewName =>
            {
                viewCheckedListBox.Items.Add(viewName, false);
            });
        }
        public bool isOK = false;
        public List<string> tables = new List<string>();
        public List<string> views = new List<string>();
        private void okButton_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < tableCheckedListBox.Items.Count; i++)
            {
                if (tableCheckedListBox.GetItemChecked(i))
                    tables.Add(tableCheckedListBox.Items[i].ToString());
            }
            for (int i = 0; i < viewCheckedListBox.Items.Count; i++)
            {
                if (viewCheckedListBox.GetItemChecked(i))
                    views.Add(viewCheckedListBox.Items[i].ToString());
            }
            isOK = true;
            this.Close();
        }
    }
}

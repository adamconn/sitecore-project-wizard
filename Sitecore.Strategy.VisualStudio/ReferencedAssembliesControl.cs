using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Sitecore.Strategy.VisualStudio
{
    public partial class ReferencedAssembliesControl : UserControl
    {
        public ReferencedAssembliesControl(ReferencedAssembliesPage referencesPage)
        {
            this.ReferencesPage = referencesPage;
            InitializeComponent();
        }
        public ReferencedAssembliesPage ReferencesPage { get; private set; }

        public void Initialize()
        {
            if (string.IsNullOrEmpty(this.ReferencesPage.ReferencedAssemblies))
            {
                return;
            }
            var assemblies = this.ReferencesPage.ReferencedAssemblies.Split(new char[]{','});
            if (assemblies.Length == 0)
            {
                return;
            }
            foreach (var assembly in assemblies)
            {
                listAssemblies.Items.Add(assembly);    
            }
        }

        private void listAssemblies_SelectedIndexChanged(object sender, EventArgs e)
        {
            btnRemove.Enabled = (listAssemblies.SelectedIndex != -1);
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            var dialog = new OpenFileDialog();
            dialog.Filter = "Component Files (*.dll; *.tlb; *.olb; *.exe; *.manifest)|*.dll;*.tlb;*.olb;*.exe;*.manifest";
            dialog.FilterIndex = 0;
            dialog.Multiselect = true;
            var response = dialog.ShowDialog();
            if (response != DialogResult.OK)
            {
                return;
            }
            foreach (var name in dialog.SafeFileNames)
            {
                if (!listAssemblies.Items.Contains(name))
                {
                    listAssemblies.Items.Add(name);
                }
            }
            SaveReferences();
        }

        private void SaveReferences()
        {
            var list = new List<string>();
            foreach (string item in listAssemblies.Items)
            {
                list.Add(item);
            }
            this.ReferencesPage.ReferencedAssemblies = string.Join(",", list);
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            if (listAssemblies.SelectedIndex == -1)
            {
                return;
            }
            var result = MessageBox.Show("Are you sure you want to remove the selected assemblies?", "Remove Assembly", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                for (var i = (listAssemblies.SelectedIndices.Count - 1); i >= 0; i--)
                {
                    listAssemblies.Items.RemoveAt(listAssemblies.SelectedIndices[i]);
                }
            }
            listAssemblies.SelectedIndex = -1;
            SaveReferences();
        }
    }
}

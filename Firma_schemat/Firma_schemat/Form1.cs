using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Firma_schemat
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            OdczytZPliku();
        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {

        }

        private void dodajGalazToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form2 dialog = new Form2();
            dialog.Text = "Dodawanie gałęzi";
            if(dialog.ShowDialog() == DialogResult.OK)
            {
                treeView1.Nodes.Add(dialog.nazwa);
            }
        }

        private void dodajPodgalazToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form2 dialog = new Form2();
            dialog.Text = "Dodawanie podgałęzi";
            if(dialog.ShowDialog() == DialogResult.OK)
            {
                treeView1.SelectedNode.Nodes.Add(dialog.nazwa);

            }
        }

        private void zmieńToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form2 dialog = new Form2();
            dialog.Text = "Modyfikowanie gałęzi";
            dialog.nazwa = treeView1.SelectedNode.Text;
            if(dialog.ShowDialog() == DialogResult.OK)
            {
                treeView1.SelectedNode.Text = dialog.nazwa;
            }
        }

        private void usuńToolStripMenuItem_Click(object sender, EventArgs e)
        {
            treeView1.Nodes.Remove(treeView1.SelectedNode);
        }

        public class myNode
        {
            public string rodzic;
            public string nazwa;

            public myNode(string rodzic, string nazwa)
            {
                this.rodzic = rodzic;
                this.nazwa = nazwa;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            DialogResult res = MessageBox.Show("Zapisać zmiany przed zamknięciem?", "Zamykanie",
                MessageBoxButtons.YesNoCancel);
            if(res == DialogResult.Yes)
            {
                ZapisDoPliku();
                Application.Exit();
            }
            else if (res == DialogResult.No)
            {
                Application.Exit();
            }
        }

        private void DodajDolisty(TreeNode node, ref List<myNode> lista)
        {
            if (node == null)
                return;
            string r, n;
            if (node.Parent == null)
                r = "brak";
            else
                r = node.Parent.Text;
            n = node.Text;

            lista.Add(new myNode(r, n));
            if (node.NextNode != null)
                DodajDolisty(node.NextNode, ref lista);
            if (node.GetNodeCount(true) > 0)
                DodajDolisty(node.FirstNode, ref lista);
        }

        private void ZapisDoPliku()
        {
            List<myNode> lista = new List<myNode>();
            DodajDolisty(treeView1.Nodes[0], ref lista);
            if (lista.Count == 0)
                return;
            string text = "";
            foreach(myNode elem in lista)
            {
                text += elem.nazwa + " " + elem.rodzic + "\n";
            }
            File.WriteAllText("firma.txt", text);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ZapisDoPliku();
            Application.Exit();
        }

        private TreeNode ZnajdzRodzica(TreeNode node, string rodzic)
            {
                TreeNode wynik = null;
                if (node == null)
                    return null;
                if (node.Text == rodzic)
                    return node;

                if (node.NextNode != null)
                    wynik = ZnajdzRodzica(node.NextNode, rodzic);
                if (node.GetNodeCount(true) > 0)
                    wynik = ZnajdzRodzica(node.FirstNode, rodzic);

                return wynik;

           }


        private void OdczytZPliku()
        {
            treeView1.Nodes.Clear();
            List<myNode> lista = new List<myNode>();
            if (!File.Exists("Brudas.txt"))
                return;

            string[] tab = File.ReadAllLines("firma.txt");
            foreach (string elem in tab)
            {
                string[] pom = elem.Split(' ');
                lista.Add(new myNode(pom[1], pom[0]));

            }
            foreach (myNode node in lista)
            {
                if (node.rodzic == "brak")
                    treeView1.Nodes.Add(new TreeNode(node.nazwa));
                else
                {
                    TreeNode rodzic = ZnajdzRodzica(node.rodzic);
                    if (rodzic != null)
                        rodzic.Nodes.Add(node.nazwa);

                }
            }
        }

        private TreeNode ZnajdzRodzica(string rodzic)
        {
            
            return null;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            OdczytZPliku();
            
        }
    }
}

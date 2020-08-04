using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SprocOnlyApp
{
    public partial class Form1 : Form
    {
        private const string CONNECT_STRING = @"Data Source=(LocalDB)\MSSQLLocalDB;
                                                AttachDbFilename=C:\linqtest7\northwnd.mdf;
                                                Integrated Security=True;
                                                Connect Timeout=30";

        Northwnd db = new Northwnd(CONNECT_STRING);

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string param = textBox1.Text;
            var custQuery = db.CustOrdersDetail(Convert.ToInt32(param));
            string msg = "";
            foreach (CustOrdersDetailResult custOrdersDetail in custQuery)
            {
                msg = msg + custOrdersDetail.ProductName + "\n";
            }
            if(msg == "")
                msg = "No results.";
            MessageBox.Show(msg);
            // Clear the variables before continuing.
            param = "";
            textBox1.Text = "";
        }

        private void button2_Click(object sender, EventArgs e)
        {
            // Comments in the code for button2 are the same
            // as for button1.
            string param = textBox2.Text;
            var custquery = db.CustOrderHist(param);
            string msg = "";
            foreach (CustOrderHistResult custOrdHist in custquery)
            {
                msg = msg + custOrdHist.ProductName + "\n";
            }
            MessageBox.Show(msg);
            param = "";
            textBox2.Text = "";
        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Oracle.DataAccess.Client;
using Oracle.DataAccess.Types;
namespace sw
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        OracleDataAdapter adapter;
        DataSet ds;
        private void button13_Click(object sender, EventArgs e)
        {
            string constr = "Data Source=ORCL;User Id=hr;Password=hr;";
            string cmdstr = "";

            if (radioButton1.Checked)
            {
                cmdstr = "SELECT * FROM electricbills;";
                adapter = new OracleDataAdapter(cmdstr, constr);
                ds = new DataSet();
                adapter.Fill(ds);
                dataGridView1.DataSource = ds.Tables[0];
                MessageBox.Show("Now you can update info !!!");

            }
            else if (radioButton2.Checked)
            {
                cmdstr = "SELECT user_name, passwordd, BANKCREDIT FROM ACCOUNTinfo WHERE customer_id = :idd";
                adapter = new OracleDataAdapter(cmdstr, constr);
                adapter.SelectCommand.Parameters.Add("idd", textBox11.Text);

                ds = new DataSet();
                DataTable dataTable = new DataTable("ACCOUNTinfo");

                // Define the schema for the DataTable
                dataTable.Columns.Add("user_name", typeof(string));
                dataTable.Columns.Add("passwordd", typeof(string));
                dataTable.Columns.Add("BANKCREDIT", typeof(string));
                dataTable.PrimaryKey = new DataColumn[] { dataTable.Columns["customer_id"] };

                ds.Tables.Add(dataTable);

                adapter.Fill(ds, "ACCOUNTinfo");

                dataGridView1.DataSource = ds.Tables["ACCOUNTinfo"];
                MessageBox.Show("you can only retieve your info not update!");


            }
        }

        private void button12_Click(object sender, EventArgs e)
        {

            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);

            adapter.Update(ds.Tables[0]);

            MessageBox.Show("Changes saved successfully.");
        }

        private void button10_Click(object sender, EventArgs e)
        {
            Form1 form1 = new Form1();
            form1.Show();
            this.Hide();
        }
    }
}

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
using CrystalDecisions.Shared;

using Oracle.DataAccess.Types;
namespace sw
{
    public partial class Form1 : Form
    {
        string ordb = "Data Source=ORCL; User Id=hr;Password=hr;";
        CrystalReport1 cr;
        OracleConnection conn;

        OracleDataAdapter adapter;
        DataSet ds;

        public Form1()
        {
            InitializeComponent();
        }


        private void Form1_Load(object sender, EventArgs e)
        {
            conn = new OracleConnection(ordb);
            conn.Open();

            cr = new CrystalReport1();
            foreach (ParameterDiscreteValue v in cr.ParameterFields[0].DefaultValues)
                comboBox1.Items.Add(v.Value);
            
        }






        private void button1_Click(object sender, EventArgs e)
        {
            int x1;
            OracleCommand cmd = new OracleCommand();
            cmd.Connection = conn;
            cmd.CommandText = "CheckCredentials";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("name", textBox3.Text);
            cmd.Parameters.Add("pass", textBox4.Text);
            cmd.Parameters.Add("p_result", OracleDbType.Int32, ParameterDirection.Output);
            cmd.ExecuteNonQuery();
            try
            {
                x1 = Convert.ToInt32(cmd.Parameters["p_result"].Value.ToString());
                if (x1 == 1)
                {
                    if (textBox3.Text == "ADMIN" && textBox4.Text == "ADMIN123")
                    {
                        panel3.Visible = true;
                        panel1.Visible = false;
                        panel2.Visible = false;

                    }
                    else
                    {

                        panel2.Visible = true;
                        panel1.Visible = false;
                    }
                    MessageBox.Show("login successfully!");

                }
                else
                {
                    MessageBox.Show("invaild");
                }
            }
            catch
            {
                MessageBox.Show("try again");

            }

            OracleCommand cmd2 = new OracleCommand();
            cmd2.Connection = conn;
            cmd2.CommandText = "UpdateBillValueForEachRow";
            cmd2.CommandType = CommandType.StoredProcedure;
            cmd2.ExecuteNonQuery();
        }



        private void button2_Click_1(object sender, EventArgs e)
        {
            OracleCommand cmd = new OracleCommand();
            cmd.Connection = conn;
            cmd.CommandText = "SELECT bill_value, due_date FROM ElectricBills, ACCOUNTinfo " +
                              "WHERE ElectricBills.customer_id = ACCOUNTinfo.customer_id AND " +
                              "ACCOUNTinfo.user_name = :userName";
            cmd.CommandType = CommandType.Text;

            // Add parameter for user_name, use the value from textBox3
            cmd.Parameters.Add("userName", OracleDbType.Varchar2).Value = textBox3.Text;

            OracleDataReader dr = cmd.ExecuteReader();
            if (dr.Read())  // Use if if you expect a single record, or while if there can be multiple
            {
                textBox1.Text = dr["bill_value"].ToString();
                textBox2.Text = dr["due_date"].ToString();
            }
            else
            {
                textBox1.Text = "No data found.";
                textBox2.Text = "No data found.";
            }


        }
        private void button4_Click_1(object sender, EventArgs e)
        {
            panel1.Visible = true;
            panel3.Visible = false;
            panel2.Visible = false;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            int x2;
            OracleCommand cmd = new OracleCommand();
            cmd.Connection = conn;
            cmd.CommandText = "CheckAndUpdateBankCredit";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("userName", OracleDbType.Varchar2).Value = textBox3.Text;
            cmd.Parameters.Add("result", OracleDbType.Int32, ParameterDirection.Output);
            cmd.ExecuteReader();
            x2 = Convert.ToInt32(cmd.Parameters["result"].Value.ToString());
            if (x2 == 1)
            {
                if (textBox1.Text == "0")
                {
                    MessageBox.Show("NO BILLS FOR YOU!");

                }
                else
                {
                    MessageBox.Show("payment successfully!");
                }
            }
            else
            {
                MessageBox.Show("invaild payment");
            }

        }

      


        private void button7_Click_1(object sender, EventArgs e)
        {
            OracleCommand cmd = new OracleCommand();
            cmd.Connection = conn;
            cmd.CommandText = "select meter_reading FROM ElectricBills where customer_id =:customerId";
            cmd.CommandType = CommandType.Text;

            // Add parameter for user_name, use the value from textBox3
            cmd.Parameters.Add("customerId", textBox5.Text);

            OracleDataReader dr = cmd.ExecuteReader();
            if (dr.Read())  // Use if if you expect a single record, or while if there can be multiple
            {
                textBox7.Text = dr["meter_reading"].ToString();
                textBox6.Text = DateTime.Now.ToString();

            }
            else
            {
                textBox1.Text = "No data found.";
                textBox2.Text = "No data found.";
            }
            dr.Close();

        }

        private void button5_Click_1(object sender, EventArgs e)
        {
            panel1.Visible = true;
            panel2.Visible = false;
            panel3.Visible = false;
        }

        private void button6_Click(object sender, EventArgs e)
        {
            OracleCommand cmd = new OracleCommand();
            cmd.Connection = conn;
            cmd.CommandText = "CalculateConsumption";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("id", textBox5.Text);
            cmd.ExecuteReader();
            MessageBox.Show("calculation successfully!");


        }

    


        private void button9_Click(object sender, EventArgs e)
        {
            panel4.Visible = true;
            panel1.Visible = false;
            panel2.Visible = false;
            panel3.Visible = false;

        }

        private void button8_Click_2(object sender, EventArgs e)
        {
            int newid;
            OracleCommand cmd2 = new OracleCommand();
            cmd2.Connection = conn;
            cmd2.CommandText = "SELECT NVL(MAX(customer_id), 0) + 1 FROM ACCOUNTinfo";
            cmd2.CommandType = CommandType.Text;
            newid = Convert.ToInt32(cmd2.ExecuteScalar());

            if (!string.IsNullOrEmpty(textBox8.Text) && !string.IsNullOrEmpty(textBox10.Text) && !string.IsNullOrEmpty(textBox9.Text))
            {
                OracleCommand cmd = new OracleCommand();
                cmd.Connection = conn;
                cmd.CommandText = "INSERT INTO ACCOUNTinfo (customer_id, user_name, passwordd, BANKCREDIT) VALUES (:newid, :textBox8Value, :textBox10Value, :textBox9Value)";
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.Add("newid", newid);
                cmd.Parameters.Add("textBox8Value", textBox8.Text);
                cmd.Parameters.Add("textBox10Value", textBox10.Text);
                cmd.Parameters.Add("textBox9Value", textBox9.Text);
                cmd.ExecuteNonQuery();
                panel1.Visible = true;
                panel2.Visible = false;
                panel3.Visible = false;
                panel4.Visible = false;
               

                MessageBox.Show("Signed up successfully!!");
            }
            else
            {
                MessageBox.Show("Please fill all the fields.");
            }

        }

 

        private void button12_Click(object sender, EventArgs e)
        {
            panel1.Visible = true;
            panel2.Visible = false;
            panel3.Visible = false;
            panel4.Visible = false;

        }



        private void button11_Click_1(object sender, EventArgs e)
        {

            Form2 form2 = new Form2();
            form2.Show();
            this.Hide();
        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            conn.Dispose();
        }

        private void panel3_Paint(object sender, PaintEventArgs e)
        {

        }

        private void _Click(object sender, EventArgs e)
        {

        }

        private void button10_Click(object sender, EventArgs e)
        {
            cr.SetParameterValue(0, comboBox1.Text);
            crystalReportViewer1.ReportSource = cr;
        }

       

    }
    }


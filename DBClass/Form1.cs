using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.Odbc;

namespace DBClass
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        OdbcConnection conn = null;

        private void Form1_Load(object sender, EventArgs e)
        {
            label1.Text = "";
        }

        private void PopulateTable(string[] parameters)
        {
            string select;
            if (parameters == null)
            {
                select = "select * from " + comboBox1.SelectedItem.ToString();
            }
            else
            {
                if (parameters.Length != 2)
                {
                    throw new ArgumentException("Invalid argument for select!");
                }

                select = String.Format("select * from {0} where {1} = '{2}'", comboBox1.SelectedItem.ToString(),
                                                                        parameters[0].Trim(), parameters[1].Trim());                
            }

            OdbcDataAdapter da = new OdbcDataAdapter(select, conn);
            DataSet ds = new DataSet();
            da.Fill(ds, comboBox1.SelectedItem.ToString());
            dataGridView1.AutoGenerateColumns = true;
            dataGridView1.DataSource = ds;
            dataGridView1.DataMember = comboBox1.SelectedItem.ToString();

            label1.Text = "Enter values separate by comma, in the " + Environment.NewLine + "order of ";
            label1.Text += ColName(dataGridView1.Columns, null);
        }

        private string ColName(DataGridViewColumnCollection cols, string mask)
        {
            string s = mask == null ? cols[0].Name : mask;
            for (int i = 1; i < cols.Count; ++i)
            {
                s += ", " + (mask == null ? cols[i].Name : mask);
            }

            return s;
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (conn != null)
                conn.Close();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            PopulateTable(null);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "" || textBox2.Text == "")
            {
                MessageBox.Show("Need both tuples before inserting");
                return;
            }

            using (OdbcCommand command = new OdbcCommand())
            {
                command.Connection = conn;

                command.CommandText = InsertString(textBox1.Text.Split(','));
                command.ExecuteNonQuery();

                command.CommandText = InsertString(textBox2.Text.Split(','));
                command.ExecuteNonQuery();
            }

            PopulateTable(null);
        }

        private string InsertString(string[] parameters)
        {
            if (dataGridView1.Columns.Count != parameters.Length)
            {
                throw new ArgumentException("Parameters counts must be equal to number of colums");
            }

            string s = String.Format("insert into {0} ({1}) values ('{2}'", comboBox1.SelectedItem.ToString(),
                                                                    ColName(dataGridView1.Columns, null), parameters[0]);

            for (int i = 1; i < parameters.Length; ++i)
            {
                s += ", '" + parameters[i] + "'";
            }

            s += ")";

            return s;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (textBox3.Text == "")
            {
                MessageBox.Show("Need select parameters in the from of attribute = value");
            }
            if (textBox3.Text.Split('=')[0] == "model")
            {
                MessageBox.Show("Attribute to look up can not be primary key");
            }
            else
            {
                PopulateTable(textBox3.Text.Split('='));
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            conn = new OdbcConnection();
            conn.ConnectionString = String.Format("Driver={{PostgreSQL ANSI}};Server=131.252.208.122;Port=5432;Database=class39db;Uid={0};Pwd={1};",
                                                                textBox4.Text, textBox5.Text);

            try
            {
                conn.Open();
                MessageBox.Show("Connected!");
            }
            catch (Exception exc)
            {
                MessageBox.Show("Can not open connection." + Environment.NewLine +
                                 exc.Message + Environment.NewLine +
                                 "Program terminating...!");
                this.Close();
                return;
            }
            textBox1.Enabled = true;
            textBox2.Enabled = true;
            textBox3.Enabled = true;
            button1.Enabled = true;
            button2.Enabled = true;
            comboBox1.Enabled = true;
            comboBox1.SelectedIndex = 0;
        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {
            this.AcceptButton = button3;
        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {
            this.AcceptButton = button3;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            this.AcceptButton = button1;
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            this.AcceptButton = button1;
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            this.AcceptButton = button2;
        }
    }
}

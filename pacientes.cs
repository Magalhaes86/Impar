using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using System.IO;
using MySql.Data.MySqlClient;
using System.Data;
using MySql.Data;
using System.Drawing;

namespace Impar
{
    public partial class pacientes : Form
    {
        public pacientes()
        {
            InitializeComponent();
        }

        //   MySqlConnection connection = new MySqlConnection(@"server=localhost;database=ContabSysDB;port=3308;userid=root;password=xd");
        MySqlConnection connection = new MySqlConnection(@"server=" + Properties.Settings.Default.server + ";database=" + Properties.Settings.Default.basedados + ";port=" + Properties.Settings.Default.porta + ";userid=" + Properties.Settings.Default.username + ";password=" + Properties.Settings.Default.password);


        MySqlCommand command;


        public void openConnection()
        {
            if (connection.State == ConnectionState.Closed)
            {
                connection.Open();
            }

        }

        public void closeConnection()
        {
            if (connection.State == ConnectionState.Open)
            {
                connection.Close();
            }

        }


        public void executeMyQuery(string query)
        {
            try
            {
                openConnection();
                command = new MySqlCommand(query, connection);

                if (command.ExecuteNonQuery() == 1)
                {
                    MessageBox.Show("Cliente Guardado com Sucesso");
                }
                else
                {
                    MessageBox.Show("Erro");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            finally
            {
                closeConnection();
            }
        }


        private void btnGravar_Click(object sender, EventArgs e)
        {
            string insertQuery = "INSERT INTO pacientes (Nome,Nif,Morada,Email,Tlm,Tlf,Obs) VALUES('" + tbNome.Text + "','" + tbNif.Text + "','" + tbMorada.Text + "','" + tbEmail.Text + "','" + tbTlm.Text + "','" + tbTlf.Text + "','" + tbobs.Text + "');";
            executeMyQuery(insertQuery);
        }

        private void pacientes_Load(object sender, EventArgs e)
        {

        }
    }
}

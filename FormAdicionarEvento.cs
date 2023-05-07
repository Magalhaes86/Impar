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
    public partial class FormAdicionarEvento : Form
    {

        public FormAdicionarEvento(string data, string horaInicio, string horaFim, string Idgoogle, string descricao, string titulo, string CodCliente)
        {
            InitializeComponent();

            // Definindo os valores das TextBoxes
            tbhorario.Text = data;
            tbhorainicio.Text = horaInicio;
            tbhorafim.Text = horaFim;
            tbidgoogle.Text = Idgoogle;
            tbdescricao.Text = descricao;
            tbtitulogoogle.Text = titulo;
            tbcodcliente.Text = CodCliente;



        }

        public FormAdicionarEvento(string data, DateTime dateTime, DateTime dateTime1)
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


        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            string insertQuery = "INSERT INTO marcacoes (Idcliente,IDGoogle,Horario,TipoTratamento,Obs,Descricao,TituloGoogle,Horainicio,Horafim) " +
                "VALUES('" + tbcodcliente.Text + "','" + tbidgoogle.Text + "','" + tbhorario.Text + "','" + tbtipotratamento.Text + "','" + tbobs.Text + "','" + tbdescricao.Text + "','" + tbtitulogoogle.Text + "','" + tbhorainicio.Text + "','" + tbhorafim.Text + "');";
            executeMyQuery(insertQuery);
        }

        private void FormAdicionarEvento_Load(object sender, EventArgs e)
        {

        }
    }
}

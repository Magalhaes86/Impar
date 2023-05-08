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
            //string insertQuery = "INSERT INTO pacientes (Nome,Nif,Morada,Email,Tlm,Tlf,Obs) VALUES('" + tbNome.Text + "','" + tbNif.Text + "','" + tbMorada.Text + "','" + tbEmail.Text + "','" + tbTlm.Text + "','" + tbTlf.Text + "','" + tbobs.Text + "');";
            //executeMyQuery(insertQuery);


            // Verifica se o número de telefone foi informado
            if (string.IsNullOrEmpty(tbTlm.Text))
            {
                // Exibe uma mensagem de aviso e pergunta se o usuário deseja continuar sem informar o número de telefone
                DialogResult result = MessageBox.Show("Pretende continuar sem inserir um número de telemóvel? Caso tenha modulo SMS o número de telemóvel é necessário para o envio das mesmas!", "Aviso", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                // Se o usuário escolher "Sim", continua sem inserir o número de telefone
                if (result == DialogResult.Yes)
                {
                    // Monta a consulta SQL de inserção
                    string insertQuery = "INSERT INTO pacientes (Nome,Nif,Morada,Email,Tlm,Tlf,Obs) VALUES('" + tbNome.Text + "','" + tbNif.Text + "','" + tbMorada.Text + "','" + tbEmail.Text + "','" + tbTlm.Text + "','" + tbTlf.Text + "','" + tbobs.Text + "');";

                    // Executa a consulta SQL
                    executeMyQuery(insertQuery);

                    // Exibe uma mensagem de sucesso
                    MessageBox.Show("Os dados foram inseridos com sucesso.", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    // Se o usuário escolher "Não", posiciona o foco no campo de número de telefone
                    tbTlm.Focus();
                }
            }
            else
            {
                // Se o número de telefone foi informado, monta a consulta SQL de inserção
                string insertQuery = "INSERT INTO pacientes (Nome,Nif,Morada,Email,Tlm,Tlf,Obs) VALUES('" + tbNome.Text + "','" + tbNif.Text + "','" + tbMorada.Text + "','" + tbEmail.Text + "','" + tbTlm.Text + "','" + tbTlf.Text + "','" + tbobs.Text + "');";

                // Executa a consulta SQL
                executeMyQuery(insertQuery);

                // Exibe uma mensagem de sucesso
                MessageBox.Show("Os dados foram inseridos com sucesso.", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

        }

        private void pacientes_Load(object sender, EventArgs e)
        {

        }
    }
}

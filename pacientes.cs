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


            // Verifica se o número de telemóvel foi inserido corretamente
            if (string.IsNullOrEmpty(tbTlm.Text))
            {
                // Exibe mensagem de erro
                MessageBox.Show("O número de telemóvel deve ser preenchido.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                tbTlm.Focus();
                return;
            }
            else if (tbTlm.Text.Length != 9)
            {
                // Exibe mensagem de erro
                MessageBox.Show("O número de telemóvel deve possuir 9 dígitos.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                tbTlm.Focus();
                return;
            }
            else if (!int.TryParse(tbTlm.Text, out int n))
            {
                // Exibe mensagem de erro
                MessageBox.Show("O número de telemóvel deve ser numérico.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                tbTlm.Focus();
                return;
            }
            // Concatena o prefixo "+351" com o número de telemóvel
            string numeroTelemovel = "+351" + tbTlm.Text;

            // Monta a consulta SQL de inserção
            string insertQuery = "INSERT INTO pacientes (Nome,Nif,Morada,Email,Tlm,Tlf,Obs) VALUES('" + tbNome.Text + "','" + tbNif.Text + "','" + tbMorada.Text + "','" + tbEmail.Text + "','" + numeroTelemovel + "','" + tbTlf.Text + "','" + tbobs.Text + "');";

            // Executa a consulta SQL
            executeMyQuery(insertQuery);

            // Exibe uma mensagem de sucesso
            MessageBox.Show("Os dados foram inseridos com sucesso.", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }






            //// Verifica se o número de telemóvel foi inserido
            //if (string.IsNullOrEmpty(tbTlm.Text))
            //{
            //    // Exibe uma mensagem de aviso e pergunta se deseja continuar sem inserido o número de telemóvel
            //    DialogResult result = MessageBox.Show("Pretende continuar sem inserir um número de telemóvel? Caso tenha modulo SMS o número de telemóvel é necessário para o envio das mesmas!", "Aviso", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            //    // Se o escolher "Sim", continua sem inserir o número de telemóvel
            //    if (result == DialogResult.Yes)
            //    {
            //        // Monta a consulta SQL de inserção
            //        string insertQuery = "INSERT INTO pacientes (Nome,Nif,Morada,Email,Tlm,Tlf,Obs) VALUES('" + tbNome.Text + "','" + tbNif.Text + "','" + tbMorada.Text + "','" + tbEmail.Text + "','" + tbTlm.Text + "','" + tbTlf.Text + "','" + tbobs.Text + "');";

            //        // Executa a consulta SQL
            //        executeMyQuery(insertQuery);

            //        // Exibe uma mensagem de sucesso
            //        MessageBox.Show("Os dados foram inseridos com sucesso.", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //    }
            //    else
            //    {
            //        // Se escolher "Não", posiciona o foco no campo de número de telemóvel
            //        tbTlm.Focus();
            //    }
            //}
            //else
            //{
            //    // Se o número de telemóvel foi inserido, monta a consulta SQL de inserção
            //    string insertQuery = "INSERT INTO pacientes (Nome,Nif,Morada,Email,Tlm,Tlf,Obs) VALUES('" + tbNome.Text + "','" + tbNif.Text + "','" + tbMorada.Text + "','" + tbEmail.Text + "','" + tbTlm.Text + "','" + tbTlf.Text + "','" + tbobs.Text + "');";

            //    // Executa a consulta SQL
            //    executeMyQuery(insertQuery);

            //    // Exibe uma mensagem de sucesso
            //    MessageBox.Show("Os dados foram inseridos com sucesso.", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //}

       

        private void pacientes_Load(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                // Configurando a conexão com o banco de dados MySQL
                string connectionString = (@"server=" + Properties.Settings.Default.server + ";database=" + Properties.Settings.Default.basedados + ";port=" + Properties.Settings.Default.porta + ";userid=" + Properties.Settings.Default.username + ";password=" + Properties.Settings.Default.password);

                MySqlConnection connection = new MySqlConnection(connectionString);
                connection.Open();

                // Criando um objeto DataTable e preenchendo-o com os dados da tabela pacientes
                DataTable dt = new DataTable();
                MySqlDataAdapter adapter = new MySqlDataAdapter("SELECT * FROM pacientes", connection);
                adapter.Fill(dt);

                // Atribuindo o objeto DataTable ao DataSource do DataGridView
                kryptonDataGridView1.DataSource = dt;

                // Fechando a conexão com o banco de dados
                connection.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao listar clientes: " + ex.Message);
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            // Verifica se o número de telemóvel foi inserido corretamente
            if (string.IsNullOrEmpty(tbTlm.Text))
            {
                // Exibe mensagem de erro
                MessageBox.Show("O número de telemóvel deve ser preenchido.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                tbTlm.Focus();
                return;
            }
            else if (tbTlm.Text.Length != 9)
            {
                // Exibe mensagem de erro
                MessageBox.Show("O número de telemóvel deve possuir 9 dígitos.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                tbTlm.Focus();
                return;
            }
            else if (!int.TryParse(tbTlm.Text, out int n))
            {
                // Exibe mensagem de erro
                MessageBox.Show("O número de telemóvel deve ser numérico.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                tbTlm.Focus();
                return;
            }
            // Concatena o prefixo "+351" com o número de telemóvel
            string numeroTelemovel = "+351" + tbTlm.Text;

            // Monta a consulta SQL de atualização
            string updateQuery = "UPDATE pacientes SET Nome = '" + tbNome.Text + "', Nif = '" + tbNif.Text + "', Morada = '" + tbMorada.Text + "', Email = '" + tbEmail.Text + "', Tlm = '" + numeroTelemovel + "', Tlf = '" + tbTlf.Text + "', Obs = '" + tbobs.Text + "' WHERE ID = " + tbId.Text + ";";

            // Executa a consulta SQL
            executeMyQuery(updateQuery);

            // Exibe uma mensagem de sucesso
            MessageBox.Show("Os dados foram atualizados com sucesso.", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void kryptonDataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            // Verifica se o clique foi na célula de uma linha e não no cabeçalho
            if (e.RowIndex >= 0)
            {
                // Obtém a linha clicada
                DataGridViewRow row = kryptonDataGridView1.Rows[e.RowIndex];

                // Obtém os valores das colunas e atribui às textboxes correspondentes
                tbId.Text = row.Cells[0].Value.ToString();
                tbNome.Text = row.Cells[1].Value.ToString();
                tbNif.Text = row.Cells[2].Value.ToString();
                tbTlm.Text = row.Cells[5].Value.ToString();
                tbEmail.Text = row.Cells[4].Value.ToString();
                tbTlf.Text = row.Cells[6].Value.ToString();
                tbMorada.Text = row.Cells[3].Value.ToString();
                tbobs.Text = row.Cells[7].Value.ToString();
            }
        }
    }

       
    }
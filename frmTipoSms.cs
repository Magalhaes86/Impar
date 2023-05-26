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
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Calendar.v3;
using Google.Apis.Services;
using Google.Apis.Calendar.v3.Data;
using System.Globalization;
using System.Threading;

using System.Net.Http;
using System.Threading.Tasks;
namespace Impar
{
    public partial class frmTipoSms : Form
    {
        public frmTipoSms()
        {
            InitializeComponent();
        }

        private void frmTipoSms_Load(object sender, EventArgs e)
        {

        }

        private void InserirTipoSms()
        {
            string tipoSms = kryptonTextBox2.Text;
            string mensagem = kryptonTextBox3.Text;

            string insertQuery = "INSERT INTO tiposms (TipoSms, Mensagem) VALUES (@TipoSms, @Mensagem)";
            MySqlCommand insertCommand = new MySqlCommand(insertQuery, connection);
            insertCommand.Parameters.AddWithValue("@TipoSms", tipoSms);
            insertCommand.Parameters.AddWithValue("@Mensagem", mensagem);

            try
            {
                connection.Open();
                insertCommand.ExecuteNonQuery();
                MessageBox.Show("Tipo de SMS inserido com sucesso!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao inserir o tipo de SMS: " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                connection.Close();
            }
        }

        private void AtualizarTipoSms()
        {
            int idTipoSms = Convert.ToInt32(kryptonTextBox1.Text);
            string tipoSms = kryptonTextBox2.Text;
            string mensagem = kryptonTextBox3.Text;

            string updateQuery = "UPDATE tiposms SET TipoSms = @TipoSms, Mensagem = @Mensagem WHERE idTipoSms = @IdTipoSms";
            MySqlCommand updateCommand = new MySqlCommand(updateQuery, connection);
            updateCommand.Parameters.AddWithValue("@TipoSms", tipoSms);
            updateCommand.Parameters.AddWithValue("@Mensagem", mensagem);
            updateCommand.Parameters.AddWithValue("@IdTipoSms", idTipoSms);

            try
            {
                connection.Open();
                updateCommand.ExecuteNonQuery();
                MessageBox.Show("Tipo de SMS atualizado com sucesso!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao atualizar o tipo de SMS: " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                connection.Close();
            }
        }



        //   MySqlConnection connection = new MySqlConnection(@"server=localhost;database=ContabSysDB;port=3308;userid=root;password=xd");
        MySqlConnection connection = new MySqlConnection(@"server=" + Properties.Settings.Default.server + ";database=" + Properties.Settings.Default.basedados + ";port=" + Properties.Settings.Default.porta + ";userid=" + Properties.Settings.Default.username + ";password=" + Properties.Settings.Default.password);

        MySqlCommand command;


        private void LerDadosTabelaAgendamentoSMS()
        {
            
                string tipoSmsQuery = "SELECT idTipoSms, TipoSms, Mensagem FROM tiposms";
                MySqlCommand tipoSmsCommand = new MySqlCommand(tipoSmsQuery, connection);
                MySqlDataAdapter tipoSmsAdapter = new MySqlDataAdapter(tipoSmsCommand);
                DataTable tipoSmsDataTable = new DataTable();

                try
                {
                    connection.Open();
                    tipoSmsAdapter.Fill(tipoSmsDataTable);

                    // Configurar as colunas do kryptonDataGridView1
                    kryptonDataGridView1.AutoGenerateColumns = false;
                    kryptonDataGridView1.Columns.Clear();

                    // Adicionar a coluna "Editar" na primeira posição
                    DataGridViewButtonColumn editarColumn = new DataGridViewButtonColumn();
                    editarColumn.Name = "Editar";
                    editarColumn.HeaderText = "Editar";
                    editarColumn.Text = "Editar";
                    editarColumn.UseColumnTextForButtonValue = true;
                    kryptonDataGridView1.Columns.Add(editarColumn);

                    // Adicionar as outras colunas manualmente
                    kryptonDataGridView1.Columns.Add(new DataGridViewTextBoxColumn
                    {
                        DataPropertyName = "idTipoSms",
                        Name = "ID",
                        HeaderText = "ID"
                    });

                    kryptonDataGridView1.Columns.Add(new DataGridViewTextBoxColumn
                    {
                        DataPropertyName = "TipoSms",
                        Name = "TipoSms",
                        HeaderText = "Tipo de SMS"
                    });

                    kryptonDataGridView1.Columns.Add(new DataGridViewTextBoxColumn
                    {
                        DataPropertyName = "Mensagem",
                        Name = "Mensagem",
                        HeaderText = "Mensagem"
                    });

                    // Preencher os dados do DataTable no kryptonDataGridView1
                    kryptonDataGridView1.DataSource = tipoSmsDataTable;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Erro ao carregar os dados da tabela 'tiposms': " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    connection.Close();
                }
            }





        private void kryptonButton3_Click(object sender, EventArgs e)
        {
            LerDadosTabelaAgendamentoSMS();
        }

        private void kryptonButton1_Click(object sender, EventArgs e)
        {
            InserirTipoSms();
        }

        private void kryptonButton2_Click(object sender, EventArgs e)
        {
            AtualizarTipoSms();
        }

        private void kryptonDataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex == kryptonDataGridView1.Columns["Editar"].Index)
            {
                DataGridViewRow row = kryptonDataGridView1.Rows[e.RowIndex];

                // Obter os valores da linha selecionada
                int idTipoSms = Convert.ToInt32(row.Cells["ID"].Value);
                string tipoSms = row.Cells["TipoSms"].Value.ToString();
                string mensagem = row.Cells["Mensagem"].Value.ToString();

                // Atribuir os valores aos campos correspondentes
                kryptonTextBox1.Text = idTipoSms.ToString();
                kryptonTextBox2.Text = tipoSms;
                kryptonTextBox3.Text = mensagem;
            }
        }
    }
}

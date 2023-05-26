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
    public partial class lsttiposms : Form
    {
        public lsttiposms()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            LerDadosTabelaAgendamentoSMS();
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




        private void lsttiposms_Load(object sender, EventArgs e)
        {

        }

        private void kryptonDataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                DataGridViewRow row = kryptonDataGridView1.Rows[e.RowIndex];

                if (kryptonCheckBox3.Checked)
                {
                    Marcacoes form = new Marcacoes();
                    form.kryptonTextBox1.Text = row.Cells["TipoSms"].Value.ToString();
                    form.kryptonTextBox2.Text = row.Cells["Mensagem"].Value.ToString();
                    form.ShowDialog();
                }
                else if (kryptonCheckBox1.Checked)
                {
                    Marcacoes form = new Marcacoes();
                    form.kryptonTextBox4.Text = row.Cells["TipoSms"].Value.ToString();
                    form.kryptonTextBox3.Text = row.Cells["Mensagem"].Value.ToString();
                    form.ShowDialog();
                }
                else if (kryptonCheckBox2.Checked)
                {
                    Marcacoes form = new Marcacoes();
                    form.kryptonTextBox6.Text = row.Cells["TipoSms"].Value.ToString();
                    form.kryptonTextBox5.Text = row.Cells["Mensagem"].Value.ToString();
                    form.ShowDialog();
                }
            }
        }
    }
}

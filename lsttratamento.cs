using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using MySql.Data;

namespace Impar
{
    public partial class lsttratamento : Form
    {
        public lsttratamento()
        {
            InitializeComponent();
        }

        private void LoadData()
        {
            try
            {
                // Configurar a conexão com o banco de dados MySQL
                string connectionString = (@"server=" + Properties.Settings.Default.server + ";database=" + Properties.Settings.Default.basedadosXD + ";port=" + Properties.Settings.Default.porta + ";userid=" + Properties.Settings.Default.username + ";password=" + Properties.Settings.Default.password);

                MySqlConnection connection = new MySqlConnection(connectionString);
                connection.Open();

                // Criar um objeto DataTable e preenchê-lo com os dados da tabela itens
                DataTable dt = new DataTable();
                //   MySqlDataAdapter adapter = new MySqlDataAdapter("SELECT Id, Description, RetailPrice1, PurchasePrice, GroupId FROM items", connection);
                MySqlDataAdapter adapter = new MySqlDataAdapter("SELECT Id, Description, RetailPrice1, PurchasePrice, GroupId FROM items WHERE GroupId <> 0", connection);
                adapter.Fill(dt);

                // Atribuir o objeto DataTable ao DataSource do DataGridView
                kryptonDataGridView1.DataSource = dt;

                // Fechar a conexão com o banco de dados
                connection.Close();

                ApplyFilter();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao listar itens: " + ex.Message);
            }
        }


        private void button1_Click(object sender, EventArgs e)
        {
            LoadData();
        }

        private void kryptonDataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                // Obtém os valores da linha selecionada
                string codtratamento = kryptonDataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString();
                string descricaotratamento = kryptonDataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString();
             
                // Procura o formulário Marcacoes entre os formulários abertos
                Marcacoes form = Application.OpenForms.OfType<Marcacoes>().FirstOrDefault();

                // Atualiza as propriedades do formulário se ele foi encontrado
                if (form != null)
                {
                    form.tbidtipotratamento.Text = codtratamento;
                    form.tbtipotratamento.Text = descricaotratamento;
                    
                }
                else
                {
                    // Cria um novo formulário se ele não foi encontrado
                    form = new Marcacoes();
                    form.tbidtipotratamento.Text = codtratamento;
                    form.tbtipotratamento.Text = descricaotratamento;
              
                    form.Show();
                }

                // Fecha o formulário atual
                this.Close();
            }
        }

        private void tbpesquisaId_TextChanged(object sender, EventArgs e)
        {
            BindingSource bs = new BindingSource();
            bs.DataSource = kryptonDataGridView1.DataSource;
            bs.Filter = string.Format("CONVERT(" + this.kryptonDataGridView1.Columns[0].DataPropertyName + ", System.String) like '%" + tbpesquisaId.Text.Replace("'", "''") + "%'");
            kryptonDataGridView1.DataSource = bs;
        }

        private void tbpesquisanome_TextChanged(object sender, EventArgs e)
        {
            BindingSource bs = new BindingSource();
            bs.DataSource = kryptonDataGridView1.DataSource;
            bs.Filter = string.Format("CONVERT(" + this.kryptonDataGridView1.Columns[1].DataPropertyName + ", System.String) like '%" + tbpesquisanome.Text.Replace("'", "''") + "%'");
            kryptonDataGridView1.DataSource = bs;
        }

        private void tbpesquisatlm_TextChanged(object sender, EventArgs e)
        {
            BindingSource bs = new BindingSource();
            bs.DataSource = kryptonDataGridView1.DataSource;
            bs.Filter = string.Format("CONVERT(" + this.kryptonDataGridView1.Columns[2].DataPropertyName + ", System.String) like '%" + tbpesquisanome.Text.Replace("'", "''") + "%'");
            kryptonDataGridView1.DataSource = bs;
        }

        private void tbpesquisatlf_TextChanged(object sender, EventArgs e)
        {
            BindingSource bs = new BindingSource();
            bs.DataSource = kryptonDataGridView1.DataSource;
            bs.Filter = string.Format("CONVERT(" + this.kryptonDataGridView1.Columns[3].DataPropertyName + ", System.String) like '%" + tbpesquisanome.Text.Replace("'", "''") + "%'");
            kryptonDataGridView1.DataSource = bs;
        }

        private void kryptonCheckBox1_CheckedChanged(object sender, EventArgs e)
        {
            LoadData();
            ApplyFilter();
        }

        private void ApplyFilter()
        {
            bool isCheckboxChecked = kryptonCheckBox1.Checked;

            if (kryptonDataGridView1.DataSource is DataTable dt)
            {
                // Criar um DataView a partir do DataTable
                DataView dataView = new DataView(dt);

                if (isCheckboxChecked)
                {
                    // Filtrar o DataView para exibir apenas as linhas com GroupId igual a 1
                    dataView.RowFilter = "GroupId = 1";
                }
                else
                {
                    // Limpar o filtro para exibir todas as linhas
                    dataView.RowFilter = "";
                }

                // Atribuir o DataView filtrado ao DataSource do DataGridView
                kryptonDataGridView1.DataSource = dataView;
            }
        }

    

        private void lsttratamento_Load(object sender, EventArgs e)
        {
            LoadData();
            ApplyFilter();
        }
    }
}

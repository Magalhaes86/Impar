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
    public partial class listagemPacientesMarcacoes : Form
    {
        public listagemPacientesMarcacoes()
        {
            InitializeComponent();

        }




        //   MySqlConnection connection = new MySqlConnection(@"server=localhost;database=ContabSysDB;port=3308;userid=root;password=xd");
        // MySqlConnection connection = new MySqlConnection(@"server=" + Properties.Settings.Default.server + ";database=" + Properties.Settings.Default.basedados + ";port=" + Properties.Settings.Default.porta + ";userid=" + Properties.Settings.Default.username + ";password=" + Properties.Settings.Default.password);


        //  MySqlCommand command;

        private void button1_Click(object sender, EventArgs e)
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

        private void kryptonDataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void kryptonDataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                // Obtém os valores da linha selecionada
                string codCliente = kryptonDataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString();
                string nomePaciente = kryptonDataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString();
                string tlmPaciente = kryptonDataGridView1.Rows[e.RowIndex].Cells[6].Value.ToString();

                // Procura o formulário Marcacoes entre os formulários abertos
                Marcacoes form = Application.OpenForms.OfType<Marcacoes>().FirstOrDefault();

                // Atualiza as propriedades do formulário se ele foi encontrado
                if (form != null)
                {
                    form.tbcodcliente.Text = codCliente;
                    form.tbnomepaciente.Text = nomePaciente;
                    form.tbtlmpaciente.Text = tlmPaciente;
                }
                else
                {
                    // Cria um novo formulário se ele não foi encontrado
                    form = new Marcacoes();
                    form.tbcodcliente.Text = codCliente;
                    form.tbnomepaciente.Text = nomePaciente;
                    form.tbtlmpaciente.Text = tlmPaciente;
                    form.Show();
                }

                // Fecha o formulário atual
                this.Close();
            }
        }

        private void listagemPacientesMarcacoes_Load(object sender, EventArgs e)
        {

        }

        private void kryptonHeaderGroup1_Paint(object sender, PaintEventArgs e)
        {

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
            bs.Filter = string.Format("CONVERT(" + this.kryptonDataGridView1.Columns[5].DataPropertyName + ", System.String) like '%" + tbpesquisatlm.Text.Replace("'", "''") + "%'");
            kryptonDataGridView1.DataSource = bs;
         

            }

        private void tbpesquisatlf_TextChanged(object sender, EventArgs e)
        {
            BindingSource bs = new BindingSource();
            bs.DataSource = kryptonDataGridView1.DataSource;
            bs.Filter = string.Format("CONVERT(" + this.kryptonDataGridView1.Columns[6].DataPropertyName + ", System.String) like '%" + tbpesquisatlf.Text.Replace("'", "''") + "%'");
            kryptonDataGridView1.DataSource = bs;
   
        }
    }
}
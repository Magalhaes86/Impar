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

                // Define os valores das textboxes no formulário Marcacoes
                Marcacoes form = new Marcacoes();
                form.tbcodcliente.Text = codCliente;
                form.tbnomepaciente.Text = nomePaciente;
                form.tbtlmpaciente.Text = tlmPaciente;

                // Abre o formulário Marcacoes
                form.Show();

                // Fecha o formulário atual
                this.Close();
            
        }
    }
    }
}

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

namespace Impar
{
    public partial class Marcacoes : Form
    {
        public Marcacoes()
        {
            InitializeComponent();
            timer1 = new System.Windows.Forms.Timer();
            timer1.Interval = 3000; // 3 segundos
            timer1.Tick += timer1_Tick;
        }

        private void Marcacoes_Load(object sender, EventArgs e)
        {

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
        private void GravarNovaMarcacao()
        {

            string smsEnviada = kryptonCheckBox1.Checked ? "1" : "0";
            string insertQuery = "INSERT INTO marcacoes (Idcliente, IDGoogle, Horario, TipoTratamento, Obs, Descricao, TituloGoogle, Horainicio, Horafim, Nome, telemovel, SmsEnviada) " +
                         "VALUES('" + tbcodcliente.Text + "','" + tbidgoogle.Text + "','" + tbhorario.Text + "','" + tbtipotratamento.Text + "','" + tbobs.Text + "','" + tbdescricao.Text + "','" + tbtitulogoogle.Text + "','" + tbhorainicio.Text + "','" + tbhorafim.Text + "','" + tbnomepaciente.Text + "','" + tbtlmpaciente.Text + "','" + smsEnviada + "');";
            executeMyQuery(insertQuery);



            // a ESTUDAR NAO ESTA CORRETO AINDA
      
            //string insertQuery = "INSERT INTO marcacoes (Idcliente,IDGoogle,Horario,TipoTratamento,Obs,Descricao,TituloGoogle,Horainicio,Horafim,Nome,telemovel) " +
            // "VALUES('" + tbcodcliente.Text + "','" + tbidgoogle.Text + "','" + tbhorario.Text + "','" + tbtipotratamento.Text + "','" + tbobs.Text + "','" + tbdescricao.Text + "','" + tbtitulogoogle.Text + "','" + tbhorainicio.Text + "','" + tbhorafim.Text + "','" + tbnomepaciente.Text + "','" + tbtlmpaciente.Text + "');";
            //executeMyQuery(insertQuery);
        }

        private void atualizarMarcacoes()
        {

            string smsEnviada = kryptonCheckBox1.Checked ? "1" : "0";
            string updateQuery = "UPDATE marcacoes SET Idcliente = '" + tbcodcliente.Text + "', Horario = '" + tbhorario.Text + "', TipoTratamento = '" + tbtipotratamento.Text + "', Obs = '" + tbobs.Text + "', Descricao = '" + tbdescricao.Text + "', TituloGoogle = '" + tbtitulogoogle.Text + "', Horainicio = '" + tbhorainicio.Text + "', Horafim = '" + tbhorafim.Text + "', Nome = '" + tbnomepaciente.Text + "', telemovel = '" + tbtlmpaciente.Text + "', SmsEnviada = '" + smsEnviada + "' WHERE IDGoogle = '" + tbidgoogle.Text + "';";
            executeMyQuery(updateQuery);


            // a ESTUDAR NAO ESTA CORRETO AINDA

            //string updateQuery = "UPDATE marcacoes SET Idcliente='" + tbcodcliente.Text + "', Horario='" + tbhorario2.Text + "', TipoTratamento='" + tbtipotratamento.Text + "', Obs='" + tbobs.Text + "', Descricao='" + tbdescricao.Text + "', TituloGoogle='" + tbtitulogoogle.Text + "', Horainicio='" + tbhorainicio.Text + "', Horafim='" + tbhorafim.Text + "', Nome='" + tbnomepaciente.Text + "', telemovel='" + tbtlmpaciente.Text + "' WHERE IDGoogle='" + tbidgoogle.Text + "';";
            //executeMyQuery(updateQuery);
        }


        private void atualizarMarcacoesnoGoogle()
        {

            // a ESTUDAR NAO ESTA CORRETO AINDA

        }



        //Funciona mas nao esta a passar as datas nem os campos definidos nas textbox.
        private void InserirMarcacoesnoGoogle()
        {

            GoogleCredential credential;

            using (var stream = new FileStream("C:\\Desenvolvimentos\\EmDesenvolvimento\\IMPAR\\Impar\\bin\\Debug\\keys.json", FileMode.Open, FileAccess.Read))
            {
                credential = GoogleCredential.FromStream(stream)
                .CreateScoped(CalendarService.Scope.Calendar);

            }


            // Criando o serviço de calendário
            var service = new CalendarService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = "GoogleAgenda"
            });

            // Define parameters of request.
            Event addEvent_body = new Event();

            addEvent_body.Summary = tbtitulogoogle.Text;
            addEvent_body.Location = "Local do Evento";
            addEvent_body.Description = tbdescricao.Text;
            addEvent_body.Start = new EventDateTime()
            {
                // DateTime = new DateTime(2023, 5, 7, 14, 0, 0),
                DateTime = new DateTime(tbhorario.Value.Year, tbhorario.Value.Month, tbhorario.Value.Day, tbhorainicio.Value.Hour, tbhorainicio.Value.Minute, 0),
                TimeZone = "Europe/Lisbon"
            };
            addEvent_body.End = new EventDateTime()
            {
                //DateTime = new DateTime(2023, 6, 7, 15, 0, 0),
                DateTime = new DateTime(tbhorario.Value.Year, tbhorario.Value.Month, tbhorario.Value.Day, tbhorafim.Value.Hour, tbhorafim.Value.Minute, 0),
                TimeZone = "Europe/Lisbon"
            };


            // Event addEvent = service.Events.Insert(addEvent_body, "marcosmagalhaes86@gmail.com").Execute();
            try
            {
                Event addEvent = service.Events.Insert(addEvent_body, "marcosmagalhaes86@gmail.com").Execute();
                string eventId = addEvent.Id;
                tbidgoogle.Text = eventId;
                MessageBox.Show("Evento inserido com sucesso!");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao inserir evento: {ex.Message}");
            }

        }


     

        private void button1_Click(object sender, EventArgs e)
        {
            //InserirMarcacoesnoGoogleComCampos();
            InserirMarcacoesnoGoogle();
        }


        private void timer1_Tick(object sender, EventArgs e)
        {
            if (button3.BackColor == Color.Green)
            {
                button3.BackColor = SystemColors.Control;
                timer1.Stop();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(tbcodcliente.Text))
            {
                MessageBox.Show("Por favor, selecione um paciente.");
                button3.Focus();
                button3.BackColor = Color.Green;
                // Inicia o timer para retornar a cor original do botão
                timer1.Start();

            }
            else
        {
            InserirMarcacoesnoGoogle();
            Thread.Sleep(1000); // Aguarda 1 segundo

            GravarNovaMarcacao();
        }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            // Abre o FormAdicionarEvento sem passar nenhum parâmetro
            listagemPacientesMarcacoes form = new listagemPacientesMarcacoes();
            form.Show();
        }

        private void btatualizar_Click(object sender, EventArgs e)
        {
            atualizarMarcacoes();
        }
    }
}

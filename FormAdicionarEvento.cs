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

namespace Impar
{
    public partial class FormAdicionarEvento : Form
    {

        public FormAdicionarEvento(string data, string horaInicio, string horaFim, string Idgoogle, string descricao, string titulo, string CodCliente)
        {
            InitializeComponent();
            this.Load += FormAdicionarEvento_Load;

            // Definindo os valores das TextBoxes
            tbidgoogle.Text = Idgoogle;
            tbdescricao.Text = descricao;
            tbtitulogoogle.Text = titulo;
            tbcodcliente.Text = CodCliente;

            // Definindo o valor do DateTimePicker
            tbhorario.Value = DateTime.Parse(data);
            tbhorainicio.Value = DateTime.ParseExact(horaInicio, "HH:mm", CultureInfo.InvariantCulture);
            tbhorafim.Value = DateTime.ParseExact(horaFim, "HH:mm", CultureInfo.InvariantCulture);
        }


        //public FormAdicionarEvento(string data, string horaInicio, string horaFim, string Idgoogle, string descricao, string titulo, string CodCliente)
        //{
        //    InitializeComponent();
        //    this.Load += FormAdicionarEvento_Load;

        //    // Definindo os valores das TextBoxes
        //    //tbhorario2.Text = data;
        //    //tbhorainicio2.Text = horaInicio;
        //    //tbhorafim2.Text = horaFim;
        //    tbidgoogle.Text = Idgoogle;
        //    tbdescricao.Text = descricao;
        //    tbtitulogoogle.Text = titulo;
        //    tbcodcliente.Text = CodCliente;

        //    DateTime data = DateTime.Parse(dataString);
        //    DateTime horaInicio = DateTime.ParseExact(horaInicioString, "HH:mm", CultureInfo.InvariantCulture);
        //    DateTime horaFim = DateTime.ParseExact(horaFimString, "HH:mm", CultureInfo.InvariantCulture);

        //    // Definindo o valor do DateTimePicker
        //    tbhorario.Value = data;
        //    tbhorainicio.Value = horaInicio;
        //    tbhorafim.Value = horaFim;

        //}

        public FormAdicionarEvento(string data, DateTime dateTime, DateTime dateTime1)
        {
            InitializeComponent();
        }

        public FormAdicionarEvento()
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


        private void button1_Click(object sender, EventArgs e)
        {
            //Criando uma nova instância do Google Credential
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

            // Definindo os parâmetros para a pesquisa do evento
            EventsResource.ListRequest request = service.Events.List("marcosmagalhaes86@gmail.com");
            request.TimeMin = DateTime.Now;
            request.ShowDeleted = false;
            request.SingleEvents = true;
            request.MaxResults = 10;
            request.OrderBy = EventsResource.ListRequest.OrderByEnum.StartTime;
            // Criando um novo evento
            Event newEvent = new Event
            {
                Summary = tbtitulogoogle.Text, // Título do evento
                Description = tbdescricao.Text // Descrição do evento
            };

            // Configurando a data e hora de início do evento
            DateTime startDateTime = DateTime.Parse(tbhorario2.Text);
            TimeSpan startTime = TimeSpan.Parse(tbhorainicio2.Text);
            DateTime start = startDateTime.Date + startTime;
            newEvent.Start = new EventDateTime { DateTime = start };

            // Configurando a data e hora de fim do evento
            DateTime endDateTime = DateTime.Parse(tbhorario2.Text);
            TimeSpan endTime = TimeSpan.Parse(tbhorafim2.Text);
            DateTime end = endDateTime.Date + endTime;
            newEvent.End = new EventDateTime { DateTime = end };

            try
            {
                // Inserindo o evento no Google Agenda
                newEvent = service.Events.Insert(newEvent, "primary").Execute();
                MessageBox.Show("Evento inserido com sucesso!");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao inserir evento: {ex.Message}");
            }
        }

        private void GravarNovaMarcacao()
        {

            // a ESTUDAR NAO ESTA CORRETO AINDA
            string insertQuery = "INSERT INTO marcacoes (Idcliente,IDGoogle,Horario,TipoTratamento,Obs,Descricao,TituloGoogle,Horainicio,Horafim) " +
             "VALUES('" + tbcodcliente.Text + "','" + tbidgoogle.Text + "','" + tbhorario2.Text + "','" + tbtipotratamento.Text + "','" + tbobs.Text + "','" + tbdescricao.Text + "','" + tbtitulogoogle.Text + "','" + tbhorainicio2.Text + "','" + tbhorafim2.Text + "');";
            executeMyQuery(insertQuery);
        }

        private void atualizarMarcacoes()
        {

            // a ESTUDAR NAO ESTA CORRETO AINDA
            string updateQuery = "UPDATE marcacoes SET Idcliente='" + tbcodcliente.Text + "', Horario='" + tbhorario2.Text + "', TipoTratamento='" + tbtipotratamento.Text + "', Obs='" + tbobs.Text + "', Descricao='" + tbdescricao.Text + "', TituloGoogle='" + tbtitulogoogle.Text + "', Horainicio='" + tbhorainicio2.Text + "', Horafim='" + tbhorafim2.Text + "' WHERE IDGoogle='" + tbidgoogle.Text + "';";
            executeMyQuery(updateQuery);
        }


        private void atualizarMarcacoesnoGoogle()
        {

            // a ESTUDAR NAO ESTA CORRETO AINDA
           
        }


        private void InserirMarcacoesnoGoogle()
        {

            // a ESTUDAR NAO ESTA CORRETO AINDA este codigo nao vai fazerr nada na tabela Marcaçoes da base de dadso mysql

        }


        private void button2_Click(object sender, EventArgs e)
        {
            atualizarMarcacoes();
        }

        private void FormAdicionarEvento_Load(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(tbIdMarcacao.Text))
            {
                btatualizar.Enabled = true;
            }
        }
    }
}

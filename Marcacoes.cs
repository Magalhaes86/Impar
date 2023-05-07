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
    public partial class Marcacoes : Form
    {
        public Marcacoes()
        {
            InitializeComponent();
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

            addEvent_body.Summary = "test summary";
            addEvent_body.Location = "test location";
            addEvent_body.Description = "test description";
            addEvent_body.Start = new EventDateTime()
            {
                DateTime = new DateTime(2023, 5, 7, 14, 0, 0),
                TimeZone = "Europe/Lisbon"
            };
            addEvent_body.End = new EventDateTime()
            {
                DateTime = new DateTime(2023, 6, 7, 15, 0, 0),
                TimeZone = "Europe/Lisbon"
            };
          
 
            Event addEvent = service.Events.Insert(addEvent_body, "marcosmagalhaes86@gmail.com").Execute();
           
        }




        //EM TESTES
        private void InserirMarcacoesnoGoogleComCampos()
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


            Event addEvent = service.Events.Insert(addEvent_body, "marcosmagalhaes86@gmail.com").Execute();

        }




        private void InserirMarcacoesnoGoogle2()
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

            // Criando um novo evento
            Event newEvent = new Event
            {
                Summary = tbtitulogoogle.Text, // Título do evento
                Description = tbdescricao.Text // Descrição do evento
            };

            // Definindo a data e hora de início do evento
            DateTime start = new DateTime(2023, 5, 7, 22, 10, 0, DateTimeKind.Local); // Data e hora de início
            string startString = start.ToUniversalTime().ToString("yyyy-MM-ddTHH:mm:ss.fffZ");
            newEvent.Start = new EventDateTime { DateTime = DateTime.Parse(startString), TimeZone = "Europe/Lisbon" };

            // Definindo a data e hora do fim do evento
            DateTime end = new DateTime(2023, 5, 7, 22, 40, 0, DateTimeKind.Local); // Data e hora do fim
            string endString = end.ToUniversalTime().ToString("yyyy-MM-ddTHH:mm:ss.fffZ");
            newEvent.End = new EventDateTime { DateTime = DateTime.Parse(endString), TimeZone = "Europe/Lisbon" };


           // Verifica se tem permissão para inserir eventos na agenda
try
            {
                // Lendo lista de eventos da agenda
                EventsResource.ListRequest request = service.Events.List("primary");
                Events events = request.Execute();

                // Se chegou aqui, tem permissão para inserir eventos na agenda
                MessageBox.Show("Tem permissão para inserir eventos na agenda!");
            }
            catch (Exception ex)
            {
                // Se der acesso negado, não tem permissão para inserir eventos na agenda
                MessageBox.Show($"Não tem permissão para inserir eventos na agenda: {ex.Message}");
                return;
            }
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

        //        newEvent.Start = new EventDateTime { DateTime = DateTime.Parse(startString) };
        //   
        //    newEvent.End = new EventDateTime { DateTime = DateTime.Parse(endString)};
        //
        private void CreateEvent(CalendarService _service)
        {
            Event body = new Event();
            EventAttendee a = new EventAttendee();
            a.Email = "marcosmagalhaes86@gmail.com";
            EventAttendee b = new EventAttendee();
            b.Email = "marcosmagalhaes86@gmail.com";
            List<EventAttendee> attendes = new List<EventAttendee>();
            attendes.Add(a);
            attendes.Add(b);
            body.Attendees = attendes;
            EventDateTime start = new EventDateTime();
            start.DateTime = Convert.ToDateTime("2023-05-07T09:00:00+0530");
            EventDateTime end = new EventDateTime();
            end.DateTime = Convert.ToDateTime("2023-05-07T11:00:00+0530");
            body.Start = start;
            body.End = end;
            body.Location = "Europe/Lisbon";
            body.Summary = "Discussion about new Spidey suit";
            EventsResource.InsertRequest request = new EventsResource.InsertRequest(_service, body, "marcosmagalhaes86@gmail.com");
            Event response = request.Execute();
        }

     

        private void button1_Click(object sender, EventArgs e)
        {
            InserirMarcacoesnoGoogleComCampos();
           // InserirMarcacoesnoGoogle();
        }

    }
}

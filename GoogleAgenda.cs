using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Calendar.v3;
using Google.Apis.Services;
using Google.Apis.Calendar.v3.Data;

namespace Impar
{
    public partial class GoogleAgenda : Form
    {
        public GoogleAgenda()
        {
            InitializeComponent();
        }

        private void kryptonMonthCalendar1_DateChanged(object sender, DateRangeEventArgs e)
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
            request.TimeMin = new DateTime(e.Start.Year, e.Start.Month, e.Start.Day, 0, 0, 0);
            request.TimeMax = new DateTime(e.End.Year, e.End.Month, e.End.Day, 23, 59, 59);
            request.ShowDeleted = false;
            request.SingleEvents = true;
            request.MaxResults = 10;
            request.OrderBy = EventsResource.ListRequest.OrderByEnum.StartTime;

            // Obtendo os eventos da agenda
            Events events = request.Execute();

            // Criando o DataTable
            DataTable dt = new DataTable();
            dt.Columns.Add("Título");
            dt.Columns.Add("Data de Início");
            dt.Columns.Add("Hora de Início");
            dt.Columns.Add("Data de Fim");
            dt.Columns.Add("Hora de Fim");
            dt.Columns.Add("Descrição");

            // Preenchendo o DataTable com os eventos da agenda
            foreach (var eventItem in events.Items)
            {
                DataRow row = dt.NewRow();
                row["Título"] = eventItem.Summary;
                row["Data de Início"] = eventItem.Start.DateTime.Value.ToLocalTime().ToString("dd/MM/yyyy");
                row["Hora de Início"] = eventItem.Start.DateTime.Value.ToLocalTime().ToString("HH:mm:ss");
                row["Data de Fim"] = eventItem.End.DateTime.Value.ToLocalTime().ToString("dd/MM/yyyy");
                row["Hora de Fim"] = eventItem.End.DateTime.Value.ToLocalTime().ToString("HH:mm:ss");
                row["Descrição"] = eventItem.Description;
                dt.Rows.Add(row);
            }

            // Exibindo o DataTable no DataGridView
            kryptonDataGridView1.DataSource = dt;
        }

        private void GoogleAgenda_Load(object sender, EventArgs e)
        {

        }

        private void kryptonButton1_Click(object sender, EventArgs e)
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

            // Obtendo os eventos da agenda
            Events events = request.Execute();

            // Criando o DataTable
            DataTable dt = new DataTable();
            dt.Columns.Add("Título");
            dt.Columns.Add("Data de Início");
            dt.Columns.Add("Hora de Início");
            dt.Columns.Add("Data de Fim");
            dt.Columns.Add("Hora de Fim");
            dt.Columns.Add("Descrição");

            // Preenchendo o DataTable com os eventos da agenda
            foreach (var eventItem in events.Items)
            {
                DataRow row = dt.NewRow();
                row["Título"] = eventItem.Summary;
                row["Data de Início"] = eventItem.Start.DateTime.Value.ToLocalTime().ToString("dd/MM/yyyy");
                row["Hora de Início"] = eventItem.Start.DateTime.Value.ToLocalTime().ToString("HH:mm:ss");
                row["Data de Fim"] = eventItem.End.DateTime.Value.ToLocalTime().ToString("dd/MM/yyyy");
                row["Hora de Fim"] = eventItem.End.DateTime.Value.ToLocalTime().ToString("HH:mm:ss");
                row["Descrição"] = eventItem.Description;
                dt.Rows.Add(row);
            }

            // Exibindo o DataTable no DataGridView
            kryptonDataGridView1.DataSource = dt;
        
    }

        private void kryptonButton2_Click(object sender, EventArgs e)
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

            // Obtendo os eventos da agenda
            Events events = request.Execute();

            // Criando o DataTable
            DataTable dt = new DataTable();
            dt.Columns.Add("Título");
            dt.Columns.Add("Data/Hora de Início");
            dt.Columns.Add("Data/Hora de Fim");

            // Preenchendo o DataTable com os eventos da agenda
            foreach (var eventItem in events.Items)
            {
                DataRow row = dt.NewRow();
                row["Título"] = eventItem.Summary;
                row["Data/Hora de Início"] = eventItem.Start.DateTime.Value.ToLocalTime();
                row["Data/Hora de Fim"] = eventItem.End.DateTime.Value.ToLocalTime();
                dt.Rows.Add(row);


            }

            // Exibindo o DataTable no DataGridView
            kryptonDataGridView1.DataSource = dt;
        }

        private void kryptonWrapLabel1_Click(object sender, EventArgs e)
        {

        }


        private void button1_Click(object sender, EventArgs e)
        {
            DateTime dtDataInicio = DtDataInicio.Value.Date;
            DateTime dtDataFim = DtDataFim.Value.Date;
            TimeSpan dtHoraInicioManha = DtHoraInicioManha.Value.TimeOfDay;
            TimeSpan dtHoraFimManha = DtHoraFimManha.Value.TimeOfDay;
            TimeSpan dtHoraInicioTarde = DtHoraInicioTarde.Value.TimeOfDay;
            TimeSpan dtHoraFimTarde = DtHoraFimTarde.Value.TimeOfDay;

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

            request.TimeMin = dtDataInicio;
            request.TimeMax = dtDataFim;
            request.ShowDeleted = false;
            request.SingleEvents = true;
            request.OrderBy = EventsResource.ListRequest.OrderByEnum.StartTime;

            // Obtendo os eventos da agenda
            Events events = request.Execute();

            // Criando a lista de horários disponíveis
            List<DateTime> horariosDisponiveis = new List<DateTime>();

            // Criando a DataTable que será usada como data source do DataGridView
            var dataTable = new DataTable();
            dataTable.Columns.Add("Data", typeof(DateTime));
            dataTable.Columns.Add("Horário", typeof(TimeSpan));
            dataTable.Columns.Add("Duração", typeof(TimeSpan));
            dataTable.Columns.Add("Status", typeof(string));

            // Adicionando horários disponíveis no período da manhã
            DateTime dtHorario = dtDataInicio + dtHoraInicioManha;
            while (dtHorario + new TimeSpan(1, 0, 0) <= dtDataFim + dtHoraFimManha)
            {
                bool horarioDisponivel = true;

                foreach (var evento in events.Items)
                {
                    DateTime dtInicioEvento = evento.Start.DateTime ?? DateTime.Parse(evento.Start.Date);
                    DateTime dtFimEvento = evento.End.DateTime ?? DateTime.Parse(evento.End.Date);

                    if (dtHorario >= dtInicioEvento && dtHorario < dtFimEvento)
                    {
                        horarioDisponivel = false;
                        break;
                    }
                }

                // Se o horário estiver disponível, adiciona na lista de horários disponíveis
                if (horarioDisponivel)
                {
                    horariosDisponiveis.Add(dtHorario);
                    dataTable.Rows.Add(dtHorario.Date, dtHorario.TimeOfDay, new TimeSpan(1, 0, 0), "Disponível");
                }

                // Incrementando 30 minutos
                dtHorario = dtHorario + new TimeSpan(0, 30, 0);
            }

            // Adicionando horários disponíveis no período da tarde
            dtHorario = dtDataInicio + dtHoraInicioTarde;
            while (dtHorario + new TimeSpan(1, 0, 0) <= dtDataFim + dtHoraFimTarde)
            {
                bool horarioDisponivel = true;

                // Verificando se o horário está disponível
                foreach (var evento in events.Items)
                {
                    DateTime dtInicioEvento = DateTime.Parse(evento.Start.DateTimeRaw);
                    DateTime dtFimEvento = DateTime.Parse(evento.End.DateTimeRaw);

                    if (dtHorario >= dtInicioEvento && dtHorario < dtFimEvento)
                    {
                        horarioDisponivel = false;
                        break;
                    }
                }

                // Se o horário estiver disponível, adiciona na lista de horários disponíveis
                if (horarioDisponivel)
                {
                    horariosDisponiveis.Add(dtHorario);
                    dataTable.Rows.Add(dtHorario.Date, dtHorario.TimeOfDay, new TimeSpan(1, 0, 0), "Disponível");
                }

                // Incrementando 30 minutos
                dtHorario = dtHorario + new TimeSpan(0, 30, 0);
            }

            // Exibindo os horários disponíveis no DataGridView

            dataGridView1.DataSource = dataTable;

            // Setando o DataGridView para usar a DataTable como data source

        }


        private void button2_Click(object sender, EventArgs e)
        {
            DateTime dtDataInicio = DtDataInicio.Value.Date;
            DateTime dtDataFim = DtDataFim.Value.Date;
            TimeSpan dtHoraInicioManha = DtHoraInicioManha.Value.TimeOfDay;
            TimeSpan dtHoraFimManha = DtHoraFimManha.Value.TimeOfDay;
            TimeSpan dtHoraInicioTarde = DtHoraInicioTarde.Value.TimeOfDay;
            TimeSpan dtHoraFimTarde = DtHoraFimTarde.Value.TimeOfDay;

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

            request.TimeMin = dtDataInicio;
            request.TimeMax = dtDataFim;
            request.ShowDeleted = false;
            request.SingleEvents = true;
            request.OrderBy = EventsResource.ListRequest.OrderByEnum.StartTime;

            // Obtendo os eventos da agenda
            Events events = request.Execute();

            // Criando a lista de horários disponíveis
            List<DateTime> horariosDisponiveis = new List<DateTime>();

            // Criando a DataTable que será usada como data source do DataGridView
            var dataTable = new DataTable();
            dataTable.Columns.Add("Data", typeof(DateTime));
            dataTable.Columns.Add("Horário", typeof(TimeSpan));
            dataTable.Columns.Add("Duração", typeof(TimeSpan));
            dataTable.Columns.Add("Status", typeof(string));

            // Adicionando horários disponíveis no período da manhã
            DateTime dtHorario = dtDataInicio + dtHoraInicioManha;
            while (dtHorario + new TimeSpan(1, 0, 0) <= dtDataFim + dtHoraFimManha)
            {
                bool horarioDisponivel = true;

                foreach (var evento in events.Items)
                {
                    DateTime dtInicioEvento = evento.Start.DateTime ?? DateTime.Parse(evento.Start.Date);
                    DateTime dtFimEvento = evento.End.DateTime ?? DateTime.Parse(evento.End.Date);

                    if (dtHorario >= dtInicioEvento && dtHorario < dtFimEvento)
                    {
                        horarioDisponivel = false;
                        break;
                    }
                }

                // Se o horário estiver disponível, adiciona na lista de horários disponíveis
                if (horarioDisponivel)
                {
                    horariosDisponiveis.Add(dtHorario);
                    dataTable.Rows.Add(dtHorario.Date, dtHorario.TimeOfDay, new TimeSpan(1, 0, 0), "Disponível");
                }

                // Incrementando 30 minutos
                dtHorario = dtHorario + new TimeSpan(0, 30, 0);
            }

            // Adicionando horários disponíveis no período da tarde
            dtHorario = dtDataInicio + dtHoraInicioTarde;
            while (dtHorario + new TimeSpan(1, 0, 0) <= dtDataFim + dtHoraFimTarde)
            {
                bool horarioDisponivel = true;

                // Verificando se o horário está disponível
                foreach (var evento in events.Items)
                {
                    DateTime dtInicioEvento = DateTime.Parse(evento.Start.DateTimeRaw);
                    DateTime dtFimEvento = DateTime.Parse(evento.End.DateTimeRaw);

                    if (dtHorario >= dtInicioEvento && dtHorario < dtFimEvento)
                    {
                        horarioDisponivel = false;
                        break;
                    }
                }

                // Se o horário estiver disponível, adiciona na lista de horários disponíveis
                if (horarioDisponivel)
                {
                    horariosDisponiveis.Add(dtHorario);
                    dataTable.Rows.Add(dtHorario.Date, dtHorario.TimeOfDay, new TimeSpan(1, 0, 0), "Disponível");
                }

                // Incrementando 30 minutos
                dtHorario = dtHorario + new TimeSpan(0, 30, 0);
            }

            // Exibindo os horários disponíveis no DataGridView

            dataGridView1.DataSource = dataTable;

            // Setando o DataGridView para usar a DataTable como data source

        }



        private void button3_Click(object sender, EventArgs e)
        {
            
                DateTime dtDataInicio = DtDataInicio.Value.Date;
                DateTime dtDataFim = DtDataFim.Value.Date;
                TimeSpan dtHoraInicioManha = DtHoraInicioManha.Value.TimeOfDay;
                TimeSpan dtHoraFimManha = DtHoraFimManha.Value.TimeOfDay;
                TimeSpan dtHoraInicioTarde = DtHoraInicioTarde.Value.TimeOfDay;
                TimeSpan dtHoraFimTarde = DtHoraFimTarde.Value.TimeOfDay;

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

                request.TimeMin = dtDataInicio;
                request.TimeMax = dtDataFim;
                request.ShowDeleted = false;
                request.SingleEvents = true;
                request.OrderBy = EventsResource.ListRequest.OrderByEnum.StartTime;

                // Obtendo os eventos da agenda
                Events events = request.Execute();

                // Criando a lista de horários disponíveis
                List<DateTime> horariosDisponiveis = new List<DateTime>();

                // Adicionando os horários disponíveis da manhã
                DateTime dtHoraInicioManhaDateTime = dtDataInicio + dtHoraInicioManha;
                DateTime dtHoraFimManhaDateTime = dtDataInicio + dtHoraFimManha;
                if (dtHoraInicioManhaDateTime > dtDataInicio && dtHoraInicioManhaDateTime < dtDataFim)
                {
                    horariosDisponiveis.Add(dtHoraInicioManhaDateTime);
                }
                if (dtHoraFimManhaDateTime > dtDataInicio && dtHoraFimManhaDateTime < dtDataFim)
                {
                    horariosDisponiveis.Add(dtHoraFimManhaDateTime);
                }

                // Adicionando os horários disponíveis da tarde
                DateTime dtHoraInicioTardeDateTime = dtDataInicio + dtHoraInicioTarde;
                DateTime dtHoraFimTardeDateTime = dtDataInicio + dtHoraFimTarde;
                if (dtHoraInicioTardeDateTime > dtDataInicio && dtHoraInicioTardeDateTime < dtDataFim)
                {
                    horariosDisponiveis.Add(dtHoraInicioTardeDateTime);
                    
                    }
                    if (dtHoraFimTardeDateTime > dtDataInicio && dtHoraFimTardeDateTime < dtDataFim)
                    {
                        horariosDisponiveis.Add(dtHoraFimTardeDateTime);
                    }
                // Adicionando os horários de almoço como indisponíveis
                DateTime dtHoraFimManhaMaisIntervaloDateTime = dtDataInicio + dtHoraFimManha + TimeSpan.FromMinutes(60);
                DateTime dtHoraInicioTardeMenosIntervaloDateTime = dtDataInicio + dtHoraInicioTarde - TimeSpan.FromMinutes(60);
                if (dtHoraFimManhaMaisIntervaloDateTime > dtDataInicio && dtHoraFimManhaMaisIntervaloDateTime < dtDataFim)
                {
                    horariosDisponiveis.Remove(dtHoraFimManhaDateTime);
                }
                if (dtHoraInicioTardeMenosIntervaloDateTime > dtDataInicio && dtHoraInicioTardeMenosIntervaloDateTime < dtDataFim)
                {
                    horariosDisponiveis.Remove(dtHoraInicioTardeDateTime);
                }

                // Ordenando a lista de horários disponíveis
                horariosDisponiveis.Sort();

                // Criando a DataTable que será usada como data source do DataGridView
                DataTable dataTable = new DataTable();
                dataTable.Columns.Add("Data", typeof(DateTime));
                dataTable.Columns.Add("HoraInicio", typeof(TimeSpan));
                dataTable.Columns.Add("Duracao", typeof(TimeSpan));
                dataTable.Columns.Add("Status", typeof(string));

                // Adicionando as linhas à DataTable
                for (int i = 0; i < horariosDisponiveis.Count - 1; i++)
                {
                    dataTable.Rows.Add(horariosDisponiveis[i].Date, horariosDisponiveis[i].TimeOfDay,
                        horariosDisponiveis[i + 1].TimeOfDay - horariosDisponiveis[i].TimeOfDay,
                        "Disponível");
                }

                // Definindo a DataTable como o DataSource do DataGridView
                dataGridView1.DataSource = dataTable;


            }
    }
}
    



//}

//// Exibindo a DataTable no DataGridView
//dataGridView1.DataSource = dataTable;

//        }
//    }
//}
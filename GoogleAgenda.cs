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
            dataGridView1.CellFormatting += DataGridView1_CellFormatting;
        }

      


        private void DataGridView1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dataGridView1.Rows[e.RowIndex];
                DataGridViewCell statusCell = row.Cells["Status"];

                if (statusCell != null && statusCell.Value != null)
                {
                    string status = statusCell.Value.ToString();

                    if (status == "Ocupado")
                    {
                        row.DefaultCellStyle.BackColor = Color.Red;
                    }
                    else if (status == "Disponível")
                    {
                        row.DefaultCellStyle.BackColor = Color.Green;
                    }
                }
            }
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

            // Criando a DataTable
            DataTable dataTable = new DataTable();

            // Adicionando as colunas na DataTable
            dataTable.Columns.Add("Data", typeof(DateTime));
            dataTable.Columns.Add("Horário", typeof(TimeSpan));
            dataTable.Columns.Add("Duração", typeof(TimeSpan));
            dataTable.Columns.Add("Status", typeof(string)); // Nova coluna "Status"
            dataTable.Columns.Add("Intervalo", typeof(TimeSpan));


            // Definindo a duração mínima para os eventos
            TimeSpan duracaoMinimaEvento = new TimeSpan(0, 15, 0);


            // Adicionando horários disponíveis no período da manhã
            DateTime dtHorario = dtDataInicio + dtHoraInicioManha;
            while (dtHorario + new TimeSpan(1, 0, 0) <= dtDataFim + dtHoraFimManha)
            {
                bool horarioDisponivel = true;
                string status = "Disponível";
                TimeSpan duracao = new TimeSpan(1, 0, 0); // duração padrão de 1 hora

                // Verificando se o horário está ocupado
                foreach (var evento in events.Items)
                {
                    DateTime dtInicioEvento = evento.Start.DateTime ?? DateTime.Parse(evento.Start.Date);
                    DateTime dtFimEvento = evento.End.DateTime ?? DateTime.Parse(evento.End.Date);

                    // if (dtHorario >= dtInicioEvento && dtHorario < dtFimEvento)
                    if ((dtInicioEvento - dtHorario) < duracaoMinimaEvento && (dtFimEvento - dtHorario).CompareTo(duracaoMinimaEvento) >= 0)
                    {
                        horarioDisponivel = false;
                        status = "Ocupado";
                        if ((dtFimEvento - dtHorario) < duracaoMinimaEvento) // verifica se a duração do evento é menor que a duração mínima permitida
                        {
                            duracao = dtFimEvento - dtHorario; // atualiza a duração do evento na DataTable
                            break;
                        }
                    }
                }

                // Definindo horários de início e término do intervalo de almoço
                DateTime dtInicioAlmoco = dtDataInicio + dtHoraFimManha;
                DateTime dtFimAlmoco = dtDataInicio + dtHoraInicioTarde;

                if (dtHorario >= dtInicioAlmoco && dtHorario < dtFimAlmoco)
                {
                    horarioDisponivel = false;
                    status = "Intervalo de almoço";
                }

                // Se o horário estiver disponível, adiciona na lista de horários disponíveis
                if (horarioDisponivel)
                {
                    horariosDisponiveis.Add(dtHorario);
                }

                // Calculando o intervalo
                TimeSpan intervalo = dtHorario.TimeOfDay.Add(new TimeSpan(0, 30, 0));
                // Adicionando o horário na DataTable com o status correspondente
                dataTable.Rows.Add(dtHorario.Date, dtHorario.TimeOfDay, new TimeSpan(1, 0, 0), status, intervalo);

                // Incrementando 30 minutos
                dtHorario = dtHorario + new TimeSpan(0, 30, 0);
            }


            // Adicionando horários disponíveis no período da tarde
            dtHorario = dtDataInicio + dtHoraInicioTarde;
            while (dtHorario + new TimeSpan(1, 0, 0) <= dtDataFim + dtHoraFimTarde)
            {
                bool horarioDisponivel = true;
                string status = "Disponível";

                // Verificando se o horário está ocupado
                foreach (var evento in events.Items)
                {
                    DateTime dtInicioEvento = evento.Start.DateTime ?? DateTime.Parse(evento.Start.Date);
                    DateTime dtFimEvento = evento.End.DateTime ?? DateTime.Parse(evento.End.Date);

                    if (dtHorario >= dtInicioEvento && dtHorario < dtFimEvento)
                    {
                        horarioDisponivel = false;
                        status = "Ocupado";
                        break;
                    }
                }
                // Definindo horários de início e término do intervalo de almoço
                DateTime dtInicioAlmoco = dtDataInicio + dtHoraFimManha;
                DateTime dtFimAlmoco = dtDataInicio + dtHoraInicioTarde;

                if (dtHorario >= dtInicioAlmoco && dtHorario < dtFimAlmoco)
                {
                    horarioDisponivel = false;
                    status = "Intervalo de almoço";
                }

                // Se o horário estiver disponível, adiciona na lista de horários disponíveis
                if (horarioDisponivel)
                {
                    horariosDisponiveis.Add(dtHorario);
                }
                // Calculando o intervalo
                TimeSpan intervalo = dtHorario.TimeOfDay.Add(new TimeSpan(0, 30, 0));


                // Adicionando o horário na DataTable com o status correspondente
                dataTable.Rows.Add(dtHorario.Date, dtHorario.TimeOfDay, new TimeSpan(1, 0, 0), status, intervalo);

                // Incrementando 30 minutos
                dtHorario = dtHorario + new TimeSpan(0, 30, 0);
            }
            // Exibindo os horários disponíveis no DataGridView

            dataGridView1.DataSource = dataTable;

            // Setando o DataGridView para usar a DataTable como data source
        }




        private void button2_Click(object sender, EventArgs e)
        {

            // Criando uma nova instância do Google Credential
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
            request.TimeMin = DtDataInicio.Value.Date;
            request.TimeMax = DtDataFim.Value.Date.AddDays(1);
            request.ShowDeleted = false;
            request.SingleEvents = true;
            request.OrderBy = EventsResource.ListRequest.OrderByEnum.StartTime;

            // Executando a pesquisa
            Events events = request.Execute();

            // Processando os eventos encontrados
            List<Agendamento> agendamentos = new List<Agendamento>();
            if (events.Items != null && events.Items.Count > 0)
            {
                foreach (var eventItem in events.Items)
                {
                    agendamentos.Add(new Agendamento()
                    {
                        Inicio = eventItem.Start.DateTime.Value,
                        Fim = eventItem.End.DateTime.Value
                    });
                }
            }

            DateTime startDate = DtDataInicio.Value.Date;
            DateTime endDate = DtDataFim.Value.Date;
            DateTime morningStartTime = DtHoraInicioManha.Value;
            DateTime morningEndTime = DtHoraFimManha.Value;
            DateTime afternoonStartTime = DtHoraInicioTarde.Value;
            DateTime afternoonEndTime = DtHoraFimTarde.Value;

            List<DateTime> horariosDisponiveis = GetAvailableTimeSlots(startDate, endDate, morningStartTime, morningEndTime, afternoonStartTime, afternoonEndTime);

            foreach (Agendamento agendamento in agendamentos)
            {
                for (int i = horariosDisponiveis.Count - 1; i >= 0; i--)
                {
                    DateTime horarioDisponivel = horariosDisponiveis[i];
                    if (horarioDisponivel >= agendamento.Inicio && horarioDisponivel < agendamento.Fim)
                    {
                        horariosDisponiveis.RemoveAt(i);
                    }
                }
            }

            DataTable dt = new DataTable();
            dt.Columns.Add("Horário");
            dt.Columns.Add("Status");
            dt.Columns.Add("Título"); // Adicionando a coluna "Título"
            dt.Columns.Add("Descrição"); // Adicionando a coluna "Descrição"

            foreach (DateTime horario in horariosDisponiveis)
            {
                dt.Rows.Add(horario.ToString("HH:mm"), "Disponível", "", ""); // Incluindo valores vazios para as colunas "Título" e "Descrição" quando o horário estiver disponível
            }

            foreach (Agendamento agendamento in agendamentos)
            {
                // Obtendo o evento correspondente ao agendamento
                var eventItem = events.Items.FirstOrDefault(ev => ev.Start.DateTime.Value == agendamento.Inicio && ev.End.DateTime.Value == agendamento.Fim);

                if (eventItem != null)
                {
                    string titulo = eventItem.Summary;
                    string descricao = eventItem.Description;

                    dt.Rows.Add(agendamento.Inicio.ToString("HH:mm") + " - " + agendamento.Fim.ToString("HH:mm"), "Ocupado", titulo, descricao);
                }
                else
                {
                    dt.Rows.Add(agendamento.Inicio.ToString("HH:mm") + " - " + agendamento.Fim.ToString("HH:mm"), "Ocupado", "", "");
                }
            }

            dataGridView1.DataSource = dt;
        }
    

        private TimeSpan intervaloMarcacoes;

        private void button3_Click(object sender, EventArgs e)
        {
            DateTime dtDataInicio = DtDataInicio.Value.Date;
            DateTime dtDataFim = DtDataFim.Value.Date;
            TimeSpan dtHoraInicioManha = DtHoraInicioManha.Value.TimeOfDay;
            TimeSpan dtHoraFimManha = DtHoraFimManha.Value.TimeOfDay;
            TimeSpan dtHoraInicioTarde = DtHoraInicioTarde.Value.TimeOfDay;
            TimeSpan dtHoraFimTarde = DtHoraFimTarde.Value.TimeOfDay;
          
            //intervaloMarcacoes = Intervalomarcaçoes.Value.TimeOfDay;


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

            // Criando a DataTable
            DataTable dataTable = new DataTable();



            // Adicionando as colunas na DataTable
            dataTable.Columns.Add("Data", typeof(DateTime));
            dataTable.Columns.Add("Horário", typeof(TimeSpan));
            dataTable.Columns.Add("Duração", typeof(TimeSpan));
            dataTable.Columns.Add("Status", typeof(string)); // Nova coluna "Status"
            dataTable.Columns.Add("Intervalo", typeof(TimeSpan));

            // Adicionando horários disponíveis no período da manhã
            DateTime dtHorario = dtDataInicio + dtHoraInicioManha;
            while (dtHorario < dtDataInicio + dtHoraFimManha)
            {
                bool horarioDisponivel = true;
                string status = "Disponível";
                TimeSpan duracao = new TimeSpan(1, 0, 0);

                // Verificando se o horário está ocupado
                foreach (var evento in events.Items)
                {
                    DateTime dtInicioEvento = evento.Start.DateTime ?? DateTime.Parse(evento.Start.Date);
                    DateTime dtFimEvento = evento.End.DateTime ?? DateTime.Parse(evento.End.Date);

                    if (dtHorario >= dtInicioEvento && dtHorario < dtFimEvento)
                    {
                        horarioDisponivel = false;
                        status = "Ocupado";
                        duracao = dtFimEvento - dtInicioEvento;
                        break;
                    }
                }

                // Definindo horários de início e término do intervalo de almoço
                DateTime dtInicioAlmoco = dtDataInicio + dtHoraFimManha;
                DateTime dtFimAlmoco = dtDataInicio + dtHoraInicioTarde;

                // Se o horário estiver disponível, adiciona na lista de horários disponíveis
                if (horarioDisponivel)
                {
                    horariosDisponiveis.Add(dtHorario);
                }

                // Calculando o intervalo
                TimeSpan intervalo = new TimeSpan(0, 0, 0);
                if (horarioDisponivel)
                {
                    if (dtHorario <= dtDataInicio + dtHoraFimManha || dtHorario >= dtDataInicio + dtHoraInicioTarde)
                    {
                        intervalo = intervaloMarcacoes;
                    }
                    else
                    {
                        intervalo = new TimeSpan(0, 0, 0);
                    }
                }
                else
                {
                    intervalo = new TimeSpan(0, 0, 0);
                }

                // Adicionando o horário na DataTable com o status correspondente
                dataTable.Rows.Add(dtHorario.Date, dtHorario.TimeOfDay, duracao, status, intervalo);
            }

            // Adicionando horários disponíveis no período da tarde
            dtHorario = dtDataInicio + dtHoraInicioTarde;
            while (dtHorario + new TimeSpan(1, 0, 0) <= dtDataFim + dtHoraFimTarde)
            {
                bool horarioDisponivel = true;
                string status = "Disponível";
                TimeSpan duracao = new TimeSpan(1, 0, 0);


                // Verificando se o horário está ocupado
       
                foreach (var evento in events.Items)
                {
                    DateTime dtInicioEvento = evento.Start.DateTime ?? DateTime.Parse(evento.Start.Date);
                    DateTime dtFimEvento = evento.End.DateTime ?? DateTime.Parse(evento.End.Date);

                    if (dtHorario >= dtInicioEvento && dtHorario < dtFimEvento)
                    {
                        horarioDisponivel = false;
                        status = "Ocupado";
                        break;
                    }
                }


                // Definindo horários de início e término do intervalo de almoço
                DateTime dtInicioAlmoco = dtDataInicio + dtHoraFimManha;
                DateTime dtFimAlmoco = dtDataInicio + dtHoraInicioTarde;
                if (dtHorario >= dtInicioAlmoco && dtHorario < dtFimAlmoco)
                {
                    horarioDisponivel = false;
                    status = "Intervalo de almoço";
                }

                // Se o horário estiver disponível, adiciona na lista de horários disponíveis
                if (horarioDisponivel)
                {
                    horariosDisponiveis.Add(dtHorario);
                }

                // Calculando o intervalo
                TimeSpan intervalo = new TimeSpan(0, 0, 0);
                if (horarioDisponivel)
                {
                    if (dtHorario + intervaloMarcacoes <= dtDataInicio + dtHoraFimManha || dtHorario + intervaloMarcacoes >= dtDataInicio + dtHoraInicioTarde)
                    {
                        intervalo = intervaloMarcacoes;
                    }
                    else
                    {
                        intervalo = new TimeSpan(0, 0, 0);
                    }
                }
                else
                {
                    intervalo = new TimeSpan(0, 0, 0);
                }

                // Adicionando o horário na DataTable com o status correspondente
                dataTable.Rows.Add(dtHorario.Date, dtHorario.TimeOfDay, duracao, status, intervalo);
            }

            // Exibindo os horários disponíveis no DataGridView
            dataGridView1.DataSource = dataTable;

            // Adicionando uma nova coluna com os botões de agendamento
            DataGridViewButtonColumn agendarColumn = new DataGridViewButtonColumn();
            agendarColumn.HeaderText = "";
            agendarColumn.Text = "Agendar";
            agendarColumn.UseColumnTextForButtonValue = true;
            dataGridView1.Columns.Add(agendarColumn);

            // Adicionando um handler para o evento CellContentClick
           // dataGridView1.CellContentClick += dataGridView1_CellContentClick;
        }




        private void button4_Click(object sender, EventArgs e)
        {

            // Criando uma nova instância do Google Credential
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
            request.TimeMin = DtDataInicio.Value.Date;
            request.TimeMax = DtDataFim.Value.Date.AddDays(1);
            request.ShowDeleted = false;
            request.SingleEvents = true;
            request.OrderBy = EventsResource.ListRequest.OrderByEnum.StartTime;

            // Executando a pesquisa
            Events events = request.Execute();

            // Processando os eventos encontrados
            List<Agendamento> agendamentos = new List<Agendamento>();
            if (events.Items != null && events.Items.Count > 0)
            {
                foreach (var eventItem in events.Items)
                {
                    agendamentos.Add(new Agendamento()
                    {
                        EventId = eventItem.Id, // Adicione esta linha
                        Inicio = eventItem.Start.DateTime.Value,
                        Fim = eventItem.End.DateTime.Value
                    });
                }
            }

            DateTime startDate = DtDataInicio.Value.Date;
            DateTime endDate = DtDataFim.Value.Date;
            DateTime morningStartTime = DtHoraInicioManha.Value;
            DateTime morningEndTime = DtHoraFimManha.Value;
            DateTime afternoonStartTime = DtHoraInicioTarde.Value;
            DateTime afternoonEndTime = DtHoraFimTarde.Value;

            List<DateTime> horariosDisponiveis = GetAvailableTimeSlots(startDate, endDate, morningStartTime, morningEndTime, afternoonStartTime, afternoonEndTime);

            foreach (Agendamento agendamento in agendamentos)
            {
                for (int i = horariosDisponiveis.Count - 1; i >= 0; i--)
                {
                    DateTime horarioDisponivel = horariosDisponiveis[i];
                    if (horarioDisponivel >= agendamento.Inicio && horarioDisponivel < agendamento.Fim)
                    {
                        horariosDisponiveis.RemoveAt(i);
                    }
                }
            }

            DataTable dt = new DataTable();
            dt.Columns.Add("EventId"); // Adicionando a coluna "EventId"
            dt.Columns.Add("Horário");
            dt.Columns.Add("Status");
            dt.Columns.Add("Título"); // Adicionando a coluna "Título"
            dt.Columns.Add("Descrição"); // Adicionando a coluna "Descrição"

            foreach (DateTime horario in horariosDisponiveis)
            {
                dt.Rows.Add("", horario.ToString("HH:mm"), "Disponível", "", ""); // Adicione um valor vazio para a coluna "EventId" quando o horário estiver disponível
            }

            foreach (Agendamento agendamento in agendamentos)
            {
                // Obtendo o evento correspondente ao agendamento
                var eventItem = events.Items.FirstOrDefault(ev => ev.Start.DateTime.Value == agendamento.Inicio && ev.End.DateTime.Value == agendamento.Fim);

                if (eventItem != null)
                {
                    string titulo = eventItem.Summary;
                    string descricao = eventItem.Description;

                    dt.Rows.Add(agendamento.EventId, agendamento.Inicio.ToString("HH:mm") + " - " + agendamento.Fim.ToString("HH:mm"), "Ocupado", titulo, descricao);
                }
                else
                {
                    dt.Rows.Add(agendamento.EventId, agendamento.Inicio.ToString("HH:mm") + " - " + agendamento.Fim.ToString("HH:mm"), "Ocupado", "", "");
                }
            }

            dataGridView1.DataSource = dt;
        
    }
        //private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        //{
        //    // Verificando se o botão "Agendar" foi clicado
        //    if (e.ColumnIndex == dataGridView1.Columns.Count - 1 && e.RowIndex >= 0)
        //    {
        //        // Obtendo a data e hora selecionada pelo usuário
        //        DateTime data = (DateTime)dataGridView1.Rows[e.RowIndex].Cells[0].Value;
        //        TimeSpan hora = (TimeSpan)dataGridView1.Rows[e.RowIndex].Cells[1].Value;

        //        // Criando um novo formulário para adicionar um evento na agenda
        //        FormAdicionarEvento form = new FormAdicionarEvento(data + hora, data + hora + new TimeSpan(1, 0, 0));
        //        form.ShowDialog();
        //    }

        //}

        public class Agendamento
        {
            public string EventId { get; set; } // Adicione esta linha
            public DateTime Inicio { get; set; }
            public DateTime Fim { get; set; }
        }
      

        private List<Agendamento> ObterAgendamentos()
        {
            List<Agendamento> agendamentos = new List<Agendamento>();

            // Criando uma nova instância do Google Credential
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
            request.TimeMin = DateTime.Today;
            request.TimeMax = DateTime.Today.AddDays(1);
            request.ShowDeleted = false;
            request.SingleEvents = true;
            request.OrderBy = EventsResource.ListRequest.OrderByEnum.StartTime;

            // Executando a pesquisa
            Events events = request.Execute();

            // Processando os eventos encontrados
            if (events.Items != null && events.Items.Count > 0)
            {
                foreach (var eventItem in events.Items)
                {
                    agendamentos.Add(new Agendamento()
                    {
                        Inicio = eventItem.Start.DateTime.Value,
                        Fim = eventItem.End.DateTime.Value
                    });
                }
            }

            return agendamentos;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            const int MANHA_INICIO = 9;
            const int MANHA_FIM = 12;
            const int TARDE_INICIO = 14;
            const int TARDE_FIM = 19;

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
            request.TimeMin = DtDataInicio.Value.Date;
            request.TimeMax = DtDataFim.Value.Date.AddDays(1);
            request.ShowDeleted = false;
            request.SingleEvents = true;
            request.OrderBy = EventsResource.ListRequest.OrderByEnum.StartTime;

            // Executando a pesquisa
            Events events = request.Execute();

            // Processando os eventos encontrados
            List<Agendamento> agendamentos = new List<Agendamento>();
            if (events.Items != null && events.Items.Count > 0)
            {
                foreach (var eventItem in events.Items)
                {
                    agendamentos.Add(new Agendamento()
                    {
                        Inicio = eventItem.Start.DateTime.Value,
                        Fim = eventItem.End.DateTime.Value
                    });
                }
            }

            List<DateTime> horariosDisponiveis = new List<DateTime>();

            for (int hora = MANHA_INICIO; hora <= MANHA_FIM; hora++)
            {
                DateTime horario = new DateTime(DtDataInicio.Value.Year, DtDataInicio.Value.Month, DtDataInicio.Value.Day, hora, 0, 0);
                horariosDisponiveis.Add(horario);
            }

            for (int hora = TARDE_INICIO; hora <= TARDE_FIM; hora++)
            {
                DateTime horario = new DateTime(DtDataInicio.Value.Year, DtDataInicio.Value.Month, DtDataInicio.Value.Day, hora, 0, 0);
                horariosDisponiveis.Add(horario);
            }

            foreach (Agendamento agendamento in agendamentos)
            {
                for (int i = horariosDisponiveis.Count - 1; i >= 0; i--)
                {
                    DateTime horarioDisponivel = horariosDisponiveis[i];
                    if (horarioDisponivel >= agendamento.Inicio && horarioDisponivel < agendamento.Fim)
                    {
                        horariosDisponiveis.RemoveAt(i);
                    }
                }
            }

            DataTable dt = new DataTable();
            dt.Columns.Add("Horário");
            dt.Columns.Add("Status");

            foreach (Agendamento agendamento in agendamentos)
            {
                dt.Rows.Add(agendamento.Inicio.ToString("HH:mm") + " - " + agendamento.Fim.ToString("HH:mm"), "Ocupado");
            }

            dataGridView1.DataSource = dt;

        }




        private List<DateTime> GetAvailableTimeSlots(DateTime startDate, DateTime endDate, DateTime morningStartTime, DateTime morningEndTime, DateTime afternoonStartTime, DateTime afternoonEndTime)
        {
            List<DateTime> timeSlots = new List<DateTime>();

            for (DateTime currentDate = startDate; currentDate <= endDate; currentDate = currentDate.AddDays(1))
            {
                DateTime currentMorningStartTime = currentDate.Date.Add(morningStartTime.TimeOfDay);
                DateTime currentMorningEndTime = currentDate.Date.Add(morningEndTime.TimeOfDay);
                DateTime currentAfternoonStartTime = currentDate.Date.Add(afternoonStartTime.TimeOfDay);
                DateTime currentAfternoonEndTime = currentDate.Date.Add(afternoonEndTime.TimeOfDay);

                for (DateTime time = currentMorningStartTime; time < currentMorningEndTime; time = time.AddMinutes(30))
                {
                    timeSlots.Add(time);
                }

                for (DateTime time = currentAfternoonStartTime; time < currentAfternoonEndTime; time = time.AddMinutes(30))
                {
                    timeSlots.Add(time);
                }
            }

            return timeSlots;
        }




        private void button6_Click(object sender, EventArgs e)
        {

            // Criando uma nova instância do Google Credential
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
            request.TimeMin = DtDataInicio.Value.Date;
            request.TimeMax = DtDataFim.Value.Date.AddDays(1);
            request.ShowDeleted = false;
            request.SingleEvents = true;
            request.OrderBy = EventsResource.ListRequest.OrderByEnum.StartTime;

            // Executando a pesquisa
            Events events = request.Execute();

            // Processando os eventos encontrados
            List<Agendamento> agendamentos = new List<Agendamento>();
            if (events.Items != null && events.Items.Count > 0)
            {
                foreach (var eventItem in events.Items)
                {
                    agendamentos.Add(new Agendamento()
                    {
                        Inicio = eventItem.Start.DateTime.Value,
                        Fim = eventItem.End.DateTime.Value
                    });
                }
            }

            DateTime startDate = DtDataInicio.Value.Date;
            DateTime endDate = DtDataFim.Value.Date;
            DateTime morningStartTime = DtHoraInicioManha.Value;
            DateTime morningEndTime = DtHoraFimManha.Value;
            DateTime afternoonStartTime = DtHoraInicioTarde.Value;
            DateTime afternoonEndTime = DtHoraFimTarde.Value;

            List<DateTime> horariosDisponiveis = GetAvailableTimeSlots(startDate, endDate, morningStartTime, morningEndTime, afternoonStartTime, afternoonEndTime);

            foreach (Agendamento agendamento in agendamentos)
            {
                for (int i = horariosDisponiveis.Count - 1; i >= 0; i--)
                {
                    DateTime horarioDisponivel = horariosDisponiveis[i];
                    if (horarioDisponivel >= agendamento.Inicio && horarioDisponivel < agendamento.Fim)
                    {
                        horariosDisponiveis.RemoveAt(i);
                    }
                }
            }

            DataTable dt = new DataTable();
            dt.Columns.Add("Horário");
            dt.Columns.Add("Status");
            dt.Columns.Add("Título"); // Adicionando a coluna "Título"
            dt.Columns.Add("Descrição"); // Adicionando a coluna "Descrição"

            foreach (DateTime horario in horariosDisponiveis)
            {
                dt.Rows.Add(horario.ToString("HH:mm"), "Disponível", "", ""); // Incluindo valores vazios para as colunas "Título" e "Descrição" quando o horário estiver disponível
            }

            foreach (Agendamento agendamento in agendamentos)
            {
                // Obtendo o evento correspondente ao agendamento
                var eventItem = events.Items.FirstOrDefault(ev => ev.Start.DateTime.Value == agendamento.Inicio && ev.End.DateTime.Value == agendamento.Fim);

                if (eventItem != null)
                {
                    string titulo = eventItem.Summary;
                    string descricao = eventItem.Description;

                    dt.Rows.Add(agendamento.Inicio.ToString("HH:mm") + " - " + agendamento.Fim.ToString("HH:mm"), "Ocupado", titulo, descricao);
                }
                else
                {
                    dt.Rows.Add(agendamento.Inicio.ToString("HH:mm") + " - " + agendamento.Fim.ToString("HH:mm"), "Ocupado", "", "");
                }
            }

            dataGridView1.DataSource = dt;
        }

        private void button7_Click(object sender, EventArgs e)
        {
            // Criando uma nova instância do Google Credential
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
            request.TimeMin = DtDataInicio.Value.Date;
            request.TimeMax = DtDataFim.Value.Date.AddDays(1);
            request.ShowDeleted = false;
            request.SingleEvents = true;
            request.OrderBy = EventsResource.ListRequest.OrderByEnum.StartTime;

            // Executando a pesquisa
            Events events = request.Execute();

            // Processando os eventos encontrados
            List<Agendamento> agendamentos = new List<Agendamento>();
            if (events.Items != null && events.Items.Count > 0)
            {
                foreach (var eventItem in events.Items)
                {
                    agendamentos.Add(new Agendamento()
                    {
                        EventId = eventItem.Id, // Adicione esta linha
                        Inicio = eventItem.Start.DateTime.Value,
                        Fim = eventItem.End.DateTime.Value
                    });
                }
            }

            DateTime startDate = DtDataInicio.Value.Date;
            DateTime endDate = DtDataFim.Value.Date;
            DateTime morningStartTime = DtHoraInicioManha.Value;
            DateTime morningEndTime = DtHoraFimManha.Value;
            DateTime afternoonStartTime = DtHoraInicioTarde.Value;
            DateTime afternoonEndTime = DtHoraFimTarde.Value;

            List<DateTime> horariosDisponiveis = GetAvailableTimeSlots(startDate, endDate, morningStartTime, morningEndTime, afternoonStartTime, afternoonEndTime);

            foreach (Agendamento agendamento in agendamentos)
            {
                for (int i = horariosDisponiveis.Count - 1; i >= 0; i--)
                {
                    DateTime horarioDisponivel = horariosDisponiveis[i];
                    if (horarioDisponivel >= agendamento.Inicio && horarioDisponivel < agendamento.Fim)
                    {
                        horariosDisponiveis.RemoveAt(i);
                    }
                }
            }

            DataTable dt = new DataTable();
            dt.Columns.Add("EventId"); // Adicionando a coluna "EventId"
            dt.Columns.Add("Horário");
        //    dt.Columns.Add("Status");
            dt.Columns.Add("Título"); // Adicionando a coluna "Título"
            dt.Columns.Add("Descrição"); // Adicionando a coluna "Descrição"

        

            foreach (Agendamento agendamento in agendamentos)
            {
                // Obtendo o evento correspondente ao agendamento
                var eventItem = events.Items.FirstOrDefault(ev => ev.Start.DateTime.Value == agendamento.Inicio && ev.End.DateTime.Value == agendamento.Fim);

                if (eventItem != null)
                {
                    string titulo = eventItem.Summary;
                    string descricao = eventItem.Description;

                    // Remova "Ocupado" da linha abaixo
                    dt.Rows.Add(agendamento.EventId, agendamento.Inicio.ToString("HH:mm") + " - " + agendamento.Fim.ToString("HH:mm"), titulo, descricao);
                }
                else
                {
                    // Remova "Ocupado" da linha abaixo
                    dt.Rows.Add(agendamento.EventId, agendamento.Inicio.ToString("HH:mm") + " - " + agendamento.Fim.ToString("HH:mm"), "", "");
                }
            }

            dataGridView2.DataSource = dt;
        }

        private void button10_Click(object sender, EventArgs e)
        {
            // Criando uma nova instância do Google Credential
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
            request.TimeMin = DtDataInicio.Value.Date;
            request.TimeMax = DtDataFim.Value.Date.AddDays(1);
            request.ShowDeleted = false;
            request.SingleEvents = true;
            request.OrderBy = EventsResource.ListRequest.OrderByEnum.StartTime;

            // Executando a pesquisa
            Events events = request.Execute();

            // Processando os eventos encontrados
            List<Agendamento> agendamentos = new List<Agendamento>();
            if (events.Items != null && events.Items.Count > 0)
            {
                foreach (var eventItem in events.Items)
                {
                    agendamentos.Add(new Agendamento()
                    {
                        EventId = eventItem.Id, // Adicione esta linha
                        Inicio = eventItem.Start.DateTime.Value,
                        Fim = eventItem.End.DateTime.Value
                    });
                }
            }

            DateTime startDate = DtDataInicio.Value.Date;
            DateTime endDate = DtDataFim.Value.Date;
            DateTime morningStartTime = DtHoraInicioManha.Value;
            DateTime morningEndTime = DtHoraFimManha.Value;
            DateTime afternoonStartTime = DtHoraInicioTarde.Value;
            DateTime afternoonEndTime = DtHoraFimTarde.Value;

            List<DateTime> horariosDisponiveis = GetAvailableTimeSlots(startDate, endDate, morningStartTime, morningEndTime, afternoonStartTime, afternoonEndTime);

            foreach (Agendamento agendamento in agendamentos)
            {
                for (int i = horariosDisponiveis.Count - 1; i >= 0; i--)
                {
                    DateTime horarioDisponivel = horariosDisponiveis[i];
                    if (horarioDisponivel >= agendamento.Inicio && horarioDisponivel < agendamento.Fim)
                    {
                        horariosDisponiveis.RemoveAt(i);
                    }
                }
            }

            DataTable dt = new DataTable();
            // Adicione colunas adicionais
            dt.Columns.Add("Data");
            dt.Columns.Add("Hora Início");
            dt.Columns.Add("Hora Fim");
            dt.Columns.Add("EventId");
            dt.Columns.Add("Título");
            dt.Columns.Add("Descrição");
            foreach (Agendamento agendamento in agendamentos)
            {
                // Obtendo o evento correspondente ao agendamento
                var eventItem = events.Items.FirstOrDefault(ev => ev.Start.DateTime.Value == agendamento.Inicio && ev.End.DateTime.Value == agendamento.Fim);

                if (eventItem != null)
                {
                    string titulo = eventItem.Summary;
                    string descricao = eventItem.Description;

                    dt.Rows.Add(agendamento.Inicio.ToString("dd/MM/yyyy"), agendamento.Inicio.ToString("HH:mm"), agendamento.Fim.ToString("HH:mm"), agendamento.EventId, titulo, descricao);
                }
                else
                {
                    dt.Rows.Add(agendamento.Inicio.ToString("dd/MM/yyyy"), agendamento.Inicio.ToString("HH:mm"), agendamento.Fim.ToString("HH:mm"), agendamento.EventId, "", "");
                }
            }

            dataGridView2.DataSource = dt;

            // Adicionando uma nova coluna com os botões de agendamento
            DataGridViewButtonColumn agendarColumn = new DataGridViewButtonColumn();
            agendarColumn.HeaderText = "Agendar";
            agendarColumn.Text = "Agendar";
            agendarColumn.UseColumnTextForButtonValue = true;
            dataGridView2.Columns.Add(agendarColumn);

            // Adicionando um handler para o evento CellContentClick
            dataGridView2.CellContentClick += dataGridView2_CellContentClick;
        }
        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
           

            var senderGrid = (DataGridView)sender;

            if (senderGrid.Columns[e.ColumnIndex] is DataGridViewButtonColumn &&
                e.RowIndex >= 0)
            {
                // Recuperando os valores das colunas
                string data = senderGrid.Rows[e.RowIndex].Cells["Data"].Value.ToString();
                string horaInicio = senderGrid.Rows[e.RowIndex].Cells["Hora Início"].Value.ToString();
                string horaFim = senderGrid.Rows[e.RowIndex].Cells["Hora Fim"].Value.ToString();
                string Idgoogle = senderGrid.Rows[e.RowIndex].Cells["EventId"].Value.ToString();
                string descricao = senderGrid.Rows[e.RowIndex].Cells["Descrição"].Value.ToString();
                string titulo = senderGrid.Rows[e.RowIndex].Cells["Título"].Value.ToString();
                string CodCliente = senderGrid.Rows[e.RowIndex].Cells["Código Cliente"].Value.ToString();
            

                // Abre o FormAdicionarEvento passando os valores como parâmetros
                FormAdicionarEvento form = new FormAdicionarEvento(data, horaInicio, horaFim, Idgoogle, descricao, titulo, CodCliente);
                form.Show();
            }

        }

        private void button9_Click(object sender, EventArgs e)
        {
            // Criando uma nova instância do Google Credential
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
            request.TimeMin = DtDataInicio.Value.Date;
            request.TimeMax = DtDataFim.Value.Date.AddDays(1);
            request.ShowDeleted = false;
            request.SingleEvents = true;
            request.OrderBy = EventsResource.ListRequest.OrderByEnum.StartTime;

            // Executando a pesquisa
            Events events = request.Execute();

            // Processando os eventos encontrados
            List<Agendamento> agendamentos = new List<Agendamento>();
            if (events.Items != null && events.Items.Count > 0)
            {
                foreach (var eventItem in events.Items)
                {
                    agendamentos.Add(new Agendamento()
                    {
                        EventId = eventItem.Id, // Adicione esta linha
                        Inicio = eventItem.Start.DateTime.Value,
                        Fim = eventItem.End.DateTime.Value
                    });
                }
            }

            DateTime startDate = DtDataInicio.Value.Date;
            DateTime endDate = DtDataFim.Value.Date;
            DateTime morningStartTime = DtHoraInicioManha.Value;
            DateTime morningEndTime = DtHoraFimManha.Value;
            DateTime afternoonStartTime = DtHoraInicioTarde.Value;
            DateTime afternoonEndTime = DtHoraFimTarde.Value;

            List<DateTime> horariosDisponiveis = GetAvailableTimeSlots(startDate, endDate, morningStartTime, morningEndTime, afternoonStartTime, afternoonEndTime);

            foreach (Agendamento agendamento in agendamentos)
            {
                for (int i = horariosDisponiveis.Count - 1; i >= 0; i--)
                {
                    DateTime horarioDisponivel = horariosDisponiveis[i];
                    if (horarioDisponivel >= agendamento.Inicio && horarioDisponivel < agendamento.Fim)
                    {
                        horariosDisponiveis.RemoveAt(i);
                    }
                }
            }

            DataTable dt = new DataTable();
            // Adicione colunas adicionais
           
            dt.Columns.Add("Data");
            dt.Columns.Add("Hora Início");
            dt.Columns.Add("Hora Fim");
            dt.Columns.Add("EventId");
            dt.Columns.Add("Título");
            dt.Columns.Add("Descrição");
            dt.Columns.Add("Código Cliente");

            foreach (Agendamento agendamento in agendamentos)
            {
                // Obtendo o evento correspondente ao agendamento
                var eventItem = events.Items.FirstOrDefault(ev => ev.Start.DateTime.Value == agendamento.Inicio && ev.End.DateTime.Value == agendamento.Fim);

                int? codigoCliente = GetClientIdByGoogleEventId(agendamento.EventId);

                if (eventItem != null)
                {
                    string titulo = eventItem.Summary;
                    string descricao = eventItem.Description;

                    dt.Rows.Add(agendamento.Inicio.ToString("dd/MM/yyyy"), agendamento.Inicio.ToString("HH:mm"), agendamento.Fim.ToString("HH:mm"), agendamento.EventId, titulo, descricao, codigoCliente);
                }
                else
                {
                    dt.Rows.Add(agendamento.Inicio.ToString("dd/MM/yyyy"), agendamento.Inicio.ToString("HH:mm"), agendamento.Fim.ToString("HH:mm"), agendamento.EventId, "", "", codigoCliente);
                }
            }

            dataGridView2.DataSource = dt;


            dataGridView2.CellFormatting += dataGridView2_CellFormatting;
            dataGridView2.CellPainting += dataGridView2_CellPainting;
           
            DataGridViewButtonColumn agendarColumn = new DataGridViewButtonColumn();
            agendarColumn.HeaderText = "Agendar";
           
            dataGridView2.Columns.Add(agendarColumn);

            dataGridView2.CellContentClick += dataGridView2_CellContentClick;
        }
        private void dataGridView2_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.ColumnIndex == 7) // Verifique se a coluna é a coluna do botão
            {
                DataGridViewRow row = dataGridView2.Rows[e.RowIndex];
                object codigoCliente = row.Cells["Código Cliente"].Value;

                // Verifique se o código do cliente está presente
                if (codigoCliente != null && codigoCliente != DBNull.Value)
                {
                    // Mude a cor do botão para amarelo
                    row.Cells[e.ColumnIndex].Style.BackColor = Color.Yellow;
                }
                else
                {
                    // Mude a cor do botão para verde
                    row.Cells[e.ColumnIndex].Style.BackColor = Color.Green;
                }
            }
        }

        private void dataGridView2_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.ColumnIndex == 7 && e.RowIndex >= 0)
            {
                DataGridViewRow row = dataGridView2.Rows[e.RowIndex];
                object codigoCliente = row.Cells["Código Cliente"].Value;

                // Verifique se o código do cliente está presente
                if (codigoCliente != null && codigoCliente != DBNull.Value)
                {
                    e.PaintBackground(e.CellBounds, true);

                    using (Brush b = new SolidBrush(Color.Yellow))
                    {
                        e.Graphics.FillRectangle(b, e.CellBounds);
                    }

                    // Mude o texto do botão para "Editar"
                    e.PaintContent(e.CellBounds);
                    e.Graphics.DrawString("Editar", e.CellStyle.Font, Brushes.Black, e.CellBounds, StringFormat.GenericDefault);

                    e.Handled = true;
                }
                else
                {
                    e.PaintBackground(e.CellBounds, true);

                    using (Brush b = new SolidBrush(Color.Green))
                    {
                        e.Graphics.FillRectangle(b, e.CellBounds);
                    }

                    // Mude o texto do botão para "Agendar"
                    e.PaintContent(e.CellBounds);
                    e.Graphics.DrawString("Agendar", e.CellStyle.Font, Brushes.Black, e.CellBounds, StringFormat.GenericDefault);

                    e.Handled = true;
                }
            }
        }


        private int? GetClientIdByGoogleEventId(string googleEventId)
        {
            int? clientId = null;

            // Defina sua string de conexão aqui
            //string connectionString = "Server=your_server;Database=your_database;Uid=your_username;Pwd=your_password;";
            string connectionString = (@"server=" + Properties.Settings.Default.server + ";database=" + Properties.Settings.Default.basedados + ";port=" + Properties.Settings.Default.porta + ";userid=" + Properties.Settings.Default.username + ";password=" + Properties.Settings.Default.password);

            // Crie a consulta SQL para buscar o IDcliente com base no IdGoogle
            string query = "SELECT IDcliente FROM marcacoes WHERE IdGoogle = @googleEventId";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    // Adicione o parâmetro à consulta
                    command.Parameters.AddWithValue("@googleEventId", googleEventId);

                    // Abra a conexão e execute a consulta
                    connection.Open();
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        // Se a consulta retornar algum resultado, atribua o IDcliente à variável
                        if (reader.Read())
                        {
                            clientId = reader.GetInt32(0);
                        }
                    }
                }
            }

            return clientId;
        }

        private void button8_Click(object sender, EventArgs e)
        {
            // Aplicando o filtro para mostrar apenas os resultados com a coluna 7 vazia
    (dataGridView2.DataSource as DataTable).DefaultView.RowFilter = "[Código Cliente] IS NULL";
        }

        private void button11_Click(object sender, EventArgs e)
        {
            // Aplicando o filtro para mostrar apenas os resultados com a coluna 7 não vazia
            (dataGridView2.DataSource as DataTable).DefaultView.RowFilter = "[Código Cliente] IS NOT NULL";
        }

        private void button12_Click(object sender, EventArgs e)
        {
            // Removendo o filtro para mostrar todos os resultados
            (dataGridView2.DataSource as DataTable).DefaultView.RowFilter = string.Empty;
        }
    }
    }

    

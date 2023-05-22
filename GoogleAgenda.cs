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
using System.Net.Http;
using WindowsFormsCalendar;
using static Impar.GoogleAgenda;
using System.Globalization;

namespace Impar
{
    public partial class GoogleAgenda : Form
    {
        public GoogleAgenda()
        {
            InitializeComponent();
            dataGridView1.CellFormatting += DataGridView1_CellFormatting;


            //dataGridView2.CellFormatting += dataGridView2_CellFormatting;
            //dataGridView2.CellPainting += dataGridView2_CellPainting;

            //DataGridViewButtonColumn agendarColumn = new DataGridViewButtonColumn();
            //agendarColumn.HeaderText = "Agendar";

            //dataGridView2.Columns.Add(agendarColumn);
            //agendarColumn.DisplayIndex = 7; // Defina a posição da coluna para 7

            //dataGridView2.CellContentClick += dataGridView2_CellContentClick;




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



        //private List<DateTime> GetOccupiedTimeSlotsInRange(List<Agendamento> agendamentos, DateTime inicio, DateTime fim)
        //{
        //    List<DateTime> horariosOcupados = new List<DateTime>();

        //    foreach (Agendamento agendamento in agendamentos)
        //    {
        //        if (agendamento.Inicio.Date >= inicio.Date && agendamento.Fim.Date <= fim.Date)
        //        {
        //            DateTime horario = agendamento.Inicio;
        //            while (horario < agendamento.Fim)
        //            {
        //                horariosOcupados.Add(horario);
        //                horario = horario.AddMinutes(30);
        //            }
        //        }
        //    }

        //    return horariosOcupados;
        //}




        // !!!!!!!!!!!!!!!!!  EVENTO NO CALENDR BIER

        //private List<Agendamento> agendamentos2 = new List<Agendamento>();


        //private List<DateTime> GetAvailableTimeSlots2(DateTime inicio, DateTime fim, DateTime manhaInicio, DateTime manhaFim, DateTime tardeInicio, DateTime tardeFim, List<Agendamento> agendamentos)
        //{
        //    List<DateTime> horariosDisponiveis = new List<DateTime>();

        //    foreach (DateTime horario in GetTimeSlots2(inicio, fim, manhaInicio, manhaFim, tardeInicio, tardeFim))
        //    {
        //        if (!IsHorarioOcupado2(horario, manhaInicio, manhaFim, tardeInicio, tardeFim, agendamentos))
        //        {
        //            horariosDisponiveis.Add(horario);
        //        }
        //    }

        //    return horariosDisponiveis;
        //}
        //private List<DateTime> GetOccupiedTimeSlots2(DateTime inicio, DateTime fim, DateTime manhaInicio, DateTime manhaFim, DateTime tardeInicio, DateTime tardeFim, List<Agendamento> agendamentos)
        //{
        //    List<DateTime> horariosOcupados = new List<DateTime>();

        //    foreach (DateTime horario in GetTimeSlots2(inicio, fim, manhaInicio, manhaFim, tardeInicio, tardeFim))
        //    {
        //        if (IsHorarioOcupado2(horario, manhaInicio, manhaFim, tardeInicio, tardeFim, agendamentos))
        //        {
        //            horariosOcupados.Add(horario);
        //        }
        //    }

        //    return horariosOcupados;
        //}

        //private bool IsHorarioOcupado2(DateTime horario, DateTime manhaInicio, DateTime manhaFim, DateTime tardeInicio, DateTime tardeFim, List<Agendamento> agendamentos)
        //{
        //    foreach (var agendamento in agendamentos)
        //    {
        //        if (horario >= agendamento.Inicio && horario < agendamento.Fim)
        //            return true;
        //    }

        //    return false;
        //}

        //private IEnumerable<DateTime> GetTimeSlots2(DateTime inicio, DateTime fim, DateTime manhaInicio, DateTime manhaFim, DateTime tardeInicio, DateTime tardeFim)
        //{
        //    DateTime horario = inicio;

        //    while (horario < fim)
        //    {
        //        if ((horario.TimeOfDay >= manhaInicio.TimeOfDay && horario.TimeOfDay < manhaFim.TimeOfDay) ||
        //            (horario.TimeOfDay >= tardeInicio.TimeOfDay && horario.TimeOfDay < tardeFim.TimeOfDay))
        //        {
        //            yield return horario;
        //        }

        //        horario = horario.AddMinutes(30);
        //    }
        //}




        //    private List<DateTime> GetAvailableTimeSlots2(DateTime startDate, DateTime endDate, DateTime morningStartTime, DateTime morningEndTime, DateTime afternoonStartTime, DateTime afternoonEndTime)
        //    {
        //        List<DateTime> timeSlots = new List<DateTime>();

        //        DateTime currentStartDate = startDate.Date;
        //        DateTime currentEndDate = endDate.Date;

        //        while (currentStartDate <= currentEndDate)
        //        {
        //            DateTime currentMorningStartTime = currentStartDate.Add(morningStartTime.TimeOfDay);
        //            DateTime currentMorningEndTime = currentStartDate.Add(morningEndTime.TimeOfDay);
        //            DateTime currentAfternoonStartTime = currentStartDate.Add(afternoonStartTime.TimeOfDay);
        //            DateTime currentAfternoonEndTime = currentStartDate.Add(afternoonEndTime.TimeOfDay);

        //            for (DateTime time = currentMorningStartTime; time < currentMorningEndTime; time = time.AddMinutes(30))
        //            {
        //                timeSlots.Add(time);
        //            }

        //            for (DateTime time = currentAfternoonStartTime; time < currentAfternoonEndTime; time = time.AddMinutes(30))
        //            {
        //                timeSlots.Add(time);
        //            }

        //            currentStartDate = currentStartDate.AddDays(1);
        //        }

        //        return timeSlots;

        //}



        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            var senderGrid = (DataGridView)sender;

            if (senderGrid.Columns[e.ColumnIndex] is DataGridViewButtonColumn &&
                e.RowIndex >= 0)
            {
                string buttonText = dataGridView2.Rows[e.RowIndex].Cells["Ação"].Value?.ToString();

                if (buttonText == "Marcar")
                {
                    string data = dataGridView2.Rows[e.RowIndex].Cells["Data"].Value?.ToString();
                    string horario = dataGridView2.Rows[e.RowIndex].Cells["Horário"].Value?.ToString();
                    DateTime horarioInicio = DateTime.ParseExact(horario, "HH:mm", CultureInfo.InvariantCulture);
                    DateTime horarioFim = horarioInicio.AddMinutes(30);
                    string ocupadoAte = horarioFim.ToString("HH:mm");

                    // Abrir o formulário Marcacoes e passar os valores
                    Marcacoes marcacoesForm = new Marcacoes();
                    marcacoesForm.tbhorario.Value = DateTime.Parse(data);
                    marcacoesForm.tbhorainicio.Value = horarioInicio;
                    marcacoesForm.tbhorafim.Value = horarioFim;
                    marcacoesForm.Show();
                }
                else if (buttonText == "Editar")
                {
                    string data = dataGridView2.Rows[e.RowIndex].Cells["Data"].Value?.ToString();
                    string horario = dataGridView2.Rows[e.RowIndex].Cells["Horário"].Value?.ToString();
                    string ocupadoAte = dataGridView2.Rows[e.RowIndex].Cells["Ocupado até"].Value?.ToString();
                    string titulo = dataGridView2.Rows[e.RowIndex].Cells["Título"].Value?.ToString();
                    string descricao = dataGridView2.Rows[e.RowIndex].Cells["Descrição"].Value?.ToString();
                    string eventId = dataGridView2.Rows[e.RowIndex].Cells["EventId"].Value?.ToString();

                    // Abrir o formulário Marcacoes e passar os valores
                    Marcacoes marcacoesForm = new Marcacoes();
                    marcacoesForm.tbhorario.Value = DateTime.Parse(data);
                    marcacoesForm.tbhorainicio.Value = DateTime.ParseExact(horario, "HH:mm", CultureInfo.InvariantCulture);
                    marcacoesForm.tbhorafim.Value = DateTime.ParseExact(ocupadoAte, "HH:mm", CultureInfo.InvariantCulture);
                    marcacoesForm.tbtitulogoogle.Text = titulo;
                    marcacoesForm.tbdescricao.Text = descricao;
                    marcacoesForm.tbidgoogle.Text = eventId;
                    marcacoesForm.Show();
                }
            }
        }

        private void dataGridView2_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.ColumnIndex == dataGridView2.Columns["Ação"].Index && e.RowIndex >= 0)
            {
                string buttonText = dataGridView2.Rows[e.RowIndex].Cells[e.ColumnIndex].Value?.ToString() ?? string.Empty;
                if (buttonText == "Editar")
                {
                    e.Value = "Editar";
                }
                else
                {
                    e.Value = "Marcar";
                }
            }
        }

        private DataTable dt; // Declare o DataTable fora do método para que possa ser reutilizado




        //private void kryptonMonthCalendar1_DateChanged(object sender, DateRangeEventArgs e)
        //{
        //    DateTime selectedStartDate = e.Start.Date;
        //    DateTime selectedEndDate = e.End.Date;

        //    selectedEndDate = selectedEndDate.AddDays(1).AddSeconds(-1);

        //    GoogleCredential credential;
        //    using (var stream = new FileStream("C:\\Desenvolvimentos\\EmDesenvolvimento\\IMPAR\\Impar\\bin\\Debug\\keys.json", FileMode.Open, FileAccess.Read))
        //    {
        //        credential = GoogleCredential.FromStream(stream)
        //            .CreateScoped(CalendarService.Scope.Calendar);
        //    }

        //    var service = new CalendarService(new BaseClientService.Initializer()
        //    {
        //        HttpClientInitializer = credential,
        //        ApplicationName = "GoogleAgenda"
        //    });

        //    EventsResource.ListRequest request = service.Events.List("marcosmagalhaes86@gmail.com");
        //    request.TimeMin = selectedStartDate;
        //    request.TimeMax = selectedEndDate;
        //    request.ShowDeleted = false;
        //    request.SingleEvents = true;
        //    request.OrderBy = EventsResource.ListRequest.OrderByEnum.StartTime;

        //    Events events = request.Execute();

        //    List<Agendamento> agendamentos = new List<Agendamento>();
        //    if (events.Items != null && events.Items.Count > 0)
        //    {
        //        foreach (var eventItem in events.Items)
        //        {
        //            agendamentos.Add(new Agendamento()
        //            {
        //                Inicio = eventItem.Start.DateTime.Value,
        //                Fim = eventItem.End.DateTime.Value
        //            });
        //        }
        //    }

        //    TimeSpan morningStartTime = DtHoraInicioManha.Value.TimeOfDay;
        //    TimeSpan morningEndTime = DtHoraFimManha.Value.TimeOfDay;
        //    TimeSpan afternoonStartTime = DtHoraInicioTarde.Value.TimeOfDay;
        //    TimeSpan afternoonEndTime = DtHoraFimTarde.Value.TimeOfDay;

        //    List<DateTime> horariosDisponiveis = new List<DateTime>();

        //    for (DateTime currentDate = selectedStartDate.Date; currentDate <= selectedEndDate.Date; currentDate = currentDate.AddDays(1))
        //    {
        //        DateTime currentMorningStartTime = currentDate.Date + morningStartTime;
        //        DateTime currentMorningEndTime = currentDate.Date + morningEndTime;
        //        DateTime currentAfternoonStartTime = currentDate.Date + afternoonStartTime;
        //        DateTime currentAfternoonEndTime = currentDate.Date + afternoonEndTime;

        //        for (DateTime time = currentMorningStartTime; time < currentMorningEndTime; time = time.AddMinutes(30))
        //        {
        //            horariosDisponiveis.Add(time);
        //        }

        //        for (DateTime time = currentAfternoonStartTime; time < currentAfternoonEndTime; time = time.AddMinutes(30))
        //        {
        //            horariosDisponiveis.Add(time);
        //        }
        //    }

        //    dataGridView2.Columns.Clear(); // Remove todas as colunas existentes

        //    // Adicione as colunas ao dataGridView2
        //    DataGridViewButtonColumn btnColumn = new DataGridViewButtonColumn();
        //    btnColumn.HeaderText = "Ação";
        //    btnColumn.Name = "Ação";
        //    dataGridView2.Columns.Add(btnColumn);
        //    dataGridView2.Columns.Add("Data", "Data");
        //    dataGridView2.Columns.Add("Horário", "Horário");
        //    dataGridView2.Columns.Add("Status", "Status");
        //    dataGridView2.Columns.Add("Ocupado até", "Ocupado até");
        //    dataGridView2.Columns.Add("Título", "Título");
        //    dataGridView2.Columns.Add("Descrição", "Descrição");
        //    dataGridView2.Columns.Add("EventId", "EventId");


        //    foreach (DateTime horario in horariosDisponiveis)
        //    {
        //        var agendamento = agendamentos.FirstOrDefault(a => horario.Date == a.Inicio.Date && horario.TimeOfDay >= a.Inicio.TimeOfDay && horario.TimeOfDay < a.Fim.TimeOfDay);

        //        if (agendamento != null)
        //        {
        //            var eventItem = events.Items.FirstOrDefault(ev => ev.Start.DateTime.Value == agendamento.Inicio && ev.End.DateTime.Value == agendamento.Fim);

        //            if (eventItem != null)
        //            {
        //                string eventId = eventItem.Id;
        //                string titulo = eventItem.Summary;
        //                string descricao = eventItem.Description;
        //                dataGridView2.Rows.Add("Editar", horario.ToString("dd/MM/yyyy"), horario.ToString("HH:mm"), "Ocupado", agendamento.Fim.ToString("HH:mm"), titulo, descricao, eventId);
        //            }
        //            else
        //            {
        //                dataGridView2.Rows.Add("Editar", horario.ToString("dd/MM/yyyy"), horario.ToString("HH:mm"), "Ocupado", agendamento.Fim.ToString("HH:mm"), "", "", "");
        //            }
        //        }
        //        else
        //        {
        //            dataGridView2.Rows.Add("Marcar", horario.ToString("dd/MM/yyyy"), horario.ToString("HH:mm"), "Disponível", "", "", "", "");
        //        }
        //    }

        //    dataGridView2.CellFormatting += dataGridView2_CellFormatting;
        //    dataGridView2.CellContentClick += dataGridView2_CellContentClick;
        //}





        private void kryptonMonthCalendar1_DateChanged(object sender, DateRangeEventArgs e)
        {
            DateTime selectedStartDate = e.Start.Date;
            DateTime selectedEndDate = e.End.Date;

            selectedEndDate = selectedEndDate.AddDays(1).AddSeconds(-1);

            GoogleCredential credential;
            using (var stream = new FileStream("C:\\Desenvolvimentos\\EmDesenvolvimento\\IMPAR\\Impar\\bin\\Debug\\keys.json", FileMode.Open, FileAccess.Read))
            {
                credential = GoogleCredential.FromStream(stream)
                    .CreateScoped(CalendarService.Scope.Calendar);
            }

            var service = new CalendarService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = "GoogleAgenda"
            });

            EventsResource.ListRequest request = service.Events.List("marcosmagalhaes86@gmail.com");
            request.TimeMin = selectedStartDate;
            request.TimeMax = selectedEndDate;
            request.ShowDeleted = false;
            request.SingleEvents = true;
            request.OrderBy = EventsResource.ListRequest.OrderByEnum.StartTime;

            Events events = request.Execute();

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

            TimeSpan morningStartTime = DtHoraInicioManha.Value.TimeOfDay;
            TimeSpan morningEndTime = DtHoraFimManha.Value.TimeOfDay;
            TimeSpan afternoonStartTime = DtHoraInicioTarde.Value.TimeOfDay;
            TimeSpan afternoonEndTime = DtHoraFimTarde.Value.TimeOfDay;

            List<DateTime> horariosDisponiveis = new List<DateTime>();

            for (DateTime currentDate = selectedStartDate.Date; currentDate <= selectedEndDate.Date; currentDate = currentDate.AddDays(1))
            {
                DateTime currentMorningStartTime = currentDate.Date + morningStartTime;
                DateTime currentMorningEndTime = currentDate.Date + morningEndTime;
                DateTime currentAfternoonStartTime = currentDate.Date + afternoonStartTime;
                DateTime currentAfternoonEndTime = currentDate.Date + afternoonEndTime;

                for (DateTime time = currentMorningStartTime; time < currentMorningEndTime; time = time.AddMinutes(30))
                {
                    horariosDisponiveis.Add(time);
                }

                for (DateTime time = currentAfternoonStartTime; time < currentAfternoonEndTime; time = time.AddMinutes(30))
                {
                    horariosDisponiveis.Add(time);
                }
            }

            dataGridView2.Columns.Clear(); // Remove todas as colunas existentes

            // Adicione as colunas ao dataGridView2
            DataGridViewButtonColumn btnColumn = new DataGridViewButtonColumn();
            btnColumn.HeaderText = "Ação";
            btnColumn.Name = "Ação";
            dataGridView2.Columns.Add(btnColumn);
            dataGridView2.Columns.Add("Data", "Data");
            dataGridView2.Columns.Add("Horário", "Horário");
            dataGridView2.Columns.Add("Status", "Status");
            dataGridView2.Columns.Add("Ocupado até", "Ocupado até");
            dataGridView2.Columns.Add("Título", "Título");
            dataGridView2.Columns.Add("Descrição", "Descrição");
            dataGridView2.Columns.Add("EventId", "EventId");

            foreach (DateTime horario in horariosDisponiveis)
            {
                var agendamento = agendamentos.FirstOrDefault(a => horario.Date == a.Inicio.Date && horario.TimeOfDay >= a.Inicio.TimeOfDay && horario.TimeOfDay < a.Fim.TimeOfDay);

                if (agendamento != null)
                {
                    var eventItem = events.Items.FirstOrDefault(ev => ev.Start.DateTime.Value == agendamento.Inicio && ev.End.DateTime.Value == agendamento.Fim);

                    if (eventItem != null)
                    {
                        string eventId = eventItem.Id;
                        string titulo = eventItem.Summary;
                        string descricao = eventItem.Description;
                        dataGridView2.Rows.Add("Editar", horario.ToString("dd/MM/yyyy"), horario.ToString("HH:mm"), "Ocupado", agendamento.Fim.ToString("HH:mm"), titulo, descricao, eventId);
                    }
                    else
                    {
                        dataGridView2.Rows.Add("Editar", horario.ToString("dd/MM/yyyy"), horario.ToString("HH:mm"), "Ocupado", agendamento.Fim.ToString("HH:mm"), "", "", "");
                    }
                }
                else
                {
                    dataGridView2.Rows.Add("Marcar", horario.ToString("dd/MM/yyyy"), horario.ToString("HH:mm"), "Disponível", "", "", "", "");
                }
            }

            // Ocultar colunas "Título", "Descrição" e "EventId"
            dataGridView2.Columns["Título"].Visible = false;
            dataGridView2.Columns["Descrição"].Visible = false;
            dataGridView2.Columns["EventId"].Visible = false;

            dataGridView2.CellFormatting += dataGridView2_CellFormatting;
            dataGridView2.CellContentClick += dataGridView2_CellContentClick;
        }



        private void GoogleAgenda_Load(object sender, EventArgs e)
        {

        }

        private void kryptonButton1_Click(object sender, EventArgs e)
        {
            

        }

        private void kryptonButton2_Click(object sender, EventArgs e)
        {
          

            

        }

        private void kryptonWrapLabel1_Click(object sender, EventArgs e)
        {

        }


        private void button1_Click(object sender, EventArgs e)
        {

        
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
        
        }

        private void button10_Click(object sender, EventArgs e)
        {
       
            

        }
       




        private bool agendarColumnAdded = false;
        private bool eventsAttached = false;
        //  !!!!!!!!!!!!!!  NAO APAGAR !!!!!!!!!!!!!!
        private void button9_Click(object sender, EventArgs e)
        {
          

        }


        //private bool agendarColumnAdded = false;
        private bool editarColumnAdded = false;


        //  !!!!!!!!!!!!!!  NAO APAGAR !!!!!!!!!!!!!!
        


        //  !!!!!!!!!!!!!!  NAO APAGAR !!!!!!!!!!!!!!
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

        //  !!!!!!!!!!!!!!  NAO APAGAR !!!!!!!!!!!!!!
        private void button8_Click(object sender, EventArgs e)
        {
            
        }

        //  !!!!!!!!!!!!!!  NAO APAGAR !!!!!!!!!!!!!!
        private void button11_Click(object sender, EventArgs e)
        {
         
        }

        //  !!!!!!!!!!!!!!  NAO APAGAR !!!!!!!!!!!!!!
        private void button12_Click(object sender, EventArgs e)
        {
            
        }





        private void kryptonButton3_Click(object sender, EventArgs e)
        {

            if (kryptonNavigator1.SelectedPage == kryptonPage1) // nome da sua tabPage onde está o DataGridView
            {
                // Cria a conexão com o banco de dados MySQL
                MySqlConnection conn = new MySqlConnection(@"server=" + Properties.Settings.Default.server + ";database=" + Properties.Settings.Default.basedados + ";port=" + Properties.Settings.Default.porta + ";userid=" + Properties.Settings.Default.username + ";password=" + Properties.Settings.Default.password);

                conn.Open();

                // Cria a consulta SQL
                string query = "SELECT IdCliente, Nome, Telemovel, Horario, Horainicio, HoraFim, SmsEnviada FROM marcacoes";

                MySqlCommand cmd = new MySqlCommand(query, conn);


                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                dgvsmspendentes.DataSource = dt;

                conn.Close();
                dgvsmspendentes.Refresh();
            }
        }

        private void kryptonButton6_Click(object sender, EventArgs e)
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

            kryptonDataGridView2.DataSource = dt;


            kryptonDataGridView2.CellFormatting += kryptonDataGridView2_CellFormatting;
            kryptonDataGridView2.CellPainting += kryptonDataGridView2_CellPainting;

            DataGridViewButtonColumn agendarColumn = new DataGridViewButtonColumn();
            agendarColumn.HeaderText = "Agendar";

            kryptonDataGridView2.Columns.Add(agendarColumn);

            kryptonDataGridView2.CellContentClick += kryptonDataGridView2_CellContentClick;




        }


        private void kryptonDataGridView2_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.ColumnIndex == 7 && e.RowIndex >= 0)
            {
                DataGridViewRow row = kryptonDataGridView2.Rows[e.RowIndex];
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


        private void kryptonDataGridView2_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.ColumnIndex == 7) // Verifique se a coluna é a coluna do botão
            {
                DataGridViewRow row = kryptonDataGridView2.Rows[e.RowIndex];
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




        private void kryptonDataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
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




        private void kryptonButton7_Click(object sender, EventArgs e)
        {
            // Aplicando o filtro para mostrar apenas os resultados com a coluna 7 vazia
            (kryptonDataGridView2.DataSource as DataTable).DefaultView.RowFilter = "[Código Cliente] IS NULL";
        }

        private void kryptonButton4_Click(object sender, EventArgs e)
        {
            if (kryptonNavigator1.SelectedPage == kryptonPage1)
            {
                // Cria a conexão com o banco de dados MySQL
                MySqlConnection conn = new MySqlConnection(@"server=" + Properties.Settings.Default.server + ";database=" + Properties.Settings.Default.basedados + ";port=" + Properties.Settings.Default.porta + ";userid=" + Properties.Settings.Default.username + ";password=" + Properties.Settings.Default.password);
                conn.Open();

                // Cria a consulta SQL com a cláusula WHERE
                string query = "SELECT IdCliente, Nome, Telemovel, Horario, Horainicio, HoraFim, SmsEnviada FROM marcacoes WHERE SmsEnviada = 0";

                MySqlCommand cmd = new MySqlCommand(query, conn);

                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                dgvsmspendentes.DataSource = dt;

                conn.Close();
                dgvsmspendentes.Refresh();
            }
        }


        //!!!!!!!!!!!!!!!!!!! ENVIA SMS PARA OS PENDENTES SELECIONADOS NO DATAGRID!!!!!!!!!!!!!!!!!
        //!!!!!!!!!!!!!!!!!!! A FUNCIONAR NAO MEXER  !!!!!!!!!!!!!!!!!

        private async void kryptonButton5_Click(object sender, EventArgs e)
        {
            //!!!!!!!!!!!!!!!!!!! ENVIA SMS PARA OS PENDENTES SELECIONADOS NO DATAGRID!!!!!!!!!!!!!!!!!
         

            // Pergunta ao usuário se deseja enviar a mensagem de texto
            var result = MessageBox.Show("Antes de continuar, verifique se tem a aplicação iniciada no seu telemóvel. Deseja continuar com o envio da mensagem?", "Confirmação", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            // Verifica se o usuário escolheu o botão Sim
            if (result == DialogResult.Yes)
            {
                try
                {
                    // Define a conexão com o banco de dados MySQL
                    MySqlConnection conn = new MySqlConnection(@"server=" + Properties.Settings.Default.server + ";database=" + Properties.Settings.Default.basedados + ";port=" + Properties.Settings.Default.porta + ";userid=" + Properties.Settings.Default.username + ";password=" + Properties.Settings.Default.password);
                    conn.Open();

                    //// Cria a lista de IDs dos clientes selecionados
           
                    List<int> idsSelecionados = new List<int>();
                    foreach (DataGridViewRow row in dgvsmspendentes.Rows)
                    {
                        DataGridViewCheckBoxCell chk = (DataGridViewCheckBoxCell)row.Cells["SmsEnviada"];
                        if (chk.Value != null && (bool)chk.Value)
                        {
                            idsSelecionados.Add(int.Parse(row.Cells["IdCliente"].Value.ToString()));
                        }
                    }
                    // Verifica se a lista de IDs selecionados não está vazia
                    if (idsSelecionados.Count > 0)
                    {
                        // Define a consulta SQL com a cláusula WHERE modificada para filtrar apenas os clientes selecionados com SmsEnviada = 0
                        //   string query = "SELECT IdCliente, Nome, Telemovel, Horario, Horainicio, HoraFim, CAST(SmsEnviada AS BOOLEAN) AS SmsEnviada FROM marcacoes WHERE SmsEnviada = 0 AND SmsEnviada IS NOT NULL AND IdCliente IN(" + string.Join(", ", idsSelecionados) + ")";
                        string query = "SELECT IdCliente, Nome, Telemovel, Horario, Horainicio, HoraFim, IF(SmsEnviada = 0, FALSE, TRUE) AS SmsEnviada FROM marcacoes WHERE SmsEnviada = 0 AND SmsEnviada IS NOT NULL AND IdCliente IN(" + string.Join(", ", idsSelecionados) + ")";
                        MySqlCommand cmd = new MySqlCommand(query, conn);

                        MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                        DataTable dt = new DataTable();
                        da.Fill(dt);

                        // Fecha a conexão com o banco de dados
                        conn.Close();

                        // Percorre os dados da DataTable para enviar os SMS
                        foreach (DataRow row in dt.Rows)
                        {
                            string from = "+351910045307"; // Número de origem PASSAR DEPOIS PARA PARAMETRO
                            string to = row["Telemovel"].ToString();
                            string nome = row["Nome"].ToString();
                            string horario = row["Horario"].ToString();
                            string horaInicio = row["Horainicio"].ToString();
                            string content = $"Estimado {nome}, informamos que tem marcação no dia {horario} às {horaInicio}";
                            bool enviadoComSucesso = await EnviarSMS(from, to, content);

                            if (enviadoComSucesso)
                            {
                                // Atualiza o status da mensagem SMS no banco de dados
                                int idMarcacao = int.Parse(row["IdCliente"].ToString());
                                string updateQuery = $"UPDATE marcacoes SET SmsEnviada = 1 WHERE IdCliente = {idMarcacao}";

                                MySqlCommand updateCmd = new MySqlCommand(updateQuery, conn);
                                conn.Open();
                                updateCmd.ExecuteNonQuery();
                                conn.Close();
                            }
                        }

                        MessageBox.Show("Todos os SMS foram enviados com sucesso!");
                    }
                }
                
                catch (Exception ex)
                {
                    MessageBox.Show($"Erro ao enviar SMS: {ex.Message}");
                }
         
                    }


               
        }




        // !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!   Daqui para BAIXO é codigo para enviar sms !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!

        // !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!   Daqui para BAIXO é codigo para enviar sms !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!

        // !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!   Daqui para BAIXO é codigo para enviar sms !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!


        private async Task<bool> EnviarSMS(string from, string to, string content)
        {
            try
            {
                var client = new HttpClient();
                client.DefaultRequestHeaders.Add("x-api-key", "gOhTfeXsLXdN7V-0Sct_GfYxROcqW1iK5JjmmvM3iOaFLkdSlJRFjrvTJQk-g_sb");

                var response = await client.PostAsync(
                    "https://api.httpsms.com/v1/messages/send",
                    new StringContent(
                        System.Text.Json.JsonSerializer.Serialize(new
                        {
                            from = from,
                            To = to,
                            Content = content
                        }),
                        Encoding.UTF8,
                        "application/json"
                    )
                );

                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                // trate a exceção aqui, se necessário
                return false;
            }
        }


        //!!!!!!!!!!!!!!!!!!! ENVIA SMS PARA OS PENDENTES SELECIONADOS NO DATAGRID!!!!!!!!!!!!!!!!!
        //!!!!!!!!!!!!!!!!!!! ENVIA SMS PARA OS PENDENTES SELECIONADOS NO DATAGRID !!!!!!!!!!!!!!!!!
        private async void btnEnviarSMSPENDENTESSELECIONADOS_Click(object sender, EventArgs e)
        {
            //!!!!!!!!!!!!!!!!!!! ENVIA SMS PARA OS PENDENTES SELECIONADOS NO DATAGRID!!!!!!!!!!!!!!!!!


            // Pergunta ao usuário se deseja enviar a mensagem de texto
            var result = MessageBox.Show("Antes de continuar, verifique se tem a aplicação iniciada no seu telemóvel. Deseja continuar com o envio da mensagem?", "Confirmação", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            // Verifica se o usuário escolheu o botão Sim
            if (result == DialogResult.Yes)
            {
                try
                {
                    // Define a conexão com o banco de dados MySQL
                    MySqlConnection conn = new MySqlConnection(@"server=" + Properties.Settings.Default.server + ";database=" + Properties.Settings.Default.basedados + ";port=" + Properties.Settings.Default.porta + ";userid=" + Properties.Settings.Default.username + ";password=" + Properties.Settings.Default.password);
                    conn.Open();

                    //// Cria a lista de IDs dos clientes selecionados

                    List<int> idsSelecionados = new List<int>();
                    foreach (DataGridViewRow row in dgvsmspendentes.Rows)
                    {
                        DataGridViewCheckBoxCell chk = (DataGridViewCheckBoxCell)row.Cells["SmsEnviada"];
                        if (chk.Value != null && (bool)chk.Value)
                        {
                            idsSelecionados.Add(int.Parse(row.Cells["IdCliente"].Value.ToString()));
                        }
                    }
                    // Verifica se a lista de IDs selecionados não está vazia
                    if (idsSelecionados.Count > 0)
                    {
                        // Define a consulta SQL com a cláusula WHERE modificada para filtrar apenas os clientes selecionados com SmsEnviada = 0
                        //   string query = "SELECT IdCliente, Nome, Telemovel, Horario, Horainicio, HoraFim, CAST(SmsEnviada AS BOOLEAN) AS SmsEnviada FROM marcacoes WHERE SmsEnviada = 0 AND SmsEnviada IS NOT NULL AND IdCliente IN(" + string.Join(", ", idsSelecionados) + ")";
                        string query = "SELECT IdCliente, Nome, Telemovel, Horario, Horainicio, HoraFim, IF(SmsEnviada = 0, FALSE, TRUE) AS SmsEnviada FROM marcacoes WHERE SmsEnviada = 0 AND SmsEnviada IS NOT NULL AND IdCliente IN(" + string.Join(", ", idsSelecionados) + ")";
                        MySqlCommand cmd = new MySqlCommand(query, conn);

                        MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                        DataTable dt = new DataTable();
                        da.Fill(dt);

                        // Fecha a conexão com o banco de dados
                        conn.Close();

                        // Percorre os dados da DataTable para enviar os SMS
                        foreach (DataRow row in dt.Rows)
                        {
                            string from = "+351910045307"; // Número de origem PASSAR DEPOIS PARA PARAMETRO
                            string to = row["Telemovel"].ToString();
                            string nome = row["Nome"].ToString();
                            string horario = row["Horario"].ToString();
                            string horaInicio = row["Horainicio"].ToString();
                            string content = $"Estimado {nome}, informamos que tem marcação no dia {horario} às {horaInicio}";
                            bool enviadoComSucesso = await EnviarSMS(from, to, content);

                            if (enviadoComSucesso)
                            {
                                // Atualiza o status da mensagem SMS no banco de dados
                                int idMarcacao = int.Parse(row["IdCliente"].ToString());
                                string updateQuery = $"UPDATE marcacoes SET SmsEnviada = 1 WHERE IdCliente = {idMarcacao}";

                                MySqlCommand updateCmd = new MySqlCommand(updateQuery, conn);
                                conn.Open();
                                updateCmd.ExecuteNonQuery();
                                conn.Close();
                            }
                        }

                        MessageBox.Show("Todos os SMS foram enviados com sucesso!");
                    }
                }

                catch (Exception ex)
                {
                    MessageBox.Show($"Erro ao enviar SMS: {ex.Message}");
                }

            }

        }




        //!!!!!!!!!!!!!!!!!!! ENVIA SMS PARA TODOS OS PENDENTES !!!!!!!!!!!!!!!!!
        //!!!!!!!!!!!!!!!!!!! ENVIA SMS PARA TODOS OS PENDENTES !!!!!!!!!!!!!!!!!
        private async void btnEnviarSMS_Click(object sender, EventArgs e)
        {
            try
            {
                // Define a conexão com o banco de dados MySQL
                MySqlConnection conn = new MySqlConnection(@"server=" + Properties.Settings.Default.server + ";database=" + Properties.Settings.Default.basedados + ";port=" + Properties.Settings.Default.porta + ";userid=" + Properties.Settings.Default.username + ";password=" + Properties.Settings.Default.password);
                conn.Open();

                // Define a consulta SQL
                string query = "SELECT IdCliente, Nome, Telemovel, Horario, Horainicio, HoraFim, SmsEnviada FROM marcacoes WHERE SmsEnviada = 0";

                MySqlCommand cmd = new MySqlCommand(query, conn);

                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                // Fecha a conexão com o banco de dados
                conn.Close();

                // Percorre os dados da DataTable para enviar os SMS
                foreach (DataRow row in dt.Rows)
                {

            

                    string from = "+351910045307"; // Número de origem PASSAR DEPOIS PARA PARAMETRO
                    string to = row["Telemovel"].ToString();
                    // string content = "Estimado" + tbnomepaciente.Text + "informamos que tem marcação no dia " + tbhorario.Text + " às " + tbhorainicio.Text; // Conteúdo da mensagem - Vamos definir o tipo de tratamento a ir buscar as mensagens 
                    string nome = row["Nome"].ToString();
                    string horario = row["Horario"].ToString();
                    string horaInicio = row["Horainicio"].ToString();
                   
                    string content = $"Estimado {nome}, informamos que tem marcação no dia {horario} às {horaInicio}";
                    bool enviadoComSucesso = await EnviarSMS(from, to, content);

                    if (enviadoComSucesso)
                    {
                        // Atualiza o status da mensagem SMS no banco de dados
                        int idMarcacao = int.Parse(row["IdCliente"].ToString());
                        string updateQuery = $"UPDATE marcacoes SET SmsEnviada = 1 WHERE IdCliente = {idMarcacao}";

                        MySqlCommand updateCmd = new MySqlCommand(updateQuery, conn);
                        conn.Open();
                        updateCmd.ExecuteNonQuery();
                        conn.Close();
                    }
                }

                MessageBox.Show("Todos os SMS foram enviados com sucesso!");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao enviar SMS: {ex.Message}");
            }
        }

        private void kryptonButton12_Click(object sender, EventArgs e)
        {
            // Aplicando o filtro para mostrar apenas os resultados com a coluna 7 não vazia
            (kryptonDataGridView2.DataSource as DataTable).DefaultView.RowFilter = "[Código Cliente] IS NOT NULL";
        }

        private void kryptonButton13_Click(object sender, EventArgs e)
        {
            // Removendo o filtro para mostrar todos os resultados
            (kryptonDataGridView2.DataSource as DataTable).DefaultView.RowFilter = string.Empty;
        }

        private void btnzxca_Click(object sender, EventArgs e)
        {

            



                // !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!   Daqui para cima é codigo para enviar sms !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!

                // !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!   Daqui para cima é codigo para enviar sms !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!

                // !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!   Daqui para cima é codigo para enviar sms !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!

            }

        private void button1_Click_1(object sender, EventArgs e)
        {
            Agenda form2 = new Agenda();  // Instancia o novo formulário
            form2.ShowDialog();        // Abre o novo formulário como uma janela modal
        }
    }
    }









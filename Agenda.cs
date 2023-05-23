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

namespace Impar
{
    public partial class Agenda : Form
    {
        private List<CalendarItem> _items = new List<CalendarItem>();

        public Agenda()
        {
            InitializeComponent();

        }


        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            var senderGrid = (DataGridView)sender;

            if (senderGrid.Columns[e.ColumnIndex] is DataGridViewButtonColumn &&
                e.RowIndex >= 0)
            {
                //TODO - Button Clicked - Implement the button-click event handling here
            }
        }

          private void dataGridView2_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.ColumnIndex == dataGridView2.Columns["Ação"].Index && e.RowIndex >= 0) // Verifica se é a coluna "Ação" e não é o cabeçalho
            {
                var buttonCell = dataGridView2.Rows[e.RowIndex].Cells["Ação"] as DataGridViewButtonCell;
                if (buttonCell != null && buttonCell.Value != null)
                {
                    string buttonText = buttonCell.Value.ToString();
                    if (buttonText == "Marcar")
                    {
                        buttonCell.Value = "Marcar";
                    }
                    else if (buttonText == "Editar")
                    {
                        buttonCell.Value = "Editar";
                    }
                }
            }
        }

        private DataTable dt; // Declare o DataTable fora do método para que possa ser reutilizado

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

            foreach (DateTime horario in horariosDisponiveis)
            {
                var agendamento = agendamentos.FirstOrDefault(a => horario.Date == a.Inicio.Date && horario.TimeOfDay >= a.Inicio.TimeOfDay && horario.TimeOfDay < a.Fim.TimeOfDay);

                if (agendamento != null)
                {
                    var eventItem = events.Items.FirstOrDefault(ev => ev.Start.DateTime.Value == agendamento.Inicio && ev.End.DateTime.Value == agendamento.Fim);

                    if (eventItem != null)
                    {
                        string titulo = eventItem.Summary;
                        string descricao = eventItem.Description;
                        dataGridView2.Rows.Add("Editar", horario.ToString("dd/MM/yyyy"), horario.ToString("HH:mm"), "Ocupado", agendamento.Fim.ToString("HH:mm"), titulo, descricao);
                    }
                    else
                    {
                        dataGridView2.Rows.Add("Editar", horario.ToString("dd/MM/yyyy"), horario.ToString("HH:mm"), "Ocupado", agendamento.Fim.ToString("HH:mm"), "", "");
                    }
                }
                else
                {
                    dataGridView2.Rows.Add("Marcar", horario.ToString("dd/MM/yyyy"), horario.ToString("HH:mm"), "Disponível", "", "", "");
                }
            }
        }

        private void Agenda_Load(object sender, EventArgs e)
        {

        }

        private void monthView1_SelectionChanged(object sender, EventArgs e)
        {
         
        }
        //private void dataGridView2_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        //{
        //    if (e.ColumnIndex == dataGridView2.Columns["Ação"].Index && e.RowIndex >= 0) // Verifica se é a coluna "Ação" e não é o cabeçalho
        //    {
        //        var buttonCell = dataGridView2.Rows[e.RowIndex].Cells["Ação"] as DataGridViewButtonCell;
        //        if (buttonCell != null && buttonCell.Value != null)
        //        {
        //            string buttonText = buttonCell.Value.ToString();
        //            if (buttonText == "Marcar")
        //            {
        //                buttonCell.Value = "Marcar";
        //            }
        //            else if (buttonText == "Editar")
        //            {
        //                buttonCell.Value = "Editar";
        //            }
        //        }
        //    }
        //}

        //private DataTable dt; // Declare o DataTable fora do método para que possa ser reutilizado

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

        //    foreach (DateTime horario in horariosDisponiveis)
        //    {
        //        var agendamento = agendamentos.FirstOrDefault(a => horario.Date == a.Inicio.Date && horario.TimeOfDay >= a.Inicio.TimeOfDay && horario.TimeOfDay < a.Fim.TimeOfDay);

        //        if (agendamento != null)
        //        {
        //            var eventItem = events.Items.FirstOrDefault(ev => ev.Start.DateTime.Value == agendamento.Inicio && ev.End.DateTime.Value == agendamento.Fim);

        //            if (eventItem != null)
        //            {
        //                string titulo = eventItem.Summary;
        //                string descricao = eventItem.Description;
        //                dataGridView2.Rows.Add("Editar", horario.ToString("dd/MM/yyyy"), horario.ToString("HH:mm"), "Ocupado", agendamento.Fim.ToString("HH:mm"), titulo, descricao);
        //            }
        //            else
        //            {
        //                dataGridView2.Rows.Add("Editar", horario.ToString("dd/MM/yyyy"), horario.ToString("HH:mm"), "Ocupado", agendamento.Fim.ToString("HH:mm"), "", "");
        //            }
        //        }
        //        else
        //        {
        //            dataGridView2.Rows.Add("Marcar", horario.ToString("dd/MM/yyyy"), horario.ToString("HH:mm"), "Disponível", "", "", "");
        //        }
        //    }
        //}


    }
}













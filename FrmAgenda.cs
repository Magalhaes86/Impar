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
using DDay.iCal;

namespace Impar
{
    public partial class FrmAgenda : Form
    {
        public FrmAgenda()
        {
            InitializeComponent();
        }

        private async void FrmAgenda_Load(object sender, EventArgs e)
        {
            DataGridDesign.CustomizeKryptonDataGridView5(kryptonDataGridView5);

            // Atribuir valores aos DateTimePickers
            // Convert string values to DateTime objects
            DateTime horarioManha = DateTime.Parse(Properties.Settings.Default.HorarioManha);
            DateTime horarioFimManha = DateTime.Parse(Properties.Settings.Default.HorarioFimManha);
            DateTime horarioTarde = DateTime.Parse(Properties.Settings.Default.HorarioTarde);
            DateTime horarioFimTarde = DateTime.Parse(Properties.Settings.Default.HorarioFimTarde);

            // Assign values to DateTimePickers
            DtHoraInicioManha.Value = horarioManha;
            DtHoraFimManha.Value = horarioFimManha;
            DtHoraInicioTarde.Value = horarioTarde;
            DtHoraFimTarde.Value = horarioFimTarde;

            // Set the values of DtDataInicio and DtDataFim to today's date
            DtDataInicio.Value = DateTime.Today;
            DtDataFim.Value = DateTime.Today;


            await obterTODASmarcacoes();

        }


        private void kryptonDataGridView5_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            var senderGrid = (DataGridView)sender;

            if (senderGrid.Columns[e.ColumnIndex] is DataGridViewButtonColumn &&
                e.RowIndex >= 0)
            {
                var cell = kryptonDataGridView5.Rows[e.RowIndex].Cells[e.ColumnIndex];

                if (cell is DataGridViewButtonCell)
                {
                    string buttonText = cell.Value?.ToString();

                    if (buttonText == "Marcar")
                    {
                        string data = kryptonDataGridView5.Rows[e.RowIndex].Cells["Data"].Value?.ToString();
                        string horario = kryptonDataGridView5.Rows[e.RowIndex].Cells["Horário"].Value?.ToString();
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
                        string data = kryptonDataGridView5.Rows[e.RowIndex].Cells["Data"].Value?.ToString();
                        string horario = kryptonDataGridView5.Rows[e.RowIndex].Cells["Horário"].Value?.ToString();
                        string ocupadoAte = kryptonDataGridView5.Rows[e.RowIndex].Cells["Ocupado até"].Value?.ToString();
                        string titulo = kryptonDataGridView5.Rows[e.RowIndex].Cells["Título"].Value?.ToString();
                        string descricao = kryptonDataGridView5.Rows[e.RowIndex].Cells["Descrição"].Value?.ToString();
                        string eventId = kryptonDataGridView5.Rows[e.RowIndex].Cells["EventId"].Value?.ToString();

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
        }

        private void kryptonDataGridView5_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.ColumnIndex == kryptonDataGridView5.Columns["Ação"].Index && e.RowIndex >= 0)
            {
                string buttonText = kryptonDataGridView5.Rows[e.RowIndex].Cells[e.ColumnIndex].Value?.ToString() ?? string.Empty;
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




        //private void obterTODASmarcacoes()
        //{
        //    // Criando uma nova instância do Google Credential
        //    GoogleCredential credential;
        //    using (var stream = new FileStream("C:\\Desenvolvimentos\\EmDesenvolvimento\\IMPAR\\Impar\\bin\\Debug\\keys.json", FileMode.Open, FileAccess.Read))
        //    {
        //        credential = GoogleCredential.FromStream(stream)
        //            .CreateScoped(CalendarService.Scope.Calendar);
        //    }

        //    // Criando o serviço de calendário
        //    var service = new CalendarService(new BaseClientService.Initializer()
        //    {
        //        HttpClientInitializer = credential,
        //        ApplicationName = "GoogleAgenda"
        //    });

        //    // Definindo os parâmetros para a pesquisa do evento
        //    EventsResource.ListRequest request = service.Events.List("marcosmagalhaes86@gmail.com");
        //    request.TimeMin = DateTime.Today.AddMonths(-3);
        //    request.TimeMax = DateTime.Today.AddDays(1);
        //    request.ShowDeleted = false;
        //    request.SingleEvents = true;
        //    request.OrderBy = EventsResource.ListRequest.OrderByEnum.StartTime;

        //    // Executando a pesquisa
        //    Events events = request.Execute();

        //    // Processando os eventos encontrados
        //    List<Agendamento> agendamentos = new List<Agendamento>();
        //    if (events.Items != null && events.Items.Count > 0)
        //    {
        //        foreach (var eventItem in events.Items)
        //        {
        //            agendamentos.Add(new Agendamento()
        //            {
        //                EventId = eventItem.Id,
        //                Inicio = eventItem.Start.DateTime.Value,
        //                Fim = eventItem.End.DateTime.Value
        //            });
        //        }
        //    }

        //    DateTime startDate = DateTime.Today.AddMonths(-3);
        //    DateTime endDate = DateTime.Today;
        //    DateTime morningStartTime = DtHoraInicioManha.Value;
        //    DateTime morningEndTime = DtHoraFimManha.Value;
        //    DateTime afternoonStartTime = DtHoraInicioTarde.Value;
        //    DateTime afternoonEndTime = DtHoraFimTarde.Value;

        //    List<DateTime> horariosDisponiveis = GetAvailableTimeSlots(startDate, endDate, morningStartTime, morningEndTime, afternoonStartTime, afternoonEndTime);

        //    foreach (Agendamento agendamento in agendamentos)
        //    {
        //        for (int i = horariosDisponiveis.Count - 1; i >= 0; i--)
        //        {
        //            DateTime horarioDisponivel = horariosDisponiveis[i];
        //            if (horarioDisponivel >= agendamento.Inicio && horarioDisponivel < agendamento.Fim)
        //            {
        //                horariosDisponiveis.RemoveAt(i);
        //            }
        //        }
        //    }

        //    DataTable dt = new DataTable();
        //    // Adicione colunas adicionais
        //    dt.Columns.Add("Data");
        //    dt.Columns.Add("Hora Início");
        //    dt.Columns.Add("Hora Fim");
        //    dt.Columns.Add("EventId");
        //    dt.Columns.Add("Título");
        //    dt.Columns.Add("Descrição");
        //    dt.Columns.Add("Código Cliente");

        //    foreach (Agendamento agendamento in agendamentos)
        //    {
        //        // Obtendo o evento correspondente ao agendamento
        //        var eventItem = events.Items.FirstOrDefault(ev => ev.Start.DateTime.Value == agendamento.Inicio && ev.End.DateTime.Value == agendamento.Fim);

        //        int? codigoCliente = GetClientIdByGoogleEventId(agendamento.EventId);

        //        if (eventItem != null)
        //        {
        //            string titulo = eventItem.Summary;
        //            string descricao = eventItem.Description;

        //            dt.Rows.Add(agendamento.Inicio.ToString("dd/MM/yyyy"), agendamento.Inicio.ToString("HH:mm"), agendamento.Fim.ToString("HH:mm"), agendamento.EventId, titulo, descricao, codigoCliente);
        //        }
        //        else
        //        {
        //            dt.Rows.Add(agendamento.Inicio.ToString("dd/MM/yyyy"), agendamento.Inicio.ToString("HH:mm"), agendamento.Fim.ToString("HH:mm"), agendamento.EventId, "", "", codigoCliente);
        //        }
        //    }

        //    // Verifique se a coluna "Agendar" já existe
        //    bool agendarColumnExists = false;
        //    foreach (DataGridViewColumn column in kryptonDataGridView2.Columns)
        //    {
        //        if (column.Name == "Agendar")
        //        {
        //            agendarColumnExists = true;
        //            break;
        //        }
        //    }

        //    if (!agendarColumnExists)
        //    {
        //        // Personalize a coluna "Agendar" para exibir botões
        //        DataGridViewButtonColumn agendarColumn = new DataGridViewButtonColumn();
        //        agendarColumn.HeaderText = "Agendar";
        //        agendarColumn.Name = "Agendar";
        //        agendarColumn.Text = "Agendar";
        //        agendarColumn.UseColumnTextForButtonValue = true;
        //        kryptonDataGridView2.Invoke(new Action(() => kryptonDataGridView2.Columns.Insert(0, agendarColumn))); // Insira a coluna na primeira posição

        //        // Defina a largura da coluna "Agendar"
        //        kryptonDataGridView2.Invoke(new Action(() => kryptonDataGridView2.Columns[0].Width = 70));

        //        // Defina o estilo de célula para alinhar o texto ao centro
        //        DataGridViewCellStyle cellStyle = new DataGridViewCellStyle();
        //        cellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
        //        kryptonDataGridView2.Invoke(new Action(() => kryptonDataGridView2.Columns[0].DefaultCellStyle = cellStyle));
        //    }

        //    // Atribua o DataTable atualizado ao DataSource do DataGridView usando o método Invoke para atualizar no thread da interface do usuário
        //    kryptonDataGridView2.Invoke(new Action(() => kryptonDataGridView2.DataSource = dt));
        //}


        private async void monthView1_SelectionChanged(object sender, EventArgs e)
        {
            // Desabilitar o calendário enquanto o código é executado
            monthView1.Enabled = false;

            GoogleCalendarService googleCalendarService = null;

            try
            {
                // Cria uma instância da classe GoogleCalendarService
                googleCalendarService = new GoogleCalendarService();
                bool connected = await googleCalendarService.Connect();

                if (connected)
                {
                    DateTime selectedStartDate = monthView1.SelectionStart.Date;
                    DateTime selectedEndDate = monthView1.SelectionEnd.Date;
                    selectedEndDate = selectedEndDate.AddDays(1).AddSeconds(-1);

                    EventsResource.ListRequest request = googleCalendarService.Service.Events.List("marcosmagalhaes86@gmail.com");
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

                    // Atualizar a interface do usuário com os dados obtidos
                    kryptonDataGridView5.Columns.Clear(); // Remove todas as colunas existentes

                    // Adicione as colunas ao dataGridView2
                    DataGridViewButtonColumn btnColumn = new DataGridViewButtonColumn();
                    btnColumn.HeaderText = "Ação";
                    btnColumn.Name = "Ação";
                    kryptonDataGridView5.Columns.Add(btnColumn);
                    kryptonDataGridView5.Columns.Add("Status", "Status");
                    kryptonDataGridView5.Columns.Add("Data", "Data");
                    kryptonDataGridView5.Columns.Add("Horário", "Horário");
                    kryptonDataGridView5.Columns.Add("Ocupado até", "Ocupado até");
                    kryptonDataGridView5.Columns.Add("Título", "Título");
                    kryptonDataGridView5.Columns.Add("Descrição", "Descrição");
                    kryptonDataGridView5.Columns.Add("EventId", "EventId");

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
                                kryptonDataGridView5.Rows.Add("Editar", "Ocupado", horario.ToString("dd/MM/yyyy"), horario.ToString("HH:mm"), agendamento.Fim.ToString("HH:mm"), titulo, descricao, eventId);
                            }
                            else
                            {
                                kryptonDataGridView5.Rows.Add("Editar", "Ocupado", horario.ToString("dd/MM/yyyy"), horario.ToString("HH:mm"), agendamento.Fim.ToString("HH:mm"), "", "", "");
                            }
                        }
                        else
                        {
                            kryptonDataGridView5.Rows.Add("Marcar", "Disponível", horario.ToString("dd/MM/yyyy"), horario.ToString("HH:mm"), "", "", "", "");
                        }
                    }

                    // Ocultar colunas "Título", "Descrição" e "EventId"
                    kryptonDataGridView5.Columns["Título"].Visible = false;
                    kryptonDataGridView5.Columns["Descrição"].Visible = false;
                    kryptonDataGridView5.Columns["EventId"].Visible = false;

                    kryptonDataGridView5.CellFormatting += kryptonDataGridView5_CellFormatting;
                    kryptonDataGridView5.CellContentClick += kryptonDataGridView5_CellContentClick;
                    DataGridDesign.CustomizeKryptonDataGridView5(kryptonDataGridView5);
                }
                else
                {
                    // Lidere com o caso de falha na conexão, se necessário
                    MessageBox.Show("Falha na conexão com o Google Calendar.");
                }
            }
            catch (Exception ex)
            {
                // Trate a exceção de alguma forma apropriada, por exemplo, mostrando uma mensagem de erro
                MessageBox.Show("Ocorreu um erro ao obter as marcações: " + ex.Message);
            }
            finally
            {
                if (googleCalendarService != null)
                {
                    // Termina a conexão com o Google Calendar
                    googleCalendarService.Service.Dispose();
                }

                // Reabilitar o calendário após a conclusão do código
                monthView1.Enabled = true;
            }

            //// Desabilitar o calendário enquanto o código é executado
            //monthView1.Enabled = false;

            ////DateTime selectedStartDate = e.Start.Date;
            ////DateTime selectedEndDate = e.End.Date;
            //DateTime selectedStartDate = monthView1.SelectionStart.Date;
            //DateTime selectedEndDate = monthView1.SelectionEnd.Date;

            //selectedEndDate = selectedEndDate.AddDays(1).AddSeconds(-1);

            //GoogleCredential credential;
            //using (var stream = new FileStream("C:\\Desenvolvimentos\\EmDesenvolvimento\\IMPAR\\Impar\\bin\\Debug\\keys.json", FileMode.Open, FileAccess.Read))
            //{
            //    credential = GoogleCredential.FromStream(stream)
            //        .CreateScoped(CalendarService.Scope.Calendar);
            //}

            //var service = new CalendarService(new BaseClientService.Initializer()
            //{
            //    HttpClientInitializer = credential,
            //    ApplicationName = "GoogleAgenda"
            //});

            //EventsResource.ListRequest request = service.Events.List("marcosmagalhaes86@gmail.com");
            //request.TimeMin = selectedStartDate;
            //request.TimeMax = selectedEndDate;
            //request.ShowDeleted = false;
            //request.SingleEvents = true;
            //request.OrderBy = EventsResource.ListRequest.OrderByEnum.StartTime;

            //Events events = request.Execute();

            //List<Agendamento> agendamentos = new List<Agendamento>();
            //if (events.Items != null && events.Items.Count > 0)
            //{
            //    foreach (var eventItem in events.Items)
            //    {
            //        agendamentos.Add(new Agendamento()
            //        {
            //            Inicio = eventItem.Start.DateTime.Value,
            //            Fim = eventItem.End.DateTime.Value
            //        });
            //    }
            //}

            //TimeSpan morningStartTime = DtHoraInicioManha.Value.TimeOfDay;
            //TimeSpan morningEndTime = DtHoraFimManha.Value.TimeOfDay;
            //TimeSpan afternoonStartTime = DtHoraInicioTarde.Value.TimeOfDay;
            //TimeSpan afternoonEndTime = DtHoraFimTarde.Value.TimeOfDay;

            //List<DateTime> horariosDisponiveis = new List<DateTime>();

            //for (DateTime currentDate = selectedStartDate.Date; currentDate <= selectedEndDate.Date; currentDate = currentDate.AddDays(1))
            //{
            //    DateTime currentMorningStartTime = currentDate.Date + morningStartTime;
            //    DateTime currentMorningEndTime = currentDate.Date + morningEndTime;
            //    DateTime currentAfternoonStartTime = currentDate.Date + afternoonStartTime;
            //    DateTime currentAfternoonEndTime = currentDate.Date + afternoonEndTime;

            //    for (DateTime time = currentMorningStartTime; time < currentMorningEndTime; time = time.AddMinutes(30))
            //    {
            //        horariosDisponiveis.Add(time);
            //    }

            //    for (DateTime time = currentAfternoonStartTime; time < currentAfternoonEndTime; time = time.AddMinutes(30))
            //    {
            //        horariosDisponiveis.Add(time);
            //    }
            //}

            //// Atualizar a interface do usuário com os dados obtidos
            //kryptonDataGridView5.Columns.Clear(); // Remove todas as colunas existentes

            //// Adicione as colunas ao dataGridView2
            //DataGridViewButtonColumn btnColumn = new DataGridViewButtonColumn();
            //btnColumn.HeaderText = "Ação";
            //btnColumn.Name = "Ação";
            //kryptonDataGridView5.Columns.Add(btnColumn);
            //kryptonDataGridView5.Columns.Add("Status", "Status");
            //kryptonDataGridView5.Columns.Add("Data", "Data");
            //kryptonDataGridView5.Columns.Add("Horário", "Horário");
            //kryptonDataGridView5.Columns.Add("Ocupado até", "Ocupado até");
            //kryptonDataGridView5.Columns.Add("Título", "Título");
            //kryptonDataGridView5.Columns.Add("Descrição", "Descrição");
            //kryptonDataGridView5.Columns.Add("EventId", "EventId");

            //foreach (DateTime horario in horariosDisponiveis)
            //{
            //    var agendamento = agendamentos.FirstOrDefault(a => horario.Date == a.Inicio.Date && horario.TimeOfDay >= a.Inicio.TimeOfDay && horario.TimeOfDay < a.Fim.TimeOfDay);

            //    if (agendamento != null)
            //    {
            //        var eventItem = events.Items.FirstOrDefault(ev => ev.Start.DateTime.Value == agendamento.Inicio && ev.End.DateTime.Value == agendamento.Fim);

            //        if (eventItem != null)
            //        {
            //            string eventId = eventItem.Id;
            //            string titulo = eventItem.Summary;
            //            string descricao = eventItem.Description;
            //            kryptonDataGridView5.Rows.Add("Editar", "Ocupado", horario.ToString("dd/MM/yyyy"), horario.ToString("HH:mm"), agendamento.Fim.ToString("HH:mm"), titulo, descricao, eventId);
            //        }
            //        else
            //        {
            //            kryptonDataGridView5.Rows.Add("Editar", "Ocupado", horario.ToString("dd/MM/yyyy"), horario.ToString("HH:mm"), agendamento.Fim.ToString("HH:mm"), "", "", "");
            //        }
            //    }
            //    else
            //    {
            //        kryptonDataGridView5.Rows.Add("Marcar", "Disponível", horario.ToString("dd/MM/yyyy"), horario.ToString("HH:mm"), "", "", "", "");
            //    }
            //}

            //// Ocultar colunas "Título", "Descrição" e "EventId"
            //kryptonDataGridView5.Columns["Título"].Visible = false;
            //kryptonDataGridView5.Columns["Descrição"].Visible = false;
            //kryptonDataGridView5.Columns["EventId"].Visible = false;

            //kryptonDataGridView5.CellFormatting += kryptonDataGridView5_CellFormatting;
            //kryptonDataGridView5.CellContentClick += kryptonDataGridView5_CellContentClick;
            //DataGridDesign.CustomizeKryptonDataGridView5(kryptonDataGridView5);

            //// Reabilitar o calendário após a conclusão do código
            //monthView1.Enabled = true;

        }


        private async Task obterTODASmarcacoes()
        {
            await Task.Run(async () =>
            {
                try
                {
                    // Cria uma instância da classe GoogleCalendarService
                    GoogleCalendarService googleCalendarService = new GoogleCalendarService();
                    bool connected = await googleCalendarService.Connect();

                    if (connected)
                    {
                        // Define os parâmetros para a pesquisa do evento
                        EventsResource.ListRequest request = googleCalendarService.Service.Events.List("marcosmagalhaes86@gmail.com");
                        request.TimeMin = DateTime.Today.AddMonths(-3);
                        request.TimeMax = DateTime.Today.AddDays(1);
                        request.ShowDeleted = false;
                        request.SingleEvents = true;
                        request.OrderBy = EventsResource.ListRequest.OrderByEnum.StartTime;

                        // Executa a pesquisa
                        Events events = request.Execute();

                        // Processa os eventos encontrados
                        List<Agendamento> agendamentos = new List<Agendamento>();
                        if (events.Items != null && events.Items.Count > 0)
                        {
                            foreach (var eventItem in events.Items)
                            {
                                agendamentos.Add(new Agendamento()
                                {
                                    EventId = eventItem.Id,
                                    Inicio = eventItem.Start.DateTime.Value,
                                    Fim = eventItem.End.DateTime.Value
                                });
                            }
                        }

                        DateTime startDate = DateTime.Today.AddMonths(-3);
                        DateTime endDate = DateTime.Today;
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

                        // Verifica se a coluna "Agendar" já existe
                        bool agendarColumnExists = false;
                        foreach (DataGridViewColumn column in kryptonDataGridView2.Columns)
                        {
                            if (column.Name == "Agendar")
                            {
                                agendarColumnExists = true;
                                break;
                            }
                        }

                        if (!agendarColumnExists)
                        {
                            // Personaliza a coluna "Agendar" para exibir botões
                            DataGridViewButtonColumn agendarColumn = new DataGridViewButtonColumn();
                            agendarColumn.HeaderText = "Agendar";
                            agendarColumn.Name = "Agendar";
                            agendarColumn.Text = "Agendar";
                            agendarColumn.UseColumnTextForButtonValue = true;
                            kryptonDataGridView2.Invoke(new Action(() => kryptonDataGridView2.Columns.Insert(0, agendarColumn))); // Insere a coluna na primeira posição

                            // Define a largura da coluna "Agendar"
                            kryptonDataGridView2.Invoke(new Action(() => kryptonDataGridView2.Columns[0].Width = 70));

                            // Define o estilo de célula para alinhar o texto ao centro
                            DataGridViewCellStyle cellStyle = new DataGridViewCellStyle();
                            cellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                            kryptonDataGridView2.Invoke(new Action(() => kryptonDataGridView2.Columns[0].DefaultCellStyle = cellStyle));
                        }

                        // Atribui o DataTable atualizado ao DataSource do DataGridView usando o método Invoke para atualizar no thread da interface do usuário
                        kryptonDataGridView2.Invoke(new Action(() => kryptonDataGridView2.DataSource = dt));

                        googleCalendarService.Service.Dispose();
                    }
                    else
                    {
                        // Lidere com o caso de falha na conexão, se necessário
                        MessageBox.Show("Falha na conexão com o Google Calendar.");
                    }
                }
                catch (Exception ex)
                {
                    // Trate a exceção de alguma forma apropriada, por exemplo, mostrando uma mensagem de erro
                    MessageBox.Show("Ocorreu um erro ao obter as marcações: " + ex.Message);
                }
            });
        }


        //private async Task obterTODASmarcacoes()
        //{
        //    await Task.Run(() =>
        //    {
        //        try
        //        {
        //            // Criando uma nova instância do Google Credential
        //            GoogleCredential credential;
        //            using (var stream = new FileStream("C:\\Desenvolvimentos\\EmDesenvolvimento\\IMPAR\\Impar\\bin\\Debug\\keys.json", FileMode.Open, FileAccess.Read))
        //            {
        //                credential = GoogleCredential.FromStream(stream)
        //                    .CreateScoped(CalendarService.Scope.Calendar);
        //            }

        //            // Criando o serviço de calendário
        //            var service = new CalendarService(new BaseClientService.Initializer()
        //            {
        //                HttpClientInitializer = credential,
        //                ApplicationName = "GoogleAgenda"
        //            });

        //            // Definindo os parâmetros para a pesquisa do evento
        //            EventsResource.ListRequest request = service.Events.List("marcosmagalhaes86@gmail.com");
        //            request.TimeMin = DateTime.Today.AddMonths(-3);
        //            request.TimeMax = DateTime.Today.AddDays(1);
        //            request.ShowDeleted = false;
        //            request.SingleEvents = true;
        //            request.OrderBy = EventsResource.ListRequest.OrderByEnum.StartTime;

        //            // Executando a pesquisa
        //            Events events = request.Execute();

        //            // Processando os eventos encontrados
        //            List<Agendamento> agendamentos = new List<Agendamento>();
        //            if (events.Items != null && events.Items.Count > 0)
        //            {
        //                foreach (var eventItem in events.Items)
        //                {
        //                    agendamentos.Add(new Agendamento()
        //                    {
        //                        EventId = eventItem.Id,
        //                        Inicio = eventItem.Start.DateTime.Value,
        //                        Fim = eventItem.End.DateTime.Value
        //                    });
        //                }
        //            }

        //            DateTime startDate = DateTime.Today.AddMonths(-3);
        //            DateTime endDate = DateTime.Today;
        //            DateTime morningStartTime = DtHoraInicioManha.Value;
        //            DateTime morningEndTime = DtHoraFimManha.Value;
        //            DateTime afternoonStartTime = DtHoraInicioTarde.Value;
        //            DateTime afternoonEndTime = DtHoraFimTarde.Value;

        //            List<DateTime> horariosDisponiveis = GetAvailableTimeSlots(startDate, endDate, morningStartTime, morningEndTime, afternoonStartTime, afternoonEndTime);

        //            foreach (Agendamento agendamento in agendamentos)
        //            {
        //                for (int i = horariosDisponiveis.Count - 1; i >= 0; i--)
        //                {
        //                    DateTime horarioDisponivel = horariosDisponiveis[i];
        //                    if (horarioDisponivel >= agendamento.Inicio && horarioDisponivel < agendamento.Fim)
        //                    {
        //                        horariosDisponiveis.RemoveAt(i);
        //                    }
        //                }
        //            }

        //            DataTable dt = new DataTable();
        //            // Adicione colunas adicionais
        //            dt.Columns.Add("Data");
        //            dt.Columns.Add("Hora Início");
        //            dt.Columns.Add("Hora Fim");
        //            dt.Columns.Add("EventId");
        //            dt.Columns.Add("Título");
        //            dt.Columns.Add("Descrição");
        //            dt.Columns.Add("Código Cliente");

        //            foreach (Agendamento agendamento in agendamentos)
        //            {
        //                // Obtendo o evento correspondente ao agendamento
        //                var eventItem = events.Items.FirstOrDefault(ev => ev.Start.DateTime.Value == agendamento.Inicio && ev.End.DateTime.Value == agendamento.Fim);

        //                int? codigoCliente = GetClientIdByGoogleEventId(agendamento.EventId);

        //                if (eventItem != null)
        //                {
        //                    string titulo = eventItem.Summary;
        //                    string descricao = eventItem.Description;

        //                    dt.Rows.Add(agendamento.Inicio.ToString("dd/MM/yyyy"), agendamento.Inicio.ToString("HH:mm"), agendamento.Fim.ToString("HH:mm"), agendamento.EventId, titulo, descricao, codigoCliente);
        //                }
        //                else
        //                {
        //                    dt.Rows.Add(agendamento.Inicio.ToString("dd/MM/yyyy"), agendamento.Inicio.ToString("HH:mm"), agendamento.Fim.ToString("HH:mm"), agendamento.EventId, "", "", codigoCliente);
        //                }
        //            }

        //            // Verifique se a coluna "Agendar" já existe
        //            bool agendarColumnExists = false;
        //            foreach (DataGridViewColumn column in kryptonDataGridView2.Columns)
        //            {
        //                if (column.Name == "Agendar")
        //                {
        //                    agendarColumnExists = true;
        //                    break;
        //                }
        //            }

        //            if (!agendarColumnExists)
        //            {
        //                // Personalize a coluna "Agendar" para exibir botões
        //                DataGridViewButtonColumn agendarColumn = new DataGridViewButtonColumn();
        //                agendarColumn.HeaderText = "Agendar";
        //                agendarColumn.Name = "Agendar";
        //                agendarColumn.Text = "Agendar";
        //                agendarColumn.UseColumnTextForButtonValue = true;
        //                kryptonDataGridView2.Invoke(new Action(() => kryptonDataGridView2.Columns.Insert(0, agendarColumn))); // Insira a coluna na primeira posição

        //                // Defina a largura da coluna "Agendar"
        //                kryptonDataGridView2.Invoke(new Action(() => kryptonDataGridView2.Columns[0].Width = 70));

        //                // Defina o estilo de célula para alinhar o texto ao centro
        //                DataGridViewCellStyle cellStyle = new DataGridViewCellStyle();
        //                cellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
        //                kryptonDataGridView2.Invoke(new Action(() => kryptonDataGridView2.Columns[0].DefaultCellStyle = cellStyle));
        //            }

        //            // Atribua o DataTable atualizado ao DataSource do DataGridView usando o método Invoke para atualizar no thread da interface do usuário
        //            kryptonDataGridView2.Invoke(new Action(() => kryptonDataGridView2.DataSource = dt));
        //        }
        //        catch (Exception ex)
        //        {
        //            // Trate a exceção de alguma forma apropriada, por exemplo, mostrando uma mensagem de erro
        //            MessageBox.Show("Ocorreu um erro ao obter as marcações: " + ex.Message);
        //        }
        //    });
        //}













        //private async void monthView1_SelectionChanged(object sender, EventArgs e)
        //{

        //    // Desabilitar o calendário enquanto o código é executado em segundo plano
        //    monthView1.Enabled = false;

        //    // Exibir um indicador de carregamento ou informar o usuário que a busca está em andamento

        //    // Executar o código em segundo plano usando uma tarefa assíncrona
        //    await Task.Run(() =>
        //    {
        //        //DateTime selectedStartDate = e.Start.Date;
        //        //DateTime selectedEndDate = e.End.Date;
        //        DateTime selectedStartDate = monthView1.SelectionStart.Date;
        //        DateTime selectedEndDate = monthView1.SelectionEnd.Date;

        //        selectedEndDate = selectedEndDate.AddDays(1).AddSeconds(-1);

        //        GoogleCredential credential;
        //        using (var stream = new FileStream("C:\\Desenvolvimentos\\EmDesenvolvimento\\IMPAR\\Impar\\bin\\Debug\\keys.json", FileMode.Open, FileAccess.Read))
        //        {
        //            credential = GoogleCredential.FromStream(stream)
        //                .CreateScoped(CalendarService.Scope.Calendar);
        //        }

        //        var service = new CalendarService(new BaseClientService.Initializer()
        //        {
        //            HttpClientInitializer = credential,
        //            ApplicationName = "GoogleAgenda"
        //        });

        //        EventsResource.ListRequest request = service.Events.List("marcosmagalhaes86@gmail.com");
        //        request.TimeMin = selectedStartDate;
        //        request.TimeMax = selectedEndDate;
        //        request.ShowDeleted = false;
        //        request.SingleEvents = true;
        //        request.OrderBy = EventsResource.ListRequest.OrderByEnum.StartTime;

        //        Events events = request.Execute();

        //        List<Agendamento> agendamentos = new List<Agendamento>();
        //        if (events.Items != null && events.Items.Count > 0)
        //        {
        //            foreach (var eventItem in events.Items)
        //            {
        //                agendamentos.Add(new Agendamento()
        //                {
        //                    Inicio = eventItem.Start.DateTime.Value,
        //                    Fim = eventItem.End.DateTime.Value
        //                });
        //            }
        //        }

        //        TimeSpan morningStartTime = DtHoraInicioManha.Value.TimeOfDay;
        //        TimeSpan morningEndTime = DtHoraFimManha.Value.TimeOfDay;
        //        TimeSpan afternoonStartTime = DtHoraInicioTarde.Value.TimeOfDay;
        //        TimeSpan afternoonEndTime = DtHoraFimTarde.Value.TimeOfDay;

        //        List<DateTime> horariosDisponiveis = new List<DateTime>();

        //        for (DateTime currentDate = selectedStartDate.Date; currentDate <= selectedEndDate.Date; currentDate = currentDate.AddDays(1))
        //        {
        //            DateTime currentMorningStartTime = currentDate.Date + morningStartTime;
        //            DateTime currentMorningEndTime = currentDate.Date + morningEndTime;
        //            DateTime currentAfternoonStartTime = currentDate.Date + afternoonStartTime;
        //            DateTime currentAfternoonEndTime = currentDate.Date + afternoonEndTime;

        //            for (DateTime time = currentMorningStartTime; time < currentMorningEndTime; time = time.AddMinutes(30))
        //            {
        //                horariosDisponiveis.Add(time);
        //            }

        //            for (DateTime time = currentAfternoonStartTime; time < currentAfternoonEndTime; time = time.AddMinutes(30))
        //            {
        //                horariosDisponiveis.Add(time);
        //            }
        //        }

        //        // Atualizar a interface do usuário com os dados obtidos em segundo plano
        //        this.Invoke(new Action(() =>
        //        {
        //            kryptonDataGridView5.Columns.Clear(); // Remove todas as colunas existentes

        //            // Adicione as colunas ao dataGridView2
        //            DataGridViewButtonColumn btnColumn = new DataGridViewButtonColumn();
        //            btnColumn.HeaderText = "Ação";
        //            btnColumn.Name = "Ação";
        //            kryptonDataGridView5.Columns.Add(btnColumn);
        //            kryptonDataGridView5.Columns.Add("Status", "Status");
        //            kryptonDataGridView5.Columns.Add("Data", "Data");
        //            kryptonDataGridView5.Columns.Add("Horário", "Horário");
        //            kryptonDataGridView5.Columns.Add("Ocupado até", "Ocupado até");
        //            kryptonDataGridView5.Columns.Add("Título", "Título");
        //            kryptonDataGridView5.Columns.Add("Descrição", "Descrição");
        //            kryptonDataGridView5.Columns.Add("EventId", "EventId");

        //            foreach (DateTime horario in horariosDisponiveis)
        //            {
        //                var agendamento = agendamentos.FirstOrDefault(a => horario.Date == a.Inicio.Date && horario.TimeOfDay >= a.Inicio.TimeOfDay && horario.TimeOfDay < a.Fim.TimeOfDay);

        //                if (agendamento != null)
        //                {
        //                    var eventItem = events.Items.FirstOrDefault(ev => ev.Start.DateTime.Value == agendamento.Inicio && ev.End.DateTime.Value == agendamento.Fim);

        //                    if (eventItem != null)
        //                    {
        //                        string eventId = eventItem.Id;
        //                        string titulo = eventItem.Summary;
        //                        string descricao = eventItem.Description;
        //                        kryptonDataGridView5.Rows.Add("Editar", "Ocupado", horario.ToString("dd/MM/yyyy"), horario.ToString("HH:mm"), agendamento.Fim.ToString("HH:mm"), titulo, descricao, eventId);
        //                    }
        //                    else
        //                    {
        //                        kryptonDataGridView5.Rows.Add("Editar", "Ocupado", horario.ToString("dd/MM/yyyy"), horario.ToString("HH:mm"), agendamento.Fim.ToString("HH:mm"), "", "", "");
        //                    }
        //                }
        //                else
        //                {
        //                    kryptonDataGridView5.Rows.Add("Marcar", "Disponível", horario.ToString("dd/MM/yyyy"), horario.ToString("HH:mm"), "", "", "", "");
        //                }
        //            }
        //            // Ocultar colunas "Título", "Descrição" e "EventId"
        //            kryptonDataGridView5.Columns["Título"].Visible = false;
        //            kryptonDataGridView5.Columns["Descrição"].Visible = false;
        //            kryptonDataGridView5.Columns["EventId"].Visible = false;

        //            kryptonDataGridView5.CellFormatting += kryptonDataGridView5_CellFormatting;
        //            kryptonDataGridView5.CellContentClick += kryptonDataGridView5_CellContentClick;
        //            DataGridDesign.CustomizeKryptonDataGridView5(kryptonDataGridView5);

        //            // Reabilitar o calendário após a conclusão do código em segundo plano
        //            monthView1.Enabled = true;
        //        }));
        //    });

        //    // Reabilitar o calendário após a conclusão do código em segundo plano
        //    monthView1.Enabled = true;

        //}

        private void btnFiltroEntreData_Click(object sender, EventArgs e)
        {
            //DateTime selectedStartDate = e.Start.Date;
            //DateTime selectedEndDate = e.End.Date;
            DateTime selectedStartDate = DtDataInicio.Value.Date;
            DateTime selectedEndDate = DtDataFim.Value.Date;


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

            kryptonDataGridView5.Columns.Clear(); // Remove todas as colunas existentes

            // Adicione as colunas ao dataGridView2
            DataGridViewButtonColumn btnColumn = new DataGridViewButtonColumn();
            btnColumn.HeaderText = "Ação";
            btnColumn.Name = "Ação";
            kryptonDataGridView5.Columns.Add(btnColumn);
            kryptonDataGridView5.Columns.Add("Status", "Status");
            kryptonDataGridView5.Columns.Add("Data", "Data");
            kryptonDataGridView5.Columns.Add("Horário", "Horário");
            kryptonDataGridView5.Columns.Add("Ocupado até", "Ocupado até");
            kryptonDataGridView5.Columns.Add("Título", "Título");
            kryptonDataGridView5.Columns.Add("Descrição", "Descrição");
            kryptonDataGridView5.Columns.Add("EventId", "EventId");

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
                        kryptonDataGridView5.Rows.Add("Editar", "Ocupado", horario.ToString("dd/MM/yyyy"), horario.ToString("HH:mm"), agendamento.Fim.ToString("HH:mm"), titulo, descricao, eventId);
                    }
                    else
                    {
                        kryptonDataGridView5.Rows.Add("Editar", "Ocupado", horario.ToString("dd/MM/yyyy"), horario.ToString("HH:mm"), agendamento.Fim.ToString("HH:mm"), "", "", "");
                    }
                }
                else
                {
                    kryptonDataGridView5.Rows.Add("Marcar", "Disponível", horario.ToString("dd/MM/yyyy"), horario.ToString("HH:mm"), "", "", "", "");
                }
            }
            // Ocultar colunas "Título", "Descrição" e "EventId"
            kryptonDataGridView5.Columns["Título"].Visible = false;
            kryptonDataGridView5.Columns["Descrição"].Visible = false;
            kryptonDataGridView5.Columns["EventId"].Visible = false;

            kryptonDataGridView5.CellFormatting += kryptonDataGridView5_CellFormatting;
            kryptonDataGridView5.CellContentClick += kryptonDataGridView5_CellContentClick;
            DataGridDesign.CustomizeKryptonDataGridView5(kryptonDataGridView5);
        }


        private void FiltrarPorStatus(string status)
        {
            foreach (DataGridViewRow row in kryptonDataGridView5.Rows)
            {
                if (row.Cells["Status"].Value.ToString() == status)
                {
                    row.Visible = true;
                    DataGridDesign.CustomizeKryptonDataGridView5(kryptonDataGridView5);
                }
                else
                {
                    row.Visible = false;
                    DataGridDesign.CustomizeKryptonDataGridView5(kryptonDataGridView5);
                }
            }
        }


        private void btnocupado_Click(object sender, EventArgs e)
        {
            FiltrarPorStatus("Ocupado");
        }

        private void btndisponivel_Click(object sender, EventArgs e)
        {
            FiltrarPorStatus("Disponível");
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


        private void kryptonButton6_Click(object sender, EventArgs e)
        {

            obterTODASmarcacoes();

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
                    e.Paint(e.CellBounds, DataGridViewPaintParts.All & ~DataGridViewPaintParts.ContentBackground);

                    using (Brush b = new SolidBrush(Color.Yellow))
                    {
                        Rectangle buttonBounds = new Rectangle(
                            e.CellBounds.X + 2,
                            e.CellBounds.Y + 2,
                            e.CellBounds.Width - 4,
                            e.CellBounds.Height - 4
                        );

                        e.Graphics.FillRectangle(b, buttonBounds);
                    }

                    // Mude o texto do botão para "Editar"
                    e.PaintContent(e.CellBounds);
                    e.Graphics.DrawString("Editar", e.CellStyle.Font, Brushes.Black, e.CellBounds, StringFormat.GenericDefault);

                    e.Handled = true;
                }
                else
                {
                    e.Paint(e.CellBounds, DataGridViewPaintParts.All & ~DataGridViewPaintParts.ContentBackground);

                    using (Brush b = new SolidBrush(Color.Blue))
                    {
                        Rectangle buttonBounds = new Rectangle(
                            e.CellBounds.X + 2,
                            e.CellBounds.Y + 2,
                            e.CellBounds.Width - 4,
                            e.CellBounds.Height - 4
                        );

                        e.Graphics.FillRectangle(b, buttonBounds);
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
            if (kryptonDataGridView2.Columns[e.ColumnIndex].Name == "Agendar")
            {
                DataGridViewRow row = kryptonDataGridView2.Rows[e.RowIndex];
                object codigoCliente = row.Cells["Código Cliente"].Value;

                // Verifique se o código do cliente está presente
                if (codigoCliente != null && codigoCliente != DBNull.Value)
                {
                    // Mude a cor do botão para amarelo
                    row.Cells[e.ColumnIndex].Style.BackColor = Color.Yellow;
                    row.Cells[e.ColumnIndex].Value = "Editar";
                }
                else
                {
                    // Mude a cor do botão para verde
                    row.Cells[e.ColumnIndex].Style.BackColor = Color.Green;
                    row.Cells[e.ColumnIndex].Value = "Agendar";
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





        //private void obterTODASmarcacoes()
        //{  // Criando uma nova instância do Google Credential
        //    GoogleCredential credential;
        //    using (var stream = new FileStream("C:\\Desenvolvimentos\\EmDesenvolvimento\\IMPAR\\Impar\\bin\\Debug\\keys.json", FileMode.Open, FileAccess.Read))
        //    {
        //        credential = GoogleCredential.FromStream(stream)
        //            .CreateScoped(CalendarService.Scope.Calendar);
        //    }

        //    // Criando o serviço de calendário
        //    var service = new CalendarService(new BaseClientService.Initializer()
        //    {
        //        HttpClientInitializer = credential,
        //        ApplicationName = "GoogleAgenda"
        //    });

        //    // Definindo os parâmetros para a pesquisa do evento
        //    EventsResource.ListRequest request = service.Events.List("marcosmagalhaes86@gmail.com");
        //    request.TimeMin = DateTime.Today.AddMonths(-3);
        //    request.TimeMax = DateTime.Today.AddDays(1);
        //    request.ShowDeleted = false;
        //    request.SingleEvents = true;
        //    request.OrderBy = EventsResource.ListRequest.OrderByEnum.StartTime;

        //    // Executando a pesquisa
        //    Events events = request.Execute();

        //    // Processando os eventos encontrados
        //    List<Agendamento> agendamentos = new List<Agendamento>();
        //    if (events.Items != null && events.Items.Count > 0)
        //    {
        //        foreach (var eventItem in events.Items)
        //        {
        //            agendamentos.Add(new Agendamento()
        //            {
        //                EventId = eventItem.Id, // Adicione esta linha
        //                Inicio = eventItem.Start.DateTime.Value,
        //                Fim = eventItem.End.DateTime.Value
        //            });
        //        }
        //    }

        //    DateTime startDate = DateTime.Today.AddMonths(-3);
        //    DateTime endDate = DateTime.Today;
        //    DateTime morningStartTime = DtHoraInicioManha.Value;
        //    DateTime morningEndTime = DtHoraFimManha.Value;
        //    DateTime afternoonStartTime = DtHoraInicioTarde.Value;
        //    DateTime afternoonEndTime = DtHoraFimTarde.Value;

        //    List<DateTime> horariosDisponiveis = GetAvailableTimeSlots(startDate, endDate, morningStartTime, morningEndTime, afternoonStartTime, afternoonEndTime);

        //    foreach (Agendamento agendamento in agendamentos)
        //    {
        //        for (int i = horariosDisponiveis.Count - 1; i >= 0; i--)
        //        {
        //            DateTime horarioDisponivel = horariosDisponiveis[i];
        //            if (horarioDisponivel >= agendamento.Inicio && horarioDisponivel < agendamento.Fim)
        //            {
        //                horariosDisponiveis.RemoveAt(i);
        //            }
        //        }
        //    }

        //    DataTable dt = new DataTable();
        //    // Adicione colunas adicionais
        //    dt.Columns.Add("Data");
        //    dt.Columns.Add("Hora Início");
        //    dt.Columns.Add("Hora Fim");
        //    dt.Columns.Add("EventId");
        //    dt.Columns.Add("Título");
        //    dt.Columns.Add("Descrição");
        //    dt.Columns.Add("Código Cliente");

        //    foreach (Agendamento agendamento in agendamentos)
        //    {
        //        // Obtendo o evento correspondente ao agendamento
        //        var eventItem = events.Items.FirstOrDefault(ev => ev.Start.DateTime.Value == agendamento.Inicio && ev.End.DateTime.Value == agendamento.Fim);

        //        int? codigoCliente = GetClientIdByGoogleEventId(agendamento.EventId);

        //        if (eventItem != null)
        //        {
        //            string titulo = eventItem.Summary;
        //            string descricao = eventItem.Description;

        //            dt.Rows.Add(agendamento.Inicio.ToString("dd/MM/yyyy"), agendamento.Inicio.ToString("HH:mm"), agendamento.Fim.ToString("HH:mm"), agendamento.EventId, titulo, descricao, codigoCliente);
        //        }
        //        else
        //        {
        //            dt.Rows.Add(agendamento.Inicio.ToString("dd/MM/yyyy"), agendamento.Inicio.ToString("HH:mm"), agendamento.Fim.ToString("HH:mm"), agendamento.EventId, "", "", codigoCliente);
        //        }
        //    }

        //    // Verifique se a coluna "Agendar" já existe
        //    bool agendarColumnExists = false;
        //    foreach (DataGridViewColumn column in kryptonDataGridView2.Columns)
        //    {
        //        if (column.Name == "Agendar")
        //        {
        //            agendarColumnExists = true;
        //            break;
        //        }
        //    }

        //    if (!agendarColumnExists)
        //    {
        //        // Personalize a coluna "Agendar" para exibir botões
        //        DataGridViewButtonColumn agendarColumn = new DataGridViewButtonColumn();
        //        agendarColumn.HeaderText = "Agendar";
        //        agendarColumn.Name = "Agendar";
        //        agendarColumn.Text = "Agendar";
        //        agendarColumn.UseColumnTextForButtonValue = true;
        //        kryptonDataGridView2.Columns.Insert(0, agendarColumn); // Insira a coluna na primeira posição

        //        // Defina a largura da coluna "Agendar"
        //        kryptonDataGridView2.Columns[0].Width = 70;

        //        // Defina o estilo de célula para alinhar o texto ao centro
        //        DataGridViewCellStyle cellStyle = new DataGridViewCellStyle();
        //        cellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
        //        kryptonDataGridView2.Columns[0].DefaultCellStyle = cellStyle;


        //       // DataGridDesign.CustomizeKryptonDataGridView2(kryptonDataGridView2);

        //       // DataGridDesign.CustomizeKryptonDataGridView2(kryptonDataGridView2);
        //         kryptonDataGridView2.CellFormatting += kryptonDataGridView2_CellFormatting;
        //         kryptonDataGridView2.CellContentClick += kryptonDataGridView2_CellContentClick;
        //        DataGridDesign.CustomizeKryptonDataGridView2(kryptonDataGridView2);
        //    }

        //    // Atribua o DataTable atualizado ao DataSource do DataGridView
        //    kryptonDataGridView2.DataSource = dt;
        //}



      


        private void obtermarcacoesPendentesAtribuicao()
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
            request.TimeMin = DateTime.Today.AddMonths(-3);
            request.TimeMax = DateTime.Today.AddDays(1);

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

            DateTime startDate = DateTime.Today.AddMonths(-3);
            DateTime endDate = DateTime.Today;
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

            // Filtrar as linhas com "Código Cliente" vazio
            var rowsFiltradas = dt.AsEnumerable().Where(row => string.IsNullOrEmpty(row.Field<string>("Código Cliente")));
            DataTable dtFiltrado = rowsFiltradas.Any() ? rowsFiltradas.CopyToDataTable() : dt.Clone();

            kryptonDataGridView2.DataSource = dtFiltrado;

            kryptonDataGridView2.CellFormatting += kryptonDataGridView2_CellFormatting;
            kryptonDataGridView2.CellPainting += kryptonDataGridView2_CellPainting;

            DataGridViewButtonColumn agendarColumn = new DataGridViewButtonColumn();
            agendarColumn.HeaderText = "Agendar";

            kryptonDataGridView2.Columns.Add(agendarColumn);

            kryptonDataGridView2.CellContentClick += kryptonDataGridView2_CellContentClick;
        }


        private void kryptonButton13_Click(object sender, EventArgs e)
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

        private void btnFiltroEntreData_Click_1(object sender, EventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }


        private void monthView1_MouseDown(object sender, MouseEventArgs e)
        {
         

        }

        private void monthView1_MouseUp(object sender, MouseEventArgs e)
        {
         
        }

        private void monthView1_MouseMove(object sender, MouseEventArgs e)
        {
          
        }

        private void monthView1_MouseUp_1(object sender, MouseEventArgs e)
        {
            
            }
        }
    }

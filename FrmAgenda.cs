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
using ComponentFactory.Krypton.Toolkit;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Impar
{
    public partial class FrmAgenda : Form
    {

        private AgendamentoSms agendamentoSms;
        private Timer timer;

        public FrmAgenda()
        {
            InitializeComponent();

            agendamentoSms = new AgendamentoSms();

            timer = new Timer();
            timer.Interval = 10000; // 10 segundos
            timer.Tick += Timer_Tick;
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            _ = agendamentoSms.EnviarSmsAgendado();
        }


        private async void FrmAgenda_Load(object sender, EventArgs e)
        {
            //Envio SMMS Agendada
            timer.Start();


            DataGridDesign.CustomizeKryptonDataGridView5(kryptonDataGridView5);
            DataGridDesign.CustomizeKryptonDataGridView2(kryptonDataGridView2);
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
            
            kryptonDateTimePicker1.Value = DateTime.Today; 
            kryptonDateTimePicker2.Value = DateTime.Today;


            //    kryptonDataGridView2.RowTemplate.Height = 40; // Aumenta a altura das linhas para 40
            await obterTODASmarcacoes();
            filtrocalendario();


        }




        private void AbrirFormularioMarcacoes()
        {
            Marcacoes marcacoesForm = new Marcacoes();
            marcacoesForm.ChamadaDoFrmAgenda = true;
            marcacoesForm.Show();
        }



        private void kryptonDataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

            var senderGrid = (DataGridView)sender;

            if (senderGrid.Columns[e.ColumnIndex] is DataGridViewButtonColumn &&
                e.RowIndex >= 0 &&
                senderGrid.Columns[e.ColumnIndex].Name == "Agendar")
            {
                var cell = senderGrid.Rows[e.RowIndex].Cells[e.ColumnIndex];

                if (cell is DataGridViewButtonCell && cell.Value != null)
                {
                    string buttonText = cell.Value.ToString();

                    if (buttonText == "Agendar" || buttonText == "Editar")
                    {
                        string data = senderGrid.Rows[e.RowIndex].Cells["Data"].Value.ToString();
                        string horarioInicio = senderGrid.Rows[e.RowIndex].Cells["Hora Início"].Value.ToString();
                        string horarioFim = senderGrid.Rows[e.RowIndex].Cells["Hora Fim"].Value.ToString();
                        string idGoogle = senderGrid.Rows[e.RowIndex].Cells["EventId"].Value?.ToString();
                        string descricao = senderGrid.Rows[e.RowIndex].Cells["Descrição"].Value.ToString();
                        string titulo = senderGrid.Rows[e.RowIndex].Cells["Título"].Value.ToString();

                        // Converter as strings de horário para DateTime
                        DateTime horarioInicioDt;
                        DateTime horarioFimDt;

                        if (!DateTime.TryParseExact(horarioInicio, "HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out horarioInicioDt))
                        {
                            // O formato do horário de início é inválido
                            // Exibir mensagem de erro ou tratar de acordo com a necessidade
                            return;
                        }

                        if (!DateTime.TryParseExact(horarioFim, "HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out horarioFimDt))
                        {
                            // O formato do horário de fim é inválido
                            // Exibir mensagem de erro ou tratar de acordo com a necessidade
                            return;
                        }

                        // Verifica se o formulário já está aberto
                        if (Application.OpenForms.OfType<Marcacoes>().Any())
                        {
                            // Formulário já está aberto, não faz nada
                            return;
                        }

                        // Abrir o formulário Marcacoes e passar os valores
                        Marcacoes marcacoesForm = new Marcacoes();
                        marcacoesForm.ChamadaDoFrmAgenda = true; // Define a propriedade ChamadaDoFrmAgenda como true
                        marcacoesForm.DefinirValoresPadrao(); // Chama o método DefinirValoresPadrao para configurar os valores padrão
                        marcacoesForm.tbhorario.Value = DateTime.Parse(data);
                        marcacoesForm.tbhorainicio.Value = horarioInicioDt;
                        marcacoesForm.tbhorafim.Value = horarioFimDt;
                        marcacoesForm.tbidgoogle.Text = idGoogle;
                        marcacoesForm.tbdescricao.Text = descricao;
                        marcacoesForm.tbtitulogoogle.Text = titulo;

                        // Buscar os valores da tabela 'marcacoes' com base no idGoogle
                        string query = "SELECT Idcliente, Nome, telemovel, SmsEnviada, AgendarSms FROM marcacoes WHERE idGoogle = @idGoogle;";
                        MySqlCommand command = new MySqlCommand(query, connection);
                        command.Parameters.AddWithValue("@idGoogle", idGoogle);
                        MySqlDataAdapter adapter = new MySqlDataAdapter(command);
                        DataTable dataTable = new DataTable();

                        try
                        {
                            connection.Open();
                            adapter.Fill(dataTable);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Erro ao carregar os dados da tabela 'marcacoes': " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        finally
                        {
                            connection.Close();
                        }

                        if (dataTable.Rows.Count > 0)
                        {
                            DataRow row = dataTable.Rows[0];
                            marcacoesForm.tbcodcliente.Text = row["Idcliente"] != DBNull.Value ? row["Idcliente"].ToString() : "";
                            marcacoesForm.tbnomepaciente.Text = row["Nome"] != DBNull.Value ? row["Nome"].ToString() : "";
                            marcacoesForm.tbtlmpaciente.Text = row["telemovel"] != DBNull.Value ? row["telemovel"].ToString() : "";
                            marcacoesForm.kryptonCheckBox1.Checked = row["SmsEnviada"] != DBNull.Value && Convert.ToBoolean(row["SmsEnviada"]);
                            marcacoesForm.kryptonCheckBox2.Checked = row["AgendarSms"] != DBNull.Value && Convert.ToBoolean(row["AgendarSms"]);
                        }


                     
                        // Buscar os valores da tabela 'agendamentos' com base no idGoogle
                        string agendamentosQuery = "SELECT Sms1, Sms1Data, Sms1Hora, Sms1CorpoSMS, Sms2, Sms2Data, Sms2Hora, Sms2CorpoSMS, Sms3, Sms3Data, Sms3Hora, Sms3CorpoSMS, Sms1Enviada, Sms2Enviada, Sms3Enviada FROM agendamentos WHERE IDGoogle = @idGoogle;";
                        MySqlCommand agendamentosCommand = new MySqlCommand(agendamentosQuery, connection);
                        agendamentosCommand.Parameters.AddWithValue("@idGoogle", idGoogle);
                        MySqlDataAdapter agendamentosAdapter = new MySqlDataAdapter(agendamentosCommand);
                        DataTable agendamentosDataTable = new DataTable();

                        try
                        {
                            connection.Open();
                            agendamentosAdapter.Fill(agendamentosDataTable);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Erro ao carregar os dados da tabela 'agendamentos': " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        finally
                        {
                            connection.Close();
                        }

                        if (agendamentosDataTable.Rows.Count > 0)
                        {
                            DataRow agendamentosRow = agendamentosDataTable.Rows[0];
                            marcacoesForm.kryptonCheckBox3.Checked = agendamentosRow["Sms1"] != DBNull.Value && Convert.ToBoolean(agendamentosRow["Sms1"]);
                            marcacoesForm.kryptonDateTimePicker1.Value = agendamentosRow["Sms1Data"] != DBNull.Value ? Convert.ToDateTime(agendamentosRow["Sms1Data"]) : DateTime.MinValue;
                            marcacoesForm.kryptonDateTimePicker2.Value = agendamentosRow["Sms1Hora"] != DBNull.Value ? Convert.ToDateTime(agendamentosRow["Sms1Hora"]) : DateTime.MinValue;
                            marcacoesForm.kryptonTextBox2.Text = agendamentosRow["Sms1CorpoSMS"] != DBNull.Value ? agendamentosRow["Sms1CorpoSMS"].ToString() : "";
                            marcacoesForm.kryptonCheckBox4.Checked = agendamentosRow["Sms2"] != DBNull.Value && Convert.ToBoolean(agendamentosRow["Sms2"]);
                            marcacoesForm.kryptonDateTimePicker3.Value = agendamentosRow["Sms2Data"] != DBNull.Value ? Convert.ToDateTime(agendamentosRow["Sms2Data"]) : DateTime.MinValue;
                            marcacoesForm.kryptonDateTimePicker4.Value = agendamentosRow["Sms2Hora"] != DBNull.Value ? Convert.ToDateTime(agendamentosRow["Sms2Hora"]) : DateTime.MinValue;
                            marcacoesForm.kryptonTextBox3.Text = agendamentosRow["Sms2CorpoSMS"] != DBNull.Value ? agendamentosRow["Sms2CorpoSMS"].ToString() : "";
                            marcacoesForm.kryptonCheckBox5.Checked = agendamentosRow["Sms3"] != DBNull.Value && Convert.ToBoolean(agendamentosRow["Sms3"]);
                            marcacoesForm.kryptonDateTimePicker5.Value = agendamentosRow["Sms3Data"] != DBNull.Value ? Convert.ToDateTime(agendamentosRow["Sms3Data"]) : DateTime.MinValue;
                            marcacoesForm.kryptonDateTimePicker6.Value = agendamentosRow["Sms3Hora"] != DBNull.Value ? Convert.ToDateTime(agendamentosRow["Sms3Hora"]) : DateTime.MinValue;
                            marcacoesForm.kryptonTextBox5.Text = agendamentosRow["Sms3CorpoSMS"] != DBNull.Value ? agendamentosRow["Sms3CorpoSMS"].ToString() : "";
                            marcacoesForm.kryptonCheckBox8.Checked = agendamentosRow["Sms1Enviada"] != DBNull.Value && Convert.ToBoolean(agendamentosRow["Sms1Enviada"]);
                            marcacoesForm.kryptonCheckBox7.Checked = agendamentosRow["Sms2Enviada"] != DBNull.Value && Convert.ToBoolean(agendamentosRow["Sms2Enviada"]);
                            marcacoesForm.kryptonCheckBox6.Checked = agendamentosRow["Sms3Enviada"] != DBNull.Value && Convert.ToBoolean(agendamentosRow["Sms3Enviada"]);
                        }

                        marcacoesForm.Show();
                    }




                    //var senderGrid = (DataGridView)sender;

                    //if (senderGrid.Columns[e.ColumnIndex] is DataGridViewButtonColumn &&
                    //    e.RowIndex >= 0 &&
                    //    senderGrid.Columns[e.ColumnIndex].Name == "Agendar")
                    //{
                    //    var cell = senderGrid.Rows[e.RowIndex].Cells[e.ColumnIndex];

                    //    if (cell is DataGridViewButtonCell && cell.Value != null)
                    //    {
                    //        string buttonText = cell.Value.ToString();

                    //        if (buttonText == "Agendar" || buttonText == "Editar")
                    //        {
                    //            string data = senderGrid.Rows[e.RowIndex].Cells["Data"].Value.ToString();
                    //            string horarioInicio = senderGrid.Rows[e.RowIndex].Cells["Hora Início"].Value.ToString();
                    //            string horarioFim = senderGrid.Rows[e.RowIndex].Cells["Hora Fim"].Value.ToString();
                    //            string idGoogle = senderGrid.Rows[e.RowIndex].Cells["EventId"].Value?.ToString();
                    //            string descricao = senderGrid.Rows[e.RowIndex].Cells["Descrição"].Value.ToString();
                    //            string titulo = senderGrid.Rows[e.RowIndex].Cells["Título"].Value.ToString();

                    //            // Converter as strings de horário para DateTime
                    //            DateTime horarioInicioDt;
                    //            DateTime horarioFimDt;

                    //            if (!DateTime.TryParseExact(horarioInicio, "HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out horarioInicioDt))
                    //            {
                    //                // O formato do horário de início é inválido
                    //                // Exibir mensagem de erro ou tratar de acordo com a necessidade
                    //                return;
                    //            }

                    //            if (!DateTime.TryParseExact(horarioFim, "HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out horarioFimDt))
                    //            {
                    //                // O formato do horário de fim é inválido
                    //                // Exibir mensagem de erro ou tratar de acordo com a necessidade
                    //                return;
                    //            }

                    //            // Verifica se o formulário já está aberto
                    //            if (Application.OpenForms.OfType<Marcacoes>().Any())
                    //            {
                    //                // Formulário já está aberto, não faz nada
                    //                return;
                    //            }

                    //            // Abrir o formulário Marcacoes e passar os valores
                    //            Marcacoes marcacoesForm = new Marcacoes();
                    //            marcacoesForm.ChamadaDoFrmAgenda = true; // Define a propriedade ChamadaDoFrmAgenda como true
                    //            marcacoesForm.tbhorario.Value = DateTime.Parse(data);
                    //            marcacoesForm.tbhorainicio.Value = horarioInicioDt;
                    //            marcacoesForm.tbhorafim.Value = horarioFimDt;
                    //            marcacoesForm.tbidgoogle.Text = idGoogle;
                    //            marcacoesForm.tbdescricao.Text = descricao;
                    //            marcacoesForm.tbtitulogoogle.Text = titulo;

                    //            // Buscar os valores da tabela 'marcacoes' com base no idGoogle
                    //            string query = "SELECT Idcliente, Nome, telemovel, SmsEnviada, AgendarSms FROM marcacoes WHERE idGoogle = @idGoogle;";
                    //            MySqlCommand command = new MySqlCommand(query, connection);
                    //            command.Parameters.AddWithValue("@idGoogle", idGoogle);
                    //            MySqlDataAdapter adapter = new MySqlDataAdapter(command);
                    //            DataTable dataTable = new DataTable();

                    //            try
                    //            {
                    //                connection.Open();
                    //                adapter.Fill(dataTable);
                    //            }
                    //            catch (Exception ex)
                    //            {
                    //                MessageBox.Show("Erro ao carregar os dados da tabela 'marcacoes': " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    //                return;
                    //            }
                    //            finally
                    //            {
                    //                connection.Close();
                    //            }

                    //            if (dataTable.Rows.Count > 0)
                    //            {
                    //                DataRow row = dataTable.Rows[0];
                    //                marcacoesForm.tbcodcliente.Text = row["Idcliente"] != DBNull.Value ? row["Idcliente"].ToString() : "";
                    //                marcacoesForm.tbnomepaciente.Text = row["Nome"] != DBNull.Value ? row["Nome"].ToString() : "";
                    //                marcacoesForm.tbtlmpaciente.Text = row["telemovel"] != DBNull.Value ? row["telemovel"].ToString() : "";
                    //                marcacoesForm.kryptonCheckBox1.Checked = row["SmsEnviada"] != DBNull.Value && Convert.ToBoolean(row["SmsEnviada"]);
                    //                marcacoesForm.kryptonCheckBox2.Checked = row["AgendarSms"] != DBNull.Value && Convert.ToBoolean(row["AgendarSms"]);
                    //            }

                    //            // Buscar os valores da tabela 'agendamentos' com base no idGoogle
                    //            string agendamentosQuery = "SELECT Sms1, Sms1Data, Sms1Hora, Sms1CorpoSMS, Sms2, Sms2Data, Sms2Hora, Sms2CorpoSMS, Sms3, Sms3Data, Sms3Hora, Sms3CorpoSMS, Sms1Enviada, Sms2Enviada, Sms3Enviada FROM agendamentos WHERE IDGoogle = @idGoogle;";
                    //            MySqlCommand agendamentosCommand = new MySqlCommand(agendamentosQuery, connection);
                    //            agendamentosCommand.Parameters.AddWithValue("@idGoogle", idGoogle);
                    //            MySqlDataAdapter agendamentosAdapter = new MySqlDataAdapter(agendamentosCommand);
                    //            DataTable agendamentosDataTable = new DataTable();

                    //            try
                    //            {
                    //                connection.Open();
                    //                agendamentosAdapter.Fill(agendamentosDataTable);
                    //            }
                    //            catch (Exception ex)
                    //            {
                    //                MessageBox.Show("Erro ao carregar os dados da tabela 'agendamentos': " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    //                return;
                    //            }
                    //            finally
                    //            {
                    //                connection.Close();
                    //            }

                    //            if (agendamentosDataTable.Rows.Count > 0)
                    //            {
                    //                DataRow agendamentosRow = agendamentosDataTable.Rows[0];
                    //                marcacoesForm.kryptonCheckBox3.Checked = agendamentosRow["Sms1"] != DBNull.Value && Convert.ToBoolean(agendamentosRow["Sms1"]);
                    //                marcacoesForm.kryptonDateTimePicker1.Value = agendamentosRow["Sms1Data"] != DBNull.Value ? Convert.ToDateTime(agendamentosRow["Sms1Data"]) : DateTime.MinValue;
                    //                marcacoesForm.kryptonDateTimePicker2.Value = agendamentosRow["Sms1Hora"] != DBNull.Value ? Convert.ToDateTime(agendamentosRow["Sms1Hora"]) : DateTime.MinValue;
                    //                marcacoesForm.kryptonTextBox2.Text = agendamentosRow["Sms1CorpoSMS"] != DBNull.Value ? agendamentosRow["Sms1CorpoSMS"].ToString() : "";
                    //                marcacoesForm.kryptonCheckBox4.Checked = agendamentosRow["Sms2"] != DBNull.Value && Convert.ToBoolean(agendamentosRow["Sms2"]);
                    //                marcacoesForm.kryptonDateTimePicker3.Value = agendamentosRow["Sms2Data"] != DBNull.Value ? Convert.ToDateTime(agendamentosRow["Sms2Data"]) : DateTime.MinValue;
                    //                marcacoesForm.kryptonDateTimePicker4.Value = agendamentosRow["Sms2Hora"] != DBNull.Value ? Convert.ToDateTime(agendamentosRow["Sms2Hora"]) : DateTime.MinValue;
                    //                marcacoesForm.kryptonTextBox3.Text = agendamentosRow["Sms2CorpoSMS"] != DBNull.Value ? agendamentosRow["Sms2CorpoSMS"].ToString() : "";
                    //                marcacoesForm.kryptonCheckBox5.Checked = agendamentosRow["Sms3"] != DBNull.Value && Convert.ToBoolean(agendamentosRow["Sms3"]);
                    //                marcacoesForm.kryptonDateTimePicker5.Value = agendamentosRow["Sms3Data"] != DBNull.Value ? Convert.ToDateTime(agendamentosRow["Sms3Data"]) : DateTime.MinValue;
                    //                marcacoesForm.kryptonDateTimePicker6.Value = agendamentosRow["Sms3Hora"] != DBNull.Value ? Convert.ToDateTime(agendamentosRow["Sms3Hora"]) : DateTime.MinValue;
                    //                marcacoesForm.kryptonTextBox5.Text = agendamentosRow["Sms3CorpoSMS"] != DBNull.Value ? agendamentosRow["Sms3CorpoSMS"].ToString() : "";
                    //                marcacoesForm.kryptonCheckBox8.Checked = agendamentosRow["Sms1Enviada"] != DBNull.Value && Convert.ToBoolean(agendamentosRow["Sms1Enviada"]);
                    //                marcacoesForm.kryptonCheckBox7.Checked = agendamentosRow["Sms2Enviada"] != DBNull.Value && Convert.ToBoolean(agendamentosRow["Sms2Enviada"]);
                    //                marcacoesForm.kryptonCheckBox6.Checked = agendamentosRow["Sms3Enviada"] != DBNull.Value && Convert.ToBoolean(agendamentosRow["Sms3Enviada"]);
                    //            }

                    //            marcacoesForm.Show();
                    //        }
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
        //   MySqlConnection connection = new MySqlConnection(@"server=localhost;database=ContabSysDB;port=3308;userid=root;password=xd");
        MySqlConnection connection = new MySqlConnection(@"server=" + Properties.Settings.Default.server + ";database=" + Properties.Settings.Default.basedados + ";port=" + Properties.Settings.Default.porta + ";userid=" + Properties.Settings.Default.username + ";password=" + Properties.Settings.Default.password);

        MySqlCommand command;

        private void kryptonDataGridView5_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            var senderGrid = (DataGridView)sender;

            if (senderGrid.Columns[e.ColumnIndex] is DataGridViewButtonColumn &&
                e.RowIndex >= 0 &&
                senderGrid.Columns[e.ColumnIndex].Name == "Ação")
            {
                var cell = kryptonDataGridView5.Rows[e.RowIndex].Cells[e.ColumnIndex];

                if (cell is DataGridViewButtonCell && cell.Value != null)
                {
                    string buttonText = cell.Value.ToString();

                    if (buttonText == "Marcar")
                    {
                        string data = kryptonDataGridView5.Rows[e.RowIndex].Cells["Data"].Value?.ToString();
                        string horario = kryptonDataGridView5.Rows[e.RowIndex].Cells["Horário"].Value?.ToString();
                        DateTime horarioInicio = DateTime.ParseExact(horario, "HH:mm", CultureInfo.InvariantCulture);
                        DateTime horarioFim = horarioInicio.AddMinutes(30);
                        string ocupadoAte = horarioFim.ToString("HH:mm");

                        // Verifica se o formulário já está aberto
                        if (Application.OpenForms.OfType<Marcacoes>().Any())
                        {
                            // Formulário já está aberto, não faz nada
                            return;
                        }

                        // Abrir o formulário Marcacoes e passar os valores
                        Marcacoes marcacoesForm = new Marcacoes();
                        marcacoesForm.tbhorario.Value = DateTime.Parse(data);
                        marcacoesForm.tbhorainicio.Value = horarioInicio;
                        marcacoesForm.tbhorafim.Value = horarioFim;

                        // Definir a propriedade ChamadaDoFrmAgenda como true
                        marcacoesForm.ChamadaDoFrmAgenda = true;

                        // Buscar os valores da tabela 'marcacoes' com base no tbidgoogle.Text
                        string idGoogle = kryptonDataGridView5.Rows[e.RowIndex].Cells["EventId"].Value?.ToString();
                        string query = "SELECT Idcliente, Nome, telemovel, SmsEnviada, AgendarSms FROM marcacoes WHERE idGoogle = @idGoogle;";
                        MySqlCommand command = new MySqlCommand(query, connection);
                        command.Parameters.AddWithValue("@idGoogle", idGoogle);
                        MySqlDataAdapter adapter = new MySqlDataAdapter(command);
                        DataTable dataTable = new DataTable();

                        try
                        {
                            connection.Open();
                            adapter.Fill(dataTable);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Erro ao carregar os dados da tabela 'marcacoes': " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        finally
                        {
                            connection.Close();
                        }

                        if (dataTable.Rows.Count > 0)
                        {
                            DataRow row = dataTable.Rows[0];
                            marcacoesForm.tbcodcliente.Text = row["Idcliente"].ToString();
                            marcacoesForm.tbnomepaciente.Text = row["Nome"].ToString();
                            marcacoesForm.tbtlmpaciente.Text = row["telemovel"].ToString();
                            marcacoesForm.kryptonCheckBox1.Checked = Convert.ToBoolean(row["SmsEnviada"]);
                            marcacoesForm.kryptonCheckBox2.Checked = Convert.ToBoolean(row["AgendarSms"]);
                        }

                        // Buscar os valores da tabela 'agendamentos' com base no tbidgoogle.Text
                        string agendamentosQuery = "SELECT Sms1, Sms1Data, Sms1Hora, Sms1CorpoSMS, Sms2, Sms2Data, Sms2Hora, Sms2CorpoSMS, Sms3, Sms3Data, Sms3Hora, Sms3CorpoSMS, Sms1Enviada, Sms2Enviada, Sms3Enviada FROM agendamentos WHERE IDGoogle = @idGoogle;";
                        MySqlCommand agendamentosCommand = new MySqlCommand(agendamentosQuery, connection);
                        agendamentosCommand.Parameters.AddWithValue("@idGoogle", idGoogle);
                        MySqlDataAdapter agendamentosAdapter = new MySqlDataAdapter(agendamentosCommand);
                        DataTable agendamentosDataTable = new DataTable();

                        try
                        {
                            connection.Open();
                            agendamentosAdapter.Fill(agendamentosDataTable);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Erro ao carregar os dados da tabela 'agendamentos': " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        finally
                        {
                            connection.Close();
                        }

                        if (agendamentosDataTable.Rows.Count > 0)
                        {
                            DataRow agendamentosRow = agendamentosDataTable.Rows[0];
                            marcacoesForm.kryptonCheckBox3.Checked = Convert.ToBoolean(agendamentosRow["Sms1"]);
                            marcacoesForm.kryptonDateTimePicker1.Value = Convert.ToDateTime(agendamentosRow["Sms1Data"]);
                            marcacoesForm.kryptonDateTimePicker2.Value = Convert.ToDateTime(agendamentosRow["Sms1Hora"]);
                            marcacoesForm.kryptonTextBox2.Text = agendamentosRow["Sms1CorpoSMS"].ToString();
                            marcacoesForm.kryptonCheckBox4.Checked = Convert.ToBoolean(agendamentosRow["Sms2"]);
                            marcacoesForm.kryptonDateTimePicker3.Value = Convert.ToDateTime(agendamentosRow["Sms2Data"]);
                            marcacoesForm.kryptonDateTimePicker4.Value = Convert.ToDateTime(agendamentosRow["Sms2Hora"]);
                            marcacoesForm.kryptonTextBox3.Text = agendamentosRow["Sms2CorpoSMS"].ToString();
                            marcacoesForm.kryptonCheckBox5.Checked = Convert.ToBoolean(agendamentosRow["Sms3"]);
                            marcacoesForm.kryptonDateTimePicker5.Value = Convert.ToDateTime(agendamentosRow["Sms3Data"]);
                            marcacoesForm.kryptonDateTimePicker6.Value = Convert.ToDateTime(agendamentosRow["Sms3Hora"]);
                            marcacoesForm.kryptonTextBox5.Text = agendamentosRow["Sms3CorpoSMS"].ToString();
                            marcacoesForm.kryptonCheckBox8.Checked = Convert.ToBoolean(agendamentosRow["Sms1Enviada"]);
                            marcacoesForm.kryptonCheckBox7.Checked = Convert.ToBoolean(agendamentosRow["Sms2Enviada"]);
                            marcacoesForm.kryptonCheckBox6.Checked = Convert.ToBoolean(agendamentosRow["Sms3Enviada"]);
                        }

                        marcacoesForm.DefinirValoresPadrao(); // Chamada do método DefinirValoresPadrao()

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

                        // Verifica se o formulário já está aberto
                        if (Application.OpenForms.OfType<Marcacoes>().Any())
                        {
                            // Formulário já está aberto, não faz nada
                            return;
                        }

                        // Abrir o formulário Marcacoes e passar os valores
                        Marcacoes marcacoesForm = new Marcacoes();
                        marcacoesForm.tbhorario.Value = DateTime.Parse(data);
                        marcacoesForm.tbhorainicio.Value = DateTime.ParseExact(horario, "HH:mm", CultureInfo.InvariantCulture);
                        marcacoesForm.tbhorafim.Value = DateTime.ParseExact(ocupadoAte, "HH:mm", CultureInfo.InvariantCulture);
                        marcacoesForm.tbtitulogoogle.Text = titulo;
                        marcacoesForm.tbdescricao.Text = descricao;
                        marcacoesForm.tbidgoogle.Text = eventId;

                        // Buscar os valores da tabela 'marcacoes' com base no tbidgoogle.Text
                        string idGoogle = kryptonDataGridView5.Rows[e.RowIndex].Cells["EventId"].Value?.ToString();
                        string query = "SELECT Idcliente, Nome, telemovel, SmsEnviada, AgendarSms FROM marcacoes WHERE Idcliente = @idGoogle;";
                        MySqlCommand command = new MySqlCommand(query, connection);
                        command.Parameters.AddWithValue("@idGoogle", idGoogle);
                        MySqlDataAdapter adapter = new MySqlDataAdapter(command);
                        DataTable dataTable = new DataTable();

                        try
                        {
                            connection.Open();
                            adapter.Fill(dataTable);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Erro ao carregar os dados da tabela 'marcacoes': " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        finally
                        {
                            connection.Close();
                        }

                        if (dataTable.Rows.Count > 0)
                        {
                            DataRow row = dataTable.Rows[0];
                            marcacoesForm.tbcodcliente.Text = row["Idcliente"].ToString();
                            marcacoesForm.tbnomepaciente.Text = row["Nome"].ToString();
                            marcacoesForm.tbtlmpaciente.Text = row["telemovel"].ToString();
                            marcacoesForm.kryptonCheckBox1.Checked = Convert.ToBoolean(row["SmsEnviada"]);
                            marcacoesForm.kryptonCheckBox2.Checked = Convert.ToBoolean(row["AgendarSms"]);
                        }

                        // Buscar os valores da tabela 'agendamentos' com base no tbidgoogle.Text
                        string agendamentosQuery = "SELECT Sms1, Sms1Data, Sms1Hora, Sms1CorpoSMS, Sms2, Sms2Data, Sms2Hora, Sms2CorpoSMS, Sms3, Sms3Data, Sms3Hora, Sms3CorpoSMS, Sms1Enviada, Sms2Enviada, Sms3Enviada FROM agendamentos WHERE IDGoogle = @idGoogle;";
                        MySqlCommand agendamentosCommand = new MySqlCommand(agendamentosQuery, connection);
                        agendamentosCommand.Parameters.AddWithValue("@idGoogle", idGoogle);
                        MySqlDataAdapter agendamentosAdapter = new MySqlDataAdapter(agendamentosCommand);
                        DataTable agendamentosDataTable = new DataTable();

                        try
                        {
                            connection.Open();
                            agendamentosAdapter.Fill(agendamentosDataTable);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Erro ao carregar os dados da tabela 'agendamentos': " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        finally
                        {
                            connection.Close();
                        }

                        if (agendamentosDataTable.Rows.Count > 0)
                        {
                            DataRow agendamentosRow = agendamentosDataTable.Rows[0];
                            marcacoesForm.kryptonCheckBox3.Checked = Convert.ToBoolean(agendamentosRow["Sms1"]);
                            marcacoesForm.kryptonDateTimePicker1.Value = Convert.ToDateTime(agendamentosRow["Sms1Data"]);
                            marcacoesForm.kryptonDateTimePicker2.Value = Convert.ToDateTime(agendamentosRow["Sms1Hora"]);
                            marcacoesForm.kryptonTextBox2.Text = agendamentosRow["Sms1CorpoSMS"].ToString();
                            marcacoesForm.kryptonCheckBox4.Checked = Convert.ToBoolean(agendamentosRow["Sms2"]);
                            marcacoesForm.kryptonDateTimePicker3.Value = Convert.ToDateTime(agendamentosRow["Sms2Data"]);
                            marcacoesForm.kryptonDateTimePicker4.Value = Convert.ToDateTime(agendamentosRow["Sms2Hora"]);
                            marcacoesForm.kryptonTextBox3.Text = agendamentosRow["Sms2CorpoSMS"].ToString();
                            marcacoesForm.kryptonCheckBox5.Checked = Convert.ToBoolean(agendamentosRow["Sms3"]);
                            marcacoesForm.kryptonDateTimePicker5.Value = Convert.ToDateTime(agendamentosRow["Sms3Data"]);
                            marcacoesForm.kryptonDateTimePicker6.Value = Convert.ToDateTime(agendamentosRow["Sms3Hora"]);
                            marcacoesForm.kryptonTextBox5.Text = agendamentosRow["Sms3CorpoSMS"].ToString();
                            marcacoesForm.kryptonCheckBox8.Checked = Convert.ToBoolean(agendamentosRow["Sms1Enviada"]);
                            marcacoesForm.kryptonCheckBox7.Checked = Convert.ToBoolean(agendamentosRow["Sms2Enviada"]);
                            marcacoesForm.kryptonCheckBox6.Checked = Convert.ToBoolean(agendamentosRow["Sms3Enviada"]);
                        }

                        marcacoesForm.ChamadaDoFrmAgenda = false; // Definir a propriedade ChamadaDoFrmAgenda como false
                        marcacoesForm.Show();
                    }



                    //var senderGrid = (DataGridView)sender;

                    //if (senderGrid.Columns[e.ColumnIndex] is DataGridViewButtonColumn &&
                    //    e.RowIndex >= 0 &&
                    //    senderGrid.Columns[e.ColumnIndex].Name == "Ação")
                    //{
                    //    var cell = kryptonDataGridView5.Rows[e.RowIndex].Cells[e.ColumnIndex];

                    //    if (cell is DataGridViewButtonCell && cell.Value != null)
                    //    {
                    //        string buttonText = cell.Value.ToString();

                    //        if (buttonText == "Marcar")
                    //        {
                    //            string data = kryptonDataGridView5.Rows[e.RowIndex].Cells["Data"].Value?.ToString();
                    //            string horario = kryptonDataGridView5.Rows[e.RowIndex].Cells["Horário"].Value?.ToString();
                    //            DateTime horarioInicio = DateTime.ParseExact(horario, "HH:mm", CultureInfo.InvariantCulture);
                    //            DateTime horarioFim = horarioInicio.AddMinutes(30);
                    //            string ocupadoAte = horarioFim.ToString("HH:mm");

                    //            // Verifica se o formulário já está aberto
                    //            if (Application.OpenForms.OfType<Marcacoes>().Any())
                    //            {
                    //                // Formulário já está aberto, não faz nada
                    //                return;
                    //            }

                    //            // Abrir o formulário Marcacoes e passar os valores
                    //            Marcacoes marcacoesForm = new Marcacoes();
                    //            marcacoesForm.tbhorario.Value = DateTime.Parse(data);
                    //            marcacoesForm.tbhorainicio.Value = horarioInicio;
                    //            marcacoesForm.tbhorafim.Value = horarioFim;

                    //            // Buscar os valores da tabela 'marcacoes' com base no tbidgoogle.Text
                    //            string idGoogle = kryptonDataGridView5.Rows[e.RowIndex].Cells["EventId"].Value?.ToString();
                    //            string query = "SELECT Idcliente, Nome, telemovel, SmsEnviada, AgendarSms FROM marcacoes WHERE idGoogle = @idGoogle;";
                    //            MySqlCommand command = new MySqlCommand(query, connection);
                    //            command.Parameters.AddWithValue("@idGoogle", idGoogle);
                    //            MySqlDataAdapter adapter = new MySqlDataAdapter(command);
                    //            DataTable dataTable = new DataTable();

                    //            try
                    //            {
                    //                connection.Open();
                    //                adapter.Fill(dataTable);
                    //            }
                    //            catch (Exception ex)
                    //            {
                    //                MessageBox.Show("Erro ao carregar os dados da tabela 'marcacoes': " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    //                return;
                    //            }
                    //            finally
                    //            {
                    //                connection.Close();
                    //            }

                    //            if (dataTable.Rows.Count > 0)
                    //            {
                    //                DataRow row = dataTable.Rows[0];
                    //                marcacoesForm.tbcodcliente.Text = row["Idcliente"].ToString();
                    //                marcacoesForm.tbnomepaciente.Text = row["Nome"].ToString();
                    //                marcacoesForm.tbtlmpaciente.Text = row["telemovel"].ToString();
                    //                marcacoesForm.kryptonCheckBox1.Checked = Convert.ToBoolean(row["SmsEnviada"]);
                    //                marcacoesForm.kryptonCheckBox2.Checked = Convert.ToBoolean(row["AgendarSms"]);
                    //            }

                    //            // Buscar os valores da tabela 'agendamentos' com base no tbidgoogle.Text
                    //            string agendamentosQuery = "SELECT Sms1, Sms1Data, Sms1Hora, Sms1CorpoSMS, Sms2, Sms2Data, Sms2Hora, Sms2CorpoSMS, Sms3, Sms3Data, Sms3Hora, Sms3CorpoSMS, Sms1Enviada, Sms2Enviada, Sms3Enviada FROM agendamentos WHERE IDGoogle = @idGoogle;";
                    //            MySqlCommand agendamentosCommand = new MySqlCommand(agendamentosQuery, connection);
                    //            agendamentosCommand.Parameters.AddWithValue("@idGoogle", idGoogle);
                    //            MySqlDataAdapter agendamentosAdapter = new MySqlDataAdapter(agendamentosCommand);
                    //            DataTable agendamentosDataTable = new DataTable();

                    //            try
                    //            {
                    //                connection.Open();
                    //                agendamentosAdapter.Fill(agendamentosDataTable);
                    //            }
                    //            catch (Exception ex)
                    //            {
                    //                MessageBox.Show("Erro ao carregar os dados da tabela 'agendamentos': " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    //                return;
                    //            }
                    //            finally
                    //            {
                    //                connection.Close();
                    //            }

                    //            if (agendamentosDataTable.Rows.Count > 0)
                    //            {
                    //                DataRow agendamentosRow = agendamentosDataTable.Rows[0];
                    //                marcacoesForm.kryptonCheckBox3.Checked = Convert.ToBoolean(agendamentosRow["Sms1"]);
                    //                marcacoesForm.kryptonDateTimePicker1.Value = Convert.ToDateTime(agendamentosRow["Sms1Data"]);
                    //                marcacoesForm.kryptonDateTimePicker2.Value = Convert.ToDateTime(agendamentosRow["Sms1Hora"]);
                    //                marcacoesForm.kryptonTextBox2.Text = agendamentosRow["Sms1CorpoSMS"].ToString();
                    //                marcacoesForm.kryptonCheckBox4.Checked = Convert.ToBoolean(agendamentosRow["Sms2"]);
                    //                marcacoesForm.kryptonDateTimePicker3.Value = Convert.ToDateTime(agendamentosRow["Sms2Data"]);
                    //                marcacoesForm.kryptonDateTimePicker4.Value = Convert.ToDateTime(agendamentosRow["Sms2Hora"]);
                    //                marcacoesForm.kryptonTextBox3.Text = agendamentosRow["Sms2CorpoSMS"].ToString();
                    //                marcacoesForm.kryptonCheckBox5.Checked = Convert.ToBoolean(agendamentosRow["Sms3"]);
                    //                marcacoesForm.kryptonDateTimePicker5.Value = Convert.ToDateTime(agendamentosRow["Sms3Data"]);
                    //                marcacoesForm.kryptonDateTimePicker6.Value = Convert.ToDateTime(agendamentosRow["Sms3Hora"]);
                    //                marcacoesForm.kryptonTextBox5.Text = agendamentosRow["Sms3CorpoSMS"].ToString();
                    //                marcacoesForm.kryptonCheckBox8.Checked = Convert.ToBoolean(agendamentosRow["Sms1Enviada"]);
                    //                marcacoesForm.kryptonCheckBox7.Checked = Convert.ToBoolean(agendamentosRow["Sms2Enviada"]);
                    //                marcacoesForm.kryptonCheckBox6.Checked = Convert.ToBoolean(agendamentosRow["Sms3Enviada"]);
                    //            }

                    //            marcacoesForm.ChamadaDoFrmAgenda = true; // Definir a propriedade ChamadaDoFrmAgenda como true
                    //            marcacoesForm.Show();
                    //        }
                    //        else if (buttonText == "Editar")
                    //        {
                    //            string data = kryptonDataGridView5.Rows[e.RowIndex].Cells["Data"].Value?.ToString();
                    //            string horario = kryptonDataGridView5.Rows[e.RowIndex].Cells["Horário"].Value?.ToString();
                    //            string ocupadoAte = kryptonDataGridView5.Rows[e.RowIndex].Cells["Ocupado até"].Value?.ToString();
                    //            string titulo = kryptonDataGridView5.Rows[e.RowIndex].Cells["Título"].Value?.ToString();
                    //            string descricao = kryptonDataGridView5.Rows[e.RowIndex].Cells["Descrição"].Value?.ToString();
                    //            string eventId = kryptonDataGridView5.Rows[e.RowIndex].Cells["EventId"].Value?.ToString();

                    //            // Verifica se o formulário já está aberto
                    //            if (Application.OpenForms.OfType<Marcacoes>().Any())
                    //            {
                    //                // Formulário já está aberto, não faz nada
                    //                return;
                    //            }

                    //            // Abrir o formulário Marcacoes e passar os valores
                    //            Marcacoes marcacoesForm = new Marcacoes();
                    //            marcacoesForm.tbhorario.Value = DateTime.Parse(data);
                    //            marcacoesForm.tbhorainicio.Value = DateTime.ParseExact(horario, "HH:mm", CultureInfo.InvariantCulture);
                    //            marcacoesForm.tbhorafim.Value = DateTime.ParseExact(ocupadoAte, "HH:mm", CultureInfo.InvariantCulture);
                    //            marcacoesForm.tbtitulogoogle.Text = titulo;
                    //            marcacoesForm.tbdescricao.Text = descricao;
                    //            marcacoesForm.tbidgoogle.Text = eventId;

                    //            // Buscar os valores da tabela 'marcacoes' com base no tbidgoogle.Text
                    //            string idGoogle = kryptonDataGridView5.Rows[e.RowIndex].Cells["EventId"].Value?.ToString();
                    //            string query = "SELECT Idcliente, Nome, telemovel, SmsEnviada, AgendarSms FROM marcacoes WHERE Idcliente = @idGoogle;";
                    //            MySqlCommand command = new MySqlCommand(query, connection);
                    //            command.Parameters.AddWithValue("@idGoogle", idGoogle);
                    //            MySqlDataAdapter adapter = new MySqlDataAdapter(command);
                    //            DataTable dataTable = new DataTable();

                    //            try
                    //            {
                    //                connection.Open();
                    //                adapter.Fill(dataTable);
                    //            }
                    //            catch (Exception ex)
                    //            {
                    //                MessageBox.Show("Erro ao carregar os dados da tabela 'marcacoes': " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    //                return;
                    //            }
                    //            finally
                    //            {
                    //                connection.Close();
                    //            }

                    //            if (dataTable.Rows.Count > 0)
                    //            {
                    //                DataRow row = dataTable.Rows[0];
                    //                marcacoesForm.tbcodcliente.Text = row["Idcliente"].ToString();
                    //                marcacoesForm.tbnomepaciente.Text = row["Nome"].ToString();
                    //                marcacoesForm.tbtlmpaciente.Text = row["telemovel"].ToString();
                    //                marcacoesForm.kryptonCheckBox1.Checked = Convert.ToBoolean(row["SmsEnviada"]);
                    //                marcacoesForm.kryptonCheckBox2.Checked = Convert.ToBoolean(row["AgendarSms"]);
                    //            }

                    //            // Buscar os valores da tabela 'agendamentos' com base no tbidgoogle.Text
                    //            string agendamentosQuery = "SELECT Sms1, Sms1Data, Sms1Hora, Sms1CorpoSMS, Sms2, Sms2Data, Sms2Hora, Sms2CorpoSMS, Sms3, Sms3Data, Sms3Hora, Sms3CorpoSMS, Sms1Enviada, Sms2Enviada, Sms3Enviada FROM agendamentos WHERE IDGoogle = @idGoogle;";
                    //            MySqlCommand agendamentosCommand = new MySqlCommand(agendamentosQuery, connection);
                    //            agendamentosCommand.Parameters.AddWithValue("@idGoogle", idGoogle);
                    //            MySqlDataAdapter agendamentosAdapter = new MySqlDataAdapter(agendamentosCommand);
                    //            DataTable agendamentosDataTable = new DataTable();

                    //            try
                    //            {
                    //                connection.Open();
                    //                agendamentosAdapter.Fill(agendamentosDataTable);
                    //            }
                    //            catch (Exception ex)
                    //            {
                    //                MessageBox.Show("Erro ao carregar os dados da tabela 'agendamentos': " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    //                return;
                    //            }
                    //            finally
                    //            {
                    //                connection.Close();
                    //            }

                    //            if (agendamentosDataTable.Rows.Count > 0)
                    //            {
                    //                DataRow agendamentosRow = agendamentosDataTable.Rows[0];
                    //                marcacoesForm.kryptonCheckBox3.Checked = Convert.ToBoolean(agendamentosRow["Sms1"]);
                    //                marcacoesForm.kryptonDateTimePicker1.Value = Convert.ToDateTime(agendamentosRow["Sms1Data"]);
                    //                marcacoesForm.kryptonDateTimePicker2.Value = Convert.ToDateTime(agendamentosRow["Sms1Hora"]);
                    //                marcacoesForm.kryptonTextBox2.Text = agendamentosRow["Sms1CorpoSMS"].ToString();
                    //                marcacoesForm.kryptonCheckBox4.Checked = Convert.ToBoolean(agendamentosRow["Sms2"]);
                    //                marcacoesForm.kryptonDateTimePicker3.Value = Convert.ToDateTime(agendamentosRow["Sms2Data"]);
                    //                marcacoesForm.kryptonDateTimePicker4.Value = Convert.ToDateTime(agendamentosRow["Sms2Hora"]);
                    //                marcacoesForm.kryptonTextBox3.Text = agendamentosRow["Sms2CorpoSMS"].ToString();
                    //                marcacoesForm.kryptonCheckBox5.Checked = Convert.ToBoolean(agendamentosRow["Sms3"]);
                    //                marcacoesForm.kryptonDateTimePicker5.Value = Convert.ToDateTime(agendamentosRow["Sms3Data"]);
                    //                marcacoesForm.kryptonDateTimePicker6.Value = Convert.ToDateTime(agendamentosRow["Sms3Hora"]);
                    //                marcacoesForm.kryptonTextBox5.Text = agendamentosRow["Sms3CorpoSMS"].ToString();
                    //                marcacoesForm.kryptonCheckBox8.Checked = Convert.ToBoolean(agendamentosRow["Sms1Enviada"]);
                    //                marcacoesForm.kryptonCheckBox7.Checked = Convert.ToBoolean(agendamentosRow["Sms2Enviada"]);
                    //                marcacoesForm.kryptonCheckBox6.Checked = Convert.ToBoolean(agendamentosRow["Sms3Enviada"]);
                    //            }

                    //            marcacoesForm.ChamadaDoFrmAgenda = false; // Definir a propriedade ChamadaDoFrmAgenda como false
                    //            marcacoesForm.Show();
                    //        }



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
                    // Define a cor de fundo como amarelo para o botão "Editar"
                    kryptonDataGridView5.Rows[e.RowIndex].Cells[e.ColumnIndex].Style.BackColor = Color.Yellow;
                }
                else if (buttonText == "Marcar")
                {
                    // Define a cor de fundo como verde para o botão "Marcar"
                    kryptonDataGridView5.Rows[e.RowIndex].Cells[e.ColumnIndex].Style.BackColor = Color.Green;
                }
            }
        }


        private async void syncgoogle()
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
                    FiltrarPorStatus("Disponível");
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

          
        }





        private async void monthView1_SelectionChanged(object sender, EventArgs e)
        {
            syncgoogle();

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





        //private void CustomizeKryptonDataGridView2()
        //{
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
        //        // Personaliza a coluna "Agendar" para exibir botões
        //        DataGridViewButtonColumn agendarColumn = new DataGridViewButtonColumn();
        //        agendarColumn.HeaderText = "Agendar";
        //        agendarColumn.Name = "Agendar";
        //        agendarColumn.Text = "Agendar";
        //        agendarColumn.UseColumnTextForButtonValue = true;
        //        kryptonDataGridView2.Columns.Insert(0, agendarColumn); // Insere a coluna na primeira posição

        //        // Define a largura da coluna "Agendar"
        //        kryptonDataGridView2.Columns[0].Width = 70;

        //        // Define o estilo de célula para alinhar o texto ao centro
        //        DataGridViewCellStyle cellStyle = new DataGridViewCellStyle();
        //        cellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
        //        kryptonDataGridView2.Columns[0].DefaultCellStyle = cellStyle;
        //    }

        //    // Assina o evento RowPrePaint para ajustar a altura das linhas
        //    kryptonDataGridView2.RowPrePaint += KryptonDataGridView2_RowPrePaint;
        //}

        //private void KryptonDataGridView2_RowPrePaint(object sender, DataGridViewRowPrePaintEventArgs e)
        //{
        //    // Ajusta a altura da linha para 40
        //    kryptonDataGridView2.Rows[e.RowIndex].Height = 40;
        //}




        private void CustomizeKryptonDataGridView2(KryptonDataGridView dataGridView)
        {

            //bool agendarColumnExists = false;
            //foreach (DataGridViewColumn column in dataGridView.Columns)
            //{
            //    if (column.Name == "Agendar")
            //    {
            //        agendarColumnExists = true;
            //        break;
            //    }
            //}

            //if (!agendarColumnExists)
            //{
            //    DataGridViewButtonColumn agendarColumn = new DataGridViewButtonColumn();
            //    agendarColumn.HeaderText = "Agendar";
            //    agendarColumn.Name = "Agendar";
            //    dataGridView.Columns.Insert(0, agendarColumn);
            //    dataGridView.Columns[0].Width = 70;

            //    DataGridViewCellStyle cellStyle = new DataGridViewCellStyle();
            //    cellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            //    dataGridView.Columns[0].DefaultCellStyle = cellStyle;
            //}
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


        private async Task obterTODASmarcacoes()
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
                    Events events = await request.ExecuteAsync();

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

                    // Atribui o DataTable atualizado ao DataSource do DataGridView
                    kryptonDataGridView2.Invoke(new Action(() =>
                    {
                        kryptonDataGridView2.DataSource = dt;

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
                            DataGridViewButtonColumn agendarColumn = new DataGridViewButtonColumn();
                            agendarColumn.HeaderText = "Agendar";
                            agendarColumn.Name = "Agendar";
                            kryptonDataGridView2.Columns.Insert(0, agendarColumn);
                            kryptonDataGridView2.Columns[0].Width = 70;

                            DataGridViewCellStyle cellStyle = new DataGridViewCellStyle();
                            cellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                            kryptonDataGridView2.Columns[0].DefaultCellStyle = cellStyle;
                        }

                        CustomizeKryptonDataGridView2(kryptonDataGridView2);
                        kryptonDataGridView2.CellFormatting += kryptonDataGridView2_CellFormatting;
                        kryptonDataGridView2.CellContentClick += kryptonDataGridView2_CellContentClick;
                        DataGridDesign.CustomizeKryptonDataGridView2(kryptonDataGridView2);
                    }));

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
        
    
    //await Task.Run(async () =>
    //{
    //    try
    //    {
    //        // Cria uma instância da classe GoogleCalendarService
    //        GoogleCalendarService googleCalendarService = new GoogleCalendarService();
    //        bool connected = await googleCalendarService.Connect();

    //        if (connected)
    //        {
    //            // Define os parâmetros para a pesquisa do evento
    //            EventsResource.ListRequest request = googleCalendarService.Service.Events.List("marcosmagalhaes86@gmail.com");
    //            request.TimeMin = DateTime.Today.AddMonths(-3);
    //            request.TimeMax = DateTime.Today.AddDays(1);
    //            request.ShowDeleted = false;
    //            request.SingleEvents = true;
    //            request.OrderBy = EventsResource.ListRequest.OrderByEnum.StartTime;

    //            // Executa a pesquisa
    //            Events events = request.Execute();

    //            // Processa os eventos encontrados
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

    //            // Verifica se a coluna "Agendar" já existe
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
    //                // Personaliza a coluna "Agendar" para exibir botões
    //                DataGridViewButtonColumn agendarColumn = new DataGridViewButtonColumn();
    //                agendarColumn.HeaderText = "Agendar";
    //                agendarColumn.Name = "Agendar";
    //                agendarColumn.Text = "Agendar";
    //                agendarColumn.UseColumnTextForButtonValue = true;
    //                kryptonDataGridView2.Invoke(new Action(() => kryptonDataGridView2.Columns.Insert(0, agendarColumn))); // Insere a coluna na primeira posição

    //                // Define a largura da coluna "Agendar"
    //                kryptonDataGridView2.Invoke(new Action(() => kryptonDataGridView2.Columns[0].Width = 70));

    //                // Define o estilo de célula para alinhar o texto ao centro
    //                DataGridViewCellStyle cellStyle = new DataGridViewCellStyle();
    //                cellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
    //                kryptonDataGridView2.Invoke(new Action(() => kryptonDataGridView2.Columns[0].DefaultCellStyle = cellStyle));


    //            }

    //            // Atribui o DataTable atualizado ao DataSource do DataGridView usando o método Invoke para atualizar no thread da interface do usuário
    //            kryptonDataGridView2.Invoke(new Action(() => kryptonDataGridView2.DataSource = dt));

    //            googleCalendarService.Service.Dispose();



    //        }
    //        else
    //        {
    //            // Lidere com o caso de falha na conexão, se necessário
    //            MessageBox.Show("Falha na conexão com o Google Calendar.");
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        // Trate a exceção de alguma forma apropriada, por exemplo, mostrando uma mensagem de erro
    //        MessageBox.Show("Ocorreu um erro ao obter as marcações: " + ex.Message);
    //    }
    //});

}



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


        private async void kryptonButton6_Click(object sender, EventArgs e)
        {
            await obterTODASmarcacoes();

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


        private async void kryptonButton13_Click(object sender, EventArgs e)
        {

      
        }


        private async void filtrocalendario()
        {
            //DateTime selectedStartDate = e.Start.Date;
            //DateTime selectedEndDate = e.End.Date;
            DateTime selectedStartDate = DtDataInicio.Value.Date;
            DateTime selectedEndDate = DtDataFim.Value.Date;

            selectedEndDate = selectedEndDate.AddDays(1).AddSeconds(-1);

            GoogleCalendarService googleCalendarService = null;

            try
            {
                // Cria uma instância da classe GoogleCalendarService
                googleCalendarService = new GoogleCalendarService();
                bool connected = await googleCalendarService.Connect();

                if (connected)
                {
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
            }
        }



        private async void btnFiltroEntreData_Click_1(object sender, EventArgs e)
        {
            //DateTime selectedStartDate = e.Start.Date;
            //DateTime selectedEndDate = e.End.Date;
            DateTime selectedStartDate = DtDataInicio.Value.Date;
            DateTime selectedEndDate = DtDataFim.Value.Date;

            selectedEndDate = selectedEndDate.AddDays(1).AddSeconds(-1);

            GoogleCalendarService googleCalendarService = null;

            try
            {
                // Cria uma instância da classe GoogleCalendarService
                googleCalendarService = new GoogleCalendarService();
                bool connected = await googleCalendarService.Connect();

                if (connected)
                {
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
            }
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

        private void kryptonButton7_Click(object sender, EventArgs e)
        {
            if (kryptonDataGridView2.Columns.Contains("Código Cliente"))
            {
                // Filtra os registros mostrando apenas os que estão vazios na coluna "Código Cliente"
                ((DataTable)kryptonDataGridView2.DataSource).DefaultView.RowFilter = "ISNULL([Código Cliente],'') = ''";
            }
        }

        private async void kryptonButton1_Click(object sender, EventArgs e)
        {
            try
            {
                // Cria uma instância da classe GoogleCalendarService
                GoogleCalendarService googleCalendarService = new GoogleCalendarService();
                bool connected = await googleCalendarService.Connect();

                if (connected)
                {
                    DateTime startDate = kryptonDateTimePicker1.Value;
                    DateTime endDate = kryptonDateTimePicker2.Value;

                    // Define os parâmetros para a pesquisa do evento
                    EventsResource.ListRequest request = googleCalendarService.Service.Events.List("marcosmagalhaes86@gmail.com");
                    request.TimeMin = startDate;
                    request.TimeMax = endDate.AddDays(1).Date;
                    request.ShowDeleted = false;
                    request.SingleEvents = true;
                    request.OrderBy = EventsResource.ListRequest.OrderByEnum.StartTime;

                    // Executa a pesquisa
                    Events events = await request.ExecuteAsync();

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

                    // Atribui o DataTable atualizado ao DataSource do DataGridView
                    kryptonDataGridView2.Invoke(new Action(() =>
                    {
                        kryptonDataGridView2.DataSource = dt;

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
                            DataGridViewButtonColumn agendarColumn = new DataGridViewButtonColumn();
                            agendarColumn.HeaderText = "Agendar";
                            agendarColumn.Name = "Agendar";
                            kryptonDataGridView2.Columns.Insert(0, agendarColumn);
                            kryptonDataGridView2.Columns[0].Width = 70;

                            DataGridViewCellStyle cellStyle = new DataGridViewCellStyle();
                            cellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                            kryptonDataGridView2.Columns[0].DefaultCellStyle = cellStyle;
                        }

                        CustomizeKryptonDataGridView2(kryptonDataGridView2);
                        kryptonDataGridView2.CellFormatting += kryptonDataGridView2_CellFormatting;
                        kryptonDataGridView2.CellContentClick += kryptonDataGridView2_CellContentClick;
                        DataGridDesign.CustomizeKryptonDataGridView2(kryptonDataGridView2);
                    }));

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

        }

        private void kryptonButton12_Click(object sender, EventArgs e)
        {
            if (kryptonDataGridView2.Columns.Contains("Código Cliente"))
            {
                // Filtra os registros mostrando apenas os que têm dados na coluna "Código Cliente"
                ((DataTable)kryptonDataGridView2.DataSource).DefaultView.RowFilter = "ISNULL([Código Cliente],'') <> ''";
            }
        }

        private void kryptonButton2_Click(object sender, EventArgs e)
        {
            Marcacoes form = new Marcacoes();
            form.Show();
        }


       

        private void LerDadosTabelaAgendamentoSMS()
        {
            string agendamentosQuery = "SELECT ID, IDGoogle, Nome, Telemovel, Sms1, Sms1Data, Sms1Hora, Sms1CorpoSMS, Sms2, Sms2Data, Sms2Hora, Sms2CorpoSMS, Sms3, Sms3Data, Sms3Hora, Sms3CorpoSMS, Sms1Enviada, Sms2Enviada, Sms3Enviada FROM agendamentos";
            MySqlCommand agendamentosCommand = new MySqlCommand(agendamentosQuery, connection);
            MySqlDataAdapter agendamentosAdapter = new MySqlDataAdapter(agendamentosCommand);
            DataTable agendamentosDataTable = new DataTable();

            try
            {
                connection.Open();
                agendamentosAdapter.Fill(agendamentosDataTable);

                // Configurar as colunas como checkboxes
                kryptonDataGridView1.AutoGenerateColumns = false;

                // Adicionar as colunas manualmente
                kryptonDataGridView1.Columns.Clear();

                // Adicionar a coluna "Editar" na primeira posição
                DataGridViewButtonColumn editarColumn = new DataGridViewButtonColumn();
                editarColumn.Name = "Editar";
                editarColumn.HeaderText = "Editar";
                editarColumn.Text = "Editar";
                editarColumn.UseColumnTextForButtonValue = true;
                kryptonDataGridView1.Columns.Add(editarColumn);

                kryptonDataGridView1.Columns.Add(new DataGridViewTextBoxColumn
                {
                    DataPropertyName = "ID",
                    Name = "ID",
                    HeaderText = "ID"
                });

                kryptonDataGridView1.Columns.Add(new DataGridViewTextBoxColumn
                {
                    DataPropertyName = "IDGoogle",
                    Name = "IDGoogle",
                    HeaderText = "IDGoogle"
                });

                kryptonDataGridView1.Columns.Add(new DataGridViewTextBoxColumn
                {
                    DataPropertyName = "Nome",
                    Name = "Nome",
                    HeaderText = "Nome"
                });

                kryptonDataGridView1.Columns.Add(new DataGridViewTextBoxColumn
                {
                    DataPropertyName = "Telemovel",
                    Name = "Telemovel",
                    HeaderText = "Telemovel"
                });

                // Configurar as colunas de checkbox
                kryptonDataGridView1.Columns.Add(new DataGridViewCheckBoxColumn
                {
                    DataPropertyName = "Sms1",
                    Name = "Sms1",
                    HeaderText = "Sms1",
                    TrueValue = true,
                    FalseValue = false
                });

                kryptonDataGridView1.Columns.Add(new DataGridViewCheckBoxColumn
                {
                    DataPropertyName = "Sms2",
                    Name = "Sms2",
                    HeaderText = "Sms2",
                    TrueValue = true,
                    FalseValue = false
                });

                kryptonDataGridView1.Columns.Add(new DataGridViewCheckBoxColumn
                {
                    DataPropertyName = "Sms3",
                    Name = "Sms3",
                    HeaderText = "Sms3",
                    TrueValue = true,
                    FalseValue = false
                });

                kryptonDataGridView1.Columns.Add(new DataGridViewCheckBoxColumn
                {
                    DataPropertyName = "Sms1Enviada",
                    Name = "Sms1Enviada",
                    HeaderText = "Sms1 Enviada",
                    TrueValue = true,
                    FalseValue = false
                });

                kryptonDataGridView1.Columns.Add(new DataGridViewCheckBoxColumn
                {
                    DataPropertyName = "Sms2Enviada",
                    Name = "Sms2Enviada",
                    HeaderText = "Sms2 Enviada",
                    TrueValue = true,
                    FalseValue = false
                });

                kryptonDataGridView1.Columns.Add(new DataGridViewCheckBoxColumn
                {
                    DataPropertyName = "Sms3Enviada",
                    Name = "Sms3Enviada",
                    HeaderText = "Sms3 Enviada",
                    TrueValue = true,
                    FalseValue = false
                });

                // Preencher os dados do DataTable no kryptonDataGridView1
                kryptonDataGridView1.DataSource = agendamentosDataTable;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao ler os dados da tabela 'agendamentos': " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                connection.Close();
            }
           
        }

        private void FiltrarSMSNaoEnviadas()
        {
            // Obter o DataTable associado ao DataGridView
            DataTable dataTable = (DataTable)kryptonDataGridView1.DataSource;

            // Criar um novo DataTable para armazenar as linhas filtradas
            DataTable dataTableFiltrado = dataTable.Clone();

            // Filtrar as linhas para exibir apenas as SMS não enviadas
            foreach (DataRow row in dataTable.Rows)
            {
                bool sms1Checked = Convert.ToBoolean(row["Sms1"]);
                bool sms1Enviada = Convert.ToBoolean(row["Sms1Enviada"]);
                bool sms2Checked = Convert.ToBoolean(row["Sms2"]);
                bool sms2Enviada = Convert.ToBoolean(row["Sms2Enviada"]);
                bool sms3Checked = Convert.ToBoolean(row["Sms3"]);
                bool sms3Enviada = Convert.ToBoolean(row["Sms3Enviada"]);

                // Verificar as condições de filtragem
                if ((sms1Checked && !sms1Enviada) ||
                    (sms2Checked && !sms2Enviada && (!sms1Checked || (sms1Checked && sms1Enviada))) ||
                    (sms3Checked && !sms3Enviada && (!sms1Checked || (sms1Checked && sms1Enviada)) && (!sms2Checked || (sms2Checked && sms2Enviada))))
                {
                    // Adicionar a linha filtrada ao novo DataTable
                    dataTableFiltrado.Rows.Add(row.ItemArray);
                }
            }

            // Atribuir o novo DataTable como DataSource do DataGridView
            kryptonDataGridView1.DataSource = dataTableFiltrado;

        }





        private void kryptonButton8_Click(object sender, EventArgs e)
        {
            LerDadosTabelaAgendamentoSMS();
        }

        private void kryptonButton9_Click(object sender, EventArgs e)
        {
         
            FiltrarSMSNaoEnviadas();
        }
    }
    }

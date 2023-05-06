using System;
using System.IO;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using Calendar.NET;
using Outlook = Microsoft.Office.Interop.Outlook;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using System.Net;
using System.Drawing;
using System.Collections.Generic;
using CefSharp.WinForms;
using CefSharp;
using System.Threading.Tasks;

namespace Impar
{
    public partial class Agenda : Form
    {

        Outlook.Application outlookApp;
        Outlook.NameSpace outlookNamespace;
        Outlook.MAPIFolder calendarFolder;
        Outlook.Items calendarItems;


        public Agenda()
        {
            InitializeComponent();
            //InitializeBrowser();
            outlookApp = new Outlook.Application();
            outlookNamespace = outlookApp.GetNamespace("MAPI");

            // Conecta-se à conta do usuário no Outlook
            outlookNamespace.Logon();

            // Obtém a pasta de calendário padrão do Outlook
            calendarFolder = outlookNamespace.GetDefaultFolder(Outlook.OlDefaultFolders.olFolderCalendar);

            // Obtém os itens da pasta de calendário
            calendarItems = calendarFolder.Items;

            // Filtra os itens para o período de um mês a partir de hoje
            string filter = $"[Start] >= '{DateTime.Today.ToString("g")}' AND [End] <= '{DateTime.Today.AddMonths(1).ToString("g")}'";
            calendarItems = calendarItems.Restrict(filter);

            // Adiciona os eventos ao controle "MonthCalendar"
            foreach (Outlook.AppointmentItem item in calendarItems)
            {
                monthCalendar4.AddBoldedDate(item.Start.Date);
            }

            // Atualiza o controle "MonthCalendar"
            monthCalendar4.UpdateBoldedDates();
        }


        //private void InitializeBrowser()
        //{
        //    Cef.Initialize(new CefSettings());

        //    chromiumWebBrowser1 = new ChromiumWebBrowser();
        //    chromiumWebBrowser1.Dock = DockStyle.Fill;
        //    this.Controls.Add(chromiumWebBrowser1);

        //    chromiumWebBrowser1.FrameLoadEnd += OnFrameLoadEnd;
        //}

        //private void OnFrameLoadEnd(object sender, FrameLoadEndEventArgs e)
        //{
        //    if (e.Frame.IsMain)
        //    {
        //        if (e.Url == "https://login.live.com/login.srf")
        //        {
        //            chromiumWebBrowser1.ExecuteScriptAsync($"document.getElementById('i0116').value = 'marcosmagalhaes86@outlook.pt'");
        //            chromiumWebBrowser1.ExecuteScriptAsync($"document.getElementById('i0118').value = 'M@galhae$020486'");
        //            chromiumWebBrowser1.ExecuteScriptAsync($"document.querySelector('#idSIButton9').click()");
        //        }
        //        else if (e.Url == "https://outlook.live.com/mail/inbox")
        //        {
        //            chromiumWebBrowser1.Load("https://outlook.live.com/calendar/0/view/month");
        //        }
        //    }
        //}


private void Agenda_Load(object sender, EventArgs e)
        {
            // chromiumWebBrowser1.Load("https://www.google.com/");
            // chromiumWebBrowser1.Load(" https://outlook.live.com/calendar/");
            // Navega para a página de login do Outlook
            chromiumWebBrowser1.Load("https://login.live.com/login");


            // Registra o evento LoadingStateChanged para saber quando a página foi carregada
            chromiumWebBrowser1.LoadingStateChanged += ChromiumWebBrowser1_LoadingStateChanged;
        }

        private async void ChromiumWebBrowser1_LoadingStateChanged(object sender, LoadingStateChangedEventArgs e)
        {

          

            if (!e.IsLoading)
            {
                if (chromiumWebBrowser1.Address.Contains("https://login.live.com/login"))
                {
                    // Insere o e-mail e aguarda o preenchimento automático do campo de senha
                    await chromiumWebBrowser1.EvaluateScriptAsync("document.getElementById('i0116').value='marcosmagalhaes86@outlook.pt';");
                    await Task.Delay(2000);

                    // Insere a senha e clica no botão de login
                    await chromiumWebBrowser1.EvaluateScriptAsync("document.getElementById('i0118').value='M@galhae$020486';");
                    await Task.Delay(2000);
                    await chromiumWebBrowser1.EvaluateScriptAsync("document.getElementById('idSIButton9').click();");

                    // Aguarda o carregamento da página de autenticação de dois fatores, se necessário
                    await Task.Delay(2000);

                    // Aguarda o carregamento completo da página de calendário
                    await Task.Delay(5000);

                    // Redireciona para a página de calendário após o login
                    chromiumWebBrowser1.Load("https://outlook.live.com/calendar/0/view/month");
                }
            }

        }



        private void buttonAdicionarEvento_Click(object sender, EventArgs e)
        {
         
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Outlook.Application outlookApp = new Outlook.Application();
            Outlook.MAPIFolder calendarFolder = outlookApp.Session.GetDefaultFolder(Outlook.OlDefaultFolders.olFolderCalendar);

            if (calendarFolder != null)
            {
                string exportFilePath = Path.Combine(@"C:\CalendarioOutlook", "Calendário.csv");

                if (File.Exists(exportFilePath))
                {
                    File.Delete(exportFilePath);
                }

          

                if (File.Exists(exportFilePath))
                {
                    using (StreamReader sr = new StreamReader(exportFilePath))
                    {
                        dataGridView2.ColumnCount = 3;
                        dataGridView2.Columns[0].Name = "Assunto";
                        dataGridView2.Columns[1].Name = "Início";
                        dataGridView2.Columns[2].Name = "Fim";

                        while (!sr.EndOfStream)
                        {
                            string[] line = sr.ReadLine().Split(',');
                            object[] appointmentInfo = new object[3];
                            appointmentInfo[0] = line[0];
                            appointmentInfo[1] = DateTime.Parse(line[1]);
                            appointmentInfo[2] = DateTime.Parse(line[2]);
                            dataGridView2.Rows.Add(appointmentInfo);
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Não foi possível exportar o calendário do Outlook.");
                }
            }
            else
            {
                MessageBox.Show("Não foi possível localizar uma pasta de calendário válida no Outlook.");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                // Inicializa o objeto do Outlook
                Outlook.Application outlookApp = new Outlook.Application();

                // Obtém a pasta de calendário padrão do Outlook
                Outlook.MAPIFolder calendarFolder = outlookApp.Session.GetDefaultFolder(Outlook.OlDefaultFolders.olFolderCalendar);

                // Obtém uma lista de todos os itens na pasta de calendário
                Outlook.Items calendarItems = calendarFolder.Items;

                // Cria uma lista de objetos para armazenar os dados do calendário
                List<object[]> calendarData = new List<object[]>();

                // Itera sobre todos os itens do calendário e adiciona seus dados à lista de dados do calendário
                foreach (Outlook.AppointmentItem item in calendarItems)
                {
                    calendarData.Add(new object[] { item.Subject, item.Start.ToString("g"), item.End.ToString("g") });
                }

                // Define as colunas do DataGridView
                dataGridView1.Columns.Add("Subject", "Assunto");
                dataGridView1.Columns.Add("Start", "Início");
                dataGridView1.Columns.Add("End", "Fim");

                // Adiciona os dados do calendário ao DataGridView
                foreach (object[] data in calendarData)
                {
                    dataGridView1.Rows.Add(data);
                }

                MessageBox.Show("Dados do calendário importados com sucesso.", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao importar dados do calendário: " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void ImportarCalendario(string importFilePath)
        {
            try
            {
                // Inicializa o objeto do Outlook
                Outlook.Application outlookApp = new Outlook.Application();

                // Obtém a pasta de calendário padrão do Outlook
                Outlook.MAPIFolder calendarFolder = outlookApp.Session.GetDefaultFolder(Outlook.OlDefaultFolders.olFolderCalendar);

                // Remove todos os itens da pasta de calendário
                foreach (object item in calendarFolder.Items)
                {
                    ((Outlook.AppointmentItem)item).Delete();
                }

                // Lê o arquivo CSV e cria itens de compromisso no calendário do Outlook
                using (StreamReader reader = new StreamReader(importFilePath, Encoding.Default))
                {
                    while (!reader.EndOfStream)
                    {
                        string[] fields = reader.ReadLine().Split(',');

                        // Cria um novo item de compromisso no calendário
                        Outlook.AppointmentItem appointmentItem = (Outlook.AppointmentItem)calendarFolder.Items.Add(Outlook.OlItemType.olAppointmentItem);

                        // Define as propriedades do item de compromisso
                        appointmentItem.Subject = fields[0];
                        appointmentItem.Start = DateTime.Parse(fields[1]);
                        appointmentItem.End = DateTime.Parse(fields[2]);
                        appointmentItem.Location = fields[3];
                        appointmentItem.Body = fields[4];
                        appointmentItem.ReminderMinutesBeforeStart = int.Parse(fields[5]);

                        // Salva o item de compromisso no calendário
                        appointmentItem.Save();
                    }
                }

                MessageBox.Show("Dados do calendário importados com sucesso.", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao importar dados do calendário: " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button3_Click(object sender, EventArgs e, string exportFilePath)
        {
            try
            {
                // Inicializa o objeto do Outlook
                Outlook.Application outlookApp = new Outlook.Application();

                // Obtém a pasta de calendário padrão do Outlook
                Outlook.MAPIFolder calendarFolder = outlookApp.Session.GetDefaultFolder(Outlook.OlDefaultFolders.olFolderCalendar);

                // Cria o arquivo CSV
                using (StreamWriter sw = new StreamWriter(exportFilePath, false, Encoding.UTF8))
                {
                    // Escreve o cabeçalho do arquivo CSV
                    sw.WriteLine("Subject,Start Time,End Time,Location,Description");

                    // Itera sobre os itens da pasta de calendário e escreve os dados no arquivo CSV
                    foreach (Outlook.AppointmentItem appt in calendarFolder.Items)
                    {
                        string subject = appt.Subject;
                        string startTime = appt.Start.ToString();
                        string endTime = appt.End.ToString();
                        string location = appt.Location;
                        string description = appt.Body;

                        // Remove as quebras de linha do campo de descrição
                        description = description.Replace("\r\n", " ");

                        // Escreve os dados no arquivo CSV
                        sw.WriteLine($"{subject},{startTime},{endTime},{location},{description}");
                    }
                }

                MessageBox.Show("Dados do calendário exportados com sucesso.", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao exportar dados do calendário: " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void MonthCalendar4_DateSelected(object sender, DateRangeEventArgs e)
        {
            Outlook.Application outlookApp = new Outlook.Application();
            Outlook.MAPIFolder calendarFolder = outlookApp.Session.GetDefaultFolder(Outlook.OlDefaultFolders.olFolderCalendar);

            DateTime startDate = monthCalendar4.SelectionRange.Start;
            DateTime endDate = monthCalendar4.SelectionRange.End;

            string filter = String.Format("[Start] >= '{0}' AND [End] <= '{1}'", startDate.ToString("g"), endDate.ToString("g"));

            Outlook.Items calendarItems = calendarFolder.Items.Restrict(filter);

            // Exibir os eventos em um DataGridView
            dataGridView1.DataSource = calendarItems;
        }

        private void MonthCalendar4_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            Outlook.Application outlookApp = new Outlook.Application();
            Outlook.MAPIFolder calendarFolder = outlookApp.Session.GetDefaultFolder(Outlook.OlDefaultFolders.olFolderCalendar);
            DateTime date = monthCalendar4.SelectionStart;
            string filter = "[Start] >= '" + date.ToString("g") + "' AND [End] <= '" + date.ToString("g") + "'";
            Outlook.Items calendarItems = calendarFolder.Items.Restrict(filter);
            if (calendarItems.Count > 0)
            {
                Outlook.AppointmentItem appointmentItem = calendarItems[1] as Outlook.AppointmentItem;
                if (appointmentItem != null)
                {
                    appointmentItem.Display();
                }
            }
        }

        private void monthCalendar4_DateSelected_1(object sender, DateRangeEventArgs e)
        {
            // Cria uma instância do objeto Outlook
            Outlook.Application outlookApp = new Outlook.Application();

            // Obtém a pasta de calendário padrão do Outlook
            Outlook.MAPIFolder calendarFolder = outlookApp.Session.GetDefaultFolder(Outlook.OlDefaultFolders.olFolderCalendar);

            // Obtém a data selecionada no MonthCalendar
            DateTime selectedDate = monthCalendar4.SelectionStart.Date;

            // Filtra os itens do calendário para mostrar apenas os eventos que ocorrem na data selecionada
            string filter = string.Format("[Start] >= '{0}' AND [End] < '{1}'", selectedDate.ToString("g"), selectedDate.AddDays(1).ToString("g"));
            Outlook.Items calendarItems = calendarFolder.Items.Restrict(filter);

            // Cria uma lista de eventos do calendário a serem exibidos no DataGridView
            List<CalendarEvent> eventsList = new List<CalendarEvent>();

            // Itera sobre cada item no calendário e adiciona os eventos que ocorrem na data selecionada à lista de eventos
            foreach (Outlook.AppointmentItem item in calendarItems)
            {
                if (item.Start.Date == selectedDate)
                {
                    eventsList.Add(new CalendarEvent { Subject = item.Subject, StartTime = item.Start.TimeOfDay, EndTime = item.End.TimeOfDay });
                }
            }

            // Exibe os eventos no DataGridView
            dataGridView1.DataSource = eventsList;
        }

        // Define uma classe para representar um evento do calendário
        public class CalendarEvent
        {
            public string Subject { get; set; }
            public TimeSpan StartTime { get; set; }
            public TimeSpan EndTime { get; set; }
        }
    }
}










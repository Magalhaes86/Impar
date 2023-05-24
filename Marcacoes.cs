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

using System.Net.Http;
using System.Threading.Tasks;

namespace Impar
{
    public partial class Marcacoes : Form
    {

        private BindingSource bindingSource = new BindingSource();

        private static AgendamentoSms agendamentoSms;
        private static System.Windows.Forms.Timer timer;

        public Marcacoes()
        {
            InitializeComponent();
            agendamentoSms = new AgendamentoSms();
            timer = new System.Windows.Forms.Timer();
            timer.Interval = 10000; // 10 segundos
            timer.Tick += Timer_Tick;
            timer.Start();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            agendamentoSms.EnviarSmsAgendado();
        }

        private void Marcacoes_Load(object sender, EventArgs e)
        {
            kryptonCheckBox1.Size = new Size(106, 40);

        }


        //   MySqlConnection connection = new MySqlConnection(@"server=localhost;database=ContabSysDB;port=3308;userid=root;password=xd");
        MySqlConnection connection = new MySqlConnection(@"server=" + Properties.Settings.Default.server + ";database=" + Properties.Settings.Default.basedados + ";port=" + Properties.Settings.Default.porta + ";userid=" + Properties.Settings.Default.username + ";password=" + Properties.Settings.Default.password);

        MySqlCommand command;

        private void AtualizarBindingSource()
        {
            // Abre a conexão com o banco de dados
            connection.Open();

            // Executa a consulta SQL para atualizar a tabela "marcacoes"
            string selectQuery = "SELECT * FROM marcacoes";
            MySqlDataAdapter adapter = new MySqlDataAdapter(selectQuery, connection);
            DataTable dataTable = new DataTable();
            adapter.Fill(dataTable);

            // Define o DataSource do BindingSource como sendo a nova tabela de dados
            bindingSource.DataSource = dataTable;

         
            // Fecha a conexão com o banco de dados
            connection.Close();
        }



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
            string agendarSms = kryptonCheckBox2.Checked ? "1" : "0";

            string insertQuery = "INSERT INTO marcacoes (Idcliente, IDGoogle, Horario, TipoTratamento, Obs, Descricao, TituloGoogle, Horainicio, Horafim, Nome, telemovel, SmsEnviada, IdTipoTratamento, AgendarSms) " +
                "VALUES('" + tbcodcliente.Text + "','" + tbidgoogle.Text + "','" + tbhorario.Text + "','" + tbtipotratamento.Text + "','" + tbobs.Text + "','" + tbdescricao.Text + "','" + tbtitulogoogle.Text + "','" + tbhorainicio.Text + "','" + tbhorafim.Text + "','" + tbnomepaciente.Text + "','" + tbtlmpaciente.Text + "','" + smsEnviada + "','" + tbidtipotratamento.Text + "','" + agendarSms + "');";
            executeMyQuery(insertQuery);


        }

        private void atualizarMarcacoes()
        {

            //string smsEnviada = kryptonCheckBox1.Checked ? "1" : "0";
            //string updateQuery = "UPDATE marcacoes SET Idcliente = '" + tbcodcliente.Text + "', Horario = '" + tbhorario.Text + "', TipoTratamento = '" + tbtipotratamento.Text + "', Obs = '" + tbobs.Text + "', Descricao = '" + tbdescricao.Text + "', TituloGoogle = '" + tbtitulogoogle.Text + "', Horainicio = '" + tbhorainicio.Text + "', Horafim = '" + tbhorafim.Text + "', Nome = '" + tbnomepaciente.Text + "', telemovel = '" + tbtlmpaciente.Text + "', SmsEnviada = '" + smsEnviada + "', IdTipoTratamento = '" + tbidtipotratamento.Text + "' WHERE IDGoogle = '" + tbidgoogle.Text + "';";
            //executeMyQuery(updateQuery);
            string smsEnviada = kryptonCheckBox1.Checked ? "1" : "0";
            string agendarSms = kryptonCheckBox2.Checked ? "1" : "0";

            string updateQuery = "UPDATE marcacoes SET Idcliente = '" + tbcodcliente.Text + "', Horario = '" + tbhorario.Text + "', TipoTratamento = '" + tbtipotratamento.Text + "', Obs = '" + tbobs.Text + "', Descricao = '" + tbdescricao.Text + "', TituloGoogle = '" + tbtitulogoogle.Text + "', Horainicio = '" + tbhorainicio.Text + "', Horafim = '" + tbhorafim.Text + "', Nome = '" + tbnomepaciente.Text + "', telemovel = '" + tbtlmpaciente.Text + "', SmsEnviada = '" + smsEnviada + "', IdTipoTratamento = '" + tbidtipotratamento.Text + "', AgendarSms = '" + agendarSms + "' WHERE IDGoogle = '" + tbidgoogle.Text + "';";
            executeMyQuery(updateQuery);


        }



        // A Trabalhar



        private void GravarNovoAgendamento()
        {

            string smsEnviada = kryptonCheckBox1.Checked ? "1" : "0";
            string sms1 = kryptonCheckBox3.Checked ? "1" : "0";
            string sms2 = kryptonCheckBox4.Checked ? "1" : "0";
            string sms3 = kryptonCheckBox5.Checked ? "1" : "0";
            string agendamentoDeSms = kryptonCheckBox2.Checked ? "1" : "0";
            string sms1Enviada = kryptonCheckBox8.Checked ? "1" : "0";
            string sms2Enviada = kryptonCheckBox7.Checked ? "1" : "0";
            string sms3Enviada = kryptonCheckBox6.Checked ? "1" : "0";

            string insertQuery = "INSERT INTO agendamentos (Idcliente, IDGoogle, Horario, TipoTratamento, Obs, Descricao, TituloGoogle, Horainicio, Horafim, Nome, telemovel, SmsEnviada, IdTipoTratamento, Sms1, Sms1Data, Sms1Hora, Sms1CorpoSMS, Sms2, Sms2Data, Sms2Hora, Sms2CorpoSMS, Sms3, Sms3Data, Sms3Hora, Sms3CorpoSMS, AgendamentoDeSms, Sms1Enviada, Sms2Enviada, Sms3Enviada) " +
                "VALUES ('" + tbcodcliente.Text + "','" + tbidgoogle.Text + "','" + tbhorario.Text + "','" + tbtipotratamento.Text + "','" + tbobs.Text + "','" + tbdescricao.Text + "','" + tbtitulogoogle.Text + "','" + tbhorainicio.Text + "','" + tbhorafim.Text + "','" + tbnomepaciente.Text + "','" + tbtlmpaciente.Text + "','" + smsEnviada + "','" + tbidtipotratamento.Text + "','" + sms1 + "','" + kryptonDateTimePicker1.Text + "','" + kryptonDateTimePicker2.Text + "','" + kryptonTextBox2.Text + "','" + sms2 + "','" + kryptonDateTimePicker3.Text + "','" + kryptonDateTimePicker4.Text + "','" + kryptonTextBox3.Text + "','" + sms3 + "','" + kryptonDateTimePicker5.Text + "','" + kryptonDateTimePicker6.Text + "','" + kryptonTextBox5.Text + "','" + agendamentoDeSms + "','" + sms1Enviada + "','" + sms2Enviada + "','" + sms3Enviada + "');";
            executeMyQuery(insertQuery);
        }





        private void AtualizarrNovoAgendamento()
        {
            string smsEnviada = kryptonCheckBox1.Checked ? "1" : "0";
            string sms1 = kryptonCheckBox3.Checked ? "1" : "0";
            string sms2 = kryptonCheckBox4.Checked ? "1" : "0";
            string sms3 = kryptonCheckBox5.Checked ? "1" : "0";
            string agendamentoDeSms = kryptonCheckBox2.Checked ? "1" : "0";
            string sms1Enviada = kryptonCheckBox8.Checked ? "1" : "0";
            string sms2Enviada = kryptonCheckBox7.Checked ? "1" : "0";
            string sms3Enviada = kryptonCheckBox6.Checked ? "1" : "0";

            string updateQuery = "UPDATE agendamentos SET Idcliente = '" + tbcodcliente.Text + "', Horario = '" + tbhorario.Text + "', TipoTratamento = '" + tbtipotratamento.Text + "', Obs = '" + tbobs.Text + "', Descricao = '" + tbdescricao.Text + "', TituloGoogle = '" + tbtitulogoogle.Text + "', Horainicio = '" + tbhorainicio.Text + "', Horafim = '" + tbhorafim.Text + "', Nome = '" + tbnomepaciente.Text + "', telemovel = '" + tbtlmpaciente.Text + "', SmsEnviada = '" + smsEnviada + "', IdTipoTratamento = '" + tbidtipotratamento.Text + "', Sms1 = '" + sms1 + "', Sms1Data = '" + kryptonDateTimePicker1.Text + "', Sms1Hora = '" + kryptonDateTimePicker2.Text + "', Sms1CorpoSMS = '" + kryptonTextBox2.Text + "', Sms2 = '" + sms2 + "', Sms2Data = '" + kryptonDateTimePicker3.Text + "', Sms2Hora = '" + kryptonDateTimePicker4.Text + "', Sms2CorpoSMS = '" + kryptonTextBox3.Text + "', Sms3 = '" + sms3 + "', Sms3Data = '" + kryptonDateTimePicker5.Text + "', Sms3Hora = '" + kryptonDateTimePicker6.Text + "', Sms3CorpoSMS = '" + kryptonTextBox5.Text + "', AgendamentoDeSms = '" + agendamentoDeSms + "', Sms1Enviada = '" + sms1Enviada + "', Sms2Enviada = '" + sms2Enviada + "', Sms3Enviada = '" + sms3Enviada + "' WHERE IDGoogle = '" + tbidgoogle.Text + "';";
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




        // Funciona bem

        private void AtualizarMarcacoesnoGoogle()
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

            // Obtém o ID do evento do TextBox tbidgoogle
            string eventId = tbidgoogle.Text;

            try
            {
                // Obtém o evento existente a ser atualizado
                Event existingEvent = service.Events.Get("marcosmagalhaes86@gmail.com", eventId).Execute();

                // Atualiza as propriedades do evento existente
                existingEvent.Summary = tbtitulogoogle.Text;
                existingEvent.Location = "Local do Evento";
                existingEvent.Description = tbdescricao.Text;
                existingEvent.Start = new EventDateTime()
                {
                    DateTime = new DateTime(tbhorario.Value.Year, tbhorario.Value.Month, tbhorario.Value.Day, tbhorainicio.Value.Hour, tbhorainicio.Value.Minute, 0),
                    TimeZone = "Europe/Lisbon"
                };
                existingEvent.End = new EventDateTime()
                {
                    DateTime = new DateTime(tbhorario.Value.Year, tbhorario.Value.Month, tbhorario.Value.Day, tbhorafim.Value.Hour, tbhorafim.Value.Minute, 0),
                    TimeZone = "Europe/Lisbon"
                };

                // Executa a atualização do evento
                Event updatedEvent = service.Events.Update(existingEvent, "marcosmagalhaes86@gmail.com", eventId).Execute();
                MessageBox.Show("Evento atualizado com sucesso!");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao atualizar evento: {ex.Message}");
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

                GravarNovoAgendamento();
            }
        }

        private void btatualizar_Click(object sender, EventArgs e)
        {
            atualizarMarcacoes();
        }

        private void btanterior_Click(object sender, EventArgs e)
        {
          
       
        }

        private void btseguinte_Click(object sender, EventArgs e)
        {
         
        }

        private void btprimeiro_Click(object sender, EventArgs e)
        {
          
        }

        private void tbultimo_Click(object sender, EventArgs e)
        {
         
        }






        //CODIGO DE ENVIO SMS
        //private async void btEnviarSMS_Click(object sender, EventArgs e)
        //{
        //    // Verifica se o kryptonCheckBox1 está marcado
        //    if (kryptonCheckBox1.Checked)
        //    {
        //        // Obtém o número de telefone do destinatário
        //        string numeroTelemovel = tbtlmpaciente.Text;

        //        // Verifica se o número de telefone foi preenchido corretamente
        //        if (string.IsNullOrEmpty(numeroTelemovel))
        //        {
        //            // Exibe mensagem de erro
        //            MessageBox.Show("O número de telemóvel deve ser preenchido.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //            return;
        //        }
        //        else if (numeroTelemovel.Length != 12)
        //        {
        //            // Exibe mensagem de erro
        //            MessageBox.Show("O número de telemóvel deve possuir 12 dígitos (incluindo o prefixo +351).", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //            return;
        //        }

        //        // Envia a mensagem
        //        string from = "YourFromNumberHere"; // Número de origem
        //        string to = numeroTelemovel; // Número de destino
        //        string content = "Sua mensagem aqui"; // Conteúdo da mensagem
        //        bool enviado = await EnviarSMS(from, to, content);

        //        // Exibe mensagem de sucesso ou erro
        //        if (enviado)
        //        {
        //            MessageBox.Show("A mensagem foi enviada com sucesso.", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
        //        }
        //        else
        //        {
        //            MessageBox.Show("Ocorreu um erro ao enviar a mensagem.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //        }
        //    }
        //    else
        //    {
        //        // Exibe mensagem de erro
        //        MessageBox.Show("O checkbox para envio de SMS não está marcado.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //    }
        //}



        //+351910045307


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



        private async void button4_Click(object sender, EventArgs e)
        {
            // Verifica se o kryptonCheckBox1 está marcado
            if (kryptonCheckBox1.Checked)
            {
                // Obtém o número de telefone do destinatário
                string numeroTelemovel = tbtlmpaciente.Text;

                // Verifica se o número de telefone foi preenchido corretamente
                if (string.IsNullOrEmpty(numeroTelemovel))
                {
                    // Exibe mensagem de erro
                    MessageBox.Show("O número de telemóvel deve ser preenchido.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Pergunta ao usuário se deseja enviar a mensagem de texto
                var result = MessageBox.Show("Antes de continuar, verifique se tem a aplicação iniciada no seu telemóvel. Deseja continuar com o envio da mensagem?", "Confirmação", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                // Verifica se o usuário escolheu o botão Sim
                if (result == DialogResult.Yes)
                {
                    // Envia a mensagem
                    string from = "+351910045307"; // Número de origem PASSAR DEPOIS PARA PARAMETRO
                    string to = numeroTelemovel; // Número de destino vai Buscar à Testbox
                    string content = "Estimado" + tbnomepaciente.Text + "informamos que tem marcação no dia " + tbhorario.Text + " às " + tbhorainicio.Text; // Conteúdo da mensagem - Vamos definir o tipo de tratamento a ir buscar as mensagens 
                    bool enviado = await EnviarSMS(from, to, content);

                    // Exibe mensagem de sucesso ou erro
                    if (enviado)
                    {
                        MessageBox.Show("A mensagem foi enviada com sucesso.", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("Ocorreu um erro ao enviar a mensagem. Verifique se tem a aplicação iniciada no seu telemóvel.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    // Exibe mensagem de erro
                    MessageBox.Show("O envio da mensagem de texto foi cancelado.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("O checkbox para envio de SMS não está marcado.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void kryptonTextBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void button3_Click_1(object sender, EventArgs e)
        {
            // Abre o FormAdicionarEvento sem passar nenhum parâmetro
            listagemPacientesMarcacoes form = new listagemPacientesMarcacoes();
            form.Show();
        }

        private void kryptonButton1_Click(object sender, EventArgs e)
        {
            // Abre o FormAdicionarEvento sem passar nenhum parâmetro
            lsttratamento form = new lsttratamento();
            form.Show();
        }

        private void kryptonButton2_Click(object sender, EventArgs e)
        {
            FrmAgenda form = new FrmAgenda();
            form.Show();
        }

        private void button3_Click_2(object sender, EventArgs e)
        {

        }

        private void button5_Click(object sender, EventArgs e)
        {

            GravarNovoAgendamento();

        }

        private void kryptonButton8_Click(object sender, EventArgs e)
        {
            AtualizarrNovoAgendamento();

        }

        private void timer2_Tick(object sender, EventArgs e)
        {

        }

        private void kryptonButton9_Click(object sender, EventArgs e)
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
                AtualizarMarcacoesnoGoogle();
                //    ATUALIZAR GOOGLE   InserirMarcacoesnoGoogle();
                Thread.Sleep(1000); // Aguarda 1 segundo
                atualizarMarcacoes();
                //    ATUALIZAR Base de dados   GravarNovaMarcacao();
                AtualizarrNovoAgendamento();
            }
        }

        private void kryptonPage4_Click(object sender, EventArgs e)
        {

        }
    }

}


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
    public class AgendamentoSms
    {
        private MySqlConnection connection;

        public AgendamentoSms()
        {
            string connectionString = "server=" + Properties.Settings.Default.server + ";database=" + Properties.Settings.Default.basedados + ";port=" + Properties.Settings.Default.porta + ";userid=" + Properties.Settings.Default.username + ";password=" + Properties.Settings.Default.password;
            connection = new MySqlConnection(connectionString);
        }

        //   public void EnviarSmsAgendado()
        public async Task EnviarSmsAgendado()
        {

            string query = "SELECT * FROM agendamentos;";
            MySqlCommand command = new MySqlCommand(query, connection);
            MySqlDataAdapter adapter = new MySqlDataAdapter(command);
            DataTable dataTable = new DataTable();

            try
            {
                connection.Open();
                adapter.Fill(dataTable);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao carregar os agendamentos: " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            finally
            {
                connection.Close();
            }

            foreach (DataRow row in dataTable.Rows)
            {
                bool sms1Ativa = Convert.ToBoolean(row["Sms1"]);
                bool sms2Ativa = Convert.ToBoolean(row["Sms2"]);
                bool sms3Ativa = Convert.ToBoolean(row["Sms3"]);

                if (sms1Ativa)
                {
                    DateTime sms1Data = Convert.ToDateTime(row["Sms1Data"]);
                    DateTime sms1Hora = Convert.ToDateTime(row["Sms1Hora"]);
                    bool sms1Enviada = Convert.ToBoolean(row["Sms1Enviada"]);

                    if (VerificarDataHoraAtual(sms1Data, sms1Hora) && !sms1Enviada)
                    {
                        await EnviarSMS("+351910045307", row["telemovel"].ToString(), row["Sms1CorpoSMS"].ToString());
                        AtualizarSmsEnviada(row["IDGoogle"].ToString(), "Sms1Enviada");
                    }
                }

                if (sms2Ativa)
                {
                    DateTime sms2Data = Convert.ToDateTime(row["Sms2Data"]);
                    DateTime sms2Hora = Convert.ToDateTime(row["Sms2Hora"]);
                    bool sms2Enviada = Convert.ToBoolean(row["Sms2Enviada"]);

                    if (VerificarDataHoraAtual(sms2Data, sms2Hora) && !sms2Enviada)
                    {
                        await EnviarSMS("+351910045307", row["telemovel"].ToString(), row["Sms2CorpoSMS"].ToString());
                        AtualizarSmsEnviada(row["IDGoogle"].ToString(), "Sms2Enviada");
                    }
                }

                if (sms3Ativa)
                {
                    DateTime sms3Data = Convert.ToDateTime(row["Sms3Data"]);
                    DateTime sms3Hora = Convert.ToDateTime(row["Sms3Hora"]);
                    bool sms3Enviada = Convert.ToBoolean(row["Sms3Enviada"]);

                    if (VerificarDataHoraAtual(sms3Data, sms3Hora) && !sms3Enviada)
                    {
                        await EnviarSMS("+351910045307", row["telemovel"].ToString(), row["Sms3CorpoSMS"].ToString());
                        AtualizarSmsEnviada(row["IDGoogle"].ToString(), "Sms3Enviada");
                    }
            
                }
            }
        }

        private bool VerificarDataHoraAtual(DateTime data, DateTime hora)
        {
            DateTime dataHoraAtual = DateTime.Now;
            DateTime dataHoraAgendada = new DateTime(data.Year, data.Month, data.Day, hora.Hour, hora.Minute, 0);

            return dataHoraAtual >= dataHoraAgendada;
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
                // Trate a exceção aqui, se necessário
                return false;
            }
        }


        
        private void AtualizarSmsEnviada(string idGoogle, string colunaEnviada)
        {
            string updateQuery = $"UPDATE agendamentos SET {colunaEnviada} = 1 WHERE IDGoogle = @IDGoogle;";
            MySqlCommand updateCommand = new MySqlCommand(updateQuery, connection);
            updateCommand.Parameters.AddWithValue("@IDGoogle", idGoogle);

            try
            {
                connection.Open();
                updateCommand.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao atualizar o status de envio de SMS: " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                connection.Close();
            }
        }
    }
}

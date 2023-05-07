using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;
using System.Text.Json;
namespace Impar
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

       

        private async Task<bool> assinc2(string from, string to, string content)
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
                            from = tbde.Text,
                            To = tbpara.Text,
                            Content = tbsms.Text
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

        private async void button1_Click(object sender, EventArgs e)
        {
            button1.Enabled = false;
            try
            {
                var enviadoComSucesso = await assinc2(tbde.Text, tbpara.Text, tbsms.Text);
                if (enviadoComSucesso)
                {
                    MessageBox.Show("SMS enviado com sucesso!");
                    //tbde.Clear();
                    //tbpara.Clear();
                    //tbsms.Clear();
                }
                else
                {
                    MessageBox.Show("Falha ao enviar SMS!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao enviar SMS: {ex.Message}");
            }
            finally
            {
                button1.Enabled = true;
            }

          
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Agenda form2 = new Agenda();  // Instancia o novo formulário
            form2.ShowDialog();        // Abre o novo formulário como uma janela modal
        }

        private void button3_Click(object sender, EventArgs e)
        {
            GoogleAgenda form2 = new GoogleAgenda();  // Instancia o novo formulário
            form2.ShowDialog();        // Abre o novo formulário como uma janela modal
        }

        private void button4_Click(object sender, EventArgs e)
        {
            pacientes form2 = new pacientes();  // Instancia o novo formulário
            form2.ShowDialog();        // Abre o novo formulário como uma janela modal
        }
    }
}

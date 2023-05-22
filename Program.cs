using System;
using System.Windows.Forms;

namespace Impar
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // Crie uma instância do formulário Agenda
            GoogleAgenda agendaForm = new GoogleAgenda();

            // Verifique se a instância não é nula
            if (agendaForm != null)
            {
                try
                {
                    Application.Run(agendaForm);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Erro ao executar o formulário Agenda: " + ex.Message);
                }
            }
            else
            {
                // Trate o caso em que a instância não foi criada corretamente
                MessageBox.Show("Erro ao criar o formulário Agenda.");
            }
        }
    }
}

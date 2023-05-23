using Google.Apis.Auth.OAuth2;
using Google.Apis.Calendar.v3;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Impar
{
    public class GoogleCalendarService
    {
        private const string ApplicationName = "GoogleAgenda";
        private const string CredentialsFilePath = "C:\\Desenvolvimentos\\EmDesenvolvimento\\IMPAR\\Impar\\bin\\Debug\\keys.json";
        private static readonly string[] Scopes = { CalendarService.Scope.Calendar };

        public CalendarService Service { get; private set; }

        public async Task<bool> Connect()
        {
            try
            {
                GoogleCredential credential;

                using (var stream = new FileStream(CredentialsFilePath, FileMode.Open, FileAccess.Read))
                {
                    credential = GoogleCredential.FromStream(stream)
                        .CreateScoped(Scopes);
                }

                Service = new CalendarService(new BaseClientService.Initializer()
                {
                    HttpClientInitializer = credential,
                    ApplicationName = ApplicationName
                });

                return true;
            }
            catch (Exception ex)
            {
                // Trate a exceção de alguma forma apropriada, por exemplo, mostrando uma mensagem de erro
                Console.WriteLine("Ocorreu um erro ao conectar ao Google Calendar: " + ex.Message);
                return false;
            }
        }
    }
}
using Fluxos.Domain.Wpp;
using System.Net.Http;
using System.Text;

namespace Fluxos.Service
{
    public class WppService
    {
        private readonly HttpClient _httpClient;
        const string constApiKey = "capellani";
        const string constUrl = "http://localhost:3000/client/sendMessage/capellani";

        public WppService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<string> SendMessageAsync(EnviarMensagem mensagem)
        {
            var url = "http://localhost:3000/client/sendMessage/capellani";
            var apiKey = constApiKey;

            var requestBody = new
            {
                chatId = mensagem.chatId,
                contentType = mensagem.contentType,
                content = mensagem.content
            };

            try
            {
                using var request = new HttpRequestMessage(HttpMethod.Post, url);

                request.Headers.Add("accept", "*/*");
                request.Headers.Add("x-api-key", apiKey);
                request.Content = new StringContent(
                    System.Text.Json.JsonSerializer.Serialize(requestBody),
                    Encoding.UTF8,
                    "application/json"
                );

                using var response = await _httpClient.SendAsync(request);
                response.EnsureSuccessStatusCode();

                // Leia o conteúdo antes de sair do bloco using
                var responseBody = await response.Content.ReadAsStringAsync();
                return responseBody;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error sending message: {ex.Message}");
                throw;
            }
        }

    }
}

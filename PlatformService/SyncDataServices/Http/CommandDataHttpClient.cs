using System.Net.Http;
using Microsoft.Extensions.Configuration;
using PlatformService.Dtos;
//using Newtonsoft.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace PlatformService.SyncDataServices.Http
{
    public class CommandDataHttpClient : ICommandDataClient
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;

        public CommandDataHttpClient(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
        }

        public async Task<string> SendPlatformToCommand(PlatformReadDto platform)
        {
            var commandServiceUrl = _configuration["CommandServiceUrl"];

            var httpContent = new StringContent(JsonSerializer.Serialize(platform), Encoding.UTF8, "application/json");

            var result = await _httpClient.PostAsync(commandServiceUrl, httpContent);

            if (result.IsSuccessStatusCode)
            {
                System.Console.WriteLine("Sending POST to command service was OK");
                return "OK";
            }
            else
            {
                System.Console.WriteLine("Sending POST to command service Failed {result.StatusCode}");
                return $"FAIL {result.StatusCode}";
            }
        }
    }
}
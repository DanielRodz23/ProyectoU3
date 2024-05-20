using Microsoft.Maui.Controls.PlatformConfiguration;
using ProyectoU3.Models.LoginModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ProyectoU3.Services
{
    public class LoginClient(HttpClient client)
    {
        private readonly HttpClient client = client;

        public async Task<string?> GetToken(LoginModel model)
        {
            var response = await client.PostAsync("api/login", new StringContent(JsonSerializer.Serialize(model), Encoding.UTF8, "application/json"));
            if (response.IsSuccessStatusCode)
            {
                var str = response.Content.ReadAsStringAsync().Result;
                HttpRequestMessage rm = new HttpRequestMessage();
                rm.RequestUri = new Uri(client.BaseAddress + "api/prueba");
                rm.Method = HttpMethod.Get;
                rm.Headers.Add("Authorization", $"Bearer {str}");
                var resp = await client.SendAsync(rm);
                if (resp.IsSuccessStatusCode)
                {
                    var id = resp.Content.ReadAsStringAsync().Result;
                    Preferences.Set("Id", int.Parse(id));
                }
                return str;
            }
            return null;
        }
        public async Task<bool> Validar(string token)
        {
            var hdrs = client.DefaultRequestHeaders;
            var response = await client.PostAsync("api/Login/Validator", new StringContent(JsonSerializer.Serialize(new { token = token }), Encoding.UTF8, "application/json"));

            return response.IsSuccessStatusCode;
        }
    }
}

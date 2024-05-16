using ProyectoU3.Models.LoginModels;
using System;
using System.Collections.Generic;
using System.Linq;
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
                return response.Content.ReadAsStringAsync().Result;
            }
            return null;
        }
        public async Task<bool> Validar(string token)
        {
            var response  = await client.GetAsync($"api/login/{token}");
            if (response.IsSuccessStatusCode) return true;
            return false;
        }
    }
}

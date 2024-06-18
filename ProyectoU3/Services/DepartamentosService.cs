using CommunityToolkit.Maui.Alerts;
using ProyectoU3.Helpers;
using ProyectoU3.Models.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ProyectoU3.Services
{
    public class DepartamentosService(HttpClient client)
    {
        private readonly HttpClient client = client;

        public async Task<bool> AdminCheckAsync(string token)
        {
            try
            {
                var rq = new HttpRequestMessage();
                rq.Method = HttpMethod.Get;
                rq.RequestUri = new Uri(client.BaseAddress + "api/login");
                rq.Headers.Add("Authorization", $"Bearer {token}");
                var response = client.SendAsync(rq).Result;
                return response.IsSuccessStatusCode;
            }
            catch
            {
                await Toast.Make("Fallo al hacer peticion a api").Show();
                return false;
            }
        }
        public IEnumerable<DepartamentosDTO> GetDepartments()
        {
            var token = SecureStorage.GetAsync("tkn").Result;
            var rq = new HttpRequestMessage();
            rq.Method = HttpMethod.Get;
            rq.Headers.Add("Authorization", $"Bearer {token}");
            rq.RequestUri = new Uri(client.BaseAddress + "api/Departamentos");
            var response = client.SendAsync(rq).Result;
            if (response.IsSuccessStatusCode)
            {
                var json = response.Content.ReadAsStringAsync().Result;
                var data = JsonSerializer.Deserialize<IEnumerable<DepartamentosDTO>>(json, JsonConfig.Options);
                return data ?? Enumerable.Empty<DepartamentosDTO>();
            }
            else
            {
                Toast.Make("Error al realizar la solicitud").Show();
                return Enumerable.Empty<DepartamentosDTO>();
            }
        }
        public bool PostDepartamento(DepartamentosDTO departamento)
        {
            var token = SecureStorage.GetAsync("tkn").Result;
            var rq = new HttpRequestMessage();
            rq.Method = HttpMethod.Post;
            rq.Headers.Add("Authorization", $"Bearer {token}");
            rq.RequestUri = new Uri(client.BaseAddress + "api/Departamentos");
            var jsoncontent = JsonSerializer.Serialize(departamento);
            rq.Content = new StringContent(jsoncontent, Encoding.UTF8, "application/json");
            var response = client.SendAsync(rq).Result;
            return response.IsSuccessStatusCode;
        }
        public bool DeleteDepartment(int id)
        {
            var token = SecureStorage.GetAsync("tkn").Result;
            var rq = new HttpRequestMessage();
            rq.Method = HttpMethod.Get;
            rq.Headers.Add("Authorization", $"Bearer {token}");
            rq.RequestUri = new Uri(client.BaseAddress + "api/Departamentos/Delete/" + $"{id.ToString()}");
            var response = client.SendAsync(rq).Result;
            return response.IsSuccessStatusCode;
        }

        public string GetDepartamento()
        {
            var token = SecureStorage.GetAsync("tkn").Result;
            var rq = new HttpRequestMessage();
            rq.Method = HttpMethod.Get;
            rq.Headers.Add("Authorization", $"Bearer {token}");
            rq.RequestUri = new Uri(client.BaseAddress + "api/Departamentos/nombre");
            var response = client.SendAsync(rq).Result;
            if (response.IsSuccessStatusCode)
            {
                var nombre = response.Content.ReadAsStringAsync().Result;
                SecureStorage.SetAsync("name", nombre);
                return nombre;
            }
            Toast.Make("No fue posible cargar el departamento").Show();
            return "User";
        }
    }
}

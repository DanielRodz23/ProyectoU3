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
    public class ActividadesService(HttpClient client)
    {
        private readonly HttpClient client = client;
        public async Task<IEnumerable<ActividadesDTO>?> GetActividades(string token)
        {
            var rm = new HttpRequestMessage() { RequestUri = new Uri(client.BaseAddress + ApiUriHelper.ListaActividadesUri), Method = HttpMethod.Get };
            rm.Headers.Add("Authorization", $"Bearer {token}");
            var response = await client.SendAsync(rm);
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                var iact = JsonSerializer.Deserialize<IEnumerable<ActividadesDTO>>(json);
                return iact?? new List<ActividadesDTO> ();
            }
            return null;
        }
        public async Task<bool> InsertarActividad(string token, ActividadesDTO actividad)
        {
            var rm = new HttpRequestMessage() { RequestUri = new Uri(client.BaseAddress + ApiUriHelper.InsertActividadUri), Method = HttpMethod.Post };
            rm.Headers.Add("Authorization", $"Bearer {token}");
            var response = await client.SendAsync(rm);
            return response.IsSuccessStatusCode;
        }
        public async Task<ActividadesDTO> GetActividadOrBorrador(string token, int id)
        {
            var rm = new HttpRequestMessage() { RequestUri = new Uri(client.BaseAddress + ApiUriHelper.ActividadConcatenarUri + id.ToString()), Method = HttpMethod.Get };
            rm.Headers.Add("Authorization", $"Bearer {token}");
            var response = await client.SendAsync(rm);
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                var iact = JsonSerializer.Deserialize<ActividadesDTO>(json);
                return iact ?? new ActividadesDTO();
            }
            return null;
        }
    }
}

using AutoMapper;
using ProyectoU3.Helpers;
using ProyectoU3.Models.DTOs;
using ProyectoU3.Models.Entities;
using ProyectoU3.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Uri = System.Uri;
using Exception = System.Exception;

namespace ProyectoU3.Services
{
    public class ActividadesService(HttpClient client, ActividadesRepository actividadesRepository, IMapper mapper)
    {
        private readonly HttpClient client = client;
        private readonly IMapper mapper = mapper;

        public ActividadesRepository ActividadesRepository { get; } = actividadesRepository;

        public async Task<IEnumerable<ActividadesDTO>?> GetActividades(string token, DateTime fecha)
        {
            var fechaString = fecha.ToString("yyyy-MM-ddTHH:mm:ss.fffZ");
            var rm = new HttpRequestMessage() { RequestUri = new Uri(client.BaseAddress + ApiUriHelper.ListaActividadesUri + $"/?fecha={Uri.EscapeDataString(fechaString)}"), Method = HttpMethod.Get };
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
        
        public async Task<int> InsertarActividad(string token, InsertAct actividad)
        {
            var rm = new HttpRequestMessage() { RequestUri = new Uri(client.BaseAddress + ApiUriHelper.InsertActividadUri ), Method = HttpMethod.Post };
            rm.Headers.Add("Authorization", $"Bearer {token}");
            rm.Content = new StringContent(JsonSerializer.Serialize(actividad), Encoding.UTF8, "application/json");
            var response = await client.SendAsync(rm);
            if (response.IsSuccessStatusCode)
            {
                var act = JsonSerializer.Deserialize<ActividadesDTO>(await response.Content.ReadAsStringAsync());
                return act.id;
            }
            return 0;
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
        public async Task<IEnumerable<ActividadesDTO>?> GetMyBorradores()
        {
            var fecha = Preferences.Get("UltimaFechaActualizacion", DateTime.MinValue);
            var token = await SecureStorage.GetAsync("tkn");
            var fechaString = fecha.ToString("yyyy-MM-ddTHH:mm:ss.fffZ");
            var rm = new HttpRequestMessage() { RequestUri = new Uri(client.BaseAddress + ApiUriHelper.GetAllBorradoresUri + $"/?fecha={Uri.EscapeDataString(fechaString)}"), Method = HttpMethod.Get };
            rm.Headers.Add("Authorization", $"Bearer {token}");
            var response = await client.SendAsync(rm);
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                var iact = JsonSerializer.Deserialize<IEnumerable<ActividadesDTO>>(json);
                return iact ?? null;
            }
            return null;
        }
        public async Task<bool> UploadImagen(int id, string base64)
        {
            var token = await SecureStorage.GetAsync("tkn");
            var rm = new HttpRequestMessage() { RequestUri = new System.Uri(client.BaseAddress + "api/image/" + id.ToString()), Method = HttpMethod.Post };
            rm.Headers.Add("Authorization", $"Bearer {token}");
            var content = new StringContent(JsonSerializer.Serialize(base64), Encoding.UTF8, "application/json");

            rm.Content = content;

            var response = await client.SendAsync(rm);


           return response.IsSuccessStatusCode;
        }


        #region SQLite
        public event Action? DatosActualizados;

        public async Task GetAllAsync()
        {
            try
            {
                var fecha = Preferences.Get("UltimaFechaActualizacion", DateTime.MinValue);

                bool aviso = false;

                HttpRequestMessage rm = new HttpRequestMessage();

                var tkn = await SecureStorage.GetAsync("tkn");

                var response = await GetActividades(tkn??"", fecha);

                var borradores = await GetMyBorradores();

                if (borradores != null || borradores.FirstOrDefault() != null)
                {
                    foreach (var actividad in borradores)
                    {
                        var entidad = ActividadesRepository.Get(actividad.id);

                        if (entidad == null) //SI no estaba en BD Local, lo agrego
                        {
                            entidad = mapper.Map<Actividades>(actividad);
                            ActividadesRepository.Insert(entidad);
                            aviso = true;
                        }
                        else
                        {
                            if (entidad != null)
                            {
                                if (actividad.estado == (int)Estado.Eliminado)
                                {
                                    ActividadesRepository.Delete(entidad);
                                    aviso = true;
                                }
                                else
                                {

                                    if (actividad.titulo != entidad.titulo || actividad.descripcion != entidad.descripcion || actividad.estado != entidad.estado || actividad.departamento != entidad.departamento)
                                    {
                                        ActividadesRepository.Update(entidad);
                                        aviso = true;
                                    }
                                }
                            }
                        }


                    }

                    if (aviso)
                    {

                        _ = MainThread.InvokeOnMainThreadAsync(() =>
                        {
                            CommunityToolkit.Maui.Alerts.Toast.Make("Actualizado").Show();
                            DatosActualizados?.Invoke();
                        });
                    }

                    Preferences.Set("UltimaFechaActualizacion", response.Max(x => x.fechaActualizacion));
                }

                if (response!=null)
                {
                    foreach (var actividad in response)
                    {
                        var entidad = ActividadesRepository.Get(actividad.id);

                        if (entidad == null) //SI no estaba en BD Local, lo agrego
                        {
                            entidad = mapper.Map<Actividades>(actividad);
                            ActividadesRepository.Insert(entidad);
                            aviso = true;
                        }
                        else
                        {
                            if (entidad != null)
                            {
                                if (actividad.estado == (int)Estado.Eliminado)
                                {
                                    ActividadesRepository.Delete(entidad);
                                    aviso = true;
                                }
                                else
                                {

                                    if (actividad.titulo != entidad.titulo || actividad.descripcion !=entidad.descripcion || actividad.estado != entidad.estado || actividad.departamento!=entidad.departamento)
                                    {
                                        ActividadesRepository.Update(entidad);
                                        aviso = true;
                                    }
                                }
                            }
                        }


                    }

                    if (aviso)
                    {

                        _ = MainThread.InvokeOnMainThreadAsync(() =>
                        {
                            DatosActualizados?.Invoke();
                        });
                    }

                    Preferences.Set("UltimaFechaActualizacion", response.Max(x => x.fechaActualizacion));
                }

            }
            catch (Exception e)
            {
            }
        }
        #endregion
    }
}

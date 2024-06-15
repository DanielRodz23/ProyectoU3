using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ProyectoU3.Models.DTOs;
using ProyectoU3.Services;
using System.Collections.ObjectModel;

namespace ProyectoU3.ViewModels
{
    public partial class AgregarDepartamentoView : ObservableObject
    {
        public AgregarDepartamentoView(DepartamentosService departamentosService)
        {
            this.departamentosService = departamentosService;
            FillDepartments();
        }
        private readonly DepartamentosService departamentosService;

        [ObservableProperty]
        string error;

        [ObservableProperty]
        DepartamentosDTO departamento = new();

        [ObservableProperty]
        DepartamentosDTO departamentoSeleccionado;

        public ObservableCollection<DepartamentosDTO> Departamentos { get; set; } = new();
        void FillDepartments()
        {
            Departamentos.Clear();
            var data = departamentosService.GetDepartments();
            foreach (var item in data)
            {
                Departamentos.Add(item);
            }
        }
        [RelayCommand]
        void AgregarDepartamento()
        {
            //Departamento.IdSuperior = DepartamentoSeleccionado.Id;

            if (string.IsNullOrWhiteSpace(Departamento.Nombre) || string.IsNullOrEmpty(Departamento.Nombre))
            {
                Error = "El nombre no debe estar vacio";
            }
            else if (string.IsNullOrWhiteSpace(Departamento.Username) || string.IsNullOrEmpty(Departamento.Username))
            {
                Error = "El nombre de usuario no debe estar vacio";
            }
            else if (!Departamento.Username.EndsWith("@realmail.com"))
            {
                Error = "El nombre de usuario debe terminar con @realmail.com";
            }
            else if (string.IsNullOrWhiteSpace(Departamento.Password) || string.IsNullOrEmpty(Departamento.Password))
            {
                Error = "Contraseña invalida";
            }
            else if (Departamento.Password.Count() < 4)
            {
                Error = "La contraseña deben ser 4 caracteres como minimo";
            }
            else if (Departamentos.Select(x => x.Username).Contains(Departamento.Username))
            {
                Error = "Ya existe un departamento con este nombre de usuario";
            }
            else if (Departamento.IdSuperior == null || Departamento.IdSuperior == 0)
            {
                Error = "Seleccione un departamento superior";
            }
            else
            {
                //Enviar a la api el departamento
                var resp = departamentosService.PostDepartamento(Departamento);
                //Un if para comprobar que sí se agregó
                if (resp)
                {
                    //Navegar hacia atras
                    Shell.Current.Navigation.PopAsync();
                    Shell.Current.Navigation.PopAsync();
                }
            }
        }
    }
}

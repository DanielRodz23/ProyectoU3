using ProyectoU3.ViewModels;

namespace ProyectoU3.Views;

public partial class LoginView : ContentPage
{
	public LoginView(LoginViewModel viewModel)
    {

		InitializeComponent();
		this.BindingContext = viewModel;
        var tkn =  SecureStorage.GetAsync("tkn").Result;
        if (tkn == null) Shell.Current.ToolbarItems.First().IsEnabled = false;
        //CheckTkn();
	}
    async void CheckTkn()
    {
        var tkn = await SecureStorage.GetAsync("tkn");
        if (tkn == null) return;
        await Shell.Current.GoToAsync("//ListaActividadesView");
        Shell.Current.ToolbarItems.First().IsEnabled = true;
        //var Valido = await loginClient.Validar(tkn);
        //if (Valido)
        //{
        //    await Shell.Current.GoToAsync("//ListaActividadesView");
        //    Shell.Current.ToolbarItems.First().IsEnabled = true;
        //}
        //else
        //{
        //    SecureStorage.RemoveAll();
        //}
    }
}
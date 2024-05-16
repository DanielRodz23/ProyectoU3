using ProyectoU3.ViewModels;

namespace ProyectoU3.Views;

public partial class LoginView : ContentPage
{
	public LoginView(LoginViewModel viewModel)
    {

		InitializeComponent();
		this.BindingContext = viewModel;

	}
}
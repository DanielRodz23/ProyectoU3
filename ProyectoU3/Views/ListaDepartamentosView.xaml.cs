using ProyectoU3.ViewModels;

namespace ProyectoU3.Views;

public partial class ListaDepartamentosView : ContentPage
{
	public ListaDepartamentosView()
	{
		InitializeComponent();
		BindingContext = new ListaDepartamentosViewModel();
	}
}
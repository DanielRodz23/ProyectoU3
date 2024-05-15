using ProyectoU3.ViewModels;

namespace ProyectoU3.Views;

public partial class ListActividadesView : ContentPage
{
	public ListActividadesView(ListActividadesViewModel viewModel)
	{
		BindingContext = viewModel;
		InitializeComponent();
	}
}
using ProyectoU3.ViewModels;

namespace ProyectoU3.Views;

public partial class VerDetallesActividadView : ContentPage
{
	public VerDetallesActividadView(DetallesViewModel detallesViewModel)
	{
		InitializeComponent();
		BindingContext = detallesViewModel;
	}
}
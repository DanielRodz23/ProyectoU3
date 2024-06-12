using ProyectoU3.ViewModels;

namespace ProyectoU3.Views;

public partial class AgregarDepartamento : ContentPage
{
	public AgregarDepartamento(AgregarDepartamentoView agregarDepartamentoView)
	{
		InitializeComponent();

		BindingContext = agregarDepartamentoView;
	}
}
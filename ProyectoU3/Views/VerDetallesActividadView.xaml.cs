using ProyectoU3.ViewModels;

namespace ProyectoU3.Views;

public partial class VerDetallesActividadView : ContentPage
{
    private readonly DetallesViewModel detallesViewModel;

    public VerDetallesActividadView(DetallesViewModel detallesViewModel)
	{
		InitializeComponent();
        this.detallesViewModel = detallesViewModel;
		BindingContext = this.detallesViewModel;
    }
}
using ProyectoU3.ViewModels;

namespace ProyectoU3.Views;

public partial class AgregarActividadView : ContentPage
{
    private readonly AgregarActividadViewModel agregarActividadViewModel;

    public AgregarActividadView(AgregarActividadViewModel agregarActividadViewModel)
	{
		InitializeComponent();
        this.agregarActividadViewModel = agregarActividadViewModel;


        BindingContext = this.agregarActividadViewModel;
    }
}
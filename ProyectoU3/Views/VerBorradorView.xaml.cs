using ProyectoU3.ViewModels;

namespace ProyectoU3.Views;

public partial class VerBorradorView : ContentPage
{
    private readonly VerBorradoresModel verBorradoresModel;

    public VerBorradorView(VerBorradoresModel verBorradoresModel)

	{
		InitializeComponent();
        this.verBorradoresModel = verBorradoresModel;
        BindingContext = this.verBorradoresModel;

    }
}
using ProyectoU3.ViewModels;

namespace ProyectoU3.Views;

public partial class ListActividadesView : ContentPage
{
	bool Showed;
	public ListActividadesView(ListActividadesViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}

    private void Button_Clicked(object sender, EventArgs e)
    {
		ShowButton.IsEnabled = false;
        if (!Showed)
        {
            ActButton.TranslateTo(ActButton.tra)
        }

    }
	
}
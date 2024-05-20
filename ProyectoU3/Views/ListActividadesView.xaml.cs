using ProyectoU3.ViewModels;

namespace ProyectoU3.Views;

public partial class ListActividadesView : ContentPage
{
    bool Showed;
    public ListActividadesView(ListActividadesViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;

        //if (Shell.Current.ToolbarItems.Count == 0)
        //{
        //    Shell.Current.ToolbarItems.Add(new ToolbarItem() { IsEnabled = true,  Text = "Cerrar sesion" ,Order=ToolbarItemOrder.Secondary, Priority = 0, Command = (Shell.Current.BindingContext as ShellViewModel).CerrarSesionCommand });
        //}
    }

    

}
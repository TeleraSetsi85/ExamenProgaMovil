using front.Views;

namespace front
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(AgregarPelicula), typeof(AgregarPelicula));
            Routing.RegisterRoute(nameof(MainPage), typeof(MainPage));
            Routing.RegisterRoute(nameof(EditarPelicula), typeof(EditarPelicula));
        }
    }
}

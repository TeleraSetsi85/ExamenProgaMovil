using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace front.Views
{
    public partial class MainPage : ContentPage
    {
        record struct Movie(
            int Id,
            string Title,
            string Genre,
            int Release,
            [property: JsonProperty("image_url")] string ImageUrl
        );

        ObservableCollection<Movie> peliculasFiltradas = new ObservableCollection<Movie>();
        List<Movie> todasLasPeliculas = new List<Movie>();

        HttpClient cliente = new HttpClient { BaseAddress = new Uri("https://5ml2ttlq-3000.usw3.devtunnels.ms/api/movies/") };

        public MainPage()
        {
            InitializeComponent();
            ListaDePeliculas.ItemsSource = peliculasFiltradas;
        }

        private void EntryBuscar_TextChanged(object sender, TextChangedEventArgs e)
        {
            string textoBusqueda = e.NewTextValue?.ToLower() ?? "";
            peliculasFiltradas.Clear();

            var filtradas = string.IsNullOrWhiteSpace(textoBusqueda)
                ? todasLasPeliculas
                : todasLasPeliculas.Where(p => p.Title.ToLower().Contains(textoBusqueda));

            foreach (var peli in filtradas)
                peliculasFiltradas.Add(peli);
        }

        private async Task CargarPeliculas()
        {
            try
            {
                var response = await cliente.GetAsync("");
                if (!response.IsSuccessStatusCode) return;

                string json = await response.Content.ReadAsStringAsync();
                var lista = JsonConvert.DeserializeObject<List<Movie>>(json) ?? new List<Movie>();

                todasLasPeliculas.Clear();
                todasLasPeliculas.AddRange(lista);

                EntryBuscar_TextChanged(EntryBuscar, new TextChangedEventArgs("", EntryBuscar?.Text ?? ""));
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"No se pudieron cargar películas: {ex.Message}", "OK");
            }
        }

        private async void BtnEditar_Clicked(object sender, EventArgs e)
        {
            if (sender is not Button button || button.CommandParameter is not Movie peli) return;

            var parametros = new Dictionary<string, object>
            {
                ["Id"] = peli.Id,
                ["Title"] = peli.Title,
                ["Genre"] = peli.Genre,
                ["Release"] = peli.Release,
                ["ImageUrl"] = peli.ImageUrl
            };

            try
            {
                await Shell.Current.GoToAsync(nameof(EditarPelicula), parametros);
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Error al navegar: {ex.Message}", "OK");
            }
        }

        private async void BtnEliminar_Clicked(object sender, EventArgs e)
        {
            if (sender is not Button button || button.CommandParameter is not Movie peli) return;

            bool respuesta = await DisplayAlert(
                "Confirmar eliminación",
                $"¿Está seguro de eliminar la película '{peli.Title}'?",
                "Sí",
                "No"
            );

            if (!respuesta) return;

            try
            {
                var response = await cliente.DeleteAsync($"{peli.Id}");
                if (response.IsSuccessStatusCode)
                {
                    await DisplayAlert("Éxito", "Película eliminada correctamente", "OK");
                    await CargarPeliculas();
                }
                else
                {
                    await DisplayAlert("Error", "No se pudo eliminar la película", "OK");
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Error al eliminar película: {ex.Message}", "OK");
            }
        }

        private async void BtnAgregarPelicula_Clicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync(nameof(AgregarPelicula));
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await CargarPeliculas();
        }
    }
}

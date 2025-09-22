using System.Collections.ObjectModel;
using System.Text;
using Newtonsoft.Json;

namespace front.Views
{
    public partial class AgregarPelicula : ContentPage
    {
        record struct Movie(int Id, string Title, string Genre, int Release, string ImageUrl);

        ObservableCollection<Movie> peliculas = new ObservableCollection<Movie>();
        HttpClient cliente = new HttpClient { BaseAddress = new Uri("http://localhost:3000/api/movies/") };

        public AgregarPelicula()
        {
            InitializeComponent();
        }

        private async void GuardarPelicula_Clicked(object sender, EventArgs e)
        {
            string titleTxt = titleEntry.Text;
            string genreTxt = genreEntry.Text;
            string releaseTxt = releaseEntry.Text;
            string imageUrlTxt = imageUrlEntry.Text;

            if (string.IsNullOrWhiteSpace(titleTxt) ||
                string.IsNullOrWhiteSpace(genreTxt) ||
                string.IsNullOrWhiteSpace(releaseTxt))
            {
                await DisplayAlert("Error", "Título, género y año son obligatorios", "OK");
                return;
            }

            if (!int.TryParse(releaseTxt, out int release))
            {
                await DisplayAlert("Error", "Año de lanzamiento inválido", "OK");
                return;
            }

            try
            {
                var peliculaObj = new
                {
                    title = titleTxt,
                    genre = genreTxt,
                    release = release,
                    image_url = imageUrlTxt
                };

                var json = JsonConvert.SerializeObject(peliculaObj);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await cliente.PostAsync("", content);

                if (response.IsSuccessStatusCode)
                {
                    await DisplayAlert("Éxito", "Película agregada correctamente", "OK");
                    await Shell.Current.GoToAsync(".."); // Regresa a la lista
                }
                else
                {
                    string errorContent = await response.Content.ReadAsStringAsync();
                    await DisplayAlert("Error", $"No se pudo agregar la película: {errorContent}", "OK");
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Error al agregar película: {ex.Message}", "OK");
            }
        }
    }
}

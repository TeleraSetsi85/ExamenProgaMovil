using System.Text;
using System.Web;
using Newtonsoft.Json;

namespace front.Views
{
    public partial class EditarPelicula : ContentPage, IQueryAttributable
    {
        public int peliculaId;
        HttpClient cliente = new HttpClient { BaseAddress = new Uri("http://localhost:3000/api/movies/") };

        public EditarPelicula()
        {
            InitializeComponent();
        }

        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            if (query.TryGetValue("Id", out var idObj) && idObj != null)
                peliculaId = Convert.ToInt32(idObj);

            if (query.TryGetValue("Title", out var titleObj) && titleObj != null)
                titleEntry.Text = HttpUtility.UrlDecode(titleObj.ToString());

            if (query.TryGetValue("Genre", out var genreObj) && genreObj != null)
                genreEntry.Text = HttpUtility.UrlDecode(genreObj.ToString());

            if (query.TryGetValue("Release", out var releaseObj) && releaseObj != null)
                releaseEntry.Text = HttpUtility.UrlDecode(releaseObj.ToString());

            if (query.TryGetValue("ImageUrl", out var imageObj) && imageObj != null)
                imageUrlEntry.Text = HttpUtility.UrlDecode(imageObj.ToString());
        }

        private async void ActualizarPelicula_Clicked(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(titleEntry.Text) ||
                    string.IsNullOrWhiteSpace(genreEntry.Text) ||
                    string.IsNullOrWhiteSpace(releaseEntry.Text))
                {
                    await DisplayAlert("Error", "Título, género y año son obligatorios", "OK");
                    return;
                }

                if (!int.TryParse(releaseEntry.Text, out int release))
                {
                    await DisplayAlert("Error", "Año de lanzamiento inválido", "OK");
                    return;
                }

                var peliculaObj = new
                {
                    title = titleEntry.Text,
                    genre = genreEntry.Text,
                    release = release,
                    image_url = imageUrlEntry.Text?.Trim() ?? ""
                };

                var json = JsonConvert.SerializeObject(peliculaObj);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await cliente.PutAsync($"{peliculaId}", content);

                if (response.IsSuccessStatusCode)
                {
                    await DisplayAlert("Éxito", "Película actualizada correctamente", "OK");
                    await Shell.Current.GoToAsync("..");
                }
                else
                {
                    string errorContent = await response.Content.ReadAsStringAsync();
                    await DisplayAlert("Error", $"No se pudo actualizar la película: {errorContent}", "OK");
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Error al actualizar la película: {ex.Message}", "OK");
            }
        }

        private async void Cancelar_Clicked(object sender, EventArgs e)
        {
            bool respuesta = await DisplayAlert(
                "Cancelar",
                "¿Está seguro de que desea cancelar? Se perderán los cambios.",
                "Sí",
                "No"
            );

            if (respuesta)
                await Shell.Current.GoToAsync("..");
        }
    }
}

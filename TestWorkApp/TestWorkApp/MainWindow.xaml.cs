using Microsoft.Win32;
using System.Configuration;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Windows;

using Path = System.IO.Path;

namespace TestWorkApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly HttpClient http;
        private string apiUrl;

        public MainWindow()
        {
            ApiConfig config = JsonSerializer.Deserialize<ApiConfig>(File.ReadAllText("config.json"));
            apiUrl = config.ApiKey;
            InitializeComponent();
            http = new HttpClient();
            LoadImages();
        }

        /// <summary>
        /// Загрузка всех изображений.
        /// </summary>
        private async void LoadImages()
        {
            try
            {
                var response = await http.GetStringAsync($"{apiUrl}/api/images/all");

                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };

                var images = JsonSerializer.Deserialize<List<Img>>(response, options);

                if (images == null || images.Count == 0)
                {
                    MessageBox.Show("Нет данных.");
                }

                ImageListBox.ItemsSource = images;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}");
            }
        }

        /// <summary>
        ///  Обработка клика на кнопку "добавить" и выполнение добавления нового изображения.
        /// </summary>
        private async void AddButton_Click(object sender, RoutedEventArgs e)
        {
            var fileDialog = new Microsoft.Win32.OpenFileDialog();

            if (fileDialog.ShowDialog() == true)
            {
                var imagePath = fileDialog.FileName;
                var image = new Img
                {
                    Name = Path.GetFileName(imagePath), 
                    Url = imagePath 
                };

                var json = JsonSerializer.Serialize(image);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await http.PostAsync($"{apiUrl}/api/images/add", content);

                if (response.IsSuccessStatusCode)
                {
                    MessageBox.Show("Изображение успешно добавлено");
                    LoadImages(); 
                }
                else
                {
                    MessageBox.Show("Ошибка при добавлении изображения.");
                }
            }
        }

        /// <summary>
        /// Обработка клика на кнопку "изменить" и выполнение обновления данных.
        /// </summary>
        private async void EditButton_Click(object sender, RoutedEventArgs e)
        {
            var selectedImage = (Img)ImageListBox.SelectedItem;

            if (selectedImage == null)
            {
                MessageBox.Show("Выберите изображение для изменения.");
                return;
            }

            var fileDialog = new OpenFileDialog();

            if (fileDialog.ShowDialog() == true)
            {
                var newPath = fileDialog.FileName;

                var updatedImage = new Img
                {
                    Id = selectedImage.Id,
                    Name = Path.GetFileName(newPath),
                    Url = newPath
                };

                var json = JsonSerializer.Serialize(updatedImage);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await http.PutAsync($"{apiUrl}/api/images/update/{updatedImage.Id}", content);

                if (response.IsSuccessStatusCode)
                {
                    MessageBox.Show("Изображение успешно обновлено.");
                    LoadImages();
                }
                else
                {
                    MessageBox.Show("Ошибка при обновлении изображения.");
                }
            }
        }

        /// <summary>
        /// Обработка клика на кнопку "удалить" и выполнение удаления строки из таблицы.
        /// </summary>
        private async void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            var selectedImage = (Img)ImageListBox.SelectedItem;

            if (selectedImage == null)
            {
                MessageBox.Show("Выберите изображение для удаления.");

                return;
            }

            var response = await http.DeleteAsync($"{apiUrl}/api/images/delete/{selectedImage.Id}");

            if (response.IsSuccessStatusCode)
            {
                MessageBox.Show("Изображение успешно удалено.");
                LoadImages(); 
            }
            else
            {
                MessageBox.Show($"Ошибка при удалении изображения: {response.StatusCode}");
            }
        }

    }

    /// <summary>
    /// Пользовательский класс изображения.
    /// </summary>
    public class Img
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public override string ToString()
        {
            return Name;
        }
    }

    /// <summary>
    /// Пользовательсий класс для хранения хоста.
    /// </summary>
    public class ApiConfig
    {
        public string ApiKey { get; set; }
    }

}
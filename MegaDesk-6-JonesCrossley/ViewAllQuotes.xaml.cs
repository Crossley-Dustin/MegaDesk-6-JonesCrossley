using System;
using System.Collections.Generic;
using System.IO;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.Storage;
using Newtonsoft.Json;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace MegaDesk_6_JonesCrossley
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ViewAllQuotes : Page
    {
        public ViewAllQuotes()
        {
            this.InitializeComponent();
        }

        private void Grid_Loading(FrameworkElement sender, object args)
        {
            DisplayFileContents();
        }

        private async void DisplayFileContents()
        {
            // Read file, display on form.
            List<DeskQuote> list;

            // Read the current JSON file and convert it to a list of quotes.
            StorageFolder folder = Windows.ApplicationModel.Package.Current.InstalledLocation;
            StorageFile file = await folder.GetFileAsync(App.QUOTES_FILE_NAME);
            string readFile = await FileIO.ReadTextAsync(file);

            //string JsonString = File.ReadAllText(App.QUOTES_FILE_NAME);
            list = JsonConvert.DeserializeObject<List<DeskQuote>>(readFile);

            // Read the JSON file.
            JsonSerializer ser = new JsonSerializer();
            string JSONstring = File.ReadAllText(App.QUOTES_FILE_NAME);

            // For now, just display the raw JSON text.
            FileContents.Text += JSONstring;
        }

        private void ReturnToMainMenu_Click(object sender, RoutedEventArgs e)
        {
            // Navigate back to the main menu page.
            this.Frame.Navigate(typeof(MainMenu));
        }
    }
}

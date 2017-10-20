using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.Storage;
using Newtonsoft.Json;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace MegaDesk_6_JonesCrossley
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class SearchAllQuotes : Page
    {
        // Class variables.
        List<DeskQuote> _lstDeskQuotes;

        public SearchAllQuotes()
        {
            this.InitializeComponent();
        }

        private void Grid_Loading(FrameworkElement sender, object args)
        {
            // Load combobox.
            LoadSurfaceDDL();
            // Hide view button.
            ViewQuote.IsEnabled = false;
            // Get list of quotes from file
            GetQuotesFromFile();
        }

        private void LoadSurfaceDDL()
        {
            // Use list of values from Desk enumeration
            List<Desk.DesktopMaterial> materials = Enum.GetValues(typeof(Desk.DesktopMaterial)).Cast<Desk.DesktopMaterial>().ToList();
            SurfaceMaterial.ItemsSource = materials;
        }

        private void ViewQuote_Click(object sender, RoutedEventArgs e)
        {
            // Identify the currently selected quote and send to view page.
            
            // Return back to call if nothing selected.
            if (QuotesList.SelectedIndex < 0)
                return;

            // Prep quote variable with seleted quote.
            DeskQuote quote = (DeskQuote)QuotesList.SelectedItem;

            // Navigate to the Display page
            this.Frame.Navigate(typeof(DisplayQuote), quote);
        }

        private void ReturnToMenu_Click(object sender, RoutedEventArgs e)
        {
            // Navigate back to the main menu page.
            this.Frame.Navigate(typeof(MainMenu));
        }

        private async void GetQuotesFromFile()
        {
            // Define our quote list.
            List<DeskQuote> list;

            // Read the current JSON file and convert it to a list of quotes.
            StorageFolder folder = Windows.ApplicationModel.Package.Current.InstalledLocation;
            StorageFile file = await folder.GetFileAsync(App.QUOTES_FILE_NAME);
            string readFile = await FileIO.ReadTextAsync(file);

            // Deserialize file string into a list of DeskQuote objects.
            list = JsonConvert.DeserializeObject<List<DeskQuote>>(readFile);

            // If there are no quotes in the file, set the list to empty rather than null (we need a reference to the list).
            if (list == null)
                list = new List<DeskQuote>();

            // Save to class variable.
            _lstDeskQuotes = list;
        }

        private void LoadList(List<DeskQuote> quotes)
        {
            // Set List datasource.
            QuotesList.ItemsSource = quotes;
            //QuotesList.DataSource = quotes;
            //QuotesList.DisplayMember = "CustomerName";
            //QuotesList.ValueMember = "QuoteDate";

        }

        private void SurfaceMaterial_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Desk.DesktopMaterial SurfaceText;
            if (Enum.TryParse(SurfaceMaterial.SelectedItem.ToString(), out Desk.DesktopMaterial selectedSurface))
                SurfaceText = selectedSurface;
            else
                return;

            // Generate a filtered list of quotes based on the selected combobox item.
            List<DeskQuote> filteredList = new List<DeskQuote>();

            foreach (DeskQuote quote in _lstDeskQuotes)
            {
                if (quote.Desk.Surface == SurfaceText)
                {
                    filteredList.Add(quote);
                }
            }

            // Load Listview based on filtered list (show the user what we found).
            LoadList(filteredList);

            // Enable view button
            ViewQuote.IsEnabled = (QuotesList.Items.Count > 0);
        }
    }
}

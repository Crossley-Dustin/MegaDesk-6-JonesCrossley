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
using Newtonsoft.Json;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace MegaDesk_6_JonesCrossley
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class AddQuote : Page
    {
        DeskQuote DQuote = new DeskQuote();

        public AddQuote()
        {
            this.InitializeComponent();

            // Load the comboxes
            LoadSurfaceDDL();
        }

        private void CancelReturn_Click(object sender, RoutedEventArgs e)
        {
            // Navigate back to the main menu page.
            this.Frame.Navigate(typeof(MainMenu));
        }

        private void SaveReturn_Click(object sender, RoutedEventArgs e)
        {
            // Save all the details
            // Grab all values and generate the price quote

            DQuote.Desk.Width = int.Parse(DeskWidth.Text);
            DQuote.Desk.Depth = int.Parse(DeskDepth.Text);
            DQuote.Desk.DrawerCount = Convert.ToInt32(NumberDrawers.SelectedItem);
            string selectedComboSurface = SurfaceMaterial.SelectedItem.ToString();
            bool comboSurfaceConverted;
            comboSurfaceConverted = Enum.TryParse(selectedComboSurface, out Desk.DesktopMaterial selectedSurface);
            if (comboSurfaceConverted)
            {
                DQuote.Desk.Surface = selectedSurface;
            }

            DQuote.CustomerName = CustomerName.Text;
            DQuote.RushOrderDays = (DeskQuote.RushDays)Convert.ToInt32(RushDays.SelectedItem.ToString());

            // Calculate the quote
            DQuote.CalculateDeskQuote();

            // Save details to file
            SaveToQuoteFile();

            // Navigate back to the main menu page.
            this.Frame.Navigate(typeof(MainMenu));
        }

        private void LoadSurfaceDDL()
        {
            // Use list of values from Desk enumeration
            List<Desk.DesktopMaterial> materials = Enum.GetValues(typeof(Desk.DesktopMaterial)).Cast<Desk.DesktopMaterial>().ToList();
            SurfaceMaterial.ItemsSource = materials;
        }

        private bool SaveToQuoteFile()
        {
            // Save current quote details to a file

            try
            {
                List<DeskQuote> list;

                // Read the current JSON file and convert it to a list of quotes.
                string JsonString = File.ReadAllText(App.QUOTES_FILE_NAME);
                list = JsonConvert.DeserializeObject<List<DeskQuote>>(JsonString);

                // If there are no quotes in the file, set the list to empty rather than null (we need a reference to the list).
                if (list == null)
                {
                    list = new List<DeskQuote>();
                }

                // Add the current quote to the list
                list.Add(DQuote);
                // Serialize the entire list to a string. Format it so it looks purty.
                string outputJSON = JsonConvert.SerializeObject(list, Formatting.Indented);
                // Write all the text back out to the JSON file. We are not appending. We are overwriting the entire file.
                File.WriteAllText(App.QUOTES_FILE_NAME, outputJSON);
            }
            catch (Exception e)
            {
                throw e;
            }
            return true;



        }
    }
}

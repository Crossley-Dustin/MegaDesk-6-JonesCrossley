﻿using System;
using System.Collections.Generic;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Newtonsoft.Json;
using Windows.Storage;

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
            if (NumberDrawers.Items[NumberDrawers.SelectedIndex] is ComboBoxItem NumDrawersItem)
                DQuote.Desk.DrawerCount = Convert.ToInt32(NumDrawersItem.Content.ToString());
            if (SurfaceMaterial.Items[SurfaceMaterial.SelectedIndex] is ComboBoxItem SurfaceItem)
            {
                string selectedComboSurface = SurfaceItem.Content.ToString();
                bool comboSurfaceConverted;
                comboSurfaceConverted = Enum.TryParse(selectedComboSurface, out Desk.DesktopMaterial selectedSurface);
                if (comboSurfaceConverted)
                {
                    DQuote.Desk.Surface = selectedSurface;
                }
            }
            

            DQuote.CustomerName = CustomerName.Text;
            if (RushDays.Items[RushDays.SelectedIndex] is ComboBoxItem RushOrder)
                DQuote.RushOrderDays = (DeskQuote.RushDays)Convert.ToInt32(RushOrder.Content.ToString());

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

        private async void SaveToQuoteFile()
        {
            // Save current quote details to a file

            try
            {
                List<DeskQuote> list;

                // Read the current JSON file and convert it to a list of quotes.
                StorageFolder folder = Windows.ApplicationModel.Package.Current.InstalledLocation;
                StorageFile file = await folder.GetFileAsync(App.QUOTES_FILE_NAME);
                string readFile = await FileIO.ReadTextAsync(file);

                //string JsonString = File.ReadAllText(App.QUOTES_FILE_NAME);
                list = JsonConvert.DeserializeObject<List<DeskQuote>>(readFile);

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
                await FileIO.WriteTextAsync(file, outputJSON);
                //File.WriteAllText(App.QUOTES_FILE_NAME, outputJSON);

            }
            catch (Exception e)
            {
                throw e;
            }
        }

        private void TextBlock_SelectionChanged(object sender, RoutedEventArgs e)
        {

        }

        private void TextBlock_SelectionChanged_1(object sender, RoutedEventArgs e)
        {

        }
    }
}

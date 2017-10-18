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

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace MegaDesk_6_JonesCrossley
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainMenu : Page
    {
        public MainMenu()
        {
            this.InitializeComponent();
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            // Exit the app.
            App.Current.Exit();
        }

        private void AddNewQuote_Click(object sender, RoutedEventArgs e)
        {
            // Navigate back to the Add Quote page.
            this.Frame.Navigate(typeof(AddQuote));
        }

        private void ViewQuotes_Click(object sender, RoutedEventArgs e)
        {
            // Navigate back to the view all quotes page.
            this.Frame.Navigate(typeof(ViewAllQuotes));
        }

        private void SearchQuotes_Click(object sender, RoutedEventArgs e)
        {
            // Navigate back to the search quotes page.
            this.Frame.Navigate(typeof(SearchAllQuotes));
        }
    }
}

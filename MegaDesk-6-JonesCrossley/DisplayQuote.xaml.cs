using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace MegaDesk_6_JonesCrossley
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class DisplayQuote : Page
    {
        // Class variables.
        DeskQuote _Quote;

        public DisplayQuote()
        {
            this.InitializeComponent();
        }

        private void ReturnToSearch_Click(object sender, RoutedEventArgs e)
        {
            // Navigate to the quote search page.
            this.Frame.Navigate(typeof(SearchAllQuotes));
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            // Cast parameter to DeskQuote object and save to local variable.
            _Quote = (DeskQuote)e.Parameter;
        }

        private void DisplayCurrentQuote()
        {
            CustomerName.Text = _Quote.CustomerName;
            QuoteDate.Text = _Quote.QuoteDate.ToString();
            QuoteAmount.Text = _Quote.QuoteAmount.ToString("C");
            RushDays.Text = Convert.ToString((int)_Quote.RushOrderDays);
            NumberDrawers.Text = _Quote.Desk.DrawerCount.ToString();
            DeskWidth.Text = _Quote.Desk.Width.ToString();
            DeskDepth.Text = _Quote.Desk.Depth.ToString();
            SurfaceMaterial.Text = _Quote.Desk.Surface.ToString();
        }

        private void Grid_Loading(FrameworkElement sender, object args)
        {
            DisplayCurrentQuote();
        }
    }
}

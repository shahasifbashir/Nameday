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

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace NameDay
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }

        private async void email_button_Click(object sender, RoutedEventArgs e)
        {
            //Here we cast the senders data contactEx class type
            var contact  = ((FrameworkElement)sender).DataContext as ContactEx;
            //here the class is made diretctly to the SendEmailAsync by casting the data context as the type of namespace data
            if (contact != null)
                await ((Namedaydata)this.DataContext).SendEmailAsync(contact.Contact);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Contacts;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

namespace NameDay
{   
    /// <summary>
    ///This class acts as a model for the contacts that will be retrived from the user contact list
    /// we here use the first name, last name,contact picture and the email
    /// also the email button visibility is controlled from here 
    /// </summary>
    public class ContactEx
    {
        public Contact Contact { get; }
        public ContactEx(Contact contact)
        {
            Contact = contact;
        }
        public ContactEx(string firstName,string lastName)
        {
            Contact = new Contact
            {
                FirstName = firstName,
                LastName = lastName
            };
        }
        public string initials => 
            GetFirstCharacter(Contact.FirstName) + GetFirstCharacter(Contact.LastName);
        private string GetFirstCharacter(string s) => 
            string.IsNullOrEmpty(s) ? "" : s.Substring(0, 1);
        //This property checks the visibilty of the email button
        public Visibility EmailVisibility =>
            DesignMode.DesignModeEnabled || 
            Contact.Emails.Count > 0 ? Visibility.Visible : Visibility.Collapsed;
        public ImageBrush picture
        {
            get
            {
                if (Contact.SmallDisplayPicture == null)
                    return null;
                var image = new BitmapImage();
                image.SetSource(Contact.SmallDisplayPicture.OpenReadAsync().GetAwaiter().GetResult());
                return new ImageBrush { ImageSource = image };

            }
        }
    }
}

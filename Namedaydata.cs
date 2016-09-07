using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using Windows.ApplicationModel.Contacts;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Email;
using Windows.ApplicationModel.Appointments;

namespace NameDay
{
    /// <summary>
    /// This class acts as the data source for the application
    /// </summary>
    public class Namedaydata : ObservableObject
    {
        public Settings settings { get; } = new Settings();
        private string _greeting = "Hello World!";
        public string greeting
        {
            get
            {
                return _greeting;
            }
            set
            { Set(ref _greeting, value); }
        }
        // this list will hold the names of all the people in nameday
        private List<NamedarModel> _Namedays = new List<NamedarModel>();
        // this is the list which will be displayed and is acted upon by the search box
        public ObservableCollection<NamedarModel> Namedays { get; set; }
        public ObservableCollection<ContactEx> Contacts { get; }
            = new ObservableCollection<ContactEx>();
        public Namedaydata()
        {
            AddReminderCommand = new AddRemindercommand(this);
            Namedays = new ObservableCollection<NamedarModel>();
            if (Windows.ApplicationModel.DesignMode.DesignModeEnabled)
            {
                Contacts = new ObservableCollection<ContactEx>
                            {
                                new ContactEx("contact","1"),
                                new ContactEx("contact","2"),
                                new ContactEx("contact","3"),
                                new ContactEx("contact","4")
                             };

                for (int month = 1; month <= 12; month++)
                {
                    _Namedays.Add(new NamedarModel(month, 1, new string[] { "Asif" }));
                    _Namedays.Add(new NamedarModel(month, 24, new string[] { "bashir", "Shah" }));
                }
                performFiltering();
            }
            else
                LoadData();
        }

        private async void LoadData()
        {
            _Namedays = await NamedayRepository.GetallNamedays();

            performFiltering();
        }

        private NamedarModel _selectedNameday;
        //this is the event handle in inotify interface that tell the framework that a specific thing has changeds

        public NamedarModel selectedNameday
        {
            get { return _selectedNameday; }
            set
            {
                _selectedNameday = value;
                if (value == null)
                    greeting = "Hello World!";
                else
                    greeting = "Hello " + value.nameAsString;

                UpdateContacts();
                AddReminderCommand.FireCanExecuteChanged();
            }
        }

        private async void UpdateContacts()
        {
            Contacts.Clear();
            if (selectedNameday != null)
            {
                var contactStore = await ContactManager.RequestStoreAsync(ContactStoreAccessType.AllContactsReadOnly);
                foreach (var name in selectedNameday.Names)
                    foreach (var contact in await contactStore.FindContactsAsync(name))
                        Contacts.Add(new ContactEx(contact));
            }
        }

        private string _filter;

        public string filter
        {
            get { return _filter; }
            set
            {
                if (Set(ref _filter, value))
                    performFiltering();
                   
               
            }
        }
        // this the filtering logic
        private void performFiltering()
        {
            if (_filter == null)
                _filter = "";
            var newfilter = filter.ToLower().Trim();
            var result = _Namedays.Where(d => d.nameAsString.ToLowerInvariant().Contains(newfilter)).ToList();
            var toremove = _Namedays.Except(result).ToList();
            foreach (var x in result)
                Namedays.Remove(x);
            var resultcount = result.Count();
            for (int i = 0; i < resultcount; i++)
            {
                var resultItem = result[i];
                if (i + 1 > Namedays.Count || !Namedays[i].Equals(resultItem))
                    Namedays.Insert(i, resultItem);
            }
        }
        //this function will make the send email dialog box avaliable
        public async Task SendEmailAsync(Contact contact)
        {
            if (contact == null || contact.Emails.Count == 0)
                return;
            var msg = new EmailMessage();
            msg.To.Add(new EmailRecipient(contact.Emails[0].Address));
            msg.Subject = "Happy nameday";
            await EmailManager.ShowComposeNewEmailAsync(msg);
        }
        public AddRemindercommand AddReminderCommand { get; }
        //Name day appoint ment manager
        public async void AddReminderToCalander()
        {
            var appointment = new Appointment();
            appointment.Subject = "Nameday reminder for " + selectedNameday.nameAsString;
            appointment.AllDay = true;
            appointment.BusyStatus = AppointmentBusyStatus.Free;
            var dateThisYear = new DateTime
            (
                DateTime.Now.Year, selectedNameday.Month, selectedNameday.Day
            );
            appointment.StartTime = dateThisYear < DateTime.Now ? dateThisYear.AddYears(1) : dateThisYear;
            await AppointmentManager.ShowEditNewAppointmentAsync(appointment);
        }
    }
    public class AddRemindercommand : System.Windows.Input.ICommand
    {
        private Namedaydata _nmd;
        public AddRemindercommand(Namedaydata nmd)
        {
            _nmd = nmd;
        }
        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter) => _nmd.selectedNameday != null;


        public void Execute(object parameter) => _nmd.AddReminderToCalander();
        public void FireCanExecuteChanged() => CanExecuteChanged?.Invoke(this, EventArgs.Empty);

    }
}

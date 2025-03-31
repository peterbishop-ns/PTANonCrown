using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using PTANonCrown.Services;

namespace PTANonCrown.ViewModel
{
    public class BaseViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private readonly HelpService _helpService = new HelpService();



        public ICommand ShowHelperTextCommand => new Command<string>((fieldName) =>
        {
            string helpText = _helpService.GetHelpText(fieldName);

            Application.Current.MainPage.DisplayAlert(fieldName, helpText, "OK");
        });

    }
}

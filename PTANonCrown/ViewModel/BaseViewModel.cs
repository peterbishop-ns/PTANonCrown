using PTANonCrown.Services;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace PTANonCrown.ViewModel
{
    public class BaseViewModel : INotifyPropertyChanged
    {
        private readonly HelpService _helpService = new HelpService();

        public event PropertyChangedEventHandler PropertyChanged;

        public ICommand ShowHelperTextCommand => new Command<string>((fieldName) =>
        {
            string helpText = _helpService.GetHelpText(fieldName);

            Application.Current.MainPage.DisplayAlert(fieldName, helpText, "OK");
        });

        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            Debug.WriteLine($"Property changed: {propertyName}");

        }

        protected bool SetProperty<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
        {
            if (!EqualityComparer<T>.Default.Equals(field, value))
            {
                field = value;
                OnPropertyChanged();
                return true;
            }
            return false;
        }
    }
}
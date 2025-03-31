using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using CommunityToolkit.Maui;

namespace PTANonCrown.ViewModel
{
    class AppShellViewModel
    {
        public ICommand LoadCommand { get; }

        public AppShellViewModel()
        {
            LoadCommand = new Command(Load);
        }

        private async void Load()
        {
        }
    }
}

using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BLEConexion
{
    public partial class App : Application
    {
        public static ViewModel.Viewmodel VMbinder;
        public App()
        {
            InitializeComponent();

            VMbinder = new ViewModel.Viewmodel();
            MainPage = new NavigationPage(new Views.Connect(VMbinder));
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}

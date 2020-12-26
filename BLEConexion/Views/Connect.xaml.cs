using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BLEConexion.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Connect : ContentPage
    {
        public Connect(ViewModel.Viewmodel vMbinder)
        {
            InitializeComponent();
            BindingContext = vMbinder;
        }
    }
}
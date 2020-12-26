using Plugin.BLE.Abstractions.Contracts;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace BLEConexion.Model
{
   public class BLEModel : INotifyPropertyChanged
    {

        public event PropertyChangedEventHandler PropertyChanged;

        private string _statusBLE;
        public string StatusBLE
        {
            get
            {
                return _statusBLE;
            }
            set
            {
                _statusBLE = value;

                OnPropertyChanged("");
            }
        }

        private ObservableCollection<IDevice> _DeviceList;
        public ObservableCollection<IDevice> DeviceList
        {
            get
            {
                return _DeviceList;
            }
            set
            {
                _DeviceList = value;
                OnPropertyChanged("");
            }
        }


        private IDevice _ConnectedDevice;
        public IDevice ConnectedDevice
        {
            get
            {
                return _ConnectedDevice;
            }
            set
            {
                _ConnectedDevice = value;
                OnPropertyChanged("");
            }
        }


        private ObservableCollection<IService> _ServiceList;
        public ObservableCollection<IService> ServiceList
        {
            get
            {
                return _ServiceList;
            }
            set
            {
                _ServiceList = value;
                OnPropertyChanged("");
            }
        }


        private IService _SelectedService;
        public IService SelectedService
        {
            get
            {
                return _SelectedService;
            }
            set
            {
                _SelectedService = value;
                OnPropertyChanged("");
            }
        }

        private ObservableCollection<ICharacteristic> _CharacteristicsList;
        public ObservableCollection<ICharacteristic> CharacteristicsList
        {
            get
            {
                return _CharacteristicsList;
            }
            set
            {
                _CharacteristicsList = value;
                OnPropertyChanged("");
            }
        }


        private ICharacteristic _SelectedCharacteristic;
        public ICharacteristic SelectedCharacteristic
        {
            get
            {
                return _SelectedCharacteristic;
            }
            set
            {
                _SelectedCharacteristic = value;
                OnPropertyChanged("");
            }
        }

        private void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}

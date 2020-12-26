using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using Plugin.BLE;
using Plugin.BLE.Abstractions.Contracts;
using Plugin.BLE.Abstractions.Exceptions;
using Xamarin.Essentials;
using Xamarin.Forms;
using BLEConexion.Model;


namespace BLEConexion.ViewModel
{
    public class Viewmodel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private BLEModel _modelBLE;
        public BLEModel modelBLE
        {
            get
            {
                return _modelBLE;
            }
            set
            {
                _modelBLE = value;
                OnPropertyChanged("");
            }
        }
        private bool _Isbusy;
        public bool Isbusy
        {
            get => _Isbusy;
            set
            {

                _Isbusy = value;
                OnPropertyChanged("");
            }

        }
        private bool _IsConnect;
        public bool IsConnect
        {
            get => _IsConnect;
            set
            {

                _IsConnect = value;
                OnPropertyChanged("");
            }

        }
        private IAdapter bleAdapter;
        private IBluetoothLE bleHandler;

        public Viewmodel()
        {

            modelBLE = new BLEModel
            {
                StatusBLE = "",
                CharacteristicsList = new System.Collections.ObjectModel.ObservableCollection<ICharacteristic>(),
                DeviceList = new System.Collections.ObjectModel.ObservableCollection<IDevice>(),
                ServiceList = new System.Collections.ObjectModel.ObservableCollection<IService>()


            };
            bleHandler = CrossBluetoothLE.Current;
            bleAdapter = CrossBluetoothLE.Current.Adapter;

            bleHandler.StateChanged += (sender, args) =>
            {
                modelBLE.StatusBLE = $"Bluetooth Status: {args.NewState}";
            };
           
            
            bleAdapter.ScanMode = ScanMode.LowPower;
            bleAdapter.ScanTimeout = 8000; 
            
            
            bleAdapter.ScanTimeoutElapsed += (sender, args) =>
            {
                System.Diagnostics.Debug.WriteLine("The scan process has finished");
                modelBLE.StatusBLE = $"Bluetooth Status: {bleHandler.State}";
            };
            
            
            bleAdapter.DeviceDiscovered += (sender, args) =>
            {
                System.Diagnostics.Debug.WriteLine("A device has been discovered");
                IDevice dispositivoDescubierto = args.Device;

                modelBLE.DeviceList.Add(dispositivoDescubierto);

            };
            modelBLE.StatusBLE = "Ready...";
        }

        private Command _cmdConnect;
        public Command cmdConnect
        {

            get
            {
                if (_cmdConnect == null)
                {
                    _cmdConnect = new Command(async () =>
                    {
                       
                        Isbusy = true;
                        
                        if (bleHandler.IsOn)
                        {
                            try
                            {

                                if (!bleAdapter.IsScanning)
                                {
                                    System.Diagnostics.Debug.WriteLine("Starting Scan");
                                    modelBLE.StatusBLE = "Scanning";
                                    modelBLE.DeviceList.Clear();
                                    await bleAdapter.StartScanningForDevicesAsync();
                                }

                            }
                            catch (System.Exception ex)
                            {
                                await App.Current.MainPage.DisplayAlert("", ex.Message, "Ok");

                            }


                            foreach (IDevice devices in modelBLE.DeviceList)
                            {
                                if (devices.Name == "HMSoft") //Change to the name of the known device.
                                {
                                    try
                                    {
                                        Isbusy = false;
                                        await bleAdapter.ConnectToDeviceAsync(devices);
                                        modelBLE.ConnectedDevice = devices;
                                        IsConnect = true;
                                        await App.Current.MainPage.DisplayAlert("Device  Connect", "", "Ok");
                                        
                                    }
                                    catch (DeviceConnectionException ex)
                                    {
                                        await App.Current.MainPage.DisplayAlert("Device Not Connect", ex.Message, "Ok");
                                    }
                                    catch (Exception ex)
                                    {
                                        //generic
                                    }


                                }

                            }
                            modelBLE.ServiceList.Clear();
                            if (IsConnect)
                            {
                                foreach (IService service in await modelBLE.ConnectedDevice.GetServicesAsync()) 
                                {
                                    if (service.Name == "TI SensorTag Smart Keys") //Change to the name of the known device' service. 
                                    {

                                        modelBLE.SelectedService = service;

                                    }

                                }


                                modelBLE.StatusBLE = $"Estado del bluetooth: {bleHandler.State}";


                                foreach (ICharacteristic characteristic in await modelBLE.SelectedService.GetCharacteristicsAsync())
                                {
                                    modelBLE.CharacteristicsList.Add(characteristic);
                                }

                               
                            }
                            else
                            {
                                await App.Current.MainPage.DisplayAlert("Not connected", "The device is turned off", "Ok");
                                Isbusy = false;
                                return;

                            }

                        }
                        else
                        {
                            await App.Current.MainPage.DisplayAlert("Bluetooth not available", "Turn on bluetooth", "Ok");
                            Isbusy = false;
                        }

                    });

                }

                return _cmdConnect;
            }
        }

        private Command _CmdDisconnect;
        public Command CmdDisconnect
        {

            get
            {
                if (_CmdDisconnect == null)
                {
                    _CmdDisconnect = new Command(async () =>
                    {
                      
                       // example to send a char through BLE 
                       /* string input = "O";
                        byte[] sendcommand = Encoding.UTF8.GetBytes(input);
                        var Charact = await modelBLE.SelectedService.GetCharacteristicAsync(Guid.Parse("0000ffe1-0000-1000-8000-00805f9b34fb")); //Characteristic of the selected service 
                        await Charact.WriteAsync(sendcommand);*/ 


                        try
                        {
                            await bleAdapter.DisconnectDeviceAsync(modelBLE.ConnectedDevice);


                            

                           
                            await App.Current.MainPage.DisplayAlert("Device Disconnect", "", "Ok");


                        }
                        catch (DeviceConnectionException ex)
                        {

                            await App.Current.MainPage.DisplayAlert("Device Not Disconnect", ex.Message, "Ok");
                        }
                        catch (Exception ex)
                        {
                            //generic 
                        }

                    });

                }


                return _CmdDisconnect;
            }

        }
        public void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

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

//chris
using Newtonsoft.Json.Linq;
using Microsoft.Azure.Devices.Client;
using Newtonsoft.Json;
using System.Text;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace IoTDeviceRegistration
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {

        public const string DeviceID = "Your Device ID "; 

        public MainPage()
        {
           
            this.InitializeComponent();
        }

        private static void AssignDeviceProperties(string deviceId, dynamic device)
        {
            dynamic deviceProperties = DeviceSchemaHelper.GetDeviceProperties(device);
            deviceProperties.HubEnabledState = true;
            deviceProperties.Manufacturer = "Hancom MDS Inc."; 
            deviceProperties.ModelNumber = "MD-112"; 
            deviceProperties.SerialNumber = "SER8628";    
            deviceProperties.FirmwareVersion = "1.46";  
                                                         
            deviceProperties.Latitude = 37.399890; 
            deviceProperties.Longitude = 127.101356;  
        }

        public static dynamic BuildDeviceStructure(string deviceId, bool isSimulated)
        {
            JObject device = new JObject();

            JObject deviceProps = new JObject();
            deviceProps.Add(DevicePropertiesConstants.DEVICE_ID, deviceId);
            deviceProps.Add(DevicePropertiesConstants.HUB_ENABLED_STATE, null);
            deviceProps.Add(DevicePropertiesConstants.CREATED_TIME, DateTime.UtcNow);
            deviceProps.Add(DevicePropertiesConstants.DEVICE_STATE, "normal");
            deviceProps.Add(DevicePropertiesConstants.UPDATED_TIME, null);

            device.Add(DeviceModelConstants.DEVICE_PROPERTIES, deviceProps);
            device.Add(DeviceModelConstants.COMMANDS, new JArray());
            device.Add(DeviceModelConstants.COMMAND_HISTORY, new JArray());
            device.Add(DeviceModelConstants.IS_SIMULATED_DEVICE, isSimulated);

            device.Add(DeviceModelConstants.VERSION, "1.0");
            device.Add(DeviceModelConstants.OBJECT_TYPE, "DeviceInfo"); 

            return device;
        }

        private static void AssignCommands(dynamic device)
        {
            dynamic command = CommandSchemaHelper.CreateNewCommand("PingDevice");
            CommandSchemaHelper.AddCommandToDevice(device, command);

            command = CommandSchemaHelper.CreateNewCommand("StopTelemetry");
            CommandSchemaHelper.AddCommandToDevice(device, command);

            command = CommandSchemaHelper.CreateNewCommand("ChangeSetPointTemp");
            CommandSchemaHelper.DefineNewParameterOnCommand(command, "SetPointTemp", "double");
            CommandSchemaHelper.AddCommandToDevice(device, command);

            command = CommandSchemaHelper.CreateNewCommand("DiagnosticTelemetry");
            CommandSchemaHelper.DefineNewParameterOnCommand(command, "Active", "boolean");
            CommandSchemaHelper.AddCommandToDevice(device, command);

            command = CommandSchemaHelper.CreateNewCommand("ChangeDeviceState");
            CommandSchemaHelper.DefineNewParameterOnCommand(command, "DeviceState", "string");
            CommandSchemaHelper.AddCommandToDevice(device, command);

            command = CommandSchemaHelper.CreateNewCommand("TurnOnTheLight");
            CommandSchemaHelper.AddCommandToDevice(device, command);

            command = CommandSchemaHelper.CreateNewCommand("TurnOffTheLight");
            CommandSchemaHelper.AddCommandToDevice(device, command);
        }

        private async void RegistrationBtn_Click(object sender, RoutedEventArgs e)
        {
            DeviceClient deviceClient = DeviceClient.CreateFromConnectionString("Your Device Connection String", TransportType.Http1);

            dynamic device = BuildDeviceStructure(DeviceID, false);

            AssignDeviceProperties(DeviceID, device);
            AssignCommands(device);

            var messageString = JsonConvert.SerializeObject(device);
            var message = new Message(Encoding.ASCII.GetBytes(messageString));

            await deviceClient.SendEventAsync(message);
        }

        private static int GetIntBasedOnString(string input, int maxValueExclusive)
        {
            int hash = input.GetHashCode();

            //Keep the result positive
            if (hash < 0)
            {
                hash = -hash;
            }

            return hash % maxValueExclusive;
        }
    }
}

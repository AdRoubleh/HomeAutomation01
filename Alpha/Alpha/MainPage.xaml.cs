using System;
using System.Threading.Tasks;
using Windows.Media.SpeechRecognition;
using Windows.UI;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.Devices.Gpio;
// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace Alpha
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
         GpioPin _led ,led1;
    

        public MainPage()
        {
            this.InitializeComponent();
            var gpio = GpioController.GetDefault();

            if (gpio == null)
            {
                return;
            }
            _led = gpio.OpenPin(26);
             led1 = gpio.OpenPin(19);
            _led.SetDriveMode(GpioPinDriveMode.Output);
             led1.SetDriveMode(GpioPinDriveMode.Output);
            _led.Write(GpioPinValue.Low);
             led1.Write(GpioPinValue.Low);

        }


        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            await ListenAsync();
        }

        private async Task ListenAsync()
        {
            while (true)
            {
                // Create an instance of SpeechRecognizer.
                var speechRecognizer = new SpeechRecognizer();

                string[] responses = { "turn off the fan", "turn on the fan", "turn off the light", "turn on the light" };


                var listConstraint = new SpeechRecognitionListConstraint(responses);


                speechRecognizer.Constraints.Add(listConstraint);
                // Compile the dictation grammar by default.
                await speechRecognizer.CompileConstraintsAsync();

                // Start recognition.
                SpeechRecognitionResult speechRecognitionResult = await speechRecognizer.RecognizeAsync();

                if (speechRecognitionResult.Text == "turn on the fan")
                {
                    circle.Fill = new SolidColorBrush(Colors.Blue);
                    _led?.Write(GpioPinValue.High);

                }
                else if (speechRecognitionResult.Text == "turn off the fan")
                {
                    circle.Fill = new SolidColorBrush(Colors.Red);
                    _led?.Write(GpioPinValue.Low);
                }
                else if (speechRecognitionResult.Text == "turn on the light")
                {
                    circle.Fill = new SolidColorBrush(Colors.Blue);
                    led1?.Write(GpioPinValue.High);
                }
                else if (speechRecognitionResult.Text == "turn off the light")
                {
                    circle.Fill = new SolidColorBrush(Colors.Red);
                    led1?.Write(GpioPinValue.Low);
                }

            }
                
        }

       
    }
}

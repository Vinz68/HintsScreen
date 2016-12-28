using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using System.Threading.Tasks;
using VibeSoft.WinIoTSolution.WebEventDistributor;
using System;
using Windows.UI.Core;


// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace HintsScreen
{

    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private WebListener webListener;
        private WebHintEventArgs webHint;

        public MainPage()
        {
            this.InitializeComponent();
        }


        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            // Defaults after startup/boot.
            this.titleImage.Opacity = 0.9;    // make image invisible
            this.hintImage.Opacity = 0.0;     

            this.hintTextBlock.Text = "Hint:\nKeep your eyes open when solving a puzzle, this might help. ";
            this.hintTextBlock.Opacity = 0.7;

            await InitializeWebServer();
        }


        private async Task InitializeWebServer()
        {

            webListener = new WebListener();
            await webListener.Run(8888);

            // Connect a handler for "Incoming Hints" 
            WebEventDistributor.Instance.RaiseWebHintEvent += HandleWebHintEvent;

            // Don't release deferral, otherwise app will stop 
        }


        // Define what actions to take when the event is raised.
        async void HandleWebHintEvent(object sender, WebHintEventArgs e)
        {
            webHint = e;

            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                if (e.Time >= 0)
                {
                    this.titleImage.Opacity = 0.9;      // Show title
                    this.hintImage.Opacity = 0.0;       // Hide hint image
                    this.hintTextBlock.Opacity = 0.7;   // Show the (new) hint
                    this.hintTextBlock.Text = e.Hint;
                    // TODO: use the e.Time to display the hint for a certain time (in Seconds).
                    // For now; just send a time= -1 to hide the hint.
                }
                else
                {
                    this.titleImage.Opacity = 0.0;      // Hide title
                    this.hintImage.Opacity = 0.0;       // Hide hint image
                    this.hintTextBlock.Opacity = 0.0;   // Hide the hint
                    this.hintTextBlock.Text = "";
                }
            });
        }


        private void UpdateUI()
        {
            throw new NotImplementedException();
        }
    }
}


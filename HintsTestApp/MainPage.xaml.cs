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
using Windows.Web.Http;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace HintsTestApp
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private readonly Uri baseUri = new Uri("http://Win10-PI3:8888/");

        public MainPage()
        {
            this.InitializeComponent();
        }


        private Uri GetAbsoluteUri(string relativeUri)
        {
            return new Uri(baseUri + relativeUri.TrimStart('/'));
        }


        private async void sendButton_Click(object sender, RoutedEventArgs e)
        {
            if ( (string.IsNullOrEmpty(this.timeInSecTextBox.Text)) || (string.IsNullOrEmpty(this.hintTextBox.Text)) )
            {
                this.responseTextBox.Text = " Enter a number in the 'id' field and a text in the ' property name' field ";
                return;
            }

            string answer = "[No Answer]";
            try
            {
                // Compose and display the URI
                // Syntax: http://{computerName}:{port}/api/hint/{timeInSec}/{hintText}
                string relativeUri = "/api/hint/" + this.timeInSecTextBox.Text + "/" + this.hintTextBox.Text;
                Uri uri = GetAbsoluteUri(relativeUri);
                this.uriTextBox.Text = uri.ToString();

                // Send WebRequest to remote webserver
                HttpClient client = new HttpClient();
                HttpResponseMessage response = await client.GetAsync(uri);

                answer = await response.Content.ReadAsStringAsync();

                if (string.IsNullOrEmpty(answer))
                {
                    answer = response.ReasonPhrase;
                }
            }
            catch (Exception ex)
            {
                answer = "ERROR:" + ex.Message.ToString();
            }
            finally
            {
                this.responseTextBox.Text = answer;
            }
        }
    }
}

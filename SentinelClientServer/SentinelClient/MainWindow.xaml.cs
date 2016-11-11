using System;
using System.Windows;
using SentinelClient.SentinelServerReference;
    /// <summary>
    /// Created by Mark Brundieck; July 2015
    /// </summary>
namespace SentinelClient
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private SentinelServiceClient server;
        public MainWindow()
        {
            InitializeComponent();
            server = new SentinelServiceClient();
        }
        /// <summary>
        /// Send the radius value on the lost focus event.
        /// </summary>
        private void txtRadius_LostFocus(object sender, RoutedEventArgs e)
        {
            // ideally send to server side when enter is received or tab off
            // make sure we have a number to work with
            if (!string.IsNullOrEmpty(txtRadius.Text.Trim()))
            {
                double radiusInput;
                if (double.TryParse(txtRadius.Text.Trim(), out radiusInput))
                {
                    try
                    {
                        AppendToScreenOutput(String.Format("Radius is {0}", radiusInput.ToString()));
                        var result = server.GetData(radiusInput.ToString());
                        AppendToScreenOutput(result);
                    }
                    catch (System.ServiceModel.EndpointNotFoundException badEndpoint)
                    {
                        AppendToScreenOutput("Sentinel server could not be found. It may need to be started.");
                    }
                    catch (System.Net.WebException badConnection)
                    {
                        AppendToScreenOutput(string.Format("Unable to connect to the Sentinel server. Error: {0}", badConnection.Message));
                    }
                    catch (Exception ex)
                    {
                        AppendToScreenOutput(string.Format("The following exception occurred attempting to communicate with the Sentinel server: {0}", ex.Message));
                    }
                }
                // add future functionality for input validation
                //else
                //{
                    //set error provider
                //}
            }
        }
        /// <summary>
        /// send feedback to screen
        /// </summary>
        /// <param name="message">message to send to screen</param>
        private void AppendToScreenOutput(string message)
        {
            outputBlock.Text = outputBlock.Text + message + Environment.NewLine;
        }
    }
}

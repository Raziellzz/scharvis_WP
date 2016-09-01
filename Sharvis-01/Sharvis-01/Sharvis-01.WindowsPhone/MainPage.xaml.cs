using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
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
using Windows.UI.Popups;
using Windows.System.Profile;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;


// Pour en savoir plus sur le modèle d'élément Page vierge, consultez la page http://go.microsoft.com/fwlink/?LinkId=234238

namespace Sharvis_01
{
    /// <summary>
    /// Une page vide peut être utilisée seule ou constituer une page de destination au sein d'un frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private string connect;
        private string user;
        private string pass;

        public MainPage()
        {
            this.InitializeComponent();

            this.NavigationCacheMode = NavigationCacheMode.Required;
        }

        
        /// <summary>
        /// Invoqué lorsque cette page est sur le point d'être affichée dans un frame.
        /// </summary>
        /// <param name="e">Données d'événement décrivant la manière dont l'utilisateur a accédé à cette page.
        /// Ce paramètre est généralement utilisé pour configurer la page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            // TODO: préparer la page pour affichage ici.

            // TODO: si votre application comporte plusieurs pages, assurez-vous que vous
            // gérez le bouton Retour physique en vous inscrivant à l’événement
            // Événement Windows.Phone.UI.Input.HardwareButtons.BackPressed.
            // Si vous utilisez le NavigationHelper fourni par certains modèles,
            // cet événement est géré automatiquement.
        }

        private void api_connexion()
        {
            /* IEnumerable <KeyValuePair<String, String>> data = new List<KeyValuePair<String, String>>()
              {
                  new KeyValuePair<String, String>("username", username),
                  new KeyValuePair<String, String>("password", password),
              };
              HttpContent q = new FormUrlEncodedContent(data);
              HttpClient client = new HttpClient();
              HttpResponseMessage answer = await client.PostAsync("http://163.5.84.234:4567/login", q);
              HttpContent content = answer.Content;
              String MyContent = await content.ReadAsStringAsync();
              HttpContentHeaders header = content.Headers;
              Debug.WriteLine(MyContent); 
                  if (MyContent == "true")
                      this.connect = true;
                  else
                  {
                      MessageDialog Dialog = new MessageDialog("Connexion Failure, try again !", "Connexion Api");
                      await Dialog.ShowAsync();
                      return;
                  }*/
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://163.5.84.234:4567/login");

            request.ContentType = "application/json";
            //request.UserAgent = "Mozilla/5.0 (compatible; MSIE 10.0; Windows NT 6.2; WOW64; Trident/6.0; Touch)";
            // request.CookieContainer = cookie;

            // Set the Method property to 'POST' to post data to the URI.
            request.Method = "POST";

            // start the asynchronous operation
           request.BeginGetRequestStream(new AsyncCallback(GetRequestStreamCallback), request);
           return;
        }
        private void GetRequestStreamCallback(IAsyncResult asynchronousResult)
        {
            HttpWebRequest request = (HttpWebRequest)asynchronousResult.AsyncState;

            // End the operation
            Stream postStream = request.EndGetRequestStream(asynchronousResult);

            //postData value
            Debug.WriteLine(user);
            Debug.WriteLine(pass);

            string postData = "{\"username\":\""+ user + "\",\"password\":\"" + pass + "\"}";

            // Convert the string into a byte array. 
            byte[] byteArray = Encoding.UTF8.GetBytes(postData);

            // Write to the request stream.
            postStream.Write(byteArray, 0, postData.Length);
            postStream.Dispose();

            // Start the asynchronous operation to get the response
            request.BeginGetResponse(new AsyncCallback(GetResponseCallback), request);

        }
        private void GetResponseCallback(IAsyncResult asynchronousResult)
        {

            HttpWebRequest request = (HttpWebRequest)asynchronousResult.AsyncState;
            // End the operation
            HttpWebResponse response = (HttpWebResponse)request.EndGetResponse(asynchronousResult);

            Stream streamResponse = response.GetResponseStream();

            StreamReader streamRead = new StreamReader(streamResponse);
            string read = streamRead.ReadToEnd();
            Debug.WriteLine(read);
            connect = read;
            //respond from httpRequest
            // Close the stream object
            streamResponse.Dispose();
            streamRead.Dispose();
            response.Dispose();
        }

        private async void button_Click(object sender, RoutedEventArgs e)
        {
            user = textBox.Text;
            pass = passwordBox.Password;
            api_connexion();
            if (connect != "fail")
            {
                Button send = (Button)sender;
                send.Name = user;
                this.Frame.Navigate(typeof(Pieces), send);
                connect = null;
            }
            else
            {
                MessageDialog Dialog = new MessageDialog("Connexion Failure, try again !", "Connexion");
                await Dialog.ShowAsync();
                return;
            }
        }
    }
}

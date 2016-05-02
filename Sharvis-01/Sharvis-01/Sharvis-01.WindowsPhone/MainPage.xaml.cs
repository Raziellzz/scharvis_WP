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
        private bool connect = false;

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

        private async Task api_connexion(string username, string password)
        {
            IEnumerable<KeyValuePair<String, String>> data = new List<KeyValuePair<String, String>>()
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
            }
        } 

        private async void button_Click(object sender, RoutedEventArgs e)
        {
            String username = textBox.Text;
            String password = passwordBox.Password;
            await api_connexion(username, password);
            if (this.connect == true)
            {
                this.Frame.Navigate(typeof(Pieces));
                this.connect = false;
            }
        }
    }
}

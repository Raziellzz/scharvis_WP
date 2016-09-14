using Sharvis_01.Common;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Net.Http.Headers;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Graphics.Display;
using Windows.UI;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// Pour en savoir plus sur le modèle d'élément Page de base, consultez la page http://go.microsoft.com/fwlink/?LinkID=390556

namespace Sharvis_01
{
    /// <summary>
    /// Une page vide peut être utilisée seule ou constituer une page de destination au sein d'un frame.
    /// </summary>
    public sealed partial class Actions : Page
    {
        private NavigationHelper navigationHelper;
        private ObservableDictionary defaultViewModel = new ObservableDictionary();
        private string ID;
        private string user;
        private string[] action;
        private string[] NameAction;
        private string[] ID_Action;
        private string[] status;
        private string Json;

        public Actions()
        {
            this.InitializeComponent();
            
            this.navigationHelper = new NavigationHelper(this);
            this.navigationHelper.LoadState += this.NavigationHelper_LoadState;
            this.navigationHelper.SaveState += this.NavigationHelper_SaveState;

        }

        /// <summary>
        /// Obtient le <see cref="NavigationHelper"/> associé à ce <see cref="Page"/>.
        /// </summary>
        public NavigationHelper NavigationHelper
        {
            get { return this.navigationHelper; }
        }

        /// <summary>
        /// Obtient le modèle d'affichage pour ce <see cref="Page"/>.
        /// Cela peut être remplacé par un modèle d'affichage fortement typé.
        /// </summary>
        public ObservableDictionary DefaultViewModel
        {
            get { return this.defaultViewModel; }
        }

        /// <summary>
        /// Remplit la page à l'aide du contenu passé lors de la navigation. Tout état enregistré est également
        /// fourni lorsqu'une page est recréée à partir d'une session antérieure.
        /// </summary>
        /// <param name="sender">
        /// La source de l'événement ; en général <see cref="NavigationHelper"/>
        /// </param>
        /// <param name="e">Données d'événement qui fournissent le paramètre de navigation transmis à
        /// <see cref="Frame.Navigate(Type, Object)"/> lors de la requête initiale de cette page et
        /// un dictionnaire d'état conservé par cette page durant une session
        /// antérieure.  L'état n'aura pas la valeur Null lors de la première visite de la page.</param>
        private void NavigationHelper_LoadState(object sender, LoadStateEventArgs e)
        {
        }

        /// <summary>
        /// Conserve l'état associé à cette page en cas de suspension de l'application ou de
        /// suppression de la page du cache de navigation. Les valeurs doivent être conformes aux conditions de
        /// exigences en matière de sérialisation de <see cref="SuspensionManager.SessionState"/>.
        /// </summary>
        /// <param name="sender">La source de l'événement ; en général <see cref="NavigationHelper"/></param>
        /// <param name="e">Données d'événement qui fournissent un dictionnaire vide à remplir à l'aide de l'
        /// état sérialisable.</param>
        private void NavigationHelper_SaveState(object sender, SaveStateEventArgs e)
        {
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            Button piece = (Button)e.Parameter;
            Debug.WriteLine(piece.Tag);
            string test = (string)piece.Tag;
            string[] tab = test.Split(' ');
            ID = tab[0];
            user = tab[1];
            Create_Actions();
            this.navigationHelper.OnNavigatedTo(e);
        }
        /*
        private void path_actions(string MyContent, char c)
        {
            int r = 0;
            int y = 0;
            string[] path = MyContent.Split(c);
            NameAction = new string[path.Length - 1];
            ID_Action = new string[path.Length - 1];
            Debug.WriteLine(path.Length);
            for (int i = 0; i < path.Length - 1; i++)
            {
                string[] tmp = path[i].Split(',');
                for (int x = 0; x < tmp.Length; x++)
                {
                    if (x == 0)
                    {
                        ID_Action[r] = tmp[x];
                        r++;
                    }
                    else
                    {
                        NameAction[y] = tmp[x];
                        y++;
                    }
                }
            }
        }*/

        private void check_actions()
        {
            int x = 0, i = 0, j = 0;
            int count = 1;
            while (i < action.Length)
            {
                if (action[i] == "timeset")
                    i = i + 2;
                if (x == 1)
                {
                    if (count == 3)
                    {
                        j++;
                        count = 0;
                    }
                    count++;
                    x = 0;
                }
                else
                    x++;
                i++;
            }
            ID_Action = new String[j];
            NameAction = new String[j];
            status = new String[j];
            i = 0;
            j = 0;
            while (i < action.Length)
            {
                if (action[i] == "timeset")
                    i = i + 2;
                if (x == 1)
                {
                    if (count == 1)
                    {
                        ID_Action[j] = action[i];
                        Debug.WriteLine(ID_Action[j]);
                    }
                    else if (count == 2)
                    {
                        NameAction[j] = action[i];
                        Debug.WriteLine(NameAction[j]);
                    }
                    else if (count == 3)
                    {
                        status[j] = action[i];
                        Debug.WriteLine(status[j]);
                        count = 0;
                        j++;
                    }
                    count++;
                    x = 0;
                }
                else
                    x++;
                i++;
            }
        }

        private void path_actions(string MyContent)
        {
            int x = 0, r = 0, count = 0;
            string[] path = MyContent.Split('"');
            for (int i = 0; i < path.Length; i++)
            {
                if (x == 1)
                {
                    x = 0;
                    count++;
                }
                else
                    x++;
            }
            action = new String[count];
            x = 0;
            for (int i = 0; i < path.Length; i++)
            {
                if (x == 1)
                {
                    action[r] = path[i];
                    r++;
                    x = 0;
                }
                else
                    x++;
            }
            check_actions();
        }


        private async Task api_Room_Id()
        {
            HttpClient client = new HttpClient();
            Debug.WriteLine(ID);
            HttpResponseMessage answer = await client.GetAsync("http://163.5.84.234:4567/room?id=" + ID);
            HttpContent content = answer.Content;
            String MyContent = await content.ReadAsStringAsync();
            HttpContentHeaders header = content.Headers;
            Debug.WriteLine(MyContent);
            path_actions(MyContent);
            Debug.WriteLine(user);
        }

        private bool check_on_off(string on_off)
        {
            if (on_off == "True" || on_off == "on")
                return (true);  
            return (false);
        }

        private async void Create_Actions()
        {
            await api_Room_Id();
            for (int i = 0; i < NameAction.Length; i++)
            {
                StackPanel stkpanel = new StackPanel();
                stkpanel.Orientation = Orientation.Horizontal;
                stkpanel.Height = 60;
                TextBlock action_n = new TextBlock();
                {
                    action_n.Name = NameAction[i];
                    action_n.Height = 100;
                    action_n.Width = 150;
                    action_n.FontSize = 25;
                    action_n.Text = NameAction[i];
                    action_n.Margin = new Thickness(5, 25, 5, 5);
                    action_n.VerticalAlignment = Windows.UI.Xaml.VerticalAlignment.Top;
                }
                ToggleSwitch action_b = new ToggleSwitch();
                {
                    action_b.Name = NameAction[i];
                    action_b.Height = 100;
                    action_b.Width = 150;
                    action_b.OffContent = "Off";
                    action_b.OnContent = "On";
                    action_b.IsOn = check_on_off(status[i]);
                    action_b.Tag = ID_Action[i];
                    action_b.Margin = new Thickness(5, 5, 5, 5);
                    action_b.VerticalAlignment = Windows.UI.Xaml.VerticalAlignment.Top;
                }
                action_b.Toggled += ToggleSwitch_Toggled;
                stkpanel.Children.Add(action_n);
                stkpanel.Children.Add(action_b);
                Content.Children.Add(stkpanel);
            }
        }

        private void ToggleSwitch_Toggled(object sender, RoutedEventArgs e)
        {
            string O_I;
            ToggleSwitch action_btn = (ToggleSwitch)sender;
            if (action_btn.IsOn.ToString() == "True")
                O_I = "on";
            else
                O_I = "off";
            Json = "{\"user\":\"" + user + "\",\"action\":\"" + O_I + "\",\"type\":\"" + action_btn.Name + "\",\"id\":\"" + action_btn.Tag + "\"}";
            state_change();
            //Content.Children.Clear();
            //Create_Actions();
        }

        private void state_change()
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://163.5.84.234:4567/action");

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

            string postData = Json;

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
            //respond from httpRequest
            // Close the stream object
            streamResponse.Dispose();
            streamRead.Dispose();
            response.Dispose();
        }
    }
}
using Sharvis_01.Common;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.InteropServices.WindowsRuntime;
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
        private int ID = 0;
        private string[] NameAction;
        private string[] ID_Action;
        private bool on_off = false;

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
            ID = (int)piece.Tag;
            this.navigationHelper.OnNavigatedTo(e);
            Create_Actions();
        }

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
        }

        private async Task api_Room_Id()
        {
            HttpClient client = new HttpClient();
            Debug.WriteLine(ID);
            HttpResponseMessage answer = await client.GetAsync("http://163.5.84.234:4567/room?id=" + ID.ToString());
            HttpContent content = answer.Content;
            String MyContent = await content.ReadAsStringAsync();
            HttpContentHeaders header = content.Headers;
            Debug.WriteLine(MyContent);
            path_actions(MyContent, ';');
        }

        private async Task check_on_off(string id_action)
        {
            HttpClient client = new HttpClient();
            Debug.WriteLine(ID);
            HttpResponseMessage answer = await client.GetAsync("http://163.5.84.234:4567/status?id=" + id_action);
            HttpContent content = answer.Content;
            String MyContent = await content.ReadAsStringAsync();
            HttpContentHeaders header = content.Headers;
            if (MyContent == "0")
                on_off = false;
            else
                on_off = true;
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
                    await check_on_off(ID_Action[i]);
                    action_b.IsOn = on_off;
                    action_b.Margin = new Thickness(5, 5, 5, 5);
                    action_b.VerticalAlignment = Windows.UI.Xaml.VerticalAlignment.Top;
                }
                stkpanel.Children.Add(action_n);
                stkpanel.Children.Add(action_b);
                Content.Children.Add(stkpanel);
            }
        }
    }
}
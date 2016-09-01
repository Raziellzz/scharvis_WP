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
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// Pour en savoir plus sur le modèle d'élément Page de base, consultez la page http://go.microsoft.com/fwlink/?LinkID=390556

namespace Sharvis_01
{
    /// <summary>
    /// Une page vide peut être utilisée seule ou constituer une page de destination au sein d'un frame.
    /// </summary>
    public sealed partial class Pieces : Page
    {
        private NavigationHelper navigationHelper;
        private ObservableDictionary defaultViewModel = new ObservableDictionary();
        private string[] rooms;
        private string[] n_room;
        private string[] id_room;
        private string[] type_room;
        private string user;


        public Pieces()
        {
            this.InitializeComponent();

            this.navigationHelper = new NavigationHelper(this);
            this.navigationHelper.LoadState += this.NavigationHelper_LoadState;
            this.navigationHelper.SaveState += this.NavigationHelper_SaveState;

            // CreatePiece();
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
            Debug.WriteLine(piece.Name);
            user = piece.Name;
            CreatePiece();
            this.navigationHelper.OnNavigatedTo(e);
        }

        private void check_room()
        {
            int x = 0, i = 0, j = 0;
            int count = 1;
            while (i < rooms.Length)
            {
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
            id_room = new String[j];
            n_room = new String[j];
            type_room = new String[j];
            i = 0;
            j = 0;
            while (i < rooms.Length)
            {
                if (x == 1)
                {
                    if (count == 1)
                    {
                        id_room[j] = rooms[i];
                        Debug.WriteLine(id_room[j]);
                    }
                    else if (count == 2)
                    {
                        n_room[j] = rooms[i];
                        Debug.WriteLine(n_room[j]);
                    }
                    else if (count == 3)
                    {
                        type_room[j] = rooms[i];
                        Debug.WriteLine(type_room[j]);
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

        private void path_rooms(string MyContent)
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
            rooms = new String[count];
            x = 0;
            for (int i = 0; i < path.Length; i++)
            {
                if (x == 1)
                {
                    rooms[r] = path[i];
                    r++;
                    x = 0;
                }
                else
                    x++;
            }
            check_room();
        }

        private async Task api_rooms()
        {
            HttpClient client = new HttpClient();
            HttpResponseMessage answer = await client.GetAsync("http://163.5.84.234:4567/rooms");
            HttpContent content = answer.Content;
            String MyContent = await content.ReadAsStringAsync();
            HttpContentHeaders header = content.Headers;
            Debug.WriteLine(MyContent);
            path_rooms(MyContent);
        }

        private async void CreatePiece()
        {
            await api_rooms();
            // How many buttons do you want ?
            for (int i = 0; i < n_room.Length; i++)
            {
                StackPanel stkpanel = new StackPanel();
                stkpanel.Orientation = Orientation.Horizontal;
                stkpanel.Height = 130;
                Button btn = new Button();
                {
                    btn.Name = n_room[i];
                    btn.Height = 100;
                    btn.Width = 100;
                    btn.Foreground = new SolidColorBrush(Colors.White);
                    var ibrush = new ImageBrush();
                    ibrush.ImageSource = new BitmapImage(new Uri(check_piece(type_room[i])));
                    btn.Background = ibrush;
                    btn.Tag = id_room[i] + " " + user;
                    btn.Margin = new Thickness(5, 5, 5, 5);
                    btn.VerticalAlignment = Windows.UI.Xaml.VerticalAlignment.Top;
                }
                TextBlock pieces = new TextBlock();
                {
                    pieces.Name = n_room[i];
                    pieces.Height = 100;
                    pieces.Width = 150;
                    pieces.Foreground = new SolidColorBrush(Colors.White);
                    pieces.FontSize = 20;
                    pieces.Tag = id_room[i];
                    pieces.Text = n_room[i];
                    pieces.Margin = new Thickness(50, 45, 5, 5);
                    pieces.VerticalAlignment = Windows.UI.Xaml.VerticalAlignment.Top;
                }
                //Add Click event Handler for each created button
                btn.Click += btn_Click;
                // Add the created btn to grid
                stkpanel.Children.Add(btn);
                stkpanel.Children.Add(pieces);
                Content.Children.Add(stkpanel);
            }
        }
       
        private string check_piece(string room)
        {
            if (room == "bedroom")
                return ("ms-appx:/Assets/Chambre.jpg");
            else if (room == "bathroom")
                return ("ms-appx:/Assets/Salle de bain.jpg");
            else if (room == "kitchen")
                return ("ms-appx:/Assets/Cuisine.jpg");
            else if (room == "living room")
                return ("ms-appx:/Assets/Salon.jpg");
            else if (room == "restroom")
                return ("ms-appx:/Assets/Toilettes.jpg");
            else
                return ("ms-appx:/Assets/Chambre.jpg");
        }

        private void btn_Click(object sender, RoutedEventArgs e)
        {
            Debug.WriteLine(sender.ToString());
            Button toto = (Button)sender;
            Debug.WriteLine(toto.Name);
            this.Frame.Navigate(typeof(Actions), sender);
        }
        #region Inscription de NavigationHelper

        /// <summary>
        /// Les méthodes fournies dans cette section sont utilisées simplement pour permettre
        /// NavigationHelper pour répondre aux méthodes de navigation de la page.
        /// <para>
        /// La logique spécifique à la page doit être placée dans les gestionnaires d'événements pour  
        /// <see cref="NavigationHelper.LoadState"/>
        /// et <see cref="NavigationHelper.SaveState"/>.
        /// Le paramètre de navigation est disponible dans la méthode LoadState 
        /// en plus de l'état de page conservé durant une session antérieure.
        /// </para>
        /// </summary>
        /// <param name="e">Fournit des données pour les méthodes de navigation et
        /// les gestionnaires d'événements qui ne peuvent pas annuler la requête de navigation.</param>

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            this.navigationHelper.OnNavigatedFrom(e);
        }

        #endregion
    }
}

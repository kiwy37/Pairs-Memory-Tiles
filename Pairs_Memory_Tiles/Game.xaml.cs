using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using System.Xml;

namespace Pairs_Bajan_Ramona
{
    /// <summary>
    /// Interaction logic for Game.xaml
    /// </summary>
    public partial class Game : Window
    {
        private int Moves = 0;
        private readonly User user;
        private readonly List<Button> buttons;
        private readonly Pair<Tile, Tile> tilePair;
        private readonly Pair<Button, Button> buttonPair;
        private readonly DispatcherTimer _timer;
        private bool start = false;
        private int contor = 0;
        private bool auxbool = false;

        public Game(User u)
        {
            InitializeComponent();
            user = new User(u);
            pic.Source = new BitmapImage(new Uri($"/{user.image}", UriKind.Relative));
            usernameText.Text = user.username;
            levelBox.Text = "Level: " + GameLogic.sUser.level.ToString();
            buttons = new List<Button>();
            tilePair = new Pair<Tile, Tile>(default, default);
            buttonPair = new Pair<Button, Button>(default, default);

            Loaded += MainWindow_Loaded;

            _timer = new DispatcherTimer();
            _timer.Interval = TimeSpan.FromSeconds(1);
            _timer.Tick += Timer_Tick;
            _timer.Start();
            Closing += Game_Closing;  // add event handler for Closing event
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            // Iterate over all the Buttons on the screen
            foreach (Button button in FindVisualChildren<Button>(this))
            {
                // Get the associated Tile object for this Button
                Tile tile = button.DataContext as Tile;

                // If a Tile object is found, print its imageInBinding property
                if (tile != null && Moves < 2)
                {
                    if (tile.imageInBinding != "done.jpg" && tile.imageInBinding != "i0.jpg")
                    {
                        if (Moves == 0)
                        {
                            Tile t = new Tile(tile);
                            tilePair.First = t;
                            buttonPair.First = button;
                        }
                        if (Moves == 1)
                        {
                            Tile t = new Tile(tile);
                            tilePair.Second = t;
                            buttonPair.Second = button;
                            auxbool = true;
                        }
                        Moves++;
                        Moves = Moves % 2;
                    }
                    if (tile.imageInBinding == "done.jpg")
                    {
                        contor++;
                        button.IsEnabled = false;
                    }
                }
            }
            contor = contor / 2;
        }

        // Helper method to find all child elements of a given type in a visual tree
        private static IEnumerable<T> FindVisualChildren<T>(DependencyObject obj) where T : DependencyObject
        {
            if (obj != null)
            {
                for (int i = 0; i < VisualTreeHelper.GetChildrenCount(obj); i++)
                {
                    DependencyObject child = VisualTreeHelper.GetChild(obj, i);
                    if (child != null && child is T)
                    {
                        yield return (T)child;
                    }

                    foreach (T childOfChild in FindVisualChildren<T>(child))
                    {
                        yield return childOfChild;
                    }
                }
            }
        }

        private void Game_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            _timer.Stop();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            // Update the time left
            GameLogic.sUser.time--;
            timerBox.Text = GameLogic.sUser.time.ToString();

            // If time is up, stop the timer
            if (GameLogic.sUser.time == 0)
            {
                _timer.Stop();
                GameLogic.sUser.level = 0;
                MessageBox.Show("Time's up!");
                Close();
            }
        }

        public static void Replace_Tile(Tile oldValue, Tile newValue)
        {
            foreach (List<Tile> innerList in GameLogic.sUser.configuratie)
            {
                for (int i = 0; i < innerList.Count; i++)
                {
                    if (innerList[i] != null && innerList[i].Equals(oldValue))
                    {
                        innerList[i] = newValue;
                    }
                }
            }
        }

        private void save()
        {
            string xmlFile = "output.xml";
            XmlDocument doc = new XmlDocument();
            if (System.IO.File.Exists(xmlFile))
            {
                doc.Load(xmlFile);

                XmlNodeList users = doc.DocumentElement.SelectNodes("//user");
                foreach (XmlNode user in users)
                {
                    XmlNode usernameNode = user.SelectSingleNode("username");
                    XmlNode imageNode = user.SelectSingleNode("image");

                    if (usernameNode.InnerText == GameLogic.sUser.username && imageNode.InnerText == GameLogic.sUser.image)
                    {
                        XmlNode gamesNode = user.SelectSingleNode("games");
                        gamesNode.InnerText = GameLogic.sUser.games.ToString();

                        XmlNode winsNode = user.SelectSingleNode("wins");
                        winsNode.InnerText = GameLogic.sUser.wins.ToString();

                        XmlNode widthNode = user.SelectSingleNode("width");
                        widthNode.InnerText = GameLogic.sUser.width.ToString();

                        XmlNode heightNode = user.SelectSingleNode("height");
                        heightNode.InnerText = GameLogic.sUser.height.ToString();

                        XmlNode levelNode = user.SelectSingleNode("level");
                        levelNode.InnerText = GameLogic.sUser.level.ToString();

                        XmlNode configuratieNode = user.SelectSingleNode("configuratie");
                        configuratieNode.RemoveAll();
                        foreach (List<Tile> row in GameLogic.sUser.configuratie)
                        {
                            XmlNode rowElement = doc.CreateElement("row");
                            foreach (Tile tile in row)
                            {
                                XmlNode tileElement = doc.CreateElement("tile");
                                XmlNode tileImageElement = doc.CreateElement("image");
                                tileImageElement.InnerText = tile.Image;
                                tileElement.AppendChild(tileImageElement);

                                XmlNode tileFlipElement = doc.CreateElement("flip");
                                tileFlipElement.InnerText = tile.Flip.ToString();
                                tileElement.AppendChild(tileFlipElement);

                                XmlNode tileDoneElement = doc.CreateElement("done");
                                tileDoneElement.InnerText = tile.Done.ToString();
                                tileElement.AppendChild(tileDoneElement);

                                XmlNode tileImageInBindingElement = doc.CreateElement("imageInBinding");
                                tileImageInBindingElement.InnerText = tile.imageInBinding.ToString();
                                tileElement.AppendChild(tileImageInBindingElement);

                                rowElement.AppendChild(tileElement);
                            }
                            configuratieNode.AppendChild(rowElement);
                        }

                        doc.Save(xmlFile);
                        break;
                    }
                }
            }
        }


        private void FlipTile_Clicked(object sender, RoutedEventArgs e)
        {
            //intoarce piese la incaput --- caz moves=0, exceptie la inceput
            if ((start != false && Moves == 0) || auxbool == true)
            {
                auxbool = false;
                if (tilePair.First.Image != tilePair.Second.Image)
                {
                    Tile clickedTile1 = buttonPair.First.DataContext as Tile;
                    Tile newTile1 = new Tile(clickedTile1);
                    newTile1.imageInBinding = newTile1.back;
                    newTile1.flip = false;
                    buttonPair.First.DataContext = newTile1;
                    Replace_Tile(clickedTile1, newTile1);
                    buttonPair.First.InvalidateVisual();

                    Tile clickedTile2 = buttonPair.Second.DataContext as Tile;
                    Tile newTile2 = new Tile(clickedTile2);
                    newTile2.imageInBinding = newTile2.back;
                    newTile2.flip = false;
                    buttonPair.Second.DataContext = newTile2;
                    Replace_Tile(clickedTile2, newTile2);
                    buttonPair.Second.InvalidateVisual();
                }
                else
                {                                                                   //pereche
                    contor++;

                    Tile clickedTile1 = buttonPair.First.DataContext as Tile;
                    Tile newTile1 = new Tile(clickedTile1);
                    newTile1.flip = true;
                    newTile1.Done = true;
                    newTile1.imageInBinding = "done.jpg";
                    buttonPair.First.IsEnabled = false;
                    buttonPair.First.DataContext = newTile1;
                    Replace_Tile(clickedTile1, newTile1);
                    buttonPair.First.InvalidateVisual();

                    Tile clickedTile2 = buttonPair.Second.DataContext as Tile;
                    Tile newTile2 = new Tile(clickedTile2);
                    newTile2.flip = true;
                    newTile2.Done = true;
                    newTile2.imageInBinding = "done.jpg";
                    buttonPair.Second.IsEnabled = false;
                    buttonPair.Second.DataContext = newTile2;
                    Replace_Tile(clickedTile2, newTile2);
                    buttonPair.Second.InvalidateVisual();
                }
            }
            else
            {
                start = true;
            }


            Button button = sender as Button;                           //default
            Tile clickedTile = button.DataContext as Tile;
            Tile newTile = new Tile(clickedTile);

            if (Moves == 1 && tilePair.First == clickedTile)
            {
                MessageBox.Show("Nu poți da flip la aceeași piesă, da clic pe alta!");
            }
            else if (clickedTile.done == true)
            {
                MessageBox.Show("Perechea este deja găsită!");
                contor--;
            }
            else
            {
                if (clickedTile.imageInBinding == clickedTile.back)             //intoarce piesa
                {
                    newTile.imageInBinding = newTile.image;
                    newTile.flip = true;
                    button.DataContext = newTile;
                    Replace_Tile(clickedTile, newTile);
                    button.InvalidateVisual();
                }
                else
                {
                    newTile.imageInBinding = newTile.back;
                    newTile.flip = false;
                    button.DataContext = newTile;
                    Replace_Tile(clickedTile, newTile);
                    button.InvalidateVisual();
                }
                if (Moves == 0)
                {
                    tilePair.First = newTile;
                    buttonPair.First = button;
                    if (contor == GameLogic.sUser.width * GameLogic.sUser.height / 2)
                    {
                        _timer.Stop();
                        GameLogic.sUser.level++;
                        GameLogic.sUser.prepare();
                        if (GameLogic.sUser.level == 4)
                        {
                            MessageBox.Show("Meci castigat!");
                            GameLogic.sUser.wins++;
                            save();
                            Close();
                        }
                        else
                        {
                            MessageBox.Show("Runda castigata!");
                            Thread.Sleep(500);
                            Game g = new Game(user);
                            g.Show();
                            Close();
                        }
                    }
                }
                if (Moves == 1)
                {
                    tilePair.Second = newTile;
                    buttonPair.Second = button;
                    if (contor + 1 == GameLogic.sUser.width * GameLogic.sUser.height / 2)
                    {
                        _timer.Stop();
                        GameLogic.sUser.level++;
                        GameLogic.sUser.prepare();
                        if (GameLogic.sUser.level == 4)
                        {
                            MessageBox.Show("Meci castigat!");
                            GameLogic.sUser.wins++;
                            save();
                            Close();
                        }
                        else
                        {
                            MessageBox.Show("Runda castigata!");
                            Thread.Sleep(500);
                            Game g = new Game(user);
                            g.Show();
                            Close();
                        }
                    }
                }
                Moves++;
                Moves = Moves % 2;
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Xml;

namespace Pairs_Bajan_Ramona
{
    /// <summary>
    /// Interaction logic for FileWindow.xaml
    /// </summary>
    public partial class FileWindow : Window
    {
        private readonly User user;
        private readonly Pair<int, int> newDimensions;
        private bool ok = false;

        public FileWindow(User u, Pair<int, int> newDim)
        {
            InitializeComponent();
            user = new User(u);
            newDimensions = new Pair<int, int>(default, default);

            newDimensions.First = newDim.First;
            newDimensions.Second = newDim.Second;

            GameLogic.sUser = new User();
            GameLogic.sUser.username = user.username;
            GameLogic.sUser.image = user.image;
            GameLogic.sUser.wins = user.wins;
            GameLogic.sUser.configuratie = user.configuratie;
            GameLogic.sUser.games = user.games;
            GameLogic.sUser.level = user.level;
            GameLogic.sUser.time = user.time;
        }

        private void NewGameButton(object sender, RoutedEventArgs e)
        {
            GameLogic.sUser.width = newDimensions.Second;
            GameLogic.sUser.height = newDimensions.First;
            GameLogic.sUser.level = 1;
            GameLogic.sUser.games++;
            GameLogic.sUser.time = 60;
            GameLogic.sUser.prepare();
            increaseGamesNumber();                     // doar ca sa adauge joc, in rest fisierul e neschimbat

            GameLogic.sUser.prepare();
            Game game = new Game(user);
            game.Show();
        }

        private void OpenGameButton(object sender, RoutedEventArgs e)
        {

            if (GameLogic.sUser.level == 4)
            {
                GameLogic.sUser.width = user.width;
                GameLogic.sUser.height = user.height;
                GameLogic.sUser.level = 1;
                GameLogic.sUser.games++;
                GameLogic.sUser.time = 60;
                GameLogic.sUser.prepare();
                increaseGamesNumber();                     // doar ca sa adauge joc, in rest fisierul e neschimbat

                GameLogic.sUser.prepare();
                Game game = new Game(user);
                game.Show();
            }
            else
            {
                UpdateUser();
                Game game = new Game(user);
                game.Show();
            }
        }

        private void UpdateUser()
        {
            // Load user data from previous XML file
            string xmlFile = "output.xml";
            if (File.Exists(xmlFile))
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(xmlFile);

                foreach (XmlNode node in doc.SelectNodes("//user"))
                {
                    GameLogic.sUser.time = int.Parse(node.SelectSingleNode("time")?.InnerText);
                    GameLogic.sUser.height = int.Parse(node.SelectSingleNode("height")?.InnerText);
                    GameLogic.sUser.width = int.Parse(node.SelectSingleNode("width")?.InnerText);

                    GameLogic.sUser.configuratie.Clear();
                    foreach (XmlNode rowNode in node.SelectNodes("configuratie/row"))
                    {
                        List<Tile> rowTiles = new List<Tile>();
                        foreach (XmlNode tileNode in rowNode.SelectNodes("tile"))
                        {
                            string tileImage = tileNode.SelectSingleNode("image")?.InnerText;
                            bool tileFlip = bool.Parse(tileNode.SelectSingleNode("flip")?.InnerText);
                            bool tileDone = bool.Parse(tileNode.SelectSingleNode("done")?.InnerText);
                            string tileImageInBinding = tileNode.SelectSingleNode("imageInBinding")?.InnerText; // new code

                            Tile tile = new Tile(tileImage, tileFlip, tileDone, tileImageInBinding); // modified code
                            rowTiles.Add(tile);
                        }
                        GameLogic.sUser.configuratie.Add(rowTiles);
                    }

                }
            }
        }

        private void increaseGamesNumber()
        {
            string xmlFile = "output.xml";
            XmlDocument doc = new XmlDocument();
            if (File.Exists(xmlFile))
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

                        doc.Save(xmlFile);
                        break;
                    }
                }
            }
        }

        private void SaveGameButton(object sender, RoutedEventArgs e)
        {
            ok = true;
            string xmlFile = "output.xml";
            XmlDocument doc = new XmlDocument();
            try
            {
                doc.Load(xmlFile);

                XmlNode userNode = doc.SelectSingleNode($"//user[username='{GameLogic.sUser.username}' and image='{GameLogic.sUser.image}']");
                if (userNode != null)
                {
                    // Update width and height
                    userNode.SelectSingleNode("time").InnerText = GameLogic.sUser.time.ToString();
                    userNode.SelectSingleNode("width").InnerText = GameLogic.sUser.width.ToString();
                    userNode.SelectSingleNode("height").InnerText = GameLogic.sUser.height.ToString();

                    // Update configuration
                    XmlNode configNode = userNode.SelectSingleNode("configuratie");
                    configNode.RemoveAll();
                    foreach (List<Tile> row in GameLogic.sUser.configuratie)
                    {
                        XmlNode rowNode = doc.CreateElement("row");
                        foreach (Tile tile in row)
                        {
                            XmlNode tileNode = doc.CreateElement("tile");

                            XmlNode imageNode = doc.CreateElement("image");
                            imageNode.InnerText = tile.Image;
                            tileNode.AppendChild(imageNode);

                            XmlNode flipNode = doc.CreateElement("flip");
                            flipNode.InnerText = tile.Flip.ToString();
                            tileNode.AppendChild(flipNode);

                            XmlNode doneNode = doc.CreateElement("done");
                            doneNode.InnerText = tile.Done.ToString();
                            tileNode.AppendChild(doneNode);

                            XmlNode imageInBinding = doc.CreateElement("imageInBinding");
                            imageInBinding.InnerText = tile.imageInBinding;
                            tileNode.AppendChild(imageInBinding);

                            rowNode.AppendChild(tileNode);
                        }
                        configNode.AppendChild(rowNode);
                    }
                    doc.Save(xmlFile);
                    MessageBox.Show("Game saved successfully.");
                }
                else
                {
                    MessageBox.Show("User not found in the XML file.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error while saving game: {ex.Message}");
            }
        }

        private void StatisticsButton(object sender, RoutedEventArgs e)
        {
            Statistics window = new Statistics();
            window.Show();
        }

        private void ExitButton(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}

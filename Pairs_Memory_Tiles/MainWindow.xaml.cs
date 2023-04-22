using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Media.Imaging;
using System.Xml;

namespace Pairs_Bajan_Ramona
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            update();
            if (userList.Count == 0)
            {
                back.IsEnabled = false;
                next.IsEnabled = false;
            }
            DeleteUser.IsEnabled = false;
            Play.IsEnabled = false;
        }

        private int currentIndex = 0;
        private readonly List<User> userList = new List<User>();

        private void NewUserButton(object sender, RoutedEventArgs e)
        {
            Window1 window1 = new Window1();
            window1.ShowDialog();
            update();
            currentIndex = userList.Count - 1;
            textBox.SelectedIndex = currentIndex;
            image.Source = new BitmapImage(new Uri($"/{userList[currentIndex].image}", UriKind.Relative));
            update();
            if (userList.Count == 0)
            {
                back.IsEnabled = false;
                next.IsEnabled = false;
            }
            else
            {
                back.IsEnabled = true;
                next.IsEnabled = true;
            }
        }

        private void update()
        {
            // Load user data from previous XML file
            string xmlFile = "output.xml";
            userList.Clear();
            if (File.Exists(xmlFile))
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(xmlFile);

                foreach (XmlNode node in doc.SelectNodes("//user"))
                {
                    string username = node.SelectSingleNode("username")?.InnerText;
                    string image = node.SelectSingleNode("image")?.InnerText;
                    int games = int.Parse(node.SelectSingleNode("games")?.InnerText);
                    int wins = int.Parse(node.SelectSingleNode("wins")?.InnerText);
                    int width = int.Parse(node.SelectSingleNode("width")?.InnerText);
                    int height = int.Parse(node.SelectSingleNode("height")?.InnerText);
                    int level = int.Parse(node.SelectSingleNode("level")?.InnerText);
                    int time = int.Parse(node.SelectSingleNode("time")?.InnerText);

                    List<List<Tile>> configuratie = new List<List<Tile>>();
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
                        configuratie.Add(rowTiles);
                    }

                    User user = new User(username, image, games, wins, width, height, level, configuratie, time);
                    userList.Add(user);
                }
            }

            // Add users to the textBox control
            textBox.Items.Clear();
            foreach (User user in userList)
            {
                textBox.Items.Add(user.username);
            }
        }


        private void CancelButton(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void NextButton(object sender, RoutedEventArgs e)
        {
            currentIndex++;
            if (currentIndex >= userList.Count)
                currentIndex %= userList.Count;
            textBox.SelectedIndex = currentIndex;
            User element = userList[currentIndex];
            image.Source = new BitmapImage(new Uri($"/{element.image}", UriKind.Relative));
        }

        private void BackButton(object sender, RoutedEventArgs e)
        {
            currentIndex--;
            if (currentIndex < 0)
                currentIndex = userList.Count + currentIndex;
            textBox.SelectedIndex = currentIndex;
            User element = userList[currentIndex];
            image.Source = new BitmapImage(new Uri($"/{element.image}", UriKind.Relative));
        }

        private void ListBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (textBox.SelectedItem != null)
            {
                string selectedItem = textBox.SelectedItem.ToString();
                currentIndex = textBox.SelectedIndex;
                User element = userList[currentIndex];
                image.Source = new BitmapImage(new Uri($"/{element.image}", UriKind.Relative));

                // Enable the Delete User and Play buttons when an item is selected
                DeleteUser.IsEnabled = true;
                Play.IsEnabled = true;
            }
        }

        private void DeleteUserButton(object sender, RoutedEventArgs e)
        {
            if (userList.Count > 0)
            {
                string xmlFile = "output.xml";
                User element = userList[currentIndex];

                if (File.Exists(xmlFile))
                {
                    XmlDocument doc = new XmlDocument();
                    doc.Load(xmlFile);
                    XmlNodeList nodes = doc.SelectNodes($"//user[username='{element.username}' and image='{element.image}']");
                    foreach (XmlNode node in nodes)
                    {
                        node.ParentNode.RemoveChild(node);
                    }

                    doc.Save(xmlFile);
                }

                update();
                if (userList.Count == 0)
                {
                    back.IsEnabled = false;
                    next.IsEnabled = false;
                    DeleteUser.IsEnabled = false;
                    Play.IsEnabled = false;
                }
                else
                {
                    currentIndex++;
                    if (currentIndex >= userList.Count)
                        currentIndex %= userList.Count;
                    textBox.SelectedIndex = currentIndex;
                    element = userList[currentIndex];
                    image.Source = new BitmapImage(new Uri($"/{element.image}", UriKind.Relative));
                }
            }
            else
            {
                MessageBox.Show("Nu poti realiza actiunea.", "Error");
            }
            if (userList.Count == 0)
            {
                back.IsEnabled = false;
                next.IsEnabled = false;
            }
            else
            {
                back.IsEnabled = true;
                next.IsEnabled = true;
            }
        }


        private void PlayButton(object sender, RoutedEventArgs e)
        {
            Play window = new Play(userList[currentIndex]);
            window.Show();
        }
    }
}

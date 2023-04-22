using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Xml;

namespace Pairs_Bajan_Ramona
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {
        private int currentImageIndex = 0;
        private readonly User u = new User();

        public Window1()
        {
            InitializeComponent();
        }

        private void BackButton(object sender, RoutedEventArgs e)
        {
            currentImageIndex--;
            if (currentImageIndex < 0)
                currentImageIndex = 10 + currentImageIndex;
            currentImageIndex %= 10;
            u.image = $"i{currentImageIndex + 1}.jpg";
            image.Source = new BitmapImage(new Uri($"/{u.image}", UriKind.Relative));
        }

        private void NextButton(object sender, RoutedEventArgs e)
        {
            currentImageIndex++;
            currentImageIndex %= 10;
            u.image = $"i{currentImageIndex + 1}.jpg";
            image.Source = new BitmapImage(new Uri($"/{u.image}", UriKind.Relative));
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            u.username = textBox.Text;
        }

        private bool UserExists(string username, string image)
        {
            string xmlFile = "output.xml";
            XmlDocument doc = new XmlDocument();
            if (File.Exists(xmlFile))
            {
                doc.Load(xmlFile);
                XmlNodeList userList = doc.SelectNodes("//user");
                foreach (XmlNode userNode in userList)
                {
                    string existingUsername = userNode.SelectSingleNode("username").InnerText;
                    string existingImage = userNode.SelectSingleNode("image").InnerText;
                    if (existingUsername == username && existingImage == image)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        private void AddUserButton(object sender, RoutedEventArgs e)
        {
            if (UserExists(u.username, u.image))
            {
                MessageBox.Show("A user with the same name and image already exists.");
                return;
            }
            else
            {
                // Load existing user data from XML file
                string xmlFile = "output.xml";
                XmlDocument doc = new XmlDocument();
                if (File.Exists(xmlFile))
                {
                    doc.Load(xmlFile);
                }
                else
                {
                    // If the XML file doesn't exist yet, create a new document with a root element
                    XmlElement root = doc.CreateElement("users");
                    doc.AppendChild(root);
                }

                // Create a new user element with child elements for each property
                XmlElement userElement = doc.CreateElement("user");
                XmlElement usernameElement = doc.CreateElement("username");
                usernameElement.InnerText = u.username;
                userElement.AppendChild(usernameElement);

                XmlElement imageElement = doc.CreateElement("image");
                imageElement.InnerText = u.image;
                userElement.AppendChild(imageElement);

                XmlElement gamesElement = doc.CreateElement("games");
                gamesElement.InnerText = u.games.ToString();
                userElement.AppendChild(gamesElement);

                XmlElement winsElement = doc.CreateElement("wins");
                winsElement.InnerText = u.wins.ToString();
                userElement.AppendChild(winsElement);

                XmlElement widthElement = doc.CreateElement("width");
                widthElement.InnerText = u.width.ToString();
                userElement.AppendChild(widthElement);

                XmlElement heightElement = doc.CreateElement("height");
                heightElement.InnerText = u.height.ToString();
                userElement.AppendChild(heightElement);

                XmlElement levelElement = doc.CreateElement("level");
                levelElement.InnerText = u.level.ToString();
                userElement.AppendChild(levelElement);

                XmlElement time = doc.CreateElement("time");
                time.InnerText = u.time.ToString();
                userElement.AppendChild(time);

                XmlElement configuratieElement = doc.CreateElement("configuratie");
                foreach (List<Tile> row in u.configuratie)
                {
                    XmlElement rowElement = doc.CreateElement("row");
                    foreach (Tile tile in row)
                    {
                        XmlElement tileElement = doc.CreateElement("tile");
                        XmlElement tileImageElement = doc.CreateElement("image");
                        tileImageElement.InnerText = tile.Image;
                        tileElement.AppendChild(tileImageElement);

                        XmlElement tileFlipElement = doc.CreateElement("flip");
                        tileFlipElement.InnerText = tile.Flip.ToString();
                        tileElement.AppendChild(tileFlipElement);

                        XmlElement tileDoneElement = doc.CreateElement("done");
                        tileDoneElement.InnerText = tile.Done.ToString();
                        tileElement.AppendChild(tileDoneElement);

                        XmlElement tileImageInBindingElement = doc.CreateElement("imageInBinding");
                        tileImageInBindingElement.InnerText = tile.imageInBinding.ToString();
                        tileElement.AppendChild(tileImageInBindingElement);

                        rowElement.AppendChild(tileElement);
                    }
                    configuratieElement.AppendChild(rowElement);
                }
                userElement.AppendChild(configuratieElement);

                // Add the new user element to the XML document and save it
                doc.DocumentElement.AppendChild(userElement);
                doc.Save(xmlFile);
                Close();
            }
        }
    }
}

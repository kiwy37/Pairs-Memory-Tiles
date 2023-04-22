using System.Collections.Generic;
using System.Windows;
using System.Xml;

namespace Pairs_Bajan_Ramona
{
    public partial class Statistics : Window
    {
        public Statistics()
        {
            InitializeComponent();

            // Load the XML file
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load("output.xml");

            // Create a list to hold the user data
            List<UserData> userDataList = new List<UserData>();

            // Extract the user data from the XML file and add it to the list
            XmlNodeList userNodes = xmlDoc.SelectNodes("//user");
            foreach (XmlNode userNode in userNodes)
            {
                string name = userNode.SelectSingleNode("username").InnerText;
                int gamesPlayed = int.Parse(userNode.SelectSingleNode("games").InnerText);
                int wins = int.Parse(userNode.SelectSingleNode("wins").InnerText);

                userDataList.Add(new UserData { Name = name, GamesPlayed = gamesPlayed, Wins = wins });
            }

            // Set the list view's data source to the list of user data
            UsersListView.ItemsSource = userDataList;
        }

        // Define a class to hold the user data
        public class UserData
        {
            public string Name { get; set; }
            public int GamesPlayed { get; set; }
            public int Wins { get; set; }
        }
    }
}

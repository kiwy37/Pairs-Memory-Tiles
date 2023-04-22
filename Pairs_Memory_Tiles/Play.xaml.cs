using System;
using System.Windows;

namespace Pairs_Bajan_Ramona
{
    /// <summary>
    /// Interaction logic for Play.xaml
    /// </summary>
    public partial class Play : Window
    {
        private readonly User user;
        private readonly Pair<int, int> newDimensions;

        public Play(User u)
        {
            InitializeComponent();
            user = new User(u);
            newDimensions = new Pair<int, int>(default, default);
            newDimensions.First = 5;
            newDimensions.Second = 5;
        }

        private void FileButton(object sender, RoutedEventArgs e)
        {
            FileWindow window = new FileWindow(user, newDimensions);
            window.Show();
        }

        private void OptionsButton(object sender, RoutedEventArgs e)
        {
            Options window = new Options(newDimensions.Second, newDimensions.First);
            window.ShowDialog();
            Tuple<int, int> selectedDimensions = window.SelectedDimensions;
            newDimensions.Second = selectedDimensions.Item1;
            newDimensions.First = selectedDimensions.Item2;
        }

        private void HelpButton(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Băjan Ramona-Maria, grupa: 10LF211, specializarea: informatică", "About");
        }

        private void ExitButton(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}

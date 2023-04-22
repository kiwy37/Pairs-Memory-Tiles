using System;
using System.Windows;
using System.Windows.Controls;

namespace Pairs_Bajan_Ramona
{
    /// <summary>
    /// Interaction logic for Options.xaml
    /// </summary>
    public partial class Options : Window
    {
        private int height;
        private int width;

        public Options(int dim1, int dim2)
        {
            InitializeComponent();
            height = dim1;
            width = dim2;
            heightBox.Text = dim1.ToString();
            widthBox.Text = dim2.ToString();
        }

        public Tuple<int, int> SelectedDimensions
        {
            get { return Tuple.Create(height, width); }
        }

        private void HeightText(object sender, TextChangedEventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            if (int.TryParse(textBox.Text, out int result))
            {
                height = result;
            }
        }

        private void WidthText(object sender, TextChangedEventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            if (int.TryParse(textBox.Text, out int result))
            {
                width = result;
            }
        }

        private void SelectButton(object sender, RoutedEventArgs e)
        {
            if (height * width / 2 > 30)
                MessageBox.Show("Dimensiune prea mare!");
            else
                Close();
        }

        private void StandardButton(object sender, RoutedEventArgs e)
        {
            width = 5;
            height = 5;
            widthBox.Text = width.ToString();
            heightBox.Text = height.ToString();
        }
    }
}

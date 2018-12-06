using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace LibraryManager
{
    /// <summary>
    /// Interaction logic for RedeemEbook.xaml
    /// </summary>
    public partial class RedeemEBookWindow : Window
    {
        public EBook Book { get; set; }

        public RedeemEBookWindow()
        {
            InitializeComponent();
        }

        public static EBook RedeemEBook(EBook toRedeem)
        {
            RedeemEBookWindow win = new RedeemEBookWindow() { Book = toRedeem };

            if (toRedeem.Redeemed())
            {
                win.iptRedemptionCode.IsReadOnly = true;
                win.iptRedemptionCode.Background = Brushes.Gray;
            }
            win.ShowDialog();

            return win.Book;
        }

        #region Title bar
        protected void btnClose_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void btnMinimize_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        protected void titleBar_MouseDown(object sender, MouseButtonEventArgs e)
        {
            // Simulates a normal window title bar, but looks a lot better
            if (e.LeftButton == MouseButtonState.Pressed)
                DragMove();
        }

        #endregion

        private void btnDone_Click(object sender, RoutedEventArgs e)
        {
            string eBookCode = iptRedemptionCode.Text;

            DateTime newD = new DateTime(Book.ExpiresOn.Ticks);
            newD.AddDays(14);

            if (!Book.Redeem(iptRedemptionCode.Text, newD))
            {
                MessageBox.Show("Invalid redemption code for E-Book \"" + Book.Name + "\".", "Invalid redemption code", MessageBoxButton.OK, MessageBoxImage.Error);
                iptRedemptionCode.Background = Brushes.Salmon;
            }
            else
            {
                Close();
            }
        }

        private void btnModify_Click(object sender, RoutedEventArgs e)
        {
            ModifyEBook.PollInformation(Book);
            Close();
        }

        private void iptRedemptionCode_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            if (iptRedemptionCode.Background.Equals(Brushes.Salmon))
                iptRedemptionCode.Background = Brushes.White;
        }

        private void btnRemove_Click(object sender, RoutedEventArgs e)
        {
            if(MessageBox.Show("Are you sure you want to remove this E-Book?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {
                Book = null;
                Close();
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace LibraryManager
{
    /// <summary>
    /// Interaction logic for ModifyEBook.xaml
    /// </summary>
    public partial class ModifyEBook : Window
    {
        public EBook Book { get; set; }

        public ModifyEBook()
        {
            InitializeComponent();
        }

        public static void PollInformation(EBook toModify)
        {
            ModifyEBook win = new ModifyEBook() { Book = toModify };
            win.iptName.Text = toModify.Name;
            win.iptRedemptionCode.Text = toModify.RedemptionCode;
            win.dteEBookRenewal.Text = toModify.ExpiresOn.ToShortDateString();

            win.ShowDialog();
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
            if (iptName.Text.Trim().Length == 0)
            {
                iptName.Background = Brushes.Salmon;
            }
            else if (iptRedemptionCode.Text.Trim().Length == 0)
            {
                iptRedemptionCode.Background = Brushes.Salmon;
            }
            else
            {
                if (dteEBookRenewal.SelectedDate == null)
                {
                    dteEBookRenewal.Background = Brushes.Salmon;
                }
                else
                {
                    Book.Name = iptName.Text;
                    Book.RedemptionCode = iptRedemptionCode.Text;
                    Book.ExpiresOn = (DateTime)dteEBookRenewal.SelectedDate;
                    Close();
                }
            }
        }

        private void dteEBookRenewal_CalendarOpened(object sender, RoutedEventArgs e)
        {
            if (dteEBookRenewal.Background.Equals(Brushes.Salmon))
                dteEBookRenewal.Background = Brushes.White;
        }

        private void iptRedemptionCode_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (iptRedemptionCode.Background.Equals(Brushes.Salmon))
                iptRedemptionCode.Background = Brushes.White;
        }

        private void iptName_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (iptName.Background.Equals(Brushes.Salmon))
                iptName.Background = Brushes.White;
        }
    }
}

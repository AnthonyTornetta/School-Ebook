using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace LibraryManager
{
    /// <summary>
    /// Interaction logic for AddStudentWindow.xaml
    /// </summary>
    public partial class AddStudentWindow : Window
    {
        private StudentInfo information = null;

        public AddStudentWindow()
        {
            InitializeComponent();
        }

        public static StudentInfo PollInformation()
        {
            return PollInformation(new StudentInfo("", 0, null), true);
        }

        public static StudentInfo PollInformation(StudentInfo info, bool isEditing = false)
        {
            AddStudentWindow win = new AddStudentWindow();

            if (isEditing)
                win.txtTitle.Text = "Edit Student";

            win.iptName.Text = info.Name;
            win.cboGrade.SelectedIndex = info.Grade;
            win.iptEBookCode.Text = info.EBook != null ? info.EBook.Name : "";

            if (info.EBook != null)
            {
                win.dteEBookRenewal.DisplayDate = info.EBook.ExpiresOn;
                win.dteEBookRenewal.Text = info.EBook.ExpiresOn.ToShortDateString();
            }

            win.information = info;

            win.ShowDialog();

            return win.information;
        }

        #region Title Bar

        private void btnMinimize_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void titleBar_MouseDown(object sender, MouseButtonEventArgs e)
        {
            // Simulates a normal window title bar, but looks a lot better

            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }

        #endregion

        private void btnDone_Click(object sender, RoutedEventArgs e)
        {
            string name = iptName.Text;
            string gradeInput = ((ComboBoxItem)cboGrade.SelectedItem).Content.ToString();

            int grade = 0;
            if(!gradeInput.Equals("Kindergarten")) // Kindergarten = Grade 0
                grade = int.Parse(gradeInput.Substring(gradeInput.IndexOf(' ') + 1)); // It goes "Grade X" so it will get the X (X is a number)

            string eBookCode = iptEBookCode.Text;

            if(name.Trim().Length == 0)
            {
                iptName.Background = Brushes.Salmon;
            }
            else if (eBookCode.Trim().Length == 0)
            {
                iptEBookCode.Background = Brushes.Salmon;
            }
            else
            {
                if (dteEBookRenewal.SelectedDate == null)
                {
                    dteEBookRenewal.Background = Brushes.Salmon;
                }
                else
                {
                    EBook book = new EBook(eBookCode, EBook.GenerateRedmptionCode(), (DateTime)dteEBookRenewal.SelectedDate);
                    information = new StudentInfo(name, grade, book);
                    Close();
                }
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            iptName.Focus();
        }

        private void iptEBookCode_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!iptEBookCode.Background.Equals(Brushes.White))
                iptEBookCode.Background = Brushes.White;
        }

        private void iptName_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!iptName.Background.Equals(Brushes.White))
                iptName.Background = Brushes.White;
        }

        private void dteEBookRenewal_CalendarOpened(object sender, RoutedEventArgs e)
        {
            if (!dteEBookRenewal.Background.Equals(Brushes.White))
                dteEBookRenewal.Background = Brushes.White;
        }

        private void btnRemove_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to remove this student?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {
                information = null;
                Close();
            }
        }
    }
}

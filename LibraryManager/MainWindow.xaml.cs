using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace LibraryManager
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public const string SAVE_FILE_PATH = "ebook-manager/";
        public const string SAVE_DATA_FILE = "save-data.json";

        private List<StudentInfo> info;

        public MainWindow()
        {
            InitializeComponent();
        }

        #region Control Elements

        #region Title bar
        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void btnResize_Click(object sender, RoutedEventArgs e)
        {
            AdjustWindow();
        }

        private void btnMinimize_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void titleBar_MouseDown(object sender, MouseButtonEventArgs e)
        {
            // Simulates a normal window title bar, but looks a lot better

            if (e.ClickCount == 2 && e.RightButton != MouseButtonState.Pressed)
            {
                AdjustWindow();
            }
            else if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }

        #endregion

        #region GUI Functions
        private void btnAddNew_Click(object sender, RoutedEventArgs e)
        {
            StudentInfo info = AddStudentWindow.PollInformation();
            if(info != null && info.EBook != null)
                CreateStudentElement(info);

            scrStudents.ScrollToEnd();
        }

        #endregion

        #endregion

        #region Window Util Functions

        /// <summary>
        /// Maximises or normalizes window based on its current state
        /// </summary>
        private void AdjustWindow()
        {
            if (WindowState == WindowState.Maximized)
                WindowState = WindowState.Normal;
            else
                WindowState = WindowState.Maximized;
        }

        private void Window_Initialized(object sender, EventArgs e)
        {
            Load();

            foreach (StudentInfo student in info)
            {
                CreateStudentElement(student, false);
            }
        }

        private void CreateStudentElement(StudentInfo studentInfo, bool addToList = true)
        {
            RowDefinition def = new RowDefinition() { Height = new GridLength() };
            gridStudents.RowDefinitions.Add(def);

            TextBlock nameTxt = new TextBlock() { Text = studentInfo.Name, FontSize = 16, Margin = new Thickness(0, 10, 0, 0) };
            TextBlock gradeTxt = new TextBlock() { Text = (studentInfo.Grade == 0 ? "K" : "" + studentInfo.Grade), FontSize = 16, Margin = new Thickness(0, 10, 0, 0) };
            TextBlock ebookCodeTxt = new TextBlock() { Text = studentInfo.EBook?.Name, FontSize = 16, Margin = new Thickness(0, 10, 0, 0) };
            ebookCodeTxt.DataContext = studentInfo.EBook;
            nameTxt.DataContext = studentInfo;
            gradeTxt.DataContext = def;

            Grid actions = new Grid();
            actions.RowDefinitions.Add(new RowDefinition());
            actions.RowDefinitions.Add(new RowDefinition());

            Button edit = new Button() { Content = "Edit" };
            edit.Style = FindResource("btn-slick-tiny") as Style;
            edit.Click += Edit_Click;

            Button view = new Button() { Content = "E-Book" };
            view.Style = FindResource("btn-slick-tiny") as Style;
            view.Click += View_Click;

            int row = gridStudents.RowDefinitions.Count - 1;
            Grid.SetRow(nameTxt, row);
            Grid.SetRow(gradeTxt, row);
            Grid.SetRow(ebookCodeTxt, row);
            Grid.SetRow(edit, row);
            Grid.SetRow(view, row);

            Grid.SetColumn(nameTxt, 0);
            Grid.SetColumn(gradeTxt, 1);
            Grid.SetColumn(ebookCodeTxt, 2);
            Grid.SetColumn(edit, 3);
            Grid.SetColumn(view, 4);

            gridStudents.Children.Add(nameTxt);
            gridStudents.Children.Add(gradeTxt);
            gridStudents.Children.Add(ebookCodeTxt);
            gridStudents.Children.Add(edit);
            gridStudents.Children.Add(view);

            if (addToList)
            {
                info.Add(studentInfo);
                Print.AddAction(studentInfo.Name, studentInfo.EBook.Name, PrintAction.GrantEBook, DateTime.Now);
            }

            Save();
        }

        private void View_Click(object sender, RoutedEventArgs e)
        {
            Button clicked = (Button)e.Source;
            List<UIElement> children = GetChildrenInRow(Grid.GetRow(clicked), gridStudents);

            TextBlock eBookBox = (TextBlock)children[2];
            TextBlock si = (TextBlock)children[0];
            StudentInfo info = (StudentInfo)si.DataContext;

            EBook book = (EBook)eBookBox.DataContext;

            if (book != null)
            {
                string name = book.Name;
                info.EBook = RedeemEBookWindow.RedeemEBook(book);

                if (info.EBook != null)
                {
                    eBookBox.Text = book.Name;
                    eBookBox.DataContext = book;
                }
                else
                {
                    eBookBox.Text = "";
                    eBookBox.DataContext = null;

                    Print.AddAction(((TextBlock)children[0]).Text, name, PrintAction.RemoveEBook, DateTime.Now);
                }

                Save();
            }
            else
            {
                MessageBox.Show("This student has no E-Book. Press Edit to add one.", "No E-Book", MessageBoxButton.OK, MessageBoxImage.Asterisk);
            }
        }

        private void Edit_Click(object sender, RoutedEventArgs e)
        {
            Button clicked = (Button)e.Source;
            List<UIElement> children = GetChildrenInRow(Grid.GetRow(clicked), gridStudents);

            TextBlock nameBox = (TextBlock)children[0];
            TextBlock gradeBox = (TextBlock)children[1];
            TextBlock eBookBox = (TextBlock)children[2];

            StudentInfo studentInfo = new StudentInfo()
            {
                Name = nameBox.Text,
                Grade = !gradeBox.Text.Equals("K") ? int.Parse(gradeBox.Text) : 0,
                EBook = (EBook)eBookBox.DataContext
            };

            info.Remove(studentInfo);

            string name = studentInfo.Name;
            string eBook = studentInfo.EBook?.Name;

            studentInfo = AddStudentWindow.PollInformation(studentInfo);

            if (studentInfo != null)
            {
                nameBox.Text = studentInfo.Name;
                nameBox.DataContext = studentInfo;
                gradeBox.Text = studentInfo.Grade != 0 ? studentInfo.Grade + "" : "K";
                eBookBox.Text = studentInfo.EBook.Name;
                eBookBox.DataContext = studentInfo.EBook;

                info.Add(studentInfo);
                Save();

                if (!studentInfo.EBook.Name.Equals(eBook))
                {
                    Print.AddAction(name, eBook, PrintAction.RemoveEBook, DateTime.Now);
                    Print.AddAction(studentInfo.Name, studentInfo.EBook.Name, PrintAction.GrantEBook, DateTime.Now);
                }
            }
            else
            {
                Print.AddAction(name, eBook, PrintAction.RemoveEBook, DateTime.Now);

                RowDefinition def = (RowDefinition)gradeBox.DataContext;

                foreach (UIElement item in children)
                    gridStudents.Children.Remove(item);

                gridStudents.RowDefinitions.Remove(def);

                Save();
            }
        }

        private static List<UIElement> GetChildrenInRow(int r, Grid g)
        {
            List<UIElement> elems = new List<UIElement>();

            for (int i = 0; i < g.Children.Count; i++)
            {
                UIElement e = g.Children[i];
                if (Grid.GetRow(e) == r)
                    elems.Add(e);
            }
            return elems;
        }

        #endregion
        
        public void Load()
        {
            if (File.Exists(SAVE_FILE_PATH + SAVE_DATA_FILE))
            {
                string json = File.ReadAllText(SAVE_FILE_PATH + SAVE_DATA_FILE);

                info = JsonConvert.DeserializeObject<List<StudentInfo>>(json);
            }
            else
                info = new List<StudentInfo>();

            Print.Load();
        }

        private void Save()
        {
            Directory.CreateDirectory("ebook-manager");
            File.WriteAllText(SAVE_FILE_PATH + SAVE_DATA_FILE, JsonConvert.SerializeObject(info));
            Print.Save();
        }

        private void btnPrint_Click(object sender, RoutedEventArgs e)
        {
            Print.PrintData();
        }
    }
}

using Microsoft.Win32;
using System;
using System.IO;
using System.Text;
using System.Windows;

namespace LibraryManager
{
    class Print
    {
        private Print() { }

        private static StringBuilder log;

        private static void Init()
        {
            log = new StringBuilder("Student name,E-Book name,Action,Time (M/D/YYYY H:MM)");
        }

        public static void PrintData()
        {
            if (log.ToString().Length != 0)
            {
                SaveFileDialog savePrintData = new SaveFileDialog()
                {
                    Filter = "CSV File|*.csv",
                    Title = "Save data into csv file"
                };

                savePrintData.ShowDialog();
                if (savePrintData.FileName.Trim().Length != 0)
                {
                    try
                    {
                        File.WriteAllText(savePrintData.FileName, log.ToString());
                    }
                    catch(IOException)
                    {
                        MessageBox.Show("Unable to write file! Is that file currently open in another program?", "Unable to write file!", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
        }

        public static void Save()
        {
            File.WriteAllText(MainWindow.SAVE_FILE_PATH + "print-data.txt", log.ToString());
        }

        public static void Load()
        {
            if (File.Exists(MainWindow.SAVE_FILE_PATH + "print-data.txt"))
            {
                log = new StringBuilder(File.ReadAllText(MainWindow.SAVE_FILE_PATH + "print-data.txt"));
            }
            else
            {
                Init();
            }
        }

        public static void AddAction(string studentName, string eBookName, PrintAction action, DateTime when)
        {
            log.Append('\n' + studentName + ',' + eBookName + ',' + ActionString(action) + ',' + when.ToShortDateString() + ' ' + when.ToShortTimeString());
        }

        private static string ActionString(PrintAction act)
        {
            switch(act)
            {
                case PrintAction.RemoveEBook:
                    return "Removed";
                case PrintAction.GrantEBook:
                    return "Given";
                default:
                    return "?";
            }
        }
    }

    enum PrintAction
    {
        RemoveEBook,
        GrantEBook
    }
}

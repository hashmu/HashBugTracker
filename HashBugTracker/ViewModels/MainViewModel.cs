using HashBugTracker.Models;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace HashBugTracker.ViewModels
{
    class MainViewModel : BaseViewModel
    {
        #region Constructors
        public MainViewModel()
        {
            SelectDatabase();
        }
        #endregion

        #region Members
        SessionManager sessionManager = new SessionManager();
        #endregion


        #region Methods
        private void SelectDatabase()
        {
            MessageBoxResult result = MessageBox.Show("Yes To Open File, No For Default", "Choose Database", MessageBoxButton.YesNo);
            if (result == MessageBoxResult.Yes)
            {
                OpenFileDialog dialog = new OpenFileDialog();
                dialog.Filter = "Database|*.db|All files (*.*)|*.*";
                bool? dialogResult = dialog.ShowDialog();

                if (dialogResult == true)
                {
                    sessionManager.SQLite = new SQLiteManager(dialog.FileName);
                }
            }
        }
        #endregion
    }
}

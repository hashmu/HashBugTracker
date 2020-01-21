using HashBugTracker.Models;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows;

namespace HashBugTracker.ViewModels
{
    class StatusBarViewModel : BaseViewModel
    {
        #region Constructors
        public StatusBarViewModel()
        {
            instance ??= this;
        }
        #endregion


        #region Members
        public static StatusBarViewModel instance;

        private string _statusText = "";
        public string StatusText
        {
            get
            {
                return _statusText;
            }
            set
            {
                _statusText = value;
                OnPropertyChanged("StatusText");
            }
        }
        #endregion
    }
}

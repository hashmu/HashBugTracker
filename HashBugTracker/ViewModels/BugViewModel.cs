using HashBugTracker.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;
using System.Windows.Input;

namespace HashBugTracker.ViewModels
{
    class BugViewModel : BaseViewModel
    {
        #region Members
        private SQLiteManager sqliteManager
        {
            get
                {
                    return SessionManager.Instance.SQLite;
                }
        }

        private Bug _selected;
        public Bug Selected
        {
            get
            {
                return _selected;
            }
            set
            {
                if (value != null)
                {
                    _selected = value;
                }
                OnPropertyChanged("Selected");
                DisplaySelected();
            }
        }

        private string _bugLabel = "";
        public string BugLabel
        {
            get
            {
                return _bugLabel;
            }
            set
            {
                _bugLabel = value;
                OnPropertyChanged("BugLabel");

            }
        }

        private int _bugSeverity = 0;
        public int BugSeverity
        {
            get
            {
                return _bugSeverity;
            }
            set
            {
                _bugSeverity = value;
                OnPropertyChanged("BugSeverity");
            }
        }

        private string _bugDescription = "";
        public string BugDescription
        {
            get
            {
                return _bugDescription;
            }
            set
            {
                _bugDescription = value;
                OnPropertyChanged("BugDescription");
            }
        }

        private string _bugNotes = "";
        public string BugNotes
        {
            get
            {
                return _bugNotes;
            }
            set
            {
                _bugNotes = value;
                OnPropertyChanged("BugNotes");
            }
        }

        private bool _bugSolved = false;
        public bool BugSolved
        {
            get
            {
                return _bugSolved;
            }
            set
            {
                _bugSolved = value;
                OnPropertyChanged("BugSolved");
            }
        }

        private bool _showSolved = false;
        public bool ShowSolved
        {
            get
            {
                return _showSolved;
            }
            set
            {
                OnPropertyChanged("ShowSolved");
                if (value)
                {
                    _showSolved = true;
                    _showUnsolved = false;
                    ShowBugs();
                }
            }
        }

        private bool _showUnsolved = true;
        public bool ShowUnsolved
        {
            get
            {
                return _showUnsolved;
            }
            set
            {
                OnPropertyChanged("ShowUnsolved");
                if (value)
                {
                    _showUnsolved = true;
                    _showSolved = false;
                    ShowBugs();
                }
            }
        }

        private bool _showAll = false;
        public bool ShowAll
        {
            get
            {
                return _showAll;
            }
            set
            {
                _showAll = value;
                OnPropertyChanged("ShowAll");
                if (_showAll)
                {
                    _showSolved = false;
                    OnPropertyChanged("ShowSolved");
                    _showUnsolved = false;
                    OnPropertyChanged("ShowUnsolved");
                    ShowBugs();
                }
            }
        }

        private readonly ObservableCollection<Bug> _bugList = new ObservableCollection<Bug>();
        #endregion


        #region Constructors
        public BugViewModel()
        {
            ShowBugs();
        }
        #endregion

        #region Commands and Enumerables
        public IEnumerable<Bug> BugList
        {
            get { return _bugList; }
        }
        
        public ICommand AddBugCommand
        {
            get { return new DelegateCommand(AddBug); }
        }

        public ICommand ShowBugsCommand
        {
            get { return new DelegateCommand(ShowBugs); }
        }

        public ICommand DeleteBugCommand
        {
            get { return new DelegateCommand(DeleteBug); }
        }

        public ICommand UpdateBugCommand
        {
            get { return new DelegateCommand(UpdateSelected); }
        }

        public ICommand SaveBugCommand
        {
            get { return new DelegateCommand(SaveBug); }
        }

        public ICommand NewBugCommand
        {
            get { return new DelegateCommand(ClearSelected); }
        }
        #endregion

        #region Methods
        async void AddBug()
        {
            await sqliteManager.AddBug(BugLabel, BugDescription, BugNotes, (Severity)BugSeverity);
            StatusBarViewModel.instance.StatusText = "Bug Added";
            ShowBugs();
        }

        async void ShowBugs()
        {
            Bug[] bugs = await sqliteManager.GetBugs();
            _bugList.Clear();
            foreach(Bug bug in bugs)
            {
                if (_showAll || (_showSolved && bug.Solved == 1) || (_showUnsolved && bug.Solved == 0))
                {
                    _bugList.Add(bug);
                }
            }
        }

        async void DeleteBug()
        {
            if (_selected != null)
            {
                System.Windows.MessageBoxResult result = System.Windows.MessageBox.Show("Are You Sure?", "Delete Bug", System.Windows.MessageBoxButton.YesNo);
                if (result == System.Windows.MessageBoxResult.Yes)
                {
                    await sqliteManager.DeleteBug(_selected);
                    StatusBarViewModel.instance.StatusText = "Bug Deleted";
                    ShowBugs();
                    BugLabel = "";
                    BugDescription = "";
                    BugNotes = "";
                    BugSeverity = 0;
                    BugSolved = false;
                    _selected = null;
                }
            }
        }

        private void DisplaySelected()
        {
            if (_selected != null)
            {
                BugLabel = _selected.Label;
                BugDescription = _selected.Description;
                BugNotes = _selected.Notes;
                BugSeverity = _selected.Severity;
                BugSolved = (_selected.Solved == 1) ? true : false;
            }
        }

        private async void UpdateSelected()
        {
            if (_selected != null)
            {
                _selected.Label = BugLabel;
                _selected.Description = BugDescription;
                _selected.Notes = BugNotes;
                _selected.Severity = BugSeverity;
                _selected.Solved = BugSolved ? 1 : 0;
                await sqliteManager.UpdateBug(_selected);
                ShowBugs();
            }
        }

        private void SaveBug()
        {
            if (_selected == null)
            {
                AddBug();
            }
            else
            {
                UpdateSelected();
            }
        }

        private void ClearSelected()
        {
            _selected = null;
            BugLabel = "";
            BugDescription = "";
            BugNotes = "";
            BugSeverity = (int)Severity.Low;
            BugSolved = false;
        }
        #endregion
    }
}

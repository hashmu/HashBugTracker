using HashBugTracker.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;
using System.Windows.Input;

namespace HashBugTracker.ViewModels
{
    class FeatureViewModel : BaseViewModel
    {
        #region Members
        private SQLiteManager sqliteManager
        {
            get
            {
                return SessionManager.Instance.SQLite;
            }
        }

        private Feature _selected;
        public Feature Selected
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

        private string _featureLabel = "";
        public string FeatureLabel
        {
            get
            {
                return _featureLabel;
            }
            set
            {
                _featureLabel = value;
                OnPropertyChanged("FeatureLabel");

            }
        }

        private int _featurePriority = 0;
        public int FeaturePriority
        {
            get
            {
                return _featurePriority;
            }
            set
            {
                _featurePriority = value;
                OnPropertyChanged("FeaturePriority");
            }
        }

        private string _featureDescription = "";
        public string FeatureDescription
        {
            get
            {
                return _featureDescription;
            }
            set
            {
                _featureDescription = value;
                OnPropertyChanged("FeatureDescription");
            }
        }

        private string _featureNotes = "";
        public string FeatureNotes
        {
            get
            {
                return _featureNotes;
            }
            set
            {
                _featureNotes= value;
                OnPropertyChanged("FeatureNotes");
            }
        }

        private bool _featureCompleted = false;
        public bool FeatureCompleted
        {
            get
            {
                return _featureCompleted;
            }
            set
            {
                _featureCompleted = value;
                OnPropertyChanged("FeatureCompleted");
            }
        }

        private bool _showComplete = false;
        public bool ShowComplete
        {
            get
            {
                return _showComplete;
            }
            set
            {
                OnPropertyChanged("ShowComplete");
                if (value)
                {
                    _showComplete = true;
                    _showIncomplete = false;
                    ShowFeatures();
                }
            }
        }

        private bool _showIncomplete = true;
        public bool ShowIncomplete
        {
            get
            {
                return _showIncomplete;
            }
            set
            {
                OnPropertyChanged("ShowIncomplete");
                if (value)
                {
                    _showIncomplete = true;
                    _showComplete = false;
                    ShowFeatures();
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
                    _showComplete = false;
                    OnPropertyChanged("ShowComplete");
                    _showIncomplete = false;
                    OnPropertyChanged("ShowIncomplete");
                    ShowFeatures();
                }
            }
        }

        private readonly ObservableCollection<Feature> _featureList = new ObservableCollection<Feature>();
        #endregion


        #region Constructors
        public FeatureViewModel()
        {
            ShowFeatures();
        }
        #endregion

        #region Commands
        public IEnumerable<Feature> FeatureList
        {
            get { return _featureList; }
        }

        public ICommand AddFeatureCommand
        {
            get { return new DelegateCommand(AddFeature); }
        }

        public ICommand ShowCommand
        {
            get { return new DelegateCommand(ShowFeatures); }
        }

        public ICommand DeleteCommand
        {
            get { return new DelegateCommand(DeleteFeature); }
        }

        public ICommand UpdateCommand
        {
            get { return new DelegateCommand(UpdateSelected); }
        }

        public ICommand SaveCommand
        {
            get { return new DelegateCommand(SaveFeature); }
        }

        public ICommand NewCommand
        {
            get { return new DelegateCommand(ClearSelected); }
        }
        #endregion

        #region Methods
        async void AddFeature()
        {
            await sqliteManager.AddFeature(FeatureLabel, FeatureDescription, FeatureNotes, (Priority)FeaturePriority);
            StatusBarViewModel.instance.StatusText = "Feature Added";
            ShowFeatures();
        }

        async void ShowFeatures()
        {
            Feature[] features = await sqliteManager.GetFeatures();
            _featureList.Clear();
            foreach (Feature feature in features)
            {
                if (_showAll || (_showComplete && feature.Completed == 1) || (_showIncomplete && feature.Completed == 0))
                {
                    _featureList.Add(feature);
                }
            }
        }

        async void DeleteFeature()
        {
            if (_selected != null)
            {
                System.Windows.MessageBoxResult result = System.Windows.MessageBox.Show("Are You Sure?", "Delete Feature", System.Windows.MessageBoxButton.YesNo);
                if (result == System.Windows.MessageBoxResult.Yes)
                {
                    await sqliteManager.Delete(_selected);
                    StatusBarViewModel.instance.StatusText = "Feature Deleted";
                    ShowFeatures();
                    FeatureLabel = "";
                    FeatureDescription = "";
                    FeaturePriority = 0;
                    FeatureCompleted = false;
                    _selected = null;
                }
            }
        }

        private void DisplaySelected()
        {
            if (_selected != null)
            {
                FeatureLabel = _selected.Label;
                FeatureDescription = _selected.Description;
                FeatureNotes = _selected.Notes;
                FeaturePriority = _selected.Priority;
                FeatureCompleted = (_selected.Completed == 1) ? true : false;
            }
        }

        private async void UpdateSelected()
        {
            if (_selected != null)
            {
                _selected.Label = FeatureLabel;
                _selected.Description = FeatureDescription;
                _selected.Notes = FeatureNotes;
                _selected.Priority = FeaturePriority;
                _selected.Completed = FeatureCompleted ? 1 : 0;
                await sqliteManager.Update(_selected);
                ShowFeatures();
            }
        }

        private void SaveFeature()
        {
            if (_selected == null)
            {
                AddFeature();
            }
            else
            {
                UpdateSelected();
            }
        }

        private void ClearSelected()
        {
            _selected = null;
            FeatureLabel = "";
            FeatureDescription = "";
            FeatureNotes = "";
            FeaturePriority = (int)Priority.Low;
            FeatureCompleted = false;
        }
        #endregion
    }
}

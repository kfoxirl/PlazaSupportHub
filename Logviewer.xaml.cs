using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using LiveLogViewer.Properties;
using Microsoft.Win32;

namespace LiveLogViewer
{
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        private readonly IEnumerable<EncodingInfo> _encodings = Encoding.GetEncodings().OrderBy(e => e.DisplayName);
        private readonly ObservableCollection<FileMonitorViewModel> _fileMonitors = new ObservableCollection<FileMonitorViewModel>();
        private readonly Timer _refreshTimer;
        private string _font;
        private DateTime? _lastUpdateDateTime;
        private string _lastUpdated;
        private FileMonitorViewModel _lastUpdatedViewModel;
        private FileMonitorViewModel _selectedItem;

        public MainWindow()
        {
            InitializeComponent();

            // Default setting
            Font = Settings.Default.Font;

            this.DataContext = this;

            _refreshTimer = CreateRefreshTimer();
            this.ContentRendered += (s, e) => PromptForFile();
        }

        /// <summary>
        ///     Gets the available encodings.
        /// </summary>
        /// <value>
        ///     The encodings.
        /// </value>
        public IEnumerable<EncodingInfo> Encodings
        {
            get { return _encodings; }
        }

        public string LastUpdated
        {
            get { return _lastUpdated; }
            set
            {
                if (value == _lastUpdated) return;
                _lastUpdated = value;
                OnPropertyChanged();
            }
        }

        public FileMonitorViewModel SelectedItem
        {
            get { return _selectedItem; }
            set
            {
                if (Equals(value, _selectedItem)) return;
                _selectedItem = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<FileMonitorViewModel> FileMonitors
        {
            get { return _fileMonitors; }
        }

        public string Font
        {
            get { return _font; }
            set
            {
                if (value == _font) return;
                _font = value;
                OnPropertyChanged();
            }
        }

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        private Timer CreateRefreshTimer()
        {
            var timer = new Timer(state => RefreshLastUpdatedText());
            timer.Change((DateTime.Now.Date.AddDays(1) - DateTime.Now), TimeSpan.FromDays(1));
            this.Closing += DisposeTimer;
            return timer;
        }

        private void DisposeTimer(object s, CancelEventArgs e)
        {
            _refreshTimer.Change(Timeout.InfiniteTimeSpan, Timeout.InfiniteTimeSpan);
            _refreshTimer.Dispose();
        }

        private void AddFileMonitor(string filepath)
        {
            var existingMonitor = FileMonitors.FirstOrDefault(m => string.Equals(m.FilePath, filepath, StringComparison.CurrentCultureIgnoreCase));

            if (existingMonitor != null)
            {
                // Already being monitored
                SelectedItem = existingMonitor;
                return;
            }

            var monitorViewModel = new FileMonitorViewModel(filepath, GetFileNameForPath(filepath), Settings.Default.DefaultEncoding, Settings.Default.BufferedRead);
            monitorViewModel.Renamed += MonitorViewModelOnRenamed;
            monitorViewModel.Updated += MonitorViewModelOnUpdated;

            FileMonitors.Add(monitorViewModel);
            SelectedItem = monitorViewModel;
        }

        private void MonitorViewModelOnUpdated(FileMonitorViewModel obj)
        {
            _lastUpdateDateTime = DateTime.Now;
            _lastUpdatedViewModel = obj;
            RefreshLastUpdatedText();
        }

        private void MonitorViewModelOnRenamed(FileMonitorViewModel renamedViewModel)
        {
            var filepath = renamedViewModel.FilePath;

            renamedViewModel.FileName = GetFileNameForPath(filepath);
        }

        private static string GetFileNameForPath(string filepath)
        {
            return Path.GetFileName(filepath);
        }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            var handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }

        private void AddButton_OnClick(object sender, RoutedEventArgs e)
        {
            PromptForFile();
        }

        private void PromptForFile()
        {
            var openFileDialog = new OpenFileDialog {CheckFileExists = false, Multiselect = true};

            if (openFileDialog.ShowDialog() == true)
            {
                try
                {
                    foreach (var fileName in openFileDialog.FileNames)
                    {
                        AddFileMonitor(fileName);
                    }
                }
                catch (Exception exception)
                {
                    MessageBox.Show("Error: " + exception.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void ClearButton_OnClick(object sender, RoutedEventArgs e)
        {
            if (SelectedItem != null)
            {
                SelectedItem.Contents = string.Empty;
            }
        }

        private void FreezeButton_OnClick(object sender, RoutedEventArgs e)
        {
            if (SelectedItem != null)
            {
                SelectedItem.IsFrozen = !SelectedItem.IsFrozen;
            }
        }

        private void RemoveButton_OnClick(object sender, RoutedEventArgs e)
        {
            if (SelectedItem != null)
            {
                SelectedItem.Dispose();
                FileMonitors.Remove(SelectedItem);
            }
        }

        private void TextBoxBase_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            var textBox = (TextBox) (sender);
            var fileMonitorViewModel = ((FileMonitorViewModel) textBox.DataContext);

            if (fileMonitorViewModel != null)
            {
                if (!fileMonitorViewModel.IsFrozen)
                {
                    textBox.ScrollToEnd();
                }
            }
        }

        private void RefreshLastUpdatedText()
        {
            if (_lastUpdateDateTime != null)
            {
                var dateTime = _lastUpdateDateTime.Value;
                var datestring = dateTime.Date != DateTime.Now.Date ? " on " + dateTime : " at " + dateTime.ToLongTimeString();
                LastUpdated = _lastUpdatedViewModel.FilePath + datestring;
            }
        }

        private void ConfigButton_OnClick(object sender, RoutedEventArgs e)
        {
            new ConfigurationWindow(this) {Owner = this}.ShowDialog();
        }
    }
}
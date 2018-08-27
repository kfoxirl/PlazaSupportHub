using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using LiveLogViewer.Properties;

namespace LiveLogViewer
{
    /// <summary>
    /// Interaction logic for ConfigurationWindow.xaml
    /// </summary>
    public partial class ConfigurationWindow : Window
    {
        private readonly MainWindow _mainWindow;

        public ConfigurationWindow(MainWindow mainWindow)
        {
            _mainWindow = mainWindow;
            InitializeComponent();

            var fonts = Fonts.SystemFontFamilies.Select(f=>f.ToString()).OrderBy(s=>s);

            FontListBox.ItemsSource = fonts;
            FontListBox.SelectedItem = fonts.FirstOrDefault(f => f.Equals(Settings.Default.Font));

            this.Loaded += (sender, args) =>
            {
                if (FontListBox.SelectedItem != null)
                {
                    FontListBox.ScrollIntoView(FontListBox.SelectedItem);
                }
            };

            var encodings = Encoding.GetEncodings();

            EncodingComboBox.ItemsSource = encodings.OrderBy(e=>e.DisplayName);
            EncodingComboBox.SelectedItem = encodings.FirstOrDefault(f => f.Name.Equals(Settings.Default.DefaultEncoding, StringComparison.CurrentCultureIgnoreCase));

            BufferedReadCheckBox.IsChecked = Settings.Default.BufferedRead;
        }

        private void FontListBox_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedItem = FontListBox.SelectedItem;
            
            if (selectedItem != null)
            {
                _mainWindow.Font = selectedItem.ToString();
                Settings.Default.Font = selectedItem.ToString();
                Settings.Default.Save();
            }
        }

        private void EncodingComboBox_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedItem = EncodingComboBox.SelectedItem;

            if (selectedItem != null)
            {
                var encodingInfo = ((EncodingInfo)selectedItem);

                Settings.Default.DefaultEncoding = encodingInfo.Name;
                Settings.Default.Save();
            }
        }

        private void OKButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void BufferedReadToggleButton_OnChecked(object sender, RoutedEventArgs e)
        {
            var bufferedRead = BufferedReadCheckBox.IsChecked ?? false; 
            Settings.Default.BufferedRead = bufferedRead;
            Settings.Default.Save();

            foreach (var fileMonitor in _mainWindow.FileMonitors)
            {
                fileMonitor.BufferedRead = bufferedRead;
            }
        }
    }
}

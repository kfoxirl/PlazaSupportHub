using PlazaConnectivityChecker.Models;
using System;
using System.Collections.ObjectModel;
using PlazaConnectivityChecker.Views;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using System.Windows.Navigation;
using CustomLoader;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Markup;
using System.Diagnostics;

namespace PlazaConnectivityChecker
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        PortTestResults portTestResults = new PortTestResults();

        public MainWindow()
        {
            
            this.InitializeComponent();
        }

        private void ListBoxItem_Selected(object sender, RoutedEventArgs e)
        {
            menuLB.SelectedIndex = -1;
            contentFrame.Source = new Uri("Views/Logviewer.xaml", UriKind.RelativeOrAbsolute);
            contentFrame.NavigationUIVisibility = NavigationUIVisibility.Hidden;
        }

        private void ListBoxItem_Selected_1(object sender, RoutedEventArgs e)
        {
            menuLB.SelectedIndex = -1;
            contentFrame.Source = new Uri("Views/NetworkConnections.xaml", UriKind.RelativeOrAbsolute);
            contentFrame.NavigationUIVisibility = NavigationUIVisibility.Hidden;
        }

        private void ListBoxItem_Selected_2(object sender, RoutedEventArgs e)
        {
            menuLB.SelectedIndex = -1;

            OpenPortCheck oPC = new OpenPortCheck(portTestResults);
            this.contentFrame.Navigate(oPC);
            contentFrame.NavigationUIVisibility = NavigationUIVisibility.Hidden;
          
        }
    }

    public class DebugConverter : IValueConverter
    {
        public static DebugConverter Instance = new DebugConverter();
        private DebugConverter() { }

        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Debugger.Break();
            return value; //Binding.DoNothing;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Debugger.Break();
            return value; //Binding.DoNothing;
        }

        #endregion
    }

    public class DebugExtension : MarkupExtension
    {
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return DebugConverter.Instance;
        }
    }


    public class StringToImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
    {
        if (value != null)
        {
            string imagename = value as string;
            return new BitmapImage(new Uri(string.Format(@"..\..\Image\{0}", imagename), UriKind.Relative));
        }
        return null;
    }
    public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}

[Localizability(LocalizationCategory.NeverLocalize)]
    public sealed class BooleanToVisibilityConverter : IValueConverter
    {
        /// <summary>
        /// Convert bool or Nullable&lt;bool&gt; to Visibility
        /// </summary>
        /// <param name="value">bool or Nullable&lt;bool&gt;</param>
        /// <param name="targetType">Visibility</param>
        /// <param name="parameter">null</param>
        /// <param name="culture">null</param>
        /// <returns>Visible or Collapsed</returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool bValue = false;
            if (value is bool)
            {
                if ((bool)value == true)
                {
                    bValue = false;
                    return Visibility.Collapsed;
                }

                else if ((bool)value == false)
                {
                    bValue = true;
                    return Visibility.Visible;
                }
            }
            else if (value is Nullable<bool>)
            {
                Nullable<bool> tmp = (Nullable<bool>)value;
                bValue = tmp.HasValue ? tmp.Value : false;
            }

            return Visibility.Collapsed;
        }

        /// <summary>
        /// Convert Visibility to boolean
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Visibility)
            {
                return (Visibility)value == Visibility.Visible;
            }
            else
            {
                return false;
            }
        }
    }


    //public void MyMethodWhichTakesLongTime()
    //{
    //    IsLoading = true;

    //    PortTestResults portTestResults = new PortTestResults();

    //    var bw = new BackgroundWorker();
    //    bw.DoWork += (o, args) => PortTestResults.CheckPort();
    //    bw.RunWorkerCompleted += new RunWorkerCompletedEventHandler(backgroundWorker1_RunWorkerCompleted);
    //    bw.RunWorkerAsync();


    //}

    //private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
    //{
    //    IsLoading = false;
    //}





    public class BindingProxy : Freezable
    {
        #region Overrides of Freezable

        protected override Freezable CreateInstanceCore()
        {
            return new BindingProxy();
        }

        #endregion
        
        public object Data
        {
            get { return (object)GetValue(DataProperty); }
            set { SetValue(DataProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Data.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DataProperty =
            DependencyProperty.Register("Data", typeof(object), typeof(BindingProxy), new UIPropertyMetadata(null));
    }

    public class NumToBoolConverter : IValueConverter
    {

        #region IValueConverter Members

        public object Convert(object value, Type targetType,
            object parameter, System.Globalization.CultureInfo culture)
        {
            if (value != null && value is int)
            {
                var val = (int)value;
                return (val == 0) ? false : true;
            }
            return null;

        }

        public object ConvertBack(object value, Type targetType,
            object parameter, System.Globalization.CultureInfo culture)
        {
            if (value != null && value is bool)
            {
                var val = (bool)value;
                return val ? 1 : 0;
            }
            return null;
        }

        #endregion
    }

    public class IntegerConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values != null)
            {
                foreach (var item in (ObservableCollection<int>)values[0]) ;
                return values[0];
            }

            else
            {
                return values;
            }
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            string[] splitValues = ((string)value).Split(' ');
            return splitValues;
        }
    }
}

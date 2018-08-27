using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using PlazaConnectivityChecker.Models;
using PlazaConnectivityChecker.Networking;

namespace PlazaConnectivityChecker.Views
{
    /// <summary>
    /// Interaction logic for OpenPortCheck.xaml
    /// 
    /// // To parse this JSON data, add NuGet 'Newtonsoft.Json' then do:
    //
    //    using QuickType;
    //
    //    var ports = Ports.FromJson(jsonString);
    /// </summary>
    public partial class OpenPortCheck : Page
    {
        public OpenPortCheck(PortTestResults portTestResults)
        {
            InitializeComponent();
            
            Style = GetStyle("Window-Black");
            PortCheckResult.Style = GetStyle("List-Modern");
            this.DataContext = portTestResults;
            PortCheckResult.ItemsSource = portTestResults;
            DataService ds = new DataService();
            ds.GetPortInfoInBackground(portTestResults);
        }






        private static Style GetStyle(string key)
        {
            var resource = Application.Current.FindResource(key);
            return resource as Style;
        }

        public class DataService
        {

            public void GetPortInfoInBackground(PortTestResults portTestResults)
            {
                PortCheck portCheck = new PortCheck();               
                var bw = new BackgroundWorker();
                bw.DoWork += (o, args) => portCheck.CheckPort(portTestResults);
                
                bw.RunWorkerAsync();


            }


        }










    }
}



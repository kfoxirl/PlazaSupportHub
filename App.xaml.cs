using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Threading;
using PlazaConnectivityChecker.Models;

namespace PlazaConnectivityChecker
{
    /// <summary>
    ///     Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private static readonly List<Exception> HandledExceptions = new List<Exception>();

     
        private void App_OnStartup(object sender, StartupEventArgs e)
        {
            this.DispatcherUnhandledException += OnDispatcherUnhandledException;
            AppDomain.CurrentDomain.UnhandledException += CurrentDomainOnUnhandledException;
        }

        private void CurrentDomainOnUnhandledException(object sender, UnhandledExceptionEventArgs unhandledExceptionEventArgs)
        {
            OnUnhandledException(unhandledExceptionEventArgs.ExceptionObject as Exception);
        }

        private void OnDispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs dispatcherUnhandledExceptionEventArgs)
        {
            OnUnhandledException(dispatcherUnhandledExceptionEventArgs.Exception);
        }

        private void OnUnhandledException(Exception exception)
        {
            if (HandledExceptions.Contains(exception))
            {
                return;
            }

            MessageBox.Show(MainWindow, "Error: " + exception.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Xml;
using PlazaConnectivityChecker.Models;
using PlazaConnectivityChecker.Networking;


namespace PlazaConnectivityChecker.Models
{
    public class PortTestResults : ObservableCollection<PortTestResult>
    {
        public PortTestResults()
        {
            PopulateTestResults();
        }

        
        //public static ObservableCollection<PortTestResult> PortTestResults { get; set; }

        public void PopulateTestResults()
        {
            this.Add(new PortTestResult { Owner = "Windows Server", ServerClient = "Server", Logo = "/Logos/Logo-WinServer.png", Success = "/Images/success-tick.jpg", Failure = "/Images/failure-x.png", Protocol = "TCP", Description = "NTP stands for Network Time Protocol, and it is an Internet protocol used to synchronize the clocks of computers to some time reference", PortRole = "NTP", PortNumbers = { 80 }, PortAvailable = 0, AvailablePorts = { }, UnavailablePorts = { } });
            this.Add(new PortTestResult { Owner = "Windows Server", ServerClient = "Server", Logo = "/Logos/Logo-WinServer.png", Success = "/Images/success-tick.jpg", Failure = "/Images/failure-x.png", Protocol = "TCP", PortRole = "Windows File & Print Services", PortNumbers = { 139, 445 }, PortAvailable = 0, AvailablePorts = { }, UnavailablePorts = { } });
            this.Add(new PortTestResult { Owner = "Windows Server", ServerClient = "Server", Logo = "/Logos/Logo-WinServer.png", Success = "/Images/success-tick.jpg", Failure = "/Images/failure-x.png", Protocol = "TCP", PortRole = "LDAP & LDAP SSL", PortNumbers = { 389, 636 }, PortAvailable = 0, AvailablePorts = { }, UnavailablePorts = { } });

        }





        public static void CheckPort()
        {
            PortTestResults portTestResults = new PortTestResults();

            PortCheck pc = new PortCheck();
            string server = "216.58.219.206";
            foreach (PortTestResult ptr in portTestResults)
            {
                try
                {
                    if (ptr.Protocol == "TCP")
                    {
                        try
                        {
                            foreach (int p in ptr.PortNumbers)
                            try
                            {
                                    ptr.PortAvailable = pc.TcpIsListening(server, p);
                                    ptr.PortAvailable = 2;
                                    ptr.AvailablePorts.Add(p);

                            }

                                catch
                                {
                                    ptr.PortAvailable = 3;
                                    ptr.UnavailablePorts.Add(p);
                                }
                        }

                        catch (Exception e)
                        {

                        }
                    }

                    else if (ptr.Protocol == "UDP")
                    {
                        try
                        {
                            foreach (int p in ptr.PortNumbers)
                                try
                                {
                                    ptr.PortAvailable = pc.TcpIsListening(server, p);
                                    ptr.PortAvailable = 2;
                                    ptr.AvailablePorts.Add(p);

                                }

                                catch
                                {
                                    ptr.PortAvailable = 3;
                                    ptr.UnavailablePorts.Add(p);
                                }
                        }

                        catch (Exception e)
                        {

                        }
                    }
                }

               catch (Exception e)
                {
                   
                }

                finally
                {
                    
                }
            }
           



        }
    }

}







    public class Port
    {
        public Port()
        {
            Name = string.Empty;
            Protocol = string.Empty;
            PortNumbers = new List<int>();
            
        }

        public string Name { get; set; }
        public string Protocol { get; set; }
        public List<Int32> PortNumbers { get; set; }
        public int PortAvailable { get; set; }


    }



    public class PortTestResult : INotifyPropertyChanged
    {
        public PortTestResult()
        {
            Owner = string.Empty;
            ServerClient = string.Empty;
            Protocol = string.Empty;
            PortRole = string.Empty;
            PortNumbers = new ObservableCollection<int>();
            Description = string.Empty;
            Logo = string.Empty;
            Success = string.Empty;
            Failure = string.Empty;
            PortAvailable = 0;
            AvailablePorts = new ObservableCollection<int>();
            UnavailablePorts = new ObservableCollection<int>();
        }


        public static PortTestResult CreateNewPortResult()
        {

            return new PortTestResult();

        }


    public string Owner
        {
            get
                {
                    return this.owner;
                }
            set
                {
                    if (value != this.owner)
                    {
                        this.owner = value;
                        NotifyPropertyChanged();
                    }
                }
        }

    public string ServerClient {
        get
        {
            return this.serverClient;
        }
        set
        {
            if (value != this.serverClient)
            {
                this.serverClient = value;
                NotifyPropertyChanged();
            }
        }
    }

    public string Protocol {
        get
        {
            return this.protocol;
        }
        set
        {
            if (value != this.protocol)
            {
                this.protocol = value;
                NotifyPropertyChanged();
            }
        }
    }

    public string PortRole {
        get
        {
            return this.portRole;
        }
        set
        {
            if (value != this.portRole)
            {
                this.portRole = value;
                NotifyPropertyChanged();
            }
        }
    }

    public string Description {
        get
        {
            return this.description;
        }
        set
        {
            if (value != this.description)
            {
                this.description = value;
                NotifyPropertyChanged();
            }
        }
    }

    public ObservableCollection<Int32> PortNumbers { get; set; }

    public int PortAvailable
    {
        get
        {
            return this.portAvailable;
        }
        set
        {
            if (value != this.portAvailable)
            {
                this.portAvailable = value;
                NotifyPropertyChanged();
            }
        }
    }

    public string Logo {
        get
        {
            return this.logo;
        }
        set
        {
            if (value != this.logo)
            {
                this.logo = value;
                NotifyPropertyChanged();
            }
        }
    }

    public string Success {
        get
        {
            return this.success;
        }
        set
        {
            if (value != this.success)
            {
                this.success = value;
                NotifyPropertyChanged();
            }
        }
    }

    public string Failure {
        get
        {
            return this.failure;
        }
        set
        {
            if (value != this.failure)
            {
                this.failure = value;
                NotifyPropertyChanged();
            }
        }
    }



    public ObservableCollection<Int32> AvailablePorts { get; set; }

    public ObservableCollection<Int32> UnavailablePorts { get; set; }

    private string owner { get; set; }
        private string serverClient { get; set; }
        private string protocol { get; set; }
        private string portRole { get; set; }
        private string description { get; set; }
        private ObservableCollection<Int32> portNumbers { get; set; }
        private int portAvailable { get; set; }
        private string logo { get; set; }
        private string success { get; set; }
        private string failure { get; set; }
        private ObservableCollection<Int32> availablePorts { get; set; }
        private ObservableCollection<Int32> unavailablePorts { get; set; }


    public event PropertyChangedEventHandler PropertyChanged;

    // This method is called by the Set accessor of each property.
    // The CallerMemberName attribute that is applied to the optional propertyName
    // parameter causes the property name of the caller to be substituted as an argument.
    private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
    {
        if (PropertyChanged != null)
        {
            PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }



}





using System;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Diagnostics;
using System.Windows.Markup;

namespace PlazaConnectivityChecker
{
    /// <summary>
    /// Interaction logic for NetworkActivity.xaml
    /// </summary>
    public partial class Eventviewer : Page
    {

        public Eventviewer()
        {
            InitializeComponent();
           
        }

    }

    [MarkupExtensionReturnType(typeof(EventLogEntry))]

    public class EventLogMarkup : MarkupExtension

    {

        public override object ProvideValue(IServiceProvider serviceProvider)

        {

            List<EventLogEntry> eventList = new List<EventLogEntry>();


            EventLog myEventLog = new EventLog("System", ".");
            

            EventLogEntryCollection myLogEntryCollection = myEventLog.Entries;




            int myCount = myLogEntryCollection.Count;


            for (int i = myCount - 1; i > 0; i--)

            {

                EventLogEntry myLogEntry = myLogEntryCollection[i];
                

            }

            return eventList;

        }

    

    }

}

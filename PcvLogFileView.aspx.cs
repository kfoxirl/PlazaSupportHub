// We need this namespace
using System;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Windows.Documents;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using PlazaConnectivityChecker.Models;

public partial class PcvLogFileView : Page
{
    private string processId; 
    private string processName;
    private string logfileName;
    private string logfilePath = @"D:\syngo.plaza\log\"; 
    private string returnPage;

    private string currentFile; 

    public String CurrentLogFile { get { return currentFile; }}

    protected void Page_Load(object sender, EventArgs e)
    {
      // Get the Process ID, then the associated Process Name. Use that to build the current logfile name
      processId = Request.QueryString["p"];
      processName = Process.GetProcessById(Int32.Parse(processId)).ProcessName;
      logfileName = processName + "_" + processId + ".current";

      returnPage = Request.QueryString["r"]; 

      BindGridview();
    }
    
    // Bind Data to Gridview
    protected void BindGridview()
    {
      currentFile = logfilePath + logfileName;
    
      DataTable InputFileTable = new DataTable();
      DataRow dRow;
      
      string LineTerminator = "<-";
      int ColumnCount = 12;

      using (Stream s = new FileStream(currentFile.ToString(), FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
      {
          // Used to read the content of the selected file
          using (StreamReader sr = new StreamReader(s))
          {
            // Add columns to the DataTable
            InputFileTable.Columns.Add("Date/Time");
            InputFileTable.Columns.Add("Thread");
            InputFileTable.Columns.Add("Type");
            InputFileTable.Columns.Add("Message");

            // Get the Input/Output format for the logfile DateTime as well as the index within the file
            string LogFileDateTimeInputFormat = "d/M/yyyy HH:mm:ss:fff";
            string LogFileDateTimeOutputFormat = "MM/dd/yyyy HH:mm:ss";
            int[] LogFileDateTimeIndex = {7,0};

            // Get the index to the Logfile Message Text
            int LogFileMessageTextIndex = 11;

            // Clear the FileLine object to receive the next line from the StreamReader
            string FileLine = string.Empty;
          
            while (!sr.EndOfStream)
            {
                // If there is a LineTerminator specified, append to the FileLine, otherwise overwrite it with the next line
                if (!LineTerminator.Equals(""))
                {
                  FileLine += sr.ReadLine();
                }
                else
                {
                  FileLine = sr.ReadLine();
                }
                
                // Split the FileLine using the specified delimiter
                string[] FileLineTokens = FileLine.Split(' ');

                if (FileLineTokens[FileLineTokens.Length - 1] == LineTerminator || LineTerminator.Equals(""))
                {
                  string[] parts = FileLine.Split(new char[] { ' ' }, ColumnCount, StringSplitOptions.None);

                  // If the total number of fields equals what we're expecting in this logfile type, proceed
                  if (parts.Length == ColumnCount)
                  {
                    // Piece together the DateTime output value using the index(es) spcified
                    string LogFileDateTime = string.Empty;
                    for (int i = 0; i < LogFileDateTimeIndex.Length; i++)
                    {
                      LogFileDateTime += (parts[LogFileDateTimeIndex[i]]);
                      if (i < LogFileDateTimeIndex.Length - 1) { LogFileDateTime += " "; }
                    }

                    // Is this a valid date that we've assembled?
                    if (LogFileDateTime.IndexOf('/') >= 0)
                    {
                      // Parse the DateTime from the string and re-write it using the standard output format
                      LogFileDateTime = DateTime.ParseExact(LogFileDateTime, LogFileDateTimeInputFormat, null).ToString(LogFileDateTimeOutputFormat);

                      // Initialize the TableRow array and add the DateTime to the first & second elements repectively
                      string[] TableRow = new String[4];
                      TableRow[0] = LogFileDateTime;
                      TableRow[1] = parts[1];
                      TableRow[2] = parts[3]; 
                      TableRow[3] = parts[11];

                      // Add the TableRow array to the DataTable as a new row
                      InputFileTable.Rows.Add(TableRow);
                    }
                  }
                }
              // Clear the FileLine string for the next pass
              FileLine = string.Empty;
            } 
          sr.Close();
        }
      }   
      gvDetails.DataSource = InputFileTable;
      gvDetails.DataBind();
    }

    protected void GridView_RowDataBound(object sender, GridViewRowEventArgs e)
    {
      if(e.Row.RowType == DataControlRowType.DataRow)
      {
        string msgType = e.Row.Cells[2].Text;

		    if (msgType == "Debug") {
          foreach (TableCell cell in e.Row.Cells) {
            cell.Attributes.Add("Style", "background-color: lightblue");
          }
        }
        else if (msgType == "Error") {
          foreach (TableCell cell in e.Row.Cells) {
            cell.Attributes.Add("Style", "background-color: red");
          }
        }
        else if (msgType == "Warning") {
          foreach (TableCell cell in e.Row.Cells) {
            cell.Attributes.Add("Style", "background-color: yellow");
          }
        }
      }
    }
}
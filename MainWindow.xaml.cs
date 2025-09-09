using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using Microsoft.Win32;
using System.Windows.Controls;
using System.Diagnostics;

namespace ZoomAttendeeVerifyApp
{

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private string salesforceFilePath = string.Empty;
        private string zoomFilePath = string.Empty;

        List<Attendee> attendees = new List<Attendee>();

        public MainWindow()
        {
            InitializeComponent();
        }

        private void BrowseFileAndSetPath(TextBox targetTextBox, ref string filePathVariable)
        {
            var dialog = new OpenFileDialog
            {
                Filter = "CSV files (*.csv)|*.csv|All files (*.*)|*.*"
            };

            if (dialog.ShowDialog() == true)
            {
                string fullPath = dialog.FileName;
                string fileNameOnly = Path.GetFileName(fullPath);

                targetTextBox.Text = fileNameOnly;
                filePathVariable = fullPath;
            }
        }

        private void BrowseSalesforce_Click(object sender, RoutedEventArgs e)
        {
            BrowseFileAndSetPath(SalesforceFileTextbox, ref salesforceFilePath);
        }

        private void BrowseZoom_Click(object sender, RoutedEventArgs e)
        {
            BrowseFileAndSetPath(ZoomFileTextbox, ref zoomFilePath);
        }

        private void Compare_Click(object sender, RoutedEventArgs e)
        {
            if (isFileErrors())
                return;

            // Reset list
            attendees.Clear();

            // Load Salesforce attendees
            List<Attendee> salesforceAttendees = LoadSalesforceAttendees();
            AddAttendees(salesforceAttendees);

            // Load Zoom attendees
            List<Attendee> zoomAttendees = LoadZoomAttendees();
            AddAttendees(zoomAttendees);

            // Refresh DataGrid
            AttendeeDataGrid.ItemsSource = null;
            AttendeeDataGrid.ItemsSource = attendees;
        }

        // Check for file errors
        private bool isFileErrors()
        {
            if (string.IsNullOrEmpty(salesforceFilePath) || !File.Exists(salesforceFilePath))
            {
                MessageBox.Show("Please select a valid Salesforce CSV file.");
                return true;
            }

            if (string.IsNullOrEmpty(zoomFilePath) || !File.Exists(zoomFilePath))
            {
                MessageBox.Show("Please select a valid Zoom CSV file.");
                return true;
            }

            if (IsFileLocked(salesforceFilePath))
            {
                MessageBox.Show("Salesforce file is currently open. Please close it and try again.");
                return true;
            }

            if (IsFileLocked(zoomFilePath))
            {
                MessageBox.Show("Zoom file is currently open. Please close it and try again.");
                return true;
            }

            return false;
        }

        // Merge new attendees into the main list
        private void AddAttendees(List<Attendee> newAttendees)
        {
            foreach (var attendee in newAttendees)
            {
                var existing = attendees.Find(a => a.Email.Equals(attendee.Email, StringComparison.OrdinalIgnoreCase));

                if (existing != null)
                {
                    // Update flags if the new attendee has true
                    if (attendee.InSalesforce)
                        existing.InSalesforce = true;
                    if (attendee.InZoom)
                        existing.InZoom = true;
                }
                else
                {
                    // New attendee — just add
                    attendees.Add(attendee);
                }

            }
        }

        // Load attendees from Salesforce CSV
        private List<Attendee> LoadSalesforceAttendees()
        {
            var attendees = new List<Attendee>();

            var lines = File.ReadAllLines(salesforceFilePath);

            for (int i = 1; i < lines.Length; i++)// Skip header
            {
                var parts = lines[i].Split(',');

                if (parts.Length >= 3)
                {
                    attendees.Add(new Attendee(
                        parts[0].Trim('"'),
                        parts[1].Trim('"') + " " + parts[2].Trim('"'),
                        parts[3].Trim('"'),
                        inSalesforce: true
                    ));
                }
            }    
                
            return attendees;
        }

        // Load attendees from Zoom CSV
        private List<Attendee> LoadZoomAttendees()
        {
            var attendees = new List<Attendee>();

            var lines = File.ReadAllLines(zoomFilePath);

            for (int i = 5; i < lines.Length; i++)// Skip header
            {
                var parts = lines[i].Split(',');

                if (parts.Length >= 3)
                {
                    attendees.Add(new Attendee(
                        parts[2].Trim('"'),
                        parts[1].Trim('"'),
                        inZoom: true
                    ));
                }
            }    
                
            return attendees;
            }

        //Summary: Check if a file is locked by another process
        private bool IsFileLocked(string filePath)
        {
            try
            {
                using (var stream = File.Open(filePath, FileMode.Open, FileAccess.Read, FileShare.None))
                {
                    // If we can open it with no sharing, it's not locked
                    return false;
                }
            }
            catch (IOException)
            {
                // IOException means the file is currently in use
                return true;
            }
        }
    }
}



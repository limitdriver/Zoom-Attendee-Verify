using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using Microsoft.Win32;
using System.Windows.Controls;

namespace ZoomAttendeeVerifyApp;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
    }

    // Event handler for "Load CSV" button click
    // private void OnLoadCsvClicked(object sender, RoutedEventArgs e)
    // {
    //     string filePath = FilePathTextBox.Text;
    //     if (File.Exists(filePath))
    //     {
    //         // Load and display CSV data in ListBox (simplified)
    //         var csvData = LoadCsv(filePath);
    //         AttendeeListBox.ItemsSource = csvData;
    //     }
    //     else
    //     {
    //         MessageBox.Show("File not found. Please enter a valid file path.");
    //     }
    // }

    // // Event handler for "Verify Attendees" button click
    // private void OnVerifyClicked(object sender, RoutedEventArgs e)
    // {
    //     // Logic to compare attendees (simplified)
    //     MessageBox.Show("Verification logic will go here.");
    // }

    // // Method to load CSV data from a file
    // private List<string> LoadCsv(string filePath)
    // {
    //     var lines = new List<string>();
    //     foreach (var line in File.ReadLines(filePath))
    //     {
    //         lines.Add(line);
    //     }
    //     return lines;
    // }

    private void BrowseFileAndSetPath(TextBox targetTextBox)
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

            // Optionally store the full path somewhere else if you need it later
            targetTextBox.Tag = fullPath;
        }
    }


    private void BrowseSalesforce_Click(object sender, RoutedEventArgs e)
    {
        BrowseFileAndSetPath(SalesforceFilePath);
    }

    private void BrowseZoom_Click(object sender, RoutedEventArgs e)
    {
        BrowseFileAndSetPath(ZoomFilePath);
    }

}

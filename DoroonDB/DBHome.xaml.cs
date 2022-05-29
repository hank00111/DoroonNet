using CsvHelper;
using DoroonNet.Command;
using MahApps.Metro.Controls;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Dynamic;
using System.Globalization;
using System.IO;
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

namespace DoroonNet.DoroonDB
{
    /// <summary>
    /// DBHome.xaml 的互動邏輯
    /// </summary>
    public partial class DBHome : MetroWindow
    {
        DoroonSQLLiteDB DoroonDB;
        List<string> Tbl = new List<string>();
        List<string> Tbi = new List<string>();
        int ExportSel;
        string SysDir = MainWindow.Func_exe_path();

        public DBHome()
        {
            InitializeComponent();
            DoroonDB = new DoroonSQLLiteDB();
            ExportCSV.IsEnabled = false;
        }

        private void ComboTableItems_Loaded(object sender, RoutedEventArgs e)
        {
            DataTable dt = new DataTable(); 
            foreach (var Tables in DoroonDB.GetAllTables())
            {
                Tbl.Add(Tables.TblName);
            }

            foreach (var ID in DoroonDB.GetFlightList())
            {
                Tbi.Add(ID.FlightID.ToString());
            }
            //Tbi.Cast<object>().ToArray();

            //DataView.Items.Add(itemsSourceList);           

            ComboTableItems.FontSize = 15;
            FlightIDCombo.FontSize = 15;
            ComboTableItems.ItemsSource = Tbl;
            FlightIDCombo.ItemsSource = Tbi;
        }

        private void ComboTableItems_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {        
           
            switch (ComboTableItems.SelectedItem.ToString())
            {
                case "Flights":
                    Console.WriteLine(ComboTableItems.SelectedItem);
                    DataView.Columns.Clear();
                    DataView.Items.Clear();
                    foreach (TableInfoData tabl in DoroonDB.GetTableInfo("Flights"))
                    {
                        DataGridTextColumn column = new DataGridTextColumn
                        {
                            Header = tabl.Name,
                            Binding = new Binding(tabl.Name)
                        };
                        DataView.Columns.Add(column);
                        //DataView.SelectionMode = DataGridSelectionMode.Single;
                    }

                    foreach (var data in DoroonDB.GetFlightList())
                    {
                        //var json = JsonConvert.SerializeObject(everyOrderDate);
                        //Console.WriteLine(json);
                        DataView.Items.Add(new
                        {
                            data.FlightID,
                            data.linkid,
                            start_datetimestamp = new DateTime(data.start_datetimestamp).ToString()
                        });            
                    }
                    FlightIDCombo.IsEnabled = false;
                    ExportCSV.IsEnabled = false;
                    break;

                case "FlightDatas":
                    Console.WriteLine(ComboTableItems.SelectedItem);
                    DataView.Columns.Clear();
                    DataView.Items.Clear();
                    foreach (TableInfoData tabl in DoroonDB.GetTableInfo("FlightDatas"))
                    {
                        DataGridTextColumn column = new DataGridTextColumn
                        {
                            Header = tabl.Name,
                            Binding = new Binding(tabl.Name)
                        };
                        DataView.Columns.Add(column);
                    }

                    FlightIDCombo.IsEnabled = true;

                    break;

                default:
                    Console.WriteLine(ComboTableItems.SelectedItem);
                    DataView.Columns.Clear();
                    DataView.Items.Clear();
                    break;

            }

        }

        private void FlightIDCombo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            DataView.Items.Clear();
            foreach (var data in DoroonDB.GetFlightData(int.Parse(FlightIDCombo.SelectedItem.ToString())))
            {
                DataView.Items.Add(new
                {
                    data.FlightDatasID,
                    data.FlightID,
                    data.latitude,
                    data.longitude,
                    data.altitude,
                    data.groundspeed,
                    data.heading,
                    data.X,
                    data.Y,
                    data.Z,
                    data.X1,
                    data.Y1,
                    data.Z1,
                    data.X2,
                    data.Y2,
                    data.Z2,
                    datetimestamp = new DateTime(data.datetimestamp).ToString()
                });
            }
            ExportSel = int.Parse(FlightIDCombo.SelectedItem.ToString());
            ExportCSV.IsEnabled = true;
        }

        private void DataView_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            dynamic dataRow = DataView.SelectedItem;
            int cellValue = dataRow.FlightID;

            //Console.WriteLine(cellValue);
        }

        private void Export_Click(object sender, RoutedEventArgs e)
        {        
            try
            {
                List<FlightPathDataCSV> records = DoroonDB.GetFlightDataCSV(ExportSel);
                if (Directory.Exists(SysDir + @"\CSV") == false)//不存在就建立
                {
                    Directory.CreateDirectory(SysDir + @"\CSV");
                }
                using (var writer = new StreamWriter(SysDir + @"CSV\" + $"{DateTime.Now.ToString("yyyy-MM-dd ffffff")}_Sel_ {ExportSel}.csv", false, Encoding.UTF8))
                using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
                {
                    foreach (var record in records)
                    {
                        var a = new List<FlightPathDataCSV>
                    {
                        new FlightPathDataCSV
                        {
                            FlightDatasID = record.FlightDatasID, FlightID = record.FlightID,
                            latitude = record.latitude,  longitude = record.longitude, altitude = record.altitude,
                            groundspeed = record.groundspeed, heading = record.heading, datetimestamp = record.datetimestamp,
                            X = record.X, Y = record.Y, Z = record.Z, X1 = record.X1, Y1 = record.Y1, Z1 = record.Z1, 
                            X2 = record.X2, Y2 = record.Y2, Z2 = record.Z2

                        }
                    };
                        //csv.NextRecord();
                        csv.WriteRecords(a);
                    }
                    //csv.WriteRecords(records);
                }
                if (ShowSnackbar.MessageQueue is { } messageQueue)
                {
                    var message = "Saved Successfully";
                    Task.Factory.StartNew(() => messageQueue.Enqueue(message));
                }
          
            }
            catch(Exception ex)
            {
                if (ShowSnackbar.MessageQueue is { } messageQueue)
                {
                    var message = "Error "+ex;
                    Task.Factory.StartNew(() => messageQueue.Enqueue(message));
                }
            }

        }
    }

}

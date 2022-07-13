﻿#pragma checksum "..\..\..\..\..\DoroonDB\DBHome.xaml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "9BD19FF4AFC0E3B88CF313F456426365DDA6F149"
//------------------------------------------------------------------------------
// <auto-generated>
//     這段程式碼是由工具產生的。
//     執行階段版本:4.0.30319.42000
//
//     對這個檔案所做的變更可能會造成錯誤的行為，而且如果重新產生程式碼，
//     變更將會遺失。
// </auto-generated>
//------------------------------------------------------------------------------

using DoroonNet.DoroonDB;
using MahApps.Metro.Controls;
using MaterialDesignThemes.Wpf;
using MaterialDesignThemes.Wpf.Converters;
using MaterialDesignThemes.Wpf.Transitions;
using ScottPlot;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Controls.Ribbon;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms.Integration;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Media.TextFormatting;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Shell;


namespace DoroonNet.DoroonDB {
    
    
    /// <summary>
    /// DBHome
    /// </summary>
    public partial class DBHome : MahApps.Metro.Controls.MetroWindow, System.Windows.Markup.IComponentConnector {
        
        
        #line 21 "..\..\..\..\..\DoroonDB\DBHome.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ComboBox ComboTableItems;
        
        #line default
        #line hidden
        
        
        #line 26 "..\..\..\..\..\DoroonDB\DBHome.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ComboBox FlightIDCombo;
        
        #line default
        #line hidden
        
        
        #line 28 "..\..\..\..\..\DoroonDB\DBHome.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button ExportCSV;
        
        #line default
        #line hidden
        
        
        #line 32 "..\..\..\..\..\DoroonDB\DBHome.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.DataGrid DataView;
        
        #line default
        #line hidden
        
        
        #line 43 "..\..\..\..\..\DoroonDB\DBHome.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal MaterialDesignThemes.Wpf.Snackbar ShowSnackbar;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "5.0.9.0")]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/DoroonNet;component/doroondb/dbhome.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\..\..\DoroonDB\DBHome.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "5.0.9.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            this.ComboTableItems = ((System.Windows.Controls.ComboBox)(target));
            
            #line 22 "..\..\..\..\..\DoroonDB\DBHome.xaml"
            this.ComboTableItems.Loaded += new System.Windows.RoutedEventHandler(this.ComboTableItems_Loaded);
            
            #line default
            #line hidden
            
            #line 22 "..\..\..\..\..\DoroonDB\DBHome.xaml"
            this.ComboTableItems.SelectionChanged += new System.Windows.Controls.SelectionChangedEventHandler(this.ComboTableItems_SelectionChanged);
            
            #line default
            #line hidden
            return;
            case 2:
            this.FlightIDCombo = ((System.Windows.Controls.ComboBox)(target));
            
            #line 26 "..\..\..\..\..\DoroonDB\DBHome.xaml"
            this.FlightIDCombo.SelectionChanged += new System.Windows.Controls.SelectionChangedEventHandler(this.FlightIDCombo_SelectionChanged);
            
            #line default
            #line hidden
            return;
            case 3:
            this.ExportCSV = ((System.Windows.Controls.Button)(target));
            
            #line 28 "..\..\..\..\..\DoroonDB\DBHome.xaml"
            this.ExportCSV.Click += new System.Windows.RoutedEventHandler(this.Export_Click);
            
            #line default
            #line hidden
            return;
            case 4:
            this.DataView = ((System.Windows.Controls.DataGrid)(target));
            
            #line 32 "..\..\..\..\..\DoroonDB\DBHome.xaml"
            this.DataView.SelectedCellsChanged += new System.Windows.Controls.SelectedCellsChangedEventHandler(this.DataView_SelectedCellsChanged);
            
            #line default
            #line hidden
            return;
            case 5:
            this.ShowSnackbar = ((MaterialDesignThemes.Wpf.Snackbar)(target));
            return;
            }
            this._contentLoaded = true;
        }
    }
}

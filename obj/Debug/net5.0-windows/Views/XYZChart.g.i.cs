﻿#pragma checksum "..\..\..\..\Views\XYZChart.xaml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "79A178903A59F396ED57BFB7D48EE5FD5F0B3BF8"
//------------------------------------------------------------------------------
// <auto-generated>
//     這段程式碼是由工具產生的。
//     執行階段版本:4.0.30319.42000
//
//     對這個檔案所做的變更可能會造成錯誤的行為，而且如果重新產生程式碼，
//     變更將會遺失。
// </auto-generated>
//------------------------------------------------------------------------------

using DoroonNet.Views;
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


namespace DoroonNet.Views {
    
    
    /// <summary>
    /// XYZChart
    /// </summary>
    public partial class XYZChart : System.Windows.Controls.UserControl, System.Windows.Markup.IComponentConnector {
        
        
        #line 10 "..\..\..\..\Views\XYZChart.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Expander Expan;
        
        #line default
        #line hidden
        
        
        #line 13 "..\..\..\..\Views\XYZChart.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ListBox SelectList;
        
        #line default
        #line hidden
        
        
        #line 32 "..\..\..\..\Views\XYZChart.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal ScottPlot.WpfPlot A_Plot;
        
        #line default
        #line hidden
        
        
        #line 33 "..\..\..\..\Views\XYZChart.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal ScottPlot.WpfPlot B_Plot;
        
        #line default
        #line hidden
        
        
        #line 34 "..\..\..\..\Views\XYZChart.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal ScottPlot.WpfPlot C_Plot;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "6.0.8.0")]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/DoroonNet;component/views/xyzchart.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\..\Views\XYZChart.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "6.0.8.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            
            #line 7 "..\..\..\..\Views\XYZChart.xaml"
            ((DoroonNet.Views.XYZChart)(target)).Loaded += new System.Windows.RoutedEventHandler(this.UserControl_Loaded);
            
            #line default
            #line hidden
            return;
            case 2:
            this.Expan = ((System.Windows.Controls.Expander)(target));
            
            #line 10 "..\..\..\..\Views\XYZChart.xaml"
            this.Expan.Expanded += new System.Windows.RoutedEventHandler(this.Expander_Expanded);
            
            #line default
            #line hidden
            
            #line 10 "..\..\..\..\Views\XYZChart.xaml"
            this.Expan.Collapsed += new System.Windows.RoutedEventHandler(this.Expander_Collapsed);
            
            #line default
            #line hidden
            return;
            case 3:
            this.SelectList = ((System.Windows.Controls.ListBox)(target));
            
            #line 13 "..\..\..\..\Views\XYZChart.xaml"
            this.SelectList.SelectionChanged += new System.Windows.Controls.SelectionChangedEventHandler(this.SelectList_SelectionChanged);
            
            #line default
            #line hidden
            return;
            case 4:
            this.A_Plot = ((ScottPlot.WpfPlot)(target));
            
            #line 32 "..\..\..\..\Views\XYZChart.xaml"
            this.A_Plot.MouseWheel += new System.Windows.Input.MouseWheelEventHandler(this.A_Plot_MouseWheel);
            
            #line default
            #line hidden
            
            #line 32 "..\..\..\..\Views\XYZChart.xaml"
            this.A_Plot.Loaded += new System.Windows.RoutedEventHandler(this.A_Plot_Loaded);
            
            #line default
            #line hidden
            return;
            case 5:
            this.B_Plot = ((ScottPlot.WpfPlot)(target));
            return;
            case 6:
            this.C_Plot = ((ScottPlot.WpfPlot)(target));
            return;
            }
            this._contentLoaded = true;
        }
    }
}


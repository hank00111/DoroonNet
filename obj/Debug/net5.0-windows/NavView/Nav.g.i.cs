﻿#pragma checksum "..\..\..\..\NavView\Nav.xaml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "B455B6E5F5B8BC183FF1414CC8DD0C769C632BD8"
//------------------------------------------------------------------------------
// <auto-generated>
//     這段程式碼是由工具產生的。
//     執行階段版本:4.0.30319.42000
//
//     對這個檔案所做的變更可能會造成錯誤的行為，而且如果重新產生程式碼，
//     變更將會遺失。
// </auto-generated>
//------------------------------------------------------------------------------

using DoroonNet.NavView;
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


namespace DoroonNet.NavView {
    
    
    /// <summary>
    /// Nav
    /// </summary>
    public partial class Nav : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 14 "..\..\..\..\NavView\Nav.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.DataGrid NavDataGrid;
        
        #line default
        #line hidden
        
        
        #line 26 "..\..\..\..\NavView\Nav.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button ClearNavPtBt;
        
        #line default
        #line hidden
        
        
        #line 28 "..\..\..\..\NavView\Nav.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox AltTxTBox;
        
        #line default
        #line hidden
        
        
        #line 30 "..\..\..\..\NavView\Nav.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Primitives.ToggleButton GoHomeBt;
        
        #line default
        #line hidden
        
        
        #line 31 "..\..\..\..\NavView\Nav.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Primitives.ToggleButton NoGoHomeBt;
        
        #line default
        #line hidden
        
        
        #line 32 "..\..\..\..\NavView\Nav.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button SendControlBt;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "5.0.14.0")]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/DoroonNet;component/navview/nav.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\..\NavView\Nav.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "5.0.14.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            
            #line 8 "..\..\..\..\NavView\Nav.xaml"
            ((DoroonNet.NavView.Nav)(target)).PreviewLostKeyboardFocus += new System.Windows.Input.KeyboardFocusChangedEventHandler(this.Window_PreviewLostKeyboardFocus);
            
            #line default
            #line hidden
            
            #line 8 "..\..\..\..\NavView\Nav.xaml"
            ((DoroonNet.NavView.Nav)(target)).Closing += new System.ComponentModel.CancelEventHandler(this.Window_Closing);
            
            #line default
            #line hidden
            
            #line 8 "..\..\..\..\NavView\Nav.xaml"
            ((DoroonNet.NavView.Nav)(target)).Loaded += new System.Windows.RoutedEventHandler(this.Window_Loaded);
            
            #line default
            #line hidden
            return;
            case 2:
            this.NavDataGrid = ((System.Windows.Controls.DataGrid)(target));
            return;
            case 3:
            this.ClearNavPtBt = ((System.Windows.Controls.Button)(target));
            
            #line 26 "..\..\..\..\NavView\Nav.xaml"
            this.ClearNavPtBt.Click += new System.Windows.RoutedEventHandler(this.ClearNavPtBt_Click);
            
            #line default
            #line hidden
            return;
            case 4:
            this.AltTxTBox = ((System.Windows.Controls.TextBox)(target));
            
            #line 28 "..\..\..\..\NavView\Nav.xaml"
            this.AltTxTBox.TextChanged += new System.Windows.Controls.TextChangedEventHandler(this.AltTxTBox_TextChanged);
            
            #line default
            #line hidden
            return;
            case 5:
            this.GoHomeBt = ((System.Windows.Controls.Primitives.ToggleButton)(target));
            
            #line 30 "..\..\..\..\NavView\Nav.xaml"
            this.GoHomeBt.Checked += new System.Windows.RoutedEventHandler(this.GoHomeBt_Checked);
            
            #line default
            #line hidden
            
            #line 30 "..\..\..\..\NavView\Nav.xaml"
            this.GoHomeBt.Unchecked += new System.Windows.RoutedEventHandler(this.GoHomeBt_Unchecked);
            
            #line default
            #line hidden
            return;
            case 6:
            this.NoGoHomeBt = ((System.Windows.Controls.Primitives.ToggleButton)(target));
            
            #line 31 "..\..\..\..\NavView\Nav.xaml"
            this.NoGoHomeBt.Checked += new System.Windows.RoutedEventHandler(this.NoGoHomeBt_Checked);
            
            #line default
            #line hidden
            
            #line 31 "..\..\..\..\NavView\Nav.xaml"
            this.NoGoHomeBt.Unchecked += new System.Windows.RoutedEventHandler(this.NoGoHomeBt_Unchecked);
            
            #line default
            #line hidden
            return;
            case 7:
            this.SendControlBt = ((System.Windows.Controls.Button)(target));
            
            #line 32 "..\..\..\..\NavView\Nav.xaml"
            this.SendControlBt.Click += new System.Windows.RoutedEventHandler(this.SendControlBt_Click);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}


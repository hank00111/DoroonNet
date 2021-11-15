using System;
using System.Windows.Threading;
using MahApps.Metro.Controls;
using Microsoft.Xaml.Behaviors;

namespace DoroonNet.Command
{
    class DateTimeNow: Behavior<DateTimePicker>
    {
        #pragma warning disable CS8632 // 可為 Null 的參考型別註釋應只用於 '#nullable' 註釋內容中的程式碼。
        private DispatcherTimer? _dispatcherTimer;
        #pragma warning restore CS8632 // 可為 Null 的參考型別註釋應只用於 '#nullable' 註釋內容中的程式碼。

        protected override void OnAttached()
        {
            base.OnAttached();
            this._dispatcherTimer = new DispatcherTimer(TimeSpan.FromSeconds(1),
                                                        DispatcherPriority.DataBind,
                                                        (sender, args) => this.AssociatedObject.SelectedDateTime = 
                                                        DateTime.Now,
                                                        Dispatcher.CurrentDispatcher);
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();
            this._dispatcherTimer?.Stop();
        }
    }
}

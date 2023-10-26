using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using TestApp.Models.Abstract;
using TestApp.Models.Enums;
using TestApp.Services.Messenger;
using TestApp.ViewModels;

namespace TestApp.Views.UserControls
{
    /// <summary>
    /// Interaction logic for ParcelView.xaml
    /// </summary>
    public partial class ParcelView : UserControl, IOpenControlAPI
    {
        public ParcelView()
        {
            InitializeComponent();
        }

        public void RaiseEvent(EVENT_TYPE eventType, string elementName)
        {
            switch (eventType)
            {
                case EVENT_TYPE.Focus:
                    UIElement? element = FindName(elementName) as UIElement;
                    if (element != null && element.Focusable)
                        InvokeFromMainThread(() => element.Focus());
                    break;
                default:
                    break;
            }
        }

        private void InvokeFromMainThread(Action action) => Dispatcher.BeginInvoke(DispatcherPriority.Input, action);

        private void RaiseOnDataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (sender is UserControl control)
            {
                if (control.DataContext != null && control.DataContext is ParcelViewModel viewModel)
                {
                    MessengerEntity entity = new(viewModel, this);
                    Messenger.Register(entity);
                }
            }
        }
    }
}

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
    /// Interaction logic for BuildingView.xaml
    /// </summary>
    public partial class BuildingView : UserControl, IOpenControlAPI
    {
        public BuildingView()
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

        // Предполагается, что мы будем дёргать событие из внешнего потока, по этому, делаем функционал для проброса экшена через главный поток.
        // В противном случае, наш фокус просто не отработает.
        private void InvokeFromMainThread(Action action) => Dispatcher.BeginInvoke(DispatcherPriority.Input, action);

        private void RaiseOnDataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            // Почему я говорил про преимущество моста, потому-что регистрация Вьюхи будет происходить именно с тем контекстом, который выберет она. 
            // Разумеется, что выбрало Вью и как оно работает, вызывающие модели знать не должны.
            /// Есть интерфейс <see cref="IOpenControlAPI"/>. Это все, что мы даем на откуп внешним системам.
            if (sender is UserControl control)
            {
                if (control.DataContext != null && control.DataContext is BuildingViewModel viewModel)
                {
                    MessengerEntity entity = new(viewModel, this);
                    Messenger.Register(entity);
                }
            }
        }
    }
}

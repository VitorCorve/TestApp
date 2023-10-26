using TestApp.Models.Abstract;
using TestApp.ViewModels.Abstract;

namespace TestApp.Services.Messenger
{
    /// <summary>
    /// Связка <see cref="ViewModelBase"/> + <see cref="IOpenControlAPI"/> контрол с открытым API для внешнего вызова событий.
    /// </summary>
    public class MessengerEntity
    {
        public ViewModelBase ViewModel { get; }
        public IOpenControlAPI Control { get; }
        public MessengerEntity(ViewModelBase viewModel, IOpenControlAPI control)
        {
            ViewModel = viewModel;
            Control = control;
        }
    }
}

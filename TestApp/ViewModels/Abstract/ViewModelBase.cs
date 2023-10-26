using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace TestApp.ViewModels.Abstract
{
    /// <summary>
    /// Базовая реализация <see cref="INotifyPropertyChanged"/> и сеттеров для Вью Модели.
    /// </summary>
    public abstract class ViewModelBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        protected bool Set<T>(ref T param, T value, string? property = null)
        {
            if (Equals(param, value))
                return false;
            else
            {
                param = value;
                OnPropertyChanged(property);
                return true;
            }
        }
    }
}

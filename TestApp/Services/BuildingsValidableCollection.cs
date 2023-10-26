using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using TestApp.Models;
using TestApp.Models.Abstract;

namespace TestApp.Services
{
    /// <summary>
    /// Мастер коллекция для валидации типов <see cref="BuildingBase"/>.
    /// </summary>
    public sealed class BuildingsValidableCollection : IEnumerable<ValidationResult>, INotifyPropertyChanged, INotifyCollectionChanged
    {
        private readonly List<ValidationResult> _validationResults = new();

        public event NotifyCollectionChangedEventHandler? CollectionChanged;
        public event PropertyChangedEventHandler? PropertyChanged;

        public IEnumerator<ValidationResult> GetEnumerator()
        {
            foreach (ValidationResult result in _validationResults)
                yield return result;
        }

        public void NotifyPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void Validate(BuildingBase entry)
        {
            /// Можно было использовать дефолтный <see cref="ObservableCollection"/>, но я решил сделать внутренний контроль внутри коллекци с возможностью валидации и обновления.
            /// Вообще, этот класс можно было сделать наследником абстрактного класса, а за место <see cref="ValidationResult"/> использовать дженерики.
            /// Но, опять же, не вижу смысла перегружать лишней абстракцией тот блок, которому это не нужно. Достаточно показать, что я эту возможность вижу и как её реализовать, знаю.
            entry.ApplyValidation(_validationResults);
            RaiseOnCollectionChanged();
        }

        private void RaiseOnCollectionChanged()
        {
            CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}

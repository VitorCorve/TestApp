using System.Collections.Generic;
using System.Linq;
using TestApp.Models.Abstract;
using TestApp.Models.Enums;
using TestApp.ViewModels.Abstract;

namespace TestApp.Services.Messenger
{
    /// <summary>
    /// Месседжер обмена сообщениями между <see cref="ViewModelBase"/> / вызова операций.
    /// </summary>
    
    /*
     Вообще, есть библиотека MvvmLight со своим собственным месседжером с той же концепцией: мост между вью моделями для обмена данными, вызовом комманд.
     Не вижу смысла тянуть целую либу для данной конкретной задачи. Если бы проект был продовским, вместо подключения сторонних либ, я бы перенёс сюда свой код месседжера,
     который я писал для проекта более чем с двумя сотнями активных вью моделей и вьюх. Он отлично себя показал и на других проектах. По сути, это такой же класс, как этот месседжер,
     но с более глубокой архитектурой, которая не загнется от реальной боевой нагрузки за счет контроля инстансов и продуманной архитектуры самой абстракции ViewModel.
     */
    public static class Messenger
    {
        private static readonly List<MessengerEntity> _entites = new();

        /// <summary>
        /// Регистрация пары Вью Модель + Вью.
        /// </summary>
        /// <param name="messengerEntity"></param>
        public static void Register(MessengerEntity messengerEntity)
        {
            // На крупный проект я бы добавил контроль по ID и замену существующих пар на новые.
            // К примеру, когда вью модели грузятся заново при открытии контрола, создается новый инстанс вью модели.
            // Может быть ситуация, когда внешняя вью модель отправит команду на инстанс, который уже выгружен.
            // В кейсе данного приложения, такой проблемы не будет, поскольку инстансы вью моделей создаются один раз.
            if (!_entites.Contains(messengerEntity))
                _entites.Add(messengerEntity);
        }

        /// <summary>
        /// Поиск зарегистрированной <see cref="ViewModelBase"/> по типу с вызовом события для элемента <paramref name="elementName"/>.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="eventType"></param>
        /// <param name="elementName"></param>
        public static void RaiseEvent<T>(EVENT_TYPE eventType, string elementName) where T : ViewModelBase
        {
            IOpenControlAPI? target = _entites
                .Where(x => x.ViewModel.GetType() == typeof(T))
                .FirstOrDefault()?.Control;

            target?.RaiseEvent(eventType, elementName);
        }
    }
}

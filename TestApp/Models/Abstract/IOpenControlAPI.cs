using TestApp.Models.Enums;

namespace TestApp.Models.Abstract
{
    /// <summary>
    /// Контракт открытых вызовов для контролов.
    /// </summary>
    public interface IOpenControlAPI
    {
        /// <summary>
        /// Вызов события <see cref="EVENT_TYPE"/> элемента <paramref name="elementName"/>.
        /// </summary>
        /// <param name="eventType"></param>
        /// <param name="elementName"></param>
        public void RaiseEvent(EVENT_TYPE eventType, string elementName);
    }
}

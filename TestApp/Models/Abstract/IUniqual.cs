using System;

namespace TestApp.Models.Abstract
{
    /// <summary>
    /// Интерфейс-обозначение уникального объекта с идентификатором по <see cref="Guid"/>.
    /// </summary>
    public interface IUniqual
    {
        public Guid ID { get; }
    }
}

using System;
using System.Collections.Generic;
using TestApp.Models.Enums;

namespace TestApp.Models.Abstract
{
    public abstract class BuildingBase : IUniqual
    {
        public TYPE Type { get; set; }
        public Guid ID { get; protected set; }

        /// <summary>
        /// Валидация модели с пробрасыванием данных в коллекцию.
        /// </summary>
        /// <param name="targetCollection"></param>
        public abstract void ApplyValidation(List<ValidationResult> targetCollection);

        public BuildingBase()
        {
            ID = Guid.NewGuid();
        }
    }
}

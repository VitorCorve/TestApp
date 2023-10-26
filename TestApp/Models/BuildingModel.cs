using System;
using System.Collections.Generic;
using System.Linq;
using TestApp.Models.Abstract;
using TestApp.Models.Enums;
using TestApp.Services;

namespace TestApp.Models
{
    public class BuildingModel : BuildingBase
    {
        public BuildingModel()
        {
            Type = TYPE.Building;
        }

        public int? FloorCount { get; set; }
        public string Address { get; set; }
        public bool IsLiving { get; set; }

        public override void ApplyValidation(List<ValidationResult> targetCollection)
        {
            // По условию ТЗ, валидация должна быть частью абстрактного класса, от которого наследуются модели предметной области.
            // Можно было вернуть новую коллекцию или использовать статичную коллекцию для ошибок, но, первый вариант не подходит из соображений оптимизации:
            // все эти коллекции будут висеть в памяти. Вариант со статиком приведет к путанице. В общем, я решил использовать 1 коллекцию для всех ошибок и прокинул зависимость.
            /// Была идея сделать жесткую привязку к мастер-коллекции <see cref="BuildingsValidableCollection"/>, но я решил отказаться от этого, т.к. по хорошему, <see cref="BuildingsValidableCollection"/>
            // должен быть реализацией абстракциии и иметь базовый абстрактный класс/интерфейс. Считаю в рамках этой задачи нецелообразным создавать широкий абстрактный слой архитектуры.

            var entriesForRemoval = targetCollection
                .Where(c => c.EntryID.Equals(this.ID))
                .ToList();

            targetCollection.RemoveAll(entriesForRemoval.Contains);

            if (FloorCount == null || (FloorCount != null && FloorCount == 0))
            {
                var result = new ValidationResult
                {
                    Description = "Задайте количество этажей здания",
                    EntryID = this.ID,
                    EntryName = nameof(FloorCount),
                    Status = VALIDATION_STATUS.Error
                };
                targetCollection.Add(result);
            }

            else if (FloorCount != null && FloorCount < 0)
            {
                var result = new ValidationResult
                {
                    Description = "Количество этажей не может быть отрицательным",
                    EntryID = this.ID,
                    EntryName = nameof(FloorCount),
                    Status = VALIDATION_STATUS.Error
                };
                targetCollection.Add(result);
            }

            if (string.IsNullOrWhiteSpace(Address))
            {
                var result = new ValidationResult
                {
                    Description = "Задайте адрес здания",
                    EntryID = this.ID,
                    EntryName = nameof(Address),
                    Status = VALIDATION_STATUS.Error
                };
                targetCollection.Add(result);
            }
        }
    }
}

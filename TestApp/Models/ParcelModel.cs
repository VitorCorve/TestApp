using System;
using System.Collections.Generic;
using System.Linq;
using TestApp.Models.Abstract;
using TestApp.Models.Enums;

namespace TestApp.Models
{
    public class ParcelModel : BuildingBase
    {
        public ParcelModel()
        {
            Type = TYPE.Parcel;
        }

        public string Number { get; set; }
        public string Location { get; set; }
        public override void ApplyValidation(List<ValidationResult> targetCollection)
        {
            /// Комментарий по аналогии с <see cref="BuildingModel.ApplyValidation(List{ValidationResult})"/>.
            
            var entriesForRemoval = targetCollection
                .Where(c => c.EntryID.Equals(this.ID))
                .ToList();

            targetCollection.RemoveAll(entriesForRemoval.Contains);

            if (string.IsNullOrWhiteSpace(Number))
            {
                var result = new ValidationResult 
                { 
                    Description = "Задайте номер земельного участка", 
                    EntryID = this.ID, 
                    EntryName = nameof(Number),
                    Status = VALIDATION_STATUS.Error
                };
                targetCollection.Add(result);
            }

            if (string.IsNullOrWhiteSpace(Location))
            {
                var result = new ValidationResult
                {
                    Description = "Задайте местоположение земельного участка",
                    EntryID = this.ID,
                    EntryName = nameof(Location),
                    Status = VALIDATION_STATUS.Error
                };
                targetCollection.Add(result);
            }
        }
    }
}

using System;
using TestApp.Models.Enums;
using TestApp.Models.Abstract;

namespace TestApp.Models
{
    /// <summary>
    /// Результат валидации вызовов <see cref="BuildingBase.ApplyValidation(System.Collections.Generic.List{ValidationResult})"/>.
    /// </summary>
    public class ValidationResult
    {
        public Guid EntryID { get; set; }
        public string? Description { get; set; }
        public VALIDATION_STATUS Status { get; set; }
        public string? EntryName { get; set; }
    }
}

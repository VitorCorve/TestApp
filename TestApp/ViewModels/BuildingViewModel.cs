using TestApp.Infrastructure;
using TestApp.Models;
using TestApp.Services;
using TestApp.ViewModels.Abstract;

namespace TestApp.ViewModels
{
    public class BuildingViewModel : ViewModelBase
    {
        public BuildingViewModel(BuildingsValidableCollection validationMaster)
        {
            _validationMaster = validationMaster;
        }

        private readonly BuildingsValidableCollection _validationMaster;

        private BuildingModel _selectedBuilding;

        private Command _validateEntry;

        private string _floorCount;

        // Количество этажей у UI привязано к FloorCount. Собственно, при смене селекта, новое значение нужно пробросить именно в него.
        // FloorCount решил перенести в отдельное свойство, т.к. он более чувствителен к ошибкам и требует дополнительной обработки.
        // Address и IsLiving связываются напрямую к свойствам SelectedBuilding.
        public BuildingModel SelectedBuilding
        {
            get => _selectedBuilding;
            set
            {
                Set(ref _selectedBuilding, value);
                FloorCount = value?.FloorCount?.ToString() ?? string.Empty;
            }
        }

        public string FloorCount
        {
            get => _floorCount;
            set => Set(ref _floorCount, value);
        }

        // Триггер комманды активируется при изменении ячеек UI, привязанных к данным модели.
        public Command ValidateEntry => _validateEntry ??= new Command(obj =>
        {
            UpdateSelectedBuilding();
            _validationMaster.Validate(SelectedBuilding);
        }, canExecute => SelectedBuilding != null);

        private void UpdateSelectedBuilding()
        {
            // Пытаемся пробросить в SelectedBuilding.FloorCount то, что ввёл юзер.
            if (_selectedBuilding != null)
            {
                bool succesfull = int.TryParse(FloorCount, out int value);
                _selectedBuilding.FloorCount = succesfull ? value : null;
            }
        }
    }
}

using TestApp.Infrastructure;
using TestApp.Models;
using TestApp.Services;
using TestApp.ViewModels.Abstract;
namespace TestApp.ViewModels
{
    public class ParcelViewModel : ViewModelBase
    {
        public ParcelViewModel(BuildingsValidableCollection validationMaster)
        {
            _validationMaster = validationMaster;
        }

        private readonly BuildingsValidableCollection _validationMaster;

        private ParcelModel _selectedParcel;

        private Command _validateEntry;

        public ParcelModel SelectedParcel
        {
            get => _selectedParcel;
            set => Set(ref _selectedParcel, value);
        }

        public Command ValidateEntry => _validateEntry ??= new Command(obj =>
        {
            _validationMaster.Validate(SelectedParcel);
        }, canExecute => SelectedParcel != null);
    }
}

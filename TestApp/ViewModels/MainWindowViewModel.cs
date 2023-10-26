using System.Collections.ObjectModel;
using System.Linq;
using TestApp.Infrastructure;
using TestApp.Models;
using TestApp.Models.Abstract;
using TestApp.Models.Enums;
using TestApp.Services;
using TestApp.Services.Messenger;
using TestApp.ViewModels.Abstract;

namespace TestApp.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        public MainWindowViewModel() => Initialize();

        private BuildingBase _selectedBuilding;
        private ValidationResult _selectedValidation;

        private ObservableCollection<BuildingBase> _buildings = new();
        private BuildingsValidableCollection _validations = new();

        private BuildingViewModel _buildingsContext;
        private ParcelViewModel _parcelsContext;

        private Command _createBuilding,
                        _createParcel,
                        _switchSelection;

        public Command CreateBuilding => _createBuilding ??= new Command(obj =>
        {
            BuildingModel building = new();
            _buildings.Add(building);
            // При создании нового инстанса, сразу проводим валидацию и ставим объект "на учет".
            _validations.Validate(building);
        });

        public Command CreateParcel => _createParcel ??= new Command(obj =>
        {
            ParcelModel parcel = new();
            _buildings.Add(parcel);
            _validations.Validate(parcel);
        });

        // Команда будет вызвана при двойном клике на строку с ошибкой.
        // Вообще, с этим отдельная большая тема. Я декомпилировал проект, который вы прислали и смотрел реализацию обратной навигации.
        // Там юзинг AttachedProperty и все намешано между собой. Грань между ViewModel и View очень размыта. Видно, что программист
        // не хотел колхозить и реализовывать фокус в код-бихайнде, но, главная проблема все равно не решена - связь между вьюхой и вью моделью присутствует:
        // вью модель влияет на вью. По идее, парадигма MVVM подразумевает что Модель не знает ни о ком, Вью модель знает о модели, а Вью знает и о модели и о вью модели.
        // Т.е. осведомленность слоев идет сверху вниз.
        // Когда мы делаем подобного рода навигацию, мы ломаем эту цепь, потому-что мы даем Вью модели узнать о вью и о том как оно работает. Если мы это делаем, мы навсегда смешиваем нашу бизнес-логику и UI в одну кашу.
        // Решение которе описано в команде ниже, проблему не решает глобально. Она в принципе не решаема. Потому-что нам нужно из данных бизнеса пробросить навигацию. Но,
        // здесь идёт использование моста.
        // Т.е. в чем отличие - в решении, которе я увидел в исходнике просто расширен контрол и добавлено новое свойство с фокусом. Однако, вызывается оно так же грубо и прямо и Вью модели.
        // Мост на мой взгляд более оптимален. Через мост можно не только вызывать события, но и активировать команды, перегонять объекты между вью моделями и создать стабильную коммуникацию.
        // Т.е. некий контейнер. В данном примере, текущая VM отправляет команду на VM, связанную с той частью бизнес-логики к которой она относится. А вот какая вьюха будет к ней привязана и куда именно эвент придет, 
        // это уже решает сама вьюха.
        public Command SwitchSelection => _switchSelection ??= new Command(obj =>
        {
            BuildingBase? building = Buildings.FirstOrDefault(b => b.ID.Equals(SelectedValidation.EntryID));
            if (building != null)
            {
                switch (building.Type)
                {
                    case TYPE.Building:
                        Messenger.RaiseEvent<BuildingViewModel>(EVENT_TYPE.Focus, SelectedValidation?.EntryName ?? string.Empty);
                        break;
                    case TYPE.Parcel:
                        Messenger.RaiseEvent<ParcelViewModel>(EVENT_TYPE.Focus, SelectedValidation?.EntryName ?? string.Empty);
                        break;
                    default:
                        break;
                }
            }
            SelectedBuilding = building ?? SelectedBuilding;

        }, canExecute => SelectedValidation != null);

        public BuildingBase SelectedBuilding
        {
            get => _selectedBuilding;
            set
            {
                if (value.Type == TYPE.Building)
                    BuildingsContext.SelectedBuilding = (BuildingModel)value;

                else if (value.Type == TYPE.Parcel)
                    ParcelsContext.SelectedParcel = (ParcelModel)value;

                Set(ref _selectedBuilding, value);
            }
        }

        public ValidationResult SelectedValidation
        {
            get => _selectedValidation;
            set => Set(ref _selectedValidation, value);
        }

        public ObservableCollection<BuildingBase> Buildings
        {
            get => _buildings;
            set => Set(ref _buildings, value);
        }

        public BuildingsValidableCollection Validations
        {
            get => _validations;
            set => Set(ref _validations, value);
        }

        public BuildingViewModel BuildingsContext
        {
            get => _buildingsContext;
            set => Set(ref _buildingsContext, value);
        }

        public ParcelViewModel ParcelsContext
        {
            get => _parcelsContext;
            set => Set(ref _parcelsContext, value);
        }

        private void Initialize()
        {
            BuildingsContext = new(Validations);
            ParcelsContext = new(Validations);
        }
    }
}

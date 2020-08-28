using System.Collections.Generic;
using System.Threading.Tasks;
using Desktop.Models;
using Desktop.Services;
using Stylet;

namespace Desktop.ViewModels
{
    public class ShellViewModel : Conductor<object>
    {
        private readonly IWindowManager _windowManager;
        public string RootName
        {
            get => _rootName;
            set
            {
                _rootName = value;
                NotifyOfPropertyChange(() => RootName);
            }
        }

        private string _rootName;

        public string NameSpace
        {
            get => _nameSpace;
            set
            {
                _nameSpace = value;
                NotifyOfPropertyChange(() => NameSpace);
            }
        }

        private string _nameSpace;

        public string RootDirectory
        {
            get => _rootDirectory;
            set
            {
                _rootDirectory = value;
                NotifyOfPropertyChange(() => RootDirectory);
            }
        }

        private string _rootDirectory;

        

        public BindableCollection<StructureEntity> StructureCollection
        {
            get => _structureCollection;
            set
            {
                _structureCollection = value;
                NotifyOfPropertyChange(() => StructureCollection);
            }
        }

        private BindableCollection<StructureEntity> _structureCollection;

        public StructureEntity SelectedStructure
        {
            get => _selectedStructure;
            set
            {
                _selectedStructure = value;
                NotifyOfPropertyChange(() => SelectedStructure);
            }
        }

        private StructureEntity _selectedStructure;


        public string CustomName
        {
            get => _customName;
            set
            {
                _customName = value;
                NotifyOfPropertyChange(() => CustomName);
            }
        }

        private string _customName;

        public string Before
        {
            get => _before;
            set
            {
                _before = value;
                NotifyOfPropertyChange(() => Before);
            }
        }

        private string _before;

        public string After
        {
            get => _after;
            set
            {
                _after = value;
                NotifyOfPropertyChange(() => After);
            }
        }

        private string _after;

        public string SelectedType
        {
            get => _selectedType;
            set
            {
                _selectedType = value;
                NotifyOfPropertyChange(() => SelectedType);
            }
        }

        private string _selectedType;

        public bool IncludeValidator
        {
            get => _includeValidator;
            set
            {
                _includeValidator = value;
                NotifyOfPropertyChange(() => IncludeValidator);
            }
        }

        private bool _includeValidator;

        public BindableCollection<string> TypeCollection
        {
            get => _typeCollection;
            set
            {
                _typeCollection = value;
                NotifyOfPropertyChange(() => TypeCollection);
            }
        }

        private BindableCollection<string> _typeCollection;

        
        
        
        public ShellViewModel(IWindowManager windowManager)
        {
            DisplayName = "MediatR Folder Generator";
            
            _windowManager = windowManager;
            _structureCollection = new BindableCollection<StructureEntity>();
            _typeCollection = new BindableCollection<string>{"Query","Command"};

            //SetDummyData();
        }

        public void AddItem()
        {
            var model = new StructureEntity
            {
                RootName = RootName,
                CustomRootName = CustomName,
                Before = Before,
                After = After,
                MediatrType = SelectedType,
                IncludeValidator = IncludeValidator
            };
            if (ValidateItem(model) == false) return;

            StructureCollection.Add(model);


            CustomName = null;
            Before = null;
            After = null;
            IncludeValidator = false;

        }

        public void RemoveItem()
        {
            if (SelectedStructure == null) return;
            StructureCollection.Remove(SelectedStructure);
            SelectedStructure = null;
        }

        public async Task Create()
        {
            if (string.IsNullOrWhiteSpace(NameSpace))
            {
                _windowManager.ShowMessageBox("need a namespace");
                return;
            }
            else if (string.IsNullOrWhiteSpace(RootDirectory))
            {
                _windowManager.ShowMessageBox("need a root path");
                return;
            }
            else if (string.IsNullOrWhiteSpace(RootName))
            {
                _windowManager.ShowMessageBox("need a root name");
                return;
            }
            var path = $"{RootDirectory}\\{RootName}";
            await FolderGeneratorService.GenerateFolders(new List<StructureEntity>(StructureCollection), path, NameSpace);
            _windowManager.ShowMessageBox("Completed");
            StructureCollection.Clear();
        }

        public void SetDummyData()
        {
            RootDirectory = @"C:\Users\derek\Desktop\DummyData";
            NameSpace = "Application.Modules.PlannerTask";
            RootName = "TaskTemplate";

            StructureCollection.Add(new StructureEntity { RootName = RootName, CustomRootName = "TaskTemplates", Before = "Get", MediatrType = "Query", IncludeValidator = false });
            StructureCollection.Add(new StructureEntity { RootName = RootName, Before = "Get", After = "ByID",MediatrType = "Query", IncludeValidator = false });
            StructureCollection.Add(new StructureEntity { RootName = RootName, Before = "Save",MediatrType = "Command", IncludeValidator = true });
        }

        public bool ValidateItem(StructureEntity entity)
        {
            if (string.IsNullOrWhiteSpace(entity.RootName)) return false;
            if (string.IsNullOrWhiteSpace(entity.MediatrType)) return false;
            return true;
        }
    }
}
using System;
using Stylet;

namespace Desktop.Models
{
    public class StructureEntity : PropertyChangedBase
    {
        private string _id;
        private string _rootName;
        private string _customRootName;
        private string _before;
        private string _after;
        private string _mediatrType;
        private bool _includeValidator;
        private string _rootNameSpace;

        public string ID
        {
            get => _id;
            set => SetAndNotify(ref _id, value);
        }
        public string RootName
        {
            get => _rootName;
            set => SetAndNotify(ref _rootName, value);
        }
        public string CustomRootName
        {
            get => _customRootName;
            set => SetAndNotify(ref _customRootName, value);
        }
        public string Before
        {
            get => _before;
            set => SetAndNotify(ref _before, value);
        }
        public string After
        {
            get => _after;
            set => SetAndNotify(ref _after, value);
        }
        public string MediatrType
        {
            get => _mediatrType;
            set => SetAndNotify(ref _mediatrType, value);
        }
        public bool IncludeValidator
        {
            get => _includeValidator;
            set => SetAndNotify(ref _includeValidator, value);
        }

        public string FullRequestName => (string.IsNullOrWhiteSpace(CustomRootName)) 
            ? $"{RootName}\\{Before}{After}\\{Before}{RootName}{After}{MediatrType}.cs" 
            : $"{RootName}\\{Before}{After}\\{Before}{CustomRootName}{After}{MediatrType}.cs";
        public string FullHandlerName => (string.IsNullOrWhiteSpace(CustomRootName))
            ? $"{RootName}\\{Before}{After}\\{Before}{RootName}{After}Handler.cs"
            : $"{RootName}\\{Before}{After}\\{Before}{CustomRootName}{After}Handler.cs";

        public string FullValidatorName => (IncludeValidator)
            ? (string.IsNullOrWhiteSpace(CustomRootName))
                ? $"{RootName}\\{Before}{After}\\{Before}{RootName}{After}Validator.cs"
                : $"{RootName}\\{Before}{After}\\{Before}{CustomRootName}{After}Validator.cs"
            : "";
    }
}
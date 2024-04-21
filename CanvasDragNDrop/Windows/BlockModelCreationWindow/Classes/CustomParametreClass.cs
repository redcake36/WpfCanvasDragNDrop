using CanvasDragNDrop.UtilityClasses;
using System.Text.RegularExpressions;

namespace CanvasDragNDrop.Windows.BlockModelCreationWindow.Classes
{
    /// <summary> Класс дополнительных параметров и параметров по умолчанию </summary>
    public class CustomParametreClass : NotifyPropertyChangedClass
    {
        public delegate void RegenerateCustomParametersHandler();

        private RegenerateCustomParametersHandler regenerateCustomParameters;

        /// <summary> Наименование параметра по умолчанию или дополнительного параметра </summary>
        public string Title { get; set; }

        /// <summary> Переменная параметра по умолчанию или дополнительного параметра </summary>
        private string symbol;
        public string Symbol
        {
            get => symbol;
            set { symbol = Regex.Replace(value, @"[^a-zA-Z0-9]", ""); OnPropertyChanged(); regenerateCustomParameters?.Invoke(); }
        }

        /// <summary> Единицы измерения параметра по умолчанию или дополнительного параметра </summary>
        public string Units { get; set; }
        public CustomParametreClass(string title, string symbol, string units, RegenerateCustomParametersHandler handler = null)
        {
            regenerateCustomParameters = handler;
            Title = title;
            Symbol = symbol;
            Units = units;
        }
    }
}

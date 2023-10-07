using System.Text.RegularExpressions;

namespace CanvasDragNDrop
{
    /// <summary> Класс дополнительных параметров и параметров по умолчанию </summary>
    public class CustomParametreClass : NotifyPropertyChangedClass
    {
        public delegate void RegenerateCustomParametresHandler();

        private RegenerateCustomParametresHandler regenerateCustomParametres;

        /// <summary> Наименование параметра по умолчанию или дополнительного параметра </summary>
        public string Title { get; set; }

        /// <summary> Переменная параметра по умолчанию или дополнительного параметра </summary>
        private string symbol;
        public string Symbol
        {
            get => symbol;
            set { symbol = Regex.Replace(value, @"[^a-zA-Z0-9]", ""); OnPropertyChanged(); regenerateCustomParametres?.Invoke(); }
        }

        /// <summary> Единицы измерения параметра по умолчанию или дополнительного параметра </summary>
        public string Units { get; set; }
        public CustomParametreClass(string title, string symbol, string units, RegenerateCustomParametresHandler handler = null)
        {
            regenerateCustomParametres = handler;
            Title = title;
            Symbol = symbol;
            Units = units;
        }
    }
}

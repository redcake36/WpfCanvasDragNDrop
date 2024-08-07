﻿using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace CanvasDragNDrop.UtilityClasses
{
    public class NotifyPropertyChangedClass : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}

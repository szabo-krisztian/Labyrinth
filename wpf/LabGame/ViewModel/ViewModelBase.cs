using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace ViewModelBaseNM;

public abstract class ViewModelBase : INotifyPropertyChanged
{
    protected ViewModelBase() { }

    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}


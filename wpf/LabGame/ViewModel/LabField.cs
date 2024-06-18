using System;
using ViewModelBaseNM;

namespace LabFieldNM;

public class LabField : ViewModelBase
{
    private Boolean _isLit;
    private Boolean _isPlayer;

    public Boolean IsLit
    {
        get { return _isLit; }
        set
        {
            if (_isLit != value)
            {
                _isLit = value;
                OnPropertyChanged();
            }
        }
    }

    public Boolean IsPlayer
    {
        get { return _isPlayer; }
        set
        {
            if (_isPlayer != value)
            {
                _isPlayer = value;
                OnPropertyChanged();
            }
        }
    }

    public LabField()
    {
        _isLit = false;
        _isPlayer = false;
    }
}
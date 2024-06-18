using ViewModelBaseNM;

namespace LabFieldNM;

public class LabField : ViewModelBase
{
    private Boolean _isLit;
    private Boolean _isPlayer;
    private Boolean _isLitAndNotPlayer;
    private Boolean _notIsLitAndNotPlayer;
    private int _x, _y;

    public int X
    {
        get { return _x; }
        set
        {
            if (_x != value)
            {
                _x = value;
                OnPropertyChanged();
            }
        }
    }

    public int Y
    {
        get { return _y; }
        set
        {
            if (_y != value)
            {
                _y = value;
                OnPropertyChanged();
            }
        }
    }

    public Boolean IsLit
    {
        get { return _isLit; }
        set
        {
            if (_isLit != value)
            {
                _isLit = value;

                _isLitAndNotPlayer = _isLit && !_isPlayer;
                _notIsLitAndNotPlayer = !_isLit && !_isPlayer;
                OnPropertyChanged(nameof(IsLitAndNotIsPlayer));
                OnPropertyChanged(nameof(NotIsLitAndNotIsPlayer));

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
                
                _isLitAndNotPlayer = _isLit && !_isPlayer;
                _notIsLitAndNotPlayer = !_isLit && !_isPlayer;
                OnPropertyChanged(nameof(IsLitAndNotIsPlayer));
                OnPropertyChanged(nameof(NotIsLitAndNotIsPlayer));

                OnPropertyChanged();
            }
        }
    }

    public Boolean IsLitAndNotIsPlayer
    {
        get { return _isLitAndNotPlayer; }
    }

    public Boolean NotIsLitAndNotIsPlayer
    {
        get { return _notIsLitAndNotPlayer; }
    }

    public LabField(int i, int j)
    {
        _x = j;
        _y = i;
        _isLit = false;
        _isPlayer = false;
        _isLitAndNotPlayer = false;
        _notIsLitAndNotPlayer = true;
    }
}
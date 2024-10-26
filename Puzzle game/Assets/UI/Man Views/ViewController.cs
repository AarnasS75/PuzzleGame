using UnityEngine;

public class ViewController : MonoBehaviour
{
    [SerializeField] protected View _startingView;
    [SerializeField] protected View[] _views;

    protected View _currentView;

    protected virtual void Start()
    {
        Initialize();
    }

    private void Initialize()
    {
        for (int i = 0; i < _views.Length; i++)
        {
            _views[i].Initialize();
            _views[i].Hide();
        }

        if (_startingView != null)
        {
            Show(_startingView);
        }
    }

    protected T Get<T>() where T : View
    {
        for (int i = 0; i < _views.Length; i++)
        {
            if (_views[i] is T tTab)
            {
                return tTab;
            }
        }
        return null;
    }

    protected void Show<T>() where T : View
    {
        for (int i = 0; i < _views.Length; i++)
        {
            if (_views[i] is T)
            {
                if (_currentView != null)
                {
                    _currentView.Hide();
                }

                _views[i].Show();

                _currentView = _views[i];
            }
        }
    }

    private void Show(View tab)
    {
        if (_currentView != null)
        {
            _currentView.Hide();
        }
        tab.Show();

        _currentView = tab;
    }
}

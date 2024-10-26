using UnityEngine;

public class MainMenuViewController : MonoBehaviour
{
    [SerializeField] private View _startingView;
    [SerializeField] private View[] _views;

    private View _currentView;

    private void Start()
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

    private T GetTab<T>() where T : View
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

    private void Show<T>() where T : View
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

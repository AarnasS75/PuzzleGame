using UnityEngine;

public abstract class OptionsController<T> : MonoBehaviour
{
    protected string _optionsKey;

    protected virtual void Start()
    {
        if (PlayerPrefs.HasKey(_optionsKey))
        {
            Load();
        }
        else
        {
            Save();
        }
    }

    public abstract void ApplySetting(T value);

    protected abstract void Save();

    protected abstract void Load();
}

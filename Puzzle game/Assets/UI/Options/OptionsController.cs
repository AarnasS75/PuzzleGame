using UnityEngine;

public abstract class OptionsController<T> : MonoBehaviour
{
    public abstract void ApplySetting(T value);

    public abstract void Save();

    public abstract void Load();
}

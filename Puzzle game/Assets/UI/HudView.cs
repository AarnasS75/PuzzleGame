using UnityEngine;

public class HudView : View
{
    [SerializeField] private HudSlot[] _slots;

    private void Awake()
    {
        StaticEventsHandler.OnPuzzleCompleted += StaticEventsHandler_OnPuzzleCompleted;
    }

    private void Start()
    {
        foreach (var slot in _slots)
        {
            slot.ResetSlot();
        }
    }

    private void StaticEventsHandler_OnPuzzleCompleted(View obj)
    {
        if (obj is PipesPuzzleView)
        {
            _slots[0].Initialize(GameResources.Instance.PipesPuzzleIcon);
        }
        else if (obj is LockPuzzleView)
        {
            _slots[1].Initialize(GameResources.Instance.LockPuzzleIcon);
        }
        else if (obj is FlashingLightsView)
        {
            _slots[2].Initialize(GameResources.Instance.FlashingLightsPuzzleIcon);
        }
        else if (obj is SymbolMatchPuzzleView)
        {
            _slots[3].Initialize(GameResources.Instance.MemoryPuzzleIcon);
        }
    }
}

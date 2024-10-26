using UnityEngine;

public class HudView : View
{
    [SerializeField] private HudSlot[] _slots;

    public override void Initialize()
    {
        foreach (var slot in _slots)
        {
            slot.ResetSlot();
        }
    }

    public void UpdateHud(View obj)
    {
        if (obj is PipesPuzzleView)
        {
            _slots[0].Initialize(GameResources.Instance.PipesPuzzleIcon);
        }
        else if (obj is LockPuzzleView)
        {
            _slots[1].Initialize(GameResources.Instance.LockPuzzleIcon);
        }
        else if (obj is FlashingLightsPuzzleView)
        {
            _slots[2].Initialize(GameResources.Instance.FlashingLightsPuzzleIcon);
        }
        else if (obj is SymbolMatchPuzzleView)
        {
            _slots[3].Initialize(GameResources.Instance.MemoryPuzzleIcon);
        }
    }

    public bool TryUsePuzzlePiece(int buttonPressed)
    {
        return _slots[buttonPressed - 1].TryUse();
    }
}

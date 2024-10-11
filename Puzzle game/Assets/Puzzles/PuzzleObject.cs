using UnityEngine;

public class PuzzleObject : MonoBehaviour
{
    [SerializeField] private Puzzle _puzzle;
    public Puzzle Puzzle => _puzzle;

    [Header("NUMBER SEQUENCE")]
    [SerializeField] private int[] _numberSequence;
    public int[] NumberSequence => _numberSequence;
}

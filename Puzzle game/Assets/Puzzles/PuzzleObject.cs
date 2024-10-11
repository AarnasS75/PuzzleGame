using UnityEngine;

public class PuzzleObject : MonoBehaviour
{
    [SerializeField] private Puzzle _puzzle;
    public Puzzle Puzzle => _puzzle;
}

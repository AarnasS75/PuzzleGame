using UnityEngine;

public class GameResources : MonoBehaviour
{
    private static GameResources instance;

    public static GameResources Instance
    {
        get
        {
            if (instance == null)
            {
                instance = Resources.Load<GameResources>("GameResources");
            }
            return instance;
        }
    }

    [Header("Puzzles Icons")]
    public Sprite PipesPuzzleIcon;
    public Sprite LockPuzzleIcon;
    public Sprite MemoryPuzzleIcon;
    public Sprite FlashingLightsPuzzleIcon;
}
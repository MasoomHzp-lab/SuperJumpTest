using UnityEngine;

public class CheatManager : MonoBehaviour
{

    public static CheatManager Instance { get; private set; }

    private int? forcedNextRoll = null;
    private float expireAt = -1f;

    private void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
}
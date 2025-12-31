using System.Collections.Generic;
using UnityEngine;

public class LudoCaptureByIndex : MonoBehaviour
{  
    [Header("References")]
    public List<PlayerController> players = new List<PlayerController>();
    public BoardManager boardManager;

    [Header("Rules")]
    public bool allowCaptureOnHomePath = false;   // معمولا false
    public bool allowCaptureOnStartTiles = false; // false یعنی خانه‌های شروع امن‌اند
    [Tooltip("RingId های امن (ستاره‌ها) روی لوپ مشترک.")]
    public List<int> safeLoopIndices = new List<int>();
    public bool allowStackSameColor = true;       // هم‌رنگ‌ها می‌تونن هم‌خانه شوند


    [Header("Same-tile precision")]
    public bool enforceCenterSnap = true;         // برای اسنپ دقیق پیشنهاد می‌شود true باشد
    public float ringCenterSnap = 0.08f;          // ~8cm

    [Header("Auto-discovery")]
    public bool autoFindPlayersIfEmpty = true;
    public float autoFindInterval = 1.0f;

 // snapshots
    private readonly Dictionary<Token, bool> lastMoving = new Dictionary<Token, bool>();
    private readonly Dictionary<Token, int>  lastIndex  = new Dictionary<Token, int>();
    private float _findTimer;

private void OnEnable()
    {
        if (boardManager == null)
            boardManager = FindAnyObjectByType<BoardManager>();
        PrimePlayersAndTokens();
    }

    private void Update()
    {
        // کشف خودکار پلیرها اگر لیست خالی است
        if (autoFindPlayersIfEmpty && (players == null || players.Count == 0))
        {
            _findTimer -= Time.deltaTime;
            if (_findTimer <= 0f)
            {
                _findTimer = autoFindInterval;
                var found = FindObjectsOfType<PlayerController>();
                if (found != null && found.Length > 0)
                {
                    players = new List<PlayerController>(found);
                    PrimePlayersAndTokens();
                    Debug.Log("[Capture] Auto-discovered players.");
                }
            }
        }
   
}
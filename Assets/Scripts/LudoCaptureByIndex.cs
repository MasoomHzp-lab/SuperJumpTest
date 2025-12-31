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
<<<<<<< HEAD
         if (players == null || players.Count == 0) return;
        if (boardManager == null || boardManager.commonPath == null || boardManager.commonPath.Count == 0) return;

        foreach (var p in players)
        {
            if (p == null) continue;
            var tokens = p.GetTokens();
            if (tokens == null) continue;

            foreach (var t in tokens)
            {
                if (t == null) continue;

                if (!lastMoving.ContainsKey(t)) lastMoving[t] = t.isMoving;
                if (!lastIndex.ContainsKey(t))  lastIndex[t]  = t.currentTileIndex;

                bool wasMoving = lastMoving[t];
                int  wasIndex  = lastIndex[t];

                bool nowMoving = t.isMoving;
                int  nowIndex  = t.currentTileIndex;

                bool landedNow = (wasMoving && !nowMoving) || ((nowIndex != wasIndex) && !nowMoving);

                if (landedNow)
                    OnTokenLanded(t);

                lastMoving[t] = nowMoving;
                lastIndex[t]  = nowIndex;
            }
        }
    }

        private void OnTokenLanded(Token landed)
    {
        if (landed == null || !landed.isOnBoard || boardManager == null) return;

        int commonCount = (boardManager.commonPath != null) ? boardManager.commonPath.Count : 0;
        if (commonCount <= 0) return;

        bool moverOnHomePath = (landed.currentTileIndex >= commonCount);

        // در مسیر خانۀ پایان معمولا کپچر نداریم
        if (moverOnHomePath && !allowCaptureOnHomePath) return;

        // RingId مهاجم
        if (!TryGetRingId(landed, out int moverRing)) return;

        // ــ اسنپ مهاجم به مرکز خانه (برای اینکه دقیقاً "روی هم" دیده شوند)
        SnapToRingCenter(landed, moverRing);

        foreach (var p in players)
        {
            if (p == null) continue;
            var tokens = p.GetTokens();
            if (tokens == null) continue;

            foreach (var other in tokens)
            {
                if (other == null || other == landed) continue;
                if (!other.isOnBoard) continue;

                bool otherOnHomePath = (other.currentTileIndex >= commonCount);
                if ((moverOnHomePath || otherOnHomePath) && !allowCaptureOnHomePath)
                    continue;

                bool sameOwner = (other.owner != null && landed.owner != null && other.owner == landed.owner);
                bool sameColor = other.color == landed.color ||
                                 (other.owner != null && landed.owner != null &&
                                  other.owner.color == landed.owner.color);
                if ((sameOwner || sameColor) && allowStackSameColor)
                    continue;

                if (!TryGetRingId(other, out int otherRing))
                    continue;

                if (otherRing != moverRing)
                    continue;

                if (enforceCenterSnap && !BothNearRingCenter(moverRing, landed, other))
                    continue;

                if (IsSafeLoopIndex(otherRing, commonCount))
                    continue;

                if (!allowCaptureOnStartTiles && IsStartRing(otherRing, commonCount))
                    continue;

                // --- کُشتن: قربانی برگردد خانه (اولین اسلات خالی)
                SendHomeRobust(other);

                Debug.Log($"[Capture] {landed.owner?.playerName} captured {other.owner?.playerName}'s token.");
            }
        }
    }

=======
>>>>>>> 5c84ac251dd8e003ed7838155a6e4da2828cbf8a
   
}
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RulesManager : MonoBehaviour
{
    
[Header("Win")]
    public WinManager winManager;

       // ---------- Finish Bays ----------
    [System.Serializable]
    public class FinishBay
    {
        public PlayerColor color;
        public List<Transform> slots = new List<Transform>(); // ترتیب اسلات‌ها
    }

    [Header("Finish Bays (assign in Inspector)")]
    public List<FinishBay> finishBays = new List<FinishBay>();

    // ---------- Internal State ----------
    private readonly HashSet<Token> finishedTokens = new HashSet<Token>();
    private readonly Dictionary<Token, int> homeSlotOfToken = new Dictionary<Token, int>();

    private readonly Dictionary<PlayerColor, int> finishCounters = new Dictionary<PlayerColor, int>();


    // ======================================
    // Public API (GameManager / others call these)
    // ======================================

    /// تمام مهره‌ها را به نزدیک‌ترین اسلات خانه‌شان مپ می‌کند (برای شروع بازی/لود صحنه)
    public void EnsureHomeSlotAssignedForAll(List<PlayerController> players)
    {
        if (players == null) return;
        foreach (var p in players)
        {
            if (p == null || p.Tokens == null) continue;
            foreach (var t in p.Tokens) EnsureHomeSlotAssigned(t);
        }
    }

}




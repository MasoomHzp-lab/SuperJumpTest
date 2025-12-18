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
}

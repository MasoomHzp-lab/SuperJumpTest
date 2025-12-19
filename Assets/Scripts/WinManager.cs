using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class WinManager : MonoBehaviour
{
    [Header("Win Condition")]
    [Tooltip("چند مهره باید به خانه‌ی آخر برسند تا آن رنگ برنده شود؟")]
    public int tokensToWin = 4;

    [Header("Win UI")]
    public GameObject winPanel;          // پنل برد
    public TextMeshProUGUI winText;      // متن داخل پنل برد

    private readonly Dictionary<PlayerColor, int> finishCounters =
        new Dictionary<PlayerColor, int>();

    private bool gameEnded = false;
    /// وقتی یک مهره وارد خانه‌های آخر خودش شد این متد را صدا بزن
    public void RegisterFinishedToken(Token token)
    {
        if (token == null || gameEnded) return;

        var owner = token.owner;
        if (owner == null)
        {
            Debug.LogError("[WinManager] Token has no owner!");
            return;
        }

        PlayerColor color = owner.color;
        Debug.Log($"[WinManager] RegisterFinishedToken called for {color}, token = {token.name}");

        if (!finishCounters.ContainsKey(color))
            finishCounters[color] = 0;

        finishCounters[color]++;

        Debug.Log($"[WinManager] {color} finished tokens = {finishCounters[color]}");

        if (finishCounters[color] >= tokensToWin)
        {
            DeclareWinner(color);
        }
    }
}
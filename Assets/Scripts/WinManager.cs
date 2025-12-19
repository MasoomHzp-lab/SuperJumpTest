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
    [Header("Lose UI ( MainLandWithAi)")]
    [Tooltip("اگر این تیک را بزنی، منطق باخت برای بازیکن حقیقی فعال می‌شود.")]
    public bool useLoseLogic = false;

    [Tooltip("رنگ بازیکن حقیقی در صحنه‌ی MainLandWithAi")]
    public PlayerColor humanPlayerColor = PlayerColor.Red;

    public GameObject losePanel;         // پنل باخت
    public TextMeshProUGUI loseText;     // متن داخل پنل باخت

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
    // -------------------- منطق برد/باخت --------------------
    private void DeclareWinner(PlayerColor winnerColor)
    {
        if (gameEnded) return;
        gameEnded = true;

        Debug.Log($"[WinManager] Winner = {winnerColor}");

        // اگر لوز فعال نباشد (سین MainLand معمولی): فقط پنل برد مثل قبل
        if (!useLoseLogic)
        {
            ShowResultPanel(winPanel, winText, winnerColor, isWin: true);
            return;
        }

        // اگر لوز فعال است (MainLandWithAi)
        bool humanWon = (winnerColor == humanPlayerColor);

        if (humanWon)
        {
            // بازیکن حقیقی برده → پنل برد
            ShowResultPanel(winPanel, winText, winnerColor, isWin: true);
        }
        else
        {
            // یکی از بات‌ها برده → پنل باخت
            ShowResultPanel(losePanel, loseText, winnerColor, isWin: false);
        }
    }
}
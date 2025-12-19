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
}
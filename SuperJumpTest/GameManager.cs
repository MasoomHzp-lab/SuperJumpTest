using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameManager : MonoBehaviour
{

   // ===== Singleton (اختیاری) =====
    public static GameManager I;
    private void Awake()
    {
        I = this;

        if (turnTimer != null)
    {
        // وقتی تایمر تموم شد → نوبت بسوزه
       
    }


        if (dice == null)
            Debug.LogError("[GameManager] Dice is not assigned.");

        // back-reference برای هر PlayerController
        foreach (var p in players)
            if (p != null && p.gameManager != this)
                p.gameManager = this;
    }
    // ===============================

    [Header("Players (order of turns)")]
    public List<PlayerController> players = new List<PlayerController>();

    [Header("Dice")]
    public Dice dice; // باید OnDiceRolled(int) داشته باشد

    [Header("Rules")]
    public RulesManager rules; // ← در Inspector ست کن


    [Header("Turn Timer")]
    public TurnTimer turnTimer; // در Inspector ست کن
    public float turnSeconds = 20f;

    // بازیکن فعلی
    public PlayerController CurrentPlayer => players.Count > 0 ? players[currentPlayerIndex] : null;

    // وضعیت نوبت/تاس
    private int currentPlayerIndex = 0;
    private int lastDice = 0;
    private bool rolledSix = false;

    // قوانین پایه لودو
    [Header("Ludo Rules")]
    [Tooltip("ورود به زمین فقط با ۶")]
    public bool enterOnlyOnSix = true;

    [Tooltip("اگر ۶ بیاید نوبت اضافه بماند")]
    public bool extraTurnOnSix = true;

    // کنترل انتخاب توکن
    private Token pendingSelected = null;

    // قفل‌کردن تاس
    [SerializeField] private bool canRoll = true;
    public bool CanRoll() => canRoll;

    private void OnEnable()
    {
        if (dice != null)
            dice.OnDiceRolled += HandleDiceRolled;
    }

    private void OnDisable()
    {
        if (dice != null)
            dice.OnDiceRolled -= HandleDiceRolled;
    }

    private void Start()
    {
        ApplyPlayerCountFromSettings();
        SetCurrentPlayer(0);
        canRoll = true;

        // تعیین اسلات خانۀ اختصاصی مهره‌ها (برای بازگشت صحیح به Home)
        rules?.EnsureHomeSlotAssignedForAll(players);
    }

    /// <summary>
    /// تعداد بازیکن‌ها را از MatchSettings گرفته و لیست players را کوتاه/فعال می‌کند.
    /// </summary>
    private void ApplyPlayerCountFromSettings()
    {
        // فقط بازیکن‌های معتبر
        players = players.Where(p => p != null && p.gameObject != null).ToList();

        int desired = players.Count;
        if (MatchSettings.Instance != null)
            desired = Mathf.Clamp(MatchSettings.Instance.playerCount, 2, 4);

        // فعال/غیرفعال کردن بر اساس desired
        for (int i = 0; i < players.Count; i++)
        {
            bool active = (i < desired) && players[i] != null;
            if (players[i] != null)
                players[i].gameObject.SetActive(active);
        }

        // لیست را کوتاه کن تا players.Count دقیق شود
        if (players.Count > desired)
            players = players.GetRange(0, desired);

        // back-reference دوباره
        foreach (var p in players)
            if (p != null && p.gameManager != this)
                p.gameManager = this;
    }

    private void SetCurrentPlayer(int index)
    {
        currentPlayerIndex = Mathf.Clamp(index, 0, Mathf.Max(0, players.Count - 1));
        lastDice = 0;
        rolledSix = false;
        pendingSelected = null;

        if (CurrentPlayer != null)
        {
            if (dice != null) dice.currentPlayer = CurrentPlayer;
            Debug.Log($"[Turn] {CurrentPlayer.playerName}");
        }

      // خاموش کردن انیمیشن همه مهره‌ها
    foreach (var p in players)
    {
        if (p == null || p.Tokens == null) continue;
        foreach (var t in p.Tokens)
        {
            if (t == null) continue;
            var animator = t.GetComponent<PieceAnimator>();
            if (animator != null) animator.SetActive(false);
        }
    }

    // روشن کردن انیمیشن مهره‌های بازیکن فعلی (درصورت وجود)
    if (CurrentPlayer != null && CurrentPlayer.Tokens != null)
    {
        foreach (var t in CurrentPlayer.Tokens)
        {
            if (t == null) continue;
            var animator = t.GetComponent<PieceAnimator>();
            if (animator != null) animator.SetActive(true);
        }
    }

        canRoll = true; // بازیکن جدید می‌تواند تاس بیندازد

        // شروع شمارش معکوس برای بازیکن جدید
        if (turnTimer != null)
            turnTimer.StartTimer(turnSeconds);

    }

    /// <summary>
    /// هوک رول از طرف Dice
    /// </summary>
    private void HandleDiceRolled(int steps)
    {
        if (!canRoll)
        {
            Debug.Log("[GM] Dice roll ignored (canRoll=false).");
            return;
        }

        if (CurrentPlayer == null) return;

        // پس از رول، تا پایان حرکت/پاس نوبت قفل شو
        canRoll = false;

        lastDice = steps;
        rolledSix = (steps == 6);

        if (!HasLegalMove(CurrentPlayer, lastDice))
        {
            Debug.Log("[GM] No legal move. Passing turn.");
            StartCoroutine(PassTurnImmediately());
            return;
        }

        Debug.Log($"[{CurrentPlayer.playerName}] rolled: {steps}. Select a token.");
        // حالا منتظر OnTokenSelected از طرف کلیک بازیکن می‌مانیم
    }

    /// <summary>
    /// از Token.OnMouseDown یا UI کال می‌شود.
    /// </summary>
    public void OnTokenSelected(Token token)
    {
        if (token == null) return;
        if (CurrentPlayer == null) return;
        if (token.owner != CurrentPlayer) return;

        if (lastDice <= 0)
        {
            Debug.Log("[GM] Roll the dice first, then select a token.");
            return;
        }

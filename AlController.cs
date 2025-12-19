using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
public class AIController : MonoBehaviour
{
 [Header("References")]
    public GameManager gameManager;   // assign in Inspector
    public Dice dice;                 // assign in Inspector (same dice)
    public PlayerController self;     // this AI's PlayerController

    [Header("Timing")]
    public float rollDelay = 0.6f;    // wait before rolling
    public float selectDelay = 0.6f;  // wait before selecting a token

    [Header("Behavior")]
    public bool preferEnterOnSix = true;        // enter when 6 if start is free
    public bool preferFarthestAdvance = true;   // otherwise choose farthest

    // internal
    private bool awaitingMyRollResult = false;

    private void Reset()
    {
        self = GetComponent<PlayerController>();
    }

    private void Awake()
    {
        if (self == null) self = GetComponent<PlayerController>();
    }
 private void OnEnable()
    {
        if (dice != null) dice.OnDiceRolled += OnDiceRolled;
    }

    private void OnDisable()
    {
        if (dice != null) dice.OnDiceRolled -= OnDiceRolled;
    }

    private void Update()
    {
        if (gameManager == null || self == null) return;

        // فقط نوبت خودِ AI
        if (gameManager.CurrentPlayer != self) return;

        // اگر مهره‌ای در حال حرکت است، صبر کن
        if (self.IsMoving()) return;

        // اگر در انتظار نتیجه‌ی رول قبلی هستیم، صبر
        if (awaitingMyRollResult) return;

        // اگر تاس از دید GM مجاز است، رول کن
        if (gameManager.CanRoll())
            StartCoroutine(AIRollAfterDelay());
    }

    private IEnumerator AIRollAfterDelay()
    {
        awaitingMyRollResult = true; // جلوگیری از رول‌های تکراری
        yield return new WaitForSeconds(rollDelay);

        // دوباره چک کن، شاید وسط تاخیر نوبت عوض شده
        if (gameManager == null || gameManager.CurrentPlayer != self || !gameManager.CanRoll())
        {
            awaitingMyRollResult = false;
            yield break;
        }

        if (dice != null)
        {
            dice.Roll(); // مطمئن شو متد صحیح همینه (Roll/RollDice)
        }
        else
        {
            Debug.LogWarning("[AI] Dice reference is missing.");
            awaitingMyRollResult = false;
        }
    }
 private void OnDiceRolled(int value)
    {
        // فقط واکنش در نوبت خودمان
        if (gameManager == null || gameManager.CurrentPlayer != self) return;

        StartCoroutine(AISelectAfterDelay(value));
    }
 private IEnumerator AISelectAfterDelay(int value)
    {
        yield return new WaitForSeconds(selectDelay);

        if (gameManager == null || self == null) { awaitingMyRollResult = false; yield break; }

        var tokens = self.Tokens != null ? new List<Token>(self.Tokens) : null;
        if (tokens == null || tokens.Count == 0)
        {
            awaitingMyRollResult = false;
            yield break;
        }

        // از بین حرکت‌های قانونی انتخاب کن
        Token choice = ChooseToken(tokens, value);

        if (choice != null)
        {
            gameManager.OnTokenSelected(choice);
            // تا پایان حرکت/مدیریت نوبت صبر کن
            StartCoroutine(ReleaseAwaitingWhenDone());
        }
        else
        {
            // هیچ حرکت قانونی نداریم → GM خودش پاس می‌ده
            awaitingMyRollResult = false;
        }
    }

    private IEnumerator ReleaseAwaitingWhenDone()
    {
        // صبر تا حرکت/نوبت مدیریت شود
        while (self != null && self.IsMoving())
            yield return null;

        // یه فریم برای هماهنگی با GM
        yield return null;
        awaitingMyRollResult = false;
    }

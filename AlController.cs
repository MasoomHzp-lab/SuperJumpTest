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

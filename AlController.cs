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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LudoExtraTurn : MonoBehaviour
{
    [Header("References")]
    public Dice dice;
    public List<PlayerController> players = new List<PlayerController>();

    [Header("Settings")]
    public bool enabledExtraTurn = true;

    private bool lastRollWasSix = false;
    private readonly Dictionary<PlayerController, bool> movingMap = new Dictionary<PlayerController, bool>();
    private PlayerController snapshotPlayerOnRoll;
    private void OnEnable()
    {
        if (dice != null) dice.OnDiceRolled += OnDiceRolled;
    }

    private void OnDisable()
    {
        if (dice != null) dice.OnDiceRolled -= OnDiceRolled;
    }

    private void Start()
    {
        foreach (var p in players)
            movingMap[p] = false;
    }

}

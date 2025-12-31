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

}

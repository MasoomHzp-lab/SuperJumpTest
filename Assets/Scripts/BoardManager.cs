using System.Collections.Generic;
using UnityEngine;

public class BoardManager : MonoBehaviour
{
    // === Singleton ===
    public static BoardManager I;
    private void Awake()
    {
        I = this;
        // If you want this to persist across scene loads, uncomment:
        // DontDestroyOnLoad(gameObject);
    }
    // ================

    [Header("Common Path (shared loop of 52 tiles)")]
    public List<Transform> commonPath = new();

    [Header("Final Home Paths (6 tiles each)")]
    public List<Transform> redHome = new();
    public List<Transform> blueHome = new();
    public List<Transform> yellowHome = new();
    public List<Transform> greenHome = new();


    [Header("Start Index on Common Path")]
    public int redStartIndex = 0;
    public int blueStartIndex = 11;
    public int yellowStartIndex = 22;
    public int greenStartIndex = 33;

    [Header("Home Entry Offset from Start")]
    [Tooltip("-1 = tile before start, 0 = start itself, commonPath.Count = after full loop")]
    public int redHomeEntryOffset = -1;
    public int blueHomeEntryOffset = -1;
    public int yellowHomeEntryOffset = -1;
    public int greenHomeEntryOffset = -1;

}
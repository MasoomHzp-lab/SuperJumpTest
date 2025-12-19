using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Token : MonoBehaviour
{
     AudioManager audioManager;
    public AudioClip TokenSound;
    [HideInInspector] public PlayerColor color;
    [HideInInspector] public int currentTileIndex = -1; // -1 یعنی هنوز روی برد نیست (خانه)
    [HideInInspector] public bool isMoving = false;
    [HideInInspector] public bool isOnBoard = false;

    private BoardManager boardManager;
    [HideInInspector] public PlayerController owner;
    private GameManager gameManager;
    public RulesManager rulesManager;

    public int homeSlot = -1;   // اسلات خانه‌ی اختصاصی این مهره (0..3). -1 یعنی هنوز ست نشده.
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;


public class Dice : MonoBehaviour
{
    AudioManager audioManager;
    public AudioClip diceRollingSound;

    [Header("Dice Settings")]
    public Sprite[] diceSides;         // 6 تصویر
    public int[] diceValues = new int[6]; // 6 مقدار متناظر
    public Image diceImage;

    [Header("Roll FX")]
    public float rollDuration = 1f;
    public float rollSpeed = 0.05f;

    public event Action<int> OnDiceRolled;

    private bool isRolling = false;

    public PlayerController currentPlayer;

    void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("AudioManager").GetComponent<AudioManager>();

    }


}
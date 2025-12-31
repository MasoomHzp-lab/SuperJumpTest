using UnityEngine;

public class ThemeApplier : MonoBehaviour
{
    [Header("Where to apply board sprite")]
    public SpriteRenderer boardRenderer;  // اختیاری، اگه خالی باشه خودش از همین آبجکت می‌گیره
    private void Awake()
    {
        // اگر یادت رفت دستی ست کنی، خودش می‌ره روی همون آبجکت دنبال SpriteRenderer می‌گرده
        if (boardRenderer == null)
            boardRenderer = GetComponent<SpriteRenderer>();
    }

}
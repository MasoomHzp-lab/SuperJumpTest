using UnityEngine;
public class ThemeButton : MonoBehaviour
{
     [Header("Theme info")]
    public string themeId;     // باید با ThemeManager.themeId یکی باشه

    [Header("Paid settings (for future)")]
    public bool isPaidTheme = false; 
    public string productId;   // اسم محصول در بازار، بعدا پرش می‌کنیم

}
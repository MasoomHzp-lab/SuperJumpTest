using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ThemeManager : MonoBehaviour
{
    public static ThemeManager I;

    [System.Serializable]
    public class ThemeEntry
    {
        public string themeId;     // مثلا: "Classic", "Galaxy", "Forest", ...
        public Sprite boardSprite;
    }
     [Header("Themes list")]
    public List<ThemeEntry> themes = new List<ThemeEntry>();

    [Header("Defaults")]
    public string defaultThemeId = "DarkSky";

    [Header("Free themes (always unlocked)")]
    public List<string> freeThemeIds = new List<string>(); // مثلا ["Classic","Galaxy"]

    private const string ActiveThemeKey = "ActiveThemeId";
}
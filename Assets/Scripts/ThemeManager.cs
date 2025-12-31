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
}
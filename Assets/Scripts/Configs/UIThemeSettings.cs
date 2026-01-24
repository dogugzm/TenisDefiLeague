using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

namespace Configs
{
    [CreateAssetMenu(fileName = "UIThemeSettings", menuName = "TenisDefi/UI Theme Settings")]
    public class UIThemeSettings : ScriptableObject
    {
        [FormerlySerializedAs("PanelColor")] [ColorUsage(true)]
        public Color PanelBGColor = new Color(0.15f, 0.15f, 0.15f, 1f);

        [ColorUsage(true)] public Color HeaderBGColor = new Color(0.15f, 0.15f, 0.15f, 1f);


        [Title("Navbar Colors")] [ColorUsage(true)]
        public Color ActiveColor = new Color(0.1f, 0.1f, 0.1f, 1f);

        [ColorUsage(true)] public Color DeactiveColor = new Color(0.1f, 0.1f, 0.1f, 1f);


        [Title("Fonts")] public TMP_FontAsset MainFont;
        public TMP_FontAsset HeaderFont;
    }
}
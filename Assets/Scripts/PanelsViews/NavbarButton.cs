using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace PanelsViews
{
    public class NavbarButton : MonoBehaviour
    {
        public Button Button;
        [SerializeField] private Image icon;
        [SerializeField] private TMP_Text label;

        public void Convert(bool isActive, Color activeColor, Color inactiveColor)
        {
            if (icon != null)
            {
                icon.color = isActive ? activeColor : inactiveColor;
            }
            
            if (label != null)
            {
                label.color = isActive ? activeColor : inactiveColor;
            }
        }
    }
}

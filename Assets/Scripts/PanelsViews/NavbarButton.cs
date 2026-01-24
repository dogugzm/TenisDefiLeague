using Configs;
using DG.Tweening;
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
        [SerializeField] private Image topSelectedImage;
        
        public void Convert(bool isActive, Color activeColor, Color inactiveColor)
        {
            DOTween.Kill(this, true);
            if (icon != null)
            {
                icon.DOColor(isActive ? activeColor : inactiveColor, .2f).SetEase(Ease.OutQuad).SetId(this);
            }

            if (label != null)
            {
                label.DOColor(isActive ? activeColor : inactiveColor, .2f).SetEase(Ease.OutQuad).SetId(this);
            }

            if (topSelectedImage != null)
            {
                topSelectedImage.transform.DOScale(1f, 0.2f).From(0).SetEase(Ease.OutBack).SetId(this);
                topSelectedImage.enabled = isActive;
                topSelectedImage.color = activeColor;
            }
        }
    }
}
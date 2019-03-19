using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace PBCore.Localization
{
    /// <summary>
    /// GUI
    /// </summary>
    [RequireComponent(typeof(Image)), DisallowMultipleComponent]
    public class LocalizationGUIImage : BaseLocalizationGUI
    {
        private Image m_Image;
        public LocalGroupImage m_localGroupImage;
        private static Sprite nullSprite = null;

        private void Awake()
        {
            m_Image = GetComponent<Image>();
        }

        public override void RefreshContent()
        {
            if (m_Image != null && m_localGroupImage != null)
            {
                Sprite sprite = m_localGroupImage.GetContent(key, m_localKey);
                if (sprite != null)
                {
                    m_Image.sprite = sprite;
                }
                else
                {
                    m_Image.sprite = nullSprite;//ResLoader.Load<Sprite>(Paths.IMG_NONE);
                }
            }
        }
    }
}

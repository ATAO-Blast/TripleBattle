using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace TripleBattle
{
    public class ItemBar : MonoBehaviour
    {
        [SerializeField] RectTransform hpBar;
        [SerializeField] RectTransform shieldBar;

        private Transform rootTrans;
        private float scaleRate = 1f * 1080f / Screen.height;
        private RectTransform rect;
        public void InitItemBars(Transform trans,float hp,float shield)
        {
            rect = GetComponent<RectTransform>();
            rootTrans = trans;
            SetHp(hp);
            SetShield(shield);
        }
        public void SetHp(float hp)
        {
            hpBar.localScale = new Vector3(hp, 1, 1);
        }
        public void SetShield(float shield)
        {
            shieldBar.localScale = new Vector3(shield, 1, 1);
        }
        private void Update()
        {
            Vector3 screenPos = Camera.main.WorldToScreenPoint(rootTrans.position);
            rect.anchoredPosition = screenPos * scaleRate;
        }
    }
}
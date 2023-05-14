using System.Collections.Generic;
using UnityEngine;

namespace TripleBattle
{
    public class DynamicWnd : WindowRoot
    {
        public Transform hpItemRoot;
        public Transform blockItemRoot;

        private Dictionary<string,ItemBar> itemBarsDic = new Dictionary<string,ItemBar>();
        protected override void InitWnd() 
        {
            base.InitWnd();
        }
        public void AddHpItemInfo(string key,Transform trans,float hp,float shield)
        {
            ItemBar itembar;
            if(itemBarsDic.TryGetValue(key, out itembar))
            {
                return;
            }
            else
            {
                var itemBarClone = resSvc.LoadPrefab(PathDefine.itemBars,new Vector3(-1000,0,0),Quaternion.identity,hpItemRoot,true);
                ItemBar itb = itemBarClone.GetComponent<ItemBar>();
                itb.InitItemBars(trans,hp,shield);
                itemBarsDic.Add(key, itb);
            }
        }
        public void SetBarInfo(string key,float hp,float shield)
        {
            ItemBar itemBar;
            if (itemBarsDic.TryGetValue(key, out itemBar))
            {
                itemBar.SetHp(hp);
                itemBar.SetShield(shield);
            }
        }
        public void ClearBars()
        {
            itemBarsDic.Clear();
            foreach (Transform item in hpItemRoot)
            {
                Destroy(item.gameObject);
            }
        }
    }
}
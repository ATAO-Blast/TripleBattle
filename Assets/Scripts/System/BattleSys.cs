using System.Collections.Generic;
using UnityEngine;

namespace TripleBattle
{
    public class BattleSys : SystemRoot<BattleSys>
    {
        public BattleWnd battleWnd;
        private BattleMgr battleMgr;
        public override void InitSys()
        {
            base.InitSys();
            instance = this;
        }
        public void StartBattle(string mapCfgPath)
        {
            var rootTrans = GameRoot.Instance.transform;
            if (rootTrans.Find("BattleRoot"))
            {
                GameObject go = rootTrans.Find("BattleRoot").gameObject;
                battleMgr = go.GetComponent<BattleMgr>();
            }
            else
            {
                GameObject go = new GameObject()
                {
                    name = "BattleRoot"
                };
                go.transform.SetParent(rootTrans);
                battleMgr = go.AddComponent<BattleMgr>();

            }
            battleMgr.InitMgr(mapCfgPath);

        }
        public void SetBattleWndState(bool isActive = true)
        {
            battleWnd.SetWndState(isActive);
        }
        public void AddBlockIcon(Transform trans,BlockType blockType)
        {
            battleWnd.AddBlockItemIcon(trans, blockType);
        }
        /// <summary>
        /// 移除掉角色头上的按键提示UI
        /// </summary>
        public void RemoveBlockIcon()
        {
            battleWnd.RemoveBlockIcons();
        }
        public void RefreshBlockItems(List<BlockType> blocks)
        {
            battleWnd.RefreshBlockItems(blocks);
        }
        /// <summary>
        /// 按照序号移除Block条上的Block
        /// </summary>
        /// <param name="index">移除序号</param>
        public void RemoveBlockItems(int index)
        {
            battleWnd.RemoveBlockItems(index);
        }
        public void ClickBlockRuletteBtn(BlockType blockType)
        {
            battleMgr.RemoveCurBlock(blockType);
        }
    }
}
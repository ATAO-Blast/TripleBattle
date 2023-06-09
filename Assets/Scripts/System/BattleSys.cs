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
        public void HideBattleWnd()
        {
            battleWnd.HideUI();
        }
        public void ShowBattleWnd()
        {
            battleWnd.ShowUI();
        }
        public void AddBlockIcon(Transform trans,BlockType blockType)
        {
            battleWnd.AddBlockItemIcon(trans, blockType);
        }
        /// <summary>
        /// �Ƴ�����ɫͷ�ϵİ�����ʾUI
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
        /// ��������Ƴ�Block���ϵ�Block
        /// </summary>
        /// <param name="index">�Ƴ����</param>
        public void RemoveBlockItems(int index)
        {
            battleWnd.RemoveBlockItems(index);
        }
        public void ClickBlockRuletteBtn(BlockType blockType)
        {
            battleMgr.RemoveCurBlock(blockType);
        }
        public void ResetActionListImages()
        {
            battleWnd.ResetActLstImg();
        }
        public void SetAcionListCharImgs(int curActionIndex, int curImgIndex, Sprite portrait)
        {
            battleWnd.SetActLstCharImgs(curActionIndex, curImgIndex, portrait);
        }
        public void SetAcionListEnemyImgs(int curEnemyActID, int curImgIndex, Sprite portrait)
        {
            battleWnd.SetActLstEnemyImgs(curEnemyActID, curImgIndex, portrait);
        }
        public void SetBattleRoundNum(int roundNum)
        {
            battleWnd.SetBattleRound(roundNum);
        }
        public void StartBattle()
        {
            battleMgr.StartBattle();
        }
    }
}
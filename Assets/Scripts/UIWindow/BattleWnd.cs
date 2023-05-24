using TMPro;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.Pool;
using System.Collections.Generic;
using DG.Tweening;
using Unity.VisualScripting;

namespace TripleBattle
{
    public class BattleWnd : WindowRoot
    {
        public TextMeshProUGUI txtRound;
        public Image cancelArea;
        public Transform charInfo;
        public Transform blockItemRoot;
        [Header("ActionList")]
        public Transform list1;
        public Transform list2;
        public Transform list3;
        public Transform list4;
        [Header("BlockItems")]
        public Transform blockItems;
        [Header("BtnXYBA")]
        public Button btnY;
        public Button btnB;
        public Button btnX;
        public Button btnA;
        [Header("CharInfo")]
        public Transform infoCharX;
        public Transform infoCharA;
        public Transform infoCharB;
        public Transform infoCharY;
        [Header("Animaions")]
        public Animation battleAction;
        public Animation blockSelect;
        public Animation charInfoList;

        private Sprite blockA, blockB, blockX, blockY;
        private CanvasGroup[] bsCgs;
        private Image[] bsImgs;
        private Image[] actionList1CharImgs;
        private Image[] actionList1EnemyImgs;
        private Image[] actionList2CharImgs;
        private Image[] actionList2EnemyImgs;
        private Image[] actionList3CharImgs;
        private Image[] actionList3EnemyImgs;
        private Image[] actionList4CharImgs;
        private Image[] actionList4EnemyImgs;

        
        protected override void InitWnd()
        {
            base.InitWnd();
            InitRefs();
            ResetActLstImg();
            RegTouchEvts();
            ShowUI();
        }
        private void InitRefs()
        {
            bsCgs = new CanvasGroup[9];
            bsImgs = new Image[9];
            for (int i = 0; i < blockItems.childCount; i++)//blockItems的子对象是
            {
                bsCgs[i] = blockItems.GetChild(i).GetComponent<CanvasGroup>();
                bsImgs[i] = blockItems.GetChild(i).Find("Image").GetComponent<Image>();
            }
            #region GetActionListImages
            actionList1CharImgs = new Image[4];
            actionList1EnemyImgs = new Image[4];
            actionList2CharImgs = new Image[4];
            actionList2EnemyImgs = new Image[4];
            actionList3CharImgs = new Image[4];
            actionList3EnemyImgs = new Image[4];
            actionList4CharImgs = new Image[4];
            actionList4EnemyImgs = new Image[4];

            actionList1CharImgs[0] = list1.transform.Find("Character/Bg/imgAtker").GetComponent<Image>();
            actionList1CharImgs[1] = list1.transform.Find("Character/Bg2/imgFoller").GetComponent<Image>();
            actionList1CharImgs[2] = list1.transform.Find("Character/Bg3/imgFoller").GetComponent<Image>();
            actionList1CharImgs[3] = list1.transform.Find("Character/Bg4/imgFoller").GetComponent<Image>();
            actionList1EnemyImgs[0] = list1.transform.Find("Enemy/Bg/imgAtker").GetComponent<Image>();
            actionList1EnemyImgs[1] = list1.transform.Find("Enemy/Bg2/imgFoller").GetComponent<Image>();
            actionList1EnemyImgs[2] = list1.transform.Find("Enemy/Bg3/imgFoller").GetComponent<Image>();
            actionList1EnemyImgs[3] = list1.transform.Find("Enemy/Bg4/imgFoller").GetComponent<Image>();

            actionList2CharImgs[0] = list2.transform.Find("Character/Bg/imgAtker").GetComponent<Image>();
            actionList2CharImgs[1] = list2.transform.Find("Character/Bg2/imgFoller").GetComponent<Image>();
            actionList2CharImgs[2] = list2.transform.Find("Character/Bg3/imgFoller").GetComponent<Image>();
            actionList2CharImgs[3] = list2.transform.Find("Character/Bg4/imgFoller").GetComponent<Image>();
            actionList2EnemyImgs[0] = list2.transform.Find("Enemy/Bg/imgAtker").GetComponent<Image>();
            actionList2EnemyImgs[1] = list2.transform.Find("Enemy/Bg2/imgFoller").GetComponent<Image>();
            actionList2EnemyImgs[2] = list2.transform.Find("Enemy/Bg3/imgFoller").GetComponent<Image>();
            actionList2EnemyImgs[3] = list2.transform.Find("Enemy/Bg4/imgFoller").GetComponent<Image>();

            actionList3CharImgs[0] = list3.transform.Find("Character/Bg/imgAtker").GetComponent<Image>();
            actionList3CharImgs[1] = list3.transform.Find("Character/Bg2/imgFoller").GetComponent<Image>();
            actionList3CharImgs[2] = list3.transform.Find("Character/Bg3/imgFoller").GetComponent<Image>();
            actionList3CharImgs[3] = list3.transform.Find("Character/Bg4/imgFoller").GetComponent<Image>();
            actionList3EnemyImgs[0] = list3.transform.Find("Enemy/Bg/imgAtker").GetComponent<Image>();
            actionList3EnemyImgs[1] = list3.transform.Find("Enemy/Bg2/imgFoller").GetComponent<Image>();
            actionList3EnemyImgs[2] = list3.transform.Find("Enemy/Bg3/imgFoller").GetComponent<Image>();
            actionList3EnemyImgs[3] = list3.transform.Find("Enemy/Bg4/imgFoller").GetComponent<Image>();

            actionList4CharImgs[0] = list4.transform.Find("Character/Bg/imgAtker").GetComponent<Image>();
            actionList4CharImgs[1] = list4.transform.Find("Character/Bg2/imgFoller").GetComponent<Image>();
            actionList4CharImgs[2] = list4.transform.Find("Character/Bg3/imgFoller").GetComponent<Image>();
            actionList4CharImgs[3] = list4.transform.Find("Character/Bg4/imgFoller").GetComponent<Image>();
            actionList4EnemyImgs[0] = list4.transform.Find("Enemy/Bg/imgAtker").GetComponent<Image>();
            actionList4EnemyImgs[1] = list4.transform.Find("Enemy/Bg2/imgFoller").GetComponent<Image>();
            actionList4EnemyImgs[2] = list4.transform.Find("Enemy/Bg3/imgFoller").GetComponent<Image>();
            actionList4EnemyImgs[3] = list4.transform.Find("Enemy/Bg4/imgFoller").GetComponent<Image>();
            #endregion
            blockA = resSvc.LoadSprite(PathDefine.blockA, true);
            blockB = resSvc.LoadSprite(PathDefine.blockB, true);
            blockX = resSvc.LoadSprite(PathDefine.blockX, true);
            blockY = resSvc.LoadSprite(PathDefine.blockY, true);
        }
        public void ResetActLstImg()
        {
            for (int i = 0; i < actionList1CharImgs.Length; i++)
            {
                if(i == 0) { actionList1CharImgs[i].gameObject.SetActive(false); }
                else { actionList1CharImgs[i].transform.parent.gameObject.SetActive(false);}
            }
            for (int i = 0; i < actionList2CharImgs.Length; i++)
            {
                if (i == 0) { actionList2CharImgs[i].gameObject.SetActive(false); }
                else { actionList2CharImgs[i].transform.parent.gameObject.SetActive(false); }
            }
            for (int i = 0; i < actionList3CharImgs.Length; i++)
            {
                if (i == 0) { actionList3CharImgs[i].gameObject.SetActive(false); }
                else { actionList3CharImgs[i].transform.parent.gameObject.SetActive(false); }
            }
            for (int i = 0; i < actionList4CharImgs.Length; i++)
            {
                if (i == 0) { actionList4CharImgs[i].gameObject.SetActive(false); }
                else { actionList4CharImgs[i].transform.parent.gameObject.SetActive(false); }
            }
            for (int i = 0; i < actionList1EnemyImgs.Length; i++)
            {
                if (i == 0) { actionList1EnemyImgs[i].gameObject.SetActive(false); }
                else { actionList1EnemyImgs[i].transform.parent.gameObject.SetActive(false); }
            }
            for (int i = 0; i < actionList2EnemyImgs.Length; i++)
            {
                if (i == 0) { actionList2EnemyImgs[i].gameObject.SetActive(false); }
                else { actionList2EnemyImgs[i].transform.parent.gameObject.SetActive(false); }
            }
            for (int i = 0; i < actionList3EnemyImgs.Length; i++)
            {
                if (i == 0) { actionList3EnemyImgs[i].gameObject.SetActive(false); }
                else { actionList3EnemyImgs[i].transform.parent.gameObject.SetActive(false); }
            }
            for (int i = 0; i < actionList4EnemyImgs.Length; i++)
            {
                if (i == 0) { actionList4EnemyImgs[i].gameObject.SetActive(false); }
                else { actionList4EnemyImgs[i].transform.parent.gameObject.SetActive(false); }
            }
        }
        private void RegTouchEvts()
        {
            OnPointerDown(cancelArea.gameObject, evt =>
            {
                charInfo.gameObject.SetActive(false);
                cancelArea.gameObject.SetActive(false);
            });
        }
        private void AddBlockIcons()
        {

        }
        public void SetBattleRound(int roundNum)
        {
            txtRound.text = roundNum.ToString();
        }
        public void ShowUI()
        {
            var batActShow = battleAction.GetClip("BattleActionShow");
            if (batActShow != null)
            {
                battleAction.Play("BattleActionShow");
                battleAction.gameObject.SetActive(true);
            }
            var blockSeleShow = blockSelect.GetClip("BlockSelecterShow");
            if (blockSeleShow != null)
            {
                blockSelect.Play("BlockSelecterShow");
                blockSelect.gameObject.SetActive(true);
            }
            var charInfoListShow = charInfoList.GetClip("CharInfoListShow");
            if (charInfoListShow != null)
            {
                charInfoList.Play("CharInfoListShow");
                charInfoList.gameObject.SetActive(true);
            }
            blockItemRoot.gameObject.SetActive(true);
        }
        public void HideUI()
        {
            var batActShow = battleAction.GetClip("BattleActionHide");
            if (batActShow != null)
            {
                battleAction.Play("BattleActionHide");
                battleAction.gameObject.SetActive(false);
            }
            var blockSeleShow = blockSelect.GetClip("BlockSelecterHide");
            if (blockSeleShow != null)
            {
                blockSelect.Play("BlockSelecterHide"); 
                blockSelect.gameObject.SetActive(false);
            }
            var charInfoListShow = charInfoList.GetClip("CharInfoListHide");
            if (charInfoListShow != null)
            {
                charInfoList.Play("CharInfoListHide"); 
                charInfoList.gameObject.SetActive(false);
            }
            blockItemRoot.gameObject.SetActive(false);
        }
        public void RefreshBlockItems(List<BlockType> blocks)
        {
            for (int i = 0; i < blocks.Count; i++)
            {
                bsCgs[i].alpha = 1;
                bsImgs[i].rectTransform.anchoredPosition = Vector2.zero;
                switch (blocks[i])
                {
                    case BlockType.A:
                        bsImgs[i].sprite = blockA;
                        break;
                    case BlockType.B:
                        bsImgs[i].sprite = blockB;
                        break;
                    case BlockType.X:
                        bsImgs[i].sprite = blockX;
                        break;
                    case BlockType.Y:
                        bsImgs[i].sprite = blockY;
                        break;
                }
            }
        }
        public void RemoveBlockItems(int index)
        {
            bsImgs[index].rectTransform.DOAnchorPosY(45f, 0.2f);
            bsCgs[index].DOFade(0f, 0.2f);
        }
        private float scaleRate = 1f * 1080f / Screen.height;
        private void SetBlockIconPos(GameObject block,Transform trans)
        {
            var rect = block.GetComponent<RectTransform>();
            var screenPos = Camera.main.WorldToScreenPoint(trans.position);
            rect.anchoredPosition = screenPos * scaleRate;
        }
        public void AddBlockItemIcon(Transform trans,BlockType blockType)
        {
            switch (blockType)
            {
                case BlockType.A:
                    {
                        GameObject blockItemClone = resSvc.LoadPrefab(PathDefine.Block_A, new Vector3(-1000, 0, 0), Quaternion.identity, blockItemRoot, true);
                        SetBlockIconPos(blockItemClone, trans);
                    }
                    break;
                case BlockType.B:
                    {
                        GameObject blockItemClone = resSvc.LoadPrefab(PathDefine.Block_B, new Vector3(-1000, 0, 0), Quaternion.identity, blockItemRoot, true);
                        SetBlockIconPos(blockItemClone, trans);
                    }
                    break;
                case BlockType.X:
                    {
                        GameObject blockItemClone = resSvc.LoadPrefab(PathDefine.Block_X, new Vector3(-1000, 0, 0), Quaternion.identity, blockItemRoot, true);
                        SetBlockIconPos(blockItemClone, trans);
                    }
                    break;
                case BlockType.Y:
                    {
                        GameObject blockItemClone = resSvc.LoadPrefab(PathDefine.Block_Y, new Vector3(-1000, 0, 0), Quaternion.identity, blockItemRoot, true);
                        SetBlockIconPos(blockItemClone, trans);
                    }
                    break;
            }
        }
        public void RemoveBlockIcons()
        {
            foreach (Transform item in blockItemRoot)
            {
                Destroy(item.gameObject);
            }
        }

        public void SetActLstCharImgs(int curActionIndex,int curImgIndex, Sprite portrait)
        {
            switch (curActionIndex)
            {
                case 1:
                    if(curImgIndex == 0)
                    {
                        actionList1CharImgs[curImgIndex].gameObject.SetActive(true);
                        actionList1CharImgs[curImgIndex].sprite = portrait;
                    }
                    else
                    {
                        actionList1CharImgs[curImgIndex].transform.parent.gameObject.SetActive(true);
                        actionList1CharImgs[curImgIndex].sprite = portrait;
                    }
                    break;
                case 2:
                    if (curImgIndex == 0)
                    {
                        actionList2CharImgs[curImgIndex].gameObject.SetActive(true);
                        actionList2CharImgs[curImgIndex].sprite = portrait;
                    }
                    else
                    {
                        actionList2CharImgs[curImgIndex].transform.parent.gameObject.SetActive(true);
                        actionList2CharImgs[curImgIndex].sprite = portrait;
                    }
                    break;
                case 3:
                    if (curImgIndex == 0)
                    {
                        actionList3CharImgs[curImgIndex].gameObject.SetActive(true);
                        actionList3CharImgs[curImgIndex].sprite = portrait;
                    }
                    else
                    {
                        actionList3CharImgs[curImgIndex].transform.parent.gameObject.SetActive(true);
                        actionList3CharImgs[curImgIndex].sprite = portrait;
                    }
                    break;
                case 4:
                    if (curImgIndex == 0)
                    {
                        actionList4CharImgs[curImgIndex].gameObject.SetActive(true);
                        actionList4CharImgs[curImgIndex].sprite = portrait;
                    }
                    else
                    {
                        actionList4CharImgs[curImgIndex].transform.parent.gameObject.SetActive(true);
                        actionList4CharImgs[curImgIndex].sprite = portrait;
                    }
                    break;
            }
        }
        public void SetActLstEnemyImgs(int curEnemyActID,int curImgIndex,Sprite portrait)
        {
            switch (curEnemyActID)
            {
                case 1:
                    if (curImgIndex == 0)
                    {
                        actionList1EnemyImgs[curImgIndex].gameObject.SetActive(true);
                        actionList1EnemyImgs[curImgIndex].sprite = portrait;
                    }
                    else
                    {
                        actionList1EnemyImgs[curImgIndex].transform.parent.gameObject.SetActive(true);
                        actionList1EnemyImgs[curImgIndex].sprite = portrait;
                    }
                    break;
                case 2:
                    if (curImgIndex == 0)
                    {
                        actionList2EnemyImgs[curImgIndex].gameObject.SetActive(true);
                        actionList2EnemyImgs[curImgIndex].sprite = portrait;
                    }
                    else
                    {
                        actionList2EnemyImgs[curImgIndex].transform.parent.gameObject.SetActive(true);
                        actionList2EnemyImgs[curImgIndex].sprite = portrait;
                    }
                    break;
                case 3:
                    if (curImgIndex == 0)
                    {
                        actionList3EnemyImgs[curImgIndex].gameObject.SetActive(true);
                        actionList3EnemyImgs[curImgIndex].sprite = portrait;
                    }
                    else
                    {
                        actionList3EnemyImgs[curImgIndex].transform.parent.gameObject.SetActive(true);
                        actionList3EnemyImgs[curImgIndex].sprite = portrait;
                    }
                    break;
                case 4:
                    if (curImgIndex == 0)
                    {
                        actionList4EnemyImgs[curImgIndex].gameObject.SetActive(true);
                        actionList4EnemyImgs[curImgIndex].sprite = portrait;
                    }
                    else
                    {
                        actionList4EnemyImgs[curImgIndex].transform.parent.gameObject.SetActive(true);
                        actionList4EnemyImgs[curImgIndex].sprite = portrait;
                    }
                    break;
            }
        }

        public void PressBtnA()
        {
            BattleSys.Instance.ClickBlockRuletteBtn(BlockType.A);
        }
        public void PressBtnB()
        {
            BattleSys.Instance.ClickBlockRuletteBtn(BlockType.B);
        }
        public void PressBtnX()
        {
            BattleSys.Instance.ClickBlockRuletteBtn(BlockType.X);
        }
        public void PressBtnY()
        {
            BattleSys.Instance.ClickBlockRuletteBtn(BlockType.Y);
        }
        public void StartBattle()
        {
            BattleSys.Instance.StartBattle();
        }
    }
}
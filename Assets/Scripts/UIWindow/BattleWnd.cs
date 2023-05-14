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
        protected override void InitWnd()
        {
            base.InitWnd();
            InitRefs();
            RegTouchEvts();
            ShowUI();
        }
        private void InitRefs()
        {
            bsCgs = new CanvasGroup[9];
            bsImgs = new Image[9];
            for (int i = 0; i < blockItems.childCount; i++)
            {
                bsCgs[i] = blockItems.GetChild(i).GetComponent<CanvasGroup>();
                bsImgs[i] = blockItems.GetChild(i).Find("Image").GetComponent<Image>();
            }

            blockA = resSvc.LoadSprite(PathDefine.blockA, true);
            blockB = resSvc.LoadSprite(PathDefine.blockB, true);
            blockX = resSvc.LoadSprite(PathDefine.blockX, true);
            blockY = resSvc.LoadSprite(PathDefine.blockY, true);
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
    }
}
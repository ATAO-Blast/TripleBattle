using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.EventSystems;
using DarknessWarGodLearning;
using UnityEngine.Events;

namespace TripleBattle
{
    public class WindowRoot : MonoBehaviour
    {
        protected ResSvc resSvc = null;
        protected AudioSvc audioSvc = null;
        protected TimerSvc timerSvc = null;
        protected Stack<WindowRoot> openWindowStack = new Stack<WindowRoot>();

        private void Update()
        {
            if (Input.GetButtonDown("Cancel"))
            {
                if(openWindowStack.Count > 0)
                {
                    var curWnd = openWindowStack.Pop();
                    curWnd.SetWndState(false);
                }
            }
        }

        public void SetWndState(bool isActive = true)
        {
            if(gameObject.activeSelf != isActive) { SetActive(gameObject, isActive); }
            if(isActive) { InitWnd(); }
            else { ClearWnd(); }

        }
        protected virtual void InitWnd()
        {
            resSvc = ResSvc.Instance;
            audioSvc = AudioSvc.Instance;
            timerSvc = TimerSvc.Instance;
        }
        protected virtual void ClearWnd()
        {
            resSvc = null;
            audioSvc = null;
            timerSvc = null;
        }
        #region SetGoActiveState
        protected void SetActive(GameObject go, bool state = true)
        {
            var canvasGroup = go.GetComponent<CanvasGroup>();
            if (canvasGroup)
            {
                if (state)
                {
                    canvasGroup.DOFade(1, 0.2f).OnStart(() =>
                    {
                        go.SetActive(state);
                    });
                }
                else
                {
                    canvasGroup.DOFade(0, 0.5f).OnComplete(() =>
                    {
                        go.SetActive(state);
                    });

                }
            }
            else
            {
                go.SetActive(state);
            }
        }
        protected void SetActive(Transform trans, bool state = true)
        {
            trans.gameObject.SetActive(state);
        }
        protected void SetActive(RectTransform rectTrans, bool state = true)
        {
            rectTrans.gameObject.SetActive(state);
        }
        protected void SetActive(Image img, bool state = true)
        {
            img.gameObject.SetActive(state);
        }
        protected void SetActive(TextMeshProUGUI textMeshProUGUI, bool state = true)
        {
            textMeshProUGUI.gameObject.SetActive(state);
        }
        #endregion
        #region SetTMPText
        protected void SetText(TextMeshProUGUI textMeshProUGUI, string content = "")
        {
            textMeshProUGUI.text = content;
        }
        protected void SetText(TextMeshProUGUI textMeshProUGUI, int num = 0)
        {
            SetText(textMeshProUGUI, num.ToString());
        }
        protected void SetText(Transform trans, string content = "")
        {
            SetText(trans.GetComponent<TextMeshProUGUI>(), content);
        }
        protected void SetText(Transform trans, int num = 0)
        {
            SetText(trans.GetComponent<TextMeshProUGUI>(), num);
        }
        #endregion
        #region SetSprite
        protected void SetSprite(Image img, string path)
        {
            Sprite sprite = resSvc.LoadSprite(path, true);
            img.sprite = sprite;
        }
        #endregion
        #region GetTrans
        protected Transform GetTrans(string name, Transform trans = null)
        {
            if (trans != null)
            {
                return trans.Find(name);
            }
            else
            {
                return transform.Find(name);
            }
        }
        protected Transform GetTrans(string name, GameObject go = null)
        {
            if (go != null)
            {
                return go.transform.Find(name);
            }
            else
            {
                return transform.Find(name);
            }
        }
        #endregion
        protected T GetOrAddComponent<T>(GameObject go) where T : Component
        {
            T t = go.GetComponent<T>();
            if (t == null)
            {
                t = go.AddComponent<T>();
            }
            return t;
        }
        #region Click Evts
        /// <summary>
        /// 给Image等组件添加OnPointerClick回调，注意回调的参数也是需要Listener指定的
        /// </summary>
        protected void OnPointerClick(GameObject go, UnityAction<object> evt, object args)
        {
            PEListener listener = GetOrAddComponent<PEListener>(go);
            listener.onPointerClick = evt;
            listener.args = args;
        }
        protected void OnPointerDown(GameObject go, UnityAction<PointerEventData> evt)
        {
            PEListener listener = GetOrAddComponent<PEListener>(go);
            listener.onPointerDown = evt;
        }
        protected void OnPointerUp(GameObject go, UnityAction<PointerEventData> evt)
        {
            PEListener listener = GetOrAddComponent<PEListener>(go);
            listener.onPointerUp = evt;
        }
        protected void OnPointerDrag(GameObject go, UnityAction<PointerEventData> evt)
        {
            PEListener listener = GetOrAddComponent<PEListener>(go);
            listener.onPointerDrag = evt;
        }
        #endregion
    }
}
using UnityEngine.UI;

namespace TripleBattle
{
    public class OptionWnd : WindowRoot
    {
        public Slider bgVolSlider, uiVolSlider;
        public Button btnOK, btnCancel;
        public Toggle fullScr, vSync;
        private float bgVolume,uiVolume;
        private float preBgVolume, preUiVolume;
        private bool preIsFullScr,preIsVSync;
        private bool isFullScr,isVSync;
        protected override void InitWnd()
        {
            base.InitWnd();
            audioSvc.PlayUIMusic(PathDefine.UIOpenPage);
            preBgVolume = audioSvc.GetBGVolume();
            bgVolSlider.value = preBgVolume;
            preUiVolume = audioSvc.GetUIVolume();
            uiVolSlider.value = preUiVolume;
            preIsFullScr = GameRoot.Instance.GetFullScreen();
            fullScr.isOn = preIsFullScr;
            preIsVSync = GameRoot.Instance.GetVSync();
            vSync.isOn = preIsVSync;
            openWindowStack.Push(this);
        }
        protected override void ClearWnd()
        {
            audioSvc.PlayUIMusic(PathDefine.UIClickBtn);
            audioSvc.SetBGVolume(preBgVolume);
            audioSvc.SetUIVolume(preUiVolume);
            GameRoot.Instance.SetFullScreen(preIsFullScr);
            GameRoot.Instance.SetVSync(preIsVSync);
            base.ClearWnd();
        }
        public void SetBGVolume(float volume)
        {
            audioSvc.SetBGVolume(volume);
            bgVolume = volume;
        }
        public void SetUIVolume(float volume)
        {
            audioSvc.SetUIVolume(volume);
            uiVolume = volume;
        }
        public void SetFullScr(bool value)
        {
            isFullScr = value;
            GameRoot.Instance.SetFullScreen(isFullScr);
        }
        public void SetVSync(bool value)
        {
            isVSync = value;
            GameRoot.Instance.SetVSync(isVSync);
        }
        public void BtnConfirm()
        {
            audioSvc.PlayUIMusic(PathDefine.UIClickBtn);
            GameRoot.Instance.SetAudioCfg(bgVolume, uiVolume);
            SetWndState(false);
        }
        public void BtnCancel()
        {
            audioSvc.PlayUIMusic(PathDefine.UIClickBtn);
            audioSvc.SetBGVolume(preBgVolume);
            audioSvc.SetUIVolume(preUiVolume);
            GameRoot.Instance.SetFullScreen(preIsFullScr);
            GameRoot.Instance.SetVSync(preIsVSync);
            SetWndState(false);
        }
    }
}
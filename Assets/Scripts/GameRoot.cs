using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace TripleBattle
{
    public class GameRoot : MonoBehaviour
    {
        private static GameRoot instance;
        public static GameRoot Instance
        {
            get { return instance; }
        }
        [SerializeField] private LoadingWnd loadingWnd;
        [SerializeField] private OptionWnd optionWnd;
        [SerializeField] private DynamicWnd dynamicWnd;
        private GameCfg gameCfg;
        private List<CharacterData> charList = new List<CharacterData>(4);

        private void Start()
        {
            instance = this;
            InitSvc();
            InitCfg();
            InitSys();
            DontDestroyOnLoad(gameObject);
            MainMenuSys.Instance.EnterMainMenu();
            dynamicWnd.SetWndState(true);
        }
        #region cfg相关
        private void InitCfg()
        {
            gameCfg = SaveSvc.Instance.GetCfg();
            if (gameCfg != null)
            {
                AudioSvc.Instance.SetBGVolume(gameCfg.BGVolume);
                AudioSvc.Instance.SetUIVolume(gameCfg.UIVolume);
                Screen.fullScreen = gameCfg.isFullScreen;
                if(gameCfg.isVSync)
                {
                    QualitySettings.vSyncCount = 1;
                }
                else
                {
                    QualitySettings.vSyncCount = 0;
                }

            }
            else
            {
                gameCfg = new GameCfg
                {
                    BGVolume = 1f,
                    UIVolume = 1f,
                    isFullScreen = true,
                    isVSync = true,
                };
                SaveSvc.Instance.SetCfg(gameCfg);
            }
        }
        public void SetAudioCfg(float bgVolume,float uiVolume)
        {
            gameCfg.BGVolume = bgVolume;
            gameCfg.UIVolume = uiVolume;
            SaveSvc.Instance.SetCfg(gameCfg);
        }
        public void SetFullScreen(bool isFullScr)
        {
            gameCfg.isFullScreen= isFullScr;
            Screen.fullScreen= isFullScr;
        }
        public bool GetFullScreen()
        {
            return gameCfg.isFullScreen;
        }
        public void SetVSync(bool isVSync)
        {
            gameCfg.isVSync= isVSync;
            if (gameCfg.isVSync)
            {
                QualitySettings.vSyncCount = 1;
            }
            else
            {
                QualitySettings.vSyncCount = 0;
            }
        }
        public bool GetVSync()
        {
            return gameCfg.isVSync;
        }
        #endregion
        private void InitSvc()
        {
            SaveSvc saveSvc = GetComponent<SaveSvc>();
            saveSvc.InitSvc();
            ResSvc resSvc = GetComponent<ResSvc>();
            resSvc.InitSvc();
            TimerSvc timerSvc = GetComponent<TimerSvc>();
            timerSvc.InitSvc();

            AudioSvc audioSvc = GetComponent<AudioSvc>();
            audioSvc.InitSvc();
        }
        private void InitSys()
        {
            MainMenuSys mainMenuSys = GetComponent<MainMenuSys>();
            mainMenuSys.InitSys();
            BattleSys battleSys = GetComponent<BattleSys>();
            battleSys.InitSys();
            
        }
        public void SetLoadingWnd(bool isActive)
        {
            loadingWnd.SetWndState(isActive);
        }
        public void ClearCharData()
        {
            charList.Clear();
        }
        public void AddCharData(CharacterData data)
        {
            charList.Add(data);
        }
        public List<CharacterData> GetCharList()
        {
            return charList;
        }
        public void SaveCharData()
        {
            SaveSvc.Instance.SetSaveData(new SaveData { characterDatas = charList});
        }
        public void ReadCharData()
        {
            charList = SaveSvc.Instance.GetSaveData().characterDatas;
        }
        public bool IsCharDataExist()
        {
            var saveData = SaveSvc.Instance.GetSaveData();
            if (saveData == null) return false;
            return true;
        }
        public void OpenOptionWnd()
        {
            optionWnd.SetWndState();
        }
        private void OnApplicationQuit()
        {
            SaveSvc.Instance.SetCfg(gameCfg);
        }
        #region dynamicWnd相关
        public void AddHpItemInfo(string key, Transform trans, float hp, float shield)
        {
            dynamicWnd.AddHpItemInfo(key, trans, hp, shield);
        }
        #endregion
#if UNITY_EDITOR
        private void OnGUI()
        {
            if (GUILayout.Button("Open"))
            {
                optionWnd.SetWndState();
            }
            if (GUILayout.Button("Close"))
            {
                SetLoadingWnd(false);
            }
            if(GUILayout.Button("Stop"))
            {
                Time.timeScale = 0f;
            }
            if (GUILayout.Button("Continue"))
            {
                Time.timeScale = 1f;
            }
            
        }
#endif
    }
}
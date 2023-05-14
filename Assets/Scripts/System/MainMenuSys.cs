namespace TripleBattle
{
    public class MainMenuSys : SystemRoot<MainMenuSys>
    {
        public StartWnd startWnd;
        public override void InitSys()
        {
            base.InitSys();
            instance = this;
        }
        public void EnterMainMenu()
        {
            AudioSvc.Instance.PlayBGMusic(PathDefine.MainMenuBG);
            startWnd.SetWndState();
            if(CanContinueGame())
            {
                startWnd.SetBtnContinue(true);
            }
            else
            {
                startWnd.SetBtnContinue(false);
            }
        }
        public void StartNewGame()
        {
            GameRoot.Instance.ClearCharData();
            var char0 = resSvc.LoadCharCfgSO(PathDefine.PathChar0).characterData;
            GameRoot.Instance.AddCharData(char0);
            var char1 = resSvc.LoadCharCfgSO(PathDefine.PathChar1).characterData;
            GameRoot.Instance.AddCharData(char1);
            var char2 = resSvc.LoadCharCfgSO(PathDefine.PathChar2).characterData;
            GameRoot.Instance.AddCharData(char2);
            var char3 = resSvc.LoadCharCfgSO(PathDefine.PathChar3).characterData;
            GameRoot.Instance.AddCharData(char3);

            BattleSys.instance.StartBattle("ResCfgs/Battle_1");//需要在关卡选择界面使用，这里用作测试

            startWnd.SetWndState(false);
        }
        public void OpenOptionWnd()
        {
            GameRoot.Instance.OpenOptionWnd();
        }
        private bool CanContinueGame()
        {
            return GameRoot.Instance.IsCharDataExist(); 
        }
        public void ContinueGame()
        {
            GameRoot.Instance.ClearCharData();
            GameRoot.Instance.ReadCharData();
            resSvc.AsyncLoadScene(PathDefine.Scene_Battle_1, () =>
            {

            });
            startWnd.SetWndState(false);
        }
    }
}
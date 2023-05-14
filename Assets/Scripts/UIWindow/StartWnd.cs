using UnityEngine;
using UnityEngine.UI;

namespace TripleBattle
{
    public class StartWnd : WindowRoot
    {
        public Button btnContinue, btnQuit, btnOption, btnNewGame;
        protected override void InitWnd()
        {
            base.InitWnd();
        }
        public void BtnQuit()
        {
            Application.Quit();
        }
        public void BtnOption()
        {
            MainMenuSys.Instance.OpenOptionWnd();
        }
        public void BtnNewGame()
        {
            MainMenuSys.Instance.StartNewGame();
        }
        public void SetBtnContinue(bool isActive = false)
        {
            btnContinue.interactable = isActive;
        }
        public void BtnContinue()
        {

        }
    }
}
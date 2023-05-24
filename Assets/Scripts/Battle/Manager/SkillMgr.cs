using UnityEngine;

namespace TripleBattle
{
    public class SkillMgr : MonoBehaviour
    {
        private ResSvc resSvc;
        private TimerSvc timerSvc;
        public void InitMgr()
        {
            resSvc = ResSvc.Instance;
            timerSvc = TimerSvc.Instance;
        }
        public void SkillAttack(EntityCharacter character, int skillID)
        {

        }
    }
}
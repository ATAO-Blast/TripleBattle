using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace TripleBattle
{
    public class StateMgr : MonoBehaviour
    {
        private Dictionary<AniState,IState> fsmDic = new Dictionary<AniState,IState>();
        public void InitMgr()
        {
            fsmDic.Add(AniState.Idle, new StateIdle());
            fsmDic.Add(AniState.Attack, new StateAttack());
            fsmDic.Add(AniState.Born, new StateBorn());
            fsmDic.Add(AniState.Die, new StateDie());
            fsmDic.Add(AniState.Hit, new StateHit());
            fsmDic.Add(AniState.Move, new StateMove());

        }
        public void ChangeStates(EntityBase entity, AniState targetState, params object[] args)
        {
            if(entity.currentAniState == targetState) return;
            if (fsmDic.ContainsKey(targetState))
            {
                if(entity.currentAniState != AniState.None)
                {
                    fsmDic[entity.currentAniState].Exit(entity, args);
                }
                fsmDic[targetState].Enter(entity, args);
                fsmDic[targetState].Process(entity, args);
            }
        }
    }
}

using UnityEngine;

namespace TripleBattle
{
    public class StateIdle : IState
    {
        public void Enter(EntityBase entity, params object[] args)
        {
            entity.currentAniState = AniState.Idle;
        }

        public void Exit(EntityBase entity, params object[] args)
        {
            
        }

        public void Process(EntityBase entity, params object[] args)
        {
            
        }
    }
}
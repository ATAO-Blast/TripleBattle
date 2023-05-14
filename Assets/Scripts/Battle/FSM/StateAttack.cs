namespace TripleBattle
{
    public class StateAttack : IState
    {
        public void Enter(EntityBase entity, params object[] args)
        {
            entity.currentAniState = AniState.Attack;
        }

        public void Exit(EntityBase entity, params object[] args)
        {

        }

        public void Process(EntityBase entity, params object[] args)
        {

        }
    }
}
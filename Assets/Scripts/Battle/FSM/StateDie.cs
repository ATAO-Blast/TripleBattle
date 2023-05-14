namespace TripleBattle
{
    public class StateDie : IState
    {
        public void Enter(EntityBase entity, params object[] args)
        {
            entity.currentAniState = AniState.Die;
        }

        public void Exit(EntityBase entity, params object[] args)
        {

        }

        public void Process(EntityBase entity, params object[] args)
        {

        }
    }
}
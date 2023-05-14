namespace TripleBattle
{
    public class StateHit : IState
    {
        public void Enter(EntityBase entity, params object[] args)
        {
            entity.currentAniState = AniState.Hit;
        }

        public void Exit(EntityBase entity, params object[] args)
        {

        }

        public void Process(EntityBase entity, params object[] args)
        {

        }
    }
}
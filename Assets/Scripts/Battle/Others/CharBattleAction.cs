namespace TripleBattle
{
    public class BattleAction
    {
        public EntityCharAction Attacker, Follower1, Follower2, Follower3;
        public EntityEnemyAction enemyAttacker, enemyFollower1, enemyFollower2, enemyFollower3;
    }
    public class EntityCharAction
    {
        public EntityCharacter character;
        public ActionType actionType;
    }
    public class EntityEnemyAction
    {
        public EntityEnemy enemy;
        public ActionType actionType;
    }

    public enum ActionType
    {
        NormalAttack,
        AddAttack,
        AuxAttack
    }
}
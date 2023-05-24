namespace TripleBattle
{
    [System.Serializable]
    public class EnemyData 
    {
        public int id;
        public string name;
        public EnemyType enemyType;
        public int hp;
        public int maxHp;
        public int atk;
        public int def;
        public int shield;
        public int maxShield;
    }
    public enum EnemyType
    {
        Boss,
        Staff
    }
}

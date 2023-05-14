namespace TripleBattle
{
    [System.Serializable]
    public class CharacterData
    {
        public int id;
        public BlockType blockType;
        public string name;
        public int hp;
        public int maxHp;
        public int atk;
        public int def;
        public int shield;
        public int maxShield;
    }
    public enum BlockType
    {
        Y,
        X,
        B,
        A
    }
}
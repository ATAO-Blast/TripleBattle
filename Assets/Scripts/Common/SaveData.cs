using System.Collections.Generic;

namespace TripleBattle
{
    [System.Serializable]
    public class SaveData
    {
        public List<CharacterData> characterDatas;
        public string SceneName;
    }
}
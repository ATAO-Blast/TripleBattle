using System.Collections.Generic;
using UnityEngine;

namespace TripleBattle
{
    public class SaveSvc : MonoBehaviour
    {
        private static SaveSvc instance;
        public static SaveSvc Instance { get { return instance; } }

        public void InitSvc()
        {
            instance = this;
        }
        private void Set<T>(string key, T value)
        {
            var record = JsonUtility.ToJson(value);
            RecordUtil.Set(key, record);
        }
        private T Get<T>(string key)
        {
            var value = default(T);
            var record = RecordUtil.Get(key);
            if (record != null)
            {
                value = JsonUtility.FromJson<T>(record);
            }
            return value;
        }
        public void Delete(string key)
        {
            RecordUtil.Delete(key);
        }
        public void Save()
        {
            RecordUtil.Save();
        }
        public void SetCfg(GameCfg cfg)
        {
            Set<GameCfg>("cfg", cfg);
            Save();
        }
        public GameCfg GetCfg()
        {
            return Get<GameCfg>("cfg");
        }
        public void SetSaveData(SaveData data)
        {
            Set("AutoSave", data);
        }
        public SaveData GetSaveData()
        {
            return Get<SaveData>("AutoSave");
        }
    }
}
using UnityEngine;

namespace TripleBattle
{
    [System.Serializable]
    public class MapCfg
    {
        public CharacterSO[] chars;
        public EnemySO[] enemys;
        public string mapName;
        public string sceneName;
        public Vector3 mainCamPos;
        public Vector3 mainCamRota;
        public float cameraMainSize;
        public float cameraMainCloseSize;
        public Vector3 bossPos;
        public Vector3[] staffPos;
        public Vector3 Ypos, Xpos, Bpos, Apos;
        public Vector3 CharBatPos, EnemyBatPos;
    }
}
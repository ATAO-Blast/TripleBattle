using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace TripleBattle
{
    public class ResSvc : MonoBehaviour
    {
        private static ResSvc instance;
        public static ResSvc Instance { get { return instance; } }
        public void InitSvc()
        {
            instance = this;
        }
        private Action prgCB = null;
        private void Update()
        {
            prgCB?.Invoke();
        }
        public void AsyncLoadScene(string sceneName, Action onSceneLoaded)
        {
            StartCoroutine(LoadScene(sceneName, onSceneLoaded));
            //GameRoot.Instance.SetLoadingWnd(true);
            //AsyncOperation handler = SceneManager.LoadSceneAsync(sceneName);
            //prgCB = () =>
            //{
            //    if (handler.isDone)
            //    {
            //        GameRoot.Instance.SetLoadingWnd(false);
            //        onSceneLoaded?.Invoke();
            //        handler = null;
            //        prgCB = null;
            //    }
            //};
        }
        IEnumerator LoadScene(string sceneName,Action onSceneLoaded)
        {
            yield return null;
            AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneName);
            asyncOperation.allowSceneActivation = false;
            asyncOperation.completed += ap => { GameRoot.Instance.SetLoadingWnd(false); onSceneLoaded?.Invoke(); };
            GameRoot.Instance.SetLoadingWnd(true);
            while (!asyncOperation.isDone)
            {
                if (asyncOperation.progress >= 0.9f)
                {
                    yield return new WaitForSeconds(2f);
                    asyncOperation.allowSceneActivation = true;
                    //GameRoot.Instance.SetLoadingWnd(false);
                    //onSceneLoaded?.Invoke();
                }
                yield return null;
            }
        }
        private Dictionary<string,AudioClip> audioCaches = new Dictionary<string,AudioClip>();
        public AudioClip LoadAudio(string path, bool cache = false)
        {
            AudioClip audioClip;
            if(!audioCaches.TryGetValue(path, out audioClip))
            {
                audioClip = Resources.Load<AudioClip>(path);
                if (cache)
                {
                    audioCaches.Add(path, audioClip);
                }
            }
            return audioClip;
        }
        private Dictionary<string,GameObject> prefabCache = new Dictionary<string,GameObject>();
        public GameObject LoadPrefab(string path,Vector3 position,Quaternion rotation,Transform parent, bool cache = false)
        {
            GameObject prefab;
            if(!prefabCache.TryGetValue(path,out prefab))
            {
                prefab = Resources.Load<GameObject>(path);
                if (cache)
                {
                    prefabCache.Add(path, prefab);
                }
            }
            GameObject go = null;
            if(prefab != null)
            {
                go = Instantiate(prefab,position,rotation,parent);
            }
            return go;
        }
        
        private Dictionary<string, Sprite> spriteDic = new Dictionary<string, Sprite>();
        public Sprite LoadSprite(string path, bool cache = false)
        {
            Sprite sprite;
            if (!spriteDic.TryGetValue(path, out sprite))
            {
                sprite = Resources.Load<Sprite>(path);
                if (cache)
                {
                    spriteDic.Add(path, sprite);
                }
            }
            return sprite;
        }
        #region ∂¡»°≈‰÷√
        private Dictionary<string, MapCfgSO> mapCfgSODic = new Dictionary<string, MapCfgSO>();
        public MapCfgSO LoadMapCfgSO(string path,bool cache = false)
        {
            MapCfgSO mapCfgSO;
            if(!mapCfgSODic.TryGetValue(path,out mapCfgSO))
            {
                mapCfgSO = Resources.Load<MapCfgSO>(path);
                if(cache)
                {
                    mapCfgSODic.Add(path, mapCfgSO);
                }
            }
            return mapCfgSO;
        }

        private Dictionary<string,CharacterSO> charCfgDic = new Dictionary<string, CharacterSO>();
        public CharacterSO LoadCharCfgSO(string path,bool cache = false)
        {
            CharacterSO charCfgSO;
            if (!charCfgDic.TryGetValue(path, out charCfgSO))
            {
                charCfgSO = Resources.Load<CharacterSO>(path);
                if (cache)
                {
                    charCfgDic.Add(path, charCfgSO);
                }
            }
            return charCfgSO;
        }
        #endregion
    }
}
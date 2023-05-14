using UnityEngine;

namespace TripleBattle
{
    public class SystemRoot<T> : MonoBehaviour where T : SystemRoot<T>
    {
        protected static T instance;
        public static T Instance { get { return instance; } }

        protected ResSvc resSvc;
        protected AudioSvc audioSvc;
        protected TimerSvc timerSvc;

        public virtual void InitSys()
        {
            resSvc = ResSvc.Instance;
            audioSvc = AudioSvc.Instance;
            timerSvc = TimerSvc.Instance;
        }
    }
}
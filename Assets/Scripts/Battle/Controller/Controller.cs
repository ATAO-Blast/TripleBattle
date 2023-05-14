using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace TripleBattle
{
    public abstract class Controller : MonoBehaviour
    {
        protected Dictionary<string,GameObject> fxDic = new Dictionary<string, GameObject>();
        protected TimerSvc timerSvc;

        public Animator animator;
        public Transform hpRoot;
        public Transform blockPoint;

        public virtual void Init()
        {
            timerSvc = TimerSvc.Instance;
        }

        public virtual void SetAnimAct(int action)
        {
            animator.SetInteger("Action", action);
        }

        public virtual void SetFX(string name,float close)
        {

        }
    }
}

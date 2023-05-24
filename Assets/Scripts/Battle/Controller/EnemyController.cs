using UnityEngine;

namespace TripleBattle
{
    public class EnemyController : Controller
    {
        [SerializeField] GameObject charFX;
        public override void Init()
        {
            base.Init();
            if (charFX != null)
            {
                fxDic.Add(charFX.name, charFX);
            }
        }
        public override void SetFX(string name, float close)
        {
            GameObject go;
            if (fxDic.TryGetValue(name, out go))
            {
                go.SetActive(true);
                timerSvc.AddTimeTask(tid =>
                {
                    go.SetActive(false);
                }, close);
            }
        }
    }
}
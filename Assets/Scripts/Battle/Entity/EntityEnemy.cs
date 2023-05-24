using DG.Tweening;
using UnityEngine;

namespace TripleBattle
{
    public class EntityEnemy : EntityBase
    {
        public EnemyData enemyData;
        public override int HP
        {
            get => base.HP;
            set
            {
                base.HP = value;
                var hpRatio = base.HP*1.0f / enemyData.maxHp;
                GameRoot.Instance.SetHPBarInfo(enemyData.name, hpRatio);
            }
        }
        public override int Shield
        {
            get => base.Shield;
            set
            {
                base.Shield = value;
                var hpRatio = base.Shield*1.0f / enemyData.maxShield;
                GameRoot.Instance.SetHPBarInfo(enemyData.name, hpRatio);
            }
        }
        public void DoAttack(EntityCharacter entityCharacter, ActionType actionType)
        {
            switch (actionType)
            {
                case ActionType.NormalAttack:
                    Attack();
                    int hit = BattleProps.atk - entityCharacter.BattleProps.def;
                    if (hit > 0)
                    {
                        if (entityCharacter.HP > hit)
                        {
                            entityCharacter.HP -= hit;
                            entityCharacter.controller.GetComponent<SpriteRenderer>().DOColor(Color.red, 0.1f).SetLoops(2, LoopType.Yoyo);
                        }
                        else if (entityCharacter.HP < hit)
                        {
                            entityCharacter.HP = 0;
                            entityCharacter.controller.GetComponent<SpriteRenderer>().DOColor(Color.red, 0.1f).SetLoops(2, LoopType.Yoyo);
                        }
                    }
                    break;

            }
        }
    }
}
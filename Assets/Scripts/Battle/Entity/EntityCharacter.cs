using DG.Tweening;
using System.Collections;
using UnityEngine;

namespace TripleBattle
{
    public class EntityCharacter : EntityBase
    {
        public CharacterData charData;
        public override int HP 
        { 
            get => base.HP;
            set
            {
                base.HP = value;
                var hpRatio = base.HP*1.0f / charData.maxHp;
                GameRoot.Instance.SetHPBarInfo(charData.name, hpRatio);
            }
        }
        public override int Shield
        {
            get => base.Shield;
            set
            {
                base.Shield = value;
                var hpRatio = base.Shield*1.0f / charData.maxShield;
                GameRoot.Instance.SetHPBarInfo(charData.name, hpRatio);
            }
        }
        public void DoAttack(EntityEnemy entityEnemy, ActionType actionType)
        {
            switch (actionType)
            {
                case ActionType.NormalAttack:
                    Attack();
                    int hit = BattleProps.atk - entityEnemy.BattleProps.def;
                    if (hit > 0)
                    {
                        if(entityEnemy.HP > hit)
                        {
                            entityEnemy.HP -= hit;
                            entityEnemy.controller.GetComponent<SpriteRenderer>().DOColor(Color.red, 0.1f).SetLoops(2, LoopType.Yoyo);
                        }
                        else if(entityEnemy.HP < hit) 
                        {
                            entityEnemy.HP = 0;
                            entityEnemy.controller.GetComponent<SpriteRenderer>().DOColor(Color.red, 0.1f).SetLoops(2, LoopType.Yoyo);
                        }
                    }
                    break;

            }
        }
    }
}
using System.Collections;

namespace TripleBattle
{
    public abstract class EntityBase
    {
        public AniState currentAniState = AniState.None;

        public BattleMgr battleMgr = null;
        public StateMgr stateMgr = null;
        public SkillMgr skillMgr = null;
        public Controller controller = null;

        private BattleProps entityProps;
        private int hp;
        private int shield;

        public BattleProps BattleProps
        {
            get
            {
                return entityProps;
            }
            protected set
            {
                entityProps = value;
            }
        }
        public virtual int HP
        {
            get { return hp; }
            set 
            { 
                //֪ͨUI
                hp = value; 
            }
        }
        public virtual int Shield
        {
            get { return shield; }
            set 
            { 
                //֪ͨUI
                shield = value; 
            }
        }

        public virtual void SetBattleProps(BattleProps battleProps)
        {
            hp = battleProps.hp;
            shield = battleProps.shield;
            entityProps = battleProps;
        }

        #region 状态切换调用入口
        public void Born()
        {
            stateMgr.ChangeStates(this, AniState.Born, null);
        }
        public void Attack()//可能直接引用配置
        {
            stateMgr.ChangeStates(this, AniState.Attack, null);
        }
        public void Move()
        {
            stateMgr.ChangeStates(this, AniState.Move, null);
        }
        public void Idle()
        {
            stateMgr.ChangeStates(this, AniState.Idle, null);
        }
        public void Hit()
        {
            stateMgr.ChangeStates(this, AniState.Hit, null);
        } 
        public void Die()
        {
            stateMgr.ChangeStates(this,AniState.Die, null);
        }
        #endregion

        #region 动画状态机调用
        public virtual void SetAnimAct(int action)
        {
            if(controller !=  null)
            {
                controller.SetAnimAct(action);
            }
        }
        #endregion

        public virtual void SetFX(string name, float destroy)
        {
            if(controller != null)
            {
                controller.SetFX(name, destroy);
            }
        }
    }
}
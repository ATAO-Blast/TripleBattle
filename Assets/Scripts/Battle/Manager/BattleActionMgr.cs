using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace TripleBattle
{
    public class BattleActionMgr : MonoBehaviour
    {
        BattleMgr mbattleMgr;
        MapCfg curMapCfg;
        List<BattleAction> batActions;
        int curBatActEnemyFollers = 0;
        public void InitMgr(BattleMgr battleMgr,MapCfg mapCfg)
        {
            mbattleMgr = battleMgr;
            curMapCfg = mapCfg;
        }
        public void StartBattle(List<BattleAction> battleActions)
        {
            batActions = battleActions;
            
            StartCoroutine(BattleActionsCor());
        }
        IEnumerator BattleActionsCor()
        {
            for (int i = 0; i < batActions.Count; i++)
            {
                yield return StartCoroutine(DoBattleAction(batActions[i]));
                if (i == batActions.Count - 1)
                {
                    mbattleMgr.InitNextBattleRound();
                }
            }
        }
        IEnumerator DoBattleAction(BattleAction battleAction)
        {
            yield return null;
            //拉近摄像机
            Camera.main.DOOrthoSize(curMapCfg.cameraMainCloseSize, 0.5f);

            //设置敌人Follower数量做标记
            if(battleAction.enemyFollower1 != null)
            {
                curBatActEnemyFollers++;
            }
            if(battleAction.enemyFollower2 != null)
            { curBatActEnemyFollers++; }
            if(battleAction.enemyFollower3 != null)
            {
                curBatActEnemyFollers++;
            }

            var entityAtkChar = battleAction.Attacker.character;
            var batType = battleAction.Attacker.actionType;
            entityAtkChar.Move();
            entityAtkChar.controller.transform.DOMove(curMapCfg.CharBatPos, 0.5f).OnComplete(() => { entityAtkChar.Idle(); });

            var entityAtkEnemy = battleAction.enemyAttacker.enemy;
            var enemybatType = battleAction.enemyAttacker.actionType;
            entityAtkEnemy.Move();
            var enemyTweener = entityAtkEnemy.controller.transform.DOMove(curMapCfg.EnemyBatPos, 0.5f).OnComplete(() => { entityAtkEnemy.Idle(); });

            if (battleAction.Follower3 != null)
            {
                yield return entityAtkChar.controller.transform.DOMoveX(curMapCfg.CharBatPos.x - 1, 0.5f);
                yield return StartCoroutine(DoFollwerBat(battleAction.Follower3,battleAction));
                yield return entityAtkChar.controller.transform.DOMove(curMapCfg.CharBatPos, 0.5f);
            }
            if (battleAction.Follower2 != null)
            {
                yield return entityAtkChar.controller.transform.DOMoveX(curMapCfg.CharBatPos.x - 1, 0.5f);
                yield return StartCoroutine(DoFollwerBat(battleAction.Follower2,battleAction));
                yield return entityAtkChar.controller.transform.DOMove(curMapCfg.CharBatPos, 0.5f);
            }
            if (battleAction.Follower1 != null)
            {
                yield return entityAtkChar.controller.transform.DOMoveX(curMapCfg.CharBatPos.x - 1, 0.5f);
                yield return StartCoroutine(DoFollwerBat(battleAction.Follower1,battleAction));
                yield return entityAtkChar.controller.transform.DOMove(curMapCfg.CharBatPos, 0.5f);
            }

            //根据Action Type进行战斗（需要等待）
            //To Do
            yield return new WaitForSeconds(1.0f);
            entityAtkChar.DoAttack(entityAtkEnemy, batType);
            entityAtkEnemy.DoAttack(entityAtkChar, enemybatType);

            yield return new WaitForSeconds(1);
            entityAtkChar.Idle();
            entityAtkEnemy.Idle();
            var charStartPos = GetCharStartPos(entityAtkChar);
            entityAtkChar.controller.transform.DOMove(charStartPos,0.5f);
            var enemyStartPos = GetEnemyStartPos(entityAtkEnemy);
            entityAtkEnemy.controller.transform.DOMove(enemyStartPos, 0.5f);

            //重置敌人Follower数量
            yield return null;
            curBatActEnemyFollers = 0;
            //重置摄像机
            yield return Camera.main.DOOrthoSize(curMapCfg.cameraMainSize, 0.5f);
            yield return new WaitForSeconds(1f);
        }
        IEnumerator DoFollwerBat(EntityCharAction follower,BattleAction curBatAction)
        {
            var entityFollower = follower.character;
            var batType = follower.actionType;
            //entityFollower.Move();
            entityFollower.controller.transform.DOMove(curMapCfg.CharBatPos, 0.5f);
            if(curBatActEnemyFollers == 3)
            {
                var entityEnemyFollower = curBatAction.enemyFollower3.enemy;
                var enemyBatType = curBatAction.enemyFollower3.actionType;
                //设置敌人移动状态
                curBatAction.enemyAttacker.enemy.controller.transform.DOMove(curMapCfg.bossPos, 0.5f);
                entityEnemyFollower.controller.transform.DOMove(curMapCfg.EnemyBatPos, 0.5f);
                //根据actionType战斗（需要等待）
                //To Do
                yield return new WaitForSeconds(1);
                entityFollower.controller.GetComponent<SpriteRenderer>().DOColor(Color.red, 0.1f).SetLoops(2, LoopType.Yoyo);
                entityEnemyFollower.controller.GetComponent<SpriteRenderer>().DOColor(Color.red, 0.1f).SetLoops(2, LoopType.Yoyo);
                curBatActEnemyFollers--;
                var charStartPos = GetCharStartPos(entityFollower);
                entityFollower.controller.transform.DOMove(charStartPos, 0.5f);

                var enemyStartPos = GetEnemyStartPos(entityEnemyFollower);
                entityEnemyFollower.controller.transform.DOMove(enemyStartPos,0.5f);
                curBatAction.enemyAttacker.enemy.controller.transform.DOMove(curMapCfg.EnemyBatPos, 0.5f);
            }

            if (curBatActEnemyFollers == 2)
            {
                var entityEnemyFollower = curBatAction.enemyFollower2.enemy;
                var enemyBatType = curBatAction.enemyFollower2.actionType;
                //设置敌人移动状态
                curBatAction.enemyAttacker.enemy.controller.transform.DOMove(curMapCfg.bossPos, 0.5f);
                entityEnemyFollower.controller.transform.DOMove(curMapCfg.bossPos, 0.5f);
                //根据actionType战斗（需要等待）
                //To Do
                yield return new WaitForSeconds(1);
                entityFollower.controller.GetComponent<SpriteRenderer>().DOColor(Color.red, 0.1f).SetLoops(2, LoopType.Yoyo);
                entityEnemyFollower.controller.GetComponent<SpriteRenderer>().DOColor(Color.red, 0.1f).SetLoops(2, LoopType.Yoyo);
                curBatActEnemyFollers--;
                var charStartPos = GetCharStartPos(entityFollower);
                entityFollower.controller.transform.DOMove(charStartPos, 0.5f);

                var enemyStartPos = GetEnemyStartPos(entityEnemyFollower);
                entityEnemyFollower.controller.transform.DOMove(enemyStartPos, 0.5f);
                curBatAction.enemyAttacker.enemy.controller.transform.DOMove(curMapCfg.EnemyBatPos, 0.5f);
            }

            if (curBatActEnemyFollers == 1)
            {
                var entityEnemyFollower = curBatAction.enemyFollower1.enemy;
                var enemyBatType = curBatAction.enemyFollower1.actionType;
                //设置敌人移动状态
                curBatAction.enemyAttacker.enemy.controller.transform.DOMove(curMapCfg.bossPos, 0.5f);
                entityEnemyFollower.controller.transform.DOMove(curMapCfg.bossPos, 0.5f);
                //根据actionType战斗（需要等待）
                //To Do
                yield return new WaitForSeconds(1);
                entityFollower.controller.GetComponent<SpriteRenderer>().DOColor(Color.red, 0.1f).SetLoops(2, LoopType.Yoyo);
                entityEnemyFollower.controller.GetComponent<SpriteRenderer>().DOColor(Color.red, 0.1f).SetLoops(2, LoopType.Yoyo);
                curBatActEnemyFollers--;
                var charStartPos = GetCharStartPos(entityFollower);
                entityFollower.controller.transform.DOMove(charStartPos, 0.5f);

                var enemyStartPos = GetEnemyStartPos(entityEnemyFollower);
                entityEnemyFollower.controller.transform.DOMove(enemyStartPos, 0.5f);
                curBatAction.enemyAttacker.enemy.controller.transform.DOMove(curMapCfg.EnemyBatPos, 0.5f);
            }

            if (curBatActEnemyFollers == 0)
            {
                var entityEnemyFollower = curBatAction.enemyAttacker.enemy;
                var enemyBatType = curBatAction.enemyAttacker.actionType;
                //设置敌人移动状态
                //var tweenerEnemy = entityEnemyFollower.controller.transform.DOMove(curMapCfg.bossPos, 0.5f);
                //根据actionType战斗（需要等待）
                //To Do
                yield return new WaitForSeconds(1);
                entityFollower.controller.GetComponent<SpriteRenderer>().DOColor(Color.red, 0.1f).SetLoops(2, LoopType.Yoyo);
                entityEnemyFollower.controller.GetComponent<SpriteRenderer>().DOColor(Color.red, 0.1f).SetLoops(2, LoopType.Yoyo);
                var charStartPos = GetCharStartPos(entityFollower);
                entityFollower.controller.transform.DOMove(charStartPos, 0.5f);

                //tweenerEnemy.PlayBackwards();//??
            }
        }
        private Vector3 GetCharStartPos(EntityCharacter entityCharacter)
        {
            var block = entityCharacter.charData.blockType;
            switch (block)
            {
                case BlockType.Y:
                    return curMapCfg.Ypos;
                case BlockType.X:
                    return curMapCfg.Xpos;
                case BlockType.A:
                    return curMapCfg.Apos;
                case BlockType.B:
                    return curMapCfg.Bpos;
            }
            return Vector3.zero;
        }
        private Vector3 GetEnemyStartPos(EntityEnemy entityEnemy)
        {
            var enemyType = entityEnemy.enemyData.enemyType;
            if(enemyType == EnemyType.Boss)
            {
                return curMapCfg.bossPos;
            }
            else if (enemyType == EnemyType.Staff)
            {
                for (int i = 1; i < curMapCfg.enemys.Length; i++)
                {
                    if (curMapCfg.enemys[i].enemyData == entityEnemy.enemyData)
                    {
                        return curMapCfg.staffPos[i - 1];
                    }
                }

            }
            return Vector3.zero;
        }
    }
}
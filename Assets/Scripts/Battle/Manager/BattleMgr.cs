using System.Collections.Generic;
using UnityEngine;

namespace TripleBattle
{
	public class BattleMgr : MonoBehaviour
	{
		ResSvc resSvc;
		AudioSvc audioSvc;

		StateMgr stateMgr;
		SkillMgr skillMgr;
		BattleActionMgr actionMgr;

		MapCfg mapCfg;

		private List<CharacterData> charList;

		private Dictionary<BlockType,EntityCharacter> entityCharDic = new Dictionary<BlockType,EntityCharacter>();
		private Dictionary<BlockType,Sprite> charPortraitDic = new Dictionary<BlockType,Sprite>();
		private Dictionary<string,Sprite> enemyPortraitDic = new Dictionary<string,Sprite>();
		private Dictionary<string,EntityEnemy> entityEnemyDic = new Dictionary<string,EntityEnemy>();
		private List<int> blockCacheLst = new List<int>(50);
		private List<int> curBlockLst = new List<int>(9);
		private List<BattleAction> battleActions = new List<BattleAction>(4);

		private int battleRound = 1;
		/// <summary>
		/// 由于计数Character BattleAction的数量
		/// </summary>
        private int curActionList = 0;
        public int BattleRound
		{
			get { return battleRound; } 
			set
			{
				battleRound = value;
			}
		}
		private T GetOrAddComponent<T> (GameObject go) where T : Component
		{
			T t = go.GetComponent<T>();
			if(t == null )
			{
				t = go.AddComponent<T>();
			}
			return t;
		}
		public void InitMgr(string mapCfgPath)
		{
			resSvc = ResSvc.Instance;
			audioSvc = AudioSvc.Instance;

			stateMgr = GetOrAddComponent<StateMgr>(gameObject);
			stateMgr.InitMgr();
			skillMgr = GetOrAddComponent<SkillMgr>(gameObject);
			skillMgr.InitMgr();

			mapCfg = resSvc.LoadMapCfgSO(mapCfgPath).mapCfg;

			actionMgr = GetOrAddComponent<BattleActionMgr>(gameObject);
			actionMgr.InitMgr(this,mapCfg);

			charList = GameRoot.Instance.GetCharList();

			resSvc.AsyncLoadScene(mapCfg.sceneName, () =>
			{
				Camera.main.transform.position = mapCfg.mainCamPos;
				Camera.main.transform.localEulerAngles = mapCfg.mainCamRota;

				battleRound = 1;
				curActionList = 0;
				LoadCharacters(mapCfg);
				LoadEnemys(mapCfg);
				BattleSys.Instance.SetBattleWndState();
				GenEnemyBatActions();
				GenerateBlocks();
				FillCurBlockList();
				RefreshBlockItems();
				AddBlockIcons();
				BattleSys.Instance.SetBattleRoundNum(battleRound);
				audioSvc.PlayBGMusic(PathDefine.BattleBG);
			});
		}

		private void LoadCharacters(MapCfg mapCfg)
		{
			foreach (var characterSO in mapCfg.chars)
			{
				
				GameObject charClone = resSvc.LoadPrefab("PrefabChar/" + characterSO.characterData.name
					,MatchBlockToPos(characterSO.characterData.blockType,mapCfg)
					,Quaternion.identity
					,null,true);

				CharacterData charData = charList[characterSO.characterData.id];

				BattleProps charBattleProps = new BattleProps
				{
					hp = charData.hp,
					atk = charData.atk,
					def = charData.def,
					shield = charData.shield,
				};

				EntityCharacter entityChar = new EntityCharacter
				{
					battleMgr = this,
					stateMgr = stateMgr,
					skillMgr = skillMgr,
					charData = charData,
				};
				entityChar.SetBattleProps(charBattleProps);
				entityChar.currentAniState = AniState.Idle;
				CharacterController charController = charClone.GetComponent<CharacterController>();
				charController.Init();
				entityChar.controller = charController;

				entityCharDic.Add(charData.blockType, entityChar);
				charPortraitDic.Add(charData.blockType, characterSO.portrait);

				var hpRate = entityChar.HP / charData.maxHp * 1.0f;

				var shieldRate = entityChar.Shield / charData.maxShield * 1.0f;

				GameRoot.Instance.AddHpItemInfo(charData.name, charController.hpRoot, hpRate,shieldRate);

			}
		}
		private void LoadEnemys(MapCfg mapCfg)
		{
			var enemyArray = mapCfg.enemys;
			for (int i = 0; i < enemyArray.Length; i++)
			{
				Vector3 enemyPos = i == 0 ? mapCfg.bossPos : mapCfg.staffPos[i - 1];
				GameObject bossClone = resSvc.LoadPrefab("PrefabEnemy/" + enemyArray[i].enemyData.name,
					enemyPos,
					Quaternion.identity,
					null,true);
				EnemyData bossData = enemyArray[i].enemyData;

				BattleProps bossBattleProps = new BattleProps()
				{
					hp = bossData.hp,
					atk = bossData.atk,
					def	= bossData.def,
					shield = bossData.shield,
				};

				EntityEnemy entityEnemy = new EntityEnemy
				{
					battleMgr = this,
					stateMgr = stateMgr,
					skillMgr = skillMgr,
					enemyData = bossData,
				};
				entityEnemy.SetBattleProps(bossBattleProps);
				entityEnemy.currentAniState = AniState.Idle;
				EnemyController enemyController = bossClone.GetComponent<EnemyController>();
				enemyController.Init();
				entityEnemy.controller = enemyController;

				entityEnemyDic.Add(bossData.name, entityEnemy);
				enemyPortraitDic.Add(bossData.name, enemyArray[i].portrait);

				var hpRate = entityEnemy.HP / bossData.maxHp * 1.0f;

				var shieldRate = entityEnemy.Shield / bossData.maxShield * 1.0f;

				GameRoot.Instance.AddHpItemInfo(bossData.name, enemyController.hpRoot, hpRate, shieldRate);
				

			}
		}
		//战斗初始化
		public void ClearEntities()
		{
			battleRound = 1;
			curActionList = 0;
			entityCharDic.Clear();
			charPortraitDic.Clear();
			entityEnemyDic.Clear();
			enemyPortraitDic.Clear();
			blockCacheLst.Clear();
			curBlockLst.Clear();
			battleActions.Clear();
		}
		/// <summary>
		/// 给BattleWnd添加每个角色头上的按键提示按钮
		/// </summary>
		public void AddBlockIcons()
		{
			foreach (var item in entityCharDic)
			{
				var charEntity = item.Value;
				BattleSys.Instance.AddBlockIcon(charEntity.controller.blockPoint, charEntity.charData.blockType);
			}
		}
		/// <summary>
		/// 加载角色时角色Prefab的位置工具函数
		/// </summary>
		/// <param name="blockType">角色所代表的三消块</param>
		/// <param name="mapCfg">地图配置</param>
		/// <returns></returns>
		private Vector3 MatchBlockToPos(BlockType blockType,MapCfg mapCfg)
		{
			if (blockType == BlockType.A)
			{
				return mapCfg.Apos;
			}
			else if (blockType == BlockType.B)
			{
				return mapCfg.Bpos;
			}
			else if (blockType == BlockType.X)
			{
				return mapCfg.Xpos;
			}
			else if (blockType == BlockType.Y)
			{
				return mapCfg.Ypos;
			}
			else
			{
				return new Vector3(Screen.width/2, Screen.height/2, 0);
			}
		}

		/// <summary>
		/// 生成三消所用的块，默认50个
		/// </summary>
		private void GenerateBlocks()
		{
			for (int i = 0; i < 50; i++)
			{
				var blockValue = Random.Range(0, 4);
				blockCacheLst.Add(blockValue);
			}
		}
		/// <summary>
		/// 将生成的三消块填充到前台的列表，前台三消块有9个
		/// </summary>
		private void FillCurBlockList()
		{
			var count = curBlockLst.Count;
			var remian = 9 - count; 
			if(remian > 0)
			{
				if(blockCacheLst.Count >= remian)
				{
					for (int i = 0; i < remian; i++)
					{
						curBlockLst.Add(blockCacheLst[0]);
						blockCacheLst.RemoveAt(0);
					}
				}
				else
				{
					GenerateBlocks();
                    for (int i = 0; i < remian; i++)
                    {
                        curBlockLst.Add(blockCacheLst[0]);
                        blockCacheLst.RemoveAt(0);
                    }
                }
			}
		}
		/// <summary>
		/// 将前台的三消块列表发送给UI刷新显示
		/// </summary>
		public void RefreshBlockItems()
		{
			List<BlockType> list = new List<BlockType>();
			for (int i = 0; i < curBlockLst.Count; i++)
			{
				list.Add((BlockType)curBlockLst[i]);
			}
			BattleSys.Instance.RefreshBlockItems(list);
		}
		private bool removing = false;
		
		/// <summary>
		/// 单消时添加BattleAction的工具函数
		/// </summary>
		private void AddActionLstByBlock(BlockType blockType)
		{
			if (curActionList < 4)
			{
				var battleAction = battleActions[curActionList];
				battleAction.Attacker = new EntityCharAction { character = entityCharDic[blockType], actionType = ActionType.NormalAttack };
				curActionList += 1;
				BattleSys.Instance.SetAcionListCharImgs(curActionList,0, charPortraitDic[blockType]);
			}
		}
		/// <summary>
		/// 三消时添加BattleAction的工具函数
		/// </summary>
        private void AddTriActionLstByBlock(BlockType blockType)
		{
			if (curActionList > 0)
			{
				var latestBatAction = battleActions[curActionList-1];
				if(latestBatAction.Follower3 != null)
				{
					if (curActionList < 4)
					{
						var battleAction = battleActions[curActionList];
						battleAction.Attacker = new EntityCharAction { character = entityCharDic[blockType],actionType = ActionType.NormalAttack };
						battleAction.Follower1 = new EntityCharAction { character = entityCharDic[blockType],actionType = ActionType.AddAttack };
                        //var battleAction = new BattleAction { Attacker = entityCharDic[blockType],Follower1 = entityCharDic[blockType] };
                        //battleActions.Add(battleAction);
                        curActionList += 1;
                        BattleSys.Instance.SetAcionListCharImgs(curActionList, 0, charPortraitDic[blockType]);
						BattleSys.Instance.SetAcionListCharImgs(curActionList,1, charPortraitDic[blockType]);
                    }
				}
				else
				{
					if(latestBatAction.Follower1 == null)
					{
						var entityToSet = entityCharDic[blockType];
						if (entityToSet == latestBatAction.Attacker.character)
						{
							latestBatAction.Follower1 = new EntityCharAction { character = entityToSet, actionType = ActionType.AddAttack };
						}
						else
						{
							latestBatAction.Follower1 = new EntityCharAction { character = entityToSet, actionType = ActionType.AuxAttack };
						}
						//latestBatAction.Follower1 = entityCharDic[blockType];
						BattleSys.Instance.SetAcionListCharImgs(curActionList, 1, charPortraitDic[blockType]);
						return;
					}
					else if(latestBatAction.Follower2 == null)
					{
                        var entityToSet = entityCharDic[blockType];
                        if (entityToSet == latestBatAction.Attacker.character)
                        {
                            latestBatAction.Follower2 = new EntityCharAction { character = entityToSet, actionType = ActionType.AddAttack };
                        }
                        else
                        {
                            latestBatAction.Follower2 = new EntityCharAction { character = entityToSet, actionType = ActionType.AuxAttack };
                        }
                        BattleSys.Instance.SetAcionListCharImgs(curActionList, 2, charPortraitDic[blockType]);
						return;
                    }
                    else if (latestBatAction.Follower3 == null)
                    {
                        var entityToSet = entityCharDic[blockType];
                        if (entityToSet == latestBatAction.Attacker.character)
                        {
                            latestBatAction.Follower3 = new EntityCharAction { character = entityToSet, actionType = ActionType.AddAttack };
                        }
                        else
                        {
                            latestBatAction.Follower3 = new EntityCharAction { character = entityToSet, actionType = ActionType.AuxAttack };
                        }
                        BattleSys.Instance.SetAcionListCharImgs(curActionList, 3, charPortraitDic[blockType]);
						return;
                    }

                }
			}
			else
			{
                var battleAction = battleActions[curActionList];
                battleAction.Attacker = new EntityCharAction { character = entityCharDic[blockType], actionType = ActionType.NormalAttack };
                battleAction.Follower1 = new EntityCharAction { character = entityCharDic[blockType], actionType = ActionType.AddAttack };
                //var battleAction = new BattleAction { Attacker = entityCharDic[blockType],Follower1 = entityCharDic[blockType] };
                //battleActions.Add(battleAction);
                curActionList += 1;
                BattleSys.Instance.SetAcionListCharImgs(curActionList, 0, charPortraitDic[blockType]);
                BattleSys.Instance.SetAcionListCharImgs(curActionList, 1, charPortraitDic[blockType]);
            }
		}
		/// <summary>
		/// 消除前台三消块的函数，供UI调用
		/// </summary>
        public void RemoveCurBlock(BlockType blockType)
		{
			if(removing) return;
			var blockId = (int)blockType;
			for (int i = 0; i < curBlockLst.Count; i++)
			{
				if (curBlockLst[i] == blockId)
				{
					if (i+1<9 && curBlockLst[i + 1] == blockId)
					{
						if (i+2<9 && curBlockLst[i + 2] == blockId)
						{
							if (curActionList == 4 && battleActions[curActionList - 1].Follower3 != null) return;
							curBlockLst.RemoveAt(i + 2);
							curBlockLst.RemoveAt(i + 1);
							curBlockLst.RemoveAt(i);
                            BattleSys.Instance.RemoveBlockItems(i);
                            BattleSys.Instance.RemoveBlockItems(i + 1);
                            BattleSys.Instance.RemoveBlockItems(i + 2);
                            removing = true;
							AddTriActionLstByBlock(blockType);
                            TimerSvc.Instance.AddTimeTask(tid =>
                            {
                                FillCurBlockList();
                                RefreshBlockItems();
                                removing = false;
                            }, 250);
                            return;
						}
						else
						{
							if (curActionList == 4) return;
                            curBlockLst.RemoveAt(i);
                            BattleSys.Instance.RemoveBlockItems(i);
                            removing = true;
							AddActionLstByBlock(blockType);
                            TimerSvc.Instance.AddTimeTask(tid =>
                            {
                                FillCurBlockList();
                                RefreshBlockItems();
                                removing = false;
                            }, 250);
                            return;
                        }
					}
					else
					{
						if (curActionList == 4) return;
						curBlockLst.RemoveAt(i);
						BattleSys.Instance.RemoveBlockItems(i);
						removing = true;
						AddActionLstByBlock(blockType);
						TimerSvc.Instance.AddTimeTask(tid =>
						{
							FillCurBlockList();
							RefreshBlockItems();
							removing = false;
						}, 250);
						return;
					}
				}
			}
		}
		/// <summary>
		/// 生成敌人战斗序列
		/// </summary>
		EntityEnemy bossEntity;
		public void GenEnemyBatActions()
		{
			curActionList = 0;
			battleActions.Clear();
			bool hasBoss = false;
			bool hasStaff = false;
			foreach (var enemyPair in entityEnemyDic)
			{
				var enemyEntity = enemyPair.Value;
				if(enemyEntity.enemyData.enemyType == EnemyType.Boss && enemyEntity.currentAniState == AniState.Idle)
				{
					hasBoss = true; 
					bossEntity = enemyPair.Value;
					continue;
				}
				if (enemyEntity.enemyData.enemyType == EnemyType.Staff && enemyEntity.currentAniState == AniState.Idle)
				{
					hasStaff = true; break;
				}
			}
			if (hasBoss)
			{
				if (hasStaff)
				{
					//给Boss添加两个Battle Action
					var bossAction = new EntityEnemyAction { enemy = bossEntity, actionType = ActionType.NormalAttack };
					battleActions.Add(new BattleAction { enemyAttacker =  bossAction });
					curActionList++;
					BattleSys.Instance.SetAcionListEnemyImgs(curActionList, 0, enemyPortraitDic[bossEntity.enemyData.name]);
					battleActions.Add(new BattleAction { enemyAttacker = bossAction });
					curActionList++;
					BattleSys.Instance.SetAcionListEnemyImgs(curActionList, 0, enemyPortraitDic[bossEntity.enemyData.name]);

					//给Staff添加Battle Action，根据配置来添加Staff的行为
					//To do
				}
				else
				{
                    var bossAction = new EntityEnemyAction { enemy = bossEntity, actionType = ActionType.NormalAttack };
					for (int i = 0; i < 4; i++)
					{
                        battleActions.Add(new BattleAction { enemyAttacker = bossAction });
                        curActionList++;
                        BattleSys.Instance.SetAcionListEnemyImgs(curActionList, 0, enemyPortraitDic[bossEntity.enemyData.name]);
                    }
                }
			}
			else
			{
				if(hasStaff)
				{
					//To do
				}
				else
				{
					//Battle End
				}
			}
			curActionList = 0;
		}

		public void StartBattle()
		{
			BattleSys.Instance.HideBattleWnd();
			actionMgr.StartBattle(battleActions);
		}
		public void InitNextBattleRound()
		{
			battleRound++;
			BattleSys.Instance.ResetActionListImages();
			BattleSys.Instance.ShowBattleWnd();
			GenEnemyBatActions();
			BattleSys.Instance.SetBattleRoundNum(battleRound);
		}
#if UNITY_EDITOR
		private void OnGUI()
        {
			if (GUILayout.Button("TestBattleAction------"))
			{
				battleActions.ForEach(battleAction => 
				{ Debug.Log(
					"Character:" + "\n"
                    + battleAction.Attacker?.character.charData.name + "/"
					+ battleAction.Follower1?.character.charData.name + "/"
                    + battleAction.Follower2?.character.charData.name + "/"
                    + battleAction.Follower3?.character.charData.name + "\n"
					+ "Enemy:" + "\n" 
                    + battleAction.enemyAttacker?.enemy.enemyData.name + "/"
                    + battleAction.enemyFollower1?.enemy.enemyData.name + "/"
                    + battleAction.enemyFollower2?.enemy.enemyData.name + "/"
                    + battleAction.enemyFollower3?.enemy.enemyData.name); 
				});
			}
        }
#endif
	}
}
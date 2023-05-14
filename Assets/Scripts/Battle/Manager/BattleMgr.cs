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

		MapCfg mapCfg;

		private List<CharacterData> charList;

		private Dictionary<string,EntityCharacter> entityCharDic = new Dictionary<string,EntityCharacter>();
		private Dictionary<string,EntityEnemy> entityEnemyDic = new Dictionary<string,EntityEnemy>();
		private List<int> blockCacheLst = new List<int>(50);
		private List<int> curBlockLst = new List<int>(9);
		public void InitMgr(string mapCfgPath)
		{
			resSvc = ResSvc.Instance;
			audioSvc = AudioSvc.Instance;

			stateMgr = gameObject.AddComponent<StateMgr>();
			stateMgr.InitMgr();
			skillMgr = gameObject.AddComponent<SkillMgr>();
			skillMgr.InitMgr();

			mapCfg = resSvc.LoadMapCfgSO(mapCfgPath).mapCfg;

			charList = GameRoot.Instance.GetCharList();

			resSvc.AsyncLoadScene(mapCfg.sceneName, () =>
			{
				Camera.main.transform.position = mapCfg.mainCamPos;
				Camera.main.transform.localEulerAngles = mapCfg.mainCamRota;

				LoadCharacters(mapCfg);
				BattleSys.Instance.SetBattleWndState();
				GenerateBlocks();
				FillCurBlockList();
				RefreshBlockItems();
				AddBlockIcons();
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
				CharacterController charController = charClone.GetComponent<CharacterController>();
				charController.Init();
				entityChar.controller = charController;

				entityCharDic.Add(charData.name, entityChar);

				var hpRate = entityChar.HP / charData.maxHp * 1.0f;

				var shieldRate = entityChar.Shield / charData.maxShield * 1.0f;

				GameRoot.Instance.AddHpItemInfo(charData.name, charController.hpRoot, hpRate,shieldRate);

			}
		}
		public void AddBlockIcons()
		{
			foreach (var item in entityCharDic)
			{
				var charEntity = item.Value;
				BattleSys.Instance.AddBlockIcon(charEntity.controller.blockPoint, charEntity.charData.blockType);
			}
		}
		public void ClearEntities()
		{
			entityCharDic.Clear();
			entityEnemyDic.Clear();
		}
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

		private void GenerateBlocks()
		{
			for (int i = 0; i < 50; i++)
			{
				var blockValue = Random.Range(0, 4);
				blockCacheLst.Add(blockValue);
			}
		}
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
							curBlockLst.RemoveAt(i + 2);
							curBlockLst.RemoveAt(i + 1);
							curBlockLst.RemoveAt(i);
                            BattleSys.Instance.RemoveBlockItems(i);
                            BattleSys.Instance.RemoveBlockItems(i + 1);
                            BattleSys.Instance.RemoveBlockItems(i + 2);
                            removing = true;
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
							curBlockLst.RemoveAt(i+1);
							curBlockLst.RemoveAt(i);
							BattleSys.Instance.RemoveBlockItems(i);
							BattleSys.Instance.RemoveBlockItems(i+1);
                            removing = true;
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
						curBlockLst.RemoveAt(i);
						BattleSys.Instance.RemoveBlockItems(i);
						removing = true;
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
	}
}
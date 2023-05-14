using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace TripleBattle
{
    [CreateAssetMenu( fileName ="new Enemy",menuName ="Create SO/New Enemy")]
    public class EnemySO : ScriptableObject
    {
        public EnemyData enemyData;
        public Sprite portrait;
    }
}

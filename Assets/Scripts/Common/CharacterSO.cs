using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace TripleBattle
{
    [CreateAssetMenu(fileName ="new Character",menuName ="Create SO/New Character")]
    public class CharacterSO : ScriptableObject
    {
        public CharacterData characterData;
        public Sprite portrait;
    }
}

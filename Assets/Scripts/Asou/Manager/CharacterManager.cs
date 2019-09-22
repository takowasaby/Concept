using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* キャラへのアクセスをまとめるstaticクラス */
public class CharacterManager : MonoBehaviour
{
    // public static Ally ally;
    // public static Enemy[] enemy;

    // まとめたほうがいい? クラス内でisAlly (味方かどうか) で識別 
    public static Character[] characters;
}

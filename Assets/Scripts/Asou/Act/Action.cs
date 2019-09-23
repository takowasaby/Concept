using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Action : MonoBehaviour
{
    private Character _ally;
    private Character _enemy;

    // 確認用
    [SerializeField]
    private bool _testAlly;
    [SerializeField]
    private FramesRoleID _framesRoleID;
    [SerializeField]
    private int _setIndex;

    // Character.charactersが初期化されたあと実行する必要があるので
    // AwakeではなくてStartに実装
    void Start()
    {
        foreach (Character character in Character.characters)
        {
            if (character.GetIsAlly()) _ally = character;
            else _enemy = character;
        }
    }

    // アニメーションの確認用
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (!_testAlly)
            {
                CallEnemy(null);
            }
            else
            {
                CallAlly(_framesRoleID, _setIndex, null);
            }
        }
    }

    public void CallAlly(FramesRoleID framesRoleID, int setIndex, UnityAction callback)
    {
        switch (framesRoleID)
        {
            case FramesRoleID.Attack:
                switch (setIndex)
                {
                    case 0:
                        StartCoroutine(Attack());
                        break;
                    case 1:
                        StartCoroutine(Talk());
                        break;
                    case 2:
                        StartCoroutine(Attack());
                        break;
                    default:
                        break;
                }
                break;
            case FramesRoleID.Buff:
                switch (setIndex)
                {
                    case 0:
                        StartCoroutine(Buff());
                        break;
                    case 1:
                        StartCoroutine(Buff());
                        break;
                    case 2:
                        StartCoroutine(Buff());
                        break;
                    default:
                        break;
                }
                break;
            case FramesRoleID.Heal:
                switch (setIndex)
                {
                    case 0:
                        StartCoroutine(Heal());
                        break;
                    case 1:
                        StartCoroutine(Heal());
                        break;
                    case 2:
                        StartCoroutine(Heal());
                        break;
                    default:
                        break;
                }
                break;
           default:
                break;
        }

        if (callback != null) callback();
    }

    public IEnumerator Attack()
    {
        _ally.ChangeStatus(AnimationID.walk);
        yield return _ally.Move(new Vector2(0, 700), 0.6f);

        // 自分が攻撃モーション
        _ally.ChangeStatus(AnimationID.stay);

        yield return new WaitForSeconds(0.2f);
        EffectMaker.instance.Make(
            EffectID.zanngeki,
            new Vector2(_enemy.GetX(), _enemy.GetY()),
            1000,
            1f,
            1
        );

        // 相手がやられモーション
        _enemy.ChangeStatus(AnimationID.stay);

        yield return new WaitForSeconds(0.3f);

        // 自分が後ろ歩きモーション
        _ally.ChangeStatus(AnimationID.stay);
        yield return _ally.Move(new Vector2(0, -700), 0.3f);
        _ally.ChangeStatus(AnimationID.stay);
    }

    public IEnumerator Talk()
    {
        _ally.ChangeStatus(AnimationID.walk);
        yield return _ally.Move(new Vector2(0, 100), 0.2f);

        // 相手にデバフのエフェクト
        EffectMaker.instance.Make(
            EffectID.zanngeki,
            new Vector2(_enemy.GetX(), _enemy.GetY()),
            1000,
            0.01f,
            1
        );

        // 相手がやられモーション
        _enemy.ChangeStatus(AnimationID.enemyStay);
        
        yield return new WaitForSeconds(1f);
        _enemy.ChangeStatus(AnimationID.enemyStay);

        // 自分が後ろ歩きモーション
        _ally.ChangeStatus(AnimationID.walk);
        yield return _ally.Move(new Vector2(0, -100), 0.2f);
        _ally.ChangeStatus(AnimationID.stay);
    }

    public IEnumerator Heal()
    {
        // 自分に回復エフェクト
        EffectMaker.instance.Make(
           EffectID.zanngeki,
           new Vector2(_ally.GetX(), _ally.GetY()),
           1000,
           0.01f,
           1
       );
       yield return new WaitForSeconds(1f);
    }

    public IEnumerator Buff()
    {
        // 自分に回復エフェクト
        EffectMaker.instance.Make(
           EffectID.zanngeki,
           new Vector2(_ally.GetX(), _ally.GetY()),
           1000,
           0.01f,
           1
       );
        yield return new WaitForSeconds(1f);
    }

    public void CallEnemy(UnityAction callback)
    {
        StartCoroutine(EnemyAttack());
        if (callback != null) callback();
    }

    public IEnumerator EnemyAttack()
    {
        _enemy.ChangeStatus(AnimationID.enemyWalk);
        yield return _enemy.Move(new Vector2(0, -700), 0.6f);

        // 相手が攻撃モーション
        _enemy.ChangeStatus(AnimationID.enemyStay);

        yield return new WaitForSeconds(0.2f);
        EffectMaker.instance.Make(
            EffectID.zanngeki,
            new Vector2(_ally.GetX(), _ally.GetY()),
            1000,
            0.01f,
            1
        );

        // 自分がやられモーション
        _ally.ChangeStatus(AnimationID.stay);

        yield return new WaitForSeconds(0.3f);
        _enemy.ChangeStatus(AnimationID.enemyWalk);
        yield return _enemy.Move(new Vector2(0, 700), 0.3f);
        _enemy.ChangeStatus(AnimationID.enemyStay);
    }

    /// <summary>
    /// 以下は使わない
    /// </summary>
    public void Call(Character character, List<FrameID> frameList, UnityAction callback)
    {
        StartCoroutine(ActionFrameList(character, frameList, callback));
    }

    public IEnumerator ActionFrameList(Character character, List<FrameID> frameList, UnityAction callback)
    {
        foreach (FrameID frameID in frameList)
        {
            switch (frameID)
            {
                case FrameID.GoForward:
                    //if (character.GetY() < 3)
                        //yield return character.Move(true);
                    break;
                case FrameID.GoBack:
                    //if (character.GetY() > 0)
                        //yield return character.Move(false);
                    break;
                case FrameID.AccumulatePower:
                    //yield return character.AccumulatePower();
                    break;
                case FrameID.RaiseSword:
                    //yield return character.RaiseSword();
                    break;
                case FrameID.CutDown:
                    //yield return character.CutDown();

                    foreach (Character existCharacter in Character.characters)
                    {
                        if (character.GetIsAlly() != existCharacter.GetIsAlly() && Math.Abs(existCharacter.GetY() - character.GetY()) <= 1.5f)
                        {
                            //int damage = (int)(character.GetAtk() * character.GetPhysicalEnhance() * character.GetSwordEnhance()
                            //    - existCharacter.GetDef());
                            //existCharacter.TakeDamage(damage);
                            break;
                        }
                    }

                    //character.Reset(); // 結の最後に行う処理

                    break;
                default:
                    break;
            }
        }
        yield return null;

        if (callback != null) callback();
    }
}

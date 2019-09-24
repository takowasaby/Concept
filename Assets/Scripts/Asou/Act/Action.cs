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
    private bool _test;
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
        if (_test)
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
    }

    public void CallAlly(FramesRoleID framesRoleID, int setIndex, UnityAction<string> callback)
    {
        switch (framesRoleID)
        {
            case FramesRoleID.Attack:
                switch (setIndex)
                {
                    case 0:
                        StartCoroutine(Attack(callback));
                        break;
                    case 1:
                        StartCoroutine(Talk(callback));
                        break;
                    case 2:
                        StartCoroutine(Attack(callback));
                        break;
                    default:
                        break;
                }
                break;
            case FramesRoleID.Buff:
                switch (setIndex)
                {
                    case 0:
                        StartCoroutine(Buff(callback));
                        break;
                    case 1:
                        StartCoroutine(Buff(callback));
                        break;
                    case 2:
                        StartCoroutine(Buff(callback));
                        break;
                    default:
                        break;
                }
                break;
            case FramesRoleID.Heal:
                switch (setIndex)
                {
                    case 0:
                        StartCoroutine(Heal(callback));
                        break;
                    case 1:
                        StartCoroutine(Heal(callback));
                        break;
                    case 2:
                        StartCoroutine(Heal(callback));
                        break;
                    default:
                        break;
                }
                break;
           default:
                break;
        }
    }

    public IEnumerator Attack(UnityAction<string> callback)
    {
        SE.instance.Play(SEID.serihukougeki);

        _ally.ChangeStatus(AnimationID.backWalk);
        yield return _ally.Move(new Vector2(0, 600), 0.6f);

        // 自分が攻撃モーション
        _ally.ChangeStatus(AnimationID.attack);

        yield return new WaitForSeconds(0.33f);

        EffectMaker.instance.Make(
            EffectID.zanngeki,
            new Vector2(_enemy.GetX(), _enemy.GetY()),
            1000,
            0.01f,
            1
        );

        SE.instance.Play(SEID.swordslash);
        
        // 相手がやられモーション
        _enemy.ChangeStatus(AnimationID.enemyDame);

        int buff = _ally.GetBuff() ? 50 : 0;
        bool endFlag = _enemy.TakeDamage(
            (int)(_ally.GetAtk() + buff - _enemy.GetDef())
        );

        yield return new WaitForSeconds(0.8f);

        if (!endFlag)
        {
            _enemy.ChangeStatus(AnimationID.enemyStay);
        }

        // 自分が後ろ歩きモーション
        _ally.ChangeStatus(AnimationID.frontWalk);
        yield return _ally.Move(new Vector2(0, -600), 0.6f);
        _ally.ChangeStatus(AnimationID.stay);

        if (callback != null)
        {
            callback(
                endFlag ? "win" : "continue"
            );
        }
    }

    public IEnumerator Talk(UnityAction<string> callback)
    {
        _ally.ChangeStatus(AnimationID.backWalk);
        yield return _ally.Move(new Vector2(0, 200), 0.4f);
        _ally.ChangeStatus(AnimationID.stay);

        SE.instance.Play(SEID.serihubatou);

        // 相手にデバフのエフェクト
        EffectMaker.instance.Make(
            EffectID.zanngeki,
            new Vector2(_enemy.GetX(), _enemy.GetY()),
            1000,
            0.01f,
            1
        );

        // 相手がやられモーション
        _enemy.ChangeStatus(AnimationID.enemyDame);

        int buff = _ally.GetBuff() ? 50 : 0;
        bool endFlag = _enemy.TakeDamage(
            (int)(_ally.GetAtk() + buff - _enemy.GetDef())
        );

        yield return new WaitForSeconds(0.8f);

        if (!endFlag)
        {
            _enemy.ChangeStatus(AnimationID.enemyStay);
        }

        // 自分が後ろ歩きモーション
        _ally.ChangeStatus(AnimationID.frontWalk);
        yield return _ally.Move(new Vector2(0, -200), 0.4f);
        _ally.ChangeStatus(AnimationID.stay);

        if (callback != null)
        {
            callback(
                endFlag ? "win" : "continue"
            );
        }
    }

    public IEnumerator Heal(UnityAction<string> callback)
    {
        SE.instance.Play(SEID.kaihuku);

        _ally.ChangeStatus(AnimationID.yorokobi);

        // 自分に回復エフェクト
        EffectMaker.instance.Make(
            EffectID.zanngeki,
            new Vector2(_ally.GetX(), _ally.GetY()),
            1000,
            0.01f,
            1
        );
        _ally.HealHp();
        yield return new WaitForSeconds(1f);

        _ally.ChangeStatus(AnimationID.stay);

        if (callback != null)
        {
            callback("continue");
        }
    }

    public IEnumerator Buff(UnityAction<string> callback)
    {
        SE.instance.Play(SEID.bahu);

        _ally.ChangeStatus(AnimationID.yorokobi);

        // 自分にバフエフェクト
        EffectMaker.instance.Make(
            EffectID.zanngeki,
            new Vector2(_ally.GetX(), _ally.GetY()),
            1000,
            0.01f,
            1
        );
        _ally.SetBuff();
        yield return new WaitForSeconds(1f);

        _ally.ChangeStatus(AnimationID.stay);

        if (callback != null)
        {
            callback("continue");
        }
    }

    public void CallEnemy(UnityAction<string> callback)
    {
        StartCoroutine(EnemyAttack(callback));
    }

    public IEnumerator EnemyAttack(UnityAction<string> callback)
    {   
        _enemy.ChangeStatus(AnimationID.enemyStay);
        yield return _enemy.Move(new Vector2(0, -600), 0.6f);

        // 相手が攻撃モーション
        _enemy.ChangeStatus(AnimationID.enemyAttack);

        yield return new WaitForSeconds(0.33f);
        
        /*
        EffectMaker.instance.Make(
            EffectID.zanngeki,
            new Vector2(_ally.GetX(), _ally.GetY()),
            1000,
            0.01f,
            1
        );
        */

        SE.instance.Play(SEID.swordslash);

        // 自分がやられモーション
        _ally.ChangeStatus(AnimationID.stay);

        int buff = _ally.GetBuff() ? 30 : 0;
        bool endFlag = _ally.TakeDamage(
            (int)(_enemy.GetAtk() - _ally.GetDef() - buff)
        );

        yield return new WaitForSeconds(0.67f);

        if (!endFlag)
        {
            _ally.ChangeStatus(AnimationID.stay);
        }

        // 相手が前歩きモーション
        _enemy.ChangeStatus(AnimationID.enemyStay);
        yield return _enemy.Move(new Vector2(0, 600), 0.3f);
        _enemy.ChangeStatus(AnimationID.enemyStay);

        if (callback != null)
        {
            callback(
                endFlag ? "lose" : "continue"
            );
        }
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

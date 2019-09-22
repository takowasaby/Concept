using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/* キャラクターを定義するスーパークラス */
public class Character : MonoBehaviour
{
    [SerializeField]
    private int _hp;
    [SerializeField]
    private int _atk;
    [SerializeField]
    private int _def;
    [SerializeField]
    private int _x;
    [SerializeField]
    private float _y;
    [SerializeField]
    private int _spd;

    [SerializeField]
    private RectTransform HpBarRTF;

    public static List<Character> characters;

    protected bool _isAlly; // 味方かどうか

    private int _id;
    private int _remainHp;
    private float _physicalEnhance;
    private float _swordEnhance;

    public void Awake()
    {
        _isAlly = true;

        if (characters == null)
            characters = new List<Character>();

        _id = characters.Count;
        characters.Add(this);
        _remainHp = _hp;
        _physicalEnhance = 1f;
        _swordEnhance = 1f;

        UpdateHpBar();
    }

    public void OnValidate()
    {
        PutScreen();
    }

    public bool GetIsAlly() { return _isAlly; }
    public int GetHp() { return _hp; }
    public int GetAtk() { return _atk; }
    public int GetDef() { return _def; }
    public int GetX() { return _x; }
    public float GetY() { return _y; }
    public int GetSpd() {
        // ランダムで変化させる (優先度は低い)
        return _spd;
    }
    public int GetRemainHp() { return _remainHp; }
    public float GetPhysicalEnhance() { return _physicalEnhance; }
    public float GetSwordEnhance() { return _swordEnhance; }

    private void UpdateHpBar()
    {
        Vector2 anchorMaxVec = HpBarRTF.anchorMax;
        anchorMaxVec.x = (float)_remainHp / _hp;
        HpBarRTF.anchorMax = anchorMaxVec;
    }

    // 今は使用していない
    private int Lerp(int y0, int y1, double ratio)
    {
        if (ratio < 0.0) ratio = 0.0;
        else if (ratio > 1.0) ratio = 1.0;

        return (int)((1.0 - ratio) * y0 + ratio * y1);
    }

    private void PutScreen()
    {
        Vector2 pos = transform.localPosition;
        pos.x = 200 + _x * 150;
        pos.y = 200 + _y * 350;
        transform.localPosition = pos;
    }

    public void TakeDamage(int damage)
    {
        _remainHp -= damage;
        Debug.Log(damage + "ダメージ");

        if (_remainHp < 0)
        {
            _remainHp = 0;
            characters.Remove(this);
            Destroy(gameObject);
        }

        UpdateHpBar();
    }

    public void Reset()
    {
        _physicalEnhance = 1f;
        _swordEnhance = 1f;
    }

    public IEnumerator Move(bool forward)
    {
        float nextY;
        if (forward)
        {
            Debug.Log("前進");
            nextY = _y + 1;
            while (_y < nextY)
            {
                _y += 0.01f;
                PutScreen();
                yield return null;
            }
        }
        else
        {
            Debug.Log("後退");
            nextY = _y - 1;
            while(_y > nextY)
            {
                _y -= 0.01f;
                PutScreen();
                yield return null;
            }
        }
        _y = nextY;
        PutScreen();
    }

    public IEnumerator AccumulatePower()
    {
        Debug.Log("物理攻撃力アップ");
        _physicalEnhance = 1.5f;
        yield return new WaitForSeconds(1f);
    }
  
    public IEnumerator RaiseSword()
    {
        Debug.Log("剣攻撃力アップ");
        _swordEnhance = 1.5f;
        yield return new WaitForSeconds(1f);
    }

    public IEnumerator CutDown()
    {
        Debug.Log("剣で攻撃");
        yield return new WaitForSeconds(1f);
    }
}

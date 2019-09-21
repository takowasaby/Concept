using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/* キャラクターを定義するスーパークラス */
public class Character : MonoBehaviour
{
    private const float deltaDist = 0.05f;

    [SerializeField]
    private int _hp;
    [SerializeField]
    private float _pos;
    [SerializeField]
    private int _spd;

    private int _remainHp;

    public void Awake()
    {
        _hp = 1;
        _pos = 0f;
        _spd = 0;
    }

    public void Set(int hp, int pos, int spd)
    {
        _hp = hp;
        _pos = (float)pos;
        _spd = spd;

        PutScreen();
    }

    public void OnValidate()
    {
        PutScreen();
    }

    private void PutScreen()
    {

    }

    public IEnumerator Move(UnityAction callback, bool forward)
    {
        float nextPos;
        if (forward)
        {
            nextPos = _pos + 1;
            while (_pos < nextPos)
            {
                _pos += deltaDist;
                PutScreen();
                yield return null;
            }
        }
        else
        {
            nextPos = _pos - 1;
            while(_pos > nextPos)
            {
                _pos -= deltaDist;
                PutScreen();
                yield return null;
            }
        }
        _pos = nextPos;
        PutScreen();

        if (callback != null) callback();
    }
}

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
    private int _x;
    [SerializeField]
    private float _y;
    [SerializeField]
    private int _spd;

    public static List<Character> characters;

    protected bool _isAlly; // 味方かどうか
    private int _remainHp;

    public void Awake()
    {
        _isAlly = true;
        _remainHp = _hp;
    }

    public void OnValidate()
    {
        PutScreen();
    }

    public int GetHp() { return _hp; }
    public int GetX() { return _x; }
    public float GetY() { return _y; }
    public int GetSpd() { return _spd; }
    public int GetRemainHp() { return _remainHp; }

    private int Lerp(int y0, int y1, double ratio)
    {
        if (ratio < 0.0) ratio = 0.0;
        else if (ratio > 1.0) ratio = 1.0;

        return (int)((1.0 - ratio) * y0 + ratio * y1);
    }

    private void PutScreen()
    {
        Vector2 endPoint = new Vector2(500, 100);


        Vector2 pos = transform.localPosition;
        pos.x = _x;
        pos.y = _y;
        transform.localPosition = pos;
    }

    public IEnumerator Move(bool forward)
    {
        float nextY;
        if (forward)
        {
            nextY = _y + 1;
            while (_y < nextY)
            {
                _y += 0.05f;
                PutScreen();
                yield return null;
            }
        }
        else
        {
            nextY = _y - 1;
            while(_y > nextY)
            {
                _y -= 0.05f;
                PutScreen();
                yield return null;
            }
        }
        _y = nextY;
        PutScreen();
    }
}

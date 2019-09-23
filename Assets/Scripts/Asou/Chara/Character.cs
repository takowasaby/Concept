using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

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

    /// <summary>
    /// アニメ関係
    /// </summary>
    private class ValueList
    {
        public List<Sprite> List = new List<Sprite>();

        public ValueList(List<Sprite> list)
        {
            List = list;
        }
    }

    private List<ValueList> _imageList = new List<ValueList>();

    private static List<string> _headerList = new List<string>
    {
        "Animation/stay/stay_",
        "Animation/walk/back/walk_"
    };

    private static List<int> _maxList = new List<int>
    {
        30,
        30
    };

    [SerializeField]
    private float _interval;

    private Image _image;

    private float _passTime;
    private int _counter;
    protected AnimationID _status;
    /// <summary>
    /// アニメ関係終了
    /// </summary>

    public void Awake()
    {
        _isAlly = true;

        if (characters == null)
            characters = new List<Character>();

        _id = characters.Count;
        characters.Add(this);
        _remainHp = _hp;

        UpdateHpBar();

        int headerIdx = 0;
        foreach (string header in _headerList)
        {
            List<Sprite> spriteList = new List<Sprite>();
            for (int i = 1; i <= _maxList[headerIdx]; i++)
            {
                string numberString;
                if (i < 10) numberString = "000" + i;
                else numberString = "00" + i;
                spriteList.Add(
                    (Sprite)Resources.Load<Sprite>(header + numberString)
                );
            }

            ValueList vl = new ValueList(spriteList);
            _imageList.Add(vl);

            headerIdx++;
        }

        _image = gameObject.GetComponent<Image>();
        _passTime = 0f;
        _counter = 0;
        _status = AnimationID.stay;
    }

    void Update()
    {
        // 常に稼働しているアニメーション
        _passTime += Time.deltaTime;
        if (_passTime >= _interval)
        {
            _passTime = 0f;

            _image.sprite = _imageList[(int)_status].List[_counter];

            _counter++;
            if (_counter == _maxList[(int)_status])
            {
                _counter = 0;
            }
        }

        if (_isAlly && Input.GetMouseButtonDown(2))
        {
            StartCoroutine(TestMove());
        }
    }

    void OnValidate()
    {
        Vector2 pos = transform.localPosition;
        pos.x = _x;
        pos.y = _y;
        transform.localPosition = pos;
    }

    // テスト用
    private IEnumerator TestMove()
    {
        ChangeStatus(AnimationID.walk);
        yield return Move(new Vector2(0, 800), 0.6f);
        ChangeStatus(AnimationID.stay);
        yield return new WaitForSeconds(0.3f);
        yield return Move(new Vector2(0, -800), 0.3f);
    }

    public void ChangeStatus(AnimationID status)
    {
        _status = status;
        _counter = 0;
        _passTime = 0f;
    }

    public bool GetIsAlly() { return _isAlly; }
    public int GetHp() { return _hp; }
    public int GetAtk() { return _atk; }
    public int GetDef() { return _def; }
    public int GetX() { return _x; }
    public float GetY() { return _y; }
    public int GetSpd() { return _spd; }
    public int GetRemainHp() { return _remainHp; }

    private void UpdateHpBar()
    {
        Vector2 anchorMaxVec = HpBarRTF.anchorMax;
        anchorMaxVec.x = (float)_remainHp / _hp;
        HpBarRTF.anchorMax = anchorMaxVec;
    }

    public bool TakeDamage(int damage)
    {
        _remainHp -= damage;
        Debug.Log(damage + "ダメージ");

        if (_remainHp < 0)
        {
            _remainHp = 0;
        }

        UpdateHpBar();

        // 死んだらTrue
        if (_remainHp == 0) return true;
        return false;
    }

    public IEnumerator Move(Vector2 dist, float time)
    {
        Vector2 startPos = transform.localPosition;
        float passTime = 0f;

        while (passTime < time)
        {
            passTime += Time.deltaTime;
            transform.localPosition = new Vector2(
                startPos.x + dist.x * passTime / time,
                startPos.y + dist.y * passTime / time
            );
            yield return null;
        }
        transform.localPosition = new Vector2(
            startPos.x + dist.x,
            startPos.y + dist.y
        );
    }
}

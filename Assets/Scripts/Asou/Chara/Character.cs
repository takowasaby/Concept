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
    private float _x;
    [SerializeField]
    private float _y;
    [SerializeField]
    private int _spd;
    [SerializeField]
    private int _heal;

    [SerializeField]
    private bool _test;

    [SerializeField]
    private RectTransform HpBarRTF;
    [SerializeField]
    private GameObject buffImage;

    public static List<Character> characters;

    protected bool _isAlly; // 味方かどうか

    private int _id;
    private int _remainHp;
    private bool _buff;

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
        "Animation/stay/stay_00",
        "Animation/walk/back/walk_00",
        "Animation/walk/front/walk_00",
        "Animation/attack/attack_",
        "Animation/yorokobi/yorokobi_",
        "Animation/enemyStay/stay_",
        "Animation/enemyAttack/kougeki_",
        "Animation/enemyDame/dame_",
    };

    private static List<int> _maxList = new List<int>
    {
        30,
        30,
        30,
        30,
        30,
        29,
        29,
        29
    };

    [SerializeField]
    private float _interval;
    [SerializeField]
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
        _buff = false;

        UpdateHpBar();

        int headerIdx = 0;
        foreach (string header in _headerList)
        {
            List<Sprite> spriteList = new List<Sprite>();
            for (int i = 1; i <= _maxList[headerIdx]; i++)
            {
                string numberString;
                if (i < 10) numberString = "0" + i;
                else numberString = i.ToString();
                spriteList.Add(
                    (Sprite)Resources.Load<Sprite>(header + numberString)
                );
            }

            ValueList vl = new ValueList(spriteList);
            _imageList.Add(vl);

            headerIdx++;
        }
        
        _passTime = 0f;
        _counter = 0;
        _status = AnimationID.stay;
    }

    void Start()
    {
        ChangeSize();
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
        
        if (_test && _isAlly && Input.GetMouseButtonDown(2))
        {
            StartCoroutine(TestMove());
        }
    }

    void OnValidate()
    {
        ChangePos();
        ChangeSize();
    }

    private void ChangePos()
    {
        transform.localPosition = new Vector2(_x, _y);
    }

    private void ChangeSize()
    {
        float size = 210000 / _y + 400;
        if (!_isAlly)
        {
            size *= 1.3f;
        }
        transform.GetComponent<RectTransform>().sizeDelta = new Vector2(size, size);
    }

    // テスト用
    private IEnumerator TestMove()
    {
        ChangeStatus(AnimationID.backWalk);
        yield return Move(new Vector2(0, 800), 0.6f);
        ChangeStatus(AnimationID.stay);
        yield return new WaitForSeconds(0.3f);
        ChangeStatus(AnimationID.frontWalk);
        yield return Move(new Vector2(0, -800), 0.3f);
        ChangeStatus(AnimationID.stay);
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
    public float GetX() { return _x; }
    public float GetY() { return _y; }
    public int GetSpd() { return _spd; }
    public int GetRemainHp() { return _remainHp; }
    public bool GetBuff() { return _buff; }

    public void SetBuff()
    {
        _buff = true;
        buffImage.SetActive(true);
    }

    private void UpdateHpBar()
    {
        Vector2 anchorMaxVec = HpBarRTF.anchorMax;
        anchorMaxVec.x = (float)_remainHp / _hp;
        HpBarRTF.anchorMax = anchorMaxVec;
    }

    public bool TakeDamage(int damage)
    {
        _remainHp -= damage;
        //Debug.Log(damage + "ダメージ");

        if (_remainHp < 0)
        {
            _remainHp = 0;
        }

        UpdateHpBar();

        // 死んだらTrue
        if (_remainHp == 0) return true;
        return false;
    }

    public void HealHp()
    {
        _remainHp += _heal;
        if (_remainHp > _hp)
        {
            _remainHp = _hp;
        }
        UpdateHpBar();
    }

    public IEnumerator Move(Vector2 dist, float time)
    {
        Vector2 startPos = transform.localPosition;
        float passTime = 0f;

        while (passTime < time)
        {
            passTime += Time.deltaTime;

            _x = (int)(startPos.x + dist.x * passTime / time);
            _y = (int)(startPos.y + dist.y * passTime / time);
            ChangePos();
            ChangeSize();
            yield return null;
        }

        _x = (int)(startPos.x + dist.x);
        _y = (int)(startPos.y + dist.y);
        ChangePos();
        ChangeSize();
    }
}

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
    private float _physicalEnhance;
    private float _swordEnhance;

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

    private List<string> _headerList = new List<string>
    {
        "Animation/stay/stay_",
        "Animation/walk/back/walk_"
    };

    private List<int> _maxList = new List<int>
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
        _physicalEnhance = 1f;
        _swordEnhance = 1f;

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
    }

    public void OnValidate()
    {
        PutScreen();
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

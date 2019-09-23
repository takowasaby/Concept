using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Effect : MonoBehaviour
{
    private Image _image;

    private List<Sprite> _imageList;
    private float _interval;
    private string _header;
    private int _max;
    private int _loop;

    private int _counter;
    private float _passTime;
    private int _passLoop;

    public void SetPos(Vector2 pos)
    {
        transform.localPosition = pos;
    }
    public void SetSize(int size)
    {
        RectTransform rtf = transform.GetComponent<RectTransform>();
        rtf.sizeDelta = new Vector2(size, size);
    }

    public void SetImageList(List<Sprite> imageList) { _imageList = imageList; }
    public void SetInterval(float interval) { _interval = interval; }
    public void SetHeader(string header) { _header = header; }
    public void SetMax(int max) { _max = max; }
    public void SetLoop(int loop) { _loop = loop; }

    void Awake()
    {
        _image = gameObject.GetComponent<Image>();
        _counter = 0;
        _passTime = 0f;
        _passLoop = 0;
    }

    void Update()
    {
        _passTime += Time.deltaTime;
        if (_passTime >= _interval)
        {
            _passTime = 0f;
            if (_counter < _max)
            {
                _image.sprite = _imageList[_counter];
                _counter++;
            }
            else
            {
                _counter = 0;
                _passLoop++;
                if (_passLoop >= _loop)
                {
                    OnComplete();
                }
            }
            _image.color = new Color(1, 1, 1, 1);
        }
    }

    private void OnComplete()
    {
        Destroy(gameObject);
    }
}

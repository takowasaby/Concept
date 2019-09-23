using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectMaker : MonoBehaviour
{
    [SerializeField]
    private GameObject _parentGameObject = null;
    
    private class ValueList
    {
        public List<Sprite> List = new List<Sprite>();

        public ValueList(List<Sprite> list)
        {
            List = list;
        }
    }
    private List<ValueList> _imageList = new List<ValueList>();

    // 確認用
    [SerializeField]
    private EffectID _effectID;
    [SerializeField]
    private int _size;
    [SerializeField]
    private float _interval;
    [SerializeField]
    private int _loop;

    void Awake()
    {
        List<string> headerList = new List<string>
        {
            "Effect/zanngeki/kirisaki."
        };

        List<int> maxList = new List<int>
        {
            40
        };

        int headerIdx = 0;
        foreach (string header in headerList)
        {
            List<Sprite> spriteList = new List<Sprite>();
            for (int i = 0; i < maxList[headerIdx]; i++) {
                spriteList.Add(
                    (Sprite)Resources.Load<Sprite>(header + i)
                );
            }

            ValueList vl = new ValueList(spriteList);
            _imageList.Add(vl);

            headerIdx++;
        }
    }

    public void Make(EffectID effectID, Vector2 pos, int size, float interval, int loop)
    {
        List<Sprite> imageList = _imageList[(int)effectID].List;

        GameObject effectPrefab = (GameObject)Resources.Load("Prefabs/EffectObject");
        GameObject effect = (GameObject)Instantiate(effectPrefab);
        Effect effectScript = effect.GetComponent<Effect>();

        effectScript.SetPos(pos);
        effectScript.SetSize(size);
        effectScript.SetInterval(interval);
        effectScript.SetImageList(imageList);
        effectScript.SetMax(imageList.Count);
        effectScript.SetLoop(loop);

        effect.transform.SetParent(_parentGameObject.transform);
    }

    // エフェクトの確認用
    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            Vector2 mousePos = Input.mousePosition;
            Make(_effectID, mousePos, _size, _interval, _loop);
        }
    }
}

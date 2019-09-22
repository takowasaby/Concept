using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* 敵キャラクターを定義するクラス */
public class Enemy : Character
{
    // Inspectorに複数データを表示するためのクラス
    [System.SerializableAttribute]
    public class ValueList
    {
        public List<FrameID> List = new List<FrameID>();

        public ValueList(List<FrameID> list)
        {
            List = list;
        }
    }

    // Inspectorに表示される
    [SerializeField]
    private List<ValueList> _preset = new List<ValueList>();

    public new void Awake()
    {
        base.Awake();
        _isAlly = false;
    }

    public int GetFrameListSize() { return _preset.Count; }

    public List<FrameID> GetFrameList(int id)
    {
        id = id % _preset.Count;
        return _preset[id].List;
    }
}

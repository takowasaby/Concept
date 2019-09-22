using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    [SerializeField]
    private Character _character;
    [SerializeField]
    private List<FrameID> _frameList;
    [SerializeField]
    private Action _action;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(_action.ActionFrameList(_character, _frameList, null));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

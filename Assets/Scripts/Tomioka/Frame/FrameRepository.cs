using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrameRepository : MonoBehaviour
{
    [SerializeField]
    List<FrameID> frameIDs = new List<FrameID>();

    [SerializeField]
    List<SituationID> situationIDs = new List<SituationID>();

    [SerializeField]
    List<GameObject> frameViewPrefabs = new List<GameObject>();

    List<FrameInfo> frameInfos = new List<FrameInfo>();

    struct FrameInfo
    {
        public FrameID frameID;
        public SituationID situationID;
        public GameObject frameViewPrefab;
    }

    void Start()
    {
        frameInfos.Clear();
        for (int i = 0; i < frameIDs.Count && i < situationIDs.Count && i < frameViewPrefabs.Count; i++)
        {
            FrameInfo frameInfo = new FrameInfo();
            frameInfo.frameID = frameIDs[i];
            frameInfo.situationID = situationIDs[i];
            frameInfo.frameViewPrefab = frameViewPrefabs[i];
            frameInfos.Add(frameInfo);
        }
    }

    public GameObject GenerateFrameView(FrameID frameID, Vector3 position, Quaternion rotation, Transform parent)
    {
        foreach (FrameInfo frameInfo in frameInfos)
        {
            if (frameInfo.frameID == frameID)
            {
                return Instantiate(frameInfo.frameViewPrefab, position, rotation, parent);
            }
        }
        throw new System.Exception("無効なFrameIDが入力されました。");
    }

    public SituationID GetSituation(FrameID frameID)
    {
        foreach (FrameInfo frameInfo in frameInfos)
        {
            if (frameInfo.frameID == frameID)
            {
                return frameInfo.situationID;
            }
        }
        throw new System.Exception("無効なFrameIDが入力されました。");
    }
}

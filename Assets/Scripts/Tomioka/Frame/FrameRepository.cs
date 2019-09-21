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
    List<Sprite> frameViews = new List<Sprite>();

    List<FrameInfo> frameInfos = new List<FrameInfo>();

    struct FrameInfo
    {
        public FrameID frameID;
        public SituationID situationID;
        public Sprite frameView;
    }

    void Start()
    {
        frameInfos.Clear();
        for (int i = 0; i < frameIDs.Count && i < situationIDs.Count && i < frameViews.Count; i++)
        {
            FrameInfo frameInfo = new FrameInfo();
            frameInfo.frameID = frameIDs[i];
            frameInfo.situationID = situationIDs[i];
            frameInfo.frameView = frameViews[i];
            frameInfos.Add(frameInfo);
        }
    }

    public Sprite GenerateFrameView(FrameID frameID)
    {
        foreach (FrameInfo frameInfo in frameInfos)
        {
            if (frameInfo.frameID == frameID)
            {
                return frameInfo.frameView;
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

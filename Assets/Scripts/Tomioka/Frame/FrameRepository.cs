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

    [SerializeField]
    List<Sprite> frameHandViews = new List<Sprite>();

    [SerializeField]
    List<bool> IsObstacleFlags = new List<bool>();

    List<FrameInfo> frameInfos = new List<FrameInfo>();

    struct FrameInfo
    {
        public FrameID frameID;
        public SituationID situationID;
        public Sprite frameView;
        public Sprite frameHandView;
        public bool IsObstacle;
    }

    void Start()
    {
        frameInfos.Clear();
        for (int i = 0; i < frameIDs.Count && i < situationIDs.Count && i < frameViews.Count && i < IsObstacleFlags.Count && i < frameHandViews.Count; i++)
        {
            FrameInfo frameInfo = new FrameInfo();
            frameInfo.frameID = frameIDs[i];
            frameInfo.situationID = situationIDs[i];
            frameInfo.frameView = frameViews[i];
            frameInfo.frameHandView = frameHandViews[i];
            frameInfo.IsObstacle = IsObstacleFlags[i];
            frameInfos.Add(frameInfo);
        }
    }

    public Sprite GetFrameView(FrameID frameID)
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

    public Sprite GetFrameHandView(FrameID frameID)
    {
        foreach (FrameInfo frameInfo in frameInfos)
        {
            if (frameInfo.frameID == frameID)
            {
                return frameInfo.frameHandView;
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

    public bool GetObstacleFlag(FrameID frameID)
    {
        foreach (FrameInfo frameInfo in frameInfos)
        {
            if (frameInfo.frameID == frameID)
            {
                return frameInfo.IsObstacle;
            }
        }
        throw new System.Exception("無効なFrameIDが入力されました。");
    }
}

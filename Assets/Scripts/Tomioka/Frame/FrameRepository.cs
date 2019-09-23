using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrameRepository : MonoBehaviour
{
    [SerializeField]
    List<CorrectFramesInfo> correctFramesList = new List<CorrectFramesInfo>();

    [SerializeField]
    List<FrameInfo> frameInfos = new List<FrameInfo>();

    [System.Serializable]
    struct CorrectFramesInfo
    {
        public FramesRoleID framesRoleID;
        public int setIndex;
        public List<FrameID> frames;
    }

    [System.Serializable]
    struct FrameInfo
    {
        public FrameID frameID;
        public SituationID situationID;
        public Sprite frameView;
        public List<string> ngWords;
    }

    public System.Tuple<FramesRoleID, int> CheckFrames(List<FrameID> frames)
    {
        if (frames.Count != 4) return null;
        foreach (CorrectFramesInfo correctFramesInfo in correctFramesList)
        {
            if (correctFramesInfo.frames[0] == frames[0])
            {
                if (correctFramesInfo.frames[1] == frames[1] && correctFramesInfo.frames[2] == frames[2] && correctFramesInfo.frames[3] == frames[3])
                {
                    return System.Tuple.Create(correctFramesInfo.framesRoleID, correctFramesInfo.setIndex);
                }
                return null;
            }
        }
        return null;
    }

    public List<FrameID> GetCorrectFrames(FramesRoleID framesRoleID, int setIndex)
    {
        foreach(CorrectFramesInfo correctFramesInfo in correctFramesList)
        {
            if (correctFramesInfo.framesRoleID == framesRoleID && correctFramesInfo.setIndex == setIndex)
            {
                return correctFramesInfo.frames;
            }
        }
        return new List<FrameID>();
    }

    public FrameID GetCurrentFramesPart(FramesRoleID framesRoleID, int setIndex, SituationID situation)
    {
        foreach (CorrectFramesInfo correctFramesInfo in correctFramesList)
        {
            if (correctFramesInfo.framesRoleID == framesRoleID && correctFramesInfo.setIndex == setIndex)
            {
                return correctFramesInfo.frames[(int)situation];
            }
        }
        return FrameID.NullFrame;
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

    public List<string> GetNGWords(FrameID frameID)
    {
        foreach (FrameInfo frameInfo in frameInfos)
        {
            if (frameInfo.frameID == frameID)
            {
                return frameInfo.ngWords;
            }
        }
        throw new System.Exception("無効なFrameIDが入力されました。");
    }
}

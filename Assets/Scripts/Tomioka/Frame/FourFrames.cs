using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class FourFrames : MonoBehaviour
{
    [SerializeField]
    FrameRepository frameRepository = null;

    [SerializeField]
    SpriteRenderer kiFrameViews = null;
    [SerializeField]
    SpriteRenderer shoFrameViews = null;
    [SerializeField]
    SpriteRenderer tenFrameViews = null;
    [SerializeField]
    SpriteRenderer ketsuFrameViews = null;

    [SerializeField]
    Sprite blackFrame = null;

    List<FrameID> frameIDs = new List<FrameID>(4);

    public void AddFrame(FrameID frameID)
    {
        switch(frameRepository.GetSituation(frameID))
        {
            case SituationID.Ki:
                if (kiFrameViews.sprite == null)
                {
                    kiFrameViews.sprite = blackFrame;
                    frameIDs[0] = frameID;
                }
                break;
            case SituationID.Sho:
                if (shoFrameViews.sprite == null)
                {
                    shoFrameViews.sprite = blackFrame;
                    frameIDs[1] = frameID;
                }
                break;
            case SituationID.Ten:
                if (tenFrameViews.sprite == null)
                {
                    tenFrameViews.sprite = blackFrame;
                    frameIDs[2] = frameID;
                }
                break;
            case SituationID.Ketsu:
                if (ketsuFrameViews.sprite == null)
                {
                    ketsuFrameViews.sprite = blackFrame;
                    frameIDs[3] = frameID;
                }
                break;
        }
    }

    public void DisplayFrame()
    {
        kiFrameViews.sprite = frameRepository.GetFrameView(frameIDs[0]);
        shoFrameViews.sprite = frameRepository.GetFrameView(frameIDs[1]);
        tenFrameViews.sprite = frameRepository.GetFrameView(frameIDs[2]);
        ketsuFrameViews.sprite = frameRepository.GetFrameView(frameIDs[3]);
    }

    public void ResetFrames()
    {
        frameIDs = new List<FrameID>(4);
        kiFrameViews.sprite = null;
        shoFrameViews.sprite = null;
        tenFrameViews.sprite = null;
        ketsuFrameViews.sprite = null;
    }

    public IEnumerable<FrameID> GetFrameIDs()
    {
        return frameIDs;
    }
}

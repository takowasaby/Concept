using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    List<FrameID> frameIDs = new List<FrameID>(4);

    public void AddFrame(FrameID frameID)
    {
        switch(frameRepository.GetSituation(frameID))
        {
            case SituationID.Ki:
                if (kiFrameViews.sprite == null)
                {
                    kiFrameViews.sprite = frameRepository.GetFrameView(frameID);
                    frameIDs[0] = frameID;
                }
                break;
            case SituationID.Sho:
                if (shoFrameViews.sprite == null)
                {
                    shoFrameViews.sprite = frameRepository.GetFrameView(frameID);
                    frameIDs[1] = frameID;
                }
                break;
            case SituationID.Ten:
                if (tenFrameViews.sprite == null)
                {
                    tenFrameViews.sprite = frameRepository.GetFrameView(frameID);
                    frameIDs[2] = frameID;
                }
                break;
            case SituationID.Ketsu:
                if (ketsuFrameViews.sprite == null)
                {
                    ketsuFrameViews.sprite = frameRepository.GetFrameView(frameID);
                    frameIDs[3] = frameID;
                }
                break;
        }
    }

    public void ResetFrames()
    {
        frameIDs = new List<FrameID>(4);
        kiFrameViews.sprite = null;
        shoFrameViews.sprite = null;
        tenFrameViews.sprite = null;
        ketsuFrameViews.sprite = null;
    }
}

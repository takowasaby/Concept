using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class Hand : MonoBehaviour
{
    [SerializeField]
    FrameRepository frameRepository = null;

    [SerializeField]
    List<Button> frameButtons = new List<Button>();

    [SerializeField]
    List<Image> frameImages = new List<Image>();

    System.Action<SituationID> onChoiseButtonCallBack = null;

    public void UpdateFrames(IEnumerable<FrameID> frameIDs)
    {
        ResetFrames();
        List<FrameID> idList = frameIDs.ToList();
        for (int i = 0; i < frameImages.Count && i < idList.Count; i++)
        {
            if (idList[i] == FrameID.NullFrame)
            {
                continue;
            }
            frameImages[i].sprite = frameRepository.GetFrameHandView(idList[i]);
        }
    }

    public void ResetFrames()
    {
        foreach(var frameImage in frameImages)
        {
            frameImage.sprite = null;
        }
    }

    public void EnableFrames(System.Action<SituationID> onChoiseButtonCallBack)
    {
        this.onChoiseButtonCallBack = onChoiseButtonCallBack;
        foreach(var frameButton in frameButtons)
        {
            frameButton.interactable = true;
        }
    }

    public void DisableFrames()
    {
        foreach (var frameButton in frameButtons)
        {
            frameButton.interactable = false;
        }
    }

    public void DisableOneFrames(SituationID situationID)
    {
        frameButtons[(int)situationID].interactable = false;
    }

    public void OnChoiseKiFrame()
    {
        onChoiseButtonCallBack(SituationID.Ki);
    }

    public void OnChoiseShoFrame()
    {
        onChoiseButtonCallBack(SituationID.Sho);
    }

    public void OnChoiseTenFrame()
    {
        onChoiseButtonCallBack(SituationID.Ten);
    }

    public void OnChoiseKetsuFrame()
    {
        onChoiseButtonCallBack(SituationID.Ketsu);
    }
}

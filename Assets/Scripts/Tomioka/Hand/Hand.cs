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

    System.Action<int> onChoiseButtonCallBack = null;

    public void UpdateFrames(IEnumerable<FrameID> frameIDs)
    {
        foreach (var frameImage in frameImages)
        {
            frameImage.sprite = null;
        }
        List<FrameID> idList = frameIDs.ToList();
        for (int i = 0; i < frameImages.Count && i < idList.Count; i++)
        {
            if (idList[i] == FrameID.NullFrame)
            {
                continue;
            }
            frameImages[i].sprite = frameRepository.GetFrameView(idList[i]);
        }
    }

    public void ResetFrames()
    {
        foreach(var frameImage in frameImages)
        {
            frameImage.sprite = null;
            frameImage.gameObject.SetActive(false);
        }
    }

    public void EnableFrames(System.Action<int> onChoiseButtonCallBack)
    {
        this.onChoiseButtonCallBack = onChoiseButtonCallBack;
        foreach(var frameButton in frameButtons)
        {
            if (!frameButton.gameObject.active)
            {
                frameButton.gameObject.SetActive(true);
            }
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

    public void OnChoiseFirstFrame()
    {
        onChoiseButtonCallBack(0);
    }

    public void OnChoiseSecondFrame()
    {
        onChoiseButtonCallBack(1);
    }

    public void OnChoiseThirdFrame()
    {
        onChoiseButtonCallBack(2);
    }
}

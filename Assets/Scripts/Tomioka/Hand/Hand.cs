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

    private void Update()
    {
        DisableFrames();
    }

    public void UpdateFrames(IEnumerable<FrameID> frameIDs)
    {
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
        }
    }

    public void EnableFrames()
    {
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
}

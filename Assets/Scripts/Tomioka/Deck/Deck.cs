using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Deck : MonoBehaviour
{
    [SerializeField]
    private FrameRepository frameRepository = null;

    [SerializeField]
    private List<int> rotationIndexs;

    private int currentRotation = 0;

    public FrameID GetFrame(FramesRoleID roleID, SituationID situationID)
    {
        FrameID retID = frameRepository.GetCurrentFramesPart(roleID, rotationIndexs[currentRotation], situationID);
        return retID;
    }

    public void Rotation()
    {
        currentRotation = (currentRotation + 1) % rotationIndexs.Count;
    }

    private int RatesToIndex(IEnumerable<float> rates)
    {
        float randomValue = Random.Range(0.0f, rates.Sum());
        int index = 0;
        foreach(float rate in rates)
        {
            randomValue -= rate;
            if (randomValue <= 0.0f)
            {
                return index;
            }
            index++;
        }
        return index;
    }
}

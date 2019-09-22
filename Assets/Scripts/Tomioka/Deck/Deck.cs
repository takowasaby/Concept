using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Deck : MonoBehaviour
{
    [SerializeField] float GoForward = 1.0f;
    [SerializeField] float GoBack = 1.0f;

    [SerializeField] float AccumulatePower = 1.0f;

    [SerializeField] float RaiseSword = 1.0f;
    [SerializeField] float Chant = 1.0f;

    [SerializeField] float CutDown = 1.0f;
    [SerializeField] float HitMagic = 1.0f;

    public FrameID GetFrame(SituationID situationID)
    {
        switch(situationID)
        {
            case SituationID.Ki:
                {
                    List<float> rates = new List<float> { GoForward, GoBack };
                    switch (RatesToIndex(rates))
                    {
                        case 0:
                            return FrameID.GoForward;
                        case 1:
                            return FrameID.GoBack;
                    }
                }
                break;
            case SituationID.Sho:
                {
                    List<float> rates = new List<float> { AccumulatePower };
                    switch (RatesToIndex(rates))
                    {
                        case 0:
                            return FrameID.AccumulatePower;
                    }
                }
                break;
            case SituationID.Ten:
                {
                    List<float> rates = new List<float> { RaiseSword, Chant };
                    switch (RatesToIndex(rates))
                    {
                        case 0:
                            return FrameID.RaiseSword;
                        case 1:
                            return FrameID.Chant;
                    }
                }
                break;
            case SituationID.Ketsu:
                {
                    List<float> rates = new List<float> { CutDown, HitMagic };
                    switch (RatesToIndex(rates))
                    {
                        case 0:
                            return FrameID.CutDown;
                        case 1:
                            return FrameID.HitMagic;
                    }
                }
                break;
        }
        return FrameID.NullFrame;
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

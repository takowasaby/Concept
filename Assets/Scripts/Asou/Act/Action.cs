using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Action : MonoBehaviour
{
    public IEnumerator ActionFrameList(int characterID, List<FrameID> frameList, UnityAction callback)
    {
        foreach (FrameID frameID in frameList)
        {
            switch (frameID)
            {
                case FrameID.GoForward:
                    if (CheckMovable(characterID, true))
                        yield return Character.characters[characterID].Move(true);
                    break;
                case FrameID.GoBack:
                    if (CheckMovable(characterID, false))
                        yield return Character.characters[characterID].Move(false);
                    break;
                default:
                    break;
            }
        }

        if (callback != null) callback();
    }

    private bool CheckMovable(int characterID, bool forward)
    {
        int allyX = Character.characters[characterID].GetX();
        float allyY = Character.characters[characterID].GetY();

        if (forward && allyY == 3) return false;
        else if (!forward && allyY == 0) return false;

        /*
        int vec = forward ? 1 : -1;

        foreach (Character character in Character.characters)
        {
            if (allyX == character.GetX() && Math.Abs(allyY + vec - character.GetY()) < 0.001)
                return false;
        }
        */

        return true;
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Action : MonoBehaviour
{
    public IEnumerator ActionFrameList(int characterID, List<int> frameList, UnityAction callback)
    {
        foreach (int frameID in frameList)
        {
            switch (frameID)
            {
                case 0:
                    if (CheckMovable(characterID, true))
                        yield return CharacterManager.characters[characterID].Move(true);
                    break;
                case 1:
                    if (CheckMovable(characterID, false))
                        yield return CharacterManager.characters[characterID].Move(false);
                    break;
                default:
                    break;
            }
        }

        if (callback != null) callback();
    }

    private bool CheckMovable(int characterID, bool forward)
    {
        int allyX = CharacterManager.characters[characterID].GetX();
        float allyY = CharacterManager.characters[characterID].GetY();

        if (forward && allyY == 3) return false;
        else if (!forward && allyY == 0) return false;

        int vec = forward ? 1 : -1;

        foreach (Character character in CharacterManager.characters)
        {
            if (allyX == character.GetX() && Math.Abs(allyY + vec - character.GetY()) < 0.001)
                return false;
        }

        return true;
    }
}

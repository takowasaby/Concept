using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Action : MonoBehaviour
{
    public void Call(Character character, List<FrameID> frameList, UnityAction callback)
    {
        StartCoroutine(ActionFrameList(character, frameList, callback));
    }

    public IEnumerator ActionFrameList(Character character, List<FrameID> frameList, UnityAction callback)
    {
        foreach (FrameID frameID in frameList)
        {
            switch (frameID)
            {
                case FrameID.GoForward:
                    if (character.GetY() < 3)
                        yield return character.Move(true);
                    break;
                case FrameID.GoBack:
                    if (character.GetY() > 0)
                        yield return character.Move(false);
                    break;
                case FrameID.AccumulatePower:
                    yield return character.AccumulatePower();
                    break;
                case FrameID.RaiseSword:
                    yield return character.RaiseSword();
                    break;
                case FrameID.CutDown:
                    yield return character.CutDown();

                    foreach (Character existCharacter in Character.characters)
                    {
                        if (character.GetIsAlly() != existCharacter.GetIsAlly() && Math.Abs(existCharacter.GetY() - character.GetY()) <= 1.5f)
                        {
                            int damage = (int)(character.GetAtk() * character.GetPhysicalEnhance() * character.GetSwordEnhance()
                                - existCharacter.GetDef());
                            existCharacter.TakeDamage(damage);
                            break;
                        }
                    }

                    character.Reset(); // 結の最後に行う処理

                    break;
                default:
                    break;
            }
        }

        if (callback != null) callback();
    }
}

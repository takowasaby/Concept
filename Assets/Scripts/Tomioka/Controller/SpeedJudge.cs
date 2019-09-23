using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class SpeedJudge : MonoBehaviour
{
    private List<Character> charactersInBattle;
    private int currentCharacter;

    public void UpdateOrder(IEnumerable<Character> characters)
    {
        charactersInBattle = characters
            .OrderBy(character => character.GetSpd())
            .Reverse()
            .ToList();
        currentCharacter = 0;
    }

    public Character Next()
    {
        Character next = charactersInBattle[currentCharacter];
        currentCharacter = (currentCharacter + 1) % charactersInBattle.Count;
        return next;
    }
}

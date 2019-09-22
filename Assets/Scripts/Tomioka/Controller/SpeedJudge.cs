using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class SpeedJudge : MonoBehaviour
{
    private List<Character> charactersInBattle;
    private int currentCharacter = 0;

    public void UpdateOrder(IEnumerable<Character> characters)
    {
        charactersInBattle = characters
            .OrderBy(character => character.GetSpd())
            .ToList();
        currentCharacter = 0;
    }

    public Character Next()
    {
        return charactersInBattle[currentCharacter];
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class SceneController : MonoBehaviour
{
    [SerializeField]
    FrameRepository frameRepository = null;

    [SerializeField]
    Deck deck = null;

    [SerializeField]
    SpeedJudge speedJudge = null;

    [SerializeField]
    FourFramesMaker fourFramesMaker = null;

    [SerializeField]
    EnemyFourFramesMaker enemyFourFramesMaker = null;

    [SerializeField]
    Action action = null;

    [SerializeField]
    List<Character> characters = new List<Character>();

    PlayerID currentPlayerID = PlayerID.First;
    Character nextActor = null;

    private void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            initializeScene();
            enterCycle();
        }
    }

    public void initializeScene()
    {
        speedJudge.UpdateOrder(characters);
        GlobalHand.Reset();
    }

    public void enterCycle()
    {
        nextActor = speedJudge.Next();
        if (nextActor.GetIsAlly())
        {
            foreach (PlayerID playerID in System.Enum.GetValues(typeof(PlayerID)))
            {
                for(int i = 0; i < 3; i++)
                {
                    FrameID newFrame = FrameID.GoForward;//deck.GetFrame(i);
                    GlobalHand.SetHand(playerID, i, newFrame);
                }
            }

            fourFramesMaker.InitializeFourFramesMaker(currentPlayerID, OnCompleteFourFrames);
            currentPlayerID = (PlayerID)(((int)currentPlayerID + 1) % 4);
        }
        else
        {
            enemyFourFramesMaker.InitializeFourFramesMaker(nextActor, OnCompleteFourFrames);
        }
    }

    public void OnCompleteFourFrames(IEnumerable<FrameID> fourFrames)
    {
        action.Call(nextActor, fourFrames.ToList(), OnEndAction);
    }

    public void OnEndAction()
    {
        enterCycle();
    }
}

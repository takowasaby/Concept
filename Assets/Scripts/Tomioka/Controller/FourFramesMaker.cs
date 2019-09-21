using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;

public class FourFramesMaker : MonoBehaviour
{
    [SerializeField]
    FourFrames fourFrames = null;

    [SerializeField]
    Hand hand = null;

    [SerializeField]
    Button handShifterLeft = null;

    [SerializeField]
    Button handShifterRight = null;

    [SerializeField]
    GameObject waitTapWindow = null;

    [SerializeField]
    Text waitWindowText = null;

    private List<SituationID> choosenFrameSituation = new List<SituationID>();
    private PlayerID currentPlayerID = PlayerID.First;
    private PlayerID displayPlayerID = PlayerID.First;
    private System.Action<IEnumerable<FrameID>> onFinishCallBack = null;

    public void InitializeFourFramesMaker(PlayerID firstPlayer, System.Action<IEnumerable<FrameID>> onFinishCallBack)
    {
        choosenFrameSituation = new List<SituationID>();
        this.currentPlayerID = firstPlayer;
        this.displayPlayerID = firstPlayer;
        this.onFinishCallBack = onFinishCallBack;
        hand.UpdateFrames(GlobalHand.GetHand(displayPlayerID));
        fourFrames.gameObject.SetActive(true);
        DisplayHand();
        handShifterLeft.interactable = true;
        handShifterRight.interactable = true;
    }

    public void OnShiftHandLeft()
    {
        displayPlayerID = PlayerIDOffset(displayPlayerID, 1);
        hand.UpdateFrames(GlobalHand.GetHand(displayPlayerID));
        DisplayHand();
    }

    public void OnChoiseFrame(SituationID situationID)
    {
        choosenFrameSituation.Add(situationID);
        FrameID frameID = GlobalHand.GetHand(currentPlayerID, situationID);
        fourFrames.AddFrame(frameID);
        if (choosenFrameSituation.Count == 4)
        {
            FinalizeFourFramesMaker();
        }
        currentPlayerID = PlayerIDOffset(currentPlayerID, 1);
        displayPlayerID = currentPlayerID;
        StartCoroutine("WaitTap", "スマホを次の人に渡したあと\nタップしてね");
    }

    private void DisplayHand()
    {
        if (displayPlayerID == currentPlayerID)
        {
            hand.EnableFrames(OnChoiseFrame);
            foreach (var situation in choosenFrameSituation)
            {
                hand.DisableOneFrames(situation);
            }
        }
        else
        {
            hand.DisableFrames();
        }
    }

    private void FinalizeFourFramesMaker()
    {
        handShifterLeft.interactable = false;
        handShifterRight.interactable = false;
        hand.ResetFrames();
        choosenFrameSituation = new List<SituationID>();
        StartCoroutine("FinalizeAct");
    }

    private PlayerID PlayerIDOffset(PlayerID playerID, int offset)
    {
        return (PlayerID)(((int)playerID + offset) % 4);
    }

    private IEnumerator WaitTap(string message)
    {
        hand.DisableFrames();

        bool leftCache = handShifterLeft.interactable;
        bool rightCache = handShifterRight.interactable;

        handShifterLeft.interactable = false;
        handShifterRight.interactable = false;

        waitTapWindow.SetActive(true);
        waitWindowText.text = message;

        yield return new WaitForSeconds(0.1f);

        while(!Input.GetMouseButtonDown(0))
        {
            yield return null;
        }

        waitWindowText.text = "";
        waitTapWindow.SetActive(false);

        handShifterLeft.interactable = leftCache;
        handShifterRight.interactable = rightCache;

        DisplayHand();
    }

    private IEnumerator FinalizeAct()
    {
        waitTapWindow.SetActive(true);
        waitWindowText.text = "四コマ漫画完成！\nスマホをみんなから見えるようにしてから\nタップしてね";

        yield return new WaitForSeconds(0.1f);

        while (!Input.GetMouseButtonDown(0))
        {
            yield return null;
        }

        waitWindowText.text = "";
        waitTapWindow.SetActive(false);

        fourFrames.DisplayFrame();

        yield return new WaitForSeconds(2.0f);

        onFinishCallBack(fourFrames.GetFrameIDs());
    }
}

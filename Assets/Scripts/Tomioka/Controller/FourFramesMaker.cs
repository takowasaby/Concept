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
    GameObject waitTapWindow = null;

    [SerializeField]
    Text waitWindowText = null;

    private List<int> choosenFrameSituation = new List<int>();
    private PlayerID currentPlayerID = PlayerID.First;
    private PlayerID displayPlayerID = PlayerID.First;
    private System.Action<IEnumerable<FrameID>> onFinishCallBack = null;

    public void InitializeFourFramesMaker(PlayerID firstPlayer, System.Action<IEnumerable<FrameID>> onFinishCallBack)
    {
        choosenFrameSituation = new List<int>();
        this.currentPlayerID = firstPlayer;
        this.displayPlayerID = firstPlayer;
        this.onFinishCallBack = onFinishCallBack;
        hand.UpdateFrames(GlobalHand.GetHand(displayPlayerID));
        fourFrames.gameObject.SetActive(true);
        DisplayHand();
    }

    public void OnChoiseFrame(int handIndex)
    {
        choosenFrameSituation.Add(handIndex);
        FrameID frameID = GlobalHand.GetHand(currentPlayerID, handIndex);
        fourFrames.AddFrame(frameID);
        if (choosenFrameSituation.Count >= 4)
        {
            FinalizeFourFramesMaker();
            return;
        }
        currentPlayerID = PlayerIDOffset(currentPlayerID, 1);
        displayPlayerID = currentPlayerID;
        StartCoroutine("WaitTap", "スマホを次の人に渡したあと\nタップしてね");
    }

    private void DisplayHand()
    {
        hand.UpdateFrames(GlobalHand.GetHand(displayPlayerID));
        if (displayPlayerID == currentPlayerID)
        {
            hand.EnableFrames(OnChoiseFrame);
        }
        else
        {
            hand.DisableFrames();
        }
    }

    private void FinalizeFourFramesMaker()
    {
        hand.DisableFrames();
        hand.ResetFrames();
        choosenFrameSituation = new List<int>();
        StartCoroutine("FinalizeAct");
    }

    private PlayerID PlayerIDOffset(PlayerID playerID, int offset)
    {
        return (PlayerID)(((int)playerID + offset + 4) % 4);
    }

    private IEnumerator WaitTap(string message)
    {
        hand.DisableFrames();

        waitTapWindow.SetActive(true);
        waitWindowText.text = message;

        yield return new WaitForSeconds(0.1f);

        while(!Input.GetMouseButtonDown(0))
        {
            yield return null;
        }

        waitWindowText.text = "";
        waitTapWindow.SetActive(false);

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

        fourFrames.ResetFrames();
        fourFrames.gameObject.SetActive(false);
        onFinishCallBack(fourFrames.GetFrameIDs());
    }
}

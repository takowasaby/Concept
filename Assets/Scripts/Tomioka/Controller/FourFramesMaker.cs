using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;

public class FourFramesMaker : MonoBehaviour
{
    [SerializeField]
    FrameRepository frameRepository = null;

    [SerializeField]
    FourFrames fourFrames = null;

    [SerializeField]
    Hand hand = null;

    [SerializeField]
    GameObject waitTapWindow = null;

    [SerializeField]
    Text waitWindowText = null;

    [SerializeField]
    Text resultDisplayText = null;

    [SerializeField]
    Timer timer = null;

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
        timer.TimerStart(System.TimeSpan.FromSeconds(15.0), () => OnChoiseFrame(0));
    }

    public void OnChoiseFrame(int handIndex)
    {
        timer.TImerStop();
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
        StartCoroutine("WaitTap", frameRepository.GetNGWords(frameID));
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

    private IEnumerator WaitTap(List<string> ngWords)
    {
        hand.DisableFrames();

        waitTapWindow.SetActive(true);
        string ngWordText = "NGワードは\n<size=144><color=#ff0000>";
        foreach(string word in ngWords)
        {
            ngWordText = ngWordText + word + "\n";
        }
        ngWordText = ngWordText + "</color></size>気を付けよう！\n";
        waitWindowText.text = ngWordText;

        yield return new WaitForSeconds(0.1f);

        while(!Input.GetMouseButtonDown(0))
        {
            yield return null;
        }

        waitWindowText.text = "スマホを次の人に渡したあと\nタップしてね";

        yield return new WaitForSeconds(0.1f);

        while (!Input.GetMouseButtonDown(0))
        {
            yield return null;
        }

        waitWindowText.text = "";
        waitTapWindow.SetActive(false);

        DisplayHand();
        timer.TimerStart(System.TimeSpan.FromSeconds(15.0), () => OnChoiseFrame(0));
    }

    private IEnumerator FinalizeAct()
    {
        waitTapWindow.SetActive(true);
        waitWindowText.text = "四コマ漫画完成！\nスマホをみんなから見えるようにしてね\nタップするごとに1コマめくれるよ！";

        yield return new WaitForSeconds(0.1f);

        while (!Input.GetMouseButtonDown(0))
        {
            yield return null;
        }

        waitWindowText.text = "";
        waitTapWindow.SetActive(false);

        fourFrames.DisplayOneFrame(0);

        yield return new WaitForSeconds(0.2f);

        while (!Input.GetMouseButtonDown(0))
        {
            yield return null;
        }

        fourFrames.DisplayOneFrame(1);

        yield return new WaitForSeconds(0.2f);

        while (!Input.GetMouseButtonDown(0))
        {
            yield return null;
        }

        fourFrames.DisplayOneFrame(2);

        yield return new WaitForSeconds(0.2f);

        while (!Input.GetMouseButtonDown(0))
        {
            yield return null;
        }

        fourFrames.DisplayOneFrame(3);

        yield return new WaitForSeconds(1.0f);

        var result = frameRepository.CheckFrames(fourFrames.GetFrameIDs().ToList());

        resultDisplayText.gameObject.SetActive(true);

        if (result != null)
        {
            string title = frameRepository.GetTitle(result.Item1, result.Item2);
            string effect = frameRepository.GetEffect(result.Item1, result.Item2);
            resultDisplayText.text = $"「{title}」\n";

            yield return new WaitForSeconds(1.0f);

            resultDisplayText.text += $"<size=160>{effect}</size>";
        }
        else
        {
            resultDisplayText.text = $"<color=#0000ff>失敗...</color>";
        }

        yield return new WaitForSeconds(2.0f);

        resultDisplayText.gameObject.SetActive(false);

        fourFrames.ResetFrames();
        fourFrames.gameObject.SetActive(false);
        onFinishCallBack(fourFrames.GetFrameIDs());
    }
}

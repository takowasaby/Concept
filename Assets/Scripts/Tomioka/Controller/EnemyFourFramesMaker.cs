using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFourFramesMaker : MonoBehaviour
{
    [SerializeField]
    FourFrames fourFrames = null;

    public void InitializeFourFramesMaker(Character enemy, System.Action<IEnumerable<FrameID>> onFinishCallBack)
    {
        fourFrames.gameObject.SetActive(true);
        foreach(var frameID in ((Enemy)enemy).GetFrameList(0))
        {
            fourFrames.AddFrame(frameID);
        }
        StartCoroutine("FinalizeAct", onFinishCallBack);
    }

    private IEnumerator FinalizeAct(System.Action<IEnumerable<FrameID>> onFinishCallBack)
    {
        fourFrames.DisplayFrame();

        yield return new WaitForSeconds(2.0f);

        fourFrames.ResetFrames();
        fourFrames.gameObject.SetActive(false);
        onFinishCallBack(fourFrames.GetFrameIDs());
    }
}

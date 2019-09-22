using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Title : MonoBehaviour
{
    public void GoBattle()
    {
        FadeManager.instance.LoadScene("Battle", 0.3f);
    }
}

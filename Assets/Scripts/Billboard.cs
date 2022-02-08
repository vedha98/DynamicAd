using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Billboard : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI p_text;
    public int score = 0;
    void OnBecameVisible()
    {
        StartCoroutine("time");
        Debug.Log("Start Timer");
    }
    void OnBecameInvisible()
    {
        StopCoroutine("time");
        Debug.Log("Stopped Timer");
    }
    IEnumerator time()
    {
        while (true)
        {
            timeCount();
            yield return new WaitForSeconds(1);
        }
    }
    void timeCount()
    {
        score += 1;
        p_text.text = score.ToString();
    }
}


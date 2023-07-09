using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreBoard : MonoBehaviour
{
    public TMPro.TMP_Text scoreText;
    private int score;

    void Start()
    {
        score = 0;
        StartCoroutine("CountTheSeconds");
    }

    void Update()
    {
    }

    IEnumerator CountTheSeconds()
    {
        while (true)
        {
            yield return new WaitForSeconds(1);
            score += 1;
            scoreText.text = score.ToString();
        }

    }
}

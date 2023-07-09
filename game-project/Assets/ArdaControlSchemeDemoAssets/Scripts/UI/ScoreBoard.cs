using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreBoard : MonoBehaviour
{
    public TMPro.TMP_Text scoreText;
    private int score;
    public AudioClip CoinSound;

    void Start()
    {
        score = 0;
        StartCoroutine("CountTheSeconds");
        GameStateManager.GameEnded += OnGameEnded;
        GameStateManager.CoinCollected += OnCoinPickedUp;
    }

    void OnGameEnded()
    {
        StopAllCoroutines();
        gameObject.SetActive(false);
        score = 0;
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

    void OnCoinPickedUp()
    {
        SoundManager.Instance.PlayEffect(CoinSound);
        score += 20;
        scoreText.text = score.ToString();
    }
}

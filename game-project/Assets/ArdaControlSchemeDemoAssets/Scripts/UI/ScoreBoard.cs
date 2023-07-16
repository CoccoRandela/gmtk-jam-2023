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
        GameStateManager.MenuEnded += ResetScore;
        GameStateManager.GameEnded += StopKeepingScore;
        GameStateManager.CoinCollected += OnCoinPickedUp;
        GameStateManager.GameMapInstantiated += KeepScore;
    }

    void KeepScore()
    {
        score = 0;
        scoreText.text = score.ToString();
        StartCoroutine("IncreaseScore");
    }

    void StopKeepingScore()
    {
        StopAllCoroutines();
    }

    void ResetScore()
    {
        score = 0;
        scoreText.text = score.ToString();
        gameObject.SetActive(false);
    }

    IEnumerator IncreaseScore()
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

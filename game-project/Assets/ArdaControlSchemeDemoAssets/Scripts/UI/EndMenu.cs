using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class EndMenu : MonoBehaviour
{

    public TMPro.TMP_Text scoreText;

    public ScoreBoard scoreBoard;

    public async void GoUp()
    {
        await transform.DOLocalMove(Vector3.zero, 1).SetEase(Ease.OutBounce).AsyncWaitForCompletion();
        scoreText.text = scoreBoard.scoreText.text;
    }
    // Start is called before the first frame update

    public void RestartGame()
    {
        Remove();
        GameStateManager.InvokeGameStartedEvent();
    }

    void Remove()
    {
        transform.DOLocalMove(new Vector3(0, -1000), 1).SetEase(Ease.OutBounce);
    }

    // Update is called once per frame
    void Update()
    {

    }
}

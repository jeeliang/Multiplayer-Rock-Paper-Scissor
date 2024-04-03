using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class GameManager : MonoBehaviourPunCallbacks
{
    public Text[] name;
    public Image player1ChoiceImage;
    public Image player2ChoiceImage;
    public Text resultText;
    public Text adviceText;
    public Text Score1;
    public Text Score2;
    public Button rockButton;
    public Button paperButton;
    public Button scissorsButton;

    public Sprite rockSprite;
    public Sprite paperSprite;
    public Sprite scissorsSprite;
    public Sprite defaultSprite;

    public Color tieColor = Color.black;
    public Color winColor = new Color(0.1f, 0.7f, 0.1f); // dark green
    public Color loseColor = new Color(0.6f, 0, 0); // maroon

    public AudioClip[] soundEffects;
    public AudioSource audioSource;

    private int player1Choice;
    private int player2Choice;
    private int totalWin1=0;
    private int totalWin2=0;

    private bool player1Ready = false;
    private bool player2Ready = false;

    private void Start()
    {
        resultText.text = null;
        player1ChoiceImage.sprite = defaultSprite;
        player2ChoiceImage.sprite = defaultSprite;
    }

    public void SetChoice(int choice)
    {
        photonView.RPC("RPC_SetChoice", RpcTarget.AllBuffered, PhotonNetwork.IsMasterClient, choice);
    }

    [PunRPC]
    private void RPC_SetChoice(bool isPlayer1, int choice)
    {
        if (isPlayer1)
        {
            player1Choice = choice;
            player1ChoiceImage.gameObject.SetActive(false);
            player1ChoiceImage.sprite = GetSprite(choice);
            player1Ready = true;
        }
        else
        {
            player2Choice = choice;
            player2ChoiceImage.gameObject.SetActive(false);
            player2ChoiceImage.sprite = GetSprite(choice);
            player2Ready = true;
        }

        if (player1Ready && player2Ready)
        {
            player1ChoiceImage.gameObject.SetActive(true);
            player2ChoiceImage.gameObject.SetActive(true);
            DetermineWinner();
        }
    }

    private Sprite GetSprite(int choice)
    {
        switch (choice)
        {
            case 1:
                return rockSprite;
            case 2:
                return paperSprite;
            case 3:
                return scissorsSprite;
            default:
                return defaultSprite;
        }
    }

    private void DetermineWinner()
    {
        if (player1Choice == player2Choice)
        {
            resultText.text = "Draw!";
            resultText.color = tieColor;
        }
        else if ((player1Choice == 1 && player2Choice == 3) || (player1Choice == 2 && player2Choice == 1) || (player1Choice == 3 && player2Choice == 2))
        {
            resultText.text = "Player 1 Wins!";
            resultText.color = winColor;
            totalWin1++;
            Score1.text = totalWin1.ToString();
        }
        else
        {
            resultText.text = "Player 2 Wins!";
            resultText.color = loseColor;
            totalWin2++;
            Score2.text = totalWin2.ToString();
        }

        StartCoroutine(StartNextRound());
    }

    IEnumerator StartNextRound()
    {
        yield return new WaitForSeconds(2f); // wait for 2 seconds before allowing players to choose again
        resultText.text = null;
        player1ChoiceImage.sprite = defaultSprite;
        player2ChoiceImage.sprite = defaultSprite;
        player1Ready = false;
        player2Ready = false;
    }

    private void PlaySoundEffect(int index)
    {
        if (index >= 0 && index < soundEffects.Length)
        {
            audioSource.clip = soundEffects[index];
            audioSource.Play();
        }
    }

    public override void OnPlayerLeftRoom(Photon.Realtime.Player otherPlayer)
    {
        player1ChoiceImage.gameObject.SetActive(false);
        player2ChoiceImage.gameObject.SetActive(false);
        if (totalWin1 > totalWin2){
            resultText.text =  name[0].text + " win the game!";
        }
        else if (totalWin2 > totalWin1){
            resultText.text =  name[1].text +" win the game!";
        }
        else{
            resultText.text = "Tied!";
        }
        resultText.color = winColor;
    }
}

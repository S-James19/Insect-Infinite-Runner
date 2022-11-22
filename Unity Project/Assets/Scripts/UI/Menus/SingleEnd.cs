using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SingleEnd : MonoBehaviour
{
    [SerializeField] private PlayerUIData _player1;
    [SerializeField] private Text _endMessage, _distance, _countdown;

    private float _totalScore;
    private void OnEnable()
    {
        GameManager.Countdown += ReturningCountdown; // subscribe to be notified when a countdown is occuring
        _totalScore = _player1.Distance * _player1.DistanceModifier; // calculate the total score of player

        _endMessage.text = "You Died!"; // set end message
        _distance.text = "Total Score: " + Mathf.RoundToInt(_player1.Distance) +  "cm "  + "x " + _player1.DistanceModifier + " = " + Mathf.RoundToInt(_totalScore); // set distance
    }

    /// <summary>
    /// Function to update the text of the countdown on screen
    /// </summary>
    /// <param name="seconds"></param>
    private void ReturningCountdown(float seconds)
    {
        _countdown.text = seconds.ToString(); // set countdown text
    }

    private void OnDisable()
    {
        GameManager.Countdown -= ReturningCountdown; // unsubscribe to be no longer be notified when a countdown is occuring
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Countdown : MonoBehaviour
{
    [SerializeField] private Text _countdownText;

    /// <summary>
    /// Function to update countdown text
    /// </summary>
    /// <param name="value"> The new number to show on the countdown timer text </param>
    public void CountdownText(float value)
    {
        int textValue = Mathf.RoundToInt(value); // round the float to an integer
        _countdownText.text = textValue.ToString(); // display new float
    }

    private void OnEnable()
    {
        GameManager.Countdown += CountdownText; // subscribe to be notified when there is a countdown occuring
    }

    private void OnDisable()
    {
        GameManager.Countdown -= CountdownText;  // un subscribe to be no longer notified when there is a countdown occuring
    }
}

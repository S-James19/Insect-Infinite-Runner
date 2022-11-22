using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
public class SingleGameUI : MonoBehaviour
{
    [SerializeField] private Text _distance;
    [SerializeField] private Text _speed;
    [SerializeField] private PlayerUIData _player1;
    private void Start()
    {
        _distance.text = "Distance: 0 cm";
        _distance.text = "Speed: x1";
    }
    private void Update()
    {
        _distance.text = "Distance: " + Mathf.RoundToInt(_player1.Distance).ToString() + " cm";
        _speed.text = "Speed: x" + Math.Round(_player1.Speed, 2).ToString();
    }

}

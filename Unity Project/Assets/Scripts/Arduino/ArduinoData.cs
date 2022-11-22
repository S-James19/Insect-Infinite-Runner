using UnityEngine;
using Uduino;

/*
 * This scipt came as part of the https://marcteyssier.com/uduino/projects/connect-a-imu-to-unity package
 * The script has been modifed to meet the needs of my program.
 */

public class ArduinoData : MonoBehaviour 
{
    [SerializeField] int id; // id of player

    private float _xTilt, _yTilt; // tilt values
    private bool _isHandsOn; // are hands on

    // event for sending out data
    public delegate void UseData(float x, float y, int id);
    public static event UseData DataRecieved; 

    /// <summary>
    /// Recieves data from Arudino inside controller and sends required data to required scripts
    /// </summary>
    /// <param name="data"> The data being passed in from Arduino </param>
    /// <param name="device"> The name of the Arduino device </param>
    public void ReadIMU (string data, UduinoDevice device)
    {
        //spilt up data
        string[] recievedData = data.Split('/');

        // save data into appropriate variables
        _xTilt = float.Parse(recievedData[0]);
        _yTilt = float.Parse(recievedData[1]);
        int hands = int.Parse(recievedData[2]);

        //check if hand input is present
        if (hands == 1)
        {
            _isHandsOn = true;
            GameManager.Instance?.AddHands(gameObject);
        }
        else
        {
            _isHandsOn = false;
            GameManager.Instance?.RemoveHands(gameObject);
        }

        // call event for any script that needs to use this data
        DataRecieved?.Invoke(_xTilt, _yTilt, id);
    }
}

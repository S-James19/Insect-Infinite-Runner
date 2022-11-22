using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InsectMovement : MonoBehaviour
{
    // event to let subscribers know when the player is hiding
    public delegate void Hiding(HidingStatus status);
    public static event Hiding Hidden;

    [SerializeField] private GameSetting _gameSetting;
    [SerializeField] private int id;
    [SerializeField] private SettingsSO _settings;
    [SerializeField] private LanesSO _playerLaneData;
    [SerializeField] private SpawnData _spawn;
    [SerializeField] private PlayerUIData _UI;

    [SerializeField] [Tooltip("Movement speed of the insect/player")]
    private float _speed = 2f; // speed of insect
    private float _runningSpeed;

    [SerializeField] [Tooltip("How much to increase the velocity multiplier by every interval")]
    private float _velocityMultiplierIntervals = 0.05f;
    [SerializeField] [Tooltip("Time between increasing velocity multiplier (seconds)")]
    private float _timeBetweenIntervals = 10f;
    [SerializeField] private float _hideDistance;
    [SerializeField] private KeyCode _left, _right, _forward, _backward;
    [SerializeField] private float _switchLaneTime = 1f;
    [SerializeField] private float _switchLaneSpeed = 1f;
    public bool IsHiding { get; private set; }
    public bool IsHidden { get; private set; }
    public HidingStatus InsectHidingStatus { get; private set; }

    private bool _isLaneSwitching = false;
    private bool _savedModifierTime;

    private Vector3 _lanePosition, _startingPosition;

    private Lane _switchingToLane, _previousLane;
    private Lane _currentLane = Lane.center;

    private float _groundPoint;
    private float _lastLaneSwitch;
    private float _lastIntervalTime, _lastIntervalHidingTime;
    public enum Lane // state of the lane the player is currently in
    {
        left, 
        center,
        right
    }

    private void OnEnable()
    {
        ArduinoData.DataRecieved += MoveInsect; // subscribe to receive data from Arduino
    }
    private void OnDisable()
    {
        ArduinoData.DataRecieved -= MoveInsect; // un subscribe to no longer receive data from Arduino
    }

    private void Awake()
    {
        _runningSpeed = _speed; // set the speed of the player during the game to the set speed
    }
    private void Start()
    {
        InsectHidingStatus = HidingStatus.none; // set the hiding status of the player to none
        _UI.Speed = _gameSetting.GameSpeed; // set the speed of the UI to the game setting
        _lastLaneSwitch = Time.time; // save time of last lane switch
        _lanePosition = transform.position; // saving the initial position of the player
        _startingPosition = _lanePosition; // setting starting position of player

        //setting all lanes to default value
        _previousLane = _currentLane; 
        _switchingToLane = _currentLane;

        _lastIntervalTime = Time.time; // set the time of last speed increase to current time.
    }
    private void Update()
    {
        _runningSpeed = _speed * _gameSetting.GameSpeed; // update the running speed of the player to the game speed

        if(GameManager.Instance.controllerType == GameManager.ControllerState.keyboard) // if the current game state is keyboard
        {
            MoveInsect(0, 0, id); // run function to move
        }
    }

    /// <summary>
    /// Functionality of player when data is recieved / when being told to move
    /// </summary>
    /// <param name="x"> The x tilt value from the Arduino </param>
    /// <param name="y"> The y tilt value from the Arduino </param>
    /// <param name="id"> The id of the player </param>
    private void MoveInsect(float x, float y, int id)
    {
        if(id != this.id) // if the event id is not the correct player
        {
            return; // do not run code to move
        }

        Hidden?.Invoke(InsectHidingStatus); // let subscribers know the current hiding status

        if(!IsHiding) // if player is not hiding
        {
            _groundPoint = transform.position.y; // set the point where they are touching the ground to the current y position
        }

        CalculateVelocityIncrease(); // check to see if speed modifier is to be increased
        if(GameManager.Instance.gameState == GameManager.GameState.Playing) // if the game is currently running
        {
            if (!_isLaneSwitching) // if not switching lanes
            {
                if (!IsHidden) // if they are not hiding
                {
                    if ((x > _settings.TiltSensitivity || (Input.GetKeyDown(KeyCode.A) && GameManager.Instance.controllerType == GameManager.ControllerState.keyboard)) && _currentLane != Lane.left) // if want to move left and can move left
                    {
                        _isLaneSwitching = true; // set the lane switching status to true
                        _lanePosition.x -= _playerLaneData.Width; // set new lane position
                        if (_lanePosition.x == _startingPosition.x) // if lane position is same as starting position
                        {
                            _switchingToLane = Lane.center; // they are in the center lane
                        }
                        else _switchingToLane = Lane.left; // they are in the left lanes
                    }
                    else if ((x < -1 * (_settings.TiltSensitivity) || (Input.GetKeyDown(KeyCode.D) && GameManager.Instance.controllerType == GameManager.ControllerState.keyboard)) && _currentLane != Lane.right) // if want to move right and can move right
                    {
                        _isLaneSwitching = true;
                        _lanePosition.x += _playerLaneData.Width; // set new lane position
                        if (_lanePosition.x == _startingPosition.x) // if lane position is same as start
                        {
                            _switchingToLane = Lane.center; // they are in the center
                        }
                        else _switchingToLane = Lane.right; // they are in the right
                    }
                }

                if (((x > _settings.TiltSensitivity || (Input.GetKey(KeyCode.A) && GameManager.Instance.controllerType == GameManager.ControllerState.keyboard)) && (_currentLane == Lane.left))) // if x value is bigger than the boundary
                {
                    IsHiding = true; // set hiding status to true
                    InsectHidingStatus = HidingStatus.left; // set hiding state to left
                    HideInShrubs(_currentLane); // move left
                }
                else if ((x < -1 * (_settings.TiltSensitivity) || (Input.GetKey(KeyCode.D) && GameManager.Instance.controllerType == GameManager.ControllerState.keyboard)) && (_currentLane == Lane.right)) // if the x values is less than the boundary
                {
                    IsHiding = true; // set hiding status to true
                    InsectHidingStatus = HidingStatus.right; // set hiding state to right
                    HideInShrubs(_currentLane); // move right
                }
                else if (y > _settings.TiltSensitivity || (Input.GetKey(KeyCode.S) && GameManager.Instance.controllerType == GameManager.ControllerState.keyboard)) // if the z value is bigger than the boundary
                {
                    IsHiding = true; // set hiding status to true
                    InsectHidingStatus = HidingStatus.under; // set hiding state to underground
                    HideUnderground(); // hide underground
                }
                else if (y < -1 * (_settings.TiltSensitivity) || (Input.GetKey(KeyCode.W) && GameManager.Instance.controllerType == GameManager.ControllerState.keyboard)) // if the z value is less than the boundary
                {
                    InsectHidingStatus = HidingStatus.stop; // set hiding state to stop movement
                    IsHidden = true; // set hiding status to true
                }
                else // if no keys are being pressed or Arduino tilt does not meet any criteria
                {
                    if (IsHidden) // if they are currently hidden
                    {
                        InsectHidingStatus = HidingStatus.none; // set the hiding status of the plater to not hiding at all
                        transform.position = new Vector3(_lanePosition.x, _groundPoint, transform.position.z); // reset position along the path
                        transform.localRotation = Quaternion.Euler(0f, 0f, 0f); // set the rotation to default

                        //reset boolean values
                        IsHiding = false;
                        IsHidden = false;
                    }
                }
            }
        }
    }
    private void FixedUpdate()
    {
        if (!IsHidden) // if they are moving and not hiding
        {
            MoveForward(); // more forward

            if (_isLaneSwitching) // if player has changed lanes
            {
                if (Time.time > _lastLaneSwitch + _switchLaneTime) // if time between switching lanes has been passed
                {
                    SwitchLanes(_switchingToLane); // switch to new lane
                }
            }
        }
    }

    /// <summary>
    /// Function to switch the player into a new lane
    /// </summary>
    /// <param name="lane"> The lane that the player wants to switch to </param>
    private void SwitchLanes(Lane lane)
    {
        if(lane < _previousLane) // if the lane is on the left
        {
            if (!(transform.position.x < _lanePosition.x)) // if they have not moved over fully to the lane
            {
                transform.Translate(Vector3.right * -1 * _switchLaneSpeed * _gameSetting.GameSpeed * Time.fixedDeltaTime); // move left
            }
            else SetNewLane();
        }
        else if (lane > _previousLane) // if the lane is on the right
        {
            if (!(transform.position.x > _lanePosition.x)) // if they have not moved fully over to the lane
            {
                transform.Translate(Vector3.right * _switchLaneSpeed * _gameSetting.GameSpeed * Time.fixedDeltaTime); // move right
            }
            else SetNewLane();
        }
    }

    /// <summary>
    /// Function to update the data when moving into a new lane
    /// </summary>
    private void SetNewLane()
    {
        _isLaneSwitching = false; // player has stopped switching lanes
        _currentLane = _switchingToLane; // set the curret lane to the lane that was being switched to 
        _previousLane = _currentLane; // set new previous lane for next switch 
        _lastLaneSwitch = Time.time; // save current time of lane switch completion
    }

    /// <summary>
    /// Function to hide the player in grass shrubs
    /// </summary>
    /// <param name="lane"> The current lane of the player </param>
    private void HideInShrubs(Lane lane)
    {
        if(!IsHidden) // if not already hidden but wants to hide
        {
            if(lane == Lane.left) // if player is in left lane
            {
                transform.position = transform.position + new Vector3(-_hideDistance, 0f, 0f); // move position 
                transform.localRotation = Quaternion.Euler(0f, 90f, 0f); // set rotation
            }
            else if (lane == Lane.right) // if player is in right lane
            {
                transform.position = transform.position + new Vector3(_hideDistance, 0f, 0f); // move position 
                transform.localRotation = Quaternion.Euler(0f, -90f, 0f); // set rotation
            }
            IsHidden = true; // is hiding
        }
    }

    /// <summary>
    /// Function to hide the player underground
    /// </summary>
    private void HideUnderground()
    {
        if(!IsHidden) // if not already hidden but wants to hide
        {
            IsHidden = true; // set hiding bool to true
            transform.position = transform.position - new Vector3(0f, _hideDistance, 0f); // move position underground
        }
    }

    /// <summary>
    /// Function to move the position of the player forwards
    /// </summary>
    private void MoveForward() // function to move player forward
    {
        if(!IsHiding) // if they are not hiding
        {
            transform.Translate(Vector3.forward * _runningSpeed * Time.fixedDeltaTime); // move forward
        }
    }

    /// <summary>
    /// Function to increase the speed of the player over time
    /// </summary>
    private void CalculateVelocityIncrease()
    {
        if(!IsHiding) // if the player is not hiding
        {
            _savedModifierTime = false;
            if (_gameSetting.GameSpeed < _gameSetting.MaxGameSpeed) // if velocity multiplier has not reached the maximum
            {
                if (Time.time > _lastIntervalTime + _timeBetweenIntervals) // if the intervals between increasing has passed
                {
                    _gameSetting.GameSpeed += _velocityMultiplierIntervals; // increase velocity multiplier by how much per interval
                    _UI.Speed = _gameSetting.GameSpeed;
                    _lastIntervalTime = Time.time; // set new time for last increase
                }
            }
        }
        else // if the player is hiding
        {
            if(!_savedModifierTime)
            {
                _lastIntervalHidingTime = Time.time - _lastIntervalTime; // how many seconds along interval it covered
                _savedModifierTime = true; 
            }
            else _lastIntervalTime = Time.time - _lastIntervalHidingTime; // new last interval time, to stop increasing modifier when hiding.
        }
    }
}

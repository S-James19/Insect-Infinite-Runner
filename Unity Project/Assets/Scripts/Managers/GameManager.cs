using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    //event to let the countdown script know that there is a countdown occuring in game
    public delegate void CountdownEvent(float value);
    public static event CountdownEvent Countdown;

    [SerializeField] private GameSetting _gameSetting;
    [SerializeField] private float _timeBeforePausing = 2f;
    [SerializeField] private float _unpauseTimer, _reloadSceneTimer;
    private float _lastPausingTime, _lastSecond, _timer;

    public static int PlayerLayer = 6; // layer of 
    public static GameManager Instance;
    private List<GameObject> _Hands = new List<GameObject>();
    private bool _isPaused, _pausingTimer, _isResuming, _canCount;
    public GameState gameState { get; set; } // state of arduino
    public ControllerState controllerType; // state of controller
    public KeyCode Pause;
    public enum GameState // enum storing different game states
    {
        Menu,
        Playing,
        Paused,
        Unpausing,
        GameOver
    }
    public enum ControllerState // enum storing state of controller
    {
        controller,
        keyboard
    }
    private void Awake()
    {
        _gameSetting.GameSpeed = _gameSetting.BaseGameSpeed; // set the game speed to the base game speed

        if (Instance != null && Instance != this) // if other gamemanager object exists
        {
            Destroy(gameObject); // destroy this version
        }
        else Instance = this; // make this the static reference

        UpdateGameState(GameState.Menu); // set gamestate to playing
        _isResuming = false; // game is not resuming
        PauseGame(); // pause game
    }

    /// <summary>
    /// Function to update the game state
    /// </summary>
    /// <param name="state"> The state to be updated to </param>
    public void UpdateGameState(GameState state) // function to update state of port connection
    {
        gameState = state; // set state to passed in state
    }
    private void Update()
    {
        if (gameState == GameState.GameOver) // if the gamestate is game over
        {
            CountdownTimer(_reloadSceneTimer); // start a countdown to reload the scene
            Time.timeScale = 0f; // pause game
        }

        if (Players.Instance.players.Count == 0) // if no players are left alive
        {
            UpdateGameState(GameState.GameOver); // end the game
            return;
        }

        if (gameState == GameState.Unpausing) // if game is unpausing
        {
            CountdownTimer(_unpauseTimer); // start a countdown to unpause game

            if(_pausingTimer) // is is pausing is true
            {
                _pausingTimer = false; // set it to false
            }
            return; // dont run any code below
        }
        else if(gameState == GameState.Playing) // if player is playing
        {
            if(_isPaused) // if game is currently paused
            {
                ResumeGame(); // resume game
            }
        }

        if(_pausingTimer) // if pausing timer is activated
        {
            if(Time.time > _lastPausingTime + _timeBeforePausing) // if time has passed for pause deadline
            {
                UpdateGameState(GameState.Paused); // set gamestate to paused
                PauseGame(); // pause the game
            }
        }

        if (controllerType == ControllerState.controller) // if it is a controller
        {
            if(_Hands.Count == Players.Instance.players.Count) // if all hands are on controller
            {
                if (gameState == GameState.Paused || gameState == GameState.Menu) // if game is paused
                {
                    UpdateGameState(GameState.Unpausing); // set gamesate to unpausing
                }
                else _pausingTimer = false;
            }
            else // if not all hands are on controller
            {
                if(!_isPaused) // if game is playing
                {
                    if(!_pausingTimer) // if no pausing timer has yet been set
                    {
                        _pausingTimer = true; // pausing timer is true
                        _lastPausingTime = Time.time; // set time to current time
                    }
                }
            }
        }
        else // if is keyboard
        {
            if(Input.GetKeyDown(Pause)) // if want to pause
            {
                if(_isPaused) // if already paused
                {
                    UpdateGameState(GameState.Unpausing); // set state to unpausing
                }
                else
                {
                    UpdateGameState(GameState.Paused); // set state to paused
                    PauseGame(); // pause game
                }
            }
        }
    }

    /// <summary>
    /// Function to add a player with hands on controller to list of players with hands on controller
    /// </summary>
    /// <param name="player"> The gameobject of the player who has hands on controller </param>

    public void AddHands(GameObject player) // function to add players with hands on controller to a list
    {
        if(!_Hands.Contains(player)) // if the list does not contain the player
        {
            _Hands.Add(player); // add the player to the list
        }
    }
    /// <summary>
    /// Function to remove a player with no hands on controller to list of players with no hands on controller
    /// </summary>
    /// <param name="player"> The gameobject of the player who has no hands on controller </param>
    public void RemoveHands(GameObject player)
    {
        if(_Hands.Contains(player)) // if list contains the player
        {
            _Hands.Remove(player); // remove the player from the list
        }
    }

    /// <summary>
    /// Function to pause the game
    /// </summary>
    public void PauseGame()
    {
        Time.timeScale = 0f; // pause game
        _isPaused = true; // set pause status to true
    }
    
    /// <summary>
    /// Function to resume game
    /// </summary>
    public void ResumeGame()
    {
        Time.timeScale = 1.0f; // unpause game speed
        _isPaused = false; // set paused status to false
    }

    /// <summary>
    /// Function to countdown a timer
    /// </summary>
    /// <param name="timer"> The time to be count down from </param>
    private void CountdownTimer(float timer)
    {
        if (!_isResuming) // if game has not started to resume
        {
            if (gameState == GameManager.GameState.Unpausing || gameState == GameState.GameOver) // if the gamestate is unpausing
            {
                _isResuming = true; // player is trying to play
                _lastSecond = Time.unscaledTime; // save time of when the countdown starts
            }
        }
        else // if resuming game
        {
            if (!_canCount) // if timer not been setup since last pause
            {
                _timer = timer; // set timer to maximum time
                Countdown?.Invoke(_timer); // let subscribers know of counting down
                _canCount = true; // start counting down
            }
            else
            {
                if (Time.unscaledTime > _lastSecond + 1) // if 1 second has passed
                {
                    _timer--; // remove 1 from timer
                    Countdown?.Invoke(_timer); // let subscribers know of counting down
                    _lastSecond = Time.unscaledTime; // set new time for last update

                    if (_timer <= 0) // if timer has been completed
                    {
                        if(gameState == GameState.Unpausing)
                        {
                            UpdateGameState(GameManager.GameState.Playing); // update gamestate
                        }
                        else if(gameState == GameState.GameOver) // if the game is over
                        {
                            SceneManager.LoadScene("MainScene"); // load the main scene to restart game
                        }
                        //reset boolean values
                        _isResuming = false;
                        _canCount = false;
                    }
                }
            }
        }
    }
}

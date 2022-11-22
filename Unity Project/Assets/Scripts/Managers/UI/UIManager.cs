using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject _pauseMenu, _gameUI, _countdown, _mainMenu, _endScreen; // pause & game UI gameobjects

    public static int UILayer = 5; 
    public static UIManager UImanager;
    private bool _isPaused;

    private void Awake()
    {
        if (UImanager != null && UImanager != this) // if there is another UI manager in scene
        {
            Destroy(gameObject); // destroy this verion
        }
        else UImanager = this; // else this is the UI manager
    }

    private void Start()
    {
        SetupUI(); // setup the UI at start of game

        if(GameManager.Instance.gameState == GameManager.GameState.Menu) // if the gamestate is menu
        {
            _isPaused = true; // set the game to paused
        }
    }
    private void Update()
    {
        if(GameManager.Instance.gameState == GameManager.GameState.Paused) // if game is currently paused
        {
            if(!_isPaused) // if not already pause UI
            {
                PauseGame(); // setup pause UI
            }
        }
        else if(GameManager.Instance.gameState == GameManager.GameState.Playing) // if game is currently playing
        {
            if(_isPaused) // if is paused
            {
                ResumeGame(); // resume game
            }
        }
        else if(GameManager.Instance.gameState == GameManager.GameState.Unpausing) // if the game is unpausing
        {
            foreach(Transform child in gameObject.transform) // for each UI element in UI Manager
            {
                if (child.gameObject != _countdown) // if UI is not the countdown
                {
                    child.gameObject.SetActive(false); // disable
                }
                else child.gameObject.SetActive(true); // else enable
            }
        }
        else if(GameManager.Instance.gameState == GameManager.GameState.GameOver) // if the gamestate is game over
        {
            SetActiveUI(_endScreen); // set the endscreen UI to active
        }
    }

    /// <summary>
    /// Function to pause the game
    /// </summary>
    private void PauseGame()
    {
        SetActiveUI(_pauseMenu); // set the pausemenu UI to active
        _isPaused = true; // set the pause status to true
    }

    /// <summary>
    /// Function to resume the game
    /// </summary>
    private void ResumeGame()
    {
        SetActiveUI(_gameUI); // set the game UI to active
        _isPaused = false; // set the pause status to false
    }

    /// <summary>
    /// Function to setup UI at start of game
    /// </summary>
    private void SetupUI()
    {
        SetActiveUI(_mainMenu); // set the main menu UI to active
    }

    /// <summary>
    /// Function to activate the passed in UI
    /// </summary>
    /// <param name="selected"> The UI in which the game wants to activate </param>
    private void SetActiveUI(GameObject selected)
    {
        foreach(Transform child in transform) // for each child in the gameobject
        {
            if (child.gameObject == selected) // if the child is the UI
            {
                child.gameObject.SetActive(true); // set the UI to acitve
            }
            else child.gameObject.SetActive(false); // else disable the UI
        }
    }
}

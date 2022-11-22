using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menu : MonoBehaviour
{
    [SerializeField] private SettingsSO _settings;
    [SerializeField] private GameObject _left, _right, _up, _down, _home;
    [SerializeField] private static int _id = 1;

    private bool _isEnabled, _canSwitch;
    public MenuState _currentState;
    public enum MenuState // state of the menu
    {
        Home,
        Left,
        Right,
        Up,
        Down
    }
    private void OnEnable()
    {
        _canSwitch = true; // menu can be switched
        _isEnabled = true; // menu is enabled
        _home.SetActive(true); // set the default menu to active
        _currentState = MenuState.Home; // set the current state to be home
        ArduinoData.DataRecieved += NavigateMenu; // subscribe to recieve data from Arduino script.

        foreach(Transform UI in gameObject.transform) // for each UI
        {
            if (UI.gameObject != _home) // if UI is not the home
            {
                UI.gameObject.SetActive(false); // deactivate
            }
            else UI.gameObject.SetActive(true); // activate
        }
    }
    private void OnDisable()
    {
        ArduinoData.DataRecieved-= NavigateMenu; // un subscribe to no longer recieve data from Arduino script.
    }

    private void Update()
    {
        if(!_isEnabled) // if menu is not enabled
        {
            ArduinoData.DataRecieved -= NavigateMenu; // unsubscribe to no longer recieve data from Arduino script.
        }

        if (GameManager.Instance.controllerType == GameManager.ControllerState.keyboard && _isEnabled) // if controller type is keyboard and is enabled
        {
            NavigateMenu(0f, 0f, _id); // run code to naviage menu
        }
    }

    /// <summary>
    /// Function to allow navigation of the menu
    /// </summary>
    /// <param name="x"> The x tilt from Arduino </param>
    /// <param name="y"> The y tilt from Arduino </param>
    /// <param name="id"> The id of the player </param>
    public void NavigateMenu(float x, float y, int id)
    {
        if(_id != id) // if the player sending data is not correct player
        {
            return; // dont run any code below
        }

        if ((x > _settings.TiltSensitivity) || Input.GetKeyDown(KeyCode.A)) // move left
        {
            if(_canSwitch) // if can switch menus
            {
                if (_currentState == MenuState.Right) // if they are currently on the right option
                {
                    _home.SetActive(true); // set home to true
                    _right.SetActive(false); // set right to false
                    _currentState = MenuState.Home; // set the current state to home
                }
                else if (_currentState == MenuState.Home) // if they are currently on the home option
                {
                    if (_left != null) // if there is a left option available to go to
                    {
                        _left.SetActive(true); // set the left option to true
                        _currentState = MenuState.Left; // set the menu state to left
                        CheckForSubMenu(_left); // check if there is a sub menu on that tab
                    }
                }
                _canSwitch = false; // cannot switch again
            }
        }
        else if ((x < -1 * _settings.TiltSensitivity) || Input.GetKeyDown(KeyCode.D)) // move right
        {
            if(_canSwitch) // if can switch
            {
                if (_currentState == MenuState.Left) // if the current menu is left
                {
                    _home.SetActive(true); // set the home option to truw
                    _left.SetActive(false);// set the left option to false
                    _currentState = MenuState.Home; // set the menu state to home
                }
                else if (_currentState == MenuState.Home) // if the current menu state is home
                {
                    if (_right != null) // if there isa right option available to go to
                    {
                        _right.SetActive(true); // set the right menu to true
                        _currentState = MenuState.Right; // set the menu state to right
                        CheckForSubMenu(_right); // check to see if right is it's own menu.
                    }
                }
                _canSwitch = false;
            }

        }
        else if ((y < -1 * _settings.TiltSensitivity) || Input.GetKeyDown(KeyCode.W)) // move up
        {
            if(_canSwitch) // if can switch 
            {
                if (_currentState == MenuState.Down) // if the current menu state is down
                {
                    _home.SetActive(true); // set home to true
                    _down.SetActive(false); // deactivate down menu
                    _currentState = MenuState.Home; // set current menu state to home
                }
                else if (_currentState == MenuState.Home) // if the menu state is home
                {
                    if (_up != null) // if there is an up option available to go to
                    {
                        _up.SetActive(true); // set up to true
                        _currentState = MenuState.Up; // set current menu state to up
                        CheckForSubMenu(_up); // check to see if up is it's own menu
                    }
                }
                _canSwitch = false;
            }
        }
        else if ((y > _settings.TiltSensitivity) || Input.GetKeyDown(KeyCode.S)) // move down
        {
            if(_canSwitch) // if can switch
            {
                if (_currentState == MenuState.Up) // if current menu state is up
                {
                    _home.SetActive(true); // set home to true
                    _up.SetActive(false); // set up to false
                    _currentState = MenuState.Home; // set current menu state to home
                }
                else if (_currentState == MenuState.Home) // if menu is currently home
                {
                    if (_down != null) // if there is a down option available to go to
                    {
                        _down.SetActive(true); // set down to true
                        _currentState = MenuState.Down; // set the current menu state to down
                        CheckForSubMenu(_down); // check to see if down is its own menu
                    }
                }
                _canSwitch = false;
            }
        }
        else _canSwitch = true;
    }

    /// <summary>
    /// Function to check if the menu option is it's own sub menu
    /// </summary>
    /// <param name="newMenu"> The menu option to check for </param>
    private void CheckForSubMenu(GameObject newMenu) // function to check if new menu is sub menu
    {
        if (newMenu.transform.childCount > 1) // if it has menu options
        {
            _isEnabled = false; // disable this menu
            DisableMenu(newMenu);
        }
        _home.SetActive(false); // deactivate home option
    }

    /// <summary>
    /// Function to disable all menu options apart from new menu
    /// </summary>
    /// <param name="newMenu"> The menu to be disabled</param>
    private void DisableMenu(GameObject newMenu)
    {
        bool isAChild = false;

        foreach(Transform child in gameObject.transform) // for each child of menu
        {
            if(child.gameObject == newMenu) // if the child  is the new menu
            {
                isAChild = true; // menu is a child
            }
        }

        if(!isAChild) // if the menu is not a child
        {
            gameObject.SetActive(false); // set the gameobject to false
            newMenu.SetActive(false); // deactivate new menu
            newMenu.SetActive(true); // activate new menu
        }
    }
}

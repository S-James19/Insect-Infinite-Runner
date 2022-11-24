using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
    The code in this script has used data from OpenWeather, which is licensed under the CC BY-SA 4.0 https://creativecommons.org/licenses/by-sa/4.0/  
*/
public class WeatherSettings : MonoBehaviour
{
    [SerializeField] private WeatherType _defaultWeather;
    [SerializeField] private Material _snowySky, _rainySky, _cloudySky, _sunnySky;
    [SerializeField] private GameObject _players;
    [SerializeField] private GameObject _rain, _snow;
    [SerializeField] private RealWorldWeather _weatherData;
    public WeatherType _weatherType { get; private set; }
    private static WeatherType _previousWeather;
    public static WeatherSettings Instance;
    private void Awake()
    {
        Debug.Log(_previousWeather);
        _weatherType = _defaultWeather; // set the weather type to the default weather
        if(Instance != null && Instance != this) // if there is already a weather settings in scene
        {
            Destroy(gameObject); // destroy this version
        }
        else Instance = this; // else this is the weather settings
    }
    public enum WeatherType // enum storing the type of weather
    {
        rain,
        snow,
        sun,
        clouds
    }

    private void Start()
    {
        bool isWeather = false; // too see if new weather has been found
        float temperature = 0; // temperature of specified location

        if (_weatherData._foundWeather) // if there is a weather found from API
        {
            temperature = _weatherData._weather.temperature; // get the temperature
            isWeather = true; // there is weather
        }

        if((temperature < 0f && isWeather) || (_previousWeather == WeatherType.snow && !isWeather)) // snow
        {
            Snowing();
        }
        else if(((temperature > 0f && temperature < 12f) && isWeather) || (_previousWeather == WeatherType.rain && !isWeather)) // rain
        {
            Raining();
        }
        else if (((temperature > 12f && temperature < 18f) && isWeather) || (_previousWeather == WeatherType.clouds && !isWeather)) // clouds
        {
            Cloudy();
        }
        else // sunny
        {
            Sunny();
        }
    }
    /// <summary>
    /// Function to update scene when raining
    /// </summary>
    private void Raining()
    {
        RenderSettings.skybox = _rainySky;
        _weatherType = WeatherType.rain;
        _previousWeather = _weatherType;
        SpawnParticleEffect(_rain);

    }

    /// <summary>
    /// Function to update scene when snowing
    /// </summary>
    private void Snowing()
    {
        RenderSettings.skybox = _snowySky;
        _weatherType = WeatherType.snow;
        _previousWeather = _weatherType;
        SpawnParticleEffect(_snow);
    }

    /// <summary>
    /// Function to update scene when sunny
    /// </summary>

    private void Sunny()
    {
        RenderSettings.skybox = _sunnySky;
        _weatherType = WeatherType.sun;
        _previousWeather = _weatherType;
    }

    /// <summary>
    /// Function to update scene when cloudy
    /// </summary>
    private void Cloudy()
    {
        RenderSettings.skybox = _cloudySky;
        _weatherType = WeatherType.clouds;
        _previousWeather = _weatherType;
    }

    /// <summary>
    /// Function to spawn in weather particle effects
    /// </summary>
    /// <param name="effect"> The gameobject of the weather effect</param>
    private void SpawnParticleEffect(GameObject effect)
    {
        foreach (Transform child in _players.transform) // for each player in scene
        {
            GameObject particleEffect = Instantiate(effect, child.transform.position, Quaternion.identity); // spawn in rain particle effect

            particleEffect.transform.parent = child.transform; // set the parent of the effect to be the player
            particleEffect.transform.localPosition = new Vector3(0f, 15f, 0f); // set the position of the effect relative to the player
        }
    }
}

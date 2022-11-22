using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeatherSettings : MonoBehaviour
{
    [SerializeField] private WeatherType _defaultWeather;
    [SerializeField] private Material _snowySky, _rainySky, _cloudySky, _sunnySky;
    [SerializeField] private GameObject _players;
    [SerializeField] private GameObject _rain, _snow;
    [SerializeField] private RealWorldWeather _weatherData;
    public WeatherType _weatherType { get; private set; }
    public static WeatherSettings Instance;
    private void Awake()
    {
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
        if(_weatherData._foundWeather) // if there is a weather found from API
        {
            float temperature = _weatherData._weather.temperature; // get the temperature

            if(temperature < 0f) // snow
            {
                Snowing();
            }
            else if(temperature > 0f && temperature < 12f) // rain
            {
                Raining();
            }
            else if (temperature > 12f && temperature < 18f) // clouds
            {
                Cloudy();
            }
            else // sunny
            {
                Sunny();
            }
        }
    }
    /// <summary>
    /// Function to update scene when raining
    /// </summary>
    private void Raining()
    {
        RenderSettings.skybox = _rainySky;
        _weatherType = WeatherType.rain;
        SpawnParticleEffect(_rain);

    }

    /// <summary>
    /// Function to update scene when snowing
    /// </summary>
    private void Snowing()
    {
        RenderSettings.skybox = _snowySky;
        _weatherType = WeatherType.snow;
        SpawnParticleEffect(_snow);
    }

    /// <summary>
    /// Function to update scene when sunny
    /// </summary>

    private void Sunny()
    {
        RenderSettings.skybox = _sunnySky;
        _weatherType = WeatherType.sun;
    }

    /// <summary>
    /// Function to update scene when cloudy
    /// </summary>
    private void Cloudy()
    {
        RenderSettings.skybox = _cloudySky;
        _weatherType = WeatherType.clouds;
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

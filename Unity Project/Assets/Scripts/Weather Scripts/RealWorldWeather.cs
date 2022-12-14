using System;
using System.Collections;
using Newtonsoft.Json.Linq;
using UnityEngine;
using UnityEngine.Networking;

/*
	All source code in this script has come from this video: https://www.youtube.com/watch?v=x53swDdRVOk
*/
public class RealWorldWeather : MonoBehaviour {


	[SerializeField] private string city;
	[SerializeField] private bool useLatLng = false;
	[SerializeField] private string latitude;
	[SerializeField] private string longitude;
	public WeatherStatus _weather { get; private set; }
	public bool _foundWeather { get; private set; }

    private void Awake()
    {
		GetRealWeather(); // get the weather of the game before the first frame is 
	}

    public void GetRealWeather () {
		string apiKey = ReadPrivateFiles.GetAPIKey();
		string uri = "api.openweathermap.org/data/2.5/weather?";
		if (useLatLng) {
			uri += "lat=" + latitude + "&lon=" + longitude + "&appid=" + apiKey;
		} else {
			uri += "id=" + city + "&appid=" + apiKey;
		}
		StartCoroutine (GetWeatherCoroutine(uri));
	}

	IEnumerator GetWeatherCoroutine (string uri) {
		using (UnityWebRequest webRequest = UnityWebRequest.Get (uri)) {
			yield return webRequest.SendWebRequest ();
			if (webRequest.isNetworkError) {
				Debug.Log ("Web request error: " + webRequest.error);
			} else {
				ParseJson (webRequest.downloadHandler.text);
			}
		}
	}

	WeatherStatus ParseJson (string json) {
		_weather = new WeatherStatus ();
		try {
			dynamic obj = JObject.Parse (json);

			_weather.weatherId = obj.weather[0].id;
			_weather.main = obj.weather[0].main;
			_weather.description = obj.weather[0].description;
			_weather.temperature = obj.main.temp;
			_weather.pressure = obj.main.pressure;
			_weather.windSpeed = obj.wind.speed;
		} catch (Exception e) {
			Debug.Log (e.StackTrace);
		}

		_weather.temperature = _weather.Celsius(); // converts temperate into celcius value
		_foundWeather = true;
		return _weather;
	}

}
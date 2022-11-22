
// Conditions explained: https://openweathermap.org/weather-conditions

/*
	I got this code from this video package: https://www.youtube.com/watch?v=x53swDdRVOk;

	Source: https://openweathermap.org

	Api response docs: https://openweathermap.org/current
*/

public class WeatherStatus {
	public int weatherId;
	public string main;
	public string description;
	public float temperature; // in kelvin
	public float pressure;
	public float windSpeed;

	public float Celsius () {
		return temperature - 273.15f;
	}
	public float Fahrenheit () {
		return Celsius () * 9.0f / 5.0f + 32.0f;
	}
}
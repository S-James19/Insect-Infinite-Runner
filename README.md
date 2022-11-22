## Insect Infinite Runner Game

---

### About

I have created an infinite runner game that uses a custom controller and uses real-time data to affect the weather of the game.

#### Aims and Objectives

The main objective of the game is to run as far as possible distance to rack up score.

* You are an insect being chased by a predator running through grass.
* Along the way, you will run into more predators (birds and ground insects) that will also try to eat you, which you will have to avoid by hiding.
* Don't hide for too long, otherwise the chasing predator will catch up and eat you.
* You have 3 lanes to run in, which you can switch to when running.
* You can collect leaves along the lanes which will boost your score at the end of the game depending on how many you collect.

#### Real-Time Weather Data

The Unity program uses [OpenWeather](https://openweathermap.org) weather data to influence the weather conditions of the game. 

* You can choose the desired location of weather, e.g. London, New York, etc.
* Dependant on temperature conditions, the game will either be:
    * Raining
    * Snowing
    * Cloudy
    * Sunny
    
---

### Controls

#### Keyboard and Mouse

The player will automatically run for you.

* W - Stop moving
* A - Switch lane left / hide left
* S - Hide underground
* D - Switch lane right / hide right

#### Custom Controller

The custom controller that I have designed involves the player putting both hands onto a flat controller surface supported by a sphere base, which they will then use to rotate the controller forwards, backwards, left and right.

The custom controller that I have designed includes the following:

* MPU6050 Acceleromter - to record orientation of the controller to map in game insect control
* IR Sensors - To detect hands on the controller for pausing and unpausing game
* Arduino Microcontroller - to send data from sensors to the Unity application.

---

### Technologies Used

* Unity
* C#
* C++ 
* OpenWeatherMap API https://openweathermap.org
* Arduino Microcontroller & IDE

---

### Installation

1. Clone repository

``` git clone https://github.com/S-James19/Insect-Infinite-Runner ```

2. Create 'key' text file in Insect-Infinite-Runner/Unity Project/User directory.

3. Navigate to https://openweathermap.org/api, choose plan and generate API key.

4. Paste API key into 'key'.

5. Open Project in Unity, Open Main Scene.

6. Navigate to https://openweathermap.org find, search for desired destination and copy number in URL.

``` https://openweathermap.org/city/2643743 // London Example ```

7. Find GameManager gameObject in MainScene, find Real World Weather script attachted to gameObject.

8. Paste city code into city field.

9. Build project, save wherever you like.

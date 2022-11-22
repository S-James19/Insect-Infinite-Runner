## Insect Infinite Runner Game

---

### About

I have created an infinite runner game that uses a custom controller and uses real-time data to affect the weather of the game.

---

#### Aims and Objectives

The main objective of the game is to run as far as possible distance as possible to rack up score.

* You are an insect being chased by a predator running through grass.
* Along the way, you will run into more predators (birds and ground insects) that will also try to eat you, which you will have to avoid by hiding.
* Don't hide for too long, otherwise the chasing predator will catch up and eat you.
* You have 3 lanes to run in, which you can swtich to when running.
* You can collect leaves along the lanes which will boost your score at the end of the game depending on how many you collect.

---

### Controls

#### Keyboard and Mouse

The player will automatically run for you.

* W - Hide underground
* A - Switch lane left / hide left
* S - Stop moving
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

---

### Installation

1. Clone repository

``` git clone https://github.com/your_username_/Project-Name.git ```

2. Create 'key.txt' text file in Insect-Infinite-Runner/Unity Project/User directory.

3. Navigate to https://openweathermap.org/api, choose plan and generate API key.

4. Paste API key into 'key.txt'.

5. Open Project in Unity, Open Main Scene.

6. Navigate to openweathermap.org/find, search for desired destination and copy number in URL.

``` https://openweathermap.org/city/2643743 // London Example ```

7. Find GameManager gameObject in MainScene, find Real World Weather script attachted to gameObject.

8. Paste city code into city field.

9. Build project, save wherever you like.

### License

---

Find external resources / credits in CREDITS.md

### COMP140 Insect Game

## What is Insect Game?
Insect Game is a first person infinite runner game. 
- The player is an insect at the bottom of the food chain.
- The player is being chased along a path in the grass by a predator
- Along the way, the player will run into more predators, who will attempt to attack/eat the player.
- The player must avoid these predators by hiding - left, right, underground, stop moving.
- Different predators will be able to still spot the player in specific hiding areas.
- The player must hide in the correct area corresponding to the predator attacking the player.
- The player must not hide for too long, otherwise the chasing predator will catch up and eat the player.
- The player can collect leaves along the path, which will boost their score modifier, boosting score at the end of the game
- The player must try and survive and run as far as possible without being killed.

## How does the Game Work / How to recreate?
Insect Game uses a custom-made controller to control the movement of the player, and the interaction with the UI (It does work with keyboard & mouse too).

- The controller consists of the following parts:
     - Gyroscope: Measure orientation of player and translates to directional movement in game
     - 2x IR sensors: Detect hand input on controller. Used to pause and unpause game.
     - Arduino Microcontroller: To allow communication between the sensors and the game.
- The Arduino Microcontroller has a script uploaded which will send the orientation of the controller, as well as hand detection to the computer.
- The game is developed in Unity.
- The script 'ArduinoData' has a function which is run when data is passed into Unity via the Uduino package.
- The 'GameManager' script will be updated with the hand input (to detect whether to pause/unpause).
- The 'InsectMovement' script attached to the player will recieve an event containing the data required to move the player.

Insect Game uses real-time weather data to change the environment of the game. Currently, it justs changes the weather conditions.
Given more time, I would like to add weather specific enemies, and weather-specific obsticles for the player to combat.

- The Script 'RealWorldWeather' will gather weather data for a specific city (default is London) using https://openweathermap.org/ api.
- The Script 'WeatherSettings' will take this data, and apply specific effects to the game scene.
- Depending on the api plan will effect how often the game can search for data. Currently on the free plan, I can only make 60 calls/minute.
- For my free version, it does not update the game that regularly, but for the Natural History Museum, a paid plan would be required to keep the game constantly updating to match the conditions of the weather.

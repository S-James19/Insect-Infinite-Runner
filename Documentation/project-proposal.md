# COMP140 Project Proposal

The contract that I have selected to undergo is "Natural Engagement".

## Game Design
### Game
- The player is controlling an insect at the bottom of the food chain, crawling through grass, being chased by a predator.
- The player is slightly quicker than the predator.
- While being chased, the player can run into more predators.
	- Aerial predators flying above, such as birds
	- Other insects.
	- Humans (although not a predator), walking across the grass
### Aim
- To teach people about different types of insects and the food chain.
- The player must avoid being eaten by the chasing predator, or running into another predator and being eaten by using the functions of the controller to hide.
- Do not hide for to long, otherwise the chasing predator will catch up and eat you.

## Controller
- Controllers typically use buttons to gather inputs from the user, using digital signals, which will allow the user to perform a task. My controller, however, will try and take on a different approach, and use a gyroscope chip to detect the orientation of the controller, to send analogue signals referencing the rotation of the controller, which in turn will allow the user to perform different tasks.
- The game is an exhibit which will be used by many different types of people, so I also wanted to design a controller that people of different ages can use, that is also very simple to use.
- The controller consists of a square surface, that is on top of a circular base, with an arduino circuit containing a Accelerometer & Gyroscope chip that will measure the rotation of the controller, as well as x2 IR sensors.
- The player will place both of their hands on the square surface, and use them to rotate the orientation of the controller.
- The player will avoid predators by using the controllers rotation functionality.
	- Forwards - hide underground to avoid aerial predators
	- Left/right - hide in grass shrubs to avoid ground predators
	- Backwards - To stop the insect from being stepped upon by a human
- The controller will connect to a PC via a USB cable which will connect the controller with the Unity software, allowing it to access the orientational values.
- The game will start when a user places both hands on the controller, and a countdown will begin. The game will pause if a user lifts both hands off of the controller.
### Design Choices
- Square surface - player will put both of their hands on the surface of the controller. The player has 4 different functions in game: hide underground, stop moving and hiding left and right. Square base means players can rotate in 4 different directions, matching the in-game functionalities.
- Circle base - This will allow the players to change the orientation of the controller to perform actions.
- Central Arduino Circuit - The arduino will change orientation with the controller when the player moves it with their hands, causing the values to change.
- Hands-on approach - Appeals to a very high percentage of users.
- IR Sensors - to stop users picking up the controller and trying to rotate it. IR sensors will detect if both hands are on the device surface to play the game.

## Key Electronic Components
- Arduino Board
- MPU 6050 Accelerometer/Gyroscope chip
- Infrared Sensors
- Resistors
- USB Power Supply
- Breadboard
- Cables
## Key User Stories
### Hardware
- Create Arduino circuit connecting the MPU 6050 chip and x2 IR Sensors
- Create controller housing for Arduino circuit.
- Export Arduino software onto the Arduino board.

### Software / Game
#### Player
- Take input values gathered from Arduino circuit and use those values to create player functions
- Create player game object and apply forward movement algorithm (longer they last, faster they go etc)
- Detect collisions with predators

#### Predators
- Program
	- Spawning
	- Player detection
	- Movement
- Detect collisions with player

#### Real-time data
- Connect to a service providing weather data for location using an API
- Apply weather type to the behaviour of predators


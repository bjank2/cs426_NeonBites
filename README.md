# CS426 Final project NEON-BITES

![image](https://github.com/bjank2/cs426_NeonBites/assets/89926012/d294e279-ad82-42aa-b9e3-675b2acbf4d1)
## Build Files
https://drive.google.com/drive/folders/1EANLb4bnvr2qkCM7hYo5mGTDaXVdcxZa?usp=sharing 



### Team: 
1. Bianca Jankiewicz
2. Alexa Osuna
3. Pranav Mishra (Project Manager)

## Game Idea:
In single player versus game, the player will play as a robot tasked with delivering food to customers while facing various conflicts such as time, enemies, and their malfunctioning cybernetic body. The player will manage time, money, and body/vehicle upgrades.

## FEs:
Interaction Pattern: Player vs game. There is no multiplayer, the player interacts with NPCs only.
Objective: Successfully and accurately deliver food to customers (rescue, getting the food to the customer safely)
Resources: Currency, character/vehicle upgrades, health, delivery app ranking
Conflicts: Time, unhappy customers, malfunctioning self. 
Boundaries: Physical boundaries: Player is confined to city. 
Outcome: Successfully upgrade and repair self with money or exchanged parts by completing orders accurately and timely. 

## INSTRUCTIONS: HOW TO RUN - FOR GRADING

Goto folder named '_SCENES'

### For the Playground scene:
	this is a customization scene, where you can add accessories to your player or bike. 
	As described on the screen, engage with the controls and arrow keys to switch between accessories. 
	To press next button, you will need to switch mode. Press I to do so. Only then will you see a cursor. 
	Pressing the button will spawn the bike. You will need to goto View mode again to interact with bike. 
	Press L to load the next scene, that is Minimaps scene

### For the Minimap scene:
	Play the level and engage with the NPC. You can talk to the NPC about anything. He is a coffeeshop owner. If you want your oreder, you need to ask him if your order is ready. 
	Press E to pickup order. When pickedup, the minimap at top-right will tell you where to deliver it.
	You can ride the bike in the scene, which is at right of where the player starts from. Use the controls as described in the UI screen. Press P to get off the bike.
	
	To engage with the blue enemy in the scene, press left click to throw a punch. The reaction of the enemy is based on the bayesian network probability. This is meant to be implemented for the customers in our game, which can 	turn hostile, based on probabilities and parameters of our delivery

### For the Patrol scene:
	This is a simple indoor scene where an AI Drone is patrolling the area. 
	The drone has 3 states. It will randomly travel around the map in search of player. You can keep track of it on the minimap.
	In order to throw grenade, keep pressing right click, move the mouse and press G together. This will spawn a grenade, which will go off after few seconds.

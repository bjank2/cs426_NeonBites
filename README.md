# CS426 Final project NEON-BITES

![image](https://raw.githubusercontent.com/bjank2/cs426_NeonBites/main/nbICON.png)
### Build Files
https://drive.google.com/drive/folders/1hZV297skXoXL6xCDH6ku1F_grX78XF9E

### Design Doc
https://docs.google.com/document/d/1csaCGoZ18wMGdjU5ncxgRVxiKu6TTFO754db7zGnMY4/edit?usp=sharing 

### Download for PC
https://drive.google.com/drive/folders/1hZV297skXoXL6xCDH6ku1F_grX78XF9E

## Game Idea:
A thrilling cyberpunk food delivery game where players navigate a neon-lit city, avoiding obstacles and enemies to deliver orders on time while managing resources and upgrading their character. Features include dynamic driving mechanics, minimaps, and interactive NPCs. 

Gameplay: NeonBites is a thrilling cyberpunk food delivery game where players take on the role of a delivery boy / robot navigating the neon-lit streets of a futuristic city. Players must run, jump, and ride their bikes through the bustling cityscape, picking up food orders and delivering them across various locations. With ramps, shortcuts, and a dynamic driving mechanic, players must use their skills to navigate the urban terrain efficiently.

### Team: 
1. Bianca Jankiewicz https://www.linkedin.com/in/bianca-mjankiewicz/
2. Alexa Osuna
3. Pranav Mishra (Project Manager) https://www.linkedin.com/in/pranavgamedev/

## Screenshots
![image](https://raw.githubusercontent.com/bjank2/cs426_NeonBites/main/nbICON.png)
![image](https://raw.githubusercontent.com/bjank2/cs426_NeonBites/main/nbICON.png)
![image](https://raw.githubusercontent.com/bjank2/cs426_NeonBites/main/nbICON.png)
![image](https://raw.githubusercontent.com/bjank2/cs426_NeonBites/main/nbICON.png)

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

# CS12-Mixed-Reality-for-Multi-domain-Operations


Our project is a virtual reality program that works to simulate strategic combat encounters in an urban environment with limited intel of the scene. This simulation works to better combat strategy in terms of squad positioning, processing intelligence, and communicating orders to allied units.


You assume the control of a team leader that can give commands to your autonomous teammates. These commands can have your teammates navigate, patrol, and even help each other in combat. The number of enemy combatants may be higher than the number of deployed allies, so the commands given will most likely be the difference between victory and defeat. 


The scene itself will only be revealed to the player by what the allied units can currently see, and partially of what has been past seen. This works to help simulate an unknown environment where the only visual intel is from the allied units. This is implemented as the common trope known as Fog of War. This implementation helps prevent the user from having what would be considered impossible knowledge of the battlefield. 


A user heads-up-display is displayed to the user to easily track, select, and view allied units. Single or multiple units can be selected at the same time to receive orders. Some application options can be toggled from launch or runtime such as fog of war, unit pathing trails, etc. These options were included to showcase certain aspects of the application under the hood but have no real advantage in the application. So, these options are toggle-able by the user. 


The allied and hostile units utilize a finite state machine to determine their current actions and possible coroutines. Primarily vision detection is used to determine the next state, but units can also alert nearby teammates to trouble they have spotted. Alerting these nearby units will have them rush to the encounter. Without visual contact of hostile forces, the autonomous units will continue to search the surrounding areas until contact is made.


Units use a rendered and mapped navigation mesh to navigate the battlefield. The mesh works to mark traversable areas and obstacles that can then be used to calculate pathing routes. These pathing routes are continuously dynamic, meaning that even if a unit is off the mapped mash, that unit will try to path back onto mapped areas.


## System Requirements

Must have an Oculus device between Oculus Rift and Oculus Quest 2

[Great website including system requirements](https://circuitstream.com/blog/vr-hardware/)

GPU Requirements: Nvidia GTX 970 or greater

CPU Requirements: Intel i5-4590 or greater

RAM Requirements: The majority of VR headsets require at least 8 GB of RAM.

System requirements will change based on the minimum requirements of your specific VR Headset.

Operating System: Windows x86_64


## Software Requirements



[Unity Hub](https://unity3d.com/get-unity/download) is recommended

[Unity Version 2019.4.14f1](https://unity3d.com/unity/whats-new/2019.4.14)

[Updated Oculus Software](https://www.oculus.com/)


## Build Directions

1. Clone the repo however you prefer to clone from github
 
2. Launch Unity Hub

3. Go to the projects tab in Unity hub and click the Add button
 
4. Navigate to the cloned directory of the project and select the folder 'CS_Capstone_Unity' from the root folder.
 
5. Left click the project in the list of projects in the projects tab

6. Wait for the Unity Editor to import everything and launch the editor

7. In the editor click the file dropdown in the top left

8. Click Build and Run

9. Select the folder to create the build folder

10. Navigate to the build folder

11. Run 'Mixed Reality Multi-Domain Operations.exe' with your Oculus headset on and oculus software running.


## KNOWN ISSUES AND BUGS


### Building:

.    Sometimes items in the project will not import correctly upon cloning which will cause all kinds of errors. Remedy by right clicking in the editor and selecting reimport all. This will make sure everything is imported correctly.


### Main Menu Scene:

·         Button click hits twice, if button is clicked then other point on canvas clicked then the buttons action will be duplicated.

.      Text sizes sometimes change between VR and non-VR views.

·         No other bugs found

 
### Town Scene:

·         Ally/Enemy units can on occasion become stuck to walls leaving them stuck to the surface.

·         If a unit is caught in a wall or surface, Ally/Enemy units cannot detect them to engage.

·         On occasion, clicking units multiple times can cause them to be no longer selectable

·         Raycast from controller can fail to collide allowing select of units

·         If fog of war is deactivated, it will not record unit movements until reactivated leaving blind spots that would have been revealed if active.

·         IF game restarted with dead ally units, ally status log will still indicated allies are dead and have no health.

·         If unit is deselected, they cannot be reselected sometimes

·         Unit trail when activated at times will be a straight line from spawn to current point

·         While patrolling they will not attack


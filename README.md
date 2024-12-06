# Elemental-Motion
Final Project for CART398

This project was made possible using [Node.js](https://nodejs.org/en/download/package-manager), [Wekinator](http://www.wekinator.org/downloads/), the [MediaPipe HandsOSC model](https://github.com/vigliensoni/MediaPipe-Hands-OSC?tab=readme-ov-file) and [Unity](https://unity.com/download).

---

## How to Install
1. First, the Elemental motion Unity project must be opened in the Unity editor. The **2022.3.48f1** version specifically because attempting to open it in a different version may cause compatibility issues.

2. You must ensure that all game objects with OSC receivers (OSC Manager, LightSlider under the UI Root object, and WekinatorController script) are set to auto connect and are enabled on runtime. This allows the game to properly receive OSC messages.

3. To run the MediaPipe Hands OSC ML model, you will need to use the following commands in a Windows PowerShell window launched in administrator mode: ```cd``` (type your path to the folder containing the downloaded MediaPipe Hands OSC model). Then type ```npm install``` to install the node.js package which is required to run the bridge script that connects it to Wekinator. Then type ```Set-ExecutionPolicy Unrestricted```, which will allow your computer to run the bridge.js without flagging it as a threat. To run the bridge script, type ```node bridge.js``` and if you see an **“osc success”** message, then you are set for the last step. Double click on the index.html file to open in a browser and enable camera access to the window (only Google Chrome or Firefox will properly run the html file).

4. Open the Elemental Motion Pose Recognition Model in Wekinator and click "Run".


5. Press the play button at the top of the Unity editor window or press **ctrl + P** to enable play mode. To interact, move around the world using the **WASD** keys, look around using the **QERF** keys, and present your right hand to the camera while rotating on the Z-axis to transform the in-game world.

6. After stopping the project, if you wish to start again, you must select the terrain object and navigate to the terrain settings to set the terrain height back to its default value of 600.

To explain how the aforementioned components work together. The MediaPipe Hands OSC model will record the x and y rotation of each landmark of the skeleton that will be mapped to your right hand. That information is then sent to Wekinator as an OSC message, and it will alter the values on the twelve output sliders. The values from these twelve sliders will be sent to Unity within an OSC message, which Unity can process using the OSC manager implemented via the ExtOSC Unity addon. Finally, the WekinatorController script will classify these twelve numbers in an array and for each number in the array, it will assign its value to a specific component in select game objects. These values modify things like the terrain’s height, the number of waves in the ocean, the softness of the mist clouds in the center of the map, the rotation of the sun, and more. All of this allows the world to be transformed by rotating your right hand.



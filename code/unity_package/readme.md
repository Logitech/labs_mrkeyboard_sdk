# MR Keyboard Plugin

Follow the steps below to get up and running with the MR keyboard.

## Setup
### Prerequisites
* You will obviously need a Microsoft MR headset and the keyboard.
* Make sure you have both SteamVR and "Windows Mixed Reality for SteamVR" installed on Steam.
* This plugin was tested without issues both in Unity 2017.3.0f1 and 2018.1.5f1.

### Unity project setup
1. Import the SteamVR plugin from the Asset Store into your Unity project. This is needed to track the keyboard.
2. Extract the unitypackage in this repository and drop the `Tracked K780` prefab into your scene.
3. Create two layers called `LeftQuad` and `RightQuad` in your project. To do this, go to Edit > Project Settings > Tags and Layers and add the exact names above in the next two available slots in the list.
4. Depending on your current or intended camera setup, you have different options. If your current setup uses, or if you foresee needing:
    * Two camera, one with Target Eye 'Left' and one 'Right' - you don't need to do anything, one of our scripts will find your cameras and adjust their culling masks.
    * A single native Unity Camera (with Target Eye 'Both') - just replace your camera with the `Native Camera` prefab we provide, or use this prefab as an example of how to setup two native Unity cameras.
    * SteamVR's Camera Rig prefab - in that case, we need to make a couple of modifications. First navigate in your GameObject to the eye object `CameraRig > Camera (head) > Camera (eye)`, and on the Camera component of that object, change the Target Eye value to 'Left'. Next, just drag our `[CameraRig] (right eye only)` prefab besides it. This second prefab has everything disabled except for a single camera that is already configured as right eye. This allows you to continue using your original Camera Rig for everything that is controller- or area-related.

Below is an illustration of the minimal setup needed to see the keyboard (maybe also add a light):

![Unity camera setup](./mr_keyboard_cameras.png?raw=true)

### Run the scene
* Make sure to have fully initialized SteamVR (light grey environment or SteamVR Home)
* In the SteamVR Status window, you should see at least one HMD and a generic device (a 'C' in a hexagon). If this is not the case, try restarting SteamVR, and make sure you have the correct driver on your machine (see instructions for driver setup).
* Run your scene.

*Note: Right now, the driver only provides the left camera image to both eyes. In the future, we will be adding the right camera, effectively making the vision of your real hands stereoscopic. The project setup, scripts, and rendering pipeline is already "stereoscopic-ready" and the upgrade will be effortless for you down the road.*

## Coding

Below is a diagram of the various components of the plugin. The big arrows indicate where an external developer may take actions through public functions or variables.

![Unity camera setup](./mr_keyboard_diagram.png?raw=true)

The most relevant scripts to take a look at are the following:

| Script                | Description |
|-----------------------|-------------|
|`TrackedKeyboard`      | Makes sure the keyboard follows the right object. Lets you know if the keyboard has been found, what is the offset between the tracking origin and the keyboard model, and will allow you to change the physical keyboard model in the future. |
|`BlendingEffectManager`| Allows to change the brightness of the hands, their color, etc. |
|`KeyboardAnimator`     | Lets you toggle a "floating keys" type of keyboard, and change materials for the various keyboard states (regular, key pressed, floating keys). Use the `ChangeSkin` method to change the skin used by the current keyboard mode. |

## Troubleshooting and known issues
These are potential issues you may run into, along with some tips on how to recover:

* As stated above, at the moment we provide you only with the left camera view of your hands. Your project and scripts will however already be ready when we add the second camera.
* If, once started, SteamVR doesn't show the 'C' in a hexagon, this means that the keyboard somehow did not make it all the way to SteamVR. In this case, first make sure that the Mixed Reality Portal did not go to sleep, and try restarting SteamVR.
* Within Unity, when using a Camera Rig prefab, an error will show up when launching your application, saying that a render model could not be loaded. This is normal since SteamVR doesn't know what to display at the location of the keyboard. This will be solved in a later version.
* The keyboard might show a white texture. If the MR Portal went to sleep (potentially closing SteamVR in the process), you might need to wake it up, start SteamVR, and then restart Unity.
* If in the headset you see two sets of hands on top of the keyboard, or that the left and right eyes see mismatching video content, make sure your cameras are set up correctly (see the Unity project setup section above).
* The hands video feed has a suboptimal framerate. This is a known issue and we are working on it. Once this is solved, an update for the your driver will be provided through Windows Update.
* When you look to the left of the keyboard, your right hand might get cut. This is related to the field of view of the left camera, and will be much less noticeable once we add stereoscopic hands vision, as the right camera has a better view of the right hand in such a case.

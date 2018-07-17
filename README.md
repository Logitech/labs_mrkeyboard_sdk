# Microsoft & Logitech MRkeyboard sdk

![Keyboard image](/Logitech_MR_Keyboard.png?raw=true)

## Introduction
Over the past few months Logitech have been working closely with the Microsoft Mixed Reality team on prototypes of a tracked keyboard for their VR headsets (see [here](https://www.microsoft.com/en-us/store/collections/vrandmixedrealityheadsets) for list of the HMDs from Samsung, Acer, Lenovo, etc.). The keyboard is tracked using the same mechanism as the Microsoft MR controllers, and you can see the keyboard and your hands in VR.

We’re at a stage now where we’ll be soon sharing some of these prototypes with a small number of external partners under NDA (in July/August). We’ll be sharing code example and built applications, as well as an overlay application that lets you use the keyboard in any SteamVR application.

![Keyboard animation](/Logitech_MR_Keyboard.gif?raw=true)

## The Development Kit content
- Hardware prototype keyboard.
- Demo app ([MR Keyboard browsing demo](https://github.com/Logitech/labs_mrkeyboard_sdk/tree/master/demos/mr_browsing_demo)) that shows basic keyboard usage in VR.
- [SteamVR Overlay](https://github.com/Logitech/labs_mrkeyboard_sdk/tree/master/demos/overlay) executable allowing the keyboard to be used on top of any SteamVR app (without modifications).
- [Unity package](https://github.com/Logitech/labs_mrkeyboard_sdk/tree/master/code/unity_package) source code to be used for the integration of the keyboard in your app.

## Setup instructions
In order to setup your MR Keyboard you'll need to: 

### 1) Enroll in Windows Insider Builds to get the latest HMD tracking driver automatically ([link](https://insider.windows.com/en-us/getting-started/))

### 2) Verify you have the correct HMD tracking driver installed
- Connect your HMD to your PC. Close the Mixed Reality Portal that automatically opens.
- Open Device Manager. You can do this by clicking on Start, and typing "Device Manager" into the Search box.
- In Device manager, expand the "Mixed Reality Devices" section, and double click on your Mixed reality headset.
- Navigate to the "Driver" tab at the top and make a note of the Driver version (it must be 17706 or later)

### 3) Connect your MR Keyboard 
- Make sure to plug a USB cable attached to a brick (or to a PC) to the upper part of the Keyboard (where the constellation is). This will provide power for the LED used for the tracking (but not the whole keybaord. The AAA batteries on the backside provide power to the rest of the keyboard).
- Turn on your Keyboard (there is a switch on the right side of it and it will be green when it's on).
- Press F2 to switch to BT Smart (BTLE) mode
- Add/pair the keyboard in Windows (Bluetooth > Add a Bluetooth Device > Add > Bluetooth Device) 
- Select the device named "Kbd K780 MR POC", enter the 6 digit numerical code, wait for the pairing process to complete.
- Plug your Mixed reality Headset, start Windows's Mixed reality portal.

### 4) Launch the demo app
- The demo app requires Steam, SteamVR, and the SteamVR for Windows Mixed Reality plugin (see [here](https://docs.microsoft.com/en-us/windows/mixed-reality/enthusiast-guide/using-steamvr-with-windows-mixed-reality) for install instructions)
- For a first test, you can launch our [MR Keyboard browsing demo app](https://github.com/Logitech/labs_mrkeyboard_sdk/tree/master/demos/mr_browsing_demo).

### 5) Use the Unity package to integrate the keyboard into your app
- You can enhance your own steamVR application with an integrated keyboard.
- More customisations will be made available, such as a specific "skin" for the keyboard when your app is launched, and customised key labelings.
- Head to our demo app folder and follow the readme [instructions] (https://github.com/Logitech/labs_mrkeyboard_sdk/tree/master/code/unity_package)
- Note that we don't support UWP apps yet.


## Changelog
### 1.0 (July 17th 2018)
- initial release to selected developers
- contains Overlay demo app
- contains Web browsing demo app
- contains unity package for the integration in your own app

## Feedback & Bugs
We strongly suggest to use our private GitHub repository for bug reports and features requests. Follow this [link](https://github.com/Logitech/logilabs_mrkeyboard_sdk/issues) and post it there.


## FAQ and questions
- look for the FAQ, instructions and more questions on our [wiki](https://github.com/Logitech/logilabs_mrkeyboard_sdk/wiki)

## License
Copyright (c) Logitech and Microsoft Corporation. All rights reserved.
Licensed under the MIT License.

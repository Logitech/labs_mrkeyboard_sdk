# Microsoft & Logitech MRkeyboard sdk

- Beta version **1.0** (released on July 4th 2018)
- Contact: tobedefined
- Instructions [wiki](https://github.com/Logitech/logilabs_mrkeyboard_sdk/wiki)

![Keyboard image](/Logitech_MR_Keyboard.png?raw=true)

## Introduction
Over the past few months Logitech have been working closely with the Microsoft Mixed Reality team on prototypes of a tracked keyboard for their VR headsets (see [here](https://www.microsoft.com/en-us/store/collections/vrandmixedrealityheadsets) for list of the HMDs from Samsung, Acer, Lenovo, etc.). The keyboard is tracked using the same mechanism as the Microsoft MR controllers, and you can see the keyboard and your hands in VR.

We’re at a stage now where we’ll be soon sharing some of these prototypes with a small number of external partners under NDA (in July/August). We’ll be sharing code example and built applications, as well as an overlay application that lets you use the keyboard in any SteamVR application.

![Keyboard animation](/Logitech_MR_Keyboard.gif?raw=true)

## The Development Kit content
- Hardware prototype keyboard
- Demo program that shows basic keyboard usage in VR
- Private github repository with supporting SDK that can be used to add support for VR keyboard in apps

## Setup instructions
In order to setup your MR Keyboard you'll need to: 

1) Enroll in Windows Insider Builds to get the latest HMD tracking driver automatically ([link](https://insider.windows.com/en-us/getting-started/))

2) Verify you have the correct HMD tracking driver installed
- Connect your HMD to your PC. Close the Mixed Reality Portal that automatically opens.
- Open Device Manager. You can do this by clicking on Start, and typing "Device Manager" into the Search box.
- In Device manager, expand the "Mixed Reality Devices" section, and double click on your Mixed reality headset.
- Navigate to the "Driver" tab at the top and make a note of the Driver version (it must be 17706 or later)

3) Connect your MR Keyboard and launch the app
- Make sure to plug a USB cable attached to a brick (or to a PC) to the upper part of the Keyboard (where the constellation is). This will provide power for the LED used for the tracking (but not the whole keybaord. The AAA batteries on the backside provide power to the rest of the keyboard).
- Turn on your Keyboard (there is a switch on the right side of it and it will be green when it's on).
- Press F2 to switch to BT Smart (BTLE) mode
- Add/pair the keyboard in Windows (Bluetooth > Add a Bluetooth Device > Add > Bluetooth Device) 
- Select the device named "Kbd K780 MR POC", enter the 6 digit numerical code, wait for the pairing process to complete.
- Plug your Mixed reality Headset, start Windows's Mixed reality portal.
- The demo app requires Steam, SteamVR, and the SteamVR for Windows Mixed Reality plugin (see [here](https://docs.microsoft.com/en-us/windows/mixed-reality/enthusiast-guide/using-steamvr-with-windows-mixed-reality) for install instructions)
- For a test, you can launch our [demo app](link).


## Changelog
### 1.0 (July 4th 2018)
- initial release to selected developers

## Feedback & Bugs
We strongly suggest to use our private GitHub repository for bug reports and features requests. Follow this [link](https://github.com/Logitech/logilabs_mrkeyboard_sdk/issues) and post it there.

## License
Copyright (c) Logitech and Microsoft Corporation. All rights reserved.
Licensed under the MIT License.

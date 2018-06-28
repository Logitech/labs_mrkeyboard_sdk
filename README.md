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
In order to setup your MR Keyboard under Windows Mixed reality ecosystem you need to go through those steps:
- make sure to plug a USB cable attached to a brick (or to a PC) to the upper part of the Keyboard (where the constellation is). This will provide power for the LED used for the tracking (but not the whole keybaord).
- turn on your Keyboard (there is a switch on the right side of it).
- press F2 to switch to BT Smart (BTLE) mode
- add/pair the keyboard in Windows (Bluetooth > Add a Bluetooth Device > Add > Bluetooth Device) 
- select the device named "Kbd K780 MR POC", enter the 6 digit numerical code, wait for the pairing process to complete.
- plug your Mixed reality Headset, start Windows's Mixed reality portal.
- for a test, you can launch one of the demo app such as [this one](link).


## Changelog
### 1.0 (July 4th 2018)
- initial release to selected developers

## Feedback & Bugs
We strongly suggest to use our private GitHub repository for bug reports and features requests. Follow this [link](https://github.com/Logitech/logilabs_mrkeyboard_sdk/issues) and post it there.

## License
Copyright (c) Logitech and Microsoft Corporation. All rights reserved.
Licensed under the MIT License.

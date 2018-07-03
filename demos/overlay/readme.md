# Overlay for Logitech Mixed Reality Keyboard

This is a SteamVR Overlay application, it is fully compatible with all applications that are developed based on SteamVR (c)Valve. To launch it,double click on **MRKeyboardOverlay.exe**. The first time you should authorize the unsigned app and the network permissions. 

![MR Keyboard Overlay](mrkeyboardOverlay.gif?raw=true)

### Web UI
The Overlay app has a Web-based UI in the WebUI folder. The WebUI.html file has been tested with Chrome, other browsers may not work. The UI requires the browser to support WebSockets communication. The UI has a number of basic settings that allow for controlling the appeareance of both the keyboard and the hands visualization. Any changes will be automatically saved. 

![Web UI](webUI.PNG?raw=true)

### Keyboard pairing
In case the keyboard appears somewhere in the center of the play area and is not tracked, it may need to be paired again. **Tracked Devices** should contain a list of devices detected by SteamVR. Select the tracked device corresponding to the keyboard (e.g. MRSOURCE0) and your keyboard should be correctly tracked.

### Hands Visualization
There are two visualization modes (hands segmentation): 
- the default MS Segmentation, where you can use the slider to modify the opacity of the image. Try changing this parameter to get better results under low lighting conditions and/or with darker skin tones. 
- luminance based method, activitated when you un-tick the default segmentation. In this case, a threshold is applied on the grayscale image. Use the sliders to define the luminance range that will be visualized. 

### Known issues
You may notice some jitter in the rendered keyboard. This is related to the SteamVR overlay rendering and we are still looking for solutions. 



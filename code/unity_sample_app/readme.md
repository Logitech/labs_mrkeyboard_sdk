# Unity Sample Application

![preview](/resources/sample_preview.png?raw=true)

This is a full Unity project illustrating how one could integrate the plugin for the MR keyboard.

The code for the app gives an example of several aspects of the SDK, such as how to create a new layout for the keyboard and dynamically change the keyboard material.

This application also serves as a really basic demo app to try out the keyboard. For a more advanced use case example, make sure to check out the browsing demo!

## Changelog

* v0.2 - Change keyboard skin according to latest SDK
* v0.1 - Fix a bug where holding Numpad1 changes color once, then outputs 1's
* v0.0 - Initial release, with plugin v0.7

## A note about layout creation

Under `Assets\SampleApp\Textures\` there is a `.xcf` Gimp file, a screenshot of which can be seen below, that shows how the custom icons were made for this app. Before exporting to PNG, the uv layer was of course hidden. These were then assigned to a material within Unity.

![uv placement](/resources/sample_uv_icon_placement.png?raw=true)

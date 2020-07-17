# NicerDicer

*No installation required, self contained single .exe to execute.*

Let's say you are tired of typing your desired rolls into chat, this application creates the complete text by buttonpress and copys that into your clipboard.
Now you can paste that Text wherever you want with "ctrl + v"!

But wait, theres more!

Automatic posting into Discord is also available! When you click a button, the software tries to find your discord window and determines which channel is opened.
If the channel name matches the one you provided in the headerbar, the text is automatically send there and posted (green button flash). 
If it cant find the Discord window or the currently openend channelname is wrong, the text is only posted to your clipboard (red button flash). 

![](https://github.com/simon-winter/NicerDicer/blob/master/NicerDicer/screen.jpg)

This is a small Windows application, which generates texts used for RPG dice rolling discord bots (currently only tested with Avrae).
Dice types, dice amount and also the specific commands are easily customizable.

This App does not hook into Discord or uses any networking, its just a set of buttons which create text-strings and optionally focuses a window 
and simulates key presses (to send that text).

New features/optins might come especially if requested.


### Building

To build this yourself, clone the repository, open the solution in Visual Studio, right click the projectfile and select "Publish..." with your desired settings.



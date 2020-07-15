# NicerDicer

This is a small Windows Application, which generates Texts used for RPG dice rolling discord bots. 
All common dice types are represented as a scalable Button grid.

![](https://github.com/simon-winter/NicerDicer/blob/master/NicerDicer/screen.jpg)

Let's say you are tired of typing your desired rolls into chat, this application creates the complete text by buttonpress and copys that into your clipboard.
Now you can paste that Text wherever you want with "ctrl + v"!

But wait, theres more!

Automatic posting into Discord is also available! When you click a button, the software tries to find your discord window and determines which channel is opened.
If the channel name matches the one you provided in the headerbar, the text is automatically send there and posted (green button flash). 
If it cant find the Discord window or the currently openend channelname is wrong, the text is only posted to your clipboard (red button flash). 

### Building

To build this yourself, clone the repository, open the solution in Visual Studio, right click the projectfile and select "Publish..." with your desired settings.

### 

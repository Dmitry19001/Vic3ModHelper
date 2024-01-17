# Vic3ModManager

Vic3ModManager is a tool designed to enhance the Victoria 3 gaming experience by allowing users to easily create and manage custom music mods. This tool streamlines the mod creation process, making it accessible for newcomers and those who don't want to spend much time doing routine things.

## Features
* Simple music mod generation
* Possibility to gather songs in albums
* Automatic transliteration from cyryllic for song and album titles
* PNG, JPG and  DDS cover images support (preview only and export)
* MP3 to OGG conversion (FFMPEG support)
* Project saving system
* Localization system

## WORK IN PROGRESS
* UI improving (constantly ipmroving)

## Installation and Setup
Currently you can clone the repository and build it yourself or download one of the releases.
Requirements for launch:
- .NET Framework 4.7.2
- [FFMPEG](https://ffbinaries.com/downloads) executable either in the ./ffmpeg or ./ffmpeg/bin directory for MP3 to OGG conversion (optional, but the game supports only .ogg music).

## Usage
- Fill starting form to create the mod
- Click "Create" button and you will be redirected to Music Manager page
- In "Albums" section click "Create new" button
- Then you are able to add new songs (you can choose multiple at the same time)
- You can write Title for your Album or Collection
- Also you can choose a cover image by pressing on vinyl disk icon (.jpeg, jpg, png and dds are supported)
- After you've done with the music you can go to Localization Manager page via navigation on the top of app
- At the Localization Manager you can add new localizations
- When you ready you can export your mod at Export page
- All you have to do is press export and wait for message that everything is done. (For that time there is no progress bar implemented)

Done! You can find the mod in game launcher and play with it. After localization update there will be possibility to make localization files for the mod, but until then it is all that app can do. 

## Short-term plans
* Multiple language support (Not started yet)
* FFMPEG-executable auto-download (Not started yet)
* HOW TO USE Tutorial (Not started yet)
* Ability to add mod thumbnail

## Long-term plans or brainstorm
* Agitator manager
* Event manager
* Decision manager

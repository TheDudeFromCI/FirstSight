# About First Sight

The First Sight package includes a set of out-of-the-box first person character controller scripts which should apply to most standard uses of first person controller scripts.

These scripts are highly customizable and should adapt nicely to fit the needs of your project. Some of these features include sprinting, camera impact shaking, footsteps, and physics object pushing.

# Installing First Sight

To install this package, follow the instructions in the [Package Manager documentation](https://docs.unity3d.com/Packages/com.unity.package-manager-ui@latest/index.html).

# Using First Sight

First Sight requires a game object, representing the player, to have a Character Controller, a Player Controller, a Mouse Controller, and an Input Settings script to be added to it. Another game object with the camera behaviour should be a child of this object, places at the position of the head of the character. The settings of these scripts should be filled out depending on the needs of your project.

After this, any additional features can be added to the player by adding more behaviors on the player game object. For additional information on behaviours, see **Behavior Scripts** below.

# Technical details

## Requirements

This package has no additional requirements.

## Package contents

### Behavior Scripts

**Screen Shake**
Can be used to add a small screen shake effect to the camera when the player receives an impact. By default, this script only handles fall damage based impacts, but exposes a public method which can be called from other scripts for events such as being damaged by an enemy.

**Footsteps**
You can use this script, along with an attached audio source, to play footstep founds as you walk. It takes in a set of audio files and plays them in sync with your player's walking speed in an alternating fashion.

**Sprint FOV**
This script automatically adjusts the camera field of view based on the player's speed. Namely, when the player sprints.

**Push Objects**
This script give the player the ability to push physics based object by walking into them.

### Prefabs

To speed up the getting-started process, a prefab for a default player controller can be found in the prefab folder. This contains a set of commonly use movement behaviors at their default settings.

In addition, a cross hairs UI canvas prefab prefab exists as a placeholder while if you are still working on a user interface. The can also be found under the prefab folder. It contains a simple Unity UI canvas object, with a small sprite in the middle of the screen.

## Document revision history

| Date         | Reason                         |
| ------------ | ------------------------------ |
| May 28, 2019 | Initial documentation written. |

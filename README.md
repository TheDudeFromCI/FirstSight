<h1 align="center">First Sight</h1>
<p align="center"><i>First Sight is an out-of-the-box first person controller script for Unity. Complete with full physics support, camera shake/fov effects, footstep support, sprinting and object pushing, it should suite as a nice foundation for most projects to build off of.</i></p>

<p align="center">
  <img src="https://img.shields.io/github/license/Wraithaven-UnityTools/FirstSight" />
  <img src="https://img.shields.io/github/repo-size/Wraithaven-UnityTools/FirstSight" />
  <img src="https://img.shields.io/github/issues/Wraithaven-UnityTools/FirstSight" />
  <img src="https://img.shields.io/github/v/release/Wraithaven-UnityTools/FirstSight?include_prereleases" />
  <a href="https://openupm.com/packages/net.wraithavengames.firstsight/"><img src="https://img.shields.io/npm/v/net.wraithavengames.firstsight?label=openupm&registry_uri=https://package.openupm.com" /></a>
</p>

---

<h2 align="center">A Super Special Thanks To</h2>
<p align="center">
  :star: Mika, Alora Brown :star:
</p>

<br />

<h3 align="center">And a Warm Thank You To</h3>
<p align="center">
  :rocket:  :rocket:
</p>

<br />
<br />

Thank you all for helping this project, and many others, to continue being developed.

---

### Getting Started

This package is distributed through [Open UPM](https://openupm.com/packages/net.wraithavengames.firstsight/). For more information about installing packages through Open UPM, see Open UPM's usage [here.](https://github.com/openupm/openupm-cli#openupm-cli)

### Features

First Sight contains many common features, ready to drop in and use, as well as a already setup prefab player which contains all common features, ready to use in seconds.

Some of these features include:
* **Screen Shake**
  * Shake the player's camera upon taking damage for falling from large heights. The amount of shake and duration are configurable and scale with the amount of damage taken. Shaking is controlled by a smooth interpolation function, making it fully compatible with time manipulation and framerate independent.
* **Footsteps**
  * Adding subtle footstep sounds to a player can greating increase the immersion for the player by making things feel a bit more responsive and life-like. Footsteps sounds are automatically played based on the player's speed, stops while jumping, supports alternating between the left and right audio speakers, and even changes in volume depending on whether or not the player is sneaking, walking, or running.
* **Dynamic FOV**
  * Automatically increase the camera's field of view slightly when the player is running, just to really sell that "moving *really* fast" effect.
* **Physics Object Pushing**
  * Walking into a very-movable box and having it just sit there isn't too much fun. This feature allows you to push thos pesky boxes out of your way, respecting mass and angle of impact. If it's in your way, move it. Short and sweet. :)

### API Changes

As the package has not yet reached the v1.0.0 release quite yet, expect API changes or prefab changes between different versions of the package. Simple patch updates, however, will not effect these changes externally.

### Additional Information

You can find more information about using this Unity package by checking out the [Documentation](https://github.com/Wraithaven-UnityTools/FirstSight/blob/master/Packages/net.wraithavengames.firstsight/Documentation%7E/FirstSight.md) page. This page is also included in local installations within the `FirstSight/Documentation~` folder in your Unity Packages directory.

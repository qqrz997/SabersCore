# SabersCore

A Beat Saber library that helps handle custom saber features. This includes files from the `Beat Saber\CustomSabers` 
directory, caching and providing saber metadata, and the ability to create new custom saber and custom saber trail 
instances.

This is intended to reduce the amount of effort needed when multiple mods require custom saber models. It also should
reduce conflicts by preventing multiple sources accessing the same assets.

## Manual Installation
> [!IMPORTANT]
> In addition to BSIPA, you must have [AssetBundleLoadingTools](https://github.com/nicoco007/AssetBundleLoadingTools) 
> and [SiraUtil](https://github.com/Auros/SiraUtil) installed for this mod to load. Install them using your mod manager
> i.e. [BSManager](https://bsmg.wiki/pc-modding.html#bsmanager).

Place the contents of the unzipped folder from the latest
[release](https://github.com/qqrz997/SabersCore/releases/latest) into your Beat Saber installation folder. If you
need more information regarding manual installation of mods
[this wiki page](https://bsmg.wiki/pc-modding.html#manual-installation) will help. For further help with installing
mods, join the [Beat Saber Modding Group](https://discord.gg/beatsabermods) discord server.

## API

In order to access the features of this mod, you must use Zenject, which is a dependency injection library used by
Beat Saber. If you need more information about this, check out [this page on the wiki](https://bsmg.wiki/modding/pc/zenject.html) to learn how to use Zenject
through the SiraUtil mod.

Documentation on the available services that this mod provides is planned and all but guaranteed, so don't get your
hopes up. You can find the services in the `Installers` within the source code underneath the `// API` comment.

The services can only be injected via their interfaces, for instance `CustomSabersLoader` implements `ISaberLoader`.

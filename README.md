# SabersCore

A Beat Saber library that helps handle custom saber features. This includes files from the `Beat Saber\CustomSabers` directory, caching and providing
saber metadata, and the ability to create new custom saber and custom saber trail instances.

This is intended to reduce the amount of effort needed when multiple mods require custom saber models. It also should reduce conflicts by preventing
multiple sources accessing the same assets.

## API

In order to access the features of this mod, you must use Zenject, which is a dependency injection library used by Beat Saber. If you need more information
about this, check out [this page on the wiki](https://bsmg.wiki/modding/pc/zenject.html) to learn how to use Zenject through the SiraUtil mod.

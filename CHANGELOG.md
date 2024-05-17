# Change Log

All notable changes to this project will be documented in this file. See [versionize](https://github.com/versionize/versionize) for commit guidelines.

<a name="1.2.0"></a>
## [1.2.0](https://www.github.com/thiagomvas/Basalt/releases/tag/v1.2.0) (2024-05-17)

### Features

* Add Octree ([258cc31](https://www.github.com/thiagomvas/Basalt/commit/258cc3181e654f0a0a91f75d1fba774060fd55b7))

<a name="1.1.0"></a>
## [1.1.0](https://www.github.com/thiagomvas/Basalt/releases/tag/v1.1.0) (2024-05-16)

### Features

* Add extension methods for EngineBuilder to add raylib presets ([5b85754](https://www.github.com/thiagomvas/Basalt/commit/5b85754f5ffa07e817d5ec6a991522617444484d))

### Bug Fixes

* Engine would not fully remove any references from component when removing entity ([b144dd4](https://www.github.com/thiagomvas/Basalt/commit/b144dd4f917ebfe7587e9c73ac5252519b77cdf4))
* Grid would non-stop add entities. ([e7c806a](https://www.github.com/thiagomvas/Basalt/commit/e7c806a83916dd8731b0d7f30089488aea0e300f))
* RaylibSoundSystem missing parameterless constructor ([05125e3](https://www.github.com/thiagomvas/Basalt/commit/05125e362373abbf09bab9374ea32eae161b343a))

<a name="1.0.1"></a>
## [1.0.1](https://www.github.com/thiagomvas/Basalt/releases/tag/v1.0.1) (2024-05-15)

### Bug Fixes
* No longer throws random exceptions for tightly timed entity instantiation ([e0f1ce0](https://www.github.com/thiagomvas/Basalt/commit/e0f1ce08e3b544ffd07b10e1809656eb94e1a11b))
* Physics Delta Time was not consistent ([e793eba](https://www.github.com/thiagomvas/Basalt/commit/e793eba484f1c517217dfaff2528fdf089544006))
* Raylib Graphics Engine always rendered info boxes ([d03d650](https://www.github.com/thiagomvas/Basalt/commit/d03d6502fa1896d2712af0b4c05991eebfe9e6ef))
* Raylib Graphics Engine logger was always null ([8cdd55a](https://www.github.com/thiagomvas/Basalt/commit/8cdd55acbb13ca378500a70ea55acfac2396618d))
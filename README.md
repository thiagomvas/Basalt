![Basalt Banner](basaltbannerwide.png)

# Basalt - A Do-It-Yourself (DIY) Framework for developing games
Basalt is a game development framework made in C# whose primary focus is a DIY aspect. It is designed with the idea that you can build your own game engine from the ground up, using Basalt as a foundation. It is not a game engine, but a framework that provides you with the tools to build your own game engine. It is designed to be simple, easy to use, and easy to understand. It is also designed to be flexible and extensible, so you can add your own features and functionality as needed.

## How does it work?
When setting up basalt in your project, you pick the implementations for certain components, like the physics engine, renderer, event handler, input system, etc. You may use an existing implementation by us or create your own. This way, you can customize the framework to your needs and build your own game engine.

All it takes is to implement the interfaces provided by Basalt and you are good to go, simply attach it to the engine. If needed, you may also implement new interfaces and engine components to attach in the engine to have it last during the entire program lifetime, all it needs is to implement ``IEngineComponent`` and your component will be attached to the engine. Adding it is as simple as less than 5 lines of code.

```cs
var builder = new EngineBuilder();
builder.AddComponent<IMyInterface, MyEngineComponent>();
var engine = builder.Build();
```

You may also pass a custom initialization function that returns your component in case it doesn't have a parameterless contructor or you'd like to do some DI. Not only that, but a second boolean parameter can be passed to determine if the component should be ran in a separate thread. 

## Features
- **Customizable Components:** Basalt allows you to select and implement various components such as the physics engine, renderer, event handler, and input system according to your project's requirements.
- **Flexibility:** Designed to be flexible and extensible, Basalt enables you to add your own features and functionalities as needed, providing a tailored solution for your game development needs.
- **Simplicity:** With a focus on simplicity, Basalt offers an easy-to-understand framework that simplifies the process of building your own game engine.
- **Ease of Use:** Built with user-friendliness in mind, Basalt provides straightforward tools and interfaces, making it accessible for both novice and experienced developers.

## Installation
To install Basalt, you can use the NuGet package manager in Visual Studio. Simply search for "Basalt" and install the package. Alternatively, you can install Basalt via the Package Manager Console by running the following command:

```
Install-Package Basalt
```

## Getting Started
For information on how to get started with Basalt, please refer to the [Basalt Wiki](https://github.com/thiagomvas/Basalt/wiki)

## Contributing
Contributions to Basalt are welcome! If you have ideas for new features, improvements, or bug fixes, feel free to contribute to the project by opening an issue or a pull request. 

## License
Basalt is licensed under the MIT License. For more information, please refer to the [LICENSE](https://github.com/thiagomvas/Basalt/blob/master/LICENSE)

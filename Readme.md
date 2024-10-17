# Networked Locomotion

## Table of Contents
1. [Project Setup](#project-setup)
2. [Tech Stack](#tech-stack)
3. [Packages](#packages)
4. [Project Idea](#project-idea)
5. [Installation Guide](#installation-guide)

## Project Setup
### Unity Version
Specify the Unity version required for this project:  
`Unity Version: 2022.3.14f1`

### Platform Targets
List the platforms the project supports (e.g., PC, Android, iOS, WebGL):  
`Supported Platforms: PC`

---

## Tech Stack
A list of technologies used in the project. Include languages, frameworks, and tools.

- **Unity**: Game engine for creating 2D/3D experiences.
- **C#**: Primary programming language for scripting.

---

## Packages
A list of Unity and third-party packages that are used in this project.

- **R3 Reactive Programming**: Used for handling asynchronous events and reactive data flow.
- **Photon Fusion 2**: Networking solution used for high-performance, multiplayer real-time applications.
- **Photon KCC (Kinematic Character Controller)**: Provides smooth and responsive character movement in a networked environment.
- **Photon FSM (Finite State Machine)**: State machine framework for managing complex state transitions in the project.
- **Odin Inspector**: Custom Unity inspector for better tools and workflow inside the Unity Editor.
- **Final IK**: Advanced inverse kinematics system for realistic character movement and animation.
- **Zenject (Dependency Injection)**: Lightweight dependency injection framework for decoupling components and simplifying code architecture.

---

## Project Idea
The main project idea is to work with best practices of using Photon (Character, Input, Syncing). The project includes a character with basic animations (they are not great at the moment, but they work for testing purposes). In the future, I plan to implement smoother transitions and more refined states.

The character can pick up and drop objects using Final IK. To see this in action, go to the scene: `Playground`.  
With the help of Fusion addons, I achieved smooth synchronization, lag compensation, and much more. The use of FSM (Finite State Machine) provided many ideas and simplified the coding process significantly.


## Installation Guide

### Clone the Repository
To clone the project download .ZIP or use the following command:
```bash
git clone git@github.com:zhiganik/Networked_FSM.git
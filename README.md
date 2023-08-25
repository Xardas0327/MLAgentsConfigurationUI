# MLAgents Configuration UI

## Introduction
This project is a user interface for [Unity ML-Agents](https://github.com/Unity-Technologies/ml-agents).
It helps the users, that they can create/modify a yaml file inside Unity Editor.  
Moreover, the users can run the `mlagents-learn` command without command line.  
Please check the `LICENSE` in `Assets/ML-Agents Configuration` before you use it.

## Test Environment
It was tested with Unity ML-Agents Release 20 only.
- OS
	- Windows 11
	- Ubuntu 20.04
	- MacOS Ventura 13.4.1
- Unity
	- 2021.3.29f1
	- 2022.3.7f1
	- 2023.1.9f1

## Source Code
There are two main folder in the Assets folder.
1.	The `Assets/ML-Agents` is not part of the Source Code. It is just help to develop/test this UI project.
	The folder contains the examples of [Unity ML-Agents Release 20](https://github.com/Unity-Technologies/ml-agents/tree/release_20_docs/Project/Assets/ML-Agents). 
2.	`Assets/ML-Agents Configuration` is the Source Code of the UI.

## Features
<img src="/Images/actionbar.png" alt="Actionbar Image" width="600">

### Creation
A user can create parts of the ML-Agents config yaml file as ScriptableObject.
They contains tooltips, validation. Futhermore, the users can see only the actual fields.
For example: If PPO Trainer Type is selected, the specific variables of SAC type is not visible.

<img src="/Images/creation.png" alt="Creation Image" width="600">

<img src="/Images/behavior.png" alt="Behavior Image" width="600">

<img src="/Images/checkpointSettings.png" alt="Checkpoint Settings Image" width="600">

### Import
A user can import any ML-Agents config yaml file from samples or their own custom project.
The files will be created in `Assets/ML-Agents Configuration/Files` folder. (If the folder doesn't exist, it will be created.)  
Limit: If the yaml file has default_settings, it will be part of the Behaviors. 
After that, if the users want to modify the same variable in the Behaviors, they have to update them one by one.
 
![Import Image](/Images/import.png)

### Export
A user can export ML-Agents Config ScriptableObjects into a yaml file.

![Export Image](/Images/export.png)

### Run Command Line
Firstly, it has to be configureated. It is in `Project Settings/ML-Agents Configuration`.
If a user use `Python Virtual Environment`, they have to add the `activate` file.

![Project Settings Image](/Images/projectSettings.png)

When a user want to run the `mlagents-learn` command, they have to select a yaml file and they can add the parameters of command.

![Command Line Interface Image](/Images/cli.png)

Note: By default, the Windows uses CMD, the MacOS uses Terminal and the Linux uses gnome-terminal.
On MacOS and on Linux, a `mlAgentsCommand.sh` file is always generated in the project folder. 
If the users use Git (or any source control system), they should add this sh file to `.gitignore`.

# ML-Agents Configuration UI

## Introduction
This project is a user interface for [Unity ML-Agents](https://github.com/Unity-Technologies/ml-agents).
It helps the users, that they can create/modify a yaml file inside Unity Editor.  
Moreover, the users can run the `mlagents-learn` command without command line.  
Please check the <a href="/Assets/Xardas/ML-Agents Configuration/LICENSE" target="_blank">LICENSE<a/> in `Assets/Xardas/ML-Agents Configuration` or in the main folder before you use it.

## Requirements
- Unity ML-Agents Release 20 and 21.
- OS
	- Windows (Tested: Windows 11)
	- Ubuntu (Tested: Ubuntu 20.04)
	- MacOS (Tested: MacOS 13.*, 14.*)
- Unity 2021.3.30f1 or later

## Source Code
There are two main folder in the Assets folder.
1.	The `Assets/ML-Agents` is not part of the Source Code. It is just help to develop/test this UI project.
	The folder contains the examples of [Unity ML-Agents Release 20](https://github.com/Unity-Technologies/ml-agents/tree/release_20_docs/Project/Assets/ML-Agents). 
2.	`Assets/Xardas/ML-Agents Configuration` is the Source Code of the UI.

## Features
<img src="/Images/actionbar.png" alt="Actionbar Image" width="600">

### Creation
The users can create parts of the ML-Agents config yaml file as ScriptableObject.
They contains tooltips, validation. Futhermore, the users can see only the actual fields.
For example: If PPO Trainer Type is selected, the specific variables of SAC type is not visible.


<img src="/Images/creation.png" alt="Creation Image" width="600">

<img src="/Images/behavior.png" alt="Behavior Image" width="600">

<img src="/Images/checkpointSettings.png" alt="Checkpoint Settings Image" width="600">

### Import
The users can import any ML-Agents config yaml file from samples or their own custom project.
The files will be created in `Assets/Xardas/ML-Agents Configuration/Files` folder. (If the folder doesn't exist, it will be created.)  
Limit: If the yaml file has default_settings, it will be part of the Behaviors. 
After that, if the users want to modify the same variable in the Behaviors, they have to update them one by one.
 
<img src="/Images/import.png" alt="Import Image" width="600">

### Export
The users can export ML-Agents Config ScriptableObjects into a yaml file.

<img src="/Images/export.png" alt="Export Image" width="600">

### Run Command Line
Firstly, it has to be configureated. It is in `Project Settings/ML-Agents Configuration`.
The users can use `Python Virtual Environment`, if they want.
There are 3 options:
- None: The user don't use Virtual Environment,
- Basic Python: The Virtual Environment, which the Python can create by default. The users have to add the `activate` file.
- Anaconda: If the users use the Anaconda package management, they have to add the `activate` file and the name of Virtual Environment.

<img src="/Images/projectSettings.png" alt="Project Settings Image" width="600">

When the users want to run the `mlagents-learn` command, they have to select a yaml file and they can add the parameters of command.

<img src="/Images/cli.png" alt="Command Line Interface Image" width="600">

Note: By default, the Windows uses CMD, the MacOS uses Terminal and the Linux uses gnome-terminal.
On MacOS and on Linux, a `mlAgentsCommand.sh` file is always generated in the project folder. 
If the users use Git (or any source control system), they should add this sh file to `.gitignore`.

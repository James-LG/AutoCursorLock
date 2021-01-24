# AutoCursorLock

Locks the cursor to a specified window while the window is in focus.

## Description

I created this program because some games don't lock the cursor when they should. Games that scroll when the cursor is near the edge of the screen can particularly be a problem. The main reason I built it though is Counter-Strike: Global Offensive sometimes does not keep up with fast mouse movements. This can be a problem when you have multiple monitors as you can actually click on the other monitor while flicking the mouse and effectively ALT-TAB at the worst possible moment.

This program will detect when a program like CS:GO is in focus and lock the cursor to the game. The cursor will automatically unlock when you ALT-TAB and relock when the game comes back into focus. There is also a hotkey to disable/enable the auto-locking.

## Usage

Start the program and select an item from the automatically populated processes list. Click the Add button to start automatically locking on that process' windows. The selections here are saved to `%appdata%/AutoCursorLock/settings.json` and loaded automatically on program start.

Press `Shift+HOME` to toggle the auto locking for all processes.

## Planned Features

- Auto start program on PC boot
- Manual selection of processes from file explorer
- Remapping of hotkeys

## Contributing

Go for it! Always happy to have help.
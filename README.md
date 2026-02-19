# Search Algorithm Simulation

This project is a simulation of graph search algorithms in C# WinForms.  

**Original Work:** This project was originally developed by **Prof. Chris Jordan Aliac**.  
**Enhancements:** For this assignment, I have implemented and improved the following:  

- **Breadth-First Search (BFS)** – Added as part of the assignment to explore uninformed search strategies.  
- **Hill Climb Search (refactored)** – Updated to use a heuristic: it chooses neighbors that are nearer to the goal node.  

These complement the original **Depth-First Search (DFS)** provided in the base project.

## Features

- **DFS (Depth-First Search):** Original implementation by Prof. Aliac.  
- **BFS (Breadth-First Search):** Original implementation by Prof. Aliac but slight changes in the Fringe. 
- **Hill Climb Search:** Refactored with a heuristic for goal-directed neighbor selection.  
- Visual simulation of node exploration for each algorithm.

## Simulation GIFs

### Depth-First Search
![DFS Simulation](./gifs/dfs_simulation.gif)

### Breadth-First Search
![BFS Simulation](./gifs/bfs_simulation.gif)

### Hill Climb Search
![Hill Climb Simulation](./gifs/hillclimb_simulation.gif)

## Paint Event Improvements

As part of the assignment, I made several improvements to the GUI visualization (paint event) to enhance clarity and usability:

- Added **Run with Delay** simulation to DFS  
- Added **Run with Delay** simulation to BFS  
- Added **Step** and **Run with Delay** simulation to Hill Climb  
- Start node now shows a **pin** instead of `S`  
- Goal node now shows a **flag** instead of `G`  
- Added **names beside nodes** upon creation for better identification

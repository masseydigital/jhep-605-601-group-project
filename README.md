# 605.601 Foundations of Software Engineering Group Project

**Class:** 605.601.81.FA19

**Group Name:** The Collective

**Project:** Clue-Less

**Team Members:**

King Carmichael III

Luc Nguyen

Bryan Zeug

Alex Hernandez

Drew Massey

**Project Description**
This game is a simplified version of the popular board game, Clue®.  The main simplification is in the navigation of the game board.  In Clue-Less there are the same nine rooms, six weapons, and six people as in the board game.  The rules are pretty much the same except for moving from room to room.

## Project Components

### Client Application

The client application for clue-less is what users use when they access the Clue-Less from their client computer.  The target platform for this build is WebGL utilizing WebAssembly.

To try out the current release of the client app, go to https://dev4286.d2cd3xkl3svzop.amplifyapp.com/

### Server Application

The server application for Clue-Less manages connections between clients and the server for instances of the games.  It manages the game state and logic for each game of Clue-Less and sends communication messages to connected clients.  The server is also responsible for connecting to the DynamoDB database and receiving game data.  The server application is a .Net standalone application running on a Windows 2019 Enterprise server.

### AWS Amplify

The game client application is hosted on an AWS Amplify app service.

### AWS Dynamo DB

Game data shall be hosted in an AWS Dynamo DB database.  This data includes game data including character names, weapons, and room names.

### AWS EC2

The game server is a Windows 2019 Enterprise server created using the AWS EC2 cloud service.  

## Project Repo Structure

### Unity Test

This folder contains all resources required to build the Unity test project for Clue-Less.  The server instance shall be a .Net standalone application and the web clients are WebGL builds.

Unity Version: 2019.2.0

Download Link: https://unity3d.com/get-unity/download?thank-you=update&download_nid=62773&os=Win

#### Scenes

DatabaseTest: Test scene for DynamoDB connection and table interaction

Gameboard: Front-end gameboard application

LoadingScreen: Loading screen for the player when they start the game

ServerTest: Test for client/server communication testing


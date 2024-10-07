# mjd-test-task
Test task for Arkadium

This assignment is meant to judge the following areas:
1. Game development architecture
2. Efficient data structures and algorithms
3. Code/Game Objects organization and clarity
4. Delivery speed

The assignment is to recreate the gameplay mechanics of this game and make a build playable on the
web (using a WebGL exporter): https://www.arkadium.com/games/mahjongg-dimensions/

Explanation of required game mechanics:
1. Game has an intro scene separated from the game scene, with a play button.
2. Game has a 4x4x4 cube grid that supports 6 different cube tiles.
3. All types of cube has a distinctive tile representing them in all 6 faces
4. The game has a 5 minute timer, decreasing count in seconds. Player loses if timer reaches zero.
5. Player can click on two cube tiles with the same sprite/colour.
6. Player can click and drag to rotate horizontally the cube (not vertically).
7. Cube tiles can only be pressed if they have mostly one neighbour in each horizontal direction
(up and down cubes don’t interfere).
8. Every time the player removes two tiles successfully, they receive 100 points.
9. Player wins if they manage to clear all cubes in the following time.

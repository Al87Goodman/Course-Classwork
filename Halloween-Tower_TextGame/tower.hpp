/**********************************************************
 * Project: Final - Text Based Game (Halloween Tower)
 *
 * Filename: tower.hpp
 *
 * Author: Alexander Goodman
 *
 * Due Date: 13 June 2017
 *
 * Description: Tower class Header File. Interface between
 * 	the Room objects and Player object.
 *
 * *******************************************************/

#ifndef TOWER_HPP
#define TOWER_HPP


#include "room.hpp"
#include "roomEntrance.hpp"
#include "roomKitchen.hpp"
#include "roomDining.hpp"
#include "roomLibrary.hpp"
#include "roomWash.hpp"
#include "roomCarving.hpp"
#include "player.hpp"


// ENUM for the Game Menu
enum DIRECTIONS{UP=1, DOWN, RIGHT, LEFT, SEARCH, BACKPACK, QUITGAME, DEFAULT_DIR};


class Tower
{
  private:

	bool towerStatus;			// Checks the Status of the Tower

	// Different Rooms
	Room* entrance;
	Room* kitchen;
	Room *dining;
	Room *library;
	Room *wash;
	Room *carving;
	DIRECTIONS Dir;				// User Selection from the Game Menu

	// Player and Player's Location
	Room* location;				// Player's Location
	Player *player;

  public:
	// Core Functions
	Tower();
	~Tower();

	void setLocation(Room*);			// Set's Player's Location
	std::string getLocation();			// Player's Location
	void moving(DIRECTIONS);

	void gameIntro();
	DIRECTIONS menu();

	void addItem();
	void itemOptions();
	void play();					// Plays the game!
	bool gameOver();				// Displays the Game Outcomes


};
#endif

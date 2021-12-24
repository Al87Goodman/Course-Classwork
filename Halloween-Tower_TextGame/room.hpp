/**********************************************************
 * Project: Final - Text Based Game (Halloween Tower)
 *
 * Filename: room.hpp
 *
 * Author: Alexander Goodman
 *
 * Due Date: 13 June 2017
 *
 * Description: Room class Header File. Abstract Base class 
	to represent the different rooms within the game.
 *
 * *******************************************************/

#ifndef ROOM_HPP
#define ROOM_HPP


#include <string>
#include <vector>

#include "item.hpp"
#include "player.hpp"

class Room
{

  protected:
	

	std::string name;				// Room Name
	std::string description;			// Room Description

	// Different Directions/Moves
	Room* up;
	Room* down;
	Room* right;
	Room* left;
	
	bool specialStatus;				// if the Special Function was compelted or not
	bool barrier;					// Some sort of Barrier preventing immediate entrance
							//       Default Setting is False...Not Present
	bool locked;					// The room on the other side of the barrier is locked until conditons are met 
	Player *player;					// Pointer to the Player Object
	

  public:
	// Core Functions:
	Room();					// Default Constructor
	Room (std::string);			// 1-Parameter Constructor Sets Name?
	virtual ~Room() = 0;			// Abstract Destructor

	//Setters & Getters
	void setName(std::string);		// Sets the Room Name
	std::string getName();			// Gets the Room Name
	
	void setDescription(std::string);	// Sets the Room Description
	std::string getDescription();		// Gets the Room Description

	void setBarrier(bool);			//Sets & Gets Barrier Status
	bool getBarrier();
	void setLocked(bool);
	bool getLocked();

	void setStatus(bool);
	bool getStatus();

	virtual void functionSp()=0;		// Special Function (Unique for each room)
	virtual void functionBarrier();		// Barrier Function (if present)

	void setPlayer(Player*);
	std::vector<Item*> items;		

	void setItem(Item*);
	Item* getItem(std::string);
	void displayItems();
	
	// Returns the Coordinates for the linked rooms
	void setCoords(Room*, Room*, Room*, Room*);
	Room* getUp();
	Room* getDown();
	Room* getRight();
	Room* getLeft();

	void Search();


};
#endif

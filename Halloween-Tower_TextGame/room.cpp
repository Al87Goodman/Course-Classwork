/**********************************************************
 * Project: Final - Text Based Game (Halloween Tower)
 *
 * Filename: room.cpp
 *
 * Author: Alexander Goodman
 *
 * Due Date: 13 June 2017
 *
 * Description: Room class Implementation File (Source Code).
 * 	Creates many of the Room Features and Functions
 * 	such as Name, Description, Items in the Room,
 * 	Coordinates, and special & barrier functions	
 *
 * *******************************************************/


#include <iostream>
#include <string>
#include <vector>

#include "inputval.hpp"
#include "room.hpp"


/************************************************
 * Room::Room()
 *
 * Description: Room class Default Constructor
 *	Sets the default Coordinates to nullptr
 *	(since each derivative class will have
 *	 a unique set of coordinates)
 * *********************************************/

Room::Room()
{
  up = nullptr;
  down = nullptr;
  right = nullptr;
  left = nullptr;
  specialStatus = false;

}

/************************************************
 * Room::Room(std::string)
 *
 * Description: Room class 1-parameter constructor.
 * 	sets the name of the room and sets the 
 * 	direction pointers all to nullptr
 * *********************************************/

Room::Room(std::string name)
{
  
  setName(name);
  up = nullptr;
  down = nullptr;
  right = nullptr;
  left = nullptr;
  specialStatus = false;
  barrier = false;
  locked = false;
}

/************************************************
 * Room::~Room()
 *
 * Description: Room class Destructor
 *
 * *********************************************/

Room::~Room()
{
  //std::cout << "Room Destructor Called." << std::endl;
}


/************************************************
 * SETTERS
 * *********************************************/

// Sets Room Name
void Room::setName(std::string name)
{
 this->name = name;
}

// Sets Room Description
void Room::setDescription(std::string description)
{
  this->description = description;
}

// Sets Barrier Status
void Room::setBarrier(bool barrier)
{
  this->barrier = barrier;
}

// Sets Locked Status
void Room::setLocked(bool locked)
{
  this->locked = locked;
}

// Sets Special Function completed or not Status
void Room::setStatus(bool specialStatus)
{
  this->specialStatus = specialStatus;
}

// Pointer to Player Object.
void Room::setPlayer(Player* player)
{
  this->player = player;
}

// Adds a new pointer to an Item Object
void Room::setItem(Item *newItem)
{
 items.push_back(newItem); 
}

/************************************************
 * GETTERS
 * *********************************************/

std::string Room::getName()
{
  return name;
}

std::string Room::getDescription()
{
  return description;
}

bool Room::getStatus()
{
  return specialStatus;
}

bool Room::getBarrier()
{
  return barrier;
}

bool Room::getLocked()
{
  return locked;
}

// Retrieves the Item by calling Items Name
Item* Room::getItem(std::string name)
{
  for(int i=0; i<items.size(); i++)
  {
    if(name == items[i]->getName())
    	return items[i];
    else
	std::cout <<  "Item Not Found" << std::endl;
  }

  
}


/************************************************
 * Display Items
 * *********************************************/

// Displays the Items currently in the Room items container.
void Room::displayItems()
{
  if(!items.empty())
  {
    /**  USED to verify if Item status changed or not when added/removed
    std::cout << "All Item Status: " << std::endl;
    for(int i=0; i<items.size(); i++)
    {
      std::cout << "Item " << i+1 << ": " <<  items[i]->getStatus() << std::endl;
    }
    **/
    std::cout << std::endl;
    std::cout << "__ Items in the Room __"<< std::endl;
    for(int i=0; i<items.size(); i++)
    {
      //Only show itmes that have not been picked-up yet
      if(items[i]->getStatus() == false)
      {
	std::cout <<"Item " << i+1 << ": " << items[i]->getName() << " - ";
	std::cout << items[i]->getDescription() << std::endl;
      }
    }
  }
  else
  {
    std::cout << "No more items left to pick up." << std::endl;
  }

}


/************************************************
 * setCoords(Room*, Room*, Room*, Room*)
 * *********************************************/

// Sets the Room Coordinates
void Room::setCoords(Room* up, Room* down, Room* right, Room* left)
{
  this-> up = up;
  this->down = down;
  this->right = right;
  this->left = left;


}

/************************************************
 * GETTERS FOR THE COORDINATES
 *
 * Description: Returns the pointers to Room Objects
 * 	for the desired location.
 *
 * *********************************************/

Room* Room::getUp()
{
   return up;

}

Room* Room::getDown()
{
  return down;
}

Room* Room::getRight()
{
  return right;
}

Room* Room::getLeft()
{
  return left;
}


/***********************************************
 * Room::functionSp() & functionBarrier()
 *
 * Description: virtual functions unique to 
 * 	each derivative class.
 *
 * *********************************************/

void Room::functionSp()
{

}

void Room::functionBarrier()
{

}



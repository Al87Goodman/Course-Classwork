/**********************************************************
 * Project: Final - Text Based Game (Halloween Tower)
 *
 * Filename: roomDining.cpp
 *
 * Author: Alexander Goodman
 *
 * Due Date: 13 June 2017
 *
 * Description: Dining class Implementation File (Source Code)
 *
 * *******************************************************/

#include <iostream>
#include <string>

#include "roomDining.hpp"
#include "inputval.hpp"

  InputVal validate;

/************************************************
 *		Core Functions
 *
 * Description: Dining class Default Constructor,
 * 	1-parameter Constructor, and Destructor
 *
 * *********************************************/

Dining::Dining()
{

}

Dining::Dining(std::string roomName)
{
  // Set Room Name, Description, and Item Objects
  setName(roomName);
  setDescription("Dining Room has some beautiful crafted furniture and candle holders.");
  item1 = new Item("candle");
    item1->setDescription("The Candle the Ghost Flickered into. This must be important.");
  item2 = new Item("Apple Cider");
    item2->setDescription("Some Nice Cider to wash down that Pumpkin Pie?");

  // Add items to the Room Vector
  items.push_back(item1);
  items.push_back(item2);

  // Room Status
   setStatus(false);   // Special Status off
   barrier = false;
   locked = true;
}

Dining::~Dining()
{
  delete item1;
  delete item2;

}


/************************************************
 * Description of the Dining Room
 *
 * *********************************************/

// In-depth Description currently clutters 
// 	up the screen




/************************************************
 * Dining::functionSp()
 *
 * Description: Special Function unique only to
 * 	the Dining Room.
 *
 * Special - No Special in the Dining Room, but
 * 	Need an object in this room to complete
 * 	the game.
 *
 * *********************************************/

void Dining::functionSp()
{

  if(!player->searchBackpack("candle"))
  {
    std::cout << "Hint: That Candle the Ghost's Spirit made Flicker must be a sign!" << std::endl;
  }

}



/************************************************
 * Dining::functionBarrier()
 *
 * Description:
 *
 * *********************************************/

void Dining::functionBarrier()
{
  // No Barrier Present in the Dining Room

}




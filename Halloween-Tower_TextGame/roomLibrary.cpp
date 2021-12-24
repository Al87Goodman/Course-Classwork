/**********************************************************
 * Project: Final - Text Based Game (Halloween Tower)
 *
 * Filename: roomLibrary.cpp
 *
 * Author: Alexander Goodman
 *
 * Due Date: 13 June 2017
 *
 * Description: Library class Implementation File (Source Code).
 *
 * *******************************************************/

#include <iostream>
#include <string>

#include "roomLibrary.hpp"

/************************************************
 *		Core Functions
 *
 * Description: Library class Default Constructor,
 * 	1-parameter Constructor, Destructor
 *
 * *********************************************/

Library::Library()
{

}

Library::Library(std::string roomName)
{
  // Set Room's Name, Description, and Item Objects
  setName(roomName);
  setDescription("Library Filled with Books and Papers. Maybe there is some Hidden Passageway here.");
  item1 = new Item("pumpkin seeds");
    item1->setDescription("Ooooh Pumpkin seeds, must be getting close..");
  item2 = new Item("matches");
    item2->setDescription("Matches would be useful to light a candle.");

  // Add Items to the Room Vector Container
  items.push_back(item1);
  items.push_back(item2);

  // Set Room Status
  barrier = true;
  locked = true;
  //setStatus(true);
  
}

Library::~Library()
{
  delete item1;
  delete item2;
}


/**************
 * Description of the Library
 *
 * ******************/


// In-depth Description not available


/************************************************
 * Library::functionSp()
 *
 * Description: Special Function unique only to the
 * 	Library Function.  
 *
 * SPECIAL - No Special, but an Item in the room is
 * 	needed to complete the objective.  There
 * 	is also a barrier
 * *********************************************/

void Library::functionSp()
{
  //No Special in the Library other than the Barrier

}


/************************************************
 * Libary::functionBarrier()
 *
 * Description: Checks to see if the player has 
 * 	the skeleton key in the backpack or not.
 *
 * *********************************************/

void Library::functionBarrier()
{
  // If PLayer has the skeleton key
  if(player->searchBackpack("Skeleton Key"))
  {
    std::cout << "I wonder what happens when I put this key I found in here." << std::endl;
    setBarrier(false);
  }

  // If Player does NOT have the skeleton Key
  else
  {
    std::cout << "Looks like a key is needed. " << std::endl;
    std::cout << "Hint: Check the Washroom for something hidden..." << std::endl;
    setBarrier(true);
  }	
  
}














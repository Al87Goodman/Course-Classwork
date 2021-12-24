/**********************************************************
 * Project: Final - Text Based Game (Halloween Tower)
 *
 * Filename: roomEntrance.cpp
 *
 * Author: Alexander Goodman
 *
 * Due Date: 13 June 2017
 *
 * Description: Entrance class Implementation File (Source Code)
 *
 * *******************************************************/

#include <iostream>
#include <string>

#include "roomEntrance.hpp"
#include "roomKitchen.hpp"
#include "item.hpp"

/************************************************
 *		Core Functions
 *
 * Description: Entrance Class Default Constructor,
 * 	1-Parameter Constructor, and Destructor.
 * *********************************************/

Entrance::Entrance()
{

}

Entrance::Entrance(std::string roomName)
{
  // Set Room Name, Description, and Item Objects
  setName(roomName);
  setDescription("Entrance to the Halloween Tower!! WOW IT IS SCARY.");
  item1 = new Item("flashlight");
     item1->setDescription("Help Navigate through this spoooky tower.");
  item2 = new Item("garlic");
    item2->setDescription("Ward off evil creatures of the night.");
  item3 = new Item("rope");
    item3->setDescription("20 ft of heavy rope");

  // Add items to the Room Vector
  items.push_back(item1);  
  items.push_back(item2);
  items.push_back(item3);

  // Set Room Status
  barrier = true;
  locked = false;
}


Entrance::~Entrance()
{
//  std::cout << "Entrance Destructor" << std::endl;
  delete item1;
  delete item2;
  delete item3;
}


/************************************************
 * Description of room...
 *
 * *********************************************/

// No Further Description because it seems to 
// 	clog up the screen: until something
// 	is implemeneted to clear the screen.


/************************************************
 * Entrance::functionSP()
 *
 * Description: Special Function unique only to 
 * 	the Entrance.
 *
 * Special: Need to have the Flashlight Item 
 * 	in the Player Backpack to move upstairs
 *
 * *********************************************/

void Entrance::functionSp()
{

  if(!player->searchBackpack("flashlight")) //item1->getStatus() == false)
  {
    setBarrier(true);
    std::cout << "Hint: It's pretty Dark around here.. I wonder if there's something that could help me see." << std::endl;
  }
}


/************************************************
 * Entrance::functionBarrier()
 *
 * Description: CHecks to see if the player has
 * 	the flashlight in the backpack or not.
 *  
 * *********************************************/

void Entrance::functionBarrier()
{

  // Player Does Not have flashlight in the backpack
  if(!player->searchBackpack("flashlight"))   //item1->getStatus() == false)
  {
    std::cout << "[Ghost Moaning]: \"BOOOOOOOOOOOOOOO\" " << std::endl;
    std::cout << "It is Too Dark and Scary to travel up the stairs!" << std::endl;
  //  std::cout << "Hmmm I wonder if there is anything here that could help light the way." << std::endl;
    setBarrier(true);
  }

  // Player Does have flashlight in the backpack
  else if(player->searchBackpack("flashlight"))  //item1->getStatus() == true)
  {
    setBarrier(false);  //turn barrier off
    std::cout << "It is pretty Dark ahead.. Good thing I found this flashlight." << std::endl;
    std::cout << "click...[flashlight illuminates].. WOW that is much better!" << std::endl;
  }


}









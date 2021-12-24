/**********************************************************
 * Project: FInal - Text Based Game (Halloween Tower)
 *
 * Filename: roomKitchen.cpp
 *
 * Author: Alexander Goodman
 *
 * Due Date: 13 June 2017
 *
 * Description: Kitchen class Implementation File (Source Code)
 * 	
 *
 * *******************************************************/


#include <iostream>
#include <string>


#include "roomEntrance.hpp"
#include "roomKitchen.hpp"
#include "inputval.hpp"

InputVal validKitchen;


/************************************************
 *		Core Functions
 *
 * Description: Kitchen class Default Constructor,
 * 	1-Parameter Constructor, and Destructor
 *
 * *********************************************/

Kitchen::Kitchen()
{

}

Kitchen::Kitchen(std::string roomName)
{
  // Set Room Name, Description, and Item Objects
  setName(roomName);
  setDescription("Looks like someone Baked a lot of food. Delicious Food.");
  item1 = new Item("pumpkin pie");
    item1->setDescription("Delicious Pumpkin Pie!");
  item2 = new Item("bottled water");
    item2->setDescription("Quench Thirst.");
  item3 = new Item("carving knife");
    item3->setDescription("Pumpkin Carving Knife.. This may come in handy later.");

  // Add items to the Room Vector
  items.push_back(item1);
  items.push_back(item2);
  items.push_back(item3);

  // Room Status
  setStatus(true);  // Special Status is Activated
  barrier = true;   // Barrier is activiated
  locked = true;
}

Kitchen::~Kitchen()
{
  //std::cout << "Kitchen Class Destructor" << std::endl;
  delete item1;
  delete item2;
  delete item3;
}



/************************************************
 * Description of the Kitchen
 *
 * *********************************************/

// In-depth Description Currently clutters up 
// 	the screen.


/************************************************
 * Kitchen::functionSp()
 *
 * Description: Special Function unique only to 
 * 	the Kitchen.
 *
 * Special" Need to have Pumpkin Pie in backpack
 * 	before moving into the Dining Room."
 *
 * *********************************************/

void Kitchen::functionSp()
{
  if(!player->searchBackpack("pumpkin pie"))  
  {
    std::cout << "Hint: I am in the mood for something sweet!" << std::endl;
  }

}

/************************************************
 * Kitchen::functionBarrier()
 *
 * Description: Checks to see if the player has 
 * 	the pumpkin pie in the backpack or not
 *
 * *********************************************/

void Kitchen::functionBarrier()
{
   // If the Special Status has not been cleared
  if(getStatus() == true)
  {
    // If Player does not have pumpkin pie in backpack
    if(!player->searchBackpack("pumpkin pie"))
    {
   	std::cout << "[Ghost]: \"BOOOOOO!!  I just want someone to eat my Dessert..\" " << std::endl;
    	std::cout << "The Ghost is a baker? hmm I wonder if there is someting in the kitchen it prepared?" << std::endl;
	setBarrier(true);  //
    }
    // If Player has pumpkin pie in backpack
    else if(player->searchBackpack("pumpkin pie"))
    {
      	std::cout << "[Ghost]: \"BOOOOOO!!  I just want someone to eat my Dessert..\" " << std::endl;
	std::cout << "[shaking while carrying the pumpkin pie]" << std::endl;
	std::cout << "[Baker Ghost]: \"AHHH I see you are interested in some of my Pumpkin PIe!\"" <<std::endl;
	std::cout << " [continued] \".. I worked extra hard on this pie and used my Special recipe, but no one came to enjoy it." << std::endl;
      	std::cout << "Would You Please try it and let me know what you think? [Y] for yes" << std::endl;
	std::string option;

	// Validates and Acts upon the User Selection
	option = validKitchen.strVal();
	if(option == "y" || option == "Y")
	{
	  std::cout << "MMMM! THis is the best Pumpkin Pie I have ever tasted.  Thank you!" << std::endl;
	  std::cout << "[Baker Ghost]: \"I am soo Happy you enjoyed it.  I can now move on from this realm.\"" << std::endl;
	  std::cout << "[Ghost goes flickering into a Candle on the Dining Room Table.]" << std::endl;
	  std::cout << "WOW That was amazing. Hmm, I wonder if that that Candle could be useful in the future?" << std::endl;
	

	  setBarrier(false);
	  setStatus(false);  //Deactive Special
	}
	// If the user Decides NOT to eat the pumpkin pie
	else
	{
	  std::cout << "[Baker Ghost]: \"FINE, BE LIKE THE OTHERS! GET OUTTTT! \"" << std::endl;
	}

    }
  }
}



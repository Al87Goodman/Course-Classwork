/**********************************************************
 * Project: Final - Text Based Game (Halloween Tower)
 *
 * Filename: player.cpp
 *
 * Author: Alexander Goodman
 *
 * Due Date: 13 June 2017
 *
 * Description: Player class Implementation File (Source Code)
 * 	Creates a container to store Item Objects,
 * 	Keeps track of player status: Win!, Defeated.,
 * 	Keeps track of time Remaining,
 * 	Add/Remove Item Objects from backpack (container)	
 *
 *
 * *******************************************************/

#include <iostream>
#include "player.hpp"
#include "inputval.hpp"

InputVal valid; // input validation object



/************************************************
 * Player::Player()
 *
 * Description: Player class Default Constructor 
 *	Setting all the Player flags and full timer
 *
 * *********************************************/

Player::Player()
{
  gameComplete = false;	// (true) if Player wins!
  defeated = false;	// (true) if Player is defeated
  packFull = false;	// Check to see if backpack is full
  timer = 100;		// Full Timer
  timeLeft = true;	// Time remaining 
}

/************************************************
 * Player::~Player()
 *
 * Description: Player class Destructor
 *
 * *********************************************/

Player::~Player()
{
  //std::cout << "Player Destructor" << std::endl;
}

/************************************************
 * TIMER FUNCTIONS
 *
 * *********************************************/

// Sets the timer and Updates the timer throughout the game
void Player::setTimer(int usedTime)
{
  if(timer > usedTime)
  {
    timer = timer - usedTime;
  }
  // Calls the time Reminaing flag
  else
  {
    timer = 0;
    timeLeft = false;
  }
}

// Returns the time remaining
double Player::getTimer()
{
  return timer;
}

// Returns the timer Flag
bool Player::getTimeStatus()
{
  return timeLeft;
}

/************************************************
 * Player::addItem(Item)
 *
 * Description: add an Item to the Player's backpack
 * 	which is a vector container
 *
 * *********************************************/

void Player::addItem(Item* itemNew)
{
  //Check to see if Maximum was reached and if item is available to be added.
  if(backpack.size() <5 && itemNew->getStatus() != true)
  {
    backpack.push_back(itemNew);	// Adds Item object to the backpack
    itemNew->setStatus(true);  		// changes the Item object status 
    std::cout << itemNew->getName() << " was added to your backpack." << std::endl;
  }

  // Notifies Player if the item is already in the backpack
  else if(itemNew->getStatus() == true)
  {
    std::cout << "Item is already in your backpack." << std::endl;
  }

  else
  {
    std::cout << "Backpack is Full. Discard an item to add a new item." << std::endl;
  }

  // Notifies Player that backpack is now at maximum capacity
  if(backpack.size() ==5)
  {
    std::cout << "Backpack is Now Full. " << std::endl;
    packFull = true;
  }

}

/************************************************
 * Player::displayBackpack()
 *
 * Description: Dyanamically displays the backpack
 * 	items as it grows and shrinks
 *
 * *********************************************/

void Player::displayBackpack()
{
  // displays the backpack's current capacity (maximum of 5)
  std::cout << "backpack capacity: " << backpack.size() << "/5" << std::endl;
  if(!backpack.empty())
  {
    std::cout << "**** BACKPACK ITEMS ****" << std::endl;
    for(int i=0; i<backpack.size(); i++)
    {
	std::cout << "Item " << i+1 << ": " << backpack[i]->getName() <<std::endl;
    }

    std::cout << "~~~ Remove an Item Enter [R] ~~~" << std::endl;
    std::cout << " (Enter any other letter to continue} " << std::endl;
    std::string option;
    option = valid.strVal();

    // If the user has chosen r or R then the removeItem function is called
    if(option == "r" || option == "R")
    {
      removeItem();
    }

    std::cout << "***************" << std::endl;
  }
  else
  {
    std::cout << "Backpack is currently Empty." << std::endl;
  }


}


/************************************************
 * Player::removeItem()
 *
 * Description: Removes an Item from the Player's
 * 	backpack
 *
 * *********************************************/

void Player::removeItem()
{  
  if(backpack.empty())
  {
    std::cout << "Backpack is Currently Empty." << std::endl;
  }
  else
  {
    std::cout << "Enter Item # you wish to Disgard: " << std::endl;
   
    // Displays the Items that are available to remove
    std::cout << "Items: " << std::endl;
    for(int i=0; i<backpack.size(); i++)
    {
	std::cout << "Item " << i+1 << ": " << backpack[i]->getName() <<std::endl;
    }

    // validates the user selection and then performs removal.
    int itemNum = valid.intVal_bound(1,backpack.size()); 
    for(int i=0; i<backpack.size(); i++)
    {
	if(itemNum == i+1 && itemNum != backpack.size()+1)
	{
	  std::cout << backpack[i]->getName() << " was removed from your backpack." << std::endl;
	  backpack[i]->setStatus(false); // Make Item Visible to be picked up again
	
	  backpack.erase(backpack.begin()+i);

	}
	else if(itemNum == backpack.size())
	{
	  backpack[backpack.size()-1]->setStatus(false);
	  backpack.pop_back();
	}
	
    }
  }

}

/************************************************
 * Search Backpack Function
 *
 * Description: Searches the backpack for Item Name
 * 	This is used to check during the barriers
 *
 * *********************************************/

bool Player::searchBackpack(std::string name)
{
  bool found = false;

  if(!backpack.empty())
  {
    for(int i=0; i<backpack.size(); i++)
    {
    	if(backpack[i]->getName() == name)
	  found = true;
    }
    return found;
  }

  else
    return found;
}

/************************************************
 * Game States 
 *
 * Description: Keeps Track if the game was
 * 	completed or not.
 * *********************************************/

// If Player Wins - Completes the game
void Player::setGameStatus(bool gameStat)
{
  gameComplete = gameStat;
}

// If Player Loses - Defeated 
void Player::setDefeated(bool gameDef)
{
  defeated = gameDef;
}

bool Player::getGameStatus()
{
  return gameComplete;
}

bool Player::getDefeated()
{
  return defeated;
}






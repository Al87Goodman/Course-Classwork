/**********************************************************
 * Project: Final - Text Based Game (Halloween Tower)
 *
 * Filename: roomCarving.cpp
 *
 * Author: Alexander Goodman
 *
 * Due Date: 13 June 2017
 *
 * Description: Carving class Implementation File (Source Code)
 *
 *  reference to riddle: www.allprodad.com/15-fall-riddles-and-jokes-for-kids/
 * *******************************************************/

#include <iostream>
#include <string>


#include "roomCarving.hpp"
#include "inputval.hpp"

InputVal validCarving;  // Input Validation Object

/************************************************
 *		Core Functions
 *
 * Description: Carving class Default Constructor,
 * 	1-parameter Constructor, and Destructor
 *
 * *********************************************/

Carving::Carving()
{

}

Carving::Carving(std::string roomName)
{
  //Sets Room Name, Description, and Item Objects
  setName(roomName);
  setDescription("Top of the Tower! Where the Great Pumpkin resides.");

  item1 = new Item("great pumpkin");
    item1->setDescription("The Great PUMPKIN!");

  // Add items to the room Vector
  items.push_back(item1);

  // Room Status
  setStatus(true);
  locked = true;
  greatPumpkin = false;
  barrier = true;
}

Carving::~Carving()
{
  delete item1;
}



/************************************************
 * Carving::getPumpkinStatus()
 *
 * Description: returns if the Great Pumpkin
 * 	has been lit or not.
 *
 * *********************************************/

bool Carving::getGreatPumpkin()
{
  return greatPumpkin;
}

void Carving::setGreatPumpkin(bool status)
{
  greatPumpkin = status;
}

/************************************************
 * Description of the Carving Room
 * *********************************************/


// In-depth description clutters up the screen


/************************************************
 * Carving::functionSp()
 *
 * Description: Special Function unique only to 
 * 	the Carving Room.
 *
 * Special - Need to Solve a Riddle and Have all 
 * 	the Items required to relight the 
 * 	Great Pumpkin
 *
 * *********************************************/

void Carving::functionSp()
{
  // If special Status has not been cleared then go to the function Barrier
  if(specialStatus)
  {
    functionBarrier();
  }

  // If the Riddle has been solved and the great pumpkin has not be relight
  if(!specialStatus && getGreatPumpkin()== false)
  {
    // Checks to see if all the necessary items are in the backpack
    std::cout << "Checking Backpack for necessary items to relight the Great Pumpkin." << std::endl;
    int count = 0;
    if(!player->searchBackpack("candle"))
    {
	std::cout << "Need Candle" << std::endl;
	count++;
    }

    if(!player->searchBackpack("carving knife"))
    {
	std::cout << "Need Carving Knife" << std::endl;
	count++;
    }

    if(!player->searchBackpack("matches"))
    {
	std::cout << "Need Matches!" << std::endl;
	count++;
    }
    // If all the items are in the backpack then Game Status is set to True and Player Wins.
    if(count == 0)
    { 
	std::cout << "[Carving new Spot for the Candle]" << std::endl;
	std::cout << " [Lighting the Candle and Placing Inside the Great Pumpkin." << std::endl;
	std::cout << "WOW, what a great looking Jack-o-Lantern!" << std::endl;
	setStatus(true);
	
	player->setGameStatus(true);  // Lets the Tower Class Know game has been Won
    }
    
  }

}

/************************************************
 * Carving::functionBarrier()
 *
 * Description: The Player has to solve a riddle
 * 	against the Demon. If incorrect: Player Loses
 * 	If corect: Player defeats demon.
 *
 * *********************************************/

void Carving::functionBarrier()
{

  if(getBarrier() == true)
  {
    std::cout << "AH it is an evil Demon guarding the Great Pumpkin" << std::endl;
    std::cout << "It must be the one behind this chaos!" << std::endl;
    std::cout << "[Demon]: \"AH I see you have found me. Soon your town will be mine!\"" << std::endl;
    std::cout << "I challenge you to a competition! If I win you leave and go back to where you came from."
	      << " If I lose..." << std::endl;
    std::cout << "[Demon]:\"If you lose, I get your SOUL!\"" << std::endl;
    std::cout << "Deal.." << std::endl;

    std::cout << "[Demon] \"Battle of the Wits. If you solve this riddle, I will leave this tower at once.  If not, well.. HAHA \"" << std::endl;

    bool soul = riddle2();  // gets the return bool from the riddle

    // If Player answers Incorrectly
    if(soul)
    {
      	std::cout << "[Demon]: \"YOUR SOUL IS MINE!!\"" << std::endl;
	player->setDefeated(true);	// Game Status: Player Loses
    }

    // If Player answeres Correctly
    else
    {
	std::cout << "[Demon]: \"CURSE YOU! A deal is a deal. Good-bye..for now.\"" << std::endl;
	setStatus(false);
    }

  }
}

/************************************************
 * Carving::riddle2()
 *
 * Description:  This is the second and last
 * 	riddle in the game.  If answered 
 * 	correctly the player defeats demon, if answered
 * 	incorrectly, player loses.
 *
 * *********************************************/

bool Carving::riddle2()
{
  bool soulRiddle = true;

  std::cout << "[Demon]: Riddle:  \"I'm tall when I'm young, I'm short when I'm old, and every Halloween I stand up "
	    << "inside Jack-O-Lanterns. What am i?\"" << std::endl;

  std::cout << "Enter a Number that you think is the correct answer: " << std::endl;
  std::cout << "Enter 1: Tree." << std::endl;
  std::cout << "Enter 2: Candle." << std::endl;
  std::cout << "Enter 3: Skeleton." << std::endl;
  std::cout << "Enter 4: Ghost." << std::endl;

  int x = 1;
  int y =4;

  inputRiddle2 = validCarving.intVal_bound(x,y);
  // Checks to see if the Player chose Correct or Incorrect
  switch(inputRiddle2)
  {
    case 1: std::cout << "Incorrect" << std::endl;
	break;
    case 2:  std::cout << "CORRECT!" << std::endl;
	soulRiddle = false;
	break;
    case 3:  std::cout << "Incorrect" << std::endl;
	break;
    case 4:  std::cout << "Incorrect" << std::endl;
	break;
   std::cout << "ERROR.." << std::endl;

  } 

  return soulRiddle;

}






/**********************************************************
 * Project: Final - Text Based Game (Halloween Tower)
 *
 * Filename: roomWash.cpp
 *
 * Author: Alexander Goodman
 *
 * Due Date: 13 June 2017
 *
 * Description: Wash class Implementation File (Source Code)
 *
 * REFERENCE/CITE: Riddle - www.allprodad.com/15-fall-riddles-and-jokes-for-kids/
 * *******************************************************/

#include <iostream>
#include <string>

#include "roomWash.hpp"
#include "inputval.hpp"

InputVal validWash;

/************************************************
 *	Core Functions
 *
 * Description: Wash class Default Constructor,
 * 	1-parameter Constructor, and Destructor
 *
 * *********************************************/

Wash::Wash()
{

}

Wash::Wash(std::string roomName)
{
  // Set Room's Name, Description, and Item Objects
  setName(roomName);
  setDescription("Washroom looks very clean!");
  item1 = new Item("Skeleton Key");
    item1->setDescription("This key looks old and Important! Must unlock something very special.");

  // Add items to the Room Vector
  //items.push_back(item1);  // Note: item becomes available if a condition is met

  // Set Room Status
  setStatus(true);	// Special is activated (Skeleton Key)
  locked  = false;
}

Wash::~Wash()
{
  delete item1;
}



/*******************
 * Washroom Details
 *
 * *****************/



/************************************************
 * Wash::functionSp()
 *
 * Description: Special Function unique only to 
 * 	the Washroom
 *
 * Special - Need to complete the riddle to 
 * 	obtain the skeleton key, which is needed
 * 	to go into the Carving Room.
 *
 * *********************************************/

void Wash::functionSp()
{
  // If the Special has not been Cleared Yet (Riddle Not Solved)
  if(specialStatus)
  {
    std::cout << "There is something written on the Mirror. It Looks like some kind of riddle." << std::endl;
    std::cout << "Try and Solve the Riddle? [Y/N]: " ;
    
    std::string option;
    option = validWash.strVal();

    if(option == "y" || option == "Y")
    {
	specialStatus = riddle();  // riddle() returns a bool, will change status

	if(!specialStatus)
	{
	  std::cout << "Oh what is this?! A Skeleton Key!" << std::endl;

    	  // Add items to the Room Vector
    	  items.push_back(item1);
   	  std::cout << "Search Room to Add Skeleton Key if you would like it." << std::endl;    
	}
    }

  }
  // If the Riddle was Correctly Solved
  if(!specialStatus)
  {
    std::cout << "Congratulations on solving the Riddle!!" << std::endl;
    std::cout << "Hint: Do Not Forget to Pick Up the Skeleton Key if you have Not already." << std::endl;
       
  }

}

/************************************************
 * Wash::riddle()
 *
 * Description: The riddle on the mirror in the 
 * 	washroom.  If solved will unlock the
 * 	skeleton key.
 *
 * *********************************************/

bool Wash::riddle()
{
  bool hidden = true;
  std::cout << "Question: What do you get when you divide the circumference of your"
	    << " Jack-O-Lantern by its diameter?" << std::endl;

  std::cout << "Enter a Number that you think is the correct answer: " << std::endl;
  std::cout << "Enter 1: Pumpkin Seeds" << std::endl;
  std::cout << "Enter 2: A Turkey Leg" << std::endl;
  std::cout << "Enter 3: Pumpkin Pi" << std::endl;
  std::cout << "Enter 4: Pumpkin Guts" << std::endl;

  int x = 1;
  int y = 4;
  
  inputRiddle = validWash.intVal_bound(x,y);
  switch(inputRiddle)
  {
    case 1: std::cout << "Incorrect" << std::endl;
	break;
    case 2: std::cout << "Incorrect" << std::endl;
	break;
    case 3: std::cout << "CORRECT!" << std::endl;
	setStatus(false);
	hidden = false;
	break;
    case 4: std::cout << "Incorrect" << std::endl;
	break;
    std::cout << "Default" << std::endl;

  }

 return hidden;

}


/************************************************
 * Wash::functionBarrier()
 *
 * Description: No Barrier for the Washroom
 *
 * *********************************************/

void Wash::functionBarrier()
{
	// NO BARRIER AT THIS TIME
}





/**********************************************************
 * Project: Final - Text Based Game (Halloween Tower)
 *
 * Filename: tower.cpp
 *
 * Author: Alexander Goodman
 *
 * Due Date: 13 June 2017
 *
 * Description: Tower class Implementation File (Source Code)
 * 	Runs the Actual Halloween Tower game.
 *
 * *******************************************************/

#include <iostream>

#include "inputval.hpp"
#include "tower.hpp"
#include "player.hpp"


InputVal validation;


/************************************************
 * Tower::Tower()
 *
 * Description: Tower class Default Constructor
 *
 * *********************************************/

Tower::Tower()
{

  // Creating "Room" Objects
  entrance = new Entrance("Entrance");
  kitchen = new Kitchen("Kitchen");
  dining = new Dining("Dining Room");
  library = new Library("Library");
  wash = new Wash("Washroom");
  carving = new Carving("Carving Room");

  // Setting the Coordinates for the room Objects
  entrance->setCoords(kitchen, nullptr, nullptr, nullptr);
  kitchen->setCoords(nullptr, entrance, dining, nullptr);
  dining->setCoords(library, entrance, nullptr, kitchen);
  library->setCoords(carving, dining, wash, wash);
  wash->setCoords(nullptr, nullptr, library, library);
  carving->setCoords(nullptr, library, nullptr, nullptr);

  // Set starting location
  setLocation(entrance);

  // Create Player Object and Pointers to the different locations
  player = new Player; 
  entrance->setPlayer(player);
  kitchen->setPlayer(player);
  dining->setPlayer(player);
  library->setPlayer(player);
  wash->setPlayer(player);
  carving->setPlayer(player);

  towerStatus = false;
  Dir = DEFAULT_DIR;
 
}

/************************************************
 * Tower::~Tower()
 *
 * Description: Tower Destructor
 * *********************************************/

Tower::~Tower()
{
  delete entrance;
  delete kitchen;
  delete dining;
  delete library;
  delete wash;
  delete carving;
  delete player;

}

/************************************************
 * Tower::gameIntro()
 *
 * Description: Displays the Opening Message?
 *
 * *********************************************/

void Tower::gameIntro()
{
  std::cout << "Welcome to Halloween Tower!" << std::endl;
  std::cout << ".... GET READY for a Spooooky fun adventure ...." << std::endl;
  std::cout << std::endl;
  std::cout << "It is All Hallows' Eve and your town's protection spell seems to not be working." << std::endl;
  
  std::cout << "The town's protection spell comes from the Great Pumpkin that resides "
	    << "in the top room in Halloween Tower." <<std::endl;  
  std::cout << "Ghosts, Ghouls, and Creatures are overrunning the town!" << std::endl;
  std::cout << "You go to the Halloween Tower and notice the Great Pumpkin is not lit." << std::endl;
  std::cout << "You must Enter the Halloween Tower and relight the Great Pumpkin before Midnight or else"
	    << " the creatures will stay in the town until Next Halloween, Terrorizing Everyone." << std::endl;
  std::cout << std::endl;
  std::cout << "OBJECTIVE: collect items in the tower that will be needed to relight the Great Pumpkin." << std::endl;
  std::cout << std::endl;
  std::cout << "NOTE: Time does Not follow conventional format in Halloween Tower, but instead changes based"
	    << "on something currently unknown." << std::endl;
  std::cout << "Hint: Time will decrease for every change in player's location." << std::endl;
  std::cout << "Goodluck! The fate of the town is all on your shoulders.." << std::endl;
  std::cout << "________________________________________________" << std::endl;
  std::cout << std::endl;
}

/************************************************
 * SETTER and GETTERS for Player's Location:
 *
 ***********************************************/

// Set Player's Location
void Tower::setLocation(Room* location)
{
  this->location = location;
}

// Get Player's Location
std::string Tower::getLocation()
{
  if(location == entrance)
	return "Entrance";
  if(location == kitchen)
	return "Kitchen"; 
  if(location == dining)
	return "Dining Room";
  if(location == library)
	return "Library";
  if(location == wash)
	return "Wash-Room";
  if(location == carving)
	return "Carving Room";

 return "No Where";	
}


/************************************************
 * Tower::Menu
 *
 * Description: Game Menu where the User selects
 * 	which options to use. Moves the player
 * 	from room to room, searches the room for
 * 	items, displays backpack and can quit 
 * 	the current game
 *
 * *********************************************/

DIRECTIONS Tower::menu()
{


  DIRECTIONS Dir = DEFAULT_DIR;
  int inputDir;
  int xdum = 1;
  int ydum = 7;

  std::cout << std::endl << "--- OPTIONS ---" << std::endl;
  std::cout << "Enter 1: Move Upstairs" << std::endl;
  std::cout << "Enter 2: Move Downstairs" << std::endl;
  std::cout << "Enter 3: Move Right" << std::endl;
  std::cout << "Enter 4: Move Left" << std::endl;
  std::cout << "Enter 5: Search the Room" << std::endl;
  std::cout << "Enter 6: Open up Backpack" << std::endl;
  std::cout << "Enter 7: Quit current game." << std::endl;
  inputDir = validation.intVal_bound(xdum, ydum);  // make sure input is within bounds

  Dir = static_cast<DIRECTIONS>(inputDir);

  switch(Dir)
  {

    // For each Direction Chosen:
    // 		- No change of Location if the player chooses a direction that points to null
    // 		- Checks to see if the room is locked and if so calls the current locations barrier function
    // 		- If Barrier function is solved then the player can move to the new location
    // 		- The timer is decremented by 5 for each change in location

    case UP:
	if(location->getUp() == nullptr)
 		std::cout << "Choose a Different Direction" << std::endl;
	else if(location->getUp()->getLocked() == true) //|| location->getBarrier() == true)
	{
	  location->functionBarrier();
	  if(location->getBarrier() == false)
	  {
		location->getUp()->setLocked(false);  	// Unlock the Room
		location = location->getUp();		// Change to new Location
		std::cout << location->getDescription() << std::endl; // New Location Description
		player->setTimer(5); // Time Remaining decreases
	  }	
	}
	else
	{
		location = location->getUp();  // Change to new Location
		std::cout << location->getDescription() << std::endl; // New Location Description
		player->setTimer(5); // Time Remaining decreases
	}
	break;

    case DOWN:
	if(location->getDown() == nullptr)
 		std::cout << "Choose a Different Direction" << std::endl;
	else if(location->getDown()->getLocked() == true)// || location->getBarrier() == true)
	{
	  location->functionBarrier();
	  if(location->getBarrier() == false)
	  {
		location->getDown()->setLocked(false);
		location = location->getDown();
		std::cout << location->getDescription() <<std::endl;
		player->setTimer(5);

	  }	
	}
	else
	{
		location = location->getDown();
		std::cout << location->getDescription() << std::endl;
		player->setTimer(5);
	}
	break;

    case RIGHT:
	if(location->getRight() == nullptr)
 		std::cout << "Choose a Different Direction" << std::endl;
	else if(location->getRight()->getLocked() == true)
	{
	  location->functionBarrier();
	  if(location->getBarrier() == false)
	  {
		location->getRight()->setLocked(false);
		location = location->getRight();
		std::cout << location->getDescription() << std::endl;
		player->setTimer(5);
	  }	
	}
	else
	{
		location = location->getRight();
		std::cout << location->getDescription() << std::endl;
		player->setTimer(5);
	}
	break;

    case LEFT:
	if(location->getLeft() == nullptr)
 		std::cout << "Choose a Different Direction" << std::endl;
	else if(location->getLeft()->getLocked() == true)
	{
	  location->functionBarrier();
	  if(location->getBarrier() == false)
	  {
		location->getLeft()->setLocked(false);
		location = location->getLeft();
		std::cout << location->getDescription() << std::endl;
		player->setTimer(5);
	  }	
	}
	else
	{
		location = location->getLeft();
		std::cout << location->getDescription() << std::endl;
		player->setTimer(5);
	}
	break;

    case SEARCH:   itemOptions();
		break;
    case BACKPACK: player->displayBackpack(); 
		break;
    case QUITGAME:  std::cout << "Thank you for playing." << std::endl;
		break;
    case DEFAULT_DIR:  std::cout<< "Default Case." << std::endl;

  }

  return Dir;
}

/************************************************
 * Display Items for User to Add 
 * *********************************************/
void Tower::addItem()
{ 
  // Location has items available
  if(!location->items.empty())
  {
    std::cout << "Enter Item # you wish to Add to backpack: " << std::endl;
    location->displayItems();
    int itemNum = validation.intVal_bound(1,location->items.size());
    for(int i=0; i<location->items.size(); i++)
    {
	if(itemNum == i+1)
	{
	  player->addItem(location->items[i]);  // Adds Item to Player Objects Backpack
	}
    }
  }

  // Location does Not have items available
  else
  {
    std::cout << "No Items to Add." << std::endl;
  }

}

/************************************************
 * Add or Remove Item to/from backpack
 *
 * Description: Calls the appropriate Function 
 * 	to satisify user selection
 *
 * **********************************************/

void Tower::itemOptions()
{

  location->displayItems();
  std::cout << "Enter [A] to add item or" << std::endl;
  std::cout << "Enter [R] to remove an item from Backpack." << std::endl;
  std::cout << "Press any other key to continue." << std::endl;
  std::string option;
  option = validation.strVal();
  
  // If user chooses to add an item
  if(option == "A" || option == "a")
  {
    addItem();
  }

  // If user chooses to delete an item
  if(option == "R" || option == "r")
  {
    player->removeItem();
  }
}



/************************************************
 * Tower::gameOver()
 *
 * displays the appropriate Game Over messages and 
 * 	returns status to the main function.
 * *********************************************/

bool Tower::gameOver()
{
  // If Player Wins! (defeats the Demon and Relight the Great Pumpkin
  if(player->getGameStatus() == true)
  {
    std::cout << std::endl;
    std::cout << "CONGLATURATIONS... I mean CONGRATULATIONS!" << std::endl;
    std::cout << "The Great Pumpkin is Shining brighter than EVER!" << std::endl;
    std::cout << "The Protection Spell has turned back on and the town is saved." << std::endl;
    std::cout << "........" << std::endl;
    std::cout << "\"ROARRR!\" .... OH NO NOW WHAT?!... " << std::endl;
    std::cout << " --------- GAME OVER --------" << std::endl;
    // DARK ENDING...
	return true;
  }

  // If the Player loses to the Demon in the Carving Room
  if(player->getDefeated() == true)
  {
    std::cout << std::endl;
    std::cout << "-------------------- GAME OVER ----------------"<< std::endl;
    std::cout << "DEMON HAS CLAIMED YOUR SOUL!!" << std::endl;
    std::cout << "The town has been overrun with creatures..." << std::endl;
    std::cout << "No escaping until Next Halloween..." << std::endl;
	return true;
  }

  // If the Plaer runs out of time before completing the objective (relighting the Great Pumpkin)
  else
  {
    std::cout << std::endl;
    std::cout << "-------------- GAME OVER --------------" << std::endl;
    std::cout << "Time has expired... The town has been overrun with Creatures." << std::endl;
    std::cout << "Hope for the best..." << std::endl;
	return true;
  }

}


/************** Play the Game ******************/

void Tower::play()
{
  // Call the Game Introdcution
  gameIntro();
 
  // Display the Current Location and Room Description
  std::cout << "Location: " << getLocation() << std::endl;
  std::cout << location->getDescription() << std::endl; 

  // Stay in Loop until user chooses Quit from the Game Menu
  while(Dir != QUITGAME)
  {
    // Gets current location and calls the special function
    std::cout << "Location: " << getLocation() << std::endl;
    location->functionSp();
    
    // Display how much Time is Remaining
    std::cout << "Time Remaining: " << player->getTimer() << std::endl;

  
    // Check Game State to see if the game is over or not
    if(player->getGameStatus() == true || player->getTimeStatus() == false || player->getDefeated() == true)
    {
	towerStatus = gameOver();  // if true, then return to main Menu.
	Dir = QUITGAME;
    }

    // If tower Status is false then Menu will be called.  If not then return to Main Menu.
    if(!towerStatus)
 	 Dir = menu();
   
  } // while loop
 
}
















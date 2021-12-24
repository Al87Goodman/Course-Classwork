/**********************************************************
 * Project: Final - Text Based Game (HALLOWEEN TOWER) 
 *
 * Filename: menu.cpp
 *
 * Author: Alexander Goodman
 *
 * Due Date: 13 June 2017
 *
 * Description: Menu Implementation file for Final
 * 
 * *******************************************************/

#include <iostream>
#include <string>
#include <fstream>


#include "menu.hpp"
#include "inputval.hpp"


using std::cout;
using std::cin;
using std::endl;
using std::string;


/**********************************************************
 * Menu::Menu()
 *
 * Description: Default Constructor to activate the Menu
 * 	for the desired program.  Loops until receiving 
 * 	the quit enum value: QUIT.
 *
 * *******************************************************/

Menu::Menu()
{
  cout << endl;
  cout << "Hello, Welcome to Halloween Tower! " << endl;
  cout << "Version 1.0" << endl;
  cout << "Last Update: 12 June 2017" << endl;
  cout << "Changes from Last Version: " << endl;
  cout << "	- More Comments Added throughout program." << endl;
  cout << "	- Less Bulky Room Descriptions (for now)" << endl;
  cout << endl;
}

/**********************************************************
 * MENU Menu::list()
 *
 * Description: ENUM list to display and implement the user 
 * 	chosen menu item.
 * 	
 * *******************************************************/

MENU Menu::list()
{
 MENU option = DEFAULT;
 
  int inputOption;
  int xdum = 1;	//Lower bound for input validation
  int ydum = 4; // Upper bound for input validation
  cout << endl << "------ MENU OPTIONS: ------ " <<endl;
  cout << "Enter 1: Start the Game/Play Again." << endl;
  cout << "Enter 2: Instructions/help" << endl;
  cout << "Enter 3: About the Creator and Project" << endl;
  cout << "Enter 4: Exit" << endl;  
  cout << "Enter Integer Option: ";
  inputOption = validation.intVal_bound(xdum,ydum); // input Validation


  option = static_cast<MENU>(inputOption);

  switch(option)
  {
    case START		: cout << "Selected: START." << endl;
			  break;
    case INSTRUCTIONS 	: cout << "Selected: Instructions." << endl;
			  instructions();
			  break;
    case ABOUT		: cout << "Selected: About Creator & Game" << endl;
			  about();
			  break;
    case QUIT		: cout << "Exit" << endl;
			  quit();
			  break;
    case DEFAULT	: cout << "Default Case, Still in Program" << endl;

  }

  
 return option;

}


/**********************************************************
 * void Menu::About()
 *
 * Description: About Author/Project
 *
 * *******************************************************/
void Menu::about()
{
  cout << "About The Game: " << endl;
  cout << "    This was a Final Project for one of my Intro to Computer Science Classes." << endl;
  cout << "About the Author: " << endl;
  cout << "    Enjoys Halloween and Carving/Sculpting Pumpkins." << endl;

}

/**********************************************************
 * void Menu::instructions();
 *
 * Description: Menu Option #5: Displays the game 
 * 	Instructions and Rules to the User.
 *
 *********************************************************/

void Menu::instructions()
{

cout << "Halloween Tower Instructions: " << endl;
cout << endl;
cout << "Traverse through the different rooms collecting Items and Solving Riddles :" <<endl;
cout << "	1. Some Items are Useful, some are Not" << endl;
cout << "Timer (Timer Remaining): " <<endl;
cout << "	1. Player starts w/ 100 on the timer. Timer decreased by 5 for ever location change." << endl;
cout << "How to Exit:" << endl;
cout << "	1. Select 4 from the Main Menu" << endl;

  //list(); Uncomment depending on how the main function is used
}

/**********************************************************
 * void Main::quit()
 *
 * Description: Menu Option #4: Outputs the exit messages 
 *
 * *******************************************************/

void Menu::quit()
{
 
  cout << "Thank You for playing - Halloween Tower!. " << endl;
  cout << "Good-bye" << endl;
    
}


/**********************************************************
 * Menu::~Menu()
 *
 * Description: Menu Destructor
 *
 * *******************************************************/

Menu::~Menu()
{

}

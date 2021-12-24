/**********************************************************
 * Project: Final - Text Based Game (Halloween Tower) 
 *
 * Filename: menu.hpp
 *
 * Author: Alexander Goodman
 *
 * Due Date: 13 June 2017
 *
 * Description: Menu class header file. Display and implement
 * 	the menu options for the desired program
 *
 * *******************************************************/

#ifndef MENU_HPP
#define MENU_HPP


#include <iostream>
#include <string>

#include "inputval.hpp"

// enum Class Definition

enum MENU {START=1, INSTRUCTIONS, ABOUT, QUIT, DEFAULT};


class Menu
{

  private:
		
	InputVal validation;			// InputVal Object

  public:
	Menu();			// Menu Default Constructor;
	~Menu();		// Destructor
	MENU list();		// enum to display options;
	

	void about();
	void instructions();	// Menu Option #
	void quit();		// Menu Option #


};
#endif




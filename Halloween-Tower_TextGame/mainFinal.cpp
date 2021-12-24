/**********************************************************
 * Project: Final Project - Text Based Game
 *
 * Filename: mainFinal.cpp
 *
 * Author: Alexander Goodman
 *
 * Due Date: 13 June 2017
 *
 * Program Description: HALLOWEEN TOWER. This program runs the 
 * 	Halloweent Tower game. This is a 1-player text based
 * 	game. The player needs to collect objects to relight
 * 	the Great Pumpkin and complete some challenges to
 * 	Complete the game before Time runs out.
 *
 * *******************************************************/


#include <iostream>
#include <string>

#include "menu.hpp"
#include "inputval.hpp"
#include "tower.hpp"
#include "room.hpp"
#include "roomEntrance.hpp"
#include "roomKitchen.hpp"

#include "player.hpp"
#include "item.hpp"





int main()
{
  InputVal valid;
  Menu M1;			// Menu Object
  MENU decision = M1.list();	// MENU Enum that takes the menu return value

  // Loop until the user selects QUIT
  while(decision != QUIT)
  {
    // Player the game if the user selects Play/PlayAgain
    if(decision == START)
    {
    	Tower tower;

    	tower.play();
    }

  decision = M1.list();  // Display main Menu again
  }

  return 0;
}

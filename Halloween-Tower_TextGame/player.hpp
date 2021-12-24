/**********************************************************
 * Project: Final - Text Based Game (Halloween Tower)
 *
 * Filename: player.hpp
 *
 * Author: Alexander Goodman
 *
 * Due Date: 13 June 2017
 *
 * Description: Player class Header File. Holds what
 * 	items the player currently has, time remaining,
 * 	if the player has won or been defeated.
 *
 * *******************************************************/

#ifndef PLAYER_HPP
#define PLAYER_HPP

#include <string>
#include <vector>

#include "item.hpp"



class Player
{

  private:
	std::vector<Item*> backpack;		// Vector Container to hold Items (max 5 items)
	double timer;				// Timer (based off of moves)
	bool packFull;				// returns true if backpack is full
	bool timeLeft;				// true until out of time
	bool gameComplete;			// 
	bool defeated;				// If defeated by Demon

  public:
	// Core Functions
	Player();			
	~Player();

	void addItem(Item*);			//Maybe make it a pointer?
	void removeItem();
	void displayBackpack();
	bool searchBackpack(std::string);	// Searches Item name and returns true or false
	void setTimer(int);			// Timer based off of moves
	double getTimer();			// get remaining Time
	bool getTimeStatus();			// If any time is remaining or not.

	// Game States
	void setGameStatus(bool);		// If player has won!
	bool getGameStatus();

	void setDefeated(bool);			// If player has been defeated..
	bool getDefeated();


};
#endif

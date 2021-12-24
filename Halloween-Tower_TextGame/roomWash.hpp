/**********************************************************
 * Project: Final - Text Based Game (Halloween Tower)
 *
 * Filename: roomWash.hpp
 *
 * Author: Alexander Goodman
 *
 * Due Date: 13 June 2017
 *
 * Description: Wash class Header File. Derivative class
 * 	of the Room class
 *
 * *******************************************************/

#ifndef ROOMWASH_HPP
#define ROOMWASH_HPP


#include <string>

#include "item.hpp"
#include "room.hpp"


class Wash : public Room
{
  private:
	Item *item1;		// Pointer to Item Object
	int inputRiddle;	// Input to the Riddle Function
  public:
	// Core Functions
	Wash();
	Wash(std::string);
	~Wash();

	// Derivative Class Unique Functions
	void functionSp();
	bool riddle();			// Riddle to be solved!
	void functionBarrier();
	
};
#endif

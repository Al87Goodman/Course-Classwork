/**********************************************************
 * Project: Final - Text Based Game (Halloween Tower)
 *
 * Filename: roomKichen.hpp
 *
 * Author: Alexander Goodman
 *
 * Due Date: 13 June 2017
 *
 * Description: Kitchen class Header File.  Derivative class
 * 	of the Room class.
 *
 *
 * *******************************************************/

#ifndef ROOMKITCHEN_HPP
#define ROOMKITCHEN_HPP

#include <string>

#include "room.hpp"

class Kitchen : public Room
{
  private:
	Item *item1;		// Pointers to Item Objects
	Item *item2;
	Item *item3;

  public:
	// Core Functions
	Kitchen();
	Kitchen(std::string);
	virtual ~Kitchen();

	// Derivative Class Unique Functions
	void functionSp();
	void functionBarrier();
};
#endif

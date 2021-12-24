/**********************************************************
 * Project: Final - Text Based Game (Halloween Tower)
 *
 * Filename: roomEntrance.hpp
 *
 * Author: Alexander Goodman
 *
 * Due Date: 13 June 2017
 *
 * Description: Entrance class Header File. Derivative class
 * 	of the Room class.
 *
 * *******************************************************/

#ifndef ROOMENTRANCE_HPP
#define ROOMENTRANCE_HPP


#include <string>

#include "room.hpp"
#include "item.hpp"


class Entrance : public Room
{
  private:
	Item *item1;		// Pointers to Items that reside within the room
	Item *item2;
	Item *item3;


  public:
	// Core Functions
	Entrance();
	Entrance(std::string);
	virtual ~Entrance();

	// Derivative Class Unique Functions
	void functionSp();
	void functionBarrier();	

};
#endif

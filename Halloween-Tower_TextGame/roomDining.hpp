/**********************************************************
 * Project: Final - Text Based Game (Halloween Tower)
 *
 * Filename: roomDining.hpp
 *
 * Author: Alexander Goodman
 *
 * Due Date: 13 June 2017
 *
 * Description: Dining Room class Header File.  Derivative
 * 	class of the Room class.
 *
 * *******************************************************/

#ifndef ROOMDINING_HPP
#define ROOMDINING_HPP

#include <string>


#include "room.hpp"
#include "item.hpp"


class Dining : public Room
{
  private:
	Item *item1;		// Pointers to Item Objects
	Item *item2;

  public:
	// Core Functions
	Dining();
	Dining(std::string);
	virtual ~Dining();

	// Derivative Class Unique Functions
	void functionSp();
	void functionBarrier();

};
#endif

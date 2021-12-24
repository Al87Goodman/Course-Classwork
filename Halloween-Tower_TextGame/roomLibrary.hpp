/**********************************************************
 * Project: Final - Text Based Game (Halloween Tower)
 *
 * Filename: roomLibrary.hpp
 *
 * Author: Alexander Goodman
 *
 * Due Date: 13 June 2017
 *
 * Desciption: Library class Header File.  Derivative class
 * 	of the Room class
 *
 * *******************************************************/

#ifndef ROOMLIBRARY_HPP
#define ROOMLIBRARY_HPP


#include "room.hpp"
#include "item.hpp"

class Library : public Room
{
  private:
	Item *item1;		// Pointers to Item Objects
	Item *item2;


  public: 
	// Core Functions
	Library();
	Library(std::string);
	~Library();

	// Derivative Class Unique Functions
	void functionSp();
	void functionBarrier();
};
#endif

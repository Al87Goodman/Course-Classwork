/**********************************************************
 * Project: Final - Text Based Game (Halloween Tower)
 *
 * Filename: roomCarving.hpp
 *
 * Author: Alexander Goodman
 *
 * Due Date: 13 June 207
 *
 * Description: Carving class Header File.  Derivative clas
 * 	of the Room class
 *
 *********************************************************/

#ifndef ROOMCARVING_HPP
#define ROOMCARVING_HPP

#include <string>

#include "item.hpp"
#include "room.hpp"


class Carving : public Room
{
  private:
	Item *item1;		// Pointer to Item Object
	bool greatPumpkin;
	int inputRiddle2;	// Input to Riddle Function


  public:
	// Core Functions
	Carving();
	Carving(std::string);
	~Carving();

	// Derivative Class Unique Functions
	void functionSp();
	void functionBarrier();
	bool getGreatPumpkin();
	void setGreatPumpkin(bool);	
	bool riddle2();			// Final Riddle

};
#endif

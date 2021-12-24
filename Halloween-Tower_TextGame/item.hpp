/**********************************************************
 * Project: Final - Text Based Game (Halloween Tower)
 *
 * Filename: item.hpp
 *
 * Author: Alexander Goodman
 *
 * Due Date: 13 June 2017
 *
 * Description: Item class Header File. Creates Items
 * 	that are used throughout the Game.
 *
 * *******************************************************/

#ifndef ITEM_HPP
#define ITEM_HPP


#include <string>



class Item
{

  private:
	std::string name;			// Item Name
	std::string description;		// Item Description
	bool status = false;			// Item Status (changes if in backpack or not)

  public:
	// Core Functions
	Item();					// Default Constructor
	Item(std::string);			// 1-parameter Constructor sets item Name
	~Item();				// Destructor
	
	void setName(std::string);		// Items name
	std::string getName();

	void setDescription(std::string);	// Items Description
	std::string getDescription(); 

	void setStatus(bool);			// Change Item Status depending if picked up or not
	bool getStatus();


};
#endif

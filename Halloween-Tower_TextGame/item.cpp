/**********************************************************
 * Project: Final - Text Based Game (Halloween Tower)
 *
 * Filename: item.cpp
 *
 *  Author: Alexander Goodman
 *
 *  Due Date: 13 June 2017
 *
 *  Description: Item class Implementation File (Source Code)
 *	Items status change if picked up or not
 *
 * *******************************************************/

#include <iostream>
#include <string>

#include "item.hpp"


/************************************************
 * Item::Item()
 *
 * Description: Item class default constructor
 *
 * *********************************************/

Item::Item()
{
  name ="Not ASSIGNED";		// Default Item Name
  description = "";
}

Item::Item(std::string name)
{
  setName(name);		// Sets Item's Name
}

/************************************************
 * Item::~Item()
 *
 * Description: Item class Destructor
 *
 * *********************************************/

Item::~Item()
{
  //std::cout << "Item Destructor" <<std::endl;
}



/************************************************
 * SETTERS
 *
 * *********************************************/

// Sets Item's Name
void Item::setName(std::string name)
{
  this->name = name;
}

// Sets Item's Description
void Item::setDescription(std::string description)
{
  this->description = description;
}

// Sets Item's Status 
void Item::setStatus(bool status)
{
  this->status = status;
}

/************************************************ 
 * GETTERS
 *
 * *********************************************/

// Gets Item's Name
std::string Item::getName()
{
  return name;
}

// Gets Item's Description
std::string Item::getDescription()
{
  return description;
}

// Gets Item's Status
bool Item::getStatus()
{
  return status;
}






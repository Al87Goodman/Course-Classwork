/**********************************************************
 * Program Name: ONGOING INPUT VALIDATION
 *
 * Filename: inputval.hpp
 *
 * Author: Alexander Goodman
 *
 * Description:
 *
 * Last Update: 27 April 2017
 *
 * *******************************************************/

#include <iostream>
#include <string>


#ifndef INPUTVAL_HPP
#define INPUTVAL_HPP

class InputVal
{


  public:
	InputVal();			//DEFAULT CONSTRUCTOR
 // Numbers:
	int intVal();			// Verify input is an Integer
	int intVal_pos();		// Verify input is positive integer
	int intVal_bound(int, int); 	//Verify input is an integer  within bounds 
	int intEven();			// Verify input is an Even Non-zero Integer
	double numVal_pos();		//Positive Number validation
	double numVal_bound(double, double);
 // Strings & Charactters:
	char strtochar_single(char, char);	// Make input NON-case Sensitive (convert to all caps)
	std::string strVal();		// Accepts strings with spaces

};
#endif







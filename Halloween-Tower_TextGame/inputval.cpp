/**********************************************************
 * Program Name: INPUT VALIDATION 
 *
 * Filename: inputval.cpp
 *
 * Author: Alexander Goodman
 *
 * Description: Input Validation class implementation files.
 * 	Used to check for invalid user inputs to prevent 
 * 	erroneous/erratic results 
 *
 * Last Update: 27 April 2017
 *
 * Reference: https://www.youtube.com/watch?v=S3_jCTb3fm0
 * 	This is about data validation using String Stream
 *
 * *******************************************************/

#include <iostream>
#include "inputval.hpp"
#include <string>
#include <sstream>


using std::cout;
using std::cin;
using std::endl;
using std::string;
using std::stringstream ;


/**********************************************************
 * InputVal::InputVal()
 *
 * Description: Default Constructor to activate input validation
 * 	class and to verify during debugging that it is
 * 	active.
 * *******************************************************/

InputVal::InputVal()
{
	//cout << "Activating Input Val" << endl;
}

/**********************************************************
 * InputVal::intVal();
 *
 * Description: Verifies that the input is an integer and
 * 	prompts user if it is not.
 * 
 * *******************************************************/

int InputVal::intVal()

{
  double xinput = 0 ;
  bool status = true;
  string str = "";
  
  while(status)
  {

    getline(cin,str);
  
    stringstream ss(str);

    // checks to make sure input can be a double, verifies it is not a double by static cast<int>
    if(ss >> xinput && xinput == static_cast<int>(xinput))
    {
      status = false;
    }
    else
    {
      cout << "please enter an integer" <<endl;
   
    }
  }
  return static_cast<int>(xinput);
}


/**********************************************************
 * int InputVal::intVal_pos();
 *
 * Description: Verifies that the input is a positive 
 * 	integer and prompts the user if it is not.
 *
 * *******************************************************/
int InputVal::intVal_pos()
{

  double xinput = 0 ;
  bool status = true;
  string str = "";
 // stringstream ss;
while(status)
{

  getline(cin,str);
  stringstream ss(str);
  if(ss >> xinput && xinput >= 0 && (xinput == static_cast<int>(xinput)))
  {
    status = false;
  }
  else
  {
    cout << "please enter a Positive Integer" <<endl;
   
  }
}
  return static_cast<int>(xinput);

}


/**********************************************************
 * int InputVal::intVal_bound(int lowerbound, int upperbound)
 *
 * Description: Verifies that the input is an integer within
 * 	the specified bounds and determines which is a lowerbound
 * 	and which is an upperbound.
 *
 * *******************************************************/

int InputVal::intVal_bound(int b1, int b2)
{
  int lowerbound;
  int upperbound;
  double xinput = 0 ;
  bool status = true;
  string str = "";

  // Setting the Lower and Upper Bounds 
  if(b1<b2)
  {
    lowerbound = b1;
    upperbound = b2;
  }
  else if(b2<b1)
  {
    lowerbound = b2;
    upperbound = b1;
  }
  else if(b1 == b2)
  {
    lowerbound = b1;
    upperbound = b2;
  }

while(status)
{

  getline(cin,str);
  stringstream ss(str);
  if((ss >> xinput) && (xinput==static_cast<int>(xinput)) && (xinput >= lowerbound) && (xinput <= upperbound))
  {
    status = false;
  }
  else
  {
    cout << "Please enter a Positive Integer within Bounds: "
	 << "[" << lowerbound << "," << upperbound << "]" <<endl;
   
  }
}
  return static_cast<int>(xinput);


}

/**********************************************************
 * InputVal::intEven()
 *
 * Description: Validates that the user entered a positive 
 * 	even integer nad notifies the user if not.
 *
 * *******************************************************/
int InputVal::intEven()
{
  int xinput = 0;
  bool status = true;
  string str = "";
  
  while(status)
  {
    getline(cin,str);
    stringstream ss(str);
    if(ss>>xinput && xinput%2 == 0 && xinput > 0)
    {
	status = false;
    }
    else
    {
	cout <<"Please Enter an Even Integer Value: " << endl;
    }
  }

  return static_cast<int>(xinput);
}

/**********************************************************
 * InputVal::numVal_pos()
 *
 * Description: Validates that the user entered a positive
 * 	number.
 * *******************************************************/
double InputVal::numVal_pos()
{
  double xinput = 0;
  bool status = true;
  string str = "";

  while(status)
  {
    getline(cin,str);
    stringstream ss(str);
    if(ss >> xinput && xinput >0)
    {
	status = false;
    }
    else
    {
	cout << "Pleaes Enter a Positive Number: " << endl;
    }
  }
  return xinput;
}

/**********************************************************
 * InputVal::numVal_bound(double, double)
 *
 * Description:  Validates that the user enetered a number
 * 	within the designated bounds
 * *******************************************************/

double InputVal::numVal_bound(double b1, double b2)
{

  double lowerbound;
  double upperbound;
  double xinput = 0 ;
  bool status = true;
  string str = "";

  // Setting the Lower and Upper Bounds 
  if(b1<b2)
  {
    lowerbound = b1;
    upperbound = b2;
  }
  else if(b2<b1)
  {
    lowerbound = b2;
    upperbound = b1;
  }
  else if(b1 == b2)
  {
    lowerbound = b1;
    upperbound = b2;
  }

 while(status)
 {
  getline(cin,str);
  stringstream ss(str);
  if((ss >> xinput) && (xinput >= lowerbound) && (xinput <= upperbound))
  {
    status = false;
  }
  else
  {
    cout << "Please enter a Positive Integer within Bounds: "
	 << "[" << lowerbound << "," << upperbound << "]" <<endl;
   
  }
 }
  return xinput;

}

/**********************************************************
 * InputVal::strtochar_single()
 *
 * Description: Validate input by making string input all
 * 	caps (case-insensitive) and return the first
 * 	 character in the string if it is in the desired 
 * 	 range.
 *
 * *******************************************************/
//NOTE: include cin.ignore() after implementing to skip the newline character
 
char InputVal::strtochar_single(char b1, char b2)
{
  bool status = true;
  string str = "";
  string str_upper;

  char output;  

  while(status)
  {
    getline(cin,str);
   
//Make the input all Capital Letters 
for(int i=0; i<str.size(); i++)
    {
	str_upper[i]=toupper(str[i]);
   	//cout << "Changing to Upper: " << str_upper[i] <<endl;
    }
    //Setting the input's FIRST character as the output variable
    output = str_upper[0]; 
       
    // If the input's first character is equal to b1 or b2 then input is valid
    if(output == b1 || output == b2)
    {
	status = false;
    }

    else
    {
	cout << "Please Enter " << b1 << " or " << b2 << ": "  << endl;
    }

  }
  return output;
}


/**********************************************************
 * InputVal::strVal()
 *
 * Description: To be able to accept strings with spaces
 *
 * *******************************************************/

std::string InputVal::strVal()
{
  string input = "";
  
   getline(cin, input);

  return input;
}

/*
 * Course: OSU CS344 - Intro to Operating Systems
 * Assignment: Program 04 - OTP
 * Author: Alexander Goodman
 * Due Date: 17 March 2019
 *
 * Description: User Enters in a desired pseudorandom key length and 
 * 	this program calculated it and sends to stdout.
 *
 * References:
 * 	1. PseudoRandom Generator (same as Prog02)
 * 	  a. //www.geeksforgeeks.org/generating-random-number-range-c/ 
 * 	2. asciitable
 */


#include <stdio.h>
#include <stdlib.h>
#include <string.h>
#include <time.h>


/* -- typedef and Global Variables -- */

// Define bool (never used, but always create -- maybe delete?)
typedef int bool;
#define true 1
#define false 0


// ASCII values: space = 32, A=65, Z=90
#define MAXGEN  90
#define MINGEN  64 //creating a larger bound to include @, but will replace with space
#define KEYGENSIZE = 27


/* -- Prototypes -- */
int RandNum(int, int);
char* RandomKey(int);


/**************************************
 * 		MAIN 
 * ***********************************/

int main(int argc, char* argv[])
{
  // verify the correct number of arguments (output error to stderr)
  if(argc < 2)
  {
    fprintf(stderr, "%s - not enough args\n", argv[0]);
    exit(1); 
  }

  //start the pseudo random seed
  srand(time(NULL));

  // Convert the passed in argument to an integer (is int long enough or need to use longer type?) 
  int keyLength = atoi(argv[1]);


  char* key = NULL;
  key = RandomKey(keyLength);
  printf("%s\n",key);


  // free the allocated key
  free(key);

  return 0;
}

/**************************************
 * 		FUNCTIONS
 *************************************/

/* RandNum(int x, int y)
 *
 * @param: x [int] - for the lowerbound
 * @param: y [int] - for the upperbound
 * @post: returns a pseudorandom integer
 *
 * description: Takes two integer values and first determins
 * 	the lowerbound and upperbound before running the 
 * 	pseudorandom generator, rand(), on the provided range
 * 	of values. Then returns this value.
 */

int RandNum(int x, int y)
{
  int ub=0,lb=0;
  if(x<y)
  {
    lb=x;
    ub=y;
  }
  else
  {
    lb=y;
    ub=x;
  }

  // returns a pseudorandom int within the provided bounds
  return (rand()%(ub-lb+1)+lb);
} 


/* RandomKey(int length)
 *
 * @param: length [int] - the length of the random key
 * @post: returns a malloc generated string that will need to be 
 * 	free'd
 *
 * description: Receives the desired length of a random key and calls
 * 	RandNum to calculate a value (turned into a char) and then add
 * 	to an array of chars (string) before sending this string back to
 * 	where it was called. This value will need to be deallocated
 */

char* RandomKey(int length)
{
  // create enough space for the desired key (need to add 1 for the null terminator)
  char* key = malloc((length+1)*sizeof(char));
  int i;
  int randnum;
  for(i=0;i<length;i++)
  {
    // call the pseudorandom number generator and if equals MINGEN, then replace with ascii value for space 
    randnum=RandNum(MINGEN,MAXGEN);
    if(randnum == MINGEN)
    {
	randnum = 32; // asciivalue for space
    }
    // add value to the new key
    key[i]=randnum;

  }
  // add null terminator to end of the key
  key[length] = '\0';

  return key;

}




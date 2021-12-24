/*
 * Course: OSU CS 344 - Intro to Operating Systems
 * Assignment: Program 04 - OTP
 * Author: Alexander Goodman
 * Due Date: 17 March 2019
 *
 * Description: Source Files for the Utility Functions for all the
 * 	encryption/decryption communication source files, the client
 * 	and server programs. 
 *
 * References:
 * 	1. OSU CS344 Lectures and Notes
 * 	2. Beej's Guide to Network Programming - Brian "Beej Jorgensen" Hall
 * 	  a. Section: 7.3 Handling Partial send()
 *
 */
 
 #include "otp_util.h"


/* void sendMessage(int, char*)
 * @param: Connected Socket File Descriptor
 * @param: message to send
 * description: Sends the desired message through the socket
 * 	while keeping track of the number of bytes sent incase
 * 	of interruption.

 * reference: #2
*/
void sendMessage(int sockFD, char* msg)
{
  int charsWritten = 0; //Hold the Number of Chars sent during each transmission
  int charsSent = 0;    // Holds the running tally of total chars sent
  int size = strlen(msg)+1;
  int charsLeft = size;

  while(charsSent < size)
  {
    charsWritten = send(sockFD, msg+charsSent,charsLeft,0);

    // Check if there was an error while trying to send data.
    if(charsWritten < 0){fprintf(stderr,"ERROR writing to socket/n");exit(1);}

    // update counters
    charsSent = charsSent + charsWritten;
    charsLeft = charsLeft - charsWritten; 
  }
  
}


/* int getMessage(int, char**)
 * @param: Connected Socket File Descriptor
 * @param: Buffer to read the sent message into
 * @post: -1 for error, or number of chars read in
 * description: Allocates memory for the passed in buffer, if needed,
 * 	and then loops through the received message from the 
 * 	socket file descriptor.

 * references: Ref#1 - Network Clients Lecture!
*/
int getMessage(int sockFD, char** buff)
{
  int charsTotal = 0;
  int charsRead = 0;
  char* id = NULL;  //going to be used to find end of messaged identifier '*'
  // If the buffer is null, allocate memory for it
  if(*buff == NULL)
  {
    // Using calloc to set everything to zero and able to run strcat without any errors
    *buff = calloc(BUFFSIZE, sizeof(char));  
  }

  char readBuffer[BUFFSIZE];
  int sizeAlloc = BUFFSIZE;

  // Loop until the end of message identifier is found
  while((id=strchr(*buff,'*'))== NULL)
  {
    // reset the bufer to contain all zeroes and read in the next chunk 
    memset(readBuffer,'\0',sizeof(readBuffer)); // clear buffer
    charsRead = recv(sockFD, readBuffer, sizeof(readBuffer)-1,0); // get the next chunk
    if(charsRead<0){fprintf(stderr,"ERROR - Reading in Message\n"); return -1;}

    // Check the Buffer Size and Resize if not able to add the new chunk
    if(charsRead + charsTotal >= sizeAlloc)
    {
	// Double the current allocated size
	sizeAlloc = sizeAlloc*2;
	char* temp = calloc(sizeAlloc, sizeof(char));
	strcpy(temp,*buff);
	free(*buff);
	*buff = temp;
    }
    //Concatenate the new read in chunk (since breaking messages up into 1000 byte chunks)
    strcat(*buff, readBuffer);
    charsTotal = charsTotal + charsRead;
   
  }

  // set null terminator at the end of the message.
  *id = '\0';

  return charsTotal;
}


/* int validMessage(char* msg)
 * @param: message to be checked for valid characters
 * @post: returns 0 if invalid, 1 if valid
 * description: checks to see that the passed in message
 * 	contains ONLY the valid characters
*/
int validMessage(char* msg)
{
  int valid = 1;
  int i;

  for(i=0; i<strlen(msg); i++)
  {
    // Checks to see if the character is Inbetween A-Z
    if((msg[i] < 65) || (msg[i] > 90))
    {    //checks to see if character is a space
	if(msg[i] != 32)
	{
	  valid = 0;
	  break;
	}
    }
  }

  return valid;
}


/* int allowMessage(char* msg_auth, char* msg_req)
 * @param: permission string
 * @param: request to send message
 * @post: returns 1 if allowed to send, 0 if not
 * description: checks for the permission string in the
	sent request message
*/
int allowMessage(char* msg_auth, char* msg_req)
{
  int val = 0;	
  char* pos = strstr(msg_req, msg_auth);
  if(pos)
  {
    val = 1;
  }
  
  return val;
}



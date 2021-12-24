/*
 * Course: OSU CS 344 - Intro to Operating Systems
 * Assignment: Program 04 - OTP
 * Author: Alexander Goodman
 * Program: otp_dec.c
 * Due Date: 17 March 2019
 * 
 * Description: Takes in a ciphertext, key and a port number
 * 	to communicate with a server. If communication is allowed,
 * 	the server will return origianl message using one-time pad
 * 	decryption. Before sending ciphertext and key to the server,
 * 	this program will verify the message only contains valid
 * 	message charcters. Finally, if run to completion, the 
 * 	returned message will output to stdout. 
 * 
 * References:
 *	1. Overview and Layout
 *  	  a. OSU CS 344 Group 4 Lectures and Notes
 * 	  b. http://www.linuxhowtos.org/C_C++/socket.htm
 *    	2. Getline
 *        a. OSU CS 344 Lecture Notes - 2.4 File Acces in C
 *        b. https://c-for-dummies.com/blog/?p=1112
 *	3. OSU CS344 Slack Channel
 *	  a. Bouncing Ideas Off of Each Other, Suggestions on Fixing Errors, and General Layout
 * 
 * */


/* -- Libraries -- */
// note: most libraries have been called in otp_util.h
#include <netdb.h> 
#include "otp_util.h"

/* -- GLOBALS and DEFINITIONS -- */
#define BUFFSIZE 1000

/* -- Prototypes -- */
// Provided Error Function used for reporting issues
void error(const char *msg) { perror(msg); exit(1); } 

/**************************************
 *              MAIN
 * ***********************************/

int main(int argc, char *argv[])
{
  /* -- Parameters and Definitions -- */  
  // Filestream for reading in the ciphertext and key files
  FILE* file;

  // Lengths & sizes of the key and ciphertext files
  int len_key, len_plain; 
  size_t size,size_plain=0,size_key=0; // zero for getline

  // Socket File Descriptor and Port Number 
  int socketFD, port;

  // Server Connection Info & Host Info
  struct sockaddr_in serverAddress;
  struct hostent* serverHostInfo;
	
  // Will need to deallocate all 3 buffers if used.
  char* buff_key = NULL;   // getline so deallocate
  char* buff_plain = NULL; // getline so deallocate (note: contains ciphertext not plaintext)
  char* buffer = NULL;
  char* host = "localhost"; 

  // Authorization Message for allowed communication
  char* msg_auth = "godzilla*"; // Request permission to send
  char* msg_roger = "roar";  	// Request to send granted
  int msg_size = 0;


  /* -- Verify the Number of Arguments is at least 4 -- */
  if (argc < 4) { fprintf(stderr,"USAGE: %s hostname port\n", argv[0]); exit(1); } // Check usage & args

  /* -- Open up the plaintext file for read only -- */
  // Open and Read in file contents into a string (Reference #2.a and #2.b)
  file = fopen(argv[1],"r");
  if(file == NULL){error("Error Opening %s");}

  size = getline(&buff_plain,&size_plain,file);
  if(size<0){error("Error - fopen");}
  else if(size == 0)
  {
    fprintf(stderr, "Error - File %s is Empty\n", argv[1]);
    exit(1);
  }

  // get length of the ciphertext (buff_plain)
  len_plain = strlen(buff_plain);

  // close file
  fclose(file);

  // Remove the trailing \n 
  buff_plain[strcspn(buff_plain,"\n")] = '\0';

  // Make sure Ciphertext Message ONLY contains valid characters
  if(!validMessage(buff_plain))
  {
    free(buff_plain);
    fprintf(stderr,"otp_dec Error: %s input contains bad characters\n", argv[1]);
    exit(1);
  }

  // replace the end of \0 with + to distinguish between messages after combining
  buff_plain[strcspn(buff_plain,"\0")] = '+';

  /* -- Open up the key file for read only -- */
  // Open and Read in file contents into a string (Reference #2.a and #2.b)
  file = fopen(argv[2],"r");
  if(file == NULL){fprintf(stderr,"CLIENT: ERROR Opening %s\n",argv[2]);exit(1);}

  size = getline(&buff_key,&size_key,file);
  if(size<0){error("Error - fopen");}
  else if(size == 0)
  {
    fprintf(stderr,"Error - FIle %s is Empty\n", argv[2]);
    exit(1);
  }

  // get length of the key (buff_key)
  len_key = strlen(buff_key);

  // close file
  fclose(file);

  // Remove the trailing \n 
  buff_key[strcspn(buff_key,"\n")] = '\0';

  // Make sure Key Message ONLY contains valid characters
  if(!validMessage(buff_key))
  {
    free(buff_plain);
    free(buff_key);
    fprintf(stderr,"otp_dec Error: %s input contains bad characters\n", argv[2]);
    exit(1);
  }

  buff_key[strcspn(buff_key,"\0")] = '*'; // end of message id

  /* -- Check Sizes & then Combine messages to be sent -- */
 
  // Check if len_key is >= len_plain
  if(len_key < len_plain){
    free(buff_plain);
    free(buff_key);
    fprintf(stderr, "Error: key '%s' is too short\n",argv[2]);
    exit(1);
  }

  // Create One Big Character Array to store [ciphertext+key*] 
  // strcat the two buffers (ciphertext and key) into 1 single message to send
  // Note: the two messages are separated by '+' and the combined msg has the
  // 	'*' end of message id
  buffer = calloc((len_key+len_plain+1),sizeof(char));
  strcat(buffer,buff_plain);
  strcat(buffer,buff_key); 

  /* -- Server Address, Socket, Connect -- */
  // Following is from Reference #1.a and #1.b
  // Set up the Server Address Struct & Host Info
  memset((char*)&serverAddress, '\0', sizeof(serverAddress)); // Clear out the address struct
  port = atoi(argv[3]); // Get the port number, convert to an integer from a string
  serverAddress.sin_family = AF_INET; // Create a network-capable socket
  serverAddress.sin_port = htons(port); // Store the port number

  serverHostInfo = gethostbyname(host); // Convert the machine name into a special form of address
  if (serverHostInfo == NULL) { fprintf(stderr, "CLIENT: ERROR, no such host\n"); exit(1); }
  // Copy in the address
  memcpy((char*)&serverAddress.sin_addr.s_addr, (char*)serverHostInfo->h_addr, serverHostInfo->h_length);

  // Create and set-up the Socket
  socketFD = socket(AF_INET, SOCK_STREAM, 0); 
  if (socketFD < 0)
  {
    free(buff_plain);
    free(buff_key);
    free(buffer);
    error("CLIENT: ERROR opening socket");
  } 

  // Connect to the Server (exit(2))...
  if (connect(socketFD, (struct sockaddr*)&serverAddress, sizeof(serverAddress)) < 0) 
  {
    close(socketFD);
    free(buff_plain);
    free(buff_key);
    free(buffer);
    fprintf(stderr, "Error: could not contact otp_dec_d on port %d\n", port); 
    exit(2);
  }	

  /* -- Check if Allowed to Communicate with Server -- */

  // Send Communication Request Message 
  sendMessage(socketFD, msg_auth);

  // free the key buffer and use it to store the incoming message 
  free(buff_key);
  buff_key = NULL;
  msg_size = getMessage(socketFD, &buff_key);

  // check msg_size
  if(msg_size<0)
  {
    close(socketFD);
    free(buff_plain); free(buff_key); free(buffer);
    fprintf(stderr,"Error: could not receive message from otp_dec_d on port %d\n",port); 
    exit(2);
  }  

  // check the received message against the "clear to send" message
  if(allowMessage(msg_roger,buff_key))
  {
    // Send the Combined ciphertext+key Message Stored in buffer
    sendMessage(socketFD, buffer);

    free(buffer);
    buffer = NULL;

    // Read in the returned decrypted text from client
    msg_size = getMessage(socketFD,&buffer);
    // check msg_size
    if(msg_size<0)
    {
	close(socketFD);
	free(buff_plain); free(buff_key); free(buffer);
	fprintf(stderr,"Error: could not receive message from otp_dec_d on port %d\n",port); 
	exit(2);
    }
    // Make sure there is no issue on the server side, if not then print out message  
    char* err = NULL;
    if((err = strstr(buffer, "ERROR")) != NULL)
    {
	close(socketFD);
	free(buff_plain); free(buff_key); free(buffer);
	exit(1); 
    }
    else
    {
    	// send the decrypted msg to stdout
    	printf("%s\n", buffer);
    } 	
  }

  // If not clear to send, then print out error message and exit
  else
  {
    fprintf(stderr,"[SERVER RESP]: %s on Port %d\n",buff_key,port);
    free(buff_plain);
    free(buff_key);
    free(buffer);
    close(socketFD);
    exit(2);
  }

  free(buff_plain);
  free(buff_key);
  free(buffer);
  close(socketFD); // Close the socket
  return 0;
}




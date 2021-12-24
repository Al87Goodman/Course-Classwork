/*
 * Course: OSU CS 344 - Intro to Operating Systems
 * Assignment: Program 04 - OTP
 * Author: Alexander Goodman
 * Due Date: 17 March 2019
 * Program: otp_enc_d.c
 * Description: Takes in plaintext message and the key
 * 	from the client. If the client is allowed to communicate,
 * 	then the program will encrypt the message and send it back
 * 	to the client. Program uses one-time pad encryption.
 * 
 * References:
 *	1. Overview and Layout
 * 	  a. OSU CS 344 Group 4 Lectures and Notes
 * 	  b. http://www.linuxhowtos.org/C_C++/socket.htm
 *	  c. OSU CS 344 Group 3 Lectures and Notes (fork, processes, etc)
 * 	2. SIGCHLD
 * 	  a.http://alas.matf.bg.ac.rs/manuals/lspe/snode=95.html
 * 	3. https://en.wikipedia.org/wiki/One-time_pad
 *	4. Search for Char in String
 *	  a. https://www.codingame.com/playgrounds/14213/how-to-play-with-strings-in-c/search-within-a-string 
 *	5. OSU CS344 Slack Channel
 *	  a. Bouncing Ideas Off of Each Other, Suggestions on Fixing Errors, and General Layout
 */



/* -- Libraries -- */
// note: Most Libraries called in otp_util.h
#include <signal.h>
#include <sys/wait.h>
#include <errno.h>
#include "otp_util.h"

/* -- GLOBALS and Definitions -- */
int count = 0;  // number of active connections

#define MAXCONN 5
#define BUFFSIZE 1000

/* -- Signal Handler for SIGCHLD -- */
void sigchld_handler(int signo)
{
  int status;
  pid_t PID;
  while((PID=waitpid(-1,&status,WNOHANG))>0)
  {
    count--;
  }
}

/* -- Prototypes -- */
void encrypt(char** msg, char* key);

// Provided Error Function used for reporting issues
void error(const char *msg) { perror(msg); exit(1); } 

/**************************************
 * 		MAIN
 * ***********************************/

int main(int argc, char *argv[])
{
  /* -- Parameters and Definitions  -- */

  // Listen and Connect File Descriptors, port number, and size of msg
  int listenFD, connFD, port, msg_size;

  // Client Info
  socklen_t sizeOfClientInfo;

  pid_t cpid;  

  // Buffers and messages
  char* buffer = NULL;      // Deallocate (primary buffer used)
  //char* buff_plain = NULL;  // currently not used, but may use
  //char* buff_key = NULL;    // currently not used, but may use
  char* msg_req = "trogdor";
  char* msg_val = "burninator*"; // need the '*' to indicate end of message (readMessage)
  char* msg_err01 = "ERROR -  Too Many Connections*";
  char* msg_err02 = "ERROR - Request Denied to use otp_enc_d*";
  char* msg_error = "ERROR - *"; // ambigous error message to send client if server issues

  // Server Connection Info & Client Info
  struct sockaddr_in serverAddress, clientAddress;

  /* -- Verify the Number of Arugments is at least 2 -- */
  if (argc < 2) { fprintf(stderr,"USAGE: %s port\n", argv[0]); exit(EXIT_FAILURE); } 

  /* -- Create and set-up the signal handler for SIGCHLD to prevent zombies -- */
  // References: Ref #1.b, #2.a
  struct sigaction SIGCHLD_action = {{0}};
  SIGCHLD_action.sa_handler = sigchld_handler;
  sigfillset(&SIGCHLD_action.sa_mask);
  SIGCHLD_action.sa_flags = SA_RESTART; // attempt reentry instead failing with an error

  sigaction(SIGCHLD, &SIGCHLD_action, NULL);

  /* -- Server Address, Socket, Bind -- */
  // References: Ref #1.a, #1.b
  // Set-up the address struct for this process (the server)
  memset((char *)&serverAddress, '\0', sizeof(serverAddress)); // Clear out the address struct
  port = atoi(argv[1]); // Get the port number, convert to an integer from a string
  serverAddress.sin_family = AF_INET; // Create a network-capable socket
  serverAddress.sin_port = htons(port); // Store the port number
  serverAddress.sin_addr.s_addr = INADDR_ANY; // Any address is allowed for connection to this process

  // Set-up the socket
  listenFD = socket(AF_INET, SOCK_STREAM, 0); // Create the socket
  if (listenFD < 0) error("ERROR opening socket");

  // Enable the socket to begin listening
  if (bind(listenFD, (struct sockaddr *)&serverAddress, sizeof(serverAddress)) < 0) // Connect socket to port
    error("ERROR on binding(otp_enc_d)");

  // Flip the Socket On - Now can Receive Up to 5 Connections
  listen(listenFD, 5); 


  /* -- Message Connections, Communications, and Encrypting -- */
  // References: Ref #1.b & #1.c
  // Accept a connection, blocking if one is not available until one connects
  sizeOfClientInfo = sizeof(clientAddress); // Get the size of the address for the client that will connect

  while(1)
  {
    connFD = accept(listenFD, (struct sockaddr *)&clientAddress, &sizeOfClientInfo); // Accept
    if (connFD < 0)
    {
	fprintf(stderr, "ERROR on accept\n");
	continue;
    } 

    // Check to make sure under the MAX CONNECTION COUNT
    if(count < 0){error("ERROR: Server Issue - Tracking Connection Issues\n");}
    if(count >= MAXCONN)
    {
	// Let the Client Know the Connection Count Activity is currently too high and to try again later
    	msg_size = getMessage(connFD, &buffer);
    	sendMessage(connFD,msg_err01);
    	close(connFD);

    	// free buffer and reset to NULL
    	free(buffer);
    	buffer = NULL;
	fprintf(stderr,"%s\n",msg_err01);
	continue; // Go to beginning of While loop (need to skip over the forking)
    }

    /* -- FORK NEW CHILD! -- */
    cpid = fork();

    // Creating a new Process Error
    if(cpid < 0)
    {
	fprintf(stderr,"Error Forking Child\n");
	close(connFD);
    }

    // Child Process
    else if(cpid == 0)
    {
	close(listenFD);
	/* -- Check Request to Encrypt Message from Client -- */

	// Read in Message Request
	msg_size = getMessage(connFD, &buffer);
    	if(msg_size < 0)
    	{
      	  free(buffer);
	  fprintf(stderr,"SERVER: ERROR - could Not read in message from client\n");
	  close(connFD);
      	  exit(EXIT_FAILURE);
    	}

	// Verify if the user can send a message to this server
    	if(!allowMessage(msg_req,buffer))
    	{
	  // Send Communication Request Denied Message
      	  sendMessage(connFD,msg_err02);
      	  close(connFD);
	  free(buffer);
      	  exit(EXIT_FAILURE);
    	}
    	else
    	{
	  // Send Request to Send Granted Message to Client
	  sendMessage(connFD, msg_val);
	  free(buffer);
	  buffer = NULL;

	  // Read in the combined plaintext+key message and verify message size
      	  msg_size = getMessage(connFD, &buffer);
	  if(msg_size < 0)
	  {
	    fprintf(stderr, "SERVER: ERROR - could Not read in message from client\n");
	    close(connFD);
	    free(buffer);
	    exit(EXIT_FAILURE);
	  }

	  
	  // Find and get the pointer to the id char in the message
	  // Ref #4.a
      	  char* id = strchr(buffer, '+');
      	  *id = '\0';
      	  id++; // increment over null to start of key position.

	  // Make sure the message only contains valid chars before sending
	  if(validMessage(buffer))
	  {
	    // Encrypt the Message and send the ciphertext back to the client
      	    encrypt(&buffer,id);
      	    sendMessage(connFD, buffer);
	  }
	  else
	  {
	    fprintf(stderr,"SERVER: ERROR - Buffer Contains Bad Chars\n");
	    sendMessage(connFD, msg_error);
	    close(connFD);
	    free(buffer);
	    exit(EXIT_FAILURE);
	  }

    	}

	close(connFD);
	free(buffer);
	exit(0);
    }

    // Parent Process
    else
    {
	count++; // Increment the connection count
	close(connFD); // close the existing socket which is connected to the client.
	continue;  
    }

  } // end of the big while-loop

  close(listenFD);
  return 0;
}


/**************************************
 *		FUNCTIONS
 *************************************/

/* void encrypt(char**, char*)
 * @param: the main buffer message that contains both plaintext and key
 * @param: pointer to where the key begins in the main buffer
 * description: Takes the main buffer, which has the the plaintext and
    key both combined and encrypts the message and places it back in
    the referenced main buffer.

 * reference: Ref #3
*/
void encrypt(char** msg, char* key)
{
  // Hold the plaintext, key, cipher, plaintext+key parameters
  int size = strlen(*msg);  // length up to first null (where the plaintext ends)
  int i;
  int p,k,c,pk;
  char pt, kt, ct;

  for(i=0;i<size;i++)  // or while (*msg)[i] != '\0'
  {
    // get the plaintext integer value
    pt = (*msg)[i];
//printf("PT = %d\n",pt);
    if(pt < 'A') {p = 26;}
    else{ p = pt-'A';} // or 65
//printf("P = %d\n",p);
    // get the key integer value
    kt = key[i];
//printf("KT = %d\n",kt);
    if(kt < 'A') {k = 26;}
    else{ k = kt-'A';} // or 65    
//printf("K = %d\n",k);
    // add key and plaintext together -> if over 27 then subtract 27 
    // (27 different chars in key)
    pk = p + k;
    if(pk>27){pk = pk-27;}

    // perform modulus to get ciphertext 
    c = pk % 27;
    if(c<0){c=c+27;}  //redundant?

    // convert the ciphertext integer to the proper char
    if(c == 26) { ct = 32;}
    else{ct = c + 'A';}

    // add ciphertext char value to the referenced main buffer
    (*msg)[i] = ct;
  }
  (*msg)[i] = '*';  // readMessage recognizes where to stop reading
}



/*
 * Course: OSU CS 344 - Intro to Operating Systems
 * Assignment: Program 04 - OTP
 * Author: Alexander Goodman
 * Due Date: 17 March 2019
 * Program: otp_dec_d.c
 * Description: Takes in an encrypted message and the key 
 * 	from the client. If the client is allowed to communicate,
 * 	then the program will decrypt the message and send it back
 * 	to the client.
 * 
 * References:
 *	1. Overview and Layout
 * 	  a. OSU CS 344 Group 4 Lectures and Notes
 * 	  b. http://www.linuxhowtos.org/C_C++/socket.htm
 *	  c. OSU CS 344 Group 3 Lectures and Notes (fork, processes, etc.)
 * 	2. SIGCHLD
 * 	  a.http://alas.matf.bg.ac.rs/manuals/lspe/snode=95.html
 * 	3. https://en.wikipedia.org/wiki/One-time_pad 
 *	4. Search for char in string 
 *	  a. https://www.codingame.com/playgrounds/14213/how-to-play-with-strings-in-c/search-within-a-string 
 *	5. OSU CS344 Slack Channel
 *	  a. Bouncing Ideas Off of Each Other, Suggestions on Fixing Errors, and General Layout
 */


/* -- Libraries -- */
// note: Most Libraries called in otp_util.h
#include <signal.h>
#include <errno.h>
#include <sys/wait.h>
#include "otp_util.h"

/* -- GLOBALS and Definitions -- */
int count = 0; // number of active connections

#define MAXCONN 5
#define BUFFSIZE 1000

/* -- Signal Handler for SIGCHLD -- */
void sigchld_handler(int signo)
{
  int status;
  pid_t PID;
  while((PID=waitpid(-1,&status,WNOHANG))>0)
  {
    count--;  // decrement number of connections
  }
}

/* -- Prototypes -- */
void decrypt(char** msg, char* key);

// Provided Error Function used for reporting issues
void error(const char *msg) { perror(msg); exit(1); } 

/**************************************
 *		MAIN
 * ***********************************/

int main(int argc, char *argv[])
{
  /* -- Parameters and Definitions -- */

  // Listen and Connect File Descriptors, port Number, and size of msg
  int listenFD, connFD, port, msg_size;

  // Client Info
  socklen_t sizeOfClientInfo;

  pid_t cpid;  

  // Buffers and messages
  char* buffer = NULL;      // Deallocate (primary buffer used)
  //char* buff_ciph = NULL;  // currently not used, but may use
  //char* buff_key = NULL;    // currently not used, but may use
  char* msg_req = "godzilla";
  char* msg_val = "roar*"; // need the '*' to indicate end of message (readMessage)
  char* msg_err01 = "ERROR - Too Many Connections*";
  char* msg_err02 = "ERROR -  Request Denied to Use otp_dec_d*";
  char* msg_error = "ERROR - *";  // ambiguous error message to send client if server encounters issues

  // Server Connection Info & Client Info
  struct sockaddr_in serverAddress, clientAddress;

  /* -- Verify the Number of Argumens is at least 2 -- */
  if (argc < 2) { fprintf(stderr,"USAGE: %s port\n", argv[0]); exit(EXIT_FAILURE); } 

  /* -- Create and set-up the signal handler for SIGCHLD to prevent zombies -- */
  // References: Ref #1.b, #2.a
  struct sigaction SIGCHLD_action = {{0}};
  SIGCHLD_action.sa_handler = sigchld_handler;
  sigfillset(&SIGCHLD_action.sa_mask);
  SIGCHLD_action.sa_flags = SA_RESTART; // attempt reentry instead failing with an error

  sigaction(SIGCHLD, &SIGCHLD_action, NULL);

  /* -- Server Address, Socket, Bind -- */
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
  {
    fprintf(stderr, "ERROR on binding (otp_dec_d)\n");
    exit(EXIT_FAILURE);
  }

  // Flip the Socket On - Now can Receive Up to 5 Connections
  listen(listenFD, 5); 

  /* -- Message Connections, Communications, and Decrypting -- */
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
    if(count < 0){error("ERROR: Server Issue - Counting Connection Issues");}
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
	continue; //Go to beginning of While Loop (need to skip over the forking)
    }

    /* -- FORK NEW CHILD! -- */
    cpid = fork();

    if(cpid < 0)
    {
	fprintf(stderr,"Error Forking Child\n");
	close(connFD);
	//exit(EXIT_FAILURE);
    }
    // Child Process
    else if(cpid == 0)
    {
	close(listenFD);	
	/* -- Check Request to Decrypt Message from Client  -- */

	// Read in Message Request
	msg_size = getMessage(connFD, &buffer); 
    	if(msg_size < 0)
    	{
	  fprintf(stderr,"SERVER: ERROR - could Not read in message from client\n");
	  free(buffer);
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
      	  //error("ERROR - Request Denied");
      	  exit(EXIT_FAILURE);
    	}
    	else
    	{
	  // Send Request to Send Granted Message to Client
	  sendMessage(connFD, msg_val);
	  free(buffer);
	  buffer = NULL;

	  // Read in the combined ciphertext+key message and verify message size
	  msg_size = getMessage(connFD, &buffer);
	  if(msg_size < 0)
	  {
	    fprintf(stderr, "SERVER: ERROR - could Not read in Message from Client\n");
	    free(buffer);
	    close(connFD);
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
	    // Decrypt the message and send back to the client
	    decrypt(&buffer,id);
	    sendMessage(connFD, buffer);
	  }
	  else
	  {
	    fprintf(stderr,"SERVER: ERROR - Buffer Contains Bad Chars\n"); 
	    sendMessage(connFD, msg_error);	    
	    free(buffer);
	    close(connFD);
	    exit(EXIT_FAILURE); 
	  }
	}
	free(buffer);
	close(connFD);
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

/* void decrypt(char**, char*)
 * @param: the main buffer message that contains both ciphertext and key
 * @param: pointer to where the key begins in the main buffer
 * description: Takes the main buffer, which has the the ciphertext and
    key both combined and decrypts the message and places it back in
    the referenced main buffer.

 * reference: Ref#3 
*/
void decrypt(char** msg, char* key)
{
  int size = strlen(*msg);  // length up to first null (where the plaintext ends)
  int i;

  for(i=0;i<size;i++)  // or while (*msg)[i] != '\0'
  {
    // hold the plaintext, key, ciphertext, ciphertext+key parameters
    int p,k,c,ck; 
    char pt,kt,ct;

    // get the ciphertext integer value
    ct = (*msg)[i];
    if(ct < 'A') {c = 26;}
    else{ c = ct-'A';} // or 65

    // get the key integer value
    kt = key[i];
    if(kt < 'A') {k = 26;}
    else{ k = kt-'A';} // or 65    

    // subtract the ciphertext and key -> if under 0 then add 27 
    // (27 different chars in key)
    ck = c - k;
    if(ck<0){ck = ck+27;}

    // perform modulus to get plaintext  
    p = ck % 27;
    if(p<0){p=p+27;}  // redundant?

    // convert the plaintext integer to the proper char
    if(p == 26) { pt = 32;}
    else{pt = p + 'A';}

    // add plaintext char value to the referenced main buffer
    (*msg)[i] = pt;
  }
  (*msg)[i] = '*';  // readMessage recognizes where to stop reading
}



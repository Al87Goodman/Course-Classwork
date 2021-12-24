/*
 * Course: OSU CS 344 - Intro to Operating Systems
 * Assignment: Program 04 - OTP
 * Author: Alexander Goodman
 * Due Date: 17 March 2019
 *
 * Description: Header File for the Utility Functions for all the 
 * 	encryption/decryption communication source files, the client
 * 	and server programs. It also calls in most of the used libraries
 *
 */
 
#ifndef OTP_UTIL_H
#define OTP_UTIL_H

#include <stdio.h>
#include <stdlib.h>
#include <unistd.h>
#include <string.h>
#include <sys/types.h>
#include <sys/socket.h>
#include <netinet/in.h>
#include <sys/ioctl.h>
#include <errno.h>

// Starting buffer size
#define BUFFSIZE 1000

// Send Message through Socket
void sendMessage(int, char*);

// Get Message from Socket
// Returns the total number of chars sent, or -1 if error occured
int getMessage(int, char**);

// Verify passed in message contains only valid characters
// Returns 1 if all valid chars, 0 if contains invalid chars
int validMessage(char*);

// Authenticate Communication between server and client is allowed
// Returns 1 is allowed to send, 0 if not allowed
int allowMessage(char*, char*);

#endif



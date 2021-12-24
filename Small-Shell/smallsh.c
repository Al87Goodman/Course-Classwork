/*
 * Course: OSU 344 - Intro to Operating Systems
 * Project: Program 03 - smallsh
 * Author: Alexander Goodman
 * Due Date: 03 March 2019
 *
 * Description: This program creates a small shell without many of the 
 * 	bells and whistles from the bash shell. This shell, smallsh, 
 * 	has three built in commands: cd, status, and exit.
 *
 *
 * REFERENCES:
 * 	1. OSU CS 344 Block 3 Lectures
 * 	2. For getline
 * 	  a.OSU CS 344 Lecture Notes - 2.4 File Access in C
 * 	  b. https://c-for-dummies.com/blog/?p=1112 
 * 	3. For Read and Write to Files
 * 	  a. https://linux.die.net/man/3/open
 * 	4. For waitpid(WNOHANG)
 * 	  a. https://www.gnu.org/software/libc/manual/html_node/Process-Completion.html
 * 	  b. https://linux.die.net/man/2/waitpid
 *	  c. https://stackoverflow.com/questions/11322488/how-to-make-sure-that-waitpid-1-stat-wnohang-collect-all-children-process
 * 	5. replace $$ with pid
 * 	  a. https://www.geeksforgeeks.org/c-program-replace-word-text-another-given-word/
 *	  b. https://www.kau.edu.sa/Files/830/Files/57493_Art_of_Programming_Contest_Part4.pdf
 */


/* -- Read In Libraries and Files -- */

#include <stdio.h>	 
#include <stdlib.h>	
#include <string.h>	// strcmp, strtok
#include <sys/stat.h>
#include <sys/types.h>	// pid_t
#include <sys/wait.h>	// waitpid
#include <unistd.h>	// chdir, for, exec, pid_t
#include <signal.h>
#include <errno.h>
#include <dirent.h>
#include <fcntl.h>	// Use with exec()

//dynamic array file (modified from CS 261) - used with holding bg_process id and
//	positions in the parsed input array where $$ variable expansion occurred.
#include "dynArray.h" 

/* -- TYPE DECS and GLOBALS -- */
typedef int bool;
#define false 0
#define true 1

#define MAXBUFFER 2048
#define MAXARGS 512


/* -- Prototypes -- */
char* getInput();
char** parseInput(char*, int*, DynArr*);// Uses dynArray.h
void freeMem(char*, char**, DynArr*);	// Uses dynArray.h
void freeBG(DynArr* ); 			// Uses dynArray.h
char* replacePID(char*, char*, char*);

// built-in command prototypes..in future add to separate file
int built_cd(char **args);
int built_exit(char **args);
int built_status(int);

// 
int status = 0;  	// hold the exit/termination status
pid_t pid_fg = -7;	// checks to verify fg process is running
int fg_all = 0;		// flag for values to only run in foreground
int stat_changed = 0;	// see if fg mode only is turned off or on


/* -- Signal Handlers -- */

// When SIGINT signal is caught, kill the fg process
void catchSIGINT(int signo)
{
  if(pid_fg != -7)  //or <0?
  {
    kill(pid_fg, SIGKILL);
  }

}

// When the SIGTSTP signal is caught, toggle between foreground-only mode and not
void catchSIGTSTP()
{
 // Exits foreground-only mode
  if(fg_all)
  {
    fg_all=0;  
    stat_changed = 1; // will print out message in main loop
    if(pid_fg<0)
    {
	stat_changed = 0; //will not print out message in main loop
    	char* msg_bg = "\nExiting foreground-only mode\n: ";
    	write(STDOUT_FILENO, msg_bg,32);
    }
    return;
  }
  // Enters foreground-only mode (all processes run in foreground)
  if(!fg_all)
  {
    fg_all=1;
    stat_changed = 1;
    if(pid_fg < 0)
    {
    	stat_changed=0;
    	char* msg_fg = "\nEntering foreground-only mode (& is now ignored)\n: ";
    	write(STDOUT_FILENO, msg_fg, 52);
    }
    return;
  }

}



/************************************************
 * ----- MAIN FUNCTION -----
 ***********************************************/

int main(int argc, char** argv)
{
  /* -- Set-up Dynamic Arrays (uses dynArray files) -- */

  // Dynamic Array to hold Background Process Ids 
  DynArr bg_process;
  initDynArr(&bg_process,10);  //capacity set to 10

  // Dynamic Array to hold the array index of malloc space for $$ expansion
  DynArr repPID;
  initDynArr(&repPID,20);	// capacity set to 20

  /* -- Set Main Variables that help control flow of the shell -- */

  // Loop and Shell "flag" 
  bool exit_sh = false;

  // Store User Input --> read-in and parse
  char* line = NULL;
  char** input;

  /* -- Signal Handlers -- */
  
  // Initialize the Signal Handlers to be empty  REFERENCE #1
  struct sigaction SIGINT_action = {{0}}, SIGTSTP_action = {{0}};
  
  // Block/delay all signals arriving while this mask is in place.
  // Set SIGINT signals to be ignored
  SIGINT_action.sa_handler = SIG_IGN;
  sigfillset(&SIGINT_action.sa_mask);
  SIGINT_action.sa_flags = 0;

  // Set SIGTSTP signals to use the catchSIGTSTP function when sent
  // Set SIGTSTP with SA_RESTART flag for re-entry
  SIGTSTP_action.sa_handler = catchSIGTSTP;
  sigfillset(&SIGTSTP_action.sa_mask);
  SIGTSTP_action.sa_flags = SA_RESTART;

  // Register signal handling functions 
  sigaction(SIGINT, &SIGINT_action, NULL);
  sigaction(SIGTSTP, &SIGTSTP_action, NULL);


  /* -- Opening Message -- */
  //printf("WElCOME TO THE SMALLSH..ENJOYS..\n\n");
  //fflush(stdout);
  
  /* -- Time for the Big do-while loop --> interactive part of the smallsh -- */
  do
  {
    /* -- Set loop variables to be reset each...loop -- */
    // Flag for if process should be run in bg or fg
    bool is_bg = false;	 	
    // input/output index in parsed data --> filename locations
    int idx_input = -7;  	
    int idx_output = -7;
    // index for where command arguments stop in the parsed input array
    int idx_arg = -7;
    // input/output flags
    bool is_input = false;	
    bool is_output = false;
    //store input/output operation values (if successful or not) 
    int file_in = -1;
    int file_out = -1;
    // hold the child process id (pid)
    pid_t cpid;  

    // Check if foreground process only status has changed (CTRL-Z Signal --> SIGTSTP)
    // 		If it has not printed out yet because of foreground process running then print
    if(stat_changed)
    {
	if(fg_all)
	{
	  printf("Entering foreground-only mode (& is now ignore)\n");
	  fflush(stdout);
	}
	else
	{
	  printf("Exiting foreground-only mode\n");
	  fflush(stdout);
	}
	stat_changed = 0; //reset flag
    }


    /* -- USER INPUT: read in, parse, and check for built-in commands -- */

    // Prompt, flush stdout buffer
    fflush(stdout);
    printf(": ");
    fflush(stdout);

    // Read in User input 
    int argCount = 0;
    line = getInput();  // note: free later

    // check for spaces before trying to parse only spaces. if space then treat like an empty line
    if(line[0]==' ')
    {
	free(line);
	continue;
    }

    // Parse user input (pass in user input, argument counter, dynArr for $$ expansion
    input = parseInput(line, &argCount, &repPID);  // note: free later
    idx_arg = argCount-1; // set end of arguments index currently at the last index in parsed array

    // Check for Empty Buffer or a Comment ('#');
    if(strncmp(input[0],"#",1)==0  || *input[0]=='\n')
    {
	//printf("COMMENT...\n");fflush(stdout);
    } 

    // Check if input is one of the built-in commands (cd, exit, status);
    else if(strcmp(input[0],"cd")==0)
    {
	exit_sh = built_cd(input);
    }

    else if(strcmp(input[0],"exit")==0)
    {
    	freeBG(&bg_process);
    	exit_sh=built_exit(input);
    }

    else if(strcmp(input[0],"status")==0)
    {
    	exit_sh=built_status(status);
    }
    // Check for Special Characters in the parsed data
    else
    {
    	/* -- Check for Special Characters -- */
	
    	// Check for bg process symbol
    	if(strcmp(input[argCount-1],"&")==0)
    	{
	  if(!fg_all) 	// if not in foreground only mode, set bg flag!
	  {
	    is_bg = true; 
	  }
    	}
    	// Check for input or output symbols    
    	int z;
    	for(z=0; z<argCount; z++)
    	{
	  // If file input symbol is found set flags
	  if(strcmp(input[z],"<")==0 && z+1 < argCount)
	  {
	    idx_input = z+1;	//position in index array where filename is located 
	    is_input = true;	// set file input flag
	    // end of arguments index for input array
	    if(z < idx_arg) {idx_arg = z;} 
	  }
	  // If file output symbol is found set flags
	  if(strcmp(input[z],">")==0 && z+1 < argCount)
	  {
	    idx_output = z+1;
	    is_output = true;
	    // end of arguments index for output flag
	    if(z < idx_arg){idx_arg = z;} // if symbol index position is lower, set it. 
	  }
    	}

    	/* -- LETS DO SOME FORKING! -- */
    	cpid = fork();  //store new child process id
	
    	// Child Process
	if(cpid == 0)
    	{
	  /* -- Set Signal Handlers (different if fg or bg) -- */	
	  //struct sigaction SIGINT_action = {{0}}, SIGTSTP_action = {{0}}; // May not need
	  // fg processes
	  if(!is_bg)
	  {
	    // SIGINT signals are now redirected to catchSIGINT function
	    SIGINT_action.sa_handler = catchSIGINT; 
	    sigfillset(&SIGINT_action.sa_mask);
	    SIGINT_action.sa_flags = SA_RESTART; 
	    // SIGTSTP signals are now ignored
	    SIGTSTP_action.sa_handler = SIG_IGN;

	    sigaction(SIGINT, &SIGINT_action, NULL);
	    sigaction(SIGTSTP, &SIGTSTP_action, NULL);
	  }
	  // background processes
	  else
	  {
	    // Background processes should ignore both signals.
	    SIGINT_action.sa_handler = SIG_IGN;
	    SIGTSTP_action.sa_handler = SIG_IGN;

	    sigaction(SIGINT, &SIGINT_action, NULL);
	    sigaction(SIGTSTP, &SIGTSTP_action, NULL);
	  }

	  /* -- File Input/Output Section - REFERENCE #3 -- */
	  /* How to handle stdout, stdin, exec w/ layout: Reference #1 (Block 3 Lectures and Slides)*/

	  // Input File Section

	  if(is_input)
	  {
	    //printf("INPUT FILE TO BE: %s\n",input[idx_input]);
	    file_in = open(input[idx_input], O_RDONLY);
	    // Error opening file
	    if(file_in == -1)
	    { 
	    	perror("open-filein"); 
	    	freeMem(line, input,&repPID);
	    	exit(1);
	    }
	    // Redirect stdin to file
	    if(dup2(file_in,0)==-1)
	    {
	    	perror("dup2-filein");
	    	exit(1);
	    }
	    close(file_in);

	  }
	  // if background process is not assigned a file input then redirect to /dev/null
	  else if(is_bg)
	  {
	    file_in = open("/dev/null", O_RDONLY);
	    // Error opening FIle
	    if(file_in == -1)
	    {
	    	perror("open-filein");
	    	freeMem(line, input, &repPID);
	    	exit(1);
	    }
	    // Redirect to /dev/null
	    if(dup2(file_in,0)==-1)
	    {
	    	perror("dup2-filein");
	    	exit(1);
	    }

	  }

	  // Output File Section

	  if(is_output)
	  {
	    //printf("OUTPUT FILE TO BE: %s\n", input[idx_output]);
	    file_out = open(input[idx_output], O_WRONLY | O_CREAT | O_TRUNC,0744);

	    //Error Opening
	    if(file_out == -1)
	    {
	    	perror("file_out"); //make a free functiont hat frees line and input array
	    	freeMem(line, input, &repPID);
	    	exit(1);
	    }
	    // Redirect stdout to file
	    if(dup2(file_out,1)==-1)
	    {
	    	perror("dup2-fileout");
	    	exit(1);
	    }
	    close(file_out);
	  }

	  // if background process is not assigned a file output then redirect to /dev/null
	  else if(is_bg)
	  {
	    file_out=open("/dev/null",O_WRONLY | O_CREAT);
	    // Error Opening
	    if(file_out == -1)
	    {
	    	perror("open-fileout");
	    	freeMem(line, input, &repPID);
	    	exit(1);
	    }
	    // Redirect stdout to /dev/null
	    if(dup2(file_out,1)==-1)
	    {
	    	perror("dup2-fileout");
	    	exit(1);
	    }
	  }


	  /* -- Execvp: Replace the program that was copied from the forked new process --  */
	  if(idx_arg < argCount-1)
	  {
	    // set array cutoff so it does not read file input/output symbols
	    input[idx_arg] = NULL;
	  }
	  // make sure not to read in "&" into execvp
	  if(strcmp(input[argCount-1],"&")==0){input[argCount-1] = NULL;}

	  if(execvp(input[0],input) < 0)
	  {
	  	perror("execvp");
	  	freeMem(line, input, &repPID);
	  	freeDynArr(&bg_process);
		freeDynArr(&repPID);
	  	exit(1);
	  } 

	}
    	// Error during Spawning new id
	else if(cpid < 0)
	{
	  perror("error-fork");
	}
    	// Parent Process
    	else
    	{
	  // Foreground Process must wait for the child to finish
	  if(!is_bg)
	  {
	    pid_fg = cpid;
	    waitpid(cpid,&status , 0); // suspend process until child process has terminated
	    // checks if exited normally
	    if(WIFEXITED(status)!= 0)
	    {
		//int exitStatus = WEXITSTATUS(status);
		////printf("Process Exit Status: %d\n",exitStatus);
	    }
	    // checks and returns message if terminated by a signal
	    else if(WIFSIGNALED(status) != 0)
	    {
	    	int termSignal = WTERMSIG(status);
	    	printf("Terminated by Signal: %d\n", termSignal);
	    }
	    pid_fg = -7;  // reset global variable tha checks to make sur eno fg process is running...maybe change?

	  }
	  // Background Process does Not wait for child to finish
	  else
	  {
	  	printf("background pid is %i\n", cpid); // print out background pid
	  	fflush(stdout);
	  	//add background pid to array of current processes...
	  	addDynArr(&bg_process, cpid);
	  }
	} 
    }


    /* -- free user input buffer and parse data -- */
    freeMem(line, input, &repPID);
    
    /* -- Check to see if any background proceses have finished -- */
    // References: #4a, #4b, & #4c
    while((cpid=waitpid(-1, &status, WNOHANG)) > 0)
    {
    	removeVal(&bg_process, cpid);// removes the bg pid from dyanmic array
    	printf("background pid %i is done: ",cpid);
    	fflush(stdout);
   	built_status(status);

    }

  } while(!exit_sh);  // end of this massive do-while loop..

  /* -- Deallocate the dynamic arrays used -- */
  freeDynArr(&bg_process);
  freeDynArr(&repPID);

  return 0;
}

/************************************************
 * FUNCTIONS BELOW
 ***********************************************/


/* Free Dynamic Memory Used for User Input and Parsing
 *  
 * param: line	; pointer to user input string
 * param: input	; pointer to parsed in data array
 * param: v 	; pointer to dynamic array that holds $$ exansion
 */
void freeMem(char* line, char** input, DynArr* v)
{
  // free the $$ variable expansion in input
  int idx;
  while(v->size != 0)
  {
    idx = v->data[0];
    free(input[idx]);
    removeVal(v,idx);
  }
  // free user input and parsed input
  if(input){free(input);}
  if(line){free(line);}
};

/* Free Remaining Background Processes
 * 
 * param: v	; pointer to dynamic array that holds background process ids
 */
void freeBG(DynArr* v)
{
  int stat = 0;
  pid_t chpid;
  while(v->size != 0)
  {
    // Get the process id from the dynamic array, kill it, double-tap, remove value from dyn array
    chpid = (pid_t)v->data[0];
    //printf("BG Arr Size: %d  ; PID: %d\n",v->size, chpid);
    kill(chpid, SIGKILL);
    waitpid(chpid, &stat, 0);
    removeVal(v, chpid);
  } 
}


/* Read in User Input from stdin
 * 
 *  param: none
 *  post: pointer to character array
 *  post: will need return value deallocated
 */

char* getInput()
{
  //getline attributes from REF #2
  int numCharsEntered = -5;
  size_t bufferSize = 0;//max of 2048. not setting this here so dynamically create enough without issues
  char* userInput = NULL;

  // Read in user command line input and store it in userInput string
  numCharsEntered = getline(&userInput, &bufferSize, stdin);
  
  // If the the command line was not empty, replace end of line with null terminator
  if(numCharsEntered > 1)
  {
    // if the user entered more than 2048 chars, only take the first 2048 chars.
    if(numCharsEntered > MAXBUFFER)
    {
	userInput[MAXBUFFER-1]='\0';  // replace end of max alloted buffer char with null terminator
    }
    else
    {
    	userInput[numCharsEntered-1]='\0';  // replace end of line char with null terminator
    }
  }
  return userInput;
}


/* Parse User Input by spaces and place in an Array. Also call function for variable expansion
 *
 * param: line	  ; user input
 * param: argCount; number of arguments
 * param: v	  ; dynamic array to store $$ expansion indices
 * post: pointer to an array of character pointers
 * post: will need return value deallocated
 * post: will need dynamic array deallocated
 */

char** parseInput(char* line, int* argCount, DynArr* v)
{
  // Allocate the array
  char **parseInput = malloc(MAXARGS*sizeof(char*));
  int pos = 0; 
  // Tokenize the long user input string
  char* hold = strtok(line, " ");

  // If the first character is "#", then it is a comment and do not perform variable expansion
  if(strncmp(hold,"#",1)==0)
  {
    while(hold != NULL)
    {
    	parseInput[pos] = hold;
    	pos++;
    	hold = strtok(NULL, " ");
    }
  }
  // If not a comment, then make sure to check for variable expansion
  else
  {
    while(hold != NULL)
    {	// if variable identifier is found, then perform variable expansion
	if(strstr(hold,"$$")!=NULL)
	{
	  //store pid and add to a string.
	  pid_t p = getpid();
	  char pid[10];
	  memset(pid, '\0',sizeof(pid));
	  sprintf(pid,"%d",p);
	  
	  // Call variable expansion function
	  char* result = replacePID(hold,"$$",pid);
	  // allocate enough space in the array to hold the new expanded variable
	  parseInput[pos] = (char*)malloc((strlen(result)+1)*sizeof(char));
	  strcpy(parseInput[pos],result);

	  // store the array position, so that it can be properly freed
	  // note: this is ugly, but struggled with finding a clean way
	  addDynArr(v, pos);

	  free(result);
	}
	else
	{
    	  parseInput[pos] = hold;
	}
	pos++;
    	hold = strtok(NULL, " ");
    }	

  }

  // end the array with NULL, so execvp knows ending
  parseInput[pos] = NULL;
  *argCount = pos;//pass the number of indices in the array back to the main loop
  return parseInput;
}

/* Variable Expansion - Replace everwhere [$$] with [pid]
 *
 * param: orig	; Original Word to be replaced
 * param: vari	; The variable/characters that need to be expanded
 * param: rep	; Replacement for the variable to be expanded
 * post: Returns new string! (will need to be deallocated)
 *
 * Reference: #5a & #5b
 */
char* replacePID(char* orig, char* vari, char* rep)
{
  // store the result of the variable expanded word
  char* result;
  // iterators
  int i; 
  int j = 0;
  int cnt = 0;
  // word lengths 
  int repLen = strlen(rep);	// Replacement,expanded variable
  int variLen = strlen(vari);	// original variable
  int origLen = strlen(orig);	// original word

  // Count how many times the original variable occurs in the original word
  i=0;
  while(i<origLen)
  {
    // Check position offset and if there is none, then we know it is at this location
    char* pos = strstr(&orig[i],vari);
    if(pos-&orig[i] == 0)
    {
	cnt++;  // update counter
	i = i+variLen-1;  // skip over the remaining variable section
    }
    i++;
  }

  // Allocate enough space for the new combined word (will need to be freed)
  result = malloc(origLen + cnt * (repLen-variLen)+1);


  i=0;
  j=0;
  // Create the variable expanded word 
  while(j<origLen)
  {
    // If the target string is at 0 offset then add it to the result array
    if(strstr(&orig[j],vari)-&orig[j] == 0)
    {
	strcpy(&result[i],rep);
	// skip over the rest of the target string
	i = i+repLen;
	j = j+variLen;
    }
    // If the taret string is not at 0 offset, then add the normal chars to result array
    else
    {
	result[i]=orig[j];
	j++;
	i++;
    }
  }

  // end with the null terminator
  result[i]='\0';
  
  return result;
}



/* Built-in Commands
 *  
 * Maybe move all the built in features to separate file...
 * fnct: int built_cd - changes the current directory
 * fnct: int built_exit - exits the shell (currently worthless really)
 * int built_status - checks to see if process exited normally or by signale and returns value
 */

// Change Directories using the 1st passed in argument.
// If no argument was given, then relocate to the home directory
int built_cd(char **args)
{
  // If not argument give, go to home directory
  if(args[1] == NULL)
  {
    chdir(getenv("HOME"));
  }
  else
  {
    if(chdir(args[1]) != 0)
    {
	perror("built_cd");
    }
  }

  return 0;
}


// Returns 1 to exit the main shell loop. Everything is handled in the loop...
int built_exit(char **args)
{
  // pretty worthless function for how it is currently setup...
  return 1;
}

// Returns 0, so it does not leave the shell loop.
// Checks if exited normally or terminated by signal.
int built_status(int status)
{
  // If exited normally, print exit status
  if(WIFEXITED(status)!= 0 ){
    int statExit = WEXITSTATUS(status);
    printf("exit value %d \n",statExit);
  } 
  // If terminated by signal, print signal that caused termination
  else if(WIFSIGNALED(status) != 0)
  {
    printf("terminated by signal %d\n", WTERMSIG(status));
  }

  return 0;
}











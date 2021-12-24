# Program: smallsh ; makefile
# Author: Alexander Goodman
# Due Date: 03 March 2019
# Description:
#

PROJECT = smallsh

CC = gcc
CFLAGS = -g -Wall
#CFLAGS = -std=c99

SRCS = dynArray.c smallsh.c
OBJS = ${SRCS:.c=.o} 
HDRS = dynArray.h

BIN = ${PROJECT}


all: ${BIN}

${BIN}: ${OBJS} ${HDRS}
	${CC} ${CFLAGS} -o ${BIN} ${OBJS}

.c.o:
	${CC} ${CFLAGS} -c $< -o $@


debug: ${BIN}
	valgrind --leak-check=full ./${BIN}

clean:
	rm -rf *.o junk* test* smallsh

.PHONY: clean





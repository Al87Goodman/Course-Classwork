
# Program Name: final ; makefile
# Author: Alexander Goodman
# Due Date: 13 June 2017
# Description:
# REFERENCES: Professor Terry Rooker Lecture Video in Week1 Module 

PROJECT = final

#CC = gcc
CXX = g++
#CFLAGS = -std=c99
CXXFLAGS = -std=c++0x
CXXFLAGS += -Wall # Turns on all warnings
CXXFLAGS += -pedantic-errors  #enforce standard and create warnings if violate
CXXFLAGS += -g  # debugging
#CXXFLAGS += -03  #Optimization
#LDFLAGS = -lboost_date_time  #Linker Flags (math,time - libraries, etc...)

OBJS = mainFinal.o inputval.o menu.o room.o item.o player.o tower.o roomEntrance.o roomKitchen.o roomDining.o roomLibrary.o roomWash.o roomCarving.o #$(SCR:.cpp=.o)

SRCS = mainFinal.cpp inputval.cpp menu.cpp room.cpp item.cpp player.cpp tower.cpp roomEntrance.cpp roomKitchen.cpp roomDining.cpp roomLibrary.cpp roomWash.cpp roomCarving.cpp

HEADERS = inputval.hpp menu.hpp room.hpp item.hpp player.hpp tower.hpp roomEntrance.hpp roomKitchen.hpp roomDining.hpp roomLibrary.hpp roomWash.hpp roomCarving.hpp

#TEXTFILES = L8Original.txt L8Early.txt L8Middle.txt L8Last.txt

#SORTEDFILES = sortedOrig.txt sortedEarly.txt sortedMiddle.txt sortedLast.txt

BIN = ${PROJECT}

${BIN}: ${OBJS} ${HEADERS}
	${CXX} ${LDFLAGS} ${OBJS} -o ${BIN}

${OBJS}: ${SRCS}
	${CXX} ${CXXFLAGS} -c ${@:.o=.cpp} 

debug: ${BIN}
	valgrind --leak-check=full ./${BIN}

clean:
	rm -rf *o final 

zip:
	zip ${PROJECT}_Goodman_Alexander.zip *.cpp *.hpp *.pdf makefile #${TEXTFILES} ${SORTEDFILES} makefile

.PHONY: clean







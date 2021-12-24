/* 	dynArray.h : Dynamic Array implementation. */

/******
 * ORIGINALLY CREATED FOR CS 261, but modified for CS 344.
 *  See the Implementation File for more details.
 *
*******/

#ifndef DYNAMIC_ARRAY_INCLUDED
#define DYNAMIC_ARRAY_INCLUDED 1

/* This originally used a separate TYPE header and source files  to make it more portable.
	This was done away with and just made TYPE, type int.

*/
#ifndef _TYPE
#define _TYPE
#define TYPE int
#define TYPE_SIZE sizeof(int)
#endif


/* function used to compare two TYPE values to each other */
int compare(TYPE left, TYPE right);




struct DynArr
{
	TYPE *data;		/* pointer to the data array */
	int size;		/* Number of elements in the array */
	int capacity;	/* capacity ofthe array */
};
typedef struct DynArr DynArr;


/* Dynamic Array Functions */
void initDynArr(DynArr *v, int capacity);	
DynArr *newDynArr(int cap);

void freeDynArr(DynArr *v);
void deleteDynArr(DynArr *v);

void _dynArrSetCapacity(DynArr *v, int newCap);
int sizeDynArr(DynArr *v);

void addDynArr(DynArr *v, TYPE val);
TYPE getDynArr(DynArr *v, int pos);
void putDynArr(DynArr *v, int pos, TYPE val);
void swapDynArr(DynArr *v, int i, int  j);
void removeAtDynArr(DynArr *v, int idx);
int isEmptyDynArr(DynArr *v);

void copyDynArr(DynArr *source, DynArr *destination);

int _getIndex(DynArr*,TYPE);
void removeVal(DynArr*, TYPE);


#endif

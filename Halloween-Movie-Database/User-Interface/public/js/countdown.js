/*
 * Description: Countdown until Halloween
 *
 * REFERENCE: w3schools.com/setInterval
 *
 * NOTE: Lots of work needs to be done. Future versions:
 	- Add the other 7 Sabbats
 	- Not start the countdown over until the Sabbat is officially over 
 		x Not just the start..
*/


// Countdown with respect to Local Machine
 function countdown(e){
	var now = new Date();

 	var eventThisYear = new Date(Date.UTC(now.getUTCFullYear(), 9, 31));
 	var eventNextYear = new Date(Date.UTC(now.getUTCFullYear()+1, 9, 31));

 	//var currentTime = now_utc;
 	var currentTime = now.getTime();
	var local = currentTime - (now.getTimezoneOffset()*60000);  // 60,000 milliseconds in a minute
 	var eventTime = eventThisYear.getTime();

	// If the current date is past the holiday this current year, countdown using the following year
 	if(currentTime > eventTime){
 		eventTime = eventNextYear.getTime();
 	}
 	var remTime = eventTime - local;
	
	var s = Math.floor(remTime/1000);
	var m = Math.floor(s/60);
	var h = Math.floor(m/60);
	 var d = Math.floor(h/24);

	 h%=24;
	 m%=60;
	 s%=60;

	 if(h < 10){
	 	h="0"+h;
	 }
	if(m < 10){
		m="0"+m;
	}
	if(s<10){
		s="0"+s;
	}

   	// Populates the html with the calculated content
	document.getElementById("days").textContent = d;
	document.getElementById("hours").textContent = h;
	document.getElementById("minutes").textContent = m;
	document.getElementById("seconds").textContent = s;
	document.getElementById("timezone").textContent = "Local";

 }


// Countdown with respect to UTC 
function countdown_utc(){

	var now = new Date();
/*var date = new Date(); 
var now_utc =  Date.UTC(date.getUTCFullYear(), date.getUTCMonth(), date.getUTCDate(),
 date.getUTCHours(), date.getUTCMinutes(), date.getUTCSeconds());*/

 	var eventThisYear = new Date(Date.UTC(now.getUTCFullYear(), 9, 31));
 	var eventNextYear = new Date(Date.UTC(now.getUTCFullYear()+1, 9, 31));

 	//var currentTime = now_utc;
 	var currentTime = now.getTime();
 	var eventTime = eventThisYear.getTime();

	// If the current date is past the holiday this current year, countdown using the following year
 	if(currentTime > eventTime){
 		eventTime = eventNextYear.getTime();
 	}
 	var remTime = eventTime - currentTime;

	var local = currentTime - (now.getTimezoneOffset()*60000);
	var remTime2 = eventTime - local;


 	var s = Math.floor(remTime/1000);
 	var m = Math.floor(s/60);
 	var h = Math.floor(m/60);
 	var d = Math.floor(h/24);

 	//remainders to see if to display a 0 or not
 	h%=24;
 	m%=60;
 	s%=60;

 	if(h < 10){
 		h="0"+h;
 	}
	if(m < 10){
 		m="0"+m;
 	}
 	if(s<10){
 		s="0"+s;
 	}

    // Populates the html with the calculated content
    document.getElementById("timezone").textContent = "UTC";
 	document.getElementById("days").textContent = d;
 	document.getElementById("hours").textContent = h;
 	document.getElementById("minutes").textContent = m;
 	document.getElementById("seconds").textContent = s;


}

// To display Countdown with respect to the local machine
function counter_local(){
  var localCount = setInterval(countdown,100);

  document.getElementById("utc").addEventListener("click", function(e){
  	clearInterval(localCount);
  	counter_utc(); 
  	}); 


}

// to display the Countdown with respect to UTC 
function counter_utc(){
var utcCount = setInterval(countdown_utc,100);

  document.getElementById("local").addEventListener("click", function(e){
  	clearInterval(utcCount);
  	counter_local(); 
  	}); 

}


// Default call and make sure the DOM is fully loaded before displaying the content
document.addEventListener('DOMContentLoaded', counter_local);





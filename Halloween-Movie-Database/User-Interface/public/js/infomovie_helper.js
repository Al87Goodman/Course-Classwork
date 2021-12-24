// Description: This is a helper file to infoMovie.js and infoMovie.handlebars
// 				When the page is loaded, checks to see if the movie is in Watchlist or Favorites

document.addEventListener("DOMContentLoaded", getStatus);

//document.getElementById("").addEventListener("click", statusWatch);


function getStatus(){
  var statusWatch = document.getElementById("statusWatch").value;
  var statusFave = document.getElementById("statusFave").value;

  //Third option: statusWatch = "noAccount" means to keep add and remove disabled..
  if(statusWatch === "inWatchlist"){
/*    document.getElementById("addWatchBtn").disabled = true;
    document.getElementById("removeWatchBtn").disabled = false; */
    document.getElementById("addWatchBtnCheck").disabled = true;
    document.getElementById("removeWatchBtnCheck").disabled = false; 
  }
  else if(statusWatch === "noAccount"){
/*    document.getElementById("addWatchBtn").disabled = true;
    document.getElementById("removeWatchBtn").disabled = true; 	*/
    document.getElementById("addWatchBtnCheck").disabled = true;
    document.getElementById("removeWatchBtnCheck").disabled = true;  
  }
  else{
/*    document.getElementById("addWatchBtn").disabled = false;
    document.getElementById("removeWatchBtn").disabled = true; */
    document.getElementById("addWatchBtnCheck").disabled = false;
    document.getElementById("removeWatchBtnCheck").disabled = true; 
  }

  if(statusFave === "inFavorites"){
/*    document.getElementById("addFaveBtn").disabled = true;
    document.getElementById("removeFaveBtn").disabled = false; */
    document.getElementById("addFaveBtnHeart").disabled = true;
    document.getElementById("removeFaveBtnHeart").disabled = false; 
  }
  else if(statusFave === "noAccount"){
/*    document.getElementById("addFaveBtn").disabled = true;
    document.getElementById("removeFaveBtn").disabled = true; */
    document.getElementById("addFaveBtnHeart").disabled = true;
    document.getElementById("removeFaveBtnHeart").disabled = true; 
  }
  else{
/*    document.getElementById("addFaveBtn").disabled = false;
    document.getElementById("removeFaveBtn").disabled = true; */
    document.getElementById("addFaveBtnHeart").disabled = false;
    document.getElementById("removeFaveBtnHeart").disabled = true; 

  }


};




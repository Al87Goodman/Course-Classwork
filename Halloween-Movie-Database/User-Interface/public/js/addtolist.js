// Description: ADD or REMOVE from Watchlist/Favorites
// Used in infoMovie.handlebars & infoMovie.js


function addWatch(id){
 
  if(id){
    $.ajax({
        url: '/infoMovie/addWatchlist/' + id,
        type: 'POST',
        data: $('#note').serialize(),
        //data: '[1,2,3,4]',
        success: function(result){
            window.location.reload(true);
        }
    })
  }

};


function addFave(id){
  if(id){
    $.ajax({
        url: '/infoMovie/addFavorites/' + id,
        type: 'POST',
        data: $('#noteFave').serialize(),
        //data: '[1,2,3,4]',
        success: function(result){
            window.location.reload(true);
        }
    })
  }

};


function removeWatch(id){
  console.log(id);
    $.ajax({
        url: '/infoMovie/removeWatch/' + id,
        type: 'DELETE',
        success: function(result){
            window.location.reload(true);
        }
    })
};


function removeFave(id){
  console.log(id);
    $.ajax({
        url: '/infoMovie/removeFave/' + id,
        type: 'DELETE',
        success: function(result){
            window.location.reload(true);
        }
    })
};

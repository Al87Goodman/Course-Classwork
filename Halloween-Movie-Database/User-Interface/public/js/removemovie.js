// Currently used in the accounts.js file for the profile handlebars.

function removeWatchlist(id){
	console.log(id);
    $.ajax({
        url: '/profile/removeWatch/' + id,
        type: 'DELETE',
        success: function(result){
            window.location.reload(true);
        }
    })
};


function removeFavorites(id){
	console.log(id);
    $.ajax({
        url: '/profile/removeFave/' + id,
        type: 'DELETE',
        success: function(result){
            window.location.reload(true);
        }
    })
};

function removeWatchlistDev(id){
    //console.log(id);
    //console.log(document.getElementById("userID").value);
    $.ajax({
        url: '/devUsers/removeWatch/' + id,
        type: 'PUT',
        data: $('#userID').serialize(),
        success: function(result){
            window.location.reload(true);
        }
    })
};

function removeFavoritesDev(id){
    //console.log(id);
    //console.log(document.getElementById("userID").value);
    $.ajax({
        url: '/devUsers/removeFave/' + id,
        type: 'PUT',
        data: $('#userID').serialize(),
        success: function(result){
            window.location.reload(true);
        }
    })
};

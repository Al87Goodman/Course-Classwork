function searchUsername() {
	//console.log("In searchusers.js");
    //get the Movie Title
    $.ajax({
        url: '/devUsers/searchUsername/' + encodeURI(document.getElementById('unameSearch').value),
        type: 'GET',
        data: $('#searchUsers').serialize(),
        success: function(result){
            window.location.replace("/devUsers/searchUsername/" + encodeURI(document.getElementById('unameSearch').value));
        }
    })

}

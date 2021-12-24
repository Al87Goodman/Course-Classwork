function searchMovieByTitle() {
	//console.log("In searchmovie.js");
    //get the Movie Title
    var search_title  = document.getElementById('search_title').value;
    //construct the URL and redirect to it
/*    window.location = './devMovie/searchTitle/' + encodeURI(search_title)*/
  if(search_title){
  $.ajax({
    url: '/devMovie/searchTitle/' + encodeURI(document.getElementById('search_title').value),
    type: 'GET',
    data: $('#search_movie').serialize(),
    success: function(result){
        window.location.replace("/devMovie/searchTitle/" + encodeURI(document.getElementById('search_title').value));
    }
  })
 }
 else{
    window.location.reload(true);
 }
}

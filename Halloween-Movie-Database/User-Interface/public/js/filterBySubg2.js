function filterBySubg() {
	//NOT USED ANYMORE since multiple ids need to be selected..
    
	//get the id of the selected homeworld from the filter dropdown
    var filterSubg_id = document.getElementById('filter_subg').value


	
	//console.log($("#filter_charc").val(id));
    
	
	//construct the URL and redirect to it
    window.location = '/movie/filterSubg/' + parseInt(filterSubg_id)
}

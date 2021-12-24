function filterByCharc() {
    //get the id of the selected homeworld from the filter dropdown
    var filterCharc_id = document.getElementById('filter_charc').value


	
	//console.log($("#filter_charc").val(id));
    
	
	//construct the URL and redirect to it
    window.location = '/devMovie/filterCharc/' + parseInt(filterCharc_id)
}

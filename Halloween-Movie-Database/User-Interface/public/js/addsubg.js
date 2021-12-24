function addMovSubg(id){
  if(document.getElementById("subgAdd").value){
    $.ajax({
        url: '/devMovie/addSubg/' + id,
        type: 'PUT',
        data: $('#addMovSubg').serialize(),
        success: function(result){
            window.location.replace("./");
        }
    })

  }
  else{
		return;
  }

};

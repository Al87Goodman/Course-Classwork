function addMovCharc(id){
  if(document.getElementById("charcAdd").value){
    $.ajax({
        url: '/devMovie/addCharc/' + id,
        type: 'PUT',
        data: $('#addMovCharc').serialize(),
        success: function(result){
            window.location.replace("./");
        }
    })

  }
  else{
		return;
  }

};

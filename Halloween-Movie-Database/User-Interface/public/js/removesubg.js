function removeMovSubg(id){
    $.ajax({
        url: '/devMovie/removeSubg/' + id,
        type: 'PUT',
        data: $('#removeMovSubg').serialize(),
        success: function(result){
            window.location.replace("./");
        }
    })
};

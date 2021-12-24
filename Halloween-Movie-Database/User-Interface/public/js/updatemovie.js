function updateMovie(id){
    $.ajax({
        url: '/devMovie/' + id,
        type: 'PUT',
        data: $('#movieEdit').serialize(),
        success: function(result){
            window.location.replace("./");
        }
    })
};

function deleteMovie(id){
    $.ajax({
        url: '/devMovie/' + id,
        type: 'DELETE',
        success: function(result){
            window.location.reload(true);
        }
    })
};



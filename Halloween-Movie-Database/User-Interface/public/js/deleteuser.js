function deleteUser(id){
    $.ajax({
        url: '/devUsers/' + id,
        type: 'DELETE',
        success: function(result){
            window.location.reload(true);
        }
    })
};


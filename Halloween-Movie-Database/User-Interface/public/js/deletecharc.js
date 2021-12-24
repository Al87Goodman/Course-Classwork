function deleteCharc(id){
    $.ajax({
        url: '/editCharc/' + id,
        type: 'DELETE',
        success: function(result){
            window.location.reload(true);
        }
    })
};


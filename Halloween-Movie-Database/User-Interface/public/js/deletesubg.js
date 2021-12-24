function deleteSubg(id){
    $.ajax({
        url: '/editSubg/' + id,
        type: 'DELETE',
        success: function(result){
            window.location.reload(true);
        }
    })
};


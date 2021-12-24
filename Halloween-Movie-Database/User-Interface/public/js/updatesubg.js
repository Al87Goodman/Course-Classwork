function updateSubg(id){
    $.ajax({
        url: '/editSubg/' + id,
        type: 'PUT',
        data: $('#editSubg').serialize(),
        success: function(result){
            window.location.replace("./");
        }
    })
};

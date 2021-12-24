function updateCharc(id){
    $.ajax({
        url: '/editCharc/' + id,
        type: 'PUT',
        data: $('#editCharc').serialize(),
        success: function(result){
            window.location.replace("./");
        }
    })
};

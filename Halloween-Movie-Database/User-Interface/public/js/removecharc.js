function removeMovCharc(id){
    $.ajax({
        url: '/devMovie/removeCharc/' + id,
        type: 'PUT',
        data: $('#removeMovCharc').serialize(),
        success: function(result){
            window.location.replace("./");
        }
    })
};

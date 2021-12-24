function updateUser(id){
    $.ajax({
        url: '/profile/' + id,
        type: 'PUT',
        data: $('#updateUser').serialize(),
        success: function(result){
            window.location.replace("./profile");
        }
    })
};

function updateUserDev(id){
    $.ajax({
        url: '/devUsers/' + id,
        type: 'PUT',
        data: $('#editUser').serialize(),
        success: function(result){
            window.location.replace("./");
        }
    })
};


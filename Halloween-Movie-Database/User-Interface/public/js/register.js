function register(){
	console.log("IN REGISTER");
    $.ajax({
        url: '/register',
        type: 'GET',
        success: function(result){
            window.location.replace("./register");
        }
    })
};


var express = require('express');
var router = express.Router();

var expressValidator = require('express-validator');
var passport = require('passport');

var bcrypt = require('bcrypt');
const saltRounds = 10;


/* ______________FUNCTIONS ____________ */

/* ----- ADD Functions -----*/

function addUser(res, mysql, body, hash, complete2){
	var sql = "INSERT INTO accounts (uname, fname, lname, email, password, date_created) VALUES (?, ?, ?, ?, ?,now())";
	var inserts = [body.username, body.fname, body.lname, body.email, hash];
	sql = mysql.pool.query(sql, inserts, function(error, results, fields){
		if(error){
			console.log(JSON.stringify(error));
			res.write(JSON.stringify(error));
			res.end();
		}
		else{
			complete2();
		}
	});
};

/* ----- GET Functions ----- */

function getUsers(res, mysql, context, complete){
	mysql.pool.query("SELECT u_id as id, uname, fname, lname, email, date_created FROM accounts",
		function(error, results, fields){
            if(error){
                res.write(JSON.stringify(error));
                res.end();
            }
            context.users = results;
            complete();
        });
};

function getUser(res, mysql, context, id, complete){
	var sql = "SELECT u_id as id, fname, lname, uname, email, date_created FROM accounts WHERE u_id = ?";
	var inserts = [id];
	mysql.pool.query(sql, inserts, function(error, results, fields){
		if(error){
			res.write(JSON.stringify(error));
			res.end();
		}
		else{
			context.user = results[0];
			complete();
		}
	});
};

function getFavorites(res, mysql, context, id, complete){
	var sql = "SELECT f.u_id, m.mid, m.title, f.date_added FROM favorites f JOIN movie m ON f.mid=m.mid WHERE f.u_id = ?";
	var inserts = [id];
	mysql.pool.query(sql, inserts, function(error, results, fields){
		if(error){
			res.write(JSON.stringify(error));
			res.end();
		}
		else{
			context.fave = results;
			complete();
		}
	});
};

function getWatchlist(res, mysql, context, id, complete){
	var sql = "SELECT w.u_id, m.mid, m.title, w.date_added FROM watchlist w JOIN movie m ON w.mid=m.mid WHERE w.u_id = ?";
	var inserts = [id];
	mysql.pool.query(sql, inserts, function(error, results, fields){
		if(error){
			res.write(JSON.stringify(error));
			res.end();
		}
		else{
			context.watch = results;
			complete();
		}
	});
};



/* ----- EDIT/UPDATE Functions ----- */

function updateUser(res, mysql, req, complete){
	var sql = "UPDATE accounts SET fname=?, lname=?, uname=?, email=? WHERE u_id=?";
	var inserts = [req.body.fname, req.body.lname, req.body.uname, req.body.email, req.params.id];
	sql = mysql.pool.query(sql, inserts, function(error, results, fields){
		if(error){
        	console.log(JSON.stringify(error))
            res.write(JSON.stringify(error));
            res.end();
		}
		else{
			complete();
		}
	});
};


/* ----- FILTER/SEARCH Functions ----- */

function getUsername(req, res, mysql, context, complete) {
    var query = "SELECT u_id as id, uname, fname, lname, email, date_created FROM accounts a WHERE a.uname LIKE " + mysql.pool.escape('%'+req.params.s + '%');
    mysql.pool.query(query, function(error, results, fields){
        if(error){
            res.write(JSON.stringify(error));
            res.end();
        }
        //console.log(results);
        context.users = results;
        complete();
    });
};	


/* --- VERIFY USERNAME AND EMAIL ARE AVAILABLE --- */

function checkUsername(res, mysql, context, username, complete){
	var sql = "SELECT uname FROM accounts WHERE uname=?"
	var inserts=[username];
	mysql.pool.query(sql, inserts, function(error, results, fields){
		if(results.length>0){
			//console.log("USERNAME IS BEING USED");
			context.checkUsername = "Username is Being Used - Choose Another";
			complete();
		}
		else{
			//console.log("Username is NOTT Being USED");
			complete();
		}

	});

};


function checkEmail(res, mysql, context, email, complete){
	var sql = "SELECT email FROM accounts WHERE email=?"
	var inserts=[email];
	mysql.pool.query(sql, inserts, function(error, results, fields){
		if(results.length>0){
			//console.log("Email Address is currently registered");
			context.checkEmail = "Email Address is currently registered - Choose Another";
			complete();
		}
		else{
			//console.log("Email is NOTTT being used");
			complete();
		}

	});

};



/* __________________ ROUTES __________________ */

/* ----- Get Routes ----- */

router.get('/', function(req, res){
	console.log("devUsers - Get All Users");
	var callbackCount = 0;
	var context = {};
	context.jsscripts=["deleteuser.js", "searchusers.js"];
	context.title = "Dev/Admin Users Page";
	var mysql = req.app.get('mysql');
	getUsers(res,mysql, context, complete);
	function complete(){
		callbackCount++;
		if(callbackCount >=1){
			res.render('devUsers', context);
		}
	}
});


/* ----- Add Routes ----- */

router.post('/', function(req, res, next) {
	console.log("devUsers.js - Add User");
	
	req.checkBody('username', 'Username field cannot be empty.').notEmpty(); 
	req.checkBody('username', 'Username must be between 4-25 characters long.').len(4, 25); 
	req.checkBody('email', 'The email you entered is invalid, please try again.').isEmail(); 
	req.checkBody('email', 'Email address must be between 4-100 characters long, please try again.').len(4, 100); 
	req.checkBody('password', 'Password must be between 8-100 characters long.').len(8, 100); 
	req.checkBody("password", "Password must include one lowercase character, one uppercase character, a number, and a special character.").matches(/^(?=.*\d)(?=.*[a-z])(?=.*[A-Z])(?!.* )(?=.*[^a-zA-Z0-9]).{8,}$/, "i");
	req.checkBody('passwordMatch', 'Password must be between 8-100 characters long.').len(8, 100); 
	req.checkBody('passwordMatch', 'Passwords do not match, please try again.').equals(req.body.password); 
	// Additional validation to ensure username is alphanumeric with underscores and dashes 
	req.checkBody('username', 'Username can only contain letters, numbers, or underscores.').matches(/^[A-Za-z0-9_-]+$/, 'i');

	const errors = req.validationErrors();

	if(errors){
		console.log('errors: $JSON.stringify(errors)');
		res.render('devUsers', {title: 'Registration Error',
								errors: errors
		});
	}
	else{
		console.log("Checking Username and Email are available");
		var callbackCount = 0;
		var context = {};
		var mysql = req.app.get('mysql');
		checkUsername(res, mysql, context, req.body.username, complete);
		checkEmail(res, mysql, context, req.body.email, complete);
		function complete(){
			callbackCount++;
			if(callbackCount >=2){
				if(Object.keys(context).length != 0){
					console.log("USERNAME/EMAIL Already in Database.");
					context.title="Registration Error";
					//console.log(JSON.stringify(context));
					res.render('devUsers',context);
				}
				else{
					var callbackCount2 = 0;
					//var mysql = req.app.get('mysql');
					const db = require('./db.js');
					
					//console.log(req.body.username);
					var body = req.body;
					bcrypt.hash(req.body.password, saltRounds, function(err, hash) {
						addUser(res, db, body, hash, complete2);
						function complete2(){
							callbackCount2++;
							if(callbackCount >= 1){
								res.redirect('devUsers');
							}
						}
					});

				}
			}
		}
	}

});




/* ----- Edit/Update Routes ----- */

router.get('/:id', function(req, res){
	console.log("devUsers.js - Get User");
	var callbackCount = 0;
	var context = {};
	context.jsscripts = ["updateuser.js","removemovie.js"];
	var mysql = req.app.get('mysql');
	getUser(res, mysql, context, req.params.id, complete);
	getFavorites(res, mysql, context, req.params.id, complete);
	getWatchlist(res, mysql, context, req.params.id, complete);
	function complete(){
		callbackCount++;
		if(callbackCount >= 3){
			res.render('editUser', context);
		}
	}
		
});


router.put('/:id', function(req, res){
	console.log("devUsers.js - UPDATE USER");
	var callbackCount = 0;
	var mysql = req.app.get('mysql');
	updateUser(res, mysql, req, complete);
	function complete(){
		callbackCount++;
		if(callbackCount >=1){
			res.status(200).end();
		}
	}
});


/* ----- Search / Filter Routes ----- */

router.get('/searchUsername/:s', function(req, res){
	console.log("devUsers.js - Search By Username");
    var callbackCount = 0;
    var context = {};
    context.jsscripts = ["deleteuser.js","searchusers.js"];
    var mysql = req.app.get('mysql');
    getUsername(req, res, mysql, context, complete);
    function complete(){
        callbackCount++;
        if(callbackCount >= 1){
            res.render('devUsers', context);
        }
    }
});	



/* ----- Delete Routes ----- */

router.delete('/:id', function(req, res){
    var mysql = req.app.get('mysql');
    var sql = "DELETE FROM accounts WHERE U_id = ?";
    var inserts = [req.params.id];
    sql = mysql.pool.query(sql, inserts, function(error, results, fields){
        if(error){
            res.write(JSON.stringify(error));
            res.status(400);
            res.end();
        }else{
            res.status(202).end();
        }
    })
});



router.put('/removeWatch/:id', function(req, res){
	console.log("devUser.js - Remove From Watchlist" + JSON.stringify(req.body));
    var mysql = req.app.get('mysql');
    var sql = "DELETE FROM watchlist WHERE U_id = ? AND mid = ?";
    var inserts = [req.body.userID, req.params.id];
    sql = mysql.pool.query(sql, inserts, function(error, results, fields){
        if(error){
            res.write(JSON.stringify(error));
            res.status(400);
            res.end();
        }else{
            res.status(202).end();
        }
    })
});

router.put('/removeFave/:id', function(req, res){
	console.log("devUser.js - Remove From Favorites" + JSON.stringify(req.body));
    var mysql = req.app.get('mysql');
    var sql = "DELETE FROM favorites WHERE U_id = ? AND mid = ?";
    var inserts = [req.body.userID, req.params.id];
    sql = mysql.pool.query(sql, inserts, function(error, results, fields){
        if(error){
            res.write(JSON.stringify(error));
            res.status(400);
            res.end();
        }else{
            res.status(202).end();
        }
    })
});






passport.serializeUser(function(user_id, done) {
  done(null, user_id);
});

passport.deserializeUser(function(user_id, done) {
    done(null, user_id);
});

// To restrict access to particular pages...
// REFERENCE: https://gist.github.com/christopher4lis/f7121a07740e5dbca860c9beb2910565
function authenticationMiddleware () {  
	return (req, res, next) => {
		console.log(`req.session.passport.user: ${JSON.stringify(req.session.passport)}`);
		var user_id = req.session.passport.user;
	    if (req.isAuthenticated()) return next();
	    res.redirect('/devUsers')
	}
}


module.exports = router;


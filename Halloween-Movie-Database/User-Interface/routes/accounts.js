var express = require('express');
var router = express.Router();

var expressValidator = require('express-validator');
var passport = require('passport');

var bcrypt = require('bcrypt');
const saltRounds = 10;


/* ----- FUNCTIONS ----- */

function getUser(res, mysql, context, id, complete){
	var sql = "SELECT u_id, fname, lname, uname, email, date_created FROM accounts WHERE u_id = ?";
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



function addWatchlist(res, mysql, req, id, complete){
	if(req.user){
 		var sql = "INSERT INTO watchlist (u_id, mid, date_added) VALUES (?,?,now())";
        var inserts = [req.user, id];
        sql = mysql.pool.query(sql,inserts,function(error, results, fields){
            if(error){
                console.log(JSON.stringify(error))
                res.write(JSON.stringify(error));
                res.end();
			}
            complete();
        })
	}else{
		complete();
	} 
};	


function addFavorites(res, mysql, req, id, complete){
	if(req.user){
 		var sql = "INSERT INTO favorites (u_id, mid, date_added) VALUES (?,?,now())";
        var inserts = [req.user, id];
        sql = mysql.pool.query(sql,inserts,function(error, results, fields){
            if(error){
                console.log(JSON.stringify(error))
                res.write(JSON.stringify(error));
                res.end();
			}
            complete();
        })
	}else{
		complete();
	} 
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









/* _______________ ROUTES _______________ */

router.get('/check', function(req, res){
	console.log("USER AUTHENTICATION STATUS: ");
	console.log(req.user);
	console.log(req.isAuthenticated());
	res.render('home', {title:'check auth'});
});

router.get('/profile', authenticationMiddleware(), function(req, res){
	//console.log(req.user);
	var callbackCount = 0;
	var context = {};
	context.jsscripts = ["updateuser.js","removemovie.js"];
	var mysql = req.app.get('mysql');
	getUser(res, mysql, context, req.user, complete);
	getFavorites(res, mysql, context, req.user, complete);
	getWatchlist(res, mysql, context, req.user, complete);
	function complete(){
		callbackCount++;
		if(callbackCount >= 3){
			res.render('profile', context);
		}
	}
		
});

router.put('/profile/:id', function(req, res){
	console.log("account.js - UPDATE USER");
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



    /* ----- REMOVE Specified Movie from Watchlist or Favorites ----- */


router.delete('/profile/removeWatch/:id', function(req, res){
	console.log("accounts.js - Remove From Watchlist");
    var mysql = req.app.get('mysql');
    var sql = "DELETE FROM watchlist WHERE U_id = ? AND mid = ?";
    var inserts = [req.user, req.params.id];
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

router.delete('/profile/removeFave/:id', function(req, res){
	console.log("accounts.js - Remove From Favorites");
    var mysql = req.app.get('mysql');
    var sql = "DELETE FROM favorites WHERE U_id = ? AND mid = ?";
    var inserts = [req.user, req.params.id];
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




router.get('/login', function(req, res){
	var context = {};
	context.jsscripts=["register.js"];
	context.title = "Login";
	res.render('login', context);
});

router.post('/login', passport.authenticate('local', {
	successRedirect: '/profile',
	failureRedirect: '/login'
}));

router.get('/logout', function(req, res){
	req.logout();
	res.status(200).clearCookie('connect.sid',{path: '/'});
	req.session.destroy(function (err){
		res.redirect('/login');
	});

});


router.get('/register', function(req, res, next) {
  res.render('register', { title: 'Registration' });
});

router.post('/register', function(req, res, next) {
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

	//req.check('username','This username is already taken').isUsernameAvailable();


	const errors = req.validationErrors();
	if(errors){
		console.log('errors: $JSON.stringify(errors)');
		res.render('register', {title: 'Registration Error',
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
					console.log("USERNAME/EMAIL Currently Registered");
					context.title="Registration Error";
					//console.log(JSON.stringify(context));
					res.render('register',context);
				}
				else{
					const username = req.body.username;
					const email = req.body.email;
					const password = req.body.password;

					//console.log(req.body.username);
					const db = require('./db.js');

					bcrypt.hash(password, saltRounds, function(err, hash) {
					db.pool.query('INSERT INTO accounts (uname, email, password,date_created) VALUES (?, ?, ?,now())',
						[username, email, hash], function(error, results, fields){
							if(error) throw error;
							else{
								var result = JSON.stringify(results);
								var json = JSON.parse(result);
								console.log("User id: "+ json.insertId);
								var id = json.insertId;
								req.login(id, function(err){
									res.redirect('/profile');
								})
							}
						})
					});
				}

			}

		}

	}
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
	    res.redirect('/login')
	}
}


module.exports = router;


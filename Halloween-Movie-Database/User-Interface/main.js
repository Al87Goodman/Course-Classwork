/*
 * Name: Alexander Goodman
 * Course: OSU CS 340
 * Assignment: Final Project 
 * Due Date: 04 December 2018
 *
 * REFERENCES:
  	1. OSU CS 340 Sample-Web-App 
  	2. https://www.jqueryscript.net/form/jQuery-Plugin-For-Filterable-Multiple-Select-with-Checkboxes-fSelect.html
		a. For my Checkbox Dropdown.
    3. Chris Courses: https://www.youtube.com/watch?v=gYjHDMPrkWU&list=PLpPnRKq7eNW3Qm2OfoJ3Hyvf-36TulLDp
 */


var express = require('express');
var path = require('path');
var cookieParser = require('cookie-parser');
var bodyParser = require('body-parser');
var expressValidator = require('express-validator');

// Authentication Packages
var session = require('express-session');
var passport = require('passport');
var LocalStrategy = require('passport-local').Strategy;
var MySQLStore = require('express-mysql-session')(session);
var bcrypt = require('bcrypt');

var app = express();

// Connect to server
require('dotenv').config();
var mysql = require('./db.js');


// Handlebar / Engine Setup
var handlebars = require('express-handlebars').create({defaultLayout:'main'});
app.engine('handlebars', handlebars.engine);
app.use(bodyParser.json());
app.use(bodyParser.urlencoded({extended:true}));
app.use(expressValidator()); //this line must be immediately after any of the bodyParser middlewares;
app.use(cookieParser());
app.use('/static', express.static('public'));
app.set('view engine', 'handlebars');
app.set('port', process.argv[2]);
app.set('mysql', mysql);


// Session Storage..
var options ={
  host: process.env.DB_HOST,
  user: process.env.DB_USER,
  password: process.env.DB_PASSWORD,
  database : process.env.DB_NAME
};

var sessionStore = new MySQLStore(options);


// Authentication..
app.use(session({
  secret: 'sunvlkdzpskwqj',    //make a random string key generator..
  resave: false,
  store: sessionStore,
  saveUninitialized: false //,
  //cookie: { secure: true } // for https
}))

app.use(passport.initialize());
app.use(passport.session());

// For dynamically changing navbar selections & Page Access
app.use(function(req, res, next){
  res.locals.isAuthenticated = req.isAuthenticated();
  next();
});

// Authentication Request.
passport.use(new LocalStrategy(
  function(username, password, done) {
    const db = require('./db');

    db.pool.query('SELECT u_id, password FROM accounts WHERE uname=?', [username], 
      function(err, results, fields){
        if(err){done(err)}; // done is provided by passport

        if(results.length == 0){
          done(null, false);
        }
        else{
          const hash = results[0].password.toString();
          bcrypt.compare(password, hash, function(err, response){
            if(response === true){
              var user_id = results[0].u_id;
              return done(null, user_id);
              //return done(null, {user_id: results[0].u_id});
            }
            else{
              return done(null, false);
            }
          });

        }

    })

  }
));



// ROUTES
app.get('/', function(req,res){
  res.render('home');
});

app.get('/about',function(req,res){
  res.render('about');
});

app.use('/devMovie', require('./routes/devMovie.js'));
app.use('/movie', require('./routes/movie.js'));
app.use('/infoMovie', require('./routes/infoMovie.js'));
app.use('/editCharc', require('./routes/editCharc.js'));
app.use('/editSubg', require('./routes/editSubg.js'));
app.use('/', require('./routes/accounts.js'));
app.use('/devUsers', require('./routes/devUsers.js'));



app.use(function(req,res){
  res.status(404);
  res.render('404');
});

app.use(function(err, req, res, next){
  console.error(err.stack);
  res.status(500);
  res.render('500');
});


app.listen(app.get('port'), function(){
  console.log('Express started on http://localhost:' + app.get('port') + '; press Ctrl-C to terminate.');
});

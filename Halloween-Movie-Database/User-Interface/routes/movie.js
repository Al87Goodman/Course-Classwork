module.exports = function(){
    var express = require('express');
    var router = express.Router();

function getCharctype(res, mysql, context, complete){
        mysql.pool.query("SELECT charctype.cid as id, name FROM charctype", function(error, results, fields){
            if(error){
                res.write(JSON.stringify(error));
                res.end();
            }
            context.charctype = results;
            complete();
        });
    };
    
    function getSubgenre(res, mysql, context, complete){
        mysql.pool.query("SELECT subgenre.sid as id, name FROM subgenre", function(error, results, fields){
            if(error){
                res.write(JSON.stringify(error));
                res.end();
            }
            context.subgenre = results;
            complete();
        });
    };


    function getOneMovie(res, mysql, context, id, complete){
        var sql = "SELECT mid as id, title, year, duration, link FROM movie WHERE mid=?";
        var inserts = [id];
        mysql.pool.query(sql, inserts, function (error, results, fields){
            if(error){
                res.write(JSON.stringify(error));
                res.end();
            }
            context.onemovie = results[0];
            complete();
        });
    };


    function getMovie(res, mysql, context, complete){
        var sql = "SELECT mid as id, title, year, duration, link FROM movie";
        mysql.pool.query(sql, function(error, results, fields){
            if(error){
                res.write(JSON.stringify(error));
                res.end();
            }
            else{
                /*
                for(i in results){
                  console.log(results[i].title);  
                }
                */  
                context.movie = results;
                complete();
            }
        });
    };
	
	
/* ----- FILTER / SEARCH SECTION ----- */
	
/* -- FILTER MOVIES BY UNKNOWN AMOUNT OF Subgenre & CHARC TYPES -- */
    function getMoviebySubg(res, mysql ,req, context, complete){
        //var sql = "SELECT DISTINCT m.mid as id, title, year, duration, c.name AS charctype, s.name AS subgenre FROM movie m JOIN mov_charc mc ON m.mid = mc.mid LEFT JOIN charctype c ON mc.cid = c.cid JOIN mov_subg ms ON m.mid = ms.mid LEFT JOIN subgenre s ON ms.sid = s.sid WHERE ";
        var sql = "SELECT DISTINCT m.mid as id, title, year, duration FROM movie m JOIN mov_charc mc ON m.mid = mc.mid LEFT JOIN charctype c ON mc.cid = c.cid JOIN mov_subg ms ON m.mid = ms.mid LEFT JOIN subgenre s ON ms.sid = s.sid WHERE ";
        var length = req.body.subgenre.length;
        var sql_1 = "s.sid=?";
        var sql_2 = " OR s.sid=?";
        for(var i=0; i<length; i++)
        {
            if(i===0){sql += sql_1;}
            else{sql += sql_2;}
            // may need another else to add ; to end of sql
        }
        
        var inserts = [];
        for(var i=0; i<length; i++){
            inserts[i] = req.body.subgenre[i];
        }
        //console.log("Inserts: "+inserts);
        mysql.pool.query(sql, inserts, function (error, results, fields){
            if(error){
                res.write(JSON.stringify(error));
                res.end();
            }
            context.movie = results;
            complete();
        });
        
    };

    function getMoviebyCharc(res, mysql ,req, context, complete){
        //var sql = "SELECT DISTINCT m.mid as id, title, year, duration, c.name AS charctype, s.name AS subgenre FROM movie m JOIN mov_charc mc ON m.mid = mc.mid LEFT JOIN charctype c ON mc.cid = c.cid JOIN mov_subg ms ON m.mid = ms.mid LEFT JOIN subgenre s ON ms.sid = s.sid WHERE ";
        var sql = "SELECT DISTINCT m.mid as id, title, year, duration FROM movie m JOIN mov_charc mc ON m.mid = mc.mid LEFT JOIN charctype c ON mc.cid = c.cid JOIN mov_subg ms ON m.mid = ms.mid LEFT JOIN subgenre s ON ms.sid = s.sid WHERE ";
        var length = req.body.charctype.length;
        var sql_1 = "c.cid=?";
        var sql_2 = " OR c.cid=?";
        for(var i=0; i<length; i++)
        {
            if(i===0){sql += sql_1;}
            else{sql += sql_2;}
        }
        
        var inserts = [];
        for(var i=0; i<length; i++){
            inserts[i] = req.body.charctype[i];
        }
        //console.log("Inserts: "+inserts);
        mysql.pool.query(sql, inserts, function (error, results, fields){
            if(error){
                res.write(JSON.stringify(error));
                res.end();
            }
            context.movie = results;
            complete();
        });
        
    };
    
    function getMovieByTitle(req, res, mysql, context, complete) {
       var query = "SELECT DISTINCT m.mid as id, title, year, duration FROM movie m JOIN mov_charc mc ON m.mid = mc.mid LEFT JOIN charctype c ON mc.cid = c.cid JOIN mov_subg ms ON m.mid = ms.mid LEFT JOIN subgenre s ON ms.sid = s.sid WHERE m.title LIKE " + mysql.pool.escape('%'+req.params.s + '%');

      mysql.pool.query(query, function(error, results, fields){
            if(error){
                res.write(JSON.stringify(error));
                res.end();
            }
            context.movie = results;
            complete();
        });
    };  
	



// NOTICE THESE FUNCTIONS HAVE BEEN MOVED TO infoMovies.js -- RUN FROM THERE NOW.. KEEPING ACTIVE FOR NOW..
function addWatchlist(res, mysql, req, id, complete){
    if(req.user){
        var sql = "INSERT INTO watchlist (u_id, mid) VALUES (?,?)";
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
        var sql = "INSERT INTO favorites (u_id, mid) VALUES (?,?)";
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





/* ______________________ ROUTES _______________________*/   
	
    /* Display ALL Movies */

    router.get('/', function(req, res){
		console.log("movie - Get All Movies");
        if(req.user){console.log("Logged in User id: "+req.user);};
        var callbackCount = 0;
        var context = {};
        context.jsscripts = ["filterBySubg2.js", "filterByCharc2.js","searchmovie2.js", "addtolist.js","mustlogin.js"];
        var mysql = req.app.get('mysql');
        getMovie(res, mysql, context, complete);
		getCharctype(res, mysql, context, complete);
        getSubgenre(res, mysql, context, complete);
        function complete(){
            callbackCount++;
            if(callbackCount >= 3){
                res.render('movie', context);
            }

        }
    });



    /* ----- ADD/PUT Routes ----- */

router.post('/addWatchlist/:id', function(req, res){
    console.log("acconts.js - Adding to Watchlist");
  if(req.user){
    console.log(req.body);
    var callbackCount = 0;
    var context = {};
    context.jsscripts = ["filterBySubg2.js", "filterByCharc2.js","searchmovie2.js", "addtolist.js"];
    var mysql = req.app.get('mysql');
    addWatchlist(res, mysql, req, req.params.id, complete);
    function complete(){
        callbackCount++;
        if(callbackCount >= 1){
            res.redirect('/');
        }
    }
 }
  else{
      res.redirect('/');
  }

}); 

router.post('/addFavorites/:id', function(req, res){
    console.log("acconts.js - Adding to Favorites");
  if(req.user){
    console.log(req.body);
    var callbackCount = 0;
    var context = {};
    context.jsscripts = ["filterBySubg2.js", "filterByCharc2.js","searchmovie2.js", "addtolist.js"];
    var mysql = req.app.get('mysql');
    addFavorites(res, mysql, req, req.params.id, complete);
    function complete(){
        callbackCount++;
        if(callbackCount >= 1){
            res.redirect('/');
        }
    }
 }
  else{
      res.redirect('/');
  }

}); 




   	 /* ----- Filter/Search Movie Section ----- */
	 
	 /* FILTER by subg & charctype*/
    router.post('/filterSubg', function(req, res){
		console.log("movie - FILTER by Subg");
		//console.log(req.body);
	  if(Object.keys(req.body).length > 0){
		var callbackCount = 0;
        var context = {};
        context.jsscripts = ["filterBySubg2.js", "filterByCharc2.js","searchmovie2.js", "addtolist.js"];
        var mysql = req.app.get('mysql');
        getMoviebySubg(res, mysql, req, context, complete);
        getCharctype(res, mysql, context, complete);
        getSubgenre(res, mysql, context, complete);
        function complete(){
            callbackCount++;
            if(callbackCount >= 3){
                res.render('movie', context);
            }

        }
	  }
	  else{
		  res.redirect('/movie');
	  }
    });		
	
    router.post('/filterCharc', function(req, res){
		console.log("movie - FILTER by Charc");
		//console.log(req.body);
	  if(Object.keys(req.body).length > 0){
		var callbackCount = 0;
        var context = {};
        context.jsscripts = ["filterBySubg2.js", "filterByCharc2.js","searchmovie2.js", "addtolist.js"];
        var mysql = req.app.get('mysql');
        getMoviebyCharc(res, mysql, req, context, complete);
        getCharctype(res, mysql, context, complete);
        getSubgenre(res, mysql, context, complete);
        function complete(){
            callbackCount++;
            if(callbackCount >= 3){
                res.render('movie', context);
            }
        }
	  }
	  else{
		  res.redirect('/movie');
	  }

    });	
	 
    router.get('/searchTitle/:s', function(req, res){
		console.log("movie - Search By Title");
        var callbackCount = 0;
        var context = {};
        context.jsscripts = ["filterBySubg2.js", "filterByCharc2.js","searchmovie2.js", "addtolist.js"];
        var mysql = req.app.get('mysql');
        getMovieByTitle(req, res, mysql, context, complete);
        getCharctype(res, mysql, context, complete);
        getSubgenre(res, mysql, context, complete);
        function complete(){
            callbackCount++;
            if(callbackCount >= 3){
                res.render('movie', context);
            }
        }
    });	
	
	
    return router;
}();
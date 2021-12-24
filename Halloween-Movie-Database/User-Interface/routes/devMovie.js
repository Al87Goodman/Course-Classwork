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

    /* -- GET ALL charctypes & subgenres associated with a specific movie -- */
    function getMovCharc(res, mysql, context, id, complete){
        var sql = "SELECT m.mid as id, title, c.cid, c.name FROM movie m JOIN mov_charc mc ON m.mid = mc.mid LEFT JOIN charctype c ON mc.cid = c.cid WHERE mc.mid=(SELECT mid FROM movie WHERE mid=?);";
        var inserts = [id];
        mysql.pool.query(sql, inserts, function (error, results, fields){
            if(error){
                res.write(JSON.stringify(error));
                res.end();
            }
            context.charc = results;
            complete();
        });
    };

    function getMovSubg(res, mysql, context, id, complete){
        var sql = "SELECT m.mid as id, title, s.sid, s.name FROM movie m JOIN mov_subg ms ON m.mid = ms.mid LEFT JOIN subgenre s ON ms.sid = s.sid WHERE ms.mid=(SELECT mid FROM movie WHERE mid=?);";
        var inserts = [id];
        mysql.pool.query(sql, inserts, function (error, results, fields){
            if(error){
                res.write(JSON.stringify(error));
                res.end();
            }
            context.subg = results;
            complete();
        });
    };

    /* -- GET ALL the remaining charctypes & subgenres not associated with a specific movie -- */
    function getRemainSubg(res, mysql, context, id, complete){
        var sql = "SELECT s.sid, s.name FROM subgenre s WHERE s.sid NOT IN (SELECT ms.sid FROM mov_subg ms WHERE ms.mid=?);";
        var inserts = [id];
        mysql.pool.query(sql, inserts, function (error, results, fields){
            if(error){
                res.write(JSON.stringify(error));
                res.end();
            }
            context.remainSubg = results;
            complete();
        });

    };

    function getRemainCharc(res, mysql, context, id, complete){
        var sql = "SELECT c.cid, c.name FROM charctype c WHERE c.cid NOT IN (SELECT mc.cid FROM mov_charc mc WHERE mc.mid=?);";
        var inserts = [id];
        mysql.pool.query(sql, inserts, function (error, results, fields){
            if(error){
                res.write(JSON.stringify(error));
                res.end();
            }
            context.remainCharc = results;
            complete();
        });

    };

    // get all movies
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

	
	/* Functions for Adding a Movie */	
    function addMovie(res, mysql, body, id, complete){
        var sql = "INSERT INTO movie (title, year, duration, link) VALUES (?,?,?,?)";
        var inserts = [body.title, body.year, body.duration, body.link];

        sql = mysql.pool.query(sql,inserts,function(error, results, fields){
            if(error){
                console.log(JSON.stringify(error))
                res.write(JSON.stringify(error));
                res.end();
			}
			// Gets the newly added movie id (mid) -- maybe start thinking about using SELECT LAST_INSERT_ID()
			var result = (JSON.stringify(results));
			var json = JSON.parse(result);
			//console.log(json.insertId);
			id = json.insertId;

            complete(id);
        });
	};
	
    function addMovSubg(res, mysql, body, id, complete2){
	
		if(body.subgenre){
		for(var i=0; i<=body.subgenre.length-1; i++){
			var sql = "INSERT INTO mov_subg (mid, sid) VALUES (?,?)";
			var inserts = [id, body.subgenre[i]];
			sql = mysql.pool.query(sql,inserts,function(error, results, fields){
				if(error){
					console.log(JSON.stringify(error))
					res.write(JSON.stringify(error));
					res.end();
				}
                else{
				
                }
			});
		} complete2();
		}
		else{var sql = "INSERT INTO mov_subg (mid, sid) VALUES (?,?)";
        var inserts = [id, body.subgenre];
        sql = mysql.pool.query(sql,inserts,function(error, results, fields){
            if(error){
                console.log(JSON.stringify(error))
                res.write(JSON.stringify(error));
                res.end();
			}
			complete2();
        });
		} 
	};

    function addMovCharc(res, mysql, body, id, complete2){

		if(body.charctype){
		for(var i=0; i<=body.charctype.length-1; i++){
			var sql = "INSERT INTO mov_charc (mid, cid) VALUES (?,?)";
			var inserts = [id, body.charctype[i]];
			sql = mysql.pool.query(sql,inserts,function(error, results, fields){
				if(error){
					console.log(JSON.stringify(error))
					res.write(JSON.stringify(error));
					res.end();
				}
				
			});
		} complete2(); 
		}
		else{var sql = "INSERT INTO mov_charc (mid, cid) VALUES (?,?)";
        var inserts = [id, body.charctype];
        sql = mysql.pool.query(sql,inserts,function(error, results, fields){
            if(error){
                console.log(JSON.stringify(error))
                res.write(JSON.stringify(error));
                res.end();
			}
			complete2();
        });
		} 
	};	
	
	
	/* -- Functions for Adding a Subgenre and Character Type -- */
    function addSubg(res, mysql, body, complete){
        var sql = "INSERT INTO subgenre (name) VALUES (?)";
        var inserts = [body.addSubg];
        sql = mysql.pool.query(sql,inserts,function(error, results, fields){
            if(error){
                console.log(JSON.stringify(error))
                res.write(JSON.stringify(error));
                res.end();
			}
            complete();
        });
	};	
	
	function addCharc(res, mysql, body, complete){
        var sql = "INSERT INTO charctype (name) VALUES (?)";
        var inserts = [body.addCharc];
        sql = mysql.pool.query(sql,inserts,function(error, results, fields){
            if(error){
                console.log(JSON.stringify(error))
                res.write(JSON.stringify(error));
                res.end();
			}
            complete();
        });
	};	
	
		/* -- Functions for Updating a Movie -- */	
    function updateMovie(res, mysql, req, complete2){
		var sql = "UPDATE movie SET title=?, year=?, duration=?, link=? WHERE mid=?";
        var inserts = [req.body.title, req.body.year, req.body.duration, req.body.link, req.params.id];
        sql = mysql.pool.query(sql,inserts,function(error, results, fields){
            if(error){
                console.log(JSON.stringify(error))
                res.write(JSON.stringify(error));
                res.end();
			}
			complete2();
            
        });
	};
	
    function updateMovSubg(res, mysql, req, complete2){
		var sql = "UPDATE mov_subg SET sid=? WHERE mid=?";
		var inserts = [req.body.subgenre, req.params.id];
		console.log(req.body.subgenre);
        sql = mysql.pool.query(sql,inserts,function(error, results, fields){
            if(error){
                console.log(JSON.stringify(error))
                res.write(JSON.stringify(error));
                res.end();
			}
			complete2();
        });
	};
	
	function updateMovCharc(res, mysql, req, complete2){
		var sql = "UPDATE mov_charc SET cid=? WHERE mid=?";
		var inserts = [req.body.charctype, req.params.id];
        sql = mysql.pool.query(sql,inserts,function(error, results, fields){
            if(error){
                console.log(JSON.stringify(error))
                res.write(JSON.stringify(error));
                res.end();
			}
			complete2();
        });
	};
	


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
	
	
    /* ----- REMOVE from Lists ----- */

    function removeSubg(res, mysql, req, id, complete){
        if(req.body.subgenre){
        for(var i=0; i<=req.body.subgenre.length-1; i++){
            var sql = "DELETE FROM mov_subg WHERE mid = ? AND sid = ?";
            var inserts = [id, req.body.subgenre[i]];
            sql = mysql.pool.query(sql,inserts,function(error, results, fields){
                if(error){
                    console.log(JSON.stringify(error))
                    res.write(JSON.stringify(error));
                    res.end();
                }
                
            });
        } complete();
        }
        else{
            var sql = "DELETE FROM mov_subg WHERE mid = ? AND sid = ?";
            var inserts = [id, req.body.subgenre];
            sql = mysql.pool.query(sql,inserts,function(error, results, fields){
            if(error){
                console.log(JSON.stringify(error))
                res.write(JSON.stringify(error));
                res.end();
            }
            complete();
        });
        } 
    };

    function removeCharc(res, mysql, req, id, complete){
        if(req.body.charctype){
        for(var i=0; i<=req.body.charctype.length-1; i++){
            var sql = "DELETE FROM mov_charc WHERE mid = ? AND cid = ?";
            var inserts = [id, req.body.charctype[i]];
            sql = mysql.pool.query(sql,inserts,function(error, results, fields){
                if(error){
                    console.log(JSON.stringify(error))
                    res.write(JSON.stringify(error));
                    res.end();
                }
                
            });
        } complete();
        }
        else{
            var sql = "DELETE FROM mov_charc WHERE mid = ? AND cid = ?";
            var inserts = [id, req.body.charctype];
            sql = mysql.pool.query(sql,inserts,function(error, results, fields){
            if(error){
                console.log(JSON.stringify(error))
                res.write(JSON.stringify(error));
                res.end();
            }
            complete();
        });
        } 
    };


/* _________________________________ROUTES BELOW _________________________________ */	
	
    /* Display ALL Movies */
    router.get('/', function(req, res){
		console.log("GET ALL MOVIE");
        var callbackCount = 0;
        var context = {};
        context.jsscripts = ["deletemovie.js","filterByCharc.js","searchmovie.js"];
        var mysql = req.app.get('mysql');
        getMovie(res, mysql, context, complete);
		getCharctype(res, mysql, context, complete);
        getSubgenre(res, mysql, context, complete);
        function complete(){
            callbackCount++;
            if(callbackCount >= 3){
                res.render('devMovie', context);
            }
        }
    });



	/* ----- UPDATE MOVIE SECTION ----- */
	
    /* Display one Movie for the specific purpose of updating that movie */
    router.get('/:id', function(req, res){
		console.log("IN GET ONE-MOVIE");
        var callbackCount = 0;
        var context = {};
        context.jsscripts = ["selectedsubgenre.js", "selectedcharctype.js", "updatemovie.js", "filterByCharc.js", "removesubg.js", "removecharc.js", "addsubg.js", "addcharc.js"];
        var mysql = req.app.get('mysql');
        getOneMovie(res, mysql, context, req.params.id, complete);
        getMovCharc(res, mysql, context, req.params.id, complete);
        getMovSubg(res, mysql, context, req.params.id, complete);
        getSubgenre(res, mysql, context, complete);
		getCharctype(res, mysql, context, complete);
        getRemainSubg(res, mysql, context, req.params.id, complete);
        getRemainCharc(res, mysql, context, req.params.id, complete);
        function complete(){
            callbackCount++;
            if(callbackCount >= 7){
				//console.log(context);
                res.render('movieEdit', context);
            }

        }
    });

	/* UPDATE Movie Information */
     router.put('/:id', function(req, res){
		console.log("IN UPDATE"); 
        var callbackCount = 0;
        var mysql = req.app.get('mysql');
        console.log("UPDATE: "+JSON.stringify(req.body));
        updateMovie(res, mysql, req, complete2);
        //updateMovSubg(res, mysql, req, complete2);
		//updateMovCharc(res, mysql, req, complete2);
        function complete2(){
            callbackCount++;
            if(callbackCount >= 1){
				res.status(200).end();
			}
        }
		//res.status(200).end();
	});

/* UPDATE by ADDING mov_subg for a particular movie */
     router.put('/addSubg/:id', function(req, res){
        console.log("devMovie.js - Add mov_subg"); 
        var callbackCount = 0;
        var body = req.body;
        var mysql = req.app.get('mysql');
        addMovSubg(res, mysql, body, req.params.id, complete2);
        function complete2(){
            callbackCount++;
            if(callbackCount >= 1){
                res.status(200).end();
            }
        }
    });

/* UPDATE by ADDING mov_subg for a particular movie */
     router.put('/addCharc/:id', function(req, res){
        console.log("devMovie.js - Add mov_charc"); 
        var callbackCount = 0;
        var body = req.body;
        var mysql = req.app.get('mysql');
        addMovCharc(res, mysql, body, req.params.id, complete2);
        function complete2(){
            callbackCount++;
            if(callbackCount >= 1){
                res.status(200).end();
            }
        }
    });
	
	/* ----- ADD SECTION ----- */
	
    /* ADD a Movie to the database */
    router.post('/', function(req, res){
		console.log("IN ADD Movie");
        var callbackCount = 0;
		var callbackCount2 = 0;
		var id = 0;
        var mysql = req.app.get('mysql');
		var body = req.body;
        addMovie(res, mysql, body, id, complete);
		function complete(id){
			callbackCount++;
			if(callbackCount>=1){
				addMovSubg(res, mysql, body, id, complete2);
				addMovCharc(res, mysql, body, id, complete2);
				function complete2(){
					callbackCount2++;
					if(callbackCount2 >= 2){
						res.redirect('/devMovie');
					}
				}
			}
		}
    });

	/* ADD a Subgenre to the database */
	router.post('/addSubg', function(req, res){
		console.log("IN ADD Subg");
        var callbackCount = 0;
        var mysql = req.app.get('mysql');
		var body = req.body;
        addSubg(res, mysql, body, complete);
        function complete(){
            callbackCount++;
            if(callbackCount >= 1){
                res.redirect('/devMovie');
            }
        }
    });
	
	/* ADD a Character Type to the database */
	router.post('/addCharc', function(req, res){
		console.log("IN ADD Charc");
        var callbackCount = 0;
        var mysql = req.app.get('mysql');
		var body = req.body;
        addCharc(res, mysql, body, complete);
        function complete(){
            callbackCount++;
            if(callbackCount >= 1){
                res.redirect('/devMovie');
            }

        }
    });

   	 /* ----- Filter/Search Movie Section ----- */
	 
	 /* FILTER by subg & Charctype */
    router.post('/filterSubg', function(req, res){
		console.log("devMovie - FILTER by Subg");
		//console.log(req.body);
	  if(Object.keys(req.body).length > 0){
		var callbackCount = 0;
        var context = {};
        context.jsscripts = ["deletemovie.js","filterBySubg.js","searchmovie.js"];
        var mysql = req.app.get('mysql');
        getMoviebySubg(res, mysql, req, context, complete);
        getCharctype(res, mysql, context, complete);
        getSubgenre(res, mysql, context, complete);
        function complete(){
            callbackCount++;
            if(callbackCount >= 3){
                res.render('devMovie', context);
            }

        }
	  }
	  else{
		  res.redirect('/devMovie');
	  }
    });			
	
    router.post('/filterCharc', function(req, res){
		console.log("devMovie - FILTER by Charc");
		//console.log(req.body);
	  if(Object.keys(req.body).length > 0){
		var callbackCount = 0;
        var context = {};
        context.jsscripts = ["deletemovie.js","filterByCharc.js","searchmovie.js"];
        var mysql = req.app.get('mysql');
        getMoviebyCharc(res, mysql, req, context, complete);
        getCharctype(res, mysql, context, complete);
        getSubgenre(res, mysql, context, complete);
        function complete(){
            callbackCount++;
            if(callbackCount >= 3){
                res.render('devMovie', context);
            }
        }
	  }
	  else{
		  res.redirect('/devMovie');
	  }

    });	
	 
    router.get('/searchTitle/:s', function(req, res){
		console.log("Search By Title");
        var callbackCount = 0;
        var context = {};
        context.jsscripts = ["deletemovie.js","searchmovie.js"];
        var mysql = req.app.get('mysql');
        getMovieByTitle(req, res, mysql, context, complete);
        getCharctype(res, mysql, context, complete);
        getSubgenre(res, mysql, context, complete);
        function complete(){
            callbackCount++;
            if(callbackCount >= 3){
                res.render('devMovie', context);
            }
        }
    });
	 

    /* --- DELETE a movie (handled by ajax) --- */

    router.delete('/:id', function(req, res){
        var mysql = req.app.get('mysql');
        var sql = "DELETE FROM movie WHERE mid = ?";
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


    /* ----- REMOVE Subgenre or Charctype from specified Movie ----- */
    router.put('/removeSubg/:id', function(req, res){
        console.log("devMovie.js - REMOVE SUBG: "+ JSON.stringify(req.body));
        var mysql = req.app.get('mysql');
        var callbackCount = 0;
        removeSubg(res, mysql, req, req.params.id, complete);
        function complete(){
            callbackCount++;
            if(callbackCount >= 1){
                res.status(200).end();
            }
        }
    });

    router.put('/removeCharc/:id', function(req, res){
        console.log("devMovie.js - REMOVE CHARC: "+ JSON.stringify(req.body));
        var mysql = req.app.get('mysql');
        var callbackCount = 0;
        removeCharc(res, mysql, req, req.params.id, complete);
        function complete(){
            callbackCount++;
            if(callbackCount >= 1){
                res.status(200).end();
            }
        }
    });


    return router;
}();

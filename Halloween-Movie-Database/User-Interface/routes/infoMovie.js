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


    function getMovieInfo(res, mysql, context, id, complete){
        var sql = "SELECT mid as id, title, year, duration, link FROM movie WHERE mid=?";
        var inserts = [id];
        mysql.pool.query(sql, inserts, function (error, results, fields){
            if(error){
                res.write(JSON.stringify(error));
                res.end();
            }
            context.movie = results[0];
            complete();
        });
    };


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

	

function addWatchlist(res, mysql, req, id, complete){
    if(req.user){
        var sql = "INSERT INTO watchlist (u_id, mid,date_added) VALUES (?,?,now())";
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
        var sql = "INSERT INTO favorites (u_id, mid,date_added) VALUES (?,?,now())";
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


function statusWatch(res, mysql, context, req, mid, complete){
    if(req.user){
        var sql = "SELECT mid FROM watchlist WHERE u_id=? AND mid=?"
        var inserts=[req.user, mid];
        mysql.pool.query(sql, inserts, function(error, results, fields){
            if(results.length>0){
                console.log("In WATCHLIST");
                context.statusWatch = "inWatchlist";
                complete();
            }
            else{
                console.log("NOT IN WATCHLIST");
                context.statusWatch = "notinWatchlist";
                complete();
            }

        });
    }
    else{
        // Lets Html/HandleBars page know that user cannot add to watchlist since there is no user.
        context.statusWatch = "noAccount";
        complete();
    }
};



function statusFave(res, mysql, context, req, mid, complete){
    if(req.user){
        var sql = "SELECT mid FROM favorites WHERE u_id=? AND mid=?"
        var inserts=[req.user, mid];
        mysql.pool.query(sql, inserts, function(error, results, fields){
            if(results.length>0){
                console.log("In Favorites");
                context.statusFave = "inFavorites";
                complete();
            }
            else{
                console.log("NOT IN Favorites");
                context.statusFave = "notinFavorites";
                complete();
            }

        });
    }
    else{
        // Lets Html/HandleBars page know that user cannot add to favorites since there is no user.
        context.statusFave = "noAccount";
        complete();
    }
};





/* _______________________ ROUTES _______________________*/   
	
    /* Display Movie Info */

    router.get('/:id', function(req, res){
        console.log("infoMovie.js - Get Movie Info");
        var callbackCount = 0;
        var context = {};
        context.jsscripts = ["filterByCharc.js", "removesubg.js", "removecharc.js", "addsubg.js", "addcharc.js", "addtolist.js", "infomovie_helper.js"];
        var mysql = req.app.get('mysql');
        getMovieInfo(res, mysql, context, req.params.id, complete);
        getMovCharc(res, mysql, context, req.params.id, complete);
        getMovSubg(res, mysql, context, req.params.id, complete);
        statusWatch(res, mysql, context, req, req.params.id, complete);
        statusFave(res, mysql, context, req, req.params.id, complete);
        function complete(){
            callbackCount++;
            if(callbackCount >= 5){
                //console.log(context);
                res.render('infoMovie', context);
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
    context.jsscripts = ["filterBySubg2.js", "filterByCharc2.js","searchmovie2.js", "addtolist.js", "infomovie_helper.js"];
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
    context.jsscripts = ["filterBySubg2.js", "filterByCharc2.js","searchmovie2.js", "addtolist.js", "infomovie_helper.js"];
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


router.delete('/removeWatch/:id', function(req, res){
    console.log("infoMovie.js - Remove From Watchlist");
    if(req.user){
        var mysql = req.app.get('mysql');
        var sql = "DELETE FROM watchlist WHERE U_id = ? AND mid = ?";
        var inserts = [req.user, req.params.id];
        sql = mysql.pool.query(sql, inserts, function(error, results, fields){
            if(error){
                res.write(JSON.stringify(error));
                res.status(400);
                res.end();
            }else{
                console.log("NO USER SIGNED IN");
                res.status(202).end();
            }
        })
    }
    else{
        res.status(202).end();   
    }

});

router.delete('/removeFave/:id', function(req, res){
    console.log("infoMovie.js - Remove From Favorites");
    if(req.user){
        console.log("USER SIGNED IN")
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
    }
    else{
        console.log("NO USER SIGNED IN");
        res.status(202).end();
    }

});






	
	
    return router;
}();
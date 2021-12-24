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

	function getOneCharc(res, mysql, context, id, complete){
        var sql = "SELECT c.cid as id, name FROM charctype c WHERE c.cid = ?";
		var inserts = [id];
		mysql.pool.query(sql, inserts, function (error, results, fields){
            if(error){
                res.write(JSON.stringify(error));
                res.end();
            }
	    context.editCharc = results[0];
            complete();
        });
    };
	

/* ______________________ ROUTES _______________________*/    

/* ------ UPDATE/EDIT Charctype SECTION ----- */

    router.get('/', function(req, res){
		console.log("IN EDIT CHARC");
        var callbackCount = 0;
        var context = {};
        context.jsscripts = ["deletecharc.js", "selectedcharctype.js"];
        var mysql = req.app.get('mysql');
		getCharctype(res, mysql, context, complete);
        function complete(){
            callbackCount++;
            if(callbackCount >= 1){
				//console.log(context);
                res.render('editCharc', context);
            }

        }
    });
	
	 /* Display one Charctype for the specific purpose of updating */
    router.get('/:id', function(req, res){
		console.log("IN GET ONE-Charctype");
        var callbackCount = 0;
        var context = {};
        context.jsscripts = ["selectedcharctype.js", "updatecharc.js"];
        var mysql = req.app.get('mysql');
        getOneCharc(res, mysql, context, req.params.id, complete);
        function complete(){
            callbackCount++;
            if(callbackCount >= 1){
				//console.log(context);
                res.render('editCharc', context);
            }

        }
    });
	
	/* UPDATE Charctype Entry */
    router.put('/:id', function(req, res){
		console.log("IN UPDATE Charctype");
        var mysql = req.app.get('mysql');
        //console.log(req.body)
        //console.log(req.params.id)
        var sql = "UPDATE charctype SET name=? WHERE cid=?";
        var inserts = [req.body.cname, req.params.id];
        sql = mysql.pool.query(sql,inserts,function(error, results, fields){
            if(error){
                console.log(error)
                res.write(JSON.stringify(error));
                res.end();
            }else{
                res.status(200);
                res.end();
            }
        });
    });	
	
	
    /* DELETE a charctype (handled by ajax when a 202 message is received) */
	router.delete('/:id', function(req, res){
		console.log("IN CHARC DELETE");
        var mysql = req.app.get('mysql');
        var sql = "DELETE FROM charctype WHERE cid = ?";
        var inserts = [req.params.id];
        sql = mysql.pool.query(sql, inserts, function(error, results, fields){
            if(error){
				console.log("Cannot Delete");
                res.write(JSON.stringify(error));
                res.status(400);
                res.end();
            }else{
                res.status(202).end();
            }
        })
    })
	
	
	return router;
}();

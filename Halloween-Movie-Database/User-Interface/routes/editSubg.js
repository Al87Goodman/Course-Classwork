module.exports = function(){
    var express = require('express');
    var router = express.Router();


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

	function getOneSubg(res, mysql, context, id, complete){
        var sql = "SELECT s.sid as id, name FROM subgenre s WHERE s.sid = ?";
		var inserts = [id];
		mysql.pool.query(sql, inserts, function (error, results, fields){
            if(error){
                res.write(JSON.stringify(error));
                res.end();
            }
	    context.editSubg = results[0];
            complete();
        });
    };
	
/* ______________________ ROUTES _______________________*/   
    
/* ------ UPDATE/EDIT Subgenre SECTION ----- */

    router.get('/', function(req, res){
		console.log("IN EDIT Subg");
        var callbackCount = 0;
        var context = {};
        context.jsscripts = ["deletesubg.js", "selectedsubgenre.js"];
        var mysql = req.app.get('mysql');
		getSubgenre(res, mysql, context, complete);
        function complete(){
            callbackCount++;
            if(callbackCount >= 1){
				//console.log(context);
                res.render('editSubg', context);
            }

        }
    });
	
	 /* Display one Subgenre for the specific purpose of updating */
    router.get('/:id', function(req, res){
		console.log("IN GET ONE-Subgenre");
        var callbackCount = 0;
        var context = {};
        context.jsscripts = ["selectedsubgenre.js", "updatesubg.js"];
        var mysql = req.app.get('mysql');
        getOneSubg(res, mysql, context, req.params.id, complete);
        function complete(){
            callbackCount++;
            if(callbackCount >= 1){
				//console.log(context);
                res.render('editSubg', context);
            }

        }
    });
	
	/* UPDATE Subgenre Entry */
    router.put('/:id', function(req, res){
		console.log("IN UPDATE Subgenre");
        var mysql = req.app.get('mysql');
        //console.log(req.body)
        //console.log(req.params.id)
        var sql = "UPDATE subgenre SET name=? WHERE sid=?";
        var inserts = [req.body.sname, req.params.id];
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
	
	
    /* DELETE a subgenre (handled by ajax when a 202 message is received) */
	router.delete('/:id', function(req, res){
		console.log("IN DELETE Subg");
        var mysql = req.app.get('mysql');
        var sql = "DELETE FROM subgenre WHERE sid = ?";
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

-- Course: OSU 340 Intro to Databses
-- File: Database Operations / Manipulations
-- Name: Alexander Goodman
-- Date Due: 04 December 2018
-- Last Modification: 22 November 2018

-- By Routes:

/*  --- acounts --- */

-- SELECT a User's Information 
SELECT u_id, fname, lname, uname, email, date_created FROM accounts WHERE u_id = ?;

-- SELECT and return all of a User's Watchlist
SELECT w.u_id, m.mid, m.title, w.date_added FROM watchlist w JOIN movie m ON w.mid=m.mid WHERE w.u_id = ?;

-- SELECT and return all of a User's Favorites
SELECT f.u_id, m.mid, m.title, f.date_added, f.note FROM favorites f JOIN movie m ON f.mid=m.mid WHERE f.u_id = ?;

--INSERT user into Accounts
INSERT INTO accounts (uname, email, password,date_created) VALUES (?, ?, ?,now());

-- Add a Movie to a User's Watchlist
INSERT INTO watchlist (u_id, mid, date_added) VALUES (?,?,now());

-- Add a Movie to a User's Favorites
INSERT INTO favorites (u_id, mid, date_added) VALUES (?,?,now());

-- Update a User's Account Information in Accounts
UPDATE accounts SET fname=?, lname=?, uname=?, email=? WHERE u_id=?;

-- REMOVE from Specific User's Watchlist
DELETE FROM favorites WHERE U_id = ? AND mid = ?;

-- REMOVE from Spcific User's Favorites
DELETE FROM watchlist WHERE U_id = ? AND mid = ?;

-- Verify/Check if Username is Available (aready in use or not)
SELECT uname FROM accounts WHERE uname=?;

-- Verify/Check if Email is Avaialbe (already in use or not)
SELECT email FROM accounts WHERE email=?;


/* --- devUsers --- */

-- SELECT ALL User's from Accounts
SELECT u_id as id, uname, fname, lname, email, date_created FROM accounts;

-- SELECT a specific User from Accounts
SELECT u_id as id, fname, lname, uname, email, date_created FROM accounts WHERE u_id = ?;

-- SELECT a specific User's Watchlist
SELECT w.u_id, m.mid, m.title, w.date_added FROM watchlist w JOIN movie m ON w.mid=m.mid WHERE w.u_id = ?;

-- SELECT a specific User's Favorites
SELECT f.u_id, m.mid, m.title, f.date_added, f.note FROM favorites f JOIN movie m ON f.mid=m.mid WHERE f.u_id = ?;

-- FILTER/SEARCH a User by username
SELECT u_id as id, uname, fname, lname, email, date_created FROM accounts a WHERE a.uname LIKE + mysql.pool.escape('%'+req.params.s + '%');

-- INSERT/ADD a User into Accounts
INSERT INTO accounts (uname, fname, lname, email, password, date_created) VALUES (?, ?, ?, ?, ?,now());

-- UPDATE a specific User's Information
UPDATE accounts SET fname=?, lname=?, uname=?, email=? WHERE u_id=?;

-- DELETE a User from Accounts
DELETE FROM accounts WHERE U_id = ?;

-- REMOVE a movie from a User's Watchlist
DELETE FROM watchlist WHERE U_id = ? AND mid = ?;

-- REMOVE a movie from a User's Favorites
DELETE FROM favorites WHERE U_id = ? AND mid = ?;

-- Verify/Check if Username is Available (aready in use or not)
SELECT uname FROM accounts WHERE uname=?;

-- Verify/Check if Email is Avaialbe (already in use or not)
SELECT email FROM accounts WHERE email=?;


/* --- devMovie --- */

-- SELECT all character types from charctype
SELECT charctype.cid as id, name FROM charctype;

-- SELECT all subgenre from subgenre
SELECT subgenre.sid as id, name FROM subgenre;

-- SELECT all movies from movie
SELECT mid as id, title, year, duration, link FROM movie;

-- SELECT a specific movie from movie
SELECT mid as id, title, year, duration, link FROM movie WHERE mid=?;

-- SELECT all charctypes associated with a specific movie
SELECT m.mid as id, title, c.cid, c.name FROM movie m JOIN mov_charc mc ON m.mid = mc.mid LEFT JOIN charctype c ON mc.cid = c.cid WHERE mc.mid=(SELECT mid FROM movie WHERE mid=?);

-- SELECT all subgenres associated with a specific movie
SELECT m.mid as id, title, s.sid, s.name FROM movie m JOIN mov_subg ms ON m.mid = ms.mid LEFT JOIN subgenre s ON ms.sid = s.sid WHERE ms.mid=(SELECT mid FROM movie WHERE mid=?)

--SELECT all remaining charctypes Not currently associated with a specific movie
SELECT c.cid, c.name FROM charctype c WHERE c.cid NOT IN (SELECT mc.cid FROM mov_charc mc WHERE mc.mid=?);

-- SELECT all remeaining subgenres Not currently associated with a specific movie
SELECT s.sid, s.name FROM subgenre s WHERE s.sid NOT IN (SELECT ms.sid FROM mov_subg ms WHERE ms.mid=?);

-- INSERT/ADD a movie to movie
INSERT INTO movie (title, year, duration, link) VALUES (?,?,?,?);

-- INSERT/ADD a movie-charcter relationship into mov_charc
INSERT INTO mov_charc (mid, cid) VALUES (?,?);

-- INSERT/ADD a movie-subgenre relationship into mov_subg
INSERT INTO mov_subg (mid, sid) VALUES (?,?);

-- INSERT/ADD a sugenre into subgenre
INSERT INTO subgenre (name) VALUES (?);

-- INSERT/ADD a charctype into charctype
INSERT INTO charctype (name) VALUES (?)

-- UPDATE movie information for a specific movie
UPDATE movie SET title=?, year=?, duration=?, link=? WHERE mid=?;

-- UPDATE movie-subgenre relationship in mov_subg
UPDATE mov_subg SET sid=? WHERE mid=?;

-- UPDATE movie-charctype relationship in mov_charc
UPDATE mov_charc SET cid=? WHERE mid=?;

-- FILTER movies by unknown amount of subgenres - Function that loops through adding s.sid=? to the end of the statement depending on how many subgenres were selected
SELECT DISTINCT m.mid as id, title, year, duration FROM movie m JOIN mov_charc mc ON m.mid = mc.mid LEFT JOIN charctype c ON mc.cid = c.cid JOIN mov_subg ms ON m.mid = ms.mid LEFT JOIN subgenre s ON ms.sid = s.sid WHERE ..;

-- FILTER movies by uknown amount of charctypes - Function that loops through adding c.cid=? to the end of the statement depending on how many charctypes were selected
SELECT DISTINCT m.mid as id, title, year, duration FROM movie m JOIN mov_charc mc ON m.mid = mc.mid LEFT JOIN charctype c ON mc.cid = c.cid JOIN mov_subg ms ON m.mid = ms.mid LEFT JOIN subgenre s ON ms.sid = s.sid WHERE ..;

-- SEARCH movie by title
SELECT DISTINCT m.mid as id, title, year, duration FROM movie m JOIN mov_charc mc ON m.mid = mc.mid LEFT JOIN charctype c ON mc.cid = c.cid JOIN mov_subg ms ON m.mid = ms.mid LEFT JOIN subgenre s ON ms.sid = s.sid WHERE m.title LIKE + mysql.pool.escape('%'+req.params.s + '%');

-- DELETE a movie from Movie
DELETE FROM movie WHERE mid = ?;

-- REMOVE a subgenre from a movie (mov_subg relationship table)
DELETE FROM mov_subg WHERE mid = ? AND sid = ?;

-- Remove a charctype from a movie (mov_charc relationship table)
DELETE FROM mov_charc WHERE mid = ? AND cid = ?;


/* --- editCharc --- */

-- SELECT all charctypes from charctype
SELECT charctype.cid as id, name FROM charctype;

-- SELECT a specific charctype from charctype
SELECT c.cid as id, name FROM charctype c WHERE c.cid = ?

-- UPDATE a specific charctype name from charctype
UPDATE charctype SET name=? WHERE cid=?;

-- DELETE a specific charctype from charctype
DELETE FROM charctype WHERE cid = ?;



/* --- editSubg --- */

-- SELECT all subgenre from subgenre
SELECT subgenre.sid as id, name FROM subgenre;

-- SELECT a specific subgenre from subgenre
SELECT s.sid as id, name FROM subgenre s WHERE s.sid = ?;

-- UPDATE a specific subgenre name from subgenre
UPDATE subgenre SET name=? WHERE sid=?;

-- DELETE a specific subgenre from subgenre
DELETE FROM subgenre WHERE sid = ?;



/* --- movie --- */

-- SELECT all charctypes from charctype
SELECT charctype.cid as id, name FROM charctype;

-- SELECT all subgenres from subgenre
SELECT subgenre.sid as id, name FROM subgenre;

-- SELECT all movies from movie
SELECT mid as id, title, year, duration, link FROM movie WHERE mid=?;

-- SELECT a specific movie from movie
SELECT mid as id, title, year, duration, link FROM movie;

-- FILTER movies by unknown amount of subgenres - Function that loops through adding s.sid=? to the end of the statement depending on how many subgenres were selected
SELECT DISTINCT m.mid as id, title, year, duration FROM movie m JOIN mov_charc mc ON m.mid = mc.mid LEFT JOIN charctype c ON mc.cid = c.cid JOIN mov_subg ms ON m.mid = ms.mid LEFT JOIN subgenre s ON ms.sid = s.sid WHERE ..;

-- FILTER movies by uknown amount of charctypes - Function that loops through adding c.cid=? to the end of the statement depending on how many charctypes were selected
SELECT DISTINCT m.mid as id, title, year, duration FROM movie m JOIN mov_charc mc ON m.mid = mc.mid LEFT JOIN charctype c ON mc.cid = c.cid JOIN mov_subg ms ON m.mid = ms.mid LEFT JOIN subgenre s ON ms.sid = s.sid WHERE ..;

-- SEARCH movie by title
SELECT DISTINCT m.mid as id, title, year, duration FROM movie m JOIN mov_charc mc ON m.mid = mc.mid LEFT JOIN charctype c ON mc.cid = c.cid JOIN mov_subg ms ON m.mid = ms.mid LEFT JOIN subgenre s ON ms.sid = s.sid WHERE m.title LIKE + mysql.pool.escape('%'+req.params.s + '%');

-- LEGACY: Add a movie to a User's Watchlist
INSERT INTO watchlist (u_id, mid) VALUES (?,?);

-- LEGACY: Add a movie to a User's Favorites
INSERT INTO favorites (u_id, mid) VALUES (?,?);


/* --- infoMovie --- */

-- SELECT all charctypes from charctype
SELECT charctype.cid as id, name FROM charctype;

-- SELECT all subgenres from subgenre
SELECT subgenre.sid as id, name FROM subgenre;

-- SELECT a specific movie from movie
SELECT mid as id, title, year, duration, link FROM movie WHERE mid=?;

-- SELECT a specified movie id, title, and all associated charactypes from mov_charc
SELECT m.mid as id, title, c.cid, c.name FROM movie m JOIN mov_charc mc ON m.mid = mc.mid LEFT JOIN charctype c ON mc.cid = c.cid WHERE mc.mid=(SELECT mid FROM movie WHERE mid=?);

-- SELECT a specified movie id, title, and all associated subgenres from mov_subg
SELECT m.mid as id, title, s.sid, s.name FROM movie m JOIN mov_subg ms ON m.mid = ms.mid LEFT JOIN subgenre s ON ms.sid = s.sid WHERE ms.mid=(SELECT mid FROM movie WHERE mid=?);

-- SEARCH a User's Watchlist to see if a movie is already in there or not
SELECT mid FROM watchlist WHERE u_id=? AND mid=?;

-- SEARCH a User's Favorites to see if a movie is already in there or not
SELECT mid FROM favorites WHERE u_id=? AND mid=?

-- INSERT/ADD a specified movie into a User's Watchlist
INSERT INTO watchlist (u_id, mid,date_added) VALUES (?,?,now());

-- INSERT/ADD a specified movie into a User's Favorites
INSERT INTO favorites (u_id, mid,date_added) VALUES (?,?,now());

-- REMOVE specified movie from a User's Watchlist
DELETE FROM watchlist WHERE U_id = ? AND mid = ?;

-- REMOVE specified movie from a User's Favorites
DELETE FROM favorites WHERE U_id = ? AND mid = ?;


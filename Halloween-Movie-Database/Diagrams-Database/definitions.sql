-- Course: OSU CS 340 Intro to Databases
-- Name: Alexander Goodman
-- Assignment: Final Project
-- Due Date: 04 December 2018

-- Host: classmysql.engr.oregonstate.edu
-- Server Version:10.1.22-MariaDB 


DROP TABLE IF EXISTS `mov_subg`;
DROP TABLE IF EXISTS `mov_charc`;
DROP TABLE IF EXISTS `watchlist`;
DROP TABLE IF EXISTS `favorites`;
DROP TABLE IF EXISTS `subgenre`;
DROP TABLE IF EXISTS `charctype`;
DROP TABLE IF EXISTS `accounts`;
DROP TABLE IF EXISTS `movie`;


CREATE TABLE `movie`(
	`mid` int NOT NULL AUTO_INCREMENT,
	`title` varchar(100) NOT NULL,
	`year` int DEFAULT NULL,
	`duration` int DEFAULT NULL,
	`link` varchar(255) DEFAULT "",
	PRIMARY KEY(`mid`)
) ENGINE=InnoDB;


CREATE TABLE `subgenre` (
	`sid` int NOT NULL AUTO_INCREMENT,
	`name` varchar(50) NOT NULL,
	PRIMARY KEY(`sid`)
) ENGINE=InnoDB;


CREATE TABLE `charctype` (
	`cid` int NOT NULL AUTO_INCREMENT,
	`name` varchar(50) NOT NULL,
	PRIMARY KEY (`cid`)
) ENGINE=InnoDB;


CREATE TABLE `accounts` (
	`u_id` int NOT NULL AUTO_INCREMENT,
	`fname` varchar(50),
	`lname` varchar(50),
	`uname` varchar(50) NOT NULL UNIQUE,
	`date_created` datetime,
	`email` varchar(50) NOT NULL UNIQUE,
	`password` binary(60) NOT NULL,
	PRIMARY KEY(`u_id`)
) ENGINE=InnoDB;


CREATE TABLE `mov_subg` (
	`id` int NOT NULL AUTO_INCREMENT,
	`mid` int NOT NULL,
	`sid` int,
	PRIMARY KEY(`id`,`mid`),
	FOREIGN KEY(`mid`) REFERENCES `movie`(`mid`) ON DELETE CASCADE,
	FOREIGN KEY(`sid`) REFERENCES `subgenre`(`sid`) ON DELETE SET NULL ON UPDATE CASCADE
) ENGINE=InnoDB;

CREATE TABLE `mov_charc` (
	`id` int NOT NULL AUTO_INCREMENT,
	`mid` int NOT NULL,
	`cid` int,
	PRIMARY KEY(`id`,`mid`),
	FOREIGN KEY(`mid`) REFERENCES `movie`(`mid`) ON DELETE CASCADE,
	FOREIGN KEY(`cid`) REFERENCES `charctype`(`cid`) ON DELETE SET NULL ON UPDATE CASCADE
) ENGINE=InnoDB;


CREATE TABLE `watchlist` (
	`id` int NOT NULL AUTO_INCREMENT,
	`u_id` int NOT NULL,
	`mid` int,
	`date_added` datetime,
	PRIMARY KEY(`id`,`u_id`),
	FOREIGN KEY(`u_id`) REFERENCES `accounts`(`u_id`),
	FOREIGN KEY(`mid`) REFERENCES `movie`(`mid`) ON DELETE CASCADE
) ENGINE=InnoDB;


CREATE TABLE `favorites` (
	`id` int NOT NULL AUTO_INCREMENT,
	`u_id` int NOT NULL,
	`mid` int,
	`date_added` datetime,
	PRIMARY KEY(`id`,`u_id`),
	FOREIGN KEY(`u_id`) REFERENCES `accounts`(`u_id`),
	FOREIGN KEY(`mid`) REFERENCES `movie`(`mid`) ON DELETE CASCADE
) ENGINE=InnoDB;


/*** TEST INSERTS ***/

INSERT INTO `movie`(mid,title,year,duration) VALUES(1,'Hocus Pocus', 1993, 96),(2,'Halloween',1978,91),(3,'An American Werewolf in London',1981,97),(4,'Nosferatu',1922,81);
INSERT INTO `movie`(mid, title,year,duration) VALUES(5,'The Rocky Horror Picutre Show',1975,100), (6,'Practical Magic',1998,104),(7,"Trick `r Treat",2007,82);


INSERT INTO `subgenre` VALUES(1,'Horror'),(2,'Family'),(3,'Comedy'),(4,'Gore'),(5,'Thriller'),(6,'Fantasy'),(7,'Musical');

INSERT INTO `charctype` VALUES(1,'Serial Killer'),(2,'Witch/Wizard'),(3,'Vampire'),(4,'Werewolf'),(5,'Zombie'),(6,'Alien'),(7,'Demon');

INSERT INTO `mov_subg`(mid,sid) VALUES(1,2),(1,3);
INSERT INTO `mov_charc`(mid,cid) VALUES (1,2),(1,5);

INSERT INTO `mov_subg`(mid,sid) VALUES(2,1),(2,5);
INSERT INTO `mov_charc`(mid,cid) VALUES(2,1);

INSERT INTO `mov_subg`(mid,sid) VALUES(3,1),(3,3);
INSERT INTO `mov_charc`(mid,cid) VALUES(3,4);

INSERT INTO `mov_subg`(mid,sid) VALUES(4,1),(4,6);
INSERT INTO `mov_charc`(mid,cid) VALUES(4,3);

INSERT INTO `mov_subg`(mid,sid) VALUES(5,3),(5,7);
INSERT INTO `mov_charc`(mid,cid) VALUES(5,6);

INSERT INTO `mov_subg`(mid,sid) VALUES(6,6);
INSERT INTO `mov_charc`(mid,cid) VALUES(6,2),(6,5);

INSERT INTO `mov_subg`(mid,sid) VALUES(7,1),(7,5);
INSERT INTO `mov_charc`(mid,cid) VALUES(7,1),(7,4),(7,5),(7,7);





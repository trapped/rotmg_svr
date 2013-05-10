CREATE DATABASE  IF NOT EXISTS `rotmg` /*!40100 DEFAULT CHARACTER SET utf8 */;
USE `rotmg`;
-- MySQL dump 10.13  Distrib 5.6.10, for Win32 (x86)
--
-- Host: localhost    Database: rotmg
-- ------------------------------------------------------
-- Server version	5.6.10

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8 */;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;

--
-- Table structure for table `accounts`
--

DROP TABLE IF EXISTS `accounts`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `accounts` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `uuid` varchar(128) NOT NULL,
  `password` varchar(256) NOT NULL,
  `name` varchar(64) NOT NULL DEFAULT 'DEFAULT',
  `admin` tinyint(4) NOT NULL DEFAULT '0',
  `namechosen` tinyint(1) NOT NULL DEFAULT '0',
  `verified` tinyint(1) NOT NULL DEFAULT '0',
  `guild` varchar(11) NOT NULL DEFAULT '',
  `guildRank` int(11) NOT NULL DEFAULT '0',
  `vaultCount` int(11) NOT NULL DEFAULT '1',
  `maxCharSlot` int(11) NOT NULL DEFAULT '1',
  `regTime` datetime NOT NULL,
  `guest` tinyint(1) NOT NULL DEFAULT '0',
  PRIMARY KEY (`id`)
) ENGINE=MyISAM AUTO_INCREMENT=3 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `accounts`
--

LOCK TABLES `accounts` WRITE;
/*!40000 ALTER TABLE `accounts` DISABLE KEYS */;
INSERT INTO `accounts` VALUES (1,'trapped','59f2f19eba8a32fe00d6ca1e288fb31cd01c8487','Trapped',1,1,0,'gg',0,1,5,'2013-05-07 19:06:56',0),(2,'debugger@test.it','5fbd02245cf700ea94d638c8d76924ecca52d330','Drol',0,0,0,'0',0,1,1,'2013-05-10 15:09:24',0);
/*!40000 ALTER TABLE `accounts` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `characters`
--

DROP TABLE IF EXISTS `characters`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `characters` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `accId` int(11) NOT NULL,
  `charId` int(11) NOT NULL,
  `charType` int(11) NOT NULL DEFAULT '782',
  `level` int(11) NOT NULL DEFAULT '1',
  `exp` int(11) NOT NULL DEFAULT '0',
  `fame` int(11) NOT NULL DEFAULT '0',
  `items` varchar(128) NOT NULL DEFAULT '-1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1',
  `hp` int(11) NOT NULL DEFAULT '1',
  `mp` int(11) NOT NULL DEFAULT '1',
  `stats` varchar(64) NOT NULL DEFAULT '1, 1, 1, 1, 1, 1, 1, 1',
  `dead` tinyint(1) NOT NULL DEFAULT '0',
  `tex1` int(11) NOT NULL DEFAULT '0',
  `tex2` int(11) NOT NULL DEFAULT '0',
  `pet` int(11) NOT NULL DEFAULT '-1',
  `fameStats` varchar(128) NOT NULL,
  `createTime` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `deathTime` timestamp NOT NULL DEFAULT '0000-00-00 00:00:00',
  `totalFame` int(11) NOT NULL DEFAULT '0',
  PRIMARY KEY (`id`)
) ENGINE=MyISAM AUTO_INCREMENT=10 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `characters`
--

LOCK TABLES `characters` WRITE;
/*!40000 ALTER TABLE `characters` DISABLE KEYS */;
INSERT INTO `characters` VALUES (5,1,5,784,20,702305,702,'2879, 3845, -1, -1, 2594, 2564, -1, -1, -1, 2609, -1, -1',575,290,'575, 290, 29, 0, 42, 21, 45, 31',0,0,0,-1,'eNoVx8kVgCAQBNEGcUEFd_FilKZkdibh1KH_q5bkPme8XlKspDsFK9XQSPlpoeNG6GGAERJkmGCGBVbYYLf5gzqhwAU_XIUD9Q==','2013-05-08 13:54:15','0000-00-00 00:00:00',0),(4,1,1,782,20,29750,29,'2711, 2606, -1, -1, 2594, -1, -1, -1, -1, -1, -1, -1',574,289,'574, 289, 40, 0, 29, 21, 31, 43',0,0,0,-1,'eNoVx8kVgCAAA9GAO6KA4FKBJdB_XZ5kDvkvI8m-RjLBSnKdlGrfngYY274JZnIBByt42GCHABESHJChUCfvghse-AEdoAO8','2013-05-08 13:52:36','0000-00-00 00:00:00',0),(6,1,6,775,1,39,0,'-1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1',130,100,'130, 100, 12, 0, 12, 12, 10, 12',0,1048575,0,-1,'eNoVyLkVgDAMBNG1OYy5bc4UYhqhFPpvAE2w_40kKX5O8reXFArpfUorVVDbrgANZ4TW5jqqhwFGmGCGBBkWWPlv1A4HnPADxa4Ckg==','2013-05-08 14:23:01','0000-00-00 00:00:00',0),(8,1,7,801,20,63831,63,'3075, 2730, -1, -1, 5388, -1, -1, -1, -1, -1, -1, -1',670,385,'670, 385, 60, 25, 50, 30, 75, 60',0,167772161,167772161,-1,'eNoVyccRgDAQQ1HZmGByhjMV0ARDM5RFAZTI_oPeSCNJd3BSPL2kJ5H_3mDNpYYyKcQcCltHhNKIFW8NDbTQQQ8DjDDBbLkW2gob7PADVHsEIA==','2013-05-08 16:09:08','0000-00-00 00:00:00',0);
/*!40000 ALTER TABLE `characters` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `classstats`
--

DROP TABLE IF EXISTS `classstats`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `classstats` (
  `accId` int(11) NOT NULL,
  `objType` int(11) NOT NULL,
  `bestLv` int(11) NOT NULL DEFAULT '1',
  `bestFame` int(11) NOT NULL DEFAULT '0',
  PRIMARY KEY (`accId`,`objType`)
) ENGINE=MyISAM DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `classstats`
--

LOCK TABLES `classstats` WRITE;
/*!40000 ALTER TABLE `classstats` DISABLE KEYS */;
INSERT INTO `classstats` VALUES (1,782,100000000,1410065),(1,784,20,702),(1,775,1,0),(1,801,20,63),(2,782,1,0);
/*!40000 ALTER TABLE `classstats` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `death`
--

DROP TABLE IF EXISTS `death`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `death` (
  `accId` int(11) NOT NULL,
  `chrId` int(11) NOT NULL,
  `name` varchar(64) NOT NULL DEFAULT 'DEFAULT',
  `charType` int(11) NOT NULL DEFAULT '782',
  `tex1` int(11) NOT NULL DEFAULT '0',
  `tex2` int(11) NOT NULL DEFAULT '0',
  `items` varchar(128) NOT NULL DEFAULT '-1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1',
  `fame` int(11) NOT NULL DEFAULT '0',
  `fameStats` varchar(128) NOT NULL,
  `totalFame` int(11) NOT NULL DEFAULT '0',
  `firstBorn` tinyint(1) NOT NULL,
  `killer` varchar(128) NOT NULL,
  `time` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`accId`,`chrId`)
) ENGINE=MyISAM DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `death`
--

LOCK TABLES `death` WRITE;
/*!40000 ALTER TABLE `death` DISABLE KEYS */;
/*!40000 ALTER TABLE `death` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `guilds`
--

DROP TABLE IF EXISTS `guilds`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `guilds` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `name` varchar(45) NOT NULL DEFAULT 'DEFAULT_GUILD',
  `members` varchar(45) NOT NULL,
  `level` varchar(45) NOT NULL DEFAULT '1',
  `createTime` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`id`),
  UNIQUE KEY `id_UNIQUE` (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=6 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `guilds`
--

LOCK TABLES `guilds` WRITE;
/*!40000 ALTER TABLE `guilds` DISABLE KEYS */;
INSERT INTO `guilds` VALUES (3,'gg',',1,','1','2013-05-09 13:49:45'),(4,'',',1,','1','2013-05-10 08:27:41'),(5,'m',',1,','1','2013-05-10 08:37:20');
/*!40000 ALTER TABLE `guilds` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `news`
--

DROP TABLE IF EXISTS `news`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `news` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `icon` varchar(16) NOT NULL DEFAULT 'info',
  `title` varchar(128) NOT NULL DEFAULT 'Default news title',
  `text` varchar(128) NOT NULL DEFAULT 'Default news text',
  `link` varchar(256) NOT NULL DEFAULT 'http://forums.wildshadow.com/',
  `date` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`id`)
) ENGINE=MyISAM AUTO_INCREMENT=2 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `news`
--

LOCK TABLES `news` WRITE;
/*!40000 ALTER TABLE `news` DISABLE KEYS */;
INSERT INTO `news` VALUES (1,'info','Database reset eseguito','Ora è più performante!','http://redis.io/','2013-05-08 14:30:01');
/*!40000 ALTER TABLE `news` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `stats`
--

DROP TABLE IF EXISTS `stats`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `stats` (
  `accId` int(11) NOT NULL,
  `fame` int(11) NOT NULL,
  `totalFame` int(11) NOT NULL,
  `credits` int(11) NOT NULL,
  `totalCredits` int(11) NOT NULL,
  PRIMARY KEY (`accId`)
) ENGINE=MyISAM DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `stats`
--

LOCK TABLES `stats` WRITE;
/*!40000 ALTER TABLE `stats` DISABLE KEYS */;
INSERT INTO `stats` VALUES (1,0,0,1000300,100),(2,0,0,100,100);
/*!40000 ALTER TABLE `stats` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `vaults`
--

DROP TABLE IF EXISTS `vaults`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `vaults` (
  `accId` int(11) NOT NULL,
  `chestId` int(11) NOT NULL AUTO_INCREMENT,
  `items` varchar(128) NOT NULL,
  PRIMARY KEY (`accId`,`chestId`)
) ENGINE=MyISAM DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `vaults`
--

LOCK TABLES `vaults` WRITE;
/*!40000 ALTER TABLE `vaults` DISABLE KEYS */;
INSERT INTO `vaults` VALUES (1,1,'-1, -1, -1, -1, -1, -1, -1, -1'),(1,2,'-1, -1, -1, -1, -1, -1, -1, -1'),(1,3,'-1, -1, -1, -1, -1, -1, -1, -1'),(1,4,'-1, -1, -1, -1, -1, -1, -1, -1'),(1,5,'-1, -1, -1, -1, -1, -1, -1, -1'),(1,6,'-1, -1, -1, -1, -1, -1, -1, -1'),(1,7,'-1, -1, -1, -1, -1, -1, -1, -1'),(1,8,'-1, -1, -1, -1, -1, -1, -1, -1'),(1,9,'-1, -1, -1, -1, -1, -1, -1, -1'),(1,10,'-1, -1, -1, -1, -1, -1, -1, -1'),(1,11,'-1, -1, -1, -1, -1, -1, -1, -1'),(1,12,'-1, -1, -1, -1, -1, -1, -1, -1'),(1,13,'-1, -1, -1, -1, -1, -1, -1, -1'),(1,14,'-1, -1, -1, -1, -1, -1, -1, -1'),(1,15,'-1, -1, -1, -1, -1, -1, -1, -1'),(1,16,'-1, -1, -1, -1, -1, -1, -1, -1'),(1,17,'-1, -1, -1, -1, -1, -1, -1, -1'),(1,18,'-1, -1, -1, -1, -1, -1, -1, -1'),(1,19,'-1, -1, -1, -1, -1, -1, -1, -1'),(1,20,'-1, -1, -1, -1, -1, -1, -1, -1'),(2,1,'-1, -1, -1, -1, -1, -1, -1, -1');
/*!40000 ALTER TABLE `vaults` ENABLE KEYS */;
UNLOCK TABLES;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2013-05-10 17:46:40

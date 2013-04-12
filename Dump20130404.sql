CREATE DATABASE  IF NOT EXISTS `rotmg` /*!40100 DEFAULT CHARACTER SET utf8 */;
USE `rotmg`;
-- MySQL dump 10.13  Distrib 5.6.10, for Win64 (x86_64)
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
  `name` varchar(64) NOT NULL,
  `admin` tinyint(1) NOT NULL DEFAULT '0',
  `namechosen` tinyint(1) NOT NULL DEFAULT '0',
  `verified` tinyint(1) NOT NULL DEFAULT '0',
  `guild` int(11) NOT NULL DEFAULT '0',
  `guildRank` int(11) NOT NULL DEFAULT '0',
  `vaultCount` int(11) NOT NULL DEFAULT '1',
  `maxCharSlot` int(11) NOT NULL DEFAULT '0',
  `regTime` datetime NOT NULL DEFAULT '0000-00-00 00:00:00',
  `guest` tinyint(1) NOT NULL DEFAULT '1',
  PRIMARY KEY (`id`)
) ENGINE=MyISAM AUTO_INCREMENT=5 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `accounts`
--

LOCK TABLES `accounts` WRITE;
/*!40000 ALTER TABLE `accounts` DISABLE KEYS */;
INSERT INTO `accounts` VALUES (1,'pop@gmail.com','7c4a8d09ca3762af61e59520943dc26494f8941b','Seus',0,0,0,0,0,1,1,'0000-00-00 00:00:00',0),(2,'asd@gmail.com','7c4a8d09ca3762af61e59520943dc26494f8941b','Radph',0,0,0,0,0,1,1,'0000-00-00 00:00:00',0),(3,'popmon@op.com','7c4a8d09ca3762af61e59520943dc26494f8941b','Scheev',0,0,0,0,0,1,1,'0000-00-00 00:00:00',0),(4,'botmaker@gmail.com','7c222fb2927d828af22f592134e8932480637c0d','BotMaker',0,1,0,0,0,1,1,'0000-00-00 00:00:00',0);
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
  `charType` int(11) NOT NULL,
  `level` int(11) NOT NULL,
  `exp` int(11) NOT NULL,
  `fame` int(11) NOT NULL,
  `items` varchar(128) NOT NULL,
  `hp` int(11) NOT NULL,
  `mp` int(11) NOT NULL,
  `stats` varchar(64) NOT NULL DEFAULT '"1,2,3,4,5,6,7,8"',
  `dead` tinyint(1) NOT NULL DEFAULT '0',
  `tex1` int(11) NOT NULL DEFAULT '0',
  `tex2` int(11) NOT NULL DEFAULT '0',
  `pet` int(11) NOT NULL DEFAULT '0',
  `fameStats` varchar(128) NOT NULL DEFAULT 'eNoVytkRgCAMRdGH4IIbgmsPdmNVNmZf5n7kzM0kksLjJN2V4b30vcHK1YYam9hCxxqh5zpQI0wwQ4IFMhRYYeNjpw444YIfA3kDIA',
  `createTime` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `deathTime` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `totalFame` int(11) NOT NULL DEFAULT '0',
  PRIMARY KEY (`id`)
) ENGINE=MyISAM AUTO_INCREMENT=6 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `characters`
--

LOCK TABLES `characters` WRITE;
/*!40000 ALTER TABLE `characters` DISABLE KEYS */;
INSERT INTO `characters` VALUES (1,1,1,782,0,0,0,'-1, -1, -1, -1, -1, -1, -1, -1',50,50,'1,2,3,4,5,6,7,8',0,0,0,0,'eNoVytkRgCAMRdGH4IIbgmsPdmNVNmZf5n7kzM0kksLjJN2V4b30vcHK1YYam9hCxxqh5zpQI0wwQ4IFMhRYYeNjpw444YIfA3kDIA','2013-04-04 02:35:57','2013-04-04 02:35:57',0),(2,2,1,782,0,0,0,'-1, -1, -1, -1, -1, -1, -1, -1',50,50,'1,2,3,4,5,6,7,8',0,0,0,0,'eNoVytkRgCAMRdGH4IIbgmsPdmNVNmZf5n7kzM0kksLjJN2V4b30vcHK1YYam9hCxxqh5zpQI0wwQ4IFMhRYYeNjpw444YIfA3kDIA','2013-04-04 02:35:57','2013-04-04 02:35:57',0),(3,3,1,782,0,0,0,'-1, -1, -1, -1, -1, -1, -1, -1',50,50,'1,2,3,4,5,6,7,8',0,0,0,0,'eNoVytkRgCAMRdGH4IIbgmsPdmNVNmZf5n7kzM0kksLjJN2V4b30vcHK1YYam9hCxxqh5zpQI0wwQ4IFMhRYYeNjpw444YIfA3kDIA','2013-04-04 02:35:57','2013-04-04 02:35:57',0),(4,4,1,782,4,712,0,'2712, 2606, -1, 2600, 2594, -1, -1, -1, 2678, 2675, 2681, -1',-18,165,'170, 125, 16, 0, 12, 13, 14, 19',1,0,0,-1,'eNoVy7kVgDAMBNE15jaYw4DphJyIiuiRrtAE-98okKT4OMlfhaTgpfcurVRBbfsaaDk76G0pUAOMEGGCGRZYIcHG104dkOGEH1jOBC8=','2013-04-04 02:58:58','2013-04-04 03:18:35',0),(5,4,5,782,15,10063,10,'2825, 3084, 2823, 3110, -1, -1, -1, -1, -1, -1, -1, -1',504,319,'464, 254, 35, 0, 26, 20, 28, 37',0,0,0,-1,'eNoVyTsaQDAUhNFJEG8R70KtsE5rpLMBm5C_mPPNvSPpuI3kLivpTGTeJ41NGTjJfjkU8QolVPxr1gZa6MBDDwEGGGGK2WfaAits8ANj6Qax','2013-04-04 03:18:41','2013-04-04 03:18:41',0);
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
  `bestLv` int(11) NOT NULL,
  `bestFame` int(11) NOT NULL,
  PRIMARY KEY (`accId`,`objType`)
) ENGINE=MyISAM DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `classstats`
--

LOCK TABLES `classstats` WRITE;
/*!40000 ALTER TABLE `classstats` DISABLE KEYS */;
INSERT INTO `classstats` VALUES (4,782,15,10);
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
  `name` varchar(64) NOT NULL,
  `charType` int(11) NOT NULL,
  `tex1` int(11) NOT NULL,
  `tex2` int(11) NOT NULL,
  `items` varchar(128) NOT NULL,
  `fame` int(11) NOT NULL,
  `fameStats` varchar(128) NOT NULL,
  `totalFame` int(11) NOT NULL,
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
INSERT INTO `death` VALUES (4,1,'Iri',782,0,0,'2712, 2606, -1, 2600, 2594, -1, -1, -1, 2678, 2675, 2681, -1',0,'eNoVy7kVgDAMBNE15jaYw4DphJyIiuiRrtAE-98okKT4OMlfhaTgpfcurVRBbfsaaDk76G0pUAOMEGGCGRZYIcHG104dkOGEH1jOBC8=',20,1,'Gray Satellite','2013-04-04 03:18:35');
/*!40000 ALTER TABLE `death` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `news`
--

DROP TABLE IF EXISTS `news`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `news` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `icon` varchar(16) NOT NULL,
  `title` varchar(128) NOT NULL,
  `text` varchar(128) NOT NULL,
  `link` varchar(256) NOT NULL,
  `date` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`id`)
) ENGINE=MyISAM DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `news`
--

LOCK TABLES `news` WRITE;
/*!40000 ALTER TABLE `news` DISABLE KEYS */;
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
INSERT INTO `stats` VALUES (3,0,0,1000,1000),(4,20,20,1000,1000);
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
INSERT INTO `vaults` VALUES (3,1,'-1, -1, -1, -1, -1, -1, -1, -1'),(4,1,'-1, -1, -1, -1, -1, -1, -1, -1');
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

-- Dump completed on 2013-04-04 16:31:36

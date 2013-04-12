--
-- Database: `rotmg`
--

-- --------------------------------------------------------

--
-- Table structure for table `accounts`
--

CREATE TABLE `accounts` (
  `id` int(11) NOT NULL auto_increment,
  `uuid` varchar(128) character set utf8 NOT NULL,
  `password` varchar(256) character set utf8 NOT NULL,
  `name` varchar(64) character set utf8 NOT NULL,
  `admin` tinyint(0) NOT NULL,
  `namechosen` tinyint(1) NOT NULL,
  `verified` tinyint(1) NOT NULL,
  `guild` int(11) NOT NULL,
  `guildRank` int(11) NOT NULL,
  `vaultCount` int(11) NOT NULL,
  `maxCharSlot` int(11) NOT NULL,
  `regTime` datetime NOT NULL,
  `guest` tinyint(1) NOT NULL,
  PRIMARY KEY  (`id`)
) ENGINE=MyISAM  DEFAULT CHARSET=utf8;

-- --------------------------------------------------------

--
-- Table structure for table `characters`
--

CREATE TABLE `characters` (
  `id` int(11) NOT NULL auto_increment,
  `accId` int(11) NOT NULL,
  `charId` int(11) NOT NULL,
  `charType` int(11) NOT NULL,
  `level` int(11) NOT NULL,
  `exp` int(11) NOT NULL,
  `fame` int(11) NOT NULL,
  `items` varchar(128) NOT NULL,
  `hp` int(11) NOT NULL,
  `mp` int(11) NOT NULL,
  `stats` varchar(64) NOT NULL,
  `dead` tinyint(1) NOT NULL,
  `tex1` int(11) NOT NULL,
  `tex2` int(11) NOT NULL,
  `pet` int(11) NOT NULL,
  `fameStats` varchar(128) NOT NULL,
  `createTime` timestamp NOT NULL default CURRENT_TIMESTAMP,
  `deathTime` timestamp NOT NULL default '0000-00-00 00:00:00',
  `totalFame` int(11) NOT NULL,
  PRIMARY KEY  (`id`)
) ENGINE=MyISAM  DEFAULT CHARSET=utf8;

-- --------------------------------------------------------

--
-- Table structure for table `classstats`
--

CREATE TABLE `classstats` (
  `accId` int(11) NOT NULL,
  `objType` int(11) NOT NULL,
  `bestLv` int(11) NOT NULL,
  `bestFame` int(11) NOT NULL,
  PRIMARY KEY  (`accId`,`objType`)
) ENGINE=MyISAM DEFAULT CHARSET=utf8;

-- --------------------------------------------------------

--
-- Table structure for table `death`
--

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
  `time` timestamp NOT NULL default CURRENT_TIMESTAMP,
  PRIMARY KEY  (`accId`,`chrId`)
) ENGINE=MyISAM DEFAULT CHARSET=utf8;

-- --------------------------------------------------------

--
-- Table structure for table `news`
--

CREATE TABLE `news` (
  `id` int(11) NOT NULL auto_increment,
  `icon` varchar(16) NOT NULL,
  `title` varchar(128) NOT NULL,
  `text` varchar(128) NOT NULL,
  `link` varchar(256) NOT NULL,
  `date` timestamp NOT NULL default CURRENT_TIMESTAMP,
  PRIMARY KEY  (`id`)
) ENGINE=MyISAM  DEFAULT CHARSET=utf8;

-- --------------------------------------------------------

--
-- Table structure for table `stats`
--

CREATE TABLE `stats` (
  `accId` int(11) NOT NULL,
  `fame` int(11) NOT NULL,
  `totalFame` int(11) NOT NULL,
  `credits` int(11) NOT NULL,
  `totalCredits` int(11) NOT NULL,
  PRIMARY KEY  (`accId`)
) ENGINE=MyISAM DEFAULT CHARSET=utf8;

-- --------------------------------------------------------

--
-- Table structure for table `vaults`
--

CREATE TABLE `vaults` (
  `accId` int(11) NOT NULL,
  `chestId` int(11) NOT NULL auto_increment,
  `items` varchar(128) NOT NULL,
  PRIMARY KEY  (`accId`,`chestId`)
) ENGINE=MyISAM DEFAULT CHARSET=utf8;

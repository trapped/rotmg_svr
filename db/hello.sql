SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0;
SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0;
SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='TRADITIONAL,ALLOW_INVALID_DATES';


-- -----------------------------------------------------
-- Table `ROTMG`.`accounts`
-- -----------------------------------------------------
CREATE  TABLE IF NOT EXISTS `ROTMG`.`accounts` (
  `id` INT(11) NOT NULL AUTO_INCREMENT ,
  `uuid` VARCHAR(128) CHARACTER SET 'utf8' NOT NULL ,
  `password` VARCHAR(256) CHARACTER SET 'utf8' NOT NULL ,
  `name` VARCHAR(64) CHARACTER SET 'utf8' NOT NULL ,
  `admin` TINYINT(1) NOT NULL DEFAULT 0 ,
  `namechosen` TINYINT(1) NOT NULL ,
  `verified` TINYINT(1) NOT NULL ,
  `guild` INT(11) NOT NULL ,
  `guildRank` INT(11) NOT NULL ,
  `vaultCount` INT(11) NOT NULL ,
  `maxCharSlot` INT(11) NOT NULL ,
  `regTime` DATETIME NOT NULL ,
  `guest` TINYINT(1) NOT NULL ,
  PRIMARY KEY (`id`) )
ENGINE = MyISAM
DEFAULT CHARACTER SET = utf8;


-- -----------------------------------------------------
-- Table `ROTMG`.`characters`
-- -----------------------------------------------------
CREATE  TABLE IF NOT EXISTS `ROTMG`.`characters` (
  `id` INT(11) NOT NULL AUTO_INCREMENT ,
  `accId` INT(11) NOT NULL ,
  `charId` INT(11) NOT NULL ,
  `charType` INT(11) NOT NULL ,
  `level` INT(11) NOT NULL ,
  `exp` INT(11) NOT NULL ,
  `fame` INT(11) NOT NULL ,
  `items` VARCHAR(128) NOT NULL ,
  `hp` INT(11) NOT NULL ,
  `mp` INT(11) NOT NULL ,
  `stats` VARCHAR(64) NOT NULL ,
  `dead` TINYINT(1) NOT NULL ,
  `tex1` INT(11) NOT NULL ,
  `tex2` INT(11) NOT NULL ,
  `pet` INT(11) NOT NULL ,
  `fameStats` VARCHAR(128) NOT NULL ,
  `createTime` TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ,
  `deathTime` TIMESTAMP NOT NULL DEFAULT '0000-00-00 00:00:00' ,
  `totalFame` INT(11) NOT NULL ,
  PRIMARY KEY (`id`) )
ENGINE = MyISAM
DEFAULT CHARACTER SET = utf8;


-- -----------------------------------------------------
-- Table `ROTMG`.`classstats`
-- -----------------------------------------------------
CREATE  TABLE IF NOT EXISTS `ROTMG`.`classstats` (
  `accId` INT(11) NOT NULL ,
  `objType` INT(11) NOT NULL ,
  `bestLv` INT(11) NOT NULL ,
  `bestFame` INT(11) NOT NULL ,
  PRIMARY KEY (`accId`, `objType`) )
ENGINE = MyISAM
DEFAULT CHARACTER SET = utf8;


-- -----------------------------------------------------
-- Table `ROTMG`.`death`
-- -----------------------------------------------------
CREATE  TABLE IF NOT EXISTS `ROTMG`.`death` (
  `accId` INT(11) NOT NULL ,
  `chrId` INT(11) NOT NULL ,
  `name` VARCHAR(64) NOT NULL ,
  `charType` INT(11) NOT NULL ,
  `tex1` INT(11) NOT NULL ,
  `tex2` INT(11) NOT NULL ,
  `items` VARCHAR(128) NOT NULL ,
  `fame` INT(11) NOT NULL ,
  `fameStats` VARCHAR(128) NOT NULL ,
  `totalFame` INT(11) NOT NULL ,
  `firstBorn` TINYINT(1) NOT NULL ,
  `killer` VARCHAR(128) NOT NULL ,
  `time` TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ,
  PRIMARY KEY (`accId`, `chrId`) )
ENGINE = MyISAM
DEFAULT CHARACTER SET = utf8;


-- -----------------------------------------------------
-- Table `ROTMG`.`news`
-- -----------------------------------------------------
CREATE  TABLE IF NOT EXISTS `ROTMG`.`news` (
  `id` INT(11) NOT NULL AUTO_INCREMENT ,
  `icon` VARCHAR(16) NOT NULL ,
  `title` VARCHAR(128) NOT NULL ,
  `text` VARCHAR(128) NOT NULL ,
  `link` VARCHAR(256) NOT NULL ,
  `date` TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ,
  PRIMARY KEY (`id`) )
ENGINE = MyISAM
DEFAULT CHARACTER SET = utf8;


-- -----------------------------------------------------
-- Table `ROTMG`.`stats`
-- -----------------------------------------------------
CREATE  TABLE IF NOT EXISTS `ROTMG`.`stats` (
  `accId` INT(11) NOT NULL ,
  `fame` INT(11) NOT NULL ,
  `totalFame` INT(11) NOT NULL ,
  `credits` INT(11) NOT NULL ,
  `totalCredits` INT(11) NOT NULL ,
  PRIMARY KEY (`accId`) )
ENGINE = MyISAM
DEFAULT CHARACTER SET = utf8;


-- -----------------------------------------------------
-- Table `ROTMG`.`vaults`
-- -----------------------------------------------------
CREATE  TABLE IF NOT EXISTS `ROTMG`.`vaults` (
  `accId` INT(11) NOT NULL ,
  `chestId` INT(11) NOT NULL AUTO_INCREMENT ,
  `items` VARCHAR(128) NOT NULL ,
  PRIMARY KEY (`accId`, `chestId`) )
ENGINE = MyISAM
DEFAULT CHARACTER SET = utf8;



SET SQL_MODE=@OLD_SQL_MODE;
SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS;
SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS;

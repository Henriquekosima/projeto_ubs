-- MySQL Script generated by MySQL Workbench
-- Mon Dec  2 12:21:51 2019
-- Model: New Model    Version: 1.0
-- MySQL Workbench Forward Engineering

SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0;
SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0;
SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='ONLY_FULL_GROUP_BY,STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ZERO_DATE,ERROR_FOR_DIVISION_BY_ZERO,NO_ENGINE_SUBSTITUTION';

-- -----------------------------------------------------
-- Schema mydb
-- -----------------------------------------------------

-- -----------------------------------------------------
-- Schema mydb
-- -----------------------------------------------------
CREATE SCHEMA IF NOT EXISTS `mydb` DEFAULT CHARACTER SET utf8 ;
USE `mydb` ;

-- -----------------------------------------------------
-- Table `mydb`.`attendant`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `mydb`.`attendant` (
  `AttendantID` INT(11) NOT NULL AUTO_INCREMENT,
  `AttendantCpf` VARCHAR(45) NOT NULL,
  `AttendantEmail` VARCHAR(45) NOT NULL,
  `AttendantName` VARCHAR(45) NOT NULL,
  `AttendantPass` VARCHAR(45) NOT NULL,
  PRIMARY KEY (`AttendantID`))
ENGINE = InnoDB
AUTO_INCREMENT = 12
DEFAULT CHARACTER SET = utf8;


-- -----------------------------------------------------
-- Table `mydb`.`user`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `mydb`.`user` (
  `UserID` INT(11) NOT NULL AUTO_INCREMENT,
  `UserName` VARCHAR(45) NOT NULL,
  `UserCpf` VARCHAR(45) NOT NULL,
  `UserEmail` VARCHAR(45) NOT NULL,
  PRIMARY KEY (`UserID`))
ENGINE = InnoDB
AUTO_INCREMENT = 11
DEFAULT CHARACTER SET = utf8;


-- -----------------------------------------------------
-- Table `mydb`.`dependent`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `mydb`.`dependent` (
  `DependentID` INT(11) NOT NULL AUTO_INCREMENT,
  `DependentName` VARCHAR(45) NOT NULL,
  `DependentBirth` DATE NOT NULL,
  `DependentBlood` CHAR(3) NOT NULL,
  `DependentAllergy` VARCHAR(45) NOT NULL,
  `DependentSusNo` VARCHAR(15) NOT NULL,
  `User_UserID` INT(11) NOT NULL,
  PRIMARY KEY (`DependentID`, `User_UserID`),
  INDEX `fk_Dependent_User1_idx` (`User_UserID` ASC) VISIBLE,
  CONSTRAINT `fk_Dependent_User1`
    FOREIGN KEY (`User_UserID`)
    REFERENCES `mydb`.`user` (`UserID`))
ENGINE = InnoDB
AUTO_INCREMENT = 16
DEFAULT CHARACTER SET = utf8;


-- -----------------------------------------------------
-- Table `mydb`.`dose`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `mydb`.`dose` (
  `DoseID` INT(11) NOT NULL AUTO_INCREMENT,
  `DoseType` VARCHAR(45) NULL DEFAULT NULL,
  PRIMARY KEY (`DoseID`))
ENGINE = InnoDB
AUTO_INCREMENT = 5
DEFAULT CHARACTER SET = utf8;


-- -----------------------------------------------------
-- Table `mydb`.`user_dep`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `mydb`.`user_dep` (
  `User_UserID` INT(11) NOT NULL,
  `Dep_DependentID` INT(11) NOT NULL,
  PRIMARY KEY (`User_UserID`, `Dep_DependentID`),
  INDEX `fk_Usuario_has_Dependentes_Dependentes1_idx` (`Dep_DependentID` ASC) VISIBLE,
  INDEX `fk_Usuario_has_Dependentes_Usuario1_idx` (`User_UserID` ASC) VISIBLE,
  CONSTRAINT `fk_Usuario_has_Dependentes_Dependentes1`
    FOREIGN KEY (`Dep_DependentID`)
    REFERENCES `mydb`.`dependent` (`DependentID`),
  CONSTRAINT `fk_Usuario_has_Dependentes_Usuario1`
    FOREIGN KEY (`User_UserID`)
    REFERENCES `mydb`.`user` (`UserID`))
ENGINE = InnoDB
DEFAULT CHARACTER SET = utf8;


-- -----------------------------------------------------
-- Table `mydb`.`vaccine`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `mydb`.`vaccine` (
  `VaccineID` INT(11) NOT NULL AUTO_INCREMENT,
  `VaccineName` VARCHAR(100) NULL DEFAULT NULL,
  PRIMARY KEY (`VaccineID`))
ENGINE = InnoDB
AUTO_INCREMENT = 13
DEFAULT CHARACTER SET = utf8;


-- -----------------------------------------------------
-- Table `mydb`.`vaccine_dep`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `mydb`.`vaccine_dep` (
  `VaccineID` INT(11) NOT NULL,
  `DependentID` INT(11) NOT NULL,
  PRIMARY KEY (`VaccineID`, `DependentID`),
  INDEX `fk_Vacina_idx` (`VaccineID` ASC) VISIBLE,
  INDEX `fk_Dep_idx` (`DependentID` ASC) VISIBLE,
  CONSTRAINT `fk_Dep`
    FOREIGN KEY (`DependentID`)
    REFERENCES `mydb`.`dependent` (`DependentID`),
  CONSTRAINT `fk_Vacina`
    FOREIGN KEY (`VaccineID`)
    REFERENCES `mydb`.`vaccine` (`VaccineID`))
ENGINE = InnoDB
DEFAULT CHARACTER SET = utf8;


-- -----------------------------------------------------
-- Table `mydb`.`vaccine_dose`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `mydb`.`vaccine_dose` (
  `vaccineDate` DATE NULL DEFAULT NULL,
  `Vaccineid` INT(11) NOT NULL,
  `Doseid` INT(11) NOT NULL,
  PRIMARY KEY (`Vaccineid`, `Doseid`),
  INDEX `fk_vaccine_idx` (`Vaccineid` ASC) VISIBLE,
  INDEX `fk_dose_idx` (`Doseid` ASC) VISIBLE,
  CONSTRAINT `fk_dose`
    FOREIGN KEY (`Doseid`)
    REFERENCES `mydb`.`dose` (`DoseID`),
  CONSTRAINT `fk_vac`
    FOREIGN KEY (`Vaccineid`)
    REFERENCES `mydb`.`vaccine` (`VaccineID`))
ENGINE = InnoDB
DEFAULT CHARACTER SET = utf8;


SET SQL_MODE=@OLD_SQL_MODE;
SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS;
SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS;

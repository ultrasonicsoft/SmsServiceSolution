CREATE DATABASE  IF NOT EXISTS `mitto_sms_db` /*!40100 DEFAULT CHARACTER SET utf8 */;
USE `mitto_sms_db`;
-- MySQL dump 10.13  Distrib 5.6.24, for Win64 (x86_64)
--
-- Host: localhost    Database: mitto_sms_db
-- ------------------------------------------------------
-- Server version	5.6.26-log

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
-- Table structure for table `countries`
--

DROP TABLE IF EXISTS `countries`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `countries` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `name` varchar(100) DEFAULT NULL,
  `mcc` varchar(10) DEFAULT NULL,
  `cc` varchar(10) DEFAULT NULL,
  `price_per_sms` double DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=4 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `countries`
--

LOCK TABLES `countries` WRITE;
/*!40000 ALTER TABLE `countries` DISABLE KEYS */;
INSERT INTO `countries` VALUES (1,'Germany','262','49',0.055),(2,'Austria','232','43',0.053),(3,'Poland','260','48',0.032);
/*!40000 ALTER TABLE `countries` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `sms`
--

DROP TABLE IF EXISTS `sms`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `sms` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `from` varchar(50) NOT NULL,
  `to` varchar(50) NOT NULL,
  `mobile_country_code` varchar(10) DEFAULT NULL,
  `country_code` varchar(10) NOT NULL,
  `message_text` varchar(160) DEFAULT NULL,
  `is_delivered` tinyint(4) DEFAULT NULL,
  `sent_on` datetime DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=70 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `sms`
--

LOCK TABLES `sms` WRITE;
/*!40000 ALTER TABLE `sms` DISABLE KEYS */;
INSERT INTO `sms` VALUES (47,'The Sender','17421293388','262','49','Hello Germany1',0,'2016-08-18 11:36:22'),(48,'The Sender','17421293388','262','49','Hello Germany2',0,'2016-08-18 11:36:40'),(49,'The Sender','17421293388','262','49','Hello Germany3',0,'2016-08-18 11:36:43'),(50,'The Sender','17421293388','262','49','Hello Germany4',0,'2016-08-21 11:36:48'),(51,'The Sender','17421293388','262','49','Hello Germany5',0,'2016-08-21 11:36:52'),(52,'The Sender','17421293388','232','43','Hello Austria1',0,'2016-08-19 11:37:23'),(53,'The Sender','17421293388','232','43','Hello Austria2',0,'2016-08-19 11:37:26'),(54,'The Sender','17421293388','232','43','Hello Austria3',0,'2016-08-21 11:37:28'),(55,'The Sender','17421293388','232','43','Hello Austria4',0,'2016-08-22 11:37:29'),(56,'The Sender','17421293388','232','43','Hello Austria5',0,'2016-08-23 11:37:31'),(57,'The Sender','17421293388','260','48','Hello Poland1',0,'2016-08-18 11:37:59'),(58,'The Sender','17421293388','260','48','Hello Poland2',0,'2016-08-18 11:38:01'),(59,'The Sender','17421293388','260','48','Hello Poland3',0,'2016-08-20 11:38:04'),(60,'The Sender','17421293388','260','48','Hello Poland4',0,'2016-08-21 11:38:05'),(61,'The Sender','17421293388','260','48','Hello Poland5',0,'2016-08-22 11:38:08'),(62,'The Sender','17421293388','260','48','Hello Poland6',0,'2016-08-18 13:26:13'),(63,'The Sender','17421293388','260','48','Hello Poland7',0,'2016-08-18 13:30:56'),(64,'The Sender','17421293388','260','48','Hello Poland7',0,'2016-08-18 13:34:53'),(65,'The Sender','17421293388','260','48','Hello Poland7',0,'2016-08-18 13:36:17'),(66,'The Sender','17421293388','260','48','Hello Poland8',0,'2016-08-18 13:39:17'),(67,'The Sender','17421293388','260','48','Hello Poland9',0,'2016-08-18 13:39:25'),(68,'The Sender','17421293388','260','48','Hello Poland10',0,'2016-08-18 13:42:42'),(69,'The Sender','17421293388','260','48','Hello Poland11',0,'2016-08-18 13:42:48');
/*!40000 ALTER TABLE `sms` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Dumping routines for database 'mitto_sms_db'
--
/*!50003 DROP PROCEDURE IF EXISTS `GetSentSms` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8 */ ;
/*!50003 SET character_set_results = utf8 */ ;
/*!50003 SET collation_connection  = utf8_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`localhost` PROCEDURE `GetSentSms`(dateTimeFrom dateTime, dateTimeTo dateTime, skip int, take int)
BEGIN
	SELECT s.id, s.`from`, s.`to`, s.`message_text`, s.`is_delivered`, s.`sent_on`, c.cc, c.price_per_sms FROM mitto_sms_db.sms s
	left outer join countries c
	on s.country_code = c.cc
	where sent_on between dateTimeFrom AND dateTimeTo
	LIMIT take OFFSET skip;
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `GetSmsStatistics` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8 */ ;
/*!50003 SET character_set_results = utf8 */ ;
/*!50003 SET collation_connection  = utf8_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`localhost` PROCEDURE `GetSmsStatistics`(dateFrom datetime, dateTo datetime, mccList varchar(50))
BEGIN
	/*select date(sent_on) `day`, c.mcc, c.price_per_sms, count(s.id) `count`, mobile_country_code, sum(c.price_per_sms) `totalPrice` from sms s
	left outer join countries c
	on s.country_code = c.cc and s.mobile_country_code = c.mcc
	where date(sent_on) BETWEEN '2016-08-18' AND '2016-08-19' and  mobile_country_code IN ('262','260','232')
	group by date(sent_on), mobile_country_code;*/   
    
    
    set @sql = concat("select date(sent_on) `day`, c.mcc, c.price_per_sms, count(s.id) `count`, sum(c.price_per_sms) `totalPrice` from sms s
	left outer join countries c
	on s.country_code = c.cc and s.mobile_country_code = c.mcc
	where date(sent_on) BETWEEN '", dateFrom ,"' AND '", dateTo, "' and  mobile_country_code IN (", mccList, ")
	group by date(sent_on), mobile_country_code;");

    PREPARE stmt FROM @sql;
    EXECUTE stmt;
    
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `StoreSms` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8 */ ;
/*!50003 SET character_set_results = utf8 */ ;
/*!50003 SET collation_connection  = utf8_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`localhost` PROCEDURE `StoreSms`(_from varchar(50), _to varchar(50), _country_code varchar(10), _message_text varchar(160), _isDelivered boolean)
BEGIN
	
    
    SELECT @mobile_country_code:=mcc FROM countries where cc = _country_code;
    
	INSERT INTO `mitto_sms_db`.`sms`
	(`from`,
	`to`,
    `mobile_country_code`,
	`country_code`,
	`message_text`,
	`is_delivered`,
    `sent_on`)
	VALUES
	(
    _from,
    _to,
    @mobile_country_code,
    _country_code,
    _message_text,
    _isDelivered,
    now());

END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2016-08-18 13:45:35

CREATE DATABASE  IF NOT EXISTS `student_achievements` /*!40100 DEFAULT CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci */ /*!80016 DEFAULT ENCRYPTION='N' */;
USE `student_achievements`;
-- MySQL dump 10.13  Distrib 8.0.30, for Win64 (x86_64)
--
-- Host: 127.0.0.1    Database: student_achievements
-- ------------------------------------------------------
-- Server version	8.0.30

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!50503 SET NAMES utf8 */;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;

--
-- Table structure for table `course`
--

DROP TABLE IF EXISTS `course`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `course` (
  `id_course` int NOT NULL AUTO_INCREMENT,
  `course_group_name` int DEFAULT NULL,
  `course_score` int DEFAULT NULL,
  `cource_years_of_study` varchar(26) DEFAULT NULL,
  PRIMARY KEY (`id_course`),
  KEY `idx_group_course_idx` (`course_group_name`),
  CONSTRAINT `idx_group_course` FOREIGN KEY (`course_group_name`) REFERENCES `group` (`id_group`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=47 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `course`
--

LOCK TABLES `course` WRITE;
/*!40000 ALTER TABLE `course` DISABLE KEYS */;
INSERT INTO `course` VALUES (1,3,4,'2023-2023'),(41,1,3,'2021-2023'),(42,1,3,'2021-2023'),(43,1,3,'2041-2023'),(44,1,3,'2021-2023'),(45,1,3,'2051-2023'),(46,1,3,'2022-2023');
/*!40000 ALTER TABLE `course` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `employer`
--

DROP TABLE IF EXISTS `employer`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `employer` (
  `id_employer` int NOT NULL AUTO_INCREMENT,
  `employer_FIO` varchar(100) NOT NULL,
  `employer_position` varchar(100) NOT NULL,
  `employer_phone` varchar(20) NOT NULL,
  PRIMARY KEY (`id_employer`)
) ENGINE=InnoDB AUTO_INCREMENT=8 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `employer`
--

LOCK TABLES `employer` WRITE;
/*!40000 ALTER TABLE `employer` DISABLE KEYS */;
/*!40000 ALTER TABLE `employer` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `event`
--

DROP TABLE IF EXISTS `event`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `event` (
  `id_event` int NOT NULL AUTO_INCREMENT,
  `event_name` varchar(255) DEFAULT NULL,
  `event_order_date` date DEFAULT NULL,
  `event_order_number` int DEFAULT NULL,
  `event_document` blob,
  `event_certificate` blob,
  PRIMARY KEY (`id_event`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `event`
--

LOCK TABLES `event` WRITE;
/*!40000 ALTER TABLE `event` DISABLE KEYS */;
/*!40000 ALTER TABLE `event` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `group`
--

DROP TABLE IF EXISTS `group`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `group` (
  `id_group` int NOT NULL AUTO_INCREMENT,
  `group_code` varchar(6) DEFAULT NULL,
  `group_specialization` int DEFAULT NULL,
  PRIMARY KEY (`id_group`),
  KEY `idx_group_name_idx` (`group_specialization`),
  KEY `idx_group_student_idx` (`group_code`),
  CONSTRAINT `idx_group_name` FOREIGN KEY (`group_specialization`) REFERENCES `specialization` (`id_specialization`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=20 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `group`
--

LOCK TABLES `group` WRITE;
/*!40000 ALTER TABLE `group` DISABLE KEYS */;
INSERT INTO `group` VALUES (1,'ПС-19Б',12),(3,'БУ-18',15),(4,'АТ-19',11),(12,'АТ-19',11),(18,'ААВАВЫ',13),(19,'ПС-19Б',11);
/*!40000 ALTER TABLE `group` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `level_event`
--

DROP TABLE IF EXISTS `level_event`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `level_event` (
  `id_level_event` int NOT NULL AUTO_INCREMENT,
  `level_event_name` varchar(35) NOT NULL,
  PRIMARY KEY (`id_level_event`)
) ENGINE=InnoDB AUTO_INCREMENT=28 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `level_event`
--

LOCK TABLES `level_event` WRITE;
/*!40000 ALTER TABLE `level_event` DISABLE KEYS */;
INSERT INTO `level_event` VALUES (22,'Никитка'),(24,'Сухарьу'),(26,'Никитка'),(27,'Сухарь');
/*!40000 ALTER TABLE `level_event` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `list_result`
--

DROP TABLE IF EXISTS `list_result`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `list_result` (
  `id_list_result` int NOT NULL AUTO_INCREMENT,
  `list_result_code` varchar(4) NOT NULL,
  `list_result_specialty` varchar(75) NOT NULL,
  `list_result_description` varchar(250) NOT NULL,
  PRIMARY KEY (`id_list_result`)
) ENGINE=InnoDB AUTO_INCREMENT=14 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `list_result`
--

LOCK TABLES `list_result` WRITE;
/*!40000 ALTER TABLE `list_result` DISABLE KEYS */;
INSERT INTO `list_result` VALUES (13,'22','авываыаыв','ва');
/*!40000 ALTER TABLE `list_result` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `prize_place`
--

DROP TABLE IF EXISTS `prize_place`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `prize_place` (
  `id_prize_place` int NOT NULL AUTO_INCREMENT,
  `prize_place_name` varchar(20) NOT NULL,
  PRIMARY KEY (`id_prize_place`)
) ENGINE=InnoDB AUTO_INCREMENT=17 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `prize_place`
--

LOCK TABLES `prize_place` WRITE;
/*!40000 ALTER TABLE `prize_place` DISABLE KEYS */;
INSERT INTO `prize_place` VALUES (1,'1'),(2,'2'),(4,'3'),(8,'Участие');
/*!40000 ALTER TABLE `prize_place` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `specialization`
--

DROP TABLE IF EXISTS `specialization`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `specialization` (
  `id_specialization` int NOT NULL AUTO_INCREMENT,
  `specialization_name` varchar(6) DEFAULT NULL,
  `specialization_abbreviation` varchar(200) DEFAULT NULL,
  PRIMARY KEY (`id_specialization`),
  KEY `idx_specialization_idx` (`specialization_name`)
) ENGINE=InnoDB AUTO_INCREMENT=17 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `specialization`
--

LOCK TABLES `specialization` WRITE;
/*!40000 ALTER TABLE `specialization` DISABLE KEYS */;
INSERT INTO `specialization` VALUES (11,'ПС','Программирование в компьютерных системах'),(12,'АТ','Автомобиле и тракторостроениеа'),(13,'ТМ','Техническая эксплуатация и обслуживание электрического и электромеханического оборудования (по отраслям)'),(14,'ИС','Информационные системы и программирование'),(15,'БУ','Экономика и бухгалтерский учет по отраслям  '),(16,'БД','Банковское дело');
/*!40000 ALTER TABLE `specialization` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `student`
--

DROP TABLE IF EXISTS `student`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `student` (
  `id_student` int NOT NULL AUTO_INCREMENT,
  `student_fio` varchar(100) DEFAULT NULL,
  `student_status` int DEFAULT NULL,
  `student_group_code` int DEFAULT NULL,
  PRIMARY KEY (`id_student`),
  KEY `idx_student_group_code_idx` (`student_group_code`),
  KEY `idx_student_status_idx` (`student_status`),
  CONSTRAINT `idx_student_group_code` FOREIGN KEY (`student_group_code`) REFERENCES `group` (`id_group`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `idx_student_status` FOREIGN KEY (`student_status`) REFERENCES `student_status` (`id_student_status`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=48 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `student`
--

LOCK TABLES `student` WRITE;
/*!40000 ALTER TABLE `student` DISABLE KEYS */;
INSERT INTO `student` VALUES (42,'Иванов',1,4),(44,'Иванов',1,4),(45,'Иванов Илья Александрович',1,4),(46,'Ивввв',1,18),(47,'Иванов',1,4);
/*!40000 ALTER TABLE `student` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `student_cart`
--

DROP TABLE IF EXISTS `student_cart`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `student_cart` (
  `FIO` varchar(100) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `student_cart`
--

LOCK TABLES `student_cart` WRITE;
/*!40000 ALTER TABLE `student_cart` DISABLE KEYS */;
/*!40000 ALTER TABLE `student_cart` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `student_status`
--

DROP TABLE IF EXISTS `student_status`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `student_status` (
  `id_student_status` int NOT NULL AUTO_INCREMENT,
  `student_status_name` varchar(45) DEFAULT NULL,
  PRIMARY KEY (`id_student_status`)
) ENGINE=InnoDB AUTO_INCREMENT=4 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `student_status`
--

LOCK TABLES `student_status` WRITE;
/*!40000 ALTER TABLE `student_status` DISABLE KEYS */;
INSERT INTO `student_status` VALUES (1,'Обучается'),(2,'Отчислен'),(3,'Каникулы');
/*!40000 ALTER TABLE `student_status` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `user`
--

DROP TABLE IF EXISTS `user`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `user` (
  `id_user` int NOT NULL AUTO_INCREMENT,
  `user_login` varchar(100) DEFAULT NULL,
  `user_password` varchar(100) DEFAULT NULL,
  `user_role` int DEFAULT NULL,
  PRIMARY KEY (`id_user`),
  KEY `idx_user_role_idx` (`user_role`),
  CONSTRAINT `idx_user_role` FOREIGN KEY (`user_role`) REFERENCES `user_role` (`id_user_role`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=4 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `user`
--

LOCK TABLES `user` WRITE;
/*!40000 ALTER TABLE `user` DISABLE KEYS */;
INSERT INTO `user` VALUES (1,'teacher','teacher',1),(2,'student','student',2),(3,'admin','admin',3);
/*!40000 ALTER TABLE `user` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `user_role`
--

DROP TABLE IF EXISTS `user_role`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `user_role` (
  `id_user_role` int NOT NULL AUTO_INCREMENT,
  `role_user_name` varchar(45) DEFAULT NULL,
  PRIMARY KEY (`id_user_role`)
) ENGINE=InnoDB AUTO_INCREMENT=4 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `user_role`
--

LOCK TABLES `user_role` WRITE;
/*!40000 ALTER TABLE `user_role` DISABLE KEYS */;
INSERT INTO `user_role` VALUES (1,'Классный руководитель'),(2,'Староста'),(3,'Администратор');
/*!40000 ALTER TABLE `user_role` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Dumping events for database 'student_achievements'
--

--
-- Dumping routines for database 'student_achievements'
--
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2023-05-25 13:28:40

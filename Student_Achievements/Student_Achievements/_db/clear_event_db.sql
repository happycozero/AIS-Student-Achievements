CREATE DATABASE  IF NOT EXISTS `student_achievements` /*!40100 DEFAULT CHARACTER SET utf8mb3 */ /*!80016 DEFAULT ENCRYPTION='N' */;
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
-- Table structure for table `cource`
--

DROP TABLE IF EXISTS `cource`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `cource` (
  `id_cource` int NOT NULL AUTO_INCREMENT,
  `courсe_group_name` int DEFAULT NULL,
  `courсe_score` int DEFAULT NULL,
  `cource_years_of_study` varchar(26) DEFAULT NULL,
  PRIMARY KEY (`id_cource`),
  KEY `idx_group_course_idx` (`courсe_group_name`),
  CONSTRAINT `idx_group_course` FOREIGN KEY (`courсe_group_name`) REFERENCES `group` (`id_group`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=63 DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `cource`
--

LOCK TABLES `cource` WRITE;
/*!40000 ALTER TABLE `cource` DISABLE KEYS */;
INSERT INTO `cource` VALUES (58,25,1,'2020-2021'),(59,25,2,'2021-2022'),(60,25,3,'2022-2023'),(61,25,4,'2023-2024');
/*!40000 ALTER TABLE `cource` ENABLE KEYS */;
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
) ENGINE=InnoDB AUTO_INCREMENT=11 DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `employer`
--

LOCK TABLES `employer` WRITE;
/*!40000 ALTER TABLE `employer` DISABLE KEYS */;
INSERT INTO `employer` VALUES (8,'Краснов Максиывич','Преподаватель и','8(   )   -  -'),(9,'Волков Иван Ильич','Преподаватель физики','8(234)323-33-33'),(10,'Степанова Кристина Фёдоровна','Администратор','8(   )   -  -');
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
  `event_group` int DEFAULT NULL,
  `event_cource_score` int DEFAULT NULL,
  `event_years_study` varchar(45) DEFAULT NULL,
  `event_name` varchar(255) DEFAULT NULL,
  `event_code_lr` int DEFAULT NULL,
  `event_level_event` int DEFAULT NULL,
  `event_prize_place` int DEFAULT NULL,
  `event_order_number` varchar(45) DEFAULT NULL,
  `event_order_date` date DEFAULT NULL,
  `event_document` varchar(255) DEFAULT NULL,
  `event_certificate` varchar(255) DEFAULT NULL,
  `event_students` int DEFAULT NULL,
  PRIMARY KEY (`id_event`),
  KEY `idx_group_event_idx` (`event_group`),
  KEY `idx_lr_event_idx` (`event_code_lr`),
  KEY `idx_level_event_idx` (`event_level_event`),
  KEY `idx_prize_place_idx` (`event_prize_place`),
  KEY `idx_student_event_idx` (`event_students`),
  CONSTRAINT `idx_group_event` FOREIGN KEY (`event_group`) REFERENCES `group` (`id_group`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `idx_level_event` FOREIGN KEY (`event_level_event`) REFERENCES `level_event` (`id_level_event`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `idx_lr_event` FOREIGN KEY (`event_code_lr`) REFERENCES `list_result` (`id_list_result`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `idx_prize_place` FOREIGN KEY (`event_prize_place`) REFERENCES `prize_place` (`id_prize_place`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `idx_student_event` FOREIGN KEY (`event_students`) REFERENCES `student` (`id_student`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=166 DEFAULT CHARSET=utf8mb3;
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
) ENGINE=InnoDB AUTO_INCREMENT=32 DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `group`
--

LOCK TABLES `group` WRITE;
/*!40000 ALTER TABLE `group` DISABLE KEYS */;
INSERT INTO `group` VALUES (25,'ПС-20А',11),(26,'ПС-20Б',11);
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
) ENGINE=InnoDB AUTO_INCREMENT=30 DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `level_event`
--

LOCK TABLES `level_event` WRITE;
/*!40000 ALTER TABLE `level_event` DISABLE KEYS */;
INSERT INTO `level_event` VALUES (22,'Муниципальный'),(24,'Региональный'),(26,'Федеральные'),(27,'Внутри техникума');
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
  `list_result_specialty` int NOT NULL,
  `list_result_description` varchar(250) NOT NULL,
  PRIMARY KEY (`id_list_result`),
  KEY `idx_list_spec_idx` (`list_result_specialty`),
  CONSTRAINT `idx_list_spec` FOREIGN KEY (`list_result_specialty`) REFERENCES `specialization` (`id_specialization`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=44 DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `list_result`
--

LOCK TABLES `list_result` WRITE;
/*!40000 ALTER TABLE `list_result` DISABLE KEYS */;
INSERT INTO `list_result` VALUES (22,'ЛР01',11,'Осознающий себя гражданином и защитником великой страны'),(23,'ЛР02',11,'Проявляющий активную гражданску. позицию, демонстрирующий приверженность принципам честности, порядочности, открытости'),(24,'ЛР03',11,'Соблюдающий нормы правопорядка, следующий идеалам гражданского общества'),(25,'ЛР04',11,'Проявляющий и демонстрирующий уважение к людям труда'),(26,'ЛР05',11,'Демонстрирующий приверженность к родной культуре'),(27,'ЛР06',11,'Проявляющий уважение к людям старшего поколения и готовность к участию в социальной поддержке и волонтерских движениях'),(28,'ЛР07',11,'Осознающий приоритетную ценность личности человека; уважающий собственную и чужую уникальность в различных ситуациях, во всех формах и видах деятельности.'),(29,'ЛР08',11,'Проявляющий и демонстрирующий уважение к представителям различных этнокультурных, социальных, конфессиональных и иных групп. '),(30,'ЛР09',11,'Соблюдающий и пропагандирующий правила здорового и безопасного образа жизни, спорта; предупреждающий либо преодолевающий зависимости от алкоголя, табака, психоактивных веществ, азартных игр и т.д'),(31,'ЛР10',11,'Заботящийся о защите окружающей среды, собственной и чужой безопасности, в том числе цифровой'),(32,'ЛР11',11,'Проявляющий уважение к эстетическим ценностям, обладающий основами эстетической культуры'),(33,'ЛР12',11,'Принимающий семейные ценности, готовый к созданию семьи и воспитанию детей; демонстрирующий неприятие насилия в семье, ухода от родительской ответственности, отказа от отношений со своими детьми и их финансового содержания'),(34,'ЛР13',11,'Готовность обучающегося соответствовать ожиданиям работодателей: ответственный сотрудник, дисциплинированный, трудолюбивый, нацеленный на достижение поставленных задач, эффективно взаимодействующий с членами команды, сотрудничающий с другими'),(35,'ЛР14',11,'Демонстрирующий умение эффективно взаимодействовать в команде,'),(36,'ЛР15',11,'Демонстрирующий навыки анализа и интерпретации информации из'),(37,'ЛР16',11,'Демонстрирующий готовность и способность к образованию, в том числе самообразованию, на протяжении всей жизни; сознательное отношение к непрерывному образованию как условию успешной профессиональной и'),(38,'ЛР17',11,'Ценностное отношение обучающихся к своему здоровью и здоровью окружающих, ЗОЖ и здоровой окружающей среде и т.д.'),(39,'ЛР18',11,'Нацеленный на повышение престижа IT специальностей'),(40,'ЛР19',11,'Способный в цифровой среде проводить оценку информации, ее'),(41,'ЛР20',11,'Мотивированный к освоению функционально близких видов профессиональной деятельности, имеющих общие объекты (условия,'),(42,'ЛР21',11,'Готовый к профессиональной конкуренции и конструктивной реакции на критику. Экономически активный, предприимчивый, готовый к'),(43,'ЛР22',11,'Соблюдающий трудовую этику и культуру, придерживающийся');
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
  `prize_place_name` varchar(20) DEFAULT NULL,
  PRIMARY KEY (`id_prize_place`)
) ENGINE=InnoDB AUTO_INCREMENT=19 DEFAULT CHARSET=utf8mb3;
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
  `specialization_abbreviation` varchar(300) DEFAULT NULL,
  PRIMARY KEY (`id_specialization`),
  KEY `idx_specialization_idx` (`specialization_name`)
) ENGINE=InnoDB AUTO_INCREMENT=27 DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `specialization`
--

LOCK TABLES `specialization` WRITE;
/*!40000 ALTER TABLE `specialization` DISABLE KEYS */;
INSERT INTO `specialization` VALUES (11,'ПС','Программирование в компьютерных системах'),(12,'АТ','Автомобиле и тракторостроение'),(13,'ТМ','Техническая эксплуатация и обслуживание электрического и электромеханического оборудования по отраслям'),(14,'ИС ','Информационные системы и программирование'),(15,'БУ  ','Экономика и бухгалтерский учет по отраслям'),(16,'БД','Банковское дело'),(17,'ПС','Программирование в компьютерных системах'),(18,'ЭО','Техническая эксплуатация и обслуживание электрического и электромеханического оборудования  (по отраслям)'),(19,'РА','Техническое обслуживание и ремонт автомобильного транспорта'),(20,'КБ','Контролер сберегательного банка, кассир'),(21,'ЭЛ','Электромонтер по обслуживанию и ремонту электрооборудования (по отраслям)'),(22,'СА','Слесарь по ремонту строительных машин'),(23,'НА','Наладчик станков и оборудования в механообработке');
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
) ENGINE=InnoDB AUTO_INCREMENT=77 DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `student`
--

LOCK TABLES `student` WRITE;
/*!40000 ALTER TABLE `student` DISABLE KEYS */;
INSERT INTO `student` VALUES (51,'Абрамов Иван Максимович',1,25),(52,'Абрамова Виктория Сергеевна',1,25),(53,'Антонов Николай Константинович',1,25),(54,'Вахрушин Евгений Игоревич',1,25),(55,'Горячев Леонид Алексеевич',1,25),(56,'Давыдова Кристина Денисовна',1,25),(57,'Егоров Даниил Дмитриевич',1,25),(58,'Косолапова Юлия Дмитриевна',1,25),(59,'Кудрин Роман Алексеевич',1,25),(60,'Кузнецов Никита Александрович',1,25),(61,'Лебедев Артем Сергеевич',1,25),(62,'Огнев Даниил Дмитриевич',1,25),(63,'Перистов Дмитрий Сергеевич',1,25),(64,'Ремизов Фёдор Геннадьевич',1,25),(65,'Рыжов Александр Игоревич',1,25),(66,'Рябкова Вероника Николаевна',1,25),(67,'Самсонов Алексей Николаевич',1,25),(68,'Соломонова Анна Алексеевна',1,25),(69,'Спиряков Максим Алексеевич',1,25),(70,'Спицин Сергей Алексеевич',1,25),(71,'Торопов Алексей Максимович',1,25),(72,'Трубников Андрей Алексеевич',1,25),(73,'Филиппов Илья Алексеевич',1,25),(74,'Черняков Данила Александрович',1,25),(75,'Шохирев Дмитрий Николаевич',1,25),(76,'Абрамова Виктория Сергеевна',1,25);
/*!40000 ALTER TABLE `student` ENABLE KEYS */;
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
) ENGINE=InnoDB AUTO_INCREMENT=4 DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `student_status`
--

LOCK TABLES `student_status` WRITE;
/*!40000 ALTER TABLE `student_status` DISABLE KEYS */;
INSERT INTO `student_status` VALUES (1,'Обучается'),(2,'Отчислен');
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
  `user_employer` int DEFAULT NULL,
  `user_login` varchar(50) DEFAULT NULL,
  `user_password` text,
  `user_role` int DEFAULT NULL,
  PRIMARY KEY (`id_user`),
  KEY `idx_user_role_idx` (`user_role`),
  KEY `idx_employer_fio_idx` (`user_employer`),
  CONSTRAINT `idx_employer_fio` FOREIGN KEY (`user_employer`) REFERENCES `employer` (`id_employer`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `idx_user_role` FOREIGN KEY (`user_role`) REFERENCES `user_role` (`id_user_role`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=28 DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `user`
--

LOCK TABLES `user` WRITE;
/*!40000 ALTER TABLE `user` DISABLE KEYS */;
INSERT INTO `user` VALUES (25,10,'teacher','1057a9604e04b274da5a4de0c8f4b4868d9b230989f8c8c6a28221143cc5a755',1),(26,8,'admin','8c6976e5b5410415bde908bd4dee15dfb167a9c873fc4bb8a81f6f2ab448a918',3),(27,9,'student','264c8c381bf16c982a4e59b0dd4c6f7808c51a05f64c35db42cc78a2a72875bb',2);
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
) ENGINE=InnoDB AUTO_INCREMENT=4 DEFAULT CHARSET=utf8mb3;
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

-- Dump completed on 2023-06-25 19:57:25

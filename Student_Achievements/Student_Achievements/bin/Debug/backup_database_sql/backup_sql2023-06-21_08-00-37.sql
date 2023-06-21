-- MySqlBackup.NET 2.3.6
-- Dump Time: 2023-06-21 08:00:37
-- --------------------------------------
-- Server version 8.0.30 MySQL Community Server - GPL


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb3 */;
/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;


-- 
-- Definition of employer
-- 

DROP TABLE IF EXISTS `employer`;
CREATE TABLE IF NOT EXISTS `employer` (
  `id_employer` int NOT NULL AUTO_INCREMENT,
  `employer_FIO` varchar(100) NOT NULL,
  `employer_position` varchar(100) NOT NULL,
  `employer_phone` varchar(20) NOT NULL,
  PRIMARY KEY (`id_employer`)
) ENGINE=InnoDB AUTO_INCREMENT=11 DEFAULT CHARSET=utf8mb3;

-- 
-- Dumping data for table employer
-- 

/*!40000 ALTER TABLE `employer` DISABLE KEYS */;
INSERT INTO `employer`(`id_employer`,`employer_FIO`,`employer_position`,`employer_phone`) VALUES(8,'Краснов Максиывич','Преподаватель и','8(   )   -  -'),(9,'Волков Иван Ильич','Преподаватель физики','8(234)323-33-33'),(10,'Степанова Кристина Фёдоровна','Администратор','8(   )   -  -');
/*!40000 ALTER TABLE `employer` ENABLE KEYS */;

-- 
-- Definition of level_event
-- 

DROP TABLE IF EXISTS `level_event`;
CREATE TABLE IF NOT EXISTS `level_event` (
  `id_level_event` int NOT NULL AUTO_INCREMENT,
  `level_event_name` varchar(35) NOT NULL,
  PRIMARY KEY (`id_level_event`)
) ENGINE=InnoDB AUTO_INCREMENT=30 DEFAULT CHARSET=utf8mb3;

-- 
-- Dumping data for table level_event
-- 

/*!40000 ALTER TABLE `level_event` DISABLE KEYS */;
INSERT INTO `level_event`(`id_level_event`,`level_event_name`) VALUES(22,'Муниципальный'),(24,'Региональный'),(26,'Федеральные'),(27,'Внутри техникума');
/*!40000 ALTER TABLE `level_event` ENABLE KEYS */;

-- 
-- Definition of prize_place
-- 

DROP TABLE IF EXISTS `prize_place`;
CREATE TABLE IF NOT EXISTS `prize_place` (
  `id_prize_place` int NOT NULL AUTO_INCREMENT,
  `prize_place_name` varchar(20) DEFAULT NULL,
  PRIMARY KEY (`id_prize_place`)
) ENGINE=InnoDB AUTO_INCREMENT=19 DEFAULT CHARSET=utf8mb3;

-- 
-- Dumping data for table prize_place
-- 

/*!40000 ALTER TABLE `prize_place` DISABLE KEYS */;
INSERT INTO `prize_place`(`id_prize_place`,`prize_place_name`) VALUES(1,'1'),(2,'2'),(4,'3'),(8,'Участие');
/*!40000 ALTER TABLE `prize_place` ENABLE KEYS */;

-- 
-- Definition of specialization
-- 

DROP TABLE IF EXISTS `specialization`;
CREATE TABLE IF NOT EXISTS `specialization` (
  `id_specialization` int NOT NULL AUTO_INCREMENT,
  `specialization_name` varchar(6) DEFAULT NULL,
  `specialization_abbreviation` varchar(200) DEFAULT NULL,
  PRIMARY KEY (`id_specialization`),
  KEY `idx_specialization_idx` (`specialization_name`)
) ENGINE=InnoDB AUTO_INCREMENT=27 DEFAULT CHARSET=utf8mb3;

-- 
-- Dumping data for table specialization
-- 

/*!40000 ALTER TABLE `specialization` DISABLE KEYS */;
INSERT INTO `specialization`(`id_specialization`,`specialization_name`,`specialization_abbreviation`) VALUES(11,'ПС','Программирование в компьютерных системах'),(12,'АТ','Автомобиле и тракторостроение'),(13,'ТМ','Техническая эксплуатация и обслуживание электрического и электромеханического оборудования по отраслям'),(14,'ИС ','Информационные системы и программирование'),(15,'БУ  ','Экономика и бухгалтерский учет по отраслям'),(16,'БД','Банковское дело'),(17,'ПС','Программирование в компьютерных системах'),(18,'ЭО','Техническая эксплуатация и обслуживание электрического и электромеханического оборудования  (по отраслям)'),(19,'РА','Техническое обслуживание и ремонт автомобильного транспорта'),(20,'КБ','Контролер сберегательного банка, кассир'),(21,'ЭЛ','Электромонтер по обслуживанию и ремонту электрооборудования (по отраслям)'),(22,'СА','Слесарь по ремонту строительных машин'),(23,'НА','Наладчик станков и оборудования в механообработке');
/*!40000 ALTER TABLE `specialization` ENABLE KEYS */;

-- 
-- Definition of group
-- 

DROP TABLE IF EXISTS `group`;
CREATE TABLE IF NOT EXISTS `group` (
  `id_group` int NOT NULL AUTO_INCREMENT,
  `group_code` varchar(6) DEFAULT NULL,
  `group_specialization` int DEFAULT NULL,
  PRIMARY KEY (`id_group`),
  KEY `idx_group_name_idx` (`group_specialization`),
  KEY `idx_group_student_idx` (`group_code`),
  CONSTRAINT `idx_group_name` FOREIGN KEY (`group_specialization`) REFERENCES `specialization` (`id_specialization`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=32 DEFAULT CHARSET=utf8mb3;

-- 
-- Dumping data for table group
-- 

/*!40000 ALTER TABLE `group` DISABLE KEYS */;
INSERT INTO `group`(`id_group`,`group_code`,`group_specialization`) VALUES(25,'ПС-20А',11),(26,'ПС-20Б',11);
/*!40000 ALTER TABLE `group` ENABLE KEYS */;

-- 
-- Definition of cource
-- 

DROP TABLE IF EXISTS `cource`;
CREATE TABLE IF NOT EXISTS `cource` (
  `id_cource` int NOT NULL AUTO_INCREMENT,
  `courсe_group_name` int DEFAULT NULL,
  `courсe_score` int DEFAULT NULL,
  `cource_years_of_study` varchar(26) DEFAULT NULL,
  PRIMARY KEY (`id_cource`),
  KEY `idx_group_course_idx` (`courсe_group_name`),
  CONSTRAINT `idx_group_course` FOREIGN KEY (`courсe_group_name`) REFERENCES `group` (`id_group`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=63 DEFAULT CHARSET=utf8mb3;

-- 
-- Dumping data for table cource
-- 

/*!40000 ALTER TABLE `cource` DISABLE KEYS */;
INSERT INTO `cource`(`id_cource`,`courсe_group_name`,`courсe_score`,`cource_years_of_study`) VALUES(58,25,1,'2020-2021'),(59,25,2,'2021-2022'),(60,25,3,'2022-2023'),(61,25,4,'2023-2024');
/*!40000 ALTER TABLE `cource` ENABLE KEYS */;

-- 
-- Definition of list_result
-- 

DROP TABLE IF EXISTS `list_result`;
CREATE TABLE IF NOT EXISTS `list_result` (
  `id_list_result` int NOT NULL AUTO_INCREMENT,
  `list_result_code` varchar(4) NOT NULL,
  `list_result_specialty` int NOT NULL,
  `list_result_description` varchar(250) NOT NULL,
  PRIMARY KEY (`id_list_result`),
  KEY `idx_list_spec_idx` (`list_result_specialty`),
  CONSTRAINT `idx_list_spec` FOREIGN KEY (`list_result_specialty`) REFERENCES `specialization` (`id_specialization`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=44 DEFAULT CHARSET=utf8mb3;

-- 
-- Dumping data for table list_result
-- 

/*!40000 ALTER TABLE `list_result` DISABLE KEYS */;
INSERT INTO `list_result`(`id_list_result`,`list_result_code`,`list_result_specialty`,`list_result_description`) VALUES(22,'ЛР01',11,'Осознающий себя гражданином и защитником великой страны'),(23,'ЛР02',11,'Проявляющий активную гражданску. позицию, демонстрирующий приверженность принципам честности, порядочности, открытости'),(24,'ЛР03',11,'Соблюдающий нормы правопорядка, следующий идеалам гражданского общества'),(25,'ЛР04',11,'Проявляющий и демонстрирующий уважение к людям труда'),(26,'ЛР05',11,'Демонстрирующий приверженность к родной культуре'),(27,'ЛР06',11,'Проявляющий уважение к людям старшего поколения и готовность к участию в социальной поддержке и волонтерских движениях'),(28,'ЛР07',11,'Осознающий приоритетную ценность личности человека; уважающий собственную и чужую уникальность в различных ситуациях, во всех формах и видах деятельности.'),(29,'ЛР08',11,'Проявляющий и демонстрирующий уважение к представителям различных этнокультурных, социальных, конфессиональных и иных групп. '),(30,'ЛР09',11,'Соблюдающий и пропагандирующий правила здорового и безопасного образа жизни, спорта; предупреждающий либо преодолевающий зависимости от алкоголя, табака, психоактивных веществ, азартных игр и т.д'),(31,'ЛР10',11,'Заботящийся о защите окружающей среды, собственной и чужой безопасности, в том числе цифровой'),(32,'ЛР11',11,'Проявляющий уважение к эстетическим ценностям, обладающий основами эстетической культуры'),(33,'ЛР12',11,'Принимающий семейные ценности, готовый к созданию семьи и воспитанию детей; демонстрирующий неприятие насилия в семье, ухода от родительской ответственности, отказа от отношений со своими детьми и их финансового содержания'),(34,'ЛР13',11,'Готовность обучающегося соответствовать ожиданиям работодателей: ответственный сотрудник, дисциплинированный, трудолюбивый, нацеленный на достижение поставленных задач, эффективно взаимодействующий с членами команды, сотрудничающий с другими'),(35,'ЛР14',11,'Демонстрирующий умение эффективно взаимодействовать в команде,'),(36,'ЛР15',11,'Демонстрирующий навыки анализа и интерпретации информации из'),(37,'ЛР16',11,'Демонстрирующий готовность и способность к образованию, в том числе самообразованию, на протяжении всей жизни; сознательное отношение к непрерывному образованию как условию успешной профессиональной и'),(38,'ЛР17',11,'Ценностное отношение обучающихся к своему здоровью и здоровью окружающих, ЗОЖ и здоровой окружающей среде и т.д.'),(39,'ЛР18',11,'Нацеленный на повышение престижа IT специальностей'),(40,'ЛР19',11,'Способный в цифровой среде проводить оценку информации, ее'),(41,'ЛР20',11,'Мотивированный к освоению функционально близких видов профессиональной деятельности, имеющих общие объекты (условия,'),(42,'ЛР21',11,'Готовый к профессиональной конкуренции и конструктивной реакции на критику. Экономически активный, предприимчивый, готовый к'),(43,'ЛР22',11,'Соблюдающий трудовую этику и культуру, придерживающийся');
/*!40000 ALTER TABLE `list_result` ENABLE KEYS */;

-- 
-- Definition of student_status
-- 

DROP TABLE IF EXISTS `student_status`;
CREATE TABLE IF NOT EXISTS `student_status` (
  `id_student_status` int NOT NULL AUTO_INCREMENT,
  `student_status_name` varchar(45) DEFAULT NULL,
  PRIMARY KEY (`id_student_status`)
) ENGINE=InnoDB AUTO_INCREMENT=4 DEFAULT CHARSET=utf8mb3;

-- 
-- Dumping data for table student_status
-- 

/*!40000 ALTER TABLE `student_status` DISABLE KEYS */;
INSERT INTO `student_status`(`id_student_status`,`student_status_name`) VALUES(1,'Обучается'),(2,'Отчислен');
/*!40000 ALTER TABLE `student_status` ENABLE KEYS */;

-- 
-- Definition of student
-- 

DROP TABLE IF EXISTS `student`;
CREATE TABLE IF NOT EXISTS `student` (
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

-- 
-- Dumping data for table student
-- 

/*!40000 ALTER TABLE `student` DISABLE KEYS */;
INSERT INTO `student`(`id_student`,`student_fio`,`student_status`,`student_group_code`) VALUES(51,'Абрамов Иван Максимович',1,25),(52,'Абрамова Виктория Сергеевна',1,25),(53,'Антонов Николай Константинович',1,25),(54,'Вахрушин Евгений Игоревич',1,25),(55,'Горячев Леонид Алексеевич',1,25),(56,'Давыдова Кристина Денисовна',1,25),(57,'Егоров Даниил Дмитриевич',1,25),(58,'Косолапова Юлия Дмитриевна',1,25),(59,'Кудрин Роман Алексеевич',1,25),(60,'Кузнецов Никита Александрович',1,25),(61,'Лебедев Артем Сергеевич',1,25),(62,'Огнев Даниил Дмитриевич',1,25),(63,'Перистов Дмитрий Сергеевич',1,25),(64,'Ремизов Фёдор Геннадьевич',1,25),(65,'Рыжов Александр Игоревич',1,25),(66,'Рябкова Вероника Николаевна',1,25),(67,'Самсонов Алексей Николаевич',1,25),(68,'Соломонова Анна Алексеевна',1,25),(69,'Спиряков Максим Алексеевич',1,25),(70,'Спицин Сергей Алексеевич',1,25),(71,'Торопов Алексей Максимович',1,25),(72,'Трубников Андрей Алексеевич',1,25),(73,'Филиппов Илья Алексеевич',1,25),(74,'Черняков Данила Александрович',1,25),(75,'Шохирев Дмитрий Николаевич',1,25),(76,'Абрамова Виктория Сергеевна',1,25);
/*!40000 ALTER TABLE `student` ENABLE KEYS */;

-- 
-- Definition of event
-- 

DROP TABLE IF EXISTS `event`;
CREATE TABLE IF NOT EXISTS `event` (
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

-- 
-- Dumping data for table event
-- 

/*!40000 ALTER TABLE `event` DISABLE KEYS */;
INSERT INTO `event`(`id_event`,`event_group`,`event_cource_score`,`event_years_study`,`event_name`,`event_code_lr`,`event_level_event`,`event_prize_place`,`event_order_number`,`event_order_date`,`event_document`,`event_certificate`,`event_students`) VALUES(67,25,2,'2021','викторина День народного единства',23,22,1,'','2017-06-20 00:00:00','','',60),(68,25,4,'2021','викторина День народного единства',22,24,2,'','2017-06-20 00:00:00','','',54),(69,25,4,'2021','викторина День народного единства',22,24,2,'','2017-06-20 00:00:00','','',54),(70,25,2,'2020','викторина День народного единства',22,24,1,'','2017-06-20 00:00:00','','',54),(71,25,1,'2020','викторина День народного единства',22,22,1,'','2017-06-20 00:00:00','','',56),(72,25,1,'2020','викторина \"История энергетики\"',26,22,1,'','2017-06-20 00:00:00','','',60),(73,25,1,'2020','Дежурство по техникуму',25,22,1,'','2017-06-20 00:00:00','','',60),(74,25,1,'2020','викторина \"История энергетики\"',26,22,1,'','2017-06-20 00:00:00','','',57),(75,25,2,'2020','викторина День народного единства',22,27,2,'','2017-06-20 00:00:00','','',51),(76,25,2,'2020','Соревнования по плаванию 1 курсы',23,27,2,'','2017-06-20 00:00:00','','',51),(77,25,2,'2020','Соревнования по плаванию 1 курсы',25,27,2,'','2017-06-20 00:00:00','','',51),(78,25,2,'2020','Соревнования по плаванию 1 курсы',26,27,2,'','2017-06-20 00:00:00','','',51),(79,25,2,'2020','ihiiiingbjbcvnbvjfd',24,27,2,'','2017-06-20 00:00:00','','',51),(80,25,1,'2020','h',25,24,1,'','2019-06-20 00:00:00','','',51),(86,25,1,'2020','hjjhgmjhg gbnbvn bcvbv',24,24,1,'','2017-06-20 00:00:00','','',52),(87,25,1,'2020','викторина День народного единства',23,24,1,'','2017-06-20 00:00:00','','',52),(89,25,1,'2020','hjjhgmjhg gbnbvn ',26,24,1,'','2019-06-20 00:00:00','','',52),(90,25,1,'2020','hjjhgmjhg ',25,24,1,'','2019-06-20 00:00:00','','',53),(91,25,1,'2020','hjjhgmjhg gbnbvn bcvbv',24,24,1,'','2017-06-20 00:00:00','','',53),(92,25,1,'2020','викторина День народного единства',23,24,1,'','2017-06-20 00:00:00','','',53),(94,25,1,'2020','hjjhgmjhg gbnbvn bcvbv',26,24,1,'','2017-06-20 00:00:00','','',53),(95,25,1,'2020','hjjhgmjhg gbnbvn bcvbv',25,24,1,'','2017-06-20 00:00:00','','',54),(96,25,1,'2020','hjjhgmjhg gbnbvn bcvbv',24,24,1,'','2017-06-20 00:00:00','','',54),(97,25,1,'2020','викторина День народного единства',23,24,1,'','2017-06-20 00:00:00','','',54),(99,25,1,'2020','hjjhgmjhg gbnbvn bcvbv',26,24,1,'','2017-06-20 00:00:00','','',54),(100,25,1,'2020','hjjhgmjhg gbnbvn bcvbv',25,24,1,'','2017-06-20 00:00:00','','',55),(101,25,1,'2020','hjjhgmjhg gbnbvn bcvbv',24,24,1,'','2017-06-20 00:00:00','','',55),(102,25,1,'2020','викторина День народного единства',23,24,1,'','2017-06-20 00:00:00','','',55),(104,25,1,'2020','hjjhgmjhg gbnbvn bcvbv',26,24,1,'','2017-06-20 00:00:00','','',55),(105,25,1,'2020','fdsfdssfd',26,22,1,'','2017-06-20 00:00:00','','',58),(106,25,1,'2020','fdsfdssfd',25,22,1,'','2017-06-20 00:00:00','','',58),(107,25,1,'2020','fdsfdssfd',24,22,1,'','2017-06-20 00:00:00','','',58),(108,25,1,'2020','викторина День народного единства',23,22,1,'','2017-06-20 00:00:00','','',58),(109,25,1,'2020','fdsfdssfd',22,22,1,'','2017-06-20 00:00:00','','',58),(110,25,1,'2020','fdsgfngbhbnvjhihjghfg',25,22,1,'','2017-06-20 00:00:00','','',51),(111,25,1,'2020','fdsgfngbhbnvjhihjghfg',26,22,1,'','2017-06-20 00:00:00','','',51),(112,25,1,'2020','викторина День народного единства',23,22,1,'','2017-06-20 00:00:00','','',51),(113,25,1,'2020','fdsgfngbhbnvjhihjghfg',24,22,1,'','2017-06-20 00:00:00','','',51),(114,25,1,'2020','fdsgfngbhbnvjhihjghfg',22,22,1,'','2017-06-20 00:00:00','','',51),(115,25,1,'2020','fdsgfngbhbnvjhihjghfg',25,22,1,'','2017-06-20 00:00:00','','',52),(116,25,1,'2020','fdsgfngbhbnvjhihjghfg',26,22,1,'','2017-06-20 00:00:00','','',52),(117,25,1,'2020','викторина День народного единства',23,22,1,'','2017-06-20 00:00:00','','',52),(118,25,1,'2020','fdsgfngbhbnvjhihjghfg',24,22,1,'','2017-06-20 00:00:00','','',52),(120,25,1,'2020','fdsgfngbhbnvjhihjghfg',25,22,1,'','2017-06-20 00:00:00','','',53),(121,25,1,'2020','fdsgfngbhbnvjhihjghfg',26,22,1,'','2017-06-20 00:00:00','','',53),(122,25,1,'2020','викторина День народного единства',23,22,1,'','2017-06-20 00:00:00','','',53),(123,25,1,'2020','fdsgfngbhbnvjhihjghfg',24,22,1,'','2017-06-20 00:00:00','','',53),(124,25,1,'2020','викторина День народного единства',22,22,1,'','2017-06-20 00:00:00','','',53),(125,25,1,'2020','fdsgfngbhbnvjhihjghfg',25,22,1,'','2017-06-20 00:00:00','','',54),(126,25,1,'2020','fdsgfngbhbnvjhihjghfg',26,22,1,'','2017-06-20 00:00:00','','',54),(127,25,1,'2020','викторина День народного единства',23,22,1,'','2017-06-20 00:00:00','','',54),(128,25,1,'2020','fdsgfngbhbnvjhihjghfg',24,22,1,'','2017-06-20 00:00:00','','',54),(129,25,1,'2020','fdsgfngbhbnvjhihjghfg',22,22,1,'','2017-06-20 00:00:00','','',54),(130,25,1,'2020','fdsgfngbhbnvjhihjghfg',25,22,1,'','2017-06-20 00:00:00','','',55),(131,25,1,'2020','fdsgfngbhbnvjhihjghfg',26,22,1,'','2017-06-20 00:00:00','','',55),(132,25,1,'2020','викторина День народного единства',23,22,1,'','2017-06-20 00:00:00','','',55),(133,25,1,'2020','fdsgfngbhbnvjhihjghfg',24,22,1,'','2017-06-20 00:00:00','','',55),(134,25,1,'2020','fdsgfngbhbnvjhihjghfg',22,22,1,'','2017-06-20 00:00:00','','',55),(135,25,1,'2020','reeruytytuytuytuyt',26,22,1,'','2017-06-20 00:00:00','','',51),(136,25,1,'2020','викторина День народного единства',23,22,1,'','2017-06-20 00:00:00','','',51),(137,25,1,'2020','reeruytytuytuytuyt',26,22,1,'','2017-06-20 00:00:00','','',52),(138,25,1,'2020','викторина День народного единства',23,22,1,'','2017-06-20 00:00:00','','',52),(139,25,1,'2020','yhhg',25,22,1,'','2017-06-20 00:00:00','','',53),(140,25,1,'2020','dffdhghjghgb',24,24,1,'','2017-06-20 00:00:00','','',55),(141,25,1,'2020','jknb kvb jbvjv',23,22,4,'','2017-06-20 00:00:00','','',54),(142,25,1,'2020','fbggffg',26,22,1,'','2017-06-20 00:00:00','','',53),(144,25,2,'2020','dfdsffd',25,22,1,'2020','2017-06-20 00:00:00','','',52),(145,25,2,'2020','dfdsffd',26,22,1,'2020','2017-06-20 00:00:00','','',52),(146,25,2,'2020','dfdsffd',25,22,1,'2020','2017-06-20 00:00:00','','',54),(147,25,2,'2020','dfdsffd',26,22,1,'2020','2017-06-20 00:00:00','','',54),(148,25,2,'2020','dfdsffd',25,22,1,'2020','2017-06-20 00:00:00','','',57),(149,25,2,'2020','dfdsffd',26,22,1,'2020','2017-06-20 00:00:00','','',57),(150,25,1,'2002','викторина День народного единства',23,22,1,'','2018-06-20 00:00:00','','',55),(151,25,1,'2002','викторина День народного единства',23,22,1,'','2018-06-20 00:00:00','','',58),(152,25,1,'','llllllllllllllllllllllllll',23,22,1,'','2018-06-20 00:00:00','','',55),(153,25,1,'','llllllllllllllllllllllllll',23,22,1,'','2018-06-20 00:00:00','','',58),(154,25,1,'','ав',31,24,1,'','2019-06-20 00:00:00','','',60),(155,25,1,'','ав',27,24,1,'','2019-06-20 00:00:00','','',60),(156,25,1,'','ав',35,24,1,'','2019-06-20 00:00:00','','',60),(157,25,1,'','ав',31,24,1,'','2019-06-20 00:00:00','','',59),(158,25,1,'','ав',27,24,1,'','2019-06-20 00:00:00','','',59),(159,25,1,'','ав',35,24,1,'','2019-06-20 00:00:00','','',59),(160,25,1,'','ав',31,24,1,'','2019-06-20 00:00:00','','',69),(161,25,1,'','ав',27,24,1,'','2019-06-20 00:00:00','','',69),(162,25,1,'','ав',35,24,1,'','2019-06-20 00:00:00','','',69),(163,25,1,'','ав',31,24,1,'','2019-06-20 00:00:00','','',56),(164,25,1,'','ав',27,24,1,'','2019-06-20 00:00:00','','',56),(165,25,1,'','ав',35,24,1,'','2019-06-20 00:00:00','','',56);
/*!40000 ALTER TABLE `event` ENABLE KEYS */;

-- 
-- Definition of user_role
-- 

DROP TABLE IF EXISTS `user_role`;
CREATE TABLE IF NOT EXISTS `user_role` (
  `id_user_role` int NOT NULL AUTO_INCREMENT,
  `role_user_name` varchar(45) DEFAULT NULL,
  PRIMARY KEY (`id_user_role`)
) ENGINE=InnoDB AUTO_INCREMENT=4 DEFAULT CHARSET=utf8mb3;

-- 
-- Dumping data for table user_role
-- 

/*!40000 ALTER TABLE `user_role` DISABLE KEYS */;
INSERT INTO `user_role`(`id_user_role`,`role_user_name`) VALUES(1,'Классный руководитель'),(2,'Староста'),(3,'Администратор');
/*!40000 ALTER TABLE `user_role` ENABLE KEYS */;

-- 
-- Definition of user
-- 

DROP TABLE IF EXISTS `user`;
CREATE TABLE IF NOT EXISTS `user` (
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

-- 
-- Dumping data for table user
-- 

/*!40000 ALTER TABLE `user` DISABLE KEYS */;
INSERT INTO `user`(`id_user`,`user_employer`,`user_login`,`user_password`,`user_role`) VALUES(25,10,'teacher','1057a9604e04b274da5a4de0c8f4b4868d9b230989f8c8c6a28221143cc5a755',1),(26,8,'admin','8c6976e5b5410415bde908bd4dee15dfb167a9c873fc4bb8a81f6f2ab448a918',3),(27,9,'student','264c8c381bf16c982a4e59b0dd4c6f7808c51a05f64c35db42cc78a2a72875bb',2);
/*!40000 ALTER TABLE `user` ENABLE KEYS */;


/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;


-- Dump completed on 2023-06-21 08:00:37
-- Total time: 0:0:0:0:134 (d:h:m:s:ms)

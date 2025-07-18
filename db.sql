CREATE DATABASE tes;
USE tes;

CREATE TABLE IF NOT EXISTS users (
  id INT AUTO_INCREMENT PRIMARY KEY,
  username VARCHAR(100) NOT NULL,
  email VARCHAR(255) NOT NULL UNIQUE,
  `password` VARCHAR(64) NOT NULL,
  created_at DATETIME DEFAULT CURRENT_TIMESTAMP
);

INSERT INTO users (username, email, `password`) VALUES
('faan', 'faan@gmail.com', 'admin123');
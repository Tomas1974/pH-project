DROP SCHEMA IF EXISTS ph CASCADE;
CREATE SCHEMA IF NOT EXISTS ph;
DROP TABLE IF EXISTS ph.client;
DROP TABLE IF EXISTS ph.company;
DROP TABLE IF EXISTS ph.users;
DROP TABLE IF EXISTS ph.status;
DROP TABLE IF EXISTS ph.data;
DROP TABLE IF EXISTS ph.city;
DROP TABLE IF EXISTS ph.client_user;


CREATE TABLE ph.users(
    user_id SERIAL PRIMARY KEY,
    name VARCHAR(100) NOT NULL,
    hash      VARCHAR(350) NOT NULL,
    salt      VARCHAR(180) NOT NULL,
    address VARCHAR(100) NOT NULL,
    zip_code INTEGER NOT NULL,
    cvr INTEGER
   
);

CREATE TABLE ph.client
(
    client_id varchar(20) PRIMARY KEY,  
    client_name varchar(50) NOT NULL,
    max_value DECIMAL(4,2),            
    min_value DECIMAL(4,2)             
);

INSERT INTO ph.client (client_id, client_name, max_value, min_value)
VALUES
    ('client/AAAA-AAAA', 'Chiklide akvariet', 8.00, 6.00),  
    ('client/BBBB-BBBB', 'Guppi akvariet', 8.00, 6.00),
    ('client/CCCC-CCCC', 'Skalarne', 8.00, 6.00),
    ('client/DDDD-DDDD', 'Guldfisk bowlen', 8.00, 6.00);


CREATE TABLE ph.status(
    status_id SERIAL PRIMARY KEY,
    log VARCHAR(200) NOT NULL,
    date DATE NOT NULL
);

CREATE TABLE ph.data(
    data_id SERIAL PRIMARY KEY,
    client_id VARCHAR(20) NOT NULL,
    data DECIMAL NOT NULL,
    date DATE NOT NULL,
    alarm VARCHAR(50),
    FOREIGN KEY (client_id) REFERENCES ph.client (client_iD)
);

CREATE TABLE ph.client_user(
    client_user_id SERIAL PRIMARY KEY,
    client_id VARCHAR(20) NOT NULL,
    user_id INTEGER NOT NULL,
    FOREIGN KEY (client_id) REFERENCES ph.client (client_iD),
    FOREIGN KEY (user_id) REFERENCES ph.users (user_id)
);
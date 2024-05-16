DROP SCHEMA IF EXISTS ph CASCADE;
CREATE SCHEMA IF NOT EXISTS ph;
DROP TABLE IF EXISTS ph.client;
DROP TABLE IF EXISTS ph.users;
DROP TABLE IF EXISTS ph.status;
DROP TABLE IF EXISTS ph.data;
DROP TABLE IF EXISTS ph.client_user;


CREATE TABLE ph.users(
    email VARCHAR(100) PRIMARY KEY,
    name VARCHAR(100) NOT NULL,
    hash      VARCHAR(350) NOT NULL,
    salt      VARCHAR(180) NOT NULL,
    address VARCHAR(100) NOT NULL,
    street_number VARCHAR(100) NOT NULL,
    zip_code INTEGER NOT NULL,
    cvr INTEGER
   
);

CREATE TABLE ph.client
(
    client_id varchar(20) PRIMARY KEY,  
    client_name varchar(50),
    max_value DECIMAL(4,2),            
    min_value DECIMAL(4,2)             
);

INSERT INTO ph.client (client_id)
VALUES
    ('client/AAAA-AAAA'),  
    ('client/BBBB-BBBB'),
    ('client/CCCC-CCCC'),
    ('client/DDDD-DDDD'),
    ('client/EEEE-EEEE'),
    ('client/FFFF-FFFF'),
    ('client/GGGG-GGGG'),
    ('client/HHHH-HHHH'),
    ('client/IIII-IIII'),
    ('client/JJJJ-JJJJ'),
    ('client/KKKK-KKKK'),
    ('client/LLLL-LLLL'),
    ('client/MMMM-MMMM'),
    ('client/NNNN-NNNN'),
    ('client/OOOO-OOOO'),
    ('client/PPPP-PPPP'),
    ('client/QQQQ-QQQQ'),
    ('client/RRRR-RRRR'),
    ('client/SSSS-SSSS'),
    ('client/TTTT-TTTT'),
    ('client/UUUU-UUUU'),
    ('client/VVVV-VVVV'),
    ('client/WWWW-WWWW'),
    ('client/XXXX-XXXX'),
    ('client/YYYY-YYYY'),
    ('client/ZZZZ-ZZZZ');
    

CREATE TABLE ph.status(
    status_id SERIAL PRIMARY KEY,
    log VARCHAR(200) NOT NULL,
    date DATE NOT NULL
);

CREATE TABLE ph.data(
    data_id SERIAL PRIMARY KEY,
    client_id VARCHAR(20) NOT NULL,
    data DECIMAL NOT NULL,
    time TIMESTAMP NOT NULL,
    alarm VARCHAR(50),
    FOREIGN KEY (client_id) REFERENCES ph.client (client_iD)
);

CREATE TABLE ph.client_user(
    client_user_id SERIAL PRIMARY KEY,
    client_id VARCHAR(20) NOT NULL,
    email VARCHAR(100) NOT NULL,
    FOREIGN KEY (client_id) REFERENCES ph.client (client_iD),
    FOREIGN KEY (email) REFERENCES ph.users (email)
);
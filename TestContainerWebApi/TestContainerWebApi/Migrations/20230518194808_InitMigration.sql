CREATE TABLE roleAuth(
    id INT IDENTITY (1, 1) NOT NULL,
    role VARCHAR(50) NOT NULL,
    PRIMARY KEY (id)
);

INSERT INTO roleAuth (role)
VALUES ('admin'),
    ('manager'),
    ('user');


CREATE TABLE users (
    id INT IDENTITY (1, 1) NOT NULL,
    email VARCHAR(MAX) NOT NULL,
    password VARCHAR(MAX) NOT NULL,
    roleId INT NOT NULL,
    created_at DATETIMEOFFSET NOT NULL,
    updated_at DATETIMEOFFSET NOT NULL,
    PRIMARY KEY (id),
    FOREIGN KEY (roleId) REFERENCES roleAuth(id)
);

INSERT INTO users (email, password, roleId, created_at, updated_at)
VALUES ('admin@gmail.com', '$HASH_PBKDF2v2$V1$Nhy/l9Qlq1T/eAtbM/wgIw==$B8bh+2+9SO0kku9thOPPL5dQwiPpSniPFN02scJOlfo=', 1, '2023-04-12 12:00:00', '2023-04-12 12:00:00'),
    ('manager@gmail.com', '$HASH_PBKDF2v2$V1$MLheSYoEbBjnn6kPbmjqVg==$3Q3vO8Fpx6ykVIeiWPD6iWOMG9MqgGSqTyKRbH/wa7Q=', 2, '2023-04-12 12:00:00', '2023-04-12 12:00:00'),
    ('user@gmail.com', '$HASH_PBKDF2v2$V1$pXDGAl2Bzi9vwjGob18HWw==$eCeUYGY7LOH+vyr9Ik4nWP94j1rKDmG3xe7G052/AlE=', 3, '2023-04-12 12:00:00', '2023-04-12 12:00:00');




-- In the future, authenticate using OAUTH 2.0
CREATE TABLE users_social_profiles (
    id INT IDENTITY (1, 1) NOT NULL,
    user_id INT NOT NULL,
    provider VARCHAR(20) CHECK (
        provider IN(
            'github',-----    -- developers       -- use      -- email = true
            'gitlab',-----    -- developers       -- use      -- email = true
            'bitbucket',--    -- developers       -- use      -- email = true
            'figma',------    -- designers        -- @todo
            'dribbble',---    -- designers        -- use      -- email = false
            'behance',----    -- designers        -- none provider
            'linkedin',---    -- business         -- use      -- email = true
            'xing',-------    -- business         -- @todo
            'google'------    -- global           -- use      -- email = true
         )) NOT NULL,
    social_id VARCHAR(50) NOT NULL,
    email VARCHAR(50) NOT NULL,
    username VARCHAR(50) NOT NULL,
    created_at DATETIMEOFFSET NOT NULL,
    updated_at DATETIMEOFFSET NOT NULL,
    PRIMARY KEY (id),
    FOREIGN KEY (user_id) REFERENCES users (id)
);

CREATE INDEX EMAIL_INDEX ON users_social_profiles (email);
CREATE UNIQUE INDEX PROVIDER_SOCIAL_UNIQUE ON users_social_profiles (provider, social_id);

INSERT INTO users_social_profiles (
    user_id,
    provider,
    social_id,
    email,
    username,
    created_at,
    updated_at
)
VALUES (1, 'google', 12345, 'test@email.com', 'Name Username', '2023-04-12 12:00:00', '2023-04-12 12:00:00');


CREATE TABLE urls (
    id INT IDENTITY (1, 1) NOT NULL,
    original_url TEXT NOT NULL,
    short_url VARCHAR(10) NOT NULL,
    secret_access_token uniqueidentifier NOT NULL,
    created_at DATETIMEOFFSET NOT NULL,
    updated_at DATETIMEOFFSET,
    deleted_at DATETIMEOFFSET,
    created_by INT,
    PRIMARY KEY (id),
    FOREIGN KEY (created_by) REFERENCES users(id) ON DELETE NO ACTION
);

INSERT INTO urls (
        original_url,
        short_url,
        secret_access_token,
        created_at,
        created_by
    )
VALUES ('https://fastapi.tiangolo.com/', '12345', 'a8098c1a-f86e-11da-bd1a-00112444be1e', '2023-04-12 12:00:00',  3),
    ('https://plotly.com/graphing-libraries/', '54321', 'a8098c1a-f86e-11da-bd1a-00112444be2e', '2023-04-12 12:00:00', 3),
    ('https://docs.pydantic.dev/', 'qwert', 'a8098c1a-f86e-11da-bd1a-00112444be3e', '2023-04-12 12:00:00', 3);

CREATE TABLE prohibited_domain (
    id INT IDENTITY (1, 1) NOT NULL,
    name TEXT NOT NULL,
    PRIMARY KEY (id)
);

INSERT INTO prohibited_domain (name)
VALUES ('www.google.com'),
    ('www.facebook.com'),
    ('tsn.ua');

CREATE TABLE hour_views (
    url_id INT NOT NULL,
    hour_time DATETIME NOT NULL,
    count INT NOT NULL DEFAULT 1,
    PRIMARY KEY (url_id, hour_time)
);

INSERT INTO hour_views (url_id, hour_time, count)
VALUES ('1', '2023-06-01 19:00:00', '1'),
    ('1', '2023-06-02 19:00:00', '1'),
    ('1', '2023-06-03 19:00:00', '2'),
    ('1', '2023-06-04 19:00:00', '2'),
    ('1', '2023-06-05 19:00:00', '1'),
    ('1', '2023-06-06 19:00:00', '1'),
    ('1', '2023-06-07 19:00:00', '2'),
    ('1', '2023-06-07 20:00:00', '1'),
    ('1', '2023-06-07 21:00:00', '1'),
    ('1', '2023-06-07 22:00:00', '1'),
    ('1', '2023-06-08 10:00:00', '1'),
    ('1', '2023-06-08 11:00:00', '1'),
    ('1', '2023-06-08 12:00:00', '1'),
    ('1', '2023-06-08 13:00:00', '1'),
    ('1', '2023-06-08 14:00:00', '1'),
    ('1', '2023-06-13 10:00:00', '1'),
    ('1', '2023-06-13 11:00:00', '1'),
    ('1', '2023-06-13 12:00:00', '1'),
    ('1', '2023-06-14 13:00:00', '1'),
    ('1', '2023-06-15 14:00:00', '1'),
    ('1', '2023-06-16 10:00:00', '1'),
    ('1', '2023-06-16 11:00:00', '1'),
    ('1', '2023-06-16 12:00:00', '1'),
    ('1', '2023-06-16 13:00:00', '1'),
    ('1', '2023-06-16 14:00:00', '1'),
    ('1', '2023-06-17 12:00:00', '1'),
    ('1', '2023-06-18 13:00:00', '1'),
    ('1', '2023-06-19 14:00:00', '1'),
    ('1', '2023-06-20 14:00:00', '2'),
    ('1', '2023-06-21 14:00:00', '1'),
    ('1', '2023-06-22 14:00:00', '1'),
    ('1', '2023-06-23 14:00:00', '1'),
    ('1', '2023-06-24 14:00:00', '1'),
    ('1', '2023-06-25 14:00:00', '1'),
    ('1', '2023-06-26 14:00:00', '1'),
    ('1', '2023-06-26 15:00:00', '1'),
    ('1', '2023-06-26 16:00:00', '1'),
    ('1', '2023-06-26 17:00:00', '1'),
    ('1', '2023-06-27 14:00:00', '1'),
    ('1', '2023-06-27 15:00:00', '1'),
    ('1', '2023-06-27 16:00:00', '1'),
    ('1', '2023-06-27 17:00:00', '1'),
    ('1', '2023-06-27 18:00:00', '1'),
    ('1', '2023-06-27 19:00:00', '1'),
    ('1', '2023-06-27 20:00:00', '1'),
    ('1', '2023-06-27 21:00:00', '5'),
    ('1', '2023-06-27 22:00:00', '4'),
    ('1', '2023-06-28 15:00:00', '1'),
    ('1', '2023-06-28 16:00:00', '1'),
    ('1', '2023-06-29 17:00:00', '1'),
    ('1', '2023-06-29 18:00:00', '1'),
    ('1', '2023-06-29 19:00:00', '1'),
    ('1', '2023-06-30 00:00:00', '1'),
    ('1', '2023-06-30 01:00:00', '4'),
    ('2', '2023-06-01 19:00:00', '2'),
    ('3', '2023-06-02 19:00:00', '2'),
    ('2', '2023-06-03 19:00:00', '5'),
    ('3', '2023-06-04 19:00:00', '5'),
    ('2', '2023-06-05 19:00:00', '2'),
    ('3', '2023-06-06 19:00:00', '2'),
    ('2', '2023-06-07 19:00:00', '6'),
    ('3', '2023-06-07 20:00:00', '3'),
    ('2', '2023-06-07 21:00:00', '3'),
    ('3', '2023-06-07 22:00:00', '2'),
    ('2', '2023-06-08 10:00:00', '3'),
    ('3', '2023-06-08 11:00:00', '2'),
    ('2', '2023-06-08 12:00:00', '3'),
    ('3', '2023-06-08 13:00:00', '2'),
    ('2', '2023-06-08 14:00:00', '3'),
    ('3', '2023-06-13 10:00:00', '2'),
    ('2', '2023-06-13 11:00:00', '3'),
    ('3', '2023-06-13 12:00:00', '2'),
    ('2', '2023-06-14 13:00:00', '3'),
    ('3', '2023-06-15 14:00:00', '2'),
    ('2', '2023-06-16 10:00:00', '3'),
    ('3', '2023-06-16 11:00:00', '2'),
    ('2', '2023-06-16 12:00:00', '3'),
    ('3', '2023-06-16 13:00:00', '2'),
    ('2', '2023-06-16 14:00:00', '3'),
    ('3', '2023-06-17 12:00:00', '3'),
    ('2', '2023-06-18 13:00:00', '3'),
    ('3', '2023-06-19 14:00:00', '3'),
    ('2', '2023-06-20 14:00:00', '5'),
    ('3', '2023-06-21 14:00:00', '2'),
    ('2', '2023-06-22 14:00:00', '2'),
    ('3', '2023-06-23 14:00:00', '2'),
    ('2', '2023-06-24 14:00:00', '3'),
    ('3', '2023-06-25 14:00:00', '3'),
    ('2', '2023-06-26 14:00:00', '3'),
    ('3', '2023-06-26 15:00:00', '2'),
    ('2', '2023-06-26 16:00:00', '2'),
    ('3', '2023-06-26 17:00:00', '2'),
    ('2', '2023-06-27 14:00:00', '2'),
    ('3', '2023-06-27 15:00:00', '2'),
    ('2', '2023-06-27 16:00:00', '3'),
    ('3', '2023-06-27 17:00:00', '2'),
    ('2', '2023-06-27 18:00:00', '2'),
    ('3', '2023-06-27 19:00:00', '2'),
    ('2', '2023-06-27 20:00:00', '2'),
    ('3', '2023-06-27 21:00:00', '12'),
    ('2', '2023-06-27 22:00:00', '12'),
    ('3', '2023-06-28 15:00:00', '3'),
    ('2', '2023-06-28 16:00:00', '3'),
    ('2', '2023-06-29 17:00:00', '3'),
    ('2', '2023-06-29 18:00:00', '2'),
    ('2', '2023-06-29 19:00:00', '2'),
    ('2', '2023-06-30 00:00:00', '2'),
    ('3', '2023-06-30 01:00:00', '10');
A simple, short C# program for my DB class. Program interact with postgreDB through ODBC - need [postgre ODBC driver](http://www.postgresql.org/ftp/odbc/versions/).

Server ip is hardcoded within the program - since it require special permission to log in, there is little worry about it. Username and password were not - due to obvious reasons.

Program was fairly simple and quick to write, since I've used ODBC before (technically, it's OLEDB - a different API but the usage is almost the same). Didn't run into any significant problem.

Schema:
Product(
    maker varchar(255),
    model int PRIMARY KEY,
    type varchar(255))
    
PC (
    model int PRIMARY KEY,
    speed double,
    ram integer,
    hd integer,
    price integer)
    
Laptop (
    model integer PRIMARY KEY,
    speed double,
    ram integer,
    hd integer,
    screen double,
    price integer)
    
Printer (
    model integer PRIMARY KEY,
    color boolean,
    type varchar(255),
    price integer)
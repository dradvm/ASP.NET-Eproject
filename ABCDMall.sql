create database ABCDMall

go

use ABCDMall

create table [Admin]
(
	Email nvarchar(200) primary key,
	[Password] varchar(64) not null
)

create table Gallery
(
	ID int primary key identity,
	[Image] nvarchar(200) not null,
	[Description] nvarchar(200)
)

create table Feedback
(
	ID int primary key identity,
	Email nvarchar(200),
	SendingTime datetime not null,
	Content nvarchar(1000) not null
)

create table ShopType
(
	ID int primary key identity,
	[Name] nvarchar(200) not null
)

create table [Floor]
(
	ID int primary key identity,
	[Name] nvarchar(200) not null,
	[Description] nvarchar(1000) not null
)

create table Shop
(
	ID int primary key identity,
	ShopeType int references ShopType(ID) not null,
	[Floor] int references [Floor](ID) not null,
	[Name] nvarchar(200) not null,
	Logo nvarchar(200) not null,
	[Description] nvarchar(1000) not null 
)

create table Product
(
	ID int primary key identity,
	Shop int references Shop(ID) on delete cascade not null,
	[Name] nvarchar(200) not null,
	[Image] nvarchar(200) not null,
	[Description] nvarchar(1000)
)

create table SeatType
(
	ID int primary key identity,
	[Name] nvarchar(10) not null,
	Price decimal not null check(Price >= 0)
)

create table Cinema
(
	ID int primary key identity,
	[Name] nvarchar(10) not null
)

create table Seat
(
	ID int primary key identity,
	[Name] nvarchar(10) not null,
	Cinema int references Cinema(ID) on delete cascade not null,
	unique ([Name], Cinema),
	SeatType int references SeatType(ID) not null
)

create table Movie
(
	ID int primary key identity,
	[Name] nvarchar(200) not null,
	[Image] nvarchar(200) not null,
	[Description] nvarchar(1000) not null,
	Duration int not null check(Duration > 0),
	RealeaseDate date not null,
	Director nvarchar(100) not null,
	Active int not null default 1 check(Active in (0, 1))
)

create table Showtime
(
	ID int primary key identity,
	Cinema int references Cinema(ID) not null,
	Movie int references Movie(ID) on delete cascade not null,
	StartingTime datetime not null,
	EndingTime datetime not null,
	check(EndingTime > StartingTime)
)

create table Ticket
(
	ID int primary key identity,
	Seat int references Seat(ID) on delete set null,
	Showtime int references Showtime(ID) on delete set null,
	CustomerName nvarchar(100) not null,
	CustomerEmail nvarchar(200) not null,
	Price decimal not null check(Price >= 0),
	PaymentTime datetime not null
)

create table Genre
(
	ID int primary key identity,
	[Name] nvarchar(100) not null
)

create table MovieGenre
(
	Movie int references Movie(ID) on delete cascade not null,
	Genre int references Genre(ID) not null,
	primary key (Movie, Genre)
)

insert into [Admin]
values
	('plateportal@gmail.com', '70DA09DC10089B114D7863F0BF9A5AA9CF5A6BDB5C92E35352038F131753DAEA') --plateportaladmin

insert into ShopType
values
	('Shopping center'),
	('Food court')
	
insert into [Floor]
values
	('1', 'Fashion boutiques, beauty section, and food court with local and international cuisines.'),
	('2', 'Electronics stores, home appliances, sportswear, and a large bookstore.'),
	('3', 'Lifestyle stores, furniture, cafes, and art galleries with seasonal exhibitions.'),
	('4', 'Movie theater with multiple screening rooms, VIP seating, and snacks.')

insert into SeatType
values
	('Standard', 120000),
	('VIP', 125000),
	('Sweetbox', 290000)

insert into Cinema
values
	('1'),
	('2'),
	('3')

insert into Seat
values
	('A1', 1, 1),
	('A2', 1, 1),
	('A3', 1, 1),
	('A4', 1, 1),
	('A5', 1, 1),
	('A6', 1, 1),
	('A7', 1, 1),
	('A8', 1, 1),
	('A9', 1, 1),
	('A10', 1, 1),
	('A11', 1, 1),
	('A12', 1, 1),
	('A13', 1, 1),
	('A14', 1, 1),
	('A15', 1, 1),
	('B1', 1, 1),
	('B2', 1, 1),
	('B3', 1, 1),
	('B4', 1, 1),
	('B5', 1, 1),
	('B6', 1, 1),
	('B7', 1, 1),
	('B8', 1, 1),
	('B9', 1, 1),
	('B10', 1, 1),
	('B11', 1, 1),
	('B12', 1, 1),
	('B13', 1, 1),
	('B14', 1, 1),
	('B15', 1, 1),
	('C1', 1, 2),
	('C2', 1, 2),
	('C3', 1, 2),
	('C4', 1, 2),
	('C5', 1, 2),
	('C6', 1, 2),
	('C7', 1, 2),
	('C8', 1, 2),
	('C9', 1, 2),
	('C10', 1, 2),
	('C11', 1, 2),
	('C12', 1, 2),
	('C13', 1, 2),
	('C14', 1, 2),
	('C15', 1, 2),
	('D1', 1, 2),
	('D2', 1, 2),
	('D3', 1, 2),
	('D4', 1, 2),
	('D5', 1, 2),
	('D6', 1, 2),
	('D7', 1, 2),
	('D8', 1, 2),
	('D9', 1, 2),
	('D10', 1, 2),
	('D11', 1, 2),
	('D12', 1, 2),
	('D13', 1, 2),
	('D14', 1, 2),
	('D15', 1, 2),
	('E1', 1, 2),
	('E2', 1, 2),
	('E3', 1, 2),
	('E4', 1, 2),
	('E5', 1, 2),
	('E6', 1, 2),
	('E7', 1, 2),
	('E8', 1, 2),
	('E9', 1, 2),
	('E10', 1, 2),
	('E11', 1, 2),
	('E12', 1, 2),
	('E13', 1, 2),
	('E14', 1, 2),
	('E15', 1, 2),
	('F1', 1, 2),
	('F2', 1, 2),
	('F3', 1, 2),
	('F4', 1, 2),
	('F5', 1, 2),
	('F6', 1, 2),
	('F7', 1, 2),
	('F8', 1, 2),
	('F9', 1, 2),
	('F10', 1, 2),
	('F11', 1, 2),
	('F12', 1, 2),
	('F13', 1, 2),
	('F14', 1, 2),
	('F15', 1, 2),
	('G1', 1, 3),
	('G2', 1, 3),
	('G3', 1, 3),
	('G4', 1, 3),
	('G5', 1, 3),
	('G6', 1, 3),
	('G7', 1, 3),
	('G8', 1, 3),
	('A1', 2, 1),
	('A2', 2, 1),
	('A3', 2, 1),
	('A4', 2, 1),
	('A5', 2, 1),
	('A6', 2, 1),
	('A7', 2, 1),
	('A8', 2, 1),
	('A9', 2, 1),
	('A10', 2, 1),
	('A11', 2, 1),
	('A12', 2, 1),
	('A13', 2, 1),
	('A14', 2, 1),
	('A15', 2, 1),
	('B1', 2, 1),
	('B2', 2, 1),
	('B3', 2, 1),
	('B4', 2, 1),
	('B5', 2, 1),
	('B6', 2, 1),
	('B7', 2, 1),
	('B8', 2, 1),
	('B9', 2, 1),
	('B10', 2, 1),
	('B11', 2, 1),
	('B12', 2, 1),
	('B13', 2, 1),
	('B14', 2, 1),
	('B15', 2, 1),
	('C1', 2, 2),
	('C2', 2, 2),
	('C3', 2, 2),
	('C4', 2, 2),
	('C5', 2, 2),
	('C6', 2, 2),
	('C7', 2, 2),
	('C8', 2, 2),
	('C9', 2, 2),
	('C10', 2, 2),
	('C11', 2, 2),
	('C12', 2, 2),
	('C13', 2, 2),
	('C14', 2, 2),
	('C15', 2, 2),
	('D1', 2, 2),
	('D2', 2, 2),
	('D3', 2, 2),
	('D4', 2, 2),
	('D5', 2, 2),
	('D6', 2, 2),
	('D7', 2, 2),
	('D8', 2, 2),
	('D9', 2, 2),
	('D10', 2, 2),
	('D11', 2, 2),
	('D12', 2, 2),
	('D13', 2, 2),
	('D14', 2, 2),
	('D15', 2, 2),
	('E1', 2, 2),
	('E2', 2, 2),
	('E3', 2, 2),
	('E4', 2, 2),
	('E5', 2, 2),
	('E6', 2, 2),
	('E7', 2, 2),
	('E8', 2, 2),
	('E9', 2, 2),
	('E10', 2, 2),
	('E11', 2, 2),
	('E12', 2, 2),
	('E13', 2, 2),
	('E14', 2, 2),
	('E15', 2, 2),
	('F1', 2, 2),
	('F2', 2, 2),
	('F3', 2, 2),
	('F4', 2, 2),
	('F5', 2, 2),
	('F6', 2, 2),
	('F7', 2, 2),
	('F8', 2, 2),
	('F9', 2, 2),
	('F10', 2, 2),
	('F11', 2, 2),
	('F12', 2, 2),
	('F13', 2, 2),
	('F14', 2, 2),
	('F15', 2, 2),
	('G1', 2, 3),
	('G2', 2, 3),
	('G3', 2, 3),
	('G4', 2, 3),
	('G5', 2, 3),
	('G6', 2, 3),
	('G7', 2, 3),
	('G8', 2, 3),
	('A1', 3, 1),
	('A2', 3, 1),
	('A3', 3, 1),
	('A4', 3, 1),
	('A5', 3, 1),
	('A6', 3, 1),
	('A7', 3, 1),
	('A8', 3, 1),
	('A9', 3, 1),
	('A10', 3, 1),
	('A11', 3, 1),
	('A12', 3, 1),
	('A13', 3, 1),
	('A14', 3, 1),
	('A15', 3, 1),
	('B1', 3, 1),
	('B2', 3, 1),
	('B3', 3, 1),
	('B4', 3, 1),
	('B5', 3, 1),
	('B6', 3, 1),
	('B7', 3, 1),
	('B8', 3, 1),
	('B9', 3, 1),
	('B10', 3, 1),
	('B11', 3, 1),
	('B12', 3, 1),
	('B13', 3, 1),
	('B14', 3, 1),
	('B15', 3, 1),
	('C1', 3, 2),
	('C2', 3, 2),
	('C3', 3, 2),
	('C4', 3, 2),
	('C5', 3, 2),
	('C6', 3, 2),
	('C7', 3, 2),
	('C8', 3, 2),
	('C9', 3, 2),
	('C10', 3, 2),
	('C11', 3, 2),
	('C12', 3, 2),
	('C13', 3, 2),
	('C14', 3, 2),
	('C15', 3, 2),
	('D1', 3, 2),
	('D2', 3, 2),
	('D3', 3, 2),
	('D4', 3, 2),
	('D5', 3, 2),
	('D6', 3, 2),
	('D7', 3, 2),
	('D8', 3, 2),
	('D9', 3, 2),
	('D10', 3, 2),
	('D11', 3, 2),
	('D12', 3, 2),
	('D13', 3, 2),
	('D14', 3, 2),
	('D15', 3, 2),
	('E1', 3, 2),
	('E2', 3, 2),
	('E3', 3, 2),
	('E4', 3, 2),
	('E5', 3, 2),
	('E6', 3, 2),
	('E7', 3, 2),
	('E8', 3, 2),
	('E9', 3, 2),
	('E10', 3, 2),
	('E11', 3, 2),
	('E12', 3, 2),
	('E13', 3, 2),
	('E14', 3, 2),
	('E15', 3, 2),
	('F1', 3, 2),
	('F2', 3, 2),
	('F3', 3, 2),
	('F4', 3, 2),
	('F5', 3, 2),
	('F6', 3, 2),
	('F7', 3, 2),
	('F8', 3, 2),
	('F9', 3, 2),
	('F10', 3, 2),
	('F11', 3, 2),
	('F12', 3, 2),
	('F13', 3, 2),
	('F14', 3, 2),
	('F15', 3, 2),
	('G1', 3, 3),
	('G2', 3, 3),
	('G3', 3, 3),
	('G4', 3, 3),
	('G5', 3, 3),
	('G6', 3, 3),
	('G7', 3, 3),
	('G8', 3, 3)

insert into Genre
values
	('Action'),
	('Adventure'),
	('Animated'),
	('Comedy'),
	('Drama'),
	('Fantasy'),
	('Historical'),
	('Horror'),
	('Musical'),
	('Noir'),
	('Romance'),
	('Science Fiction'),
	('Thriller'),
	('Western')

select *
from [Admin]

select *
from ShopType

select *
from [Floor]

select *
from SeatType

select *
from Cinema

select *
from Seat

select *
from Genre
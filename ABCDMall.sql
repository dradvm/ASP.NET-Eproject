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
	('abcdmallonline@gmail.com', 'E621F5899D5FB459E346F84BAC5A6B771921F3848C69742AA3D59E9075DE8392') --abcdmallonline
	
/*insert into Gallery
values*/
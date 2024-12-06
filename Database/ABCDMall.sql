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

insert into Gallery
values
	('noi-chien.png', 'Lock&Lock Air Fryer ECF-300B (3L)'),
	('Hwansaenggo.png', 'Hwansaenggo Serum Infused Foaming Cleanser 150ml'),
	('MDV-107-1A3V.png', 'Genuine Men''s CASIO Watch MDV-107-1A3'),
	('pnjsilver-ztxmw060007.png', 'Silver Ring PNJSilver ZTXMW060007'),
	('WMNS+AIR+JORDAN+1+LOW.png', 'Air Jordan 1 Low')

insert into Feedback
values
	(null, '2024-11-10', 'This website is great'),
	('ndhunga22008@cusc.ctu.edu.vn', '2024-01-16', 'Don''t know how to use this'),
	('hungb2203556@student.ctu.edu.vn', '2023-12-10', 'Nice but needs some improvements')

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

insert into Shop
values
	(1, 2, 'Lock&Lock', 'locknlock.svg', 'Lock&Lock is a household products company headquartered in Seoul, South Korea. Since its establishment in 1978, Lock & Lock has been exporting products to 119 countries worldwide.'),
	(1, 1, 'The Face Shop', '85ba6666-85aa-43a5-8c30-a474210a6c38.png', 'The Face Shop Vietnam was born with the mission of bringing premium, quality, genuine products in the field of skin care and makeup to dynamic, modern, and caring Vietnamese women. Take care of your beauty.'),
	(1, 2, 'Casio', 'logo_casio_g_store.png', 'Casio Computer Co., Ltd. is a Japanese multinational electronics manufacturing corporation headquartered in Shibuya, Tokyo, Japan. Its products include calculators, mobile phones, digital cameras, electronic musical instruments, and analogue and digital watches. It was founded in 1946, and in 1957 introduced the first entirely compact electronic calculator. It was an early digital camera innovator, and during the 1980s and 1990s, the company developed numerous affordable home electronic keyboards for musicians along with introducing the first mass-produced digital watches.'),
	(1, 1, 'PNJ', 'pnj.png', 'Every story has a beginning. Starting from Phu Nhuan Jewelry store in 1988 and now in 2021, PNJ has grown to become one of the leading corporations in the field of professional jewelry production and business.'),
	(1, 3, 'Nike', 'Logo_NIKE.svg.png', 'Nike, Inc. is an American athletic footwear and apparel corporation headquartered near Beaverton, Oregon, United States. It is the world''s largest supplier of athletic shoes and apparel and a major manufacturer of sports equipment, with revenue in excess of US$46 billion in its fiscal year 2022.'),
	(2, 1, 'Jollibee', '09314542_logo-jollibee-500x500-1-401x400.jpg', 'Jollibee is a Filipino chain of fast food restaurants owned by Jollibee Foods Corporation (JFC) which serves as its flagship brand. Established in 1978 by Tony Tan Caktiong, it is the Philippines'' top fast food restaurant and is among the world''s fastest growing restaurants, expanding its international presence from 2014 to 2024 almost sixfold. As of January 2024, there were over 1,668 Jollibee fast-food branches across 17 countries, with restaurants in Southeast Asia, East Asia (Hong Kong and Macau), the Middle East, North America, and Europe (including Spain, Italy, and the United Kingdom). Jollibee is best known for its bestselling item, the Chickenjoy.'),
	(2, 1, 'The Pizza Company', '14514819_LOGO-THE-PIZZA-COMPANY-500x500-1-400x400.jpg', 'We are The Pizza Company. The brand is owned by Minor Food Group, one of Asia''s leading corporations in the luxury hotel and culinary industry. In 2013, The Pizza Company restaurant first appeared in Vietnam.'),
	(2, 3, 'Highlands Coffee', 'cafe-highland.png', 'Highlands Coffee is an extremely familiar name for people who are passionate about coffee or fast food in Vietnam, especially young people or those who are working. Highlands Coffee was founded in 1999 by an overseas Vietnamese businessman named David Thai who had a strong love for his homeland and was willing to leave his family in the US to return to Vietnam to start a business.')

insert into Product
values
	(1, 'Lock&Lock Air Fryer ECF-300B (3L)', 'noi-chien.png', 'When it comes to Lock&Lock, many consumers have given this brand extremely positive reviews. One of the famous products of this brand that is currently trusted by many housewives is the Lock&Lock ECF-300B (3L) oil-free fryer. This product possesses many outstanding features that bring high efficiency in the kitchen.'),
	(1, 'Lock&Lock Colorful Tumbler thermos bottle LHC3222PIK (390ml – pink)', 'binh-gi-nhi-t-lock-lock-colorful-tumbler-lhc3222pik-390ml-h-ng.jpg', null),
	(2, 'Hwansaenggo Serum Infused Foaming Cleanser 150ml', 'Hwansaenggo.png', 'Facial cleanser with skin care formula from 14 premium traditional herbs helps clean the skin, while supporting the restoration of vitality, providing care solutions for skin with aging problems such as wrinkles, skin & dark color.'),
	(2, 'THE FACE SHOP All Clear Micellar Cleansing Oil Whip 250ml', 'fa4f5004-9bb6-4902-91e1-2bec1cc40610.png', 'Able to remove dirt, stubborn makeup along with dirt deep inside pores, giving skin a healthy, naturally radiant glow.'),
	(3, 'Genuine Men''s CASIO Watch MDV-107-1A3', 'MDV-107-1A3V.png', 'Take your all-metal analog style to the water. With water resistance up to 200 meters, a unidirectional rotating bezel and a screw-down case back, this is a watch that''s ready for any ocean sport or marine activity you throw your way. The sporty yet classic design also features a handy date display, providing practical convenience for a fashionable lifestyle.'),
	(4, 'Silver Ring PNJSilver ZTXMW060007', 'pnjsilver-ztxmw060007.png', 'With a trendy design and stones attached around the surface of the ring on 92.5 silver material, PNJSilver brings a ring with youthful beauty but no less unconventional, helping girls look outstanding.'),
	(5, 'Air Jordan 1 Low', 'WMNS+AIR+JORDAN+1+LOW.png', 'Inspired by the original that debuted in 1985, the Air Jordan 1 Low offers a clean, classic look that''s familiar yet always fresh. With an iconic design that pairs perfectly with any ''fit, these kicks ensure you''ll always be on point.'),
	(6, '2 Happy Crispy Chicken + 1 Medium Fries + 1 Soft Drink', 'g_gi_n_vui_v_-_1_1.png', null),
	(6, '1 medium Jolly Spaghetti + 2 boneless chicken + 1 medium French fries + 1 soft drink', 'm_jolly_-_2-compressed.jpg', null),
	(6, 'Chicken Rice with Garlic Sauce', '54d1040569d8ce8697c9_1.jpg', null)

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

insert into Movie
values
	('Moana 2', 'cgv_1200x1800.jpg', '“Moana''s Journey 2” is the rebirth of Moana and Maui after 3 years, returning on a journey with new members. Following the call of her ancestors, Moana will embark on a journey to the distant seas of Oceania and will go to dangerous, long-lost waters. Let''s wait for the upcoming end of Moana full of thorns.', 99, '2024-12-04', 'David G. Derrick Jr.', 1),
	('A Minecraft Movie', 'vn_mncrft_vert_tsr2_2764x4096_intl.jpg', 'Welcome to the world of Minecraft, where creativity not only helps you craft but is also vital to survival! Four misfits - Garrett “The Garbage Man” Garrison (Momoa), Henry (Hansen), Natalie (Myers) and Dawn (Brooks) - suddenly find themselves in trouble when they are pulled through a mysterious door leading to the Overworld: a Strange world created by cubes and developed by imagination. To return home, they need to master this world (and protect it from evil entities like Piglins and Zombies) while embarking on a magical adventure with an expert and difficult crafter. measure - Steve (Black). This journey will challenge the courage of all five people, motivating them to rediscover the qualities that make them special... and at the same time the skills necessary to return to the real world.', 120, '2025-04-04', 'Jared Hess', 1),
	('Ghost Cat Anzu', '350x495-meoma.jpg', 'The friendship story of little girl Karin - orphaned by her mother and abandoned by her father - and a ghost cat Anzu who lives a bit messy but tries very hard to heal her mental wounds.', 94, '2024-12-06', 'Yōko Kuno, Nobuhiro Yamashita', 1),
	('Venom: The Last Dance', 'rsz_vnm3_intl_online_1080x1350_tsr_01.jpg', 'This is the last and most epic film about the couple Venom and Eddie Brock (Tom Hardy). After teleporting from the Marvel Universe in ''Spider-man: No way home'' (2021) back to reality, Eddie Brock and Venom will now have to face the powerful evil god Knull - the creator of the entire Symbiote race. and other lurking forces. The couple Eddie and Venom will have to make a fierce decision to end this final bet.', 109, '2024-10-25', 'Kelly Marcel', 0),
	('We Live In Time', 'poster_ngay_ta_da_yeu_6.jpg', 'Fate brought a promising female chef and a man who had just gone through a broken marriage together in special circumstances. The film is about the ten-year deep love story of this couple, from the moment they fell in love, built a home, until an incident happened that completely changed their lives.', 108, '2024-11-15', 'John Crowley', 1)

insert into Showtime
values
	(1, 1, '2024-12-19 06:30:00', '2024-12-19 08:39:00'),
	(2, 1, '2024-12-21 16:30:00', '2024-12-21 18:39:00'),
	(3, 3, '2024-12-15 10:00:00', '2024-12-15 12:04:00'),
	(3, 3, '2024-12-15 12:10:00', '2024-12-15 14:14:00'),
	(3, 3, '2024-12-15 15:00:00', '2024-12-15 15:04:00'),
	(1, 4, '2024-12-05 18:00:00', '2024-12-05 20:19:00')

insert into Ticket
values
	(10, 1, 'Hung', 'ndhung@gmail.com', 120000, '2024-12-05 12:36:18'),
	(31, 1, 'Duy', 'duy@gmail.com', 125000, '2024-12-02 14:31:18'),
	(91, 1, 'Phuc', 'phuc@gmail.com', 290000, '2024-12-05 17:33:48'),
	(95, 6, 'Hung', 'ndhung@gmail.com', 200000, '2024-12-01 02:56:12'),
	(233, 4, 'Tem', 'tem@gmail.com', 120000, '2024-12-10 23:45:36'),
	(252, 4, 'Hung', 'ndhung@gmail.com', 125000, '2024-12-05 12:36:18'),
	(253, 4, 'Duy', 'duy@gmail.com', 125000, '2024-12-02 14:31:18'),
	(254, 4, 'Phuc', 'phuc@gmail.com', 125000, '2024-12-05 17:33:48')

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

insert into MovieGenre
values
	(1, 1),
	(1, 2),
	(1, 3),
	(1, 6),
	(2, 1),
	(2, 2),
	(2, 6),
	(3, 3),
	(4, 1),
	(4, 12),
	(4, 2),
	(4, 6),
	(5, 11)

select *
from [Admin]

select *
from Gallery

select *
from Feedback

select *
from ShopType

select *
from [Floor]

select *
from Shop

select *
from Product

select *
from SeatType

select *
from Cinema

select *
from Seat

select *
from Movie

select *
from Showtime

select *
from Ticket

select *
from Genre

select *
from MovieGenre
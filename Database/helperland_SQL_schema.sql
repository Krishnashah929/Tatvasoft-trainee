Create database Helperland

Use Helperland
go

Create Schema Helperland_Schema

go

Create table Helperland_Schema.customer_registration
(
customer_id int identity (1,1) primary key,
first_name nvarchar(50) not null,
last_name nvarchar(50),
email  nvarchar(50) not null,
phone_number int not null,
password int not null,
created_at timestamp
)

Create table Helperland_Schema.provider_registration
(
provider_id int identity (1,1) primary key,
first_name nvarchar(50) not null,
last_name nvarchar(50),
email  nvarchar(50) not null,
phone_number int not null,
password int not null,
gender nvarchar(50),
birth_date date ,
avtar int not null,
nationality nvarchar(50),
created_at timestamp
)

Create table Helperland_Schema.address
(
address_id int identity(1,1) primary key,
customer_id int not null foreign key references Helperland_Schema.customer_registration(customer_id) ,
provider_id int not null foreign key references Helperland_Schema.provider_registration(provider_id) ,
first_name nvarchar(50) not null,
last_name nvarchar(50),
email  nvarchar(50) not null,
phone_number int not null,
password int not null,
created_at timestamp
)

Create table Helperland_Schema.contact_us
(
customer_id int not null foreign key references Helperland_Schema.customer_registration(customer_id) ,
first_name nvarchar(50) not null,
last_name nvarchar(50),
email  nvarchar(50) not null,
phone_number int not null,
subject text not null,
created_at timestamp
)

Create table Helperland_Schema.book_service
(
service_id int identity(1,1) primary key,
customer_id int not null foreign key references Helperland_Schema.customer_registration(customer_id) ,
address_id int not null foreign key references Helperland_Schema.address(address_id),
booking_date date not null,
stay_time date not null,
additional_services int not null,
comment text ,
have_pet nvarchar(10) not null,
net_amount int not null,
status nvarchar(50) not null,
distance int not null,
rating int ,
feedback text ,
created_at timestamp
)





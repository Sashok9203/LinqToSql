﻿create database [Library]
go

use [Library]
go

create table [Countries]
(
  [Id] int not null primary key identity,
  [Name] nvarchar(20) not null unique
);
go

create table [Authors]
(
  [Id] int not null primary key identity,
  [Name] nvarchar(20) not null ,
  [Surname] nvarchar(20) not null,
  [CountrieId] int not null references [Countries](Id),
);
go

create table [Books]
(
  [Id] int not null primary key identity,
  [Name] nvarchar(20) not null ,
  [PageCount] int not null,
  [AuthorId] int not null references [Authors](Id),
);
go

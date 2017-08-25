create table tblPeanutRequirement (
    Id INT IDENTITY NOT NULL,
    BusinessId UNIQUEIDENTIFIER not null unique,
    Name NVARCHAR(100) not null,
    Quantity FLOAT(53) not null,
    Unit NVARCHAR(15) not null,
    Url NVARCHAR(255) null,
    Peanut_Id INT null,
    primary key (Id)
);

GO


alter table tblPeanutRequirement 
    add constraint FK_PEANUT_REQUIREMENT 
    foreign key (Peanut_Id) 
    references tblPeanut;
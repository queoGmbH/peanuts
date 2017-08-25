create table tblPeanutComment (
    Id INT IDENTITY NOT NULL,
    BusinessId UNIQUEIDENTIFIER not null unique,
    Comment NVARCHAR(1000) null,
    CreatedAt DATETIME not null,
    ChangedAt DATETIME null,
    CreatedBy_Id INT not null,
    ChangedBy_Id INT null,
    Peanut_Id INT NOT null,
    primary key (Id)
)

GO

alter table tblPeanutComment
    add constraint FK_CREATOR_OF_PEANUTCOMMENT 
    foreign key (CreatedBy_Id) 
    references tblUser

alter table tblPeanutComment 
    add constraint FK_PEANUT_WITH_COMMENTS 
    foreign key (Peanut_Id) 
    references tblPeanut
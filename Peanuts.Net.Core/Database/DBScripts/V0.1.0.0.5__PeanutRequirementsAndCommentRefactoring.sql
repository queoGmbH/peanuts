
drop table tblPeanutRequirement;
drop table tblPeanutComment;

create table tblPeanutRequirement (
    Peanut_Id INT not null,
    Name NVARCHAR(100) not null,
    Quantity FLOAT(53) not null,
    Unit NVARCHAR(15) not null,
    Url NVARCHAR(255) null
)

create table tblPeanutComment (
    Peanut_Id INT not null,
    Comment NVARCHAR(1000) null,
    CreatedAt DATETIME not null,
    ChangedAt DATETIME null,
    CreatedBy_Id INT not null,
    ChangedBy_Id INT null
)

GO

alter table tblPeanutRequirement 
	add constraint FK_PEANUT_REQUIREMENT 
	foreign key (Peanut_Id) 
	references tblPeanut

alter table tblPeanutComment 
	add constraint FK_CREATOR_OF_PEANUTCOMMENT 
	foreign key (CreatedBy_Id) 
	references tblUser

alter table tblPeanutComment 
	add constraint FK_PEANUT_WITH_COMMENTS 
	foreign key (Peanut_Id) 
	references tblPeanut

Alter table tblPeanut add 
	 PeanutState NVARCHAR(255) not null CONSTRAINT DF_PEANUT_STATE DEFAULT 'Scheduling';

GO 

Update tblPeanut set PeanutState = 'Started' where IsFixed = 1;

GO

Alter table tblPeanut drop column IsFixed;
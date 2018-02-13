Alter table tblUserGroupMembership 
	add AutoAcceptBills BIT null;

GO

Update tblUserGroupMembership set AutoAcceptBills = 0;

GO

Alter table tblUserGroupMembership 
	alter column AutoAcceptBills BIT not null;

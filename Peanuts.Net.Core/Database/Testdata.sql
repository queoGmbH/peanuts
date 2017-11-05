SET IDENTITY_INSERT tblUser ON;
insert into tblUser
( Id , BusinessId                              , UserName						, Email			, FirstName		, LastName  , PasswordResetCode , IsEnabled, Street           , StreetNumber, PostalCode, City     , PayPalBusinessName        , AutoAcceptPayPalPayments , CountryTwoLetterIsoCode, CreatedAt              , PasswordHash) Values
( 1  , '{261F709C-3256-460B-8B50-580617575C6E}', 'admin'						, 'test@test.de', 'Admin'		, 'Admin'	, null              , 'true'   , 'Tharandter Str.', '13'        , '01159'   , 'Dresden', null                      , 1                        , 'DE'                   , '2017-04-04 08:00:00' , 'AJZzK6F9hUW2dSuYPgzMMkDdes7jq/eDT1qqF6wavrxDnbe+QaHWv1xKHqce3sQWXg==');


SET IDENTITY_INSERT tblUser OFF;

insert into tblUserRoles 
(User_Id, [Role]) Values
(1      , 'Administrator');


SET IDENTITY_INSERT tblUserGroup ON;

INSERT INTO tblUserGroup
([Id], [BusinessId]                            ,[AdditionalInformations]                   , [ChangedAt], [CreatedAt]           , [Name]        , [ChangedBy_Id], [CreatedBy_Id])     VALUES
( 1  , '{27EB4A8A-3875-4029-96DF-55E4F2FEEA71}','Diese Gruppe ist nur für Testzwecke'      , null       , '2017-04-04 08:00:00' , 'Testgruppe  ', 1             , 1            );

SET IDENTITY_INSERT tblUserGroup OFF;

SET IDENTITY_INSERT tblAccount ON;

INSERT INTO tblAccount
([Id], [BusinessId]                            , [Balance])     VALUES
( 1  , '{5F37B278-B634-4705-9D65-B5F02DE07449}', 0        );

SET IDENTITY_INSERT tblAccount OFF;

SET IDENTITY_INSERT tblUserGroupMembership ON;

INSERT INTO tblUserGroupMembership
([Id], [BusinessId]                            , [UserGroup_Id], [User_Id] , [MembershipType], [Account_Id] , [ChangedAt], [CreatedAt]           , [ChangedBy_Id], [CreatedBy_Id])     VALUES
( 1  , '{F829C488-82CD-4C9B-927E-2A51FE5EDC9E}', 1             , 1         , 'Administrator' , 1            , null       , '2017-04-04 08:00:00' , null          , 1             );

SET IDENTITY_INSERT tblUserGroupMembership OFF;


SET IDENTITY_INSERT tblPeanutParticipationType ON;
INSERT INTO [dbo].[tblPeanutParticipationType]
([Id]	,[BusinessId]										,[Name]           ,[IsCreditor]           ,[IsProducer]           ,[MaxParticipatorsOfType]           ,[CreatedAt]           ,[ChangedAt]           ,[CreatedBy_Id]           ,[ChangedBy_Id]           ,[UserGroup_Id])     VALUES
(1		,'{72065393-9F65-412D-9971-B8BE6A857E2E}'		        ,'Teilnehmer'     ,1					  ,0					  ,0								  ,'2017-04-04 08:00:00' ,'2017-04-04 08:00:00' ,1						  ,1						,1)

SET IDENTITY_INSERT tblPeanutParticipationType OFF;

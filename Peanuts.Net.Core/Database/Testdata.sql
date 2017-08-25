SET IDENTITY_INSERT tblUser ON;
insert into tblUser
( Id , BusinessId                              , UserName                      , Email                        , FirstName , LastName   , PasswordResetCode , IsEnabled, Street           , StreetNumber, PostalCode, City     , PayPalBusinessName        , AutoAcceptPayPalPayments , CountryTwoLetterIsoCode, CreatedAt              , PasswordHash) Values
( 1  , '{261F709C-3256-460B-8B50-580617575C6E}', 'uwe.oehmichen@queo-flow.com' , 'uwe.oehmichen@queo-flow.com', 'Uwe'     , 'Oehmichen', null              , 'true'   , 'Tharandter Str.', '13'        , '01159'   , 'Dresden', null                      , 1                        , 'DE'                   , '2017-04-04 08:00:00' , 'AJZzK6F9hUW2dSuYPgzMMkDdes7jq/eDT1qqF6wavrxDnbe+QaHWv1xKHqce3sQWXg=='),
( 2  , '{0E8E6AEC-711A-4277-B275-4097687CC4D0}', 't.jaekel@queo-group.com'     , 't.jaekel@queo-group.com'    , 'Tobias'  , 'Jäkel'    , null              , 'true'   , 'Tharandter Str.', '13'        , '01159'   , 'Dresden', 'tojak@web.de'            , 1                        , 'DE'                   , '2017-04-04 08:00:00' , 'AJZzK6F9hUW2dSuYPgzMMkDdes7jq/eDT1qqF6wavrxDnbe+QaHWv1xKHqce3sQWXg=='),
( 3  , '{F93E3120-9EC3-49BB-9B31-2CBBB3C1599D}', 'd.henkens@queo-flow.com'     , 'd.henkens@queo-flow.com'    , 'Dana'    , 'Henkens'  , null              , 'true'   , 'Tharandter Str.', '13'        , '01159'   , 'Dresden', 'd.henkens@queo-flow.com' , 1                        , 'DE'                   , '2017-04-04 08:00:00' , 'AJZzK6F9hUW2dSuYPgzMMkDdes7jq/eDT1qqF6wavrxDnbe+QaHWv1xKHqce3sQWXg=='),
( 4  , '{8E88C69D-AC7E-445C-9477-CA68DCEA47AA}', 'd.wuttig@queo-group.com'     , 'd.wuttig@queo-group.com'    , 'Daniel'  , 'Wuttig'   , null              , 'true'   , 'Tharandter Str.', '13'        , '01159'   , 'Dresden', null                      , 1                        , 'DE'                   , '2017-04-04 08:00:00' , 'AJZzK6F9hUW2dSuYPgzMMkDdes7jq/eDT1qqF6wavrxDnbe+QaHWv1xKHqce3sQWXg==');


SET IDENTITY_INSERT tblUser OFF;

insert into tblUserRoles 
(User_Id, [Role]) Values
(1      , 'Administrator'),
(2      , 'Administrator'),
(3      , 'Administrator'),
(4      , 'Administrator');


SET IDENTITY_INSERT tblUserGroup ON;

INSERT INTO tblUserGroup
([Id], [BusinessId]                            ,[AdditionalInformations]                   , [ChangedAt], [CreatedAt]           , [Name]        , [ChangedBy_Id], [CreatedBy_Id])     VALUES
( 1  , '{27EB4A8A-3875-4029-96DF-55E4F2FEEA71}','Diese Gruppe ist nur für Testzwecke'      , null       , '2017-04-04 08:00:00' , 'Testgruppe  ', 2             , 2             ),
( 2  , '{62669616-6EC9-417A-BA63-1A0D0EA9164B}','Diese Gruppe ist auch nur für Testzwecke' , null       , '2017-04-04 08:00:00' , 'Testgruppe 2', 2             , 2             );

SET IDENTITY_INSERT tblUserGroup OFF;



SET IDENTITY_INSERT tblAccount ON;

INSERT INTO tblAccount
([Id], [BusinessId]                            , [Balance])     VALUES
( 1  , '{5F37B278-B634-4705-9D65-B5F02DE07449}', 0        ),
( 2  , '{F1185586-E5A1-4436-B8BF-98DCA2C01602}', 0        ),
( 3  , '{AF802598-C873-4A8D-AC0A-1C5839C4B5AB}', 0        ),
( 4  , '{7E26B8BF-39BC-4DE0-AA4C-BAD85BB81799}', 0        ),
( 5  , '{9BA209F9-DEB5-4E54-B177-286FA1289594}', 0        );

SET IDENTITY_INSERT tblAccount OFF;




SET IDENTITY_INSERT tblUserGroupMembership ON;

INSERT INTO tblUserGroupMembership
([Id], [BusinessId]                            , [UserGroup_Id], [User_Id] , [MembershipType], [Account_Id] , [ChangedAt], [CreatedAt]           , [ChangedBy_Id], [CreatedBy_Id])     VALUES
( 1  , '{F829C488-82CD-4C9B-927E-2A51FE5EDC9E}', 1             , 2         , 'Administrator' , 1            , null       , '2017-04-04 08:00:00' , null          , 2             ),
( 2  , '{2F789B0F-042F-4A50-B0FF-89F94F6D45D3}', 1             , 3         , 'Member'        , 2            , null       , '2017-04-04 08:00:00' , null          , 2             ),
( 3  , '{B8C24F01-811F-4C80-90B9-0C954C1691D6}', 2             , 4         , 'Administrator' , 3            , null       , '2017-04-04 08:00:00' , null          , 2             ),
( 4  , '{4CC1DC6E-3A78-4228-9F1E-2AB6FBD3684A}', 1             , 4         , 'Administrator' , 5            , null       , '2017-04-04 08:00:00' , null          , 2             ),
( 5  , '{557522A8-BF53-425E-9435-3D5C57E0DEC6}', 2             , 2         , 'Request'       , 4            , null       , '2017-04-04 08:00:00' , null          , 2             );

SET IDENTITY_INSERT tblUserGroupMembership OFF;



-- SET IDENTITY_INSERT tblPeanutParticipationType ON;

--INSERT INTO tblPeanutParticipationType

--([Id], [BusinessId]                            , [Name]                     , [IsCreditor] , [IsProducer] , [MaxParticipatorsOfType], [CreatedAt]           , [CreatedBy_Id])     VALUES
--( 1  , '{C5096CFB-B1BC-4308-B929-A9EC20C3A5CD}', 'Koch'                     , 0            , 1            , 1                       , '2017-04-04 08:00:00' , null             ),
--( 2  , '{CC920918-4674-40EF-9341-EE88F99ECA07}', 'Einkäufer'                , 1            , 0            , 2                       , '2017-04-04 08:00:00' , null             ),
--( 3  , '{660B58C7-D72E-49C8-9120-4A283E7226BA}', 'Koch und Einkäufer'       , 1            , 1            , 1                       , '2017-04-04 08:00:00' , null             ),
--( 4  , '{C88C07E9-DA74-4250-99E4-5E58B15889E8}', 'Teilnehmer'               , 0            , 0            , null                    , '2017-04-04 08:00:00' , null             ),
--( 5  , '{8190AB32-29FC-4D9A-9D4F-D32DDC317A82}', 'Hilfskoch/Schnibbelhilfe' , 0            , 0            , 2                       , '2017-04-04 08:00:00' , null             ),
--( 6  , '{B79AC27E-AD70-4BD6-B6CF-FB712F8A3410}', 'Abwascher'                , 0            , 0            , 2                       , '2017-04-04 08:00:00' , null             );

--SET IDENTITY_INSERT tblPeanutParticipationType OFF;

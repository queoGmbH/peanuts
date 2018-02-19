
    if exists (select 1 from sys.objects where object_id = OBJECT_ID(N'[FK_BILL_WITH_GUEST_CREDITORS]') AND parent_object_id = OBJECT_ID('tblBillGuestDebitor'))
alter table tblBillGuestDebitor  drop constraint FK_BILL_WITH_GUEST_CREDITORS


    if exists (select 1 from sys.objects where object_id = OBJECT_ID(N'[FK_USERGROUP_WITH_BILLS]') AND parent_object_id = OBJECT_ID('tblBill'))
alter table tblBill  drop constraint FK_USERGROUP_WITH_BILLS


    if exists (select 1 from sys.objects where object_id = OBJECT_ID(N'[FK_CREDITOR_OF_BILL]') AND parent_object_id = OBJECT_ID('tblBill'))
alter table tblBill  drop constraint FK_CREDITOR_OF_BILL


    if exists (select 1 from sys.objects where object_id = OBJECT_ID(N'[FK_CREATOR_OF_BILL]') AND parent_object_id = OBJECT_ID('tblBill'))
alter table tblBill  drop constraint FK_CREATOR_OF_BILL


    if exists (select 1 from sys.objects where object_id = OBJECT_ID(N'[FK_USERGROUPMEMBER_WITH_BILLS]') AND parent_object_id = OBJECT_ID('tblBillUserGroupDebitor'))
alter table tblBillUserGroupDebitor  drop constraint FK_USERGROUPMEMBER_WITH_BILLS


    if exists (select 1 from sys.objects where object_id = OBJECT_ID(N'[FK_BILL_WITH_GROUP_CREDITORS]') AND parent_object_id = OBJECT_ID('tblBillUserGroupDebitor'))
alter table tblBillUserGroupDebitor  drop constraint FK_BILL_WITH_GROUP_CREDITORS


    if exists (select 1 from sys.objects where object_id = OBJECT_ID(N'[FK_BOOKINGENTRY_BOOKING]') AND parent_object_id = OBJECT_ID('tblBookingEntry'))
alter table tblBookingEntry  drop constraint FK_BOOKINGENTRY_BOOKING


    if exists (select 1 from sys.objects where object_id = OBJECT_ID(N'[FK_ACCOUNT_WITH_BOOKING_ENTRIES]') AND parent_object_id = OBJECT_ID('tblBookingEntry'))
alter table tblBookingEntry  drop constraint FK_ACCOUNT_WITH_BOOKING_ENTRIES


    if exists (select 1 from sys.objects where object_id = OBJECT_ID(N'[FK_PAYMENT_SENDER]') AND parent_object_id = OBJECT_ID('tblPayment'))
alter table tblPayment  drop constraint FK_PAYMENT_SENDER


    if exists (select 1 from sys.objects where object_id = OBJECT_ID(N'[FK_PAYMENT_RECIPIENT]') AND parent_object_id = OBJECT_ID('tblPayment'))
alter table tblPayment  drop constraint FK_PAYMENT_RECIPIENT


    if exists (select 1 from sys.objects where object_id = OBJECT_ID(N'[FK_PAYMENT_CREATED_BY]') AND parent_object_id = OBJECT_ID('tblPayment'))
alter table tblPayment  drop constraint FK_PAYMENT_CREATED_BY


    if exists (select 1 from sys.objects where object_id = OBJECT_ID(N'[FK_PAYMENT_ACCEPTED_BY]') AND parent_object_id = OBJECT_ID('tblPayment'))
alter table tblPayment  drop constraint FK_PAYMENT_ACCEPTED_BY


    if exists (select 1 from sys.objects where object_id = OBJECT_ID(N'[FK_PAYMENT_DECLINED_BY]') AND parent_object_id = OBJECT_ID('tblPayment'))
alter table tblPayment  drop constraint FK_PAYMENT_DECLINED_BY


    if exists (select 1 from sys.objects where object_id = OBJECT_ID(N'[FK_PAYMENT_REQUEST_RECIPIENT]') AND parent_object_id = OBJECT_ID('tblPayment'))
alter table tblPayment  drop constraint FK_PAYMENT_REQUEST_RECIPIENT


    if exists (select 1 from sys.objects where object_id = OBJECT_ID(N'[FK_PAYMENT_REQUEST_SENDER]') AND parent_object_id = OBJECT_ID('tblPayment'))
alter table tblPayment  drop constraint FK_PAYMENT_REQUEST_SENDER


    if exists (select 1 from sys.objects where object_id = OBJECT_ID(N'[FK_PEANUT_USERGROUP]') AND parent_object_id = OBJECT_ID('tblPeanut'))
alter table tblPeanut  drop constraint FK_PEANUT_USERGROUP


    if exists (select 1 from sys.objects where object_id = OBJECT_ID(N'[FK_CREATOR_OF_PEANUT]') AND parent_object_id = OBJECT_ID('tblPeanut'))
alter table tblPeanut  drop constraint FK_CREATOR_OF_PEANUT


    if exists (select 1 from sys.objects where object_id = OBJECT_ID(N'[FK_DOCUMENT_OF_PEANUT]') AND parent_object_id = OBJECT_ID('tblPeanutDocuments'))
alter table tblPeanutDocuments  drop constraint FK_DOCUMENT_OF_PEANUT


    if exists (select 1 from sys.objects where object_id = OBJECT_ID(N'[FK_PEANUT_WITH_DOCUMENTS]') AND parent_object_id = OBJECT_ID('tblPeanutDocuments'))
alter table tblPeanutDocuments  drop constraint FK_PEANUT_WITH_DOCUMENTS


    if exists (select 1 from sys.objects where object_id = OBJECT_ID(N'[FK_PEANUT_REQUIREMENT]') AND parent_object_id = OBJECT_ID('tblPeanutRequirement'))
alter table tblPeanutRequirement  drop constraint FK_PEANUT_REQUIREMENT


    if exists (select 1 from sys.objects where object_id = OBJECT_ID(N'[FK_BILL_FOR_PEANUT]') AND parent_object_id = OBJECT_ID('tblBillForPeanut'))
alter table tblBillForPeanut  drop constraint FK_BILL_FOR_PEANUT


    if exists (select 1 from sys.objects where object_id = OBJECT_ID(N'[FK_PEANUT_WITH_BILLS]') AND parent_object_id = OBJECT_ID('tblBillForPeanut'))
alter table tblBillForPeanut  drop constraint FK_PEANUT_WITH_BILLS


    if exists (select 1 from sys.objects where object_id = OBJECT_ID(N'[FK_CREATOR_OF_PEANUTCOMMENT]') AND parent_object_id = OBJECT_ID('tblPeanutComment'))
alter table tblPeanutComment  drop constraint FK_CREATOR_OF_PEANUTCOMMENT


    if exists (select 1 from sys.objects where object_id = OBJECT_ID(N'[FK_PEANUT_WITH_COMMENTS]') AND parent_object_id = OBJECT_ID('tblPeanutComment'))
alter table tblPeanutComment  drop constraint FK_PEANUT_WITH_COMMENTS


    if exists (select 1 from sys.objects where object_id = OBJECT_ID(N'[FK_PEANUT_PARTICIPATION]') AND parent_object_id = OBJECT_ID('tblPeanutParticipation'))
alter table tblPeanutParticipation  drop constraint FK_PEANUT_PARTICIPATION


    if exists (select 1 from sys.objects where object_id = OBJECT_ID(N'[FK_PEANUT_PARTICIPATION_TYPE]') AND parent_object_id = OBJECT_ID('tblPeanutParticipation'))
alter table tblPeanutParticipation  drop constraint FK_PEANUT_PARTICIPATION_TYPE


    if exists (select 1 from sys.objects where object_id = OBJECT_ID(N'[FK_PEANUT_PARTICIPATION_MEMBERSHIP]') AND parent_object_id = OBJECT_ID('tblPeanutParticipation'))
alter table tblPeanutParticipation  drop constraint FK_PEANUT_PARTICIPATION_MEMBERSHIP


    if exists (select 1 from sys.objects where object_id = OBJECT_ID(N'[FK_CREATOR_OF_PEANUT_PARTICIPATION]') AND parent_object_id = OBJECT_ID('tblPeanutParticipation'))
alter table tblPeanutParticipation  drop constraint FK_CREATOR_OF_PEANUT_PARTICIPATION


    if exists (select 1 from sys.objects where object_id = OBJECT_ID(N'[FK_CREATOR_OF_PROPOSED_USER]') AND parent_object_id = OBJECT_ID('tblProposedUser'))
alter table tblProposedUser  drop constraint FK_CREATOR_OF_PROPOSED_USER


    if exists (select 1 from sys.objects where object_id = OBJECT_ID(N'[FK_USER_GROUP_CHANGED_BY_USER]') AND parent_object_id = OBJECT_ID('tblUserGroup'))
alter table tblUserGroup  drop constraint FK_USER_GROUP_CHANGED_BY_USER


    if exists (select 1 from sys.objects where object_id = OBJECT_ID(N'[FK_USER_GROUP_CREATED_BY_USER]') AND parent_object_id = OBJECT_ID('tblUserGroup'))
alter table tblUserGroup  drop constraint FK_USER_GROUP_CREATED_BY_USER


    if exists (select 1 from sys.objects where object_id = OBJECT_ID(N'[FK_USER_IN_GROUP]') AND parent_object_id = OBJECT_ID('tblUserGroupMembership'))
alter table tblUserGroupMembership  drop constraint FK_USER_IN_GROUP


    if exists (select 1 from sys.objects where object_id = OBJECT_ID(N'[FK_GROUP_WITH_USER]') AND parent_object_id = OBJECT_ID('tblUserGroupMembership'))
alter table tblUserGroupMembership  drop constraint FK_GROUP_WITH_USER


    if exists (select 1 from sys.objects where object_id = OBJECT_ID(N'[FK_MEMBERSHIP_ACCOUNT]') AND parent_object_id = OBJECT_ID('tblUserGroupMembership'))
alter table tblUserGroupMembership  drop constraint FK_MEMBERSHIP_ACCOUNT


    if exists (select 1 from sys.objects where object_id = OBJECT_ID(N'[FK_ROLE_TO_USER]') AND parent_object_id = OBJECT_ID('tblUserRoles'))
alter table tblUserRoles  drop constraint FK_ROLE_TO_USER


    if exists (select 1 from sys.objects where object_id = OBJECT_ID(N'[FK_USER_DOCUMENT]') AND parent_object_id = OBJECT_ID('tblUserDocument'))
alter table tblUserDocument  drop constraint FK_USER_DOCUMENT


    if exists (select 1 from sys.objects where object_id = OBJECT_ID(N'[FK_USER_WITH_DOCUMENTS]') AND parent_object_id = OBJECT_ID('tblUserDocument'))
alter table tblUserDocument  drop constraint FK_USER_WITH_DOCUMENTS


    if exists (select * from dbo.sysobjects where id = object_id(N'tblAccount') and OBJECTPROPERTY(id, N'IsUserTable') = 1) drop table tblAccount

    if exists (select * from dbo.sysobjects where id = object_id(N'tblBillGuestDebitor') and OBJECTPROPERTY(id, N'IsUserTable') = 1) drop table tblBillGuestDebitor

    if exists (select * from dbo.sysobjects where id = object_id(N'tblBill') and OBJECTPROPERTY(id, N'IsUserTable') = 1) drop table tblBill

    if exists (select * from dbo.sysobjects where id = object_id(N'tblBillUserGroupDebitor') and OBJECTPROPERTY(id, N'IsUserTable') = 1) drop table tblBillUserGroupDebitor

    if exists (select * from dbo.sysobjects where id = object_id(N'tblBookingEntry') and OBJECTPROPERTY(id, N'IsUserTable') = 1) drop table tblBookingEntry

    if exists (select * from dbo.sysobjects where id = object_id(N'tblBooking') and OBJECTPROPERTY(id, N'IsUserTable') = 1) drop table tblBooking

    if exists (select * from dbo.sysobjects where id = object_id(N'tblDocument') and OBJECTPROPERTY(id, N'IsUserTable') = 1) drop table tblDocument

    if exists (select * from dbo.sysobjects where id = object_id(N'tblPayment') and OBJECTPROPERTY(id, N'IsUserTable') = 1) drop table tblPayment

    if exists (select * from dbo.sysobjects where id = object_id(N'tblPeanut') and OBJECTPROPERTY(id, N'IsUserTable') = 1) drop table tblPeanut

    if exists (select * from dbo.sysobjects where id = object_id(N'tblPeanutDocuments') and OBJECTPROPERTY(id, N'IsUserTable') = 1) drop table tblPeanutDocuments

    if exists (select * from dbo.sysobjects where id = object_id(N'tblPeanutRequirement') and OBJECTPROPERTY(id, N'IsUserTable') = 1) drop table tblPeanutRequirement

    if exists (select * from dbo.sysobjects where id = object_id(N'tblBillForPeanut') and OBJECTPROPERTY(id, N'IsUserTable') = 1) drop table tblBillForPeanut

    if exists (select * from dbo.sysobjects where id = object_id(N'tblPeanutComment') and OBJECTPROPERTY(id, N'IsUserTable') = 1) drop table tblPeanutComment

    if exists (select * from dbo.sysobjects where id = object_id(N'tblPeanutParticipation') and OBJECTPROPERTY(id, N'IsUserTable') = 1) drop table tblPeanutParticipation

    if exists (select * from dbo.sysobjects where id = object_id(N'tblPeanutParticipationType') and OBJECTPROPERTY(id, N'IsUserTable') = 1) drop table tblPeanutParticipationType

    if exists (select * from dbo.sysobjects where id = object_id(N'tblProposedUser') and OBJECTPROPERTY(id, N'IsUserTable') = 1) drop table tblProposedUser

    if exists (select * from dbo.sysobjects where id = object_id(N'tblUserGroup') and OBJECTPROPERTY(id, N'IsUserTable') = 1) drop table tblUserGroup

    if exists (select * from dbo.sysobjects where id = object_id(N'tblUserGroupMembership') and OBJECTPROPERTY(id, N'IsUserTable') = 1) drop table tblUserGroupMembership

    if exists (select * from dbo.sysobjects where id = object_id(N'tblUser') and OBJECTPROPERTY(id, N'IsUserTable') = 1) drop table tblUser

    if exists (select * from dbo.sysobjects where id = object_id(N'tblUserRoles') and OBJECTPROPERTY(id, N'IsUserTable') = 1) drop table tblUserRoles

    if exists (select * from dbo.sysobjects where id = object_id(N'tblUserDocument') and OBJECTPROPERTY(id, N'IsUserTable') = 1) drop table tblUserDocument

    create table tblAccount (
        Id INT IDENTITY NOT NULL,
       BusinessId UNIQUEIDENTIFIER not null unique,
       Balance FLOAT(53) not null,
       primary key (Id)
    )

    create table tblBillGuestDebitor (
        Id INT IDENTITY NOT NULL,
       BusinessId UNIQUEIDENTIFIER not null unique,
       Name NVARCHAR(255) not null,
       Email NVARCHAR(255) not null,
       Portion FLOAT(53) not null,
       Bill_Id INT null,
       primary key (Id)
    )

    create table tblBill (
        Id INT IDENTITY NOT NULL,
       BusinessId UNIQUEIDENTIFIER not null unique,
       Amount FLOAT(53) not null,
       Subject NVARCHAR(254) not null,
       CreatedAt DATETIME not null,
       ChangedAt DATETIME null,
       IsSettled BIT not null,
       SettlementDate DATETIME null,
       UserGroup_Id INT not null,
       Creditor_Id INT null,
       CreatedBy_Id INT not null,
       ChangedBy_Id INT null,
       primary key (Id)
    )

    create table tblBillUserGroupDebitor (
        Id INT IDENTITY NOT NULL,
       BusinessId UNIQUEIDENTIFIER not null unique,
       BillAcceptState NVARCHAR(255) not null,
       Portion FLOAT(53) not null,
       RefuseComment NVARCHAR(1000) null,
       UserGroupMembership_Id INT not null,
       Bill_Id INT null,
       primary key (Id)
    )

    create table tblBookingEntry (
        Id INT IDENTITY NOT NULL,
       BookingEntryType nvarchar(100) not null,
       BusinessId UNIQUEIDENTIFIER not null unique,
       Booking_Id INT not null,
       Account_Id INT not null,
       primary key (Id),
      unique (BookingEntryType, Booking_Id)
    )

    create table tblBooking (
        Id INT IDENTITY NOT NULL,
       BusinessId UNIQUEIDENTIFIER not null unique,
       Amount FLOAT(53) not null,
       BookingDate DATETIME not null,
       BookingText NVARCHAR(1000) null,
       primary key (Id)
    )

    create table tblDocument (
        Id INT IDENTITY NOT NULL,
       BusinessId UNIQUEIDENTIFIER not null unique,
       ContentLength BIGINT not null,
       ContentType NVARCHAR(255) null,
       FileName NVARCHAR(255) not null,
       OriginalFileName NVARCHAR(255) not null,
       primary key (Id)
    )

    create table tblPayment (
        Id INT IDENTITY NOT NULL,
       BusinessId UNIQUEIDENTIFIER not null unique,
       CreatedAt DATETIME not null,
       Amount FLOAT(53) not null,
       PaymentStatus nvarchar(100) not null,
       PaymentType nvarchar(100) not null,
       Text NVARCHAR(1000) not null,
       AcceptedAt DATETIME null,
       DeclinedAt DATETIME null,
       DeclineReason NVARCHAR(1000) null,
       Sender_Id INT null,
       Recipient_Id INT null,
       CreatedBy_Id INT null,
       AcceptedBy_Id INT null,
       DeclinedBy_Id INT null,
       RequestRecipient_Id INT not null,
       RequestSender_Id INT not null,
       primary key (Id)
    )

    create table tblPeanut (
        Id INT IDENTITY NOT NULL,
       BusinessId UNIQUEIDENTIFIER not null unique,
       Name NVARCHAR(255) not null,
       Description NVARCHAR(4000) null,
       ExternalLinks NVARCHAR(4000) null,
       Day date not null,
       MaximumParticipations INT null,
       PeanutState NVARCHAR(255) not null,
       CreatedAt DATETIME not null,
       ChangedAt DATETIME null,
       UserGroup_Id INT not null,
       CreatedBy_Id INT not null,
       ChangedBy_Id INT null,
       primary key (Id)
    )

    create table tblPeanutDocuments (
        Peanut_Id INT not null,
       Document_Id INT not null
    )

    create table tblPeanutRequirement (
        Peanut_Id INT not null,
       Name NVARCHAR(100) not null,
       Quantity FLOAT(53) not null,
       Unit NVARCHAR(15) not null,
       Url NVARCHAR(255) null
    )

    create table tblBillForPeanut (
        Peanut_Id INT not null,
       Bill_Id INT not null
    )

    create table tblPeanutComment (
        Peanut_Id INT not null,
       Comment NVARCHAR(1000) null,
       CreatedAt DATETIME not null,
       ChangedAt DATETIME null,
       CreatedBy_Id INT not null,
       ChangedBy_Id INT null
    )

    create table tblPeanutParticipation (
        Id INT IDENTITY NOT NULL,
       BusinessId UNIQUEIDENTIFIER not null unique,
       ParticipationState NVARCHAR(255) not null,
       CreatedAt DATETIME not null,
       ChangedAt DATETIME null,
       Peanut_Id INT not null,
       ParticipationType_Id INT not null,
       UserGroupMembership_Id INT not null,
       CreatedBy_Id INT not null,
       ChangedBy_Id INT null,
       primary key (Id)
    )

    create table tblPeanutParticipationType (
        Id INT IDENTITY NOT NULL,
       BusinessId UNIQUEIDENTIFIER not null unique,
       Name NVARCHAR(255) not null,
       IsCreditor BIT not null,
       IsProducer BIT not null,
       MaxParticipatorsOfType INT null,
       CreatedAt DATETIME not null,
       ChangedAt DATETIME null,
       CreatedBy_Id INT null,
       ChangedBy_Id INT null,
       UserGroup_Id INT null,
       primary key (Id)
    )

    create table tblProposedUser (
        Id INT IDENTITY NOT NULL,
       BusinessId UNIQUEIDENTIFIER not null unique,
       Salutation NVARCHAR(255) not null,
       Title NVARCHAR(255) null,
       UserName NVARCHAR(255) not null unique,
       Email NVARCHAR(255) not null,
       FirstName NVARCHAR(255) not null,
       LastName NVARCHAR(255) not null,
       Url NVARCHAR(255) null,
       Company NVARCHAR(255) null,
       Street NVARCHAR(255) not null,
       StreetNumber NVARCHAR(20) not null,
       PostalCode NVARCHAR(10) not null,
       City NVARCHAR(255) not null,
       CountryTwoLetterIsoCode NVARCHAR(2) not null,
       Phone NVARCHAR(30) null,
       PhonePrivate NVARCHAR(30) null,
       Mobile NVARCHAR(30) null,
       Birthday date null,
       CreatedAt DATETIME not null,
       ChangedAt DATETIME null,
       CreatedBy_Id INT not null,
       ChangedBy_Id INT null,
       primary key (Id)
    )

    create table tblUserGroup (
        Id INT IDENTITY NOT NULL,
       BusinessId UNIQUEIDENTIFIER not null unique,
       AdditionalInformations NVARCHAR(4000) null,
       ChangedAt DATETIME null,
       CreatedAt DATETIME not null,
       Name NVARCHAR(255) not null,
       BalanceOverdraftLimit FLOAT(53) null,
       ChangedBy_Id INT null,
       CreatedBy_Id INT not null,
       primary key (Id)
    )

    create table tblUserGroupMembership (
        Id INT IDENTITY NOT NULL,
       BusinessId UNIQUEIDENTIFIER not null unique,
       AutoAcceptBills BIT not null,
       CreatedAt DATETIME not null,
       ChangedAt DATETIME null,
       MembershipType NVARCHAR(255) not null,
       User_Id INT null,
       UserGroup_Id INT null,
       CreatedBy_Id INT null,
       ChangedBy_Id INT null,
       Account_Id INT null,
       primary key (Id),
      unique (User_Id, UserGroup_Id)
    )

    create table tblUser (
        Id INT IDENTITY NOT NULL,
       BusinessId UNIQUEIDENTIFIER not null unique,
       UserName NVARCHAR(255) not null unique,
       Email NVARCHAR(255) null,
       PasswordHash NVARCHAR(255) null,
       FirstName NVARCHAR(255) null,
       LastName NVARCHAR(255) null,
       PasswordResetCode UNIQUEIDENTIFIER null,
       Url NVARCHAR(255) null,
       Company NVARCHAR(255) null,
       Street NVARCHAR(255) null,
       StreetNumber NVARCHAR(20) null,
       PostalCode NVARCHAR(10) null,
       City NVARCHAR(255) null,
       CountryTwoLetterIsoCode NVARCHAR(2) null,
       Phone NVARCHAR(30) null,
       PhonePrivate NVARCHAR(30) null,
       Mobile NVARCHAR(30) null,
       PayPalBusinessName NVARCHAR(255) null,
       AutoAcceptPayPalPayments BIT default 0  not null,
       NotifyMeAsCreditorOnPeanutDeleted BIT default 1  not null,
       NotifyMeAsCreditorOnPeanutRequirementsChanged BIT default 1  not null,
       NotifyMeAsParticipatorOnPeanutChanged BIT default 1  not null,
       NotifyMeAsCreditorOnDeclinedBills BIT default 1  not null,
       NotifyMeAsDebitorOnIncomingBills BIT default 1  not null,
       NotifyMeOnIncomingPayment BIT default 1  not null,
       NotifyMeAsCreditorOnSettleableBills BIT default 1  not null,
       NotifyMeOnPeanutInvitation BIT default 1  not null,
       SendMeWeeklySummaryAndForecast BIT default 1  not null,
       IsDeleted BIT default 0  not null,
       IsEnabled BIT not null,
       Birthday date null,
       CreatedAt DATETIME not null,
       ChangedAt DATETIME null,
       CreatedBy_Id INT null,
       ChangedBy_Id INT null,
       primary key (Id)
    )

    create table tblUserRoles (
        User_Id INT not null,
       Role NVARCHAR(255) null
    )

    create table tblUserDocument (
        User_Id INT not null,
       Document_Id INT not null
    )

    alter table tblBillGuestDebitor 
        add constraint FK_BILL_WITH_GUEST_CREDITORS 
        foreign key (Bill_Id) 
        references tblBill

    alter table tblBill 
        add constraint FK_USERGROUP_WITH_BILLS 
        foreign key (UserGroup_Id) 
        references tblUserGroup

    alter table tblBill 
        add constraint FK_CREDITOR_OF_BILL 
        foreign key (Creditor_Id) 
        references tblUserGroupMembership

    alter table tblBill 
        add constraint FK_CREATOR_OF_BILL 
        foreign key (CreatedBy_Id) 
        references tblUser

    alter table tblBillUserGroupDebitor 
        add constraint FK_USERGROUPMEMBER_WITH_BILLS 
        foreign key (UserGroupMembership_Id) 
        references tblUserGroupMembership

    alter table tblBillUserGroupDebitor 
        add constraint FK_BILL_WITH_GROUP_CREDITORS 
        foreign key (Bill_Id) 
        references tblBill

    alter table tblBookingEntry 
        add constraint FK_BOOKINGENTRY_BOOKING 
        foreign key (Booking_Id) 
        references tblBooking

    alter table tblBookingEntry 
        add constraint FK_ACCOUNT_WITH_BOOKING_ENTRIES 
        foreign key (Account_Id) 
        references tblAccount

    alter table tblPayment 
        add constraint FK_PAYMENT_SENDER 
        foreign key (Sender_Id) 
        references tblAccount

    alter table tblPayment 
        add constraint FK_PAYMENT_RECIPIENT 
        foreign key (Recipient_Id) 
        references tblAccount

    alter table tblPayment 
        add constraint FK_PAYMENT_CREATED_BY 
        foreign key (CreatedBy_Id) 
        references tblUser

    alter table tblPayment 
        add constraint FK_PAYMENT_ACCEPTED_BY 
        foreign key (AcceptedBy_Id) 
        references tblUser

    alter table tblPayment 
        add constraint FK_PAYMENT_DECLINED_BY 
        foreign key (DeclinedBy_Id) 
        references tblUser

    alter table tblPayment 
        add constraint FK_PAYMENT_REQUEST_RECIPIENT 
        foreign key (RequestRecipient_Id) 
        references tblUser

    alter table tblPayment 
        add constraint FK_PAYMENT_REQUEST_SENDER 
        foreign key (RequestSender_Id) 
        references tblUser

    alter table tblPeanut 
        add constraint FK_PEANUT_USERGROUP 
        foreign key (UserGroup_Id) 
        references tblUserGroup

    alter table tblPeanut 
        add constraint FK_CREATOR_OF_PEANUT 
        foreign key (CreatedBy_Id) 
        references tblUser

    alter table tblPeanutDocuments 
        add constraint FK_DOCUMENT_OF_PEANUT 
        foreign key (Document_Id) 
        references tblDocument

    alter table tblPeanutDocuments 
        add constraint FK_PEANUT_WITH_DOCUMENTS 
        foreign key (Peanut_Id) 
        references tblPeanut

    alter table tblPeanutRequirement 
        add constraint FK_PEANUT_REQUIREMENT 
        foreign key (Peanut_Id) 
        references tblPeanut

    alter table tblBillForPeanut 
        add constraint FK_BILL_FOR_PEANUT 
        foreign key (Bill_Id) 
        references tblBill

    alter table tblBillForPeanut 
        add constraint FK_PEANUT_WITH_BILLS 
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

    alter table tblPeanutParticipation 
        add constraint FK_PEANUT_PARTICIPATION 
        foreign key (Peanut_Id) 
        references tblPeanut

    alter table tblPeanutParticipation 
        add constraint FK_PEANUT_PARTICIPATION_TYPE 
        foreign key (ParticipationType_Id) 
        references tblPeanutParticipationType

    alter table tblPeanutParticipation 
        add constraint FK_PEANUT_PARTICIPATION_MEMBERSHIP 
        foreign key (UserGroupMembership_Id) 
        references tblUserGroupMembership

    alter table tblPeanutParticipation 
        add constraint FK_CREATOR_OF_PEANUT_PARTICIPATION 
        foreign key (CreatedBy_Id) 
        references tblUser

    alter table tblProposedUser 
        add constraint FK_CREATOR_OF_PROPOSED_USER 
        foreign key (CreatedBy_Id) 
        references tblUser

    alter table tblUserGroup 
        add constraint FK_USER_GROUP_CHANGED_BY_USER 
        foreign key (ChangedBy_Id) 
        references tblUser

    alter table tblUserGroup 
        add constraint FK_USER_GROUP_CREATED_BY_USER 
        foreign key (CreatedBy_Id) 
        references tblUser

    alter table tblUserGroupMembership 
        add constraint FK_USER_IN_GROUP 
        foreign key (User_Id) 
        references tblUser

    alter table tblUserGroupMembership 
        add constraint FK_GROUP_WITH_USER 
        foreign key (UserGroup_Id) 
        references tblUserGroup

    alter table tblUserGroupMembership 
        add constraint FK_MEMBERSHIP_ACCOUNT 
        foreign key (Account_Id) 
        references tblAccount

    alter table tblUserRoles 
        add constraint FK_ROLE_TO_USER 
        foreign key (User_Id) 
        references tblUser

    alter table tblUserDocument 
        add constraint FK_USER_DOCUMENT 
        foreign key (Document_Id) 
        references tblDocument

    alter table tblUserDocument 
        add constraint FK_USER_WITH_DOCUMENTS 
        foreign key (User_Id) 
        references tblUser
er table tblUserRoles 
        add constraint FK_ROLE_TO_USER 
        foreign key (User_Id) 
        references tblUser

    alter table tblUserDocument 
        add constraint FK_USER_DOCUMENT 
        foreign key (Document_Id) 
        references tblDocument

    alter table tblUserDocument 
        add constraint FK_USER_WITH_DOCUMENTS 
        foreign key (User_Id) 
        references tblUser
add constraint FK_ROLE_TO_USER 
        foreign key (User_Id) 
        references tblUser

    alter table tblUserDocument 
        add constraint FK_USER_DOCUMENT 
        foreign key (Document_Id) 
        references tblDocument

    alter table tblUserDocument 
        add constraint FK_USER_WITH_DOCUMENTS 
        foreign key (User_Id) 
        references tblUser
User_Id) 
        references tblUser

        references tblAccount

    alter table tblUserRoles 
        add constraint FK_ROLE_TO_USER 
        foreign key (User_Id) 
        references tblUser

    alter table tblUserDocument 
        add constraint FK_USER_DOCUMENT 
        foreign key (Document_Id) 
        references tblDocument

    alter table tblUserDocument 
        add constraint FK_USER_WITH_DOCUMENTS 
        foreign key (User_Id) 
        references tblUser

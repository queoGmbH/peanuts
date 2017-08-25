Alter table tblUser add 
	NotifyMeAsCreditorOnPeanutDeleted             BIT not null CONSTRAINT DF_USER_NOTIFY_CreditorOnPeanutDeleted             DEFAULT 1,
    NotifyMeAsCreditorOnPeanutRequirementsChanged BIT not null CONSTRAINT DF_USER_NOTIFY_CreditorOnPeanutRequirementsChanged DEFAULT 1,
    NotifyMeAsParticipatorOnPeanutChanged         BIT not null CONSTRAINT DF_USER_NOTIFY_ParticipatorOnPeanutChanged         DEFAULT 1,
    NotifyMeAsCreditorOnDeclinedBills             BIT not null CONSTRAINT DF_USER_NOTIFY_CreditorOnDeclinedBills             DEFAULT 1,
    NotifyMeAsDebitorOnIncomingBills              BIT not null CONSTRAINT DF_USER_NOTIFY_DebitorOnIncomingBills              DEFAULT 1,
    NotifyMeOnIncomingPayment                     BIT not null CONSTRAINT DF_USER_NOTIFY_OnIncomingPayment                   DEFAULT 1,
    NotifyMeAsCreditorOnSettleableBills           BIT not null CONSTRAINT DF_USER_NOTIFY_CreditorOnSettleableBills           DEFAULT 1,
	NotifyMeOnPeanutInvitation                    BIT not null CONSTRAINT DF_USER_NOTIFY_NotifyMeOnPeanutInvitation          DEFAULT 1,
    SendMeWeeklySummaryAndForecast                BIT not null CONSTRAINT DF_USER_SEND_WeeklySummaryAndForecast              DEFAULT 1;


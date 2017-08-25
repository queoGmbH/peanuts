SET IDENTITY_INSERT tblPeanutParticipationType ON;

INSERT INTO tblPeanutParticipationType

([Id], [BusinessId]                            , [Name]                     , [IsCreditor] , [IsProducer] , [MaxParticipatorsOfType], [CreatedAt]           , [CreatedBy_Id])     VALUES
( 1  , '{C5096CFB-B1BC-4308-B929-A9EC20C3A5CD}', 'Koch'                     , 0            , 1            , 1                       , '2017-04-04 08:00:00' , 2             ),
( 2  , '{CC920918-4674-40EF-9341-EE88F99ECA07}', 'Einkäufer'                , 1            , 0            , 2                       , '2017-04-04 08:00:00' , 2             ),
( 3  , '{660B58C7-D72E-49C8-9120-4A283E7226BA}', 'Koch und Einkäufer'       , 1            , 1            , 1                       , '2017-04-04 08:00:00' , 2             ),
( 4  , '{C88C07E9-DA74-4250-99E4-5E58B15889E8}', 'Teilnehmer'               , 0            , 0            , null                    , '2017-04-04 08:00:00' , 2             ),
( 5  , '{8190AB32-29FC-4D9A-9D4F-D32DDC317A82}', 'Hilfskoch/Schnibbelhilfe' , 0            , 0            , 2                       , '2017-04-04 08:00:00' , 2             ),
( 6  , '{B79AC27E-AD70-4BD6-B6CF-FB712F8A3410}', 'Abwascher'                , 0            , 0            , 2                       , '2017-04-04 08:00:00' , 2             );

SET IDENTITY_INSERT tblPeanutParticipationType OFF;
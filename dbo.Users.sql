CREATE TABLE [dbo].[Users] (
    [UserID]          INT              IDENTITY (1, 1) NOT NULL,
    [FirstName]       VARCHAR (50)     NOT NULL,
    [LastName]        VARCHAR (50)     NOT NULL,
    [Email]           VARCHAR (50)     NOT NULL,
    [DOB]             DATETIME         NULL,
    [Password]        VARCHAR (50)     NOT NULL,
    [IsEmailVerified] BIT       NOT NULL,
    [ActivationCode]  UNIQUEIDENTIFIER NOT NULL,
    PRIMARY KEY CLUSTERED ([UserID] ASC)
);


﻿CREATE TABLE [dbo].[Application]
(
	[Id]					INT				IDENTITY (1, 1) NOT NULL,
    [EmployerAccountId]		BIGINT			NOT NULL,
	[PledgeId]				INT				NOT NULL,
	[Details]				NVARCHAR(MAX)	NOT NULL,
	[NumberOfApprentices]	INT				NOT NULL,
	[StandardId]			VARCHAR(20)		NOT NULL,
	[StartDate]				DATETIME2		NOT NULL,
	[Amount]				INT				NOT NULL,
	[HasTrainingProvider]	BIT				NOT NULL,
	[Sectors]				INT		        NOT NULL,
	[PostCode]				VARCHAR(8)		NOT NULL,
	[FirstName]				NVARCHAR(25)	NOT NULL,
	[LastName]				NVARCHAR(25)	NOT NULL,
	[BusinessWebsite]		NVARCHAR(75)	NOT NULL,
	[CreatedOn]				DATETIME2 (7) CONSTRAINT [DF_Application__CreationDate] DEFAULT (getdate()) NOT NULL,
	[RowVersion]			TIMESTAMP     NOT NULL,
    CONSTRAINT [PK_Application] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Application_EmployerAccount] FOREIGN KEY ([EmployerAccountId]) REFERENCES [dbo].[EmployerAccount] ([Id]),
	CONSTRAINT [FK_Application_Pledge] FOREIGN KEY ([PledgeId]) REFERENCES [dbo].[Pledge] ([Id])
)


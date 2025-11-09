IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;
GO

BEGIN TRANSACTION;
IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251109065003_Initial'
)
BEGIN
    IF SCHEMA_ID(N'masstransit') IS NULL EXEC(N'CREATE SCHEMA [masstransit];');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251109065003_Initial'
)
BEGIN
    CREATE TABLE [masstransit].[InboxState] (
        [Id] bigint NOT NULL IDENTITY,
        [MessageId] uniqueidentifier NOT NULL,
        [ConsumerId] uniqueidentifier NOT NULL,
        [LockId] uniqueidentifier NOT NULL,
        [RowVersion] rowversion NULL,
        [Received] datetime2 NOT NULL,
        [ReceiveCount] int NOT NULL,
        [ExpirationTime] datetime2 NULL,
        [Consumed] datetime2 NULL,
        [Delivered] datetime2 NULL,
        [LastSequenceNumber] bigint NULL,
        CONSTRAINT [PK_InboxState] PRIMARY KEY ([Id]),
        CONSTRAINT [AK_InboxState_MessageId_ConsumerId] UNIQUE ([MessageId], [ConsumerId])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251109065003_Initial'
)
BEGIN
    CREATE TABLE [masstransit].[OutboxState] (
        [OutboxId] uniqueidentifier NOT NULL,
        [LockId] uniqueidentifier NOT NULL,
        [RowVersion] rowversion NULL,
        [Created] datetime2 NOT NULL,
        [Delivered] datetime2 NULL,
        [LastSequenceNumber] bigint NULL,
        CONSTRAINT [PK_OutboxState] PRIMARY KEY ([OutboxId])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251109065003_Initial'
)
BEGIN
    CREATE TABLE [masstransit].[OutboxMessage] (
        [SequenceNumber] bigint NOT NULL IDENTITY,
        [EnqueueTime] datetime2 NULL,
        [SentTime] datetime2 NOT NULL,
        [Headers] nvarchar(max) NULL,
        [Properties] nvarchar(max) NULL,
        [InboxMessageId] uniqueidentifier NULL,
        [InboxConsumerId] uniqueidentifier NULL,
        [OutboxId] uniqueidentifier NULL,
        [MessageId] uniqueidentifier NOT NULL,
        [ContentType] nvarchar(256) NOT NULL,
        [MessageType] nvarchar(max) NOT NULL,
        [Body] nvarchar(max) NOT NULL,
        [ConversationId] uniqueidentifier NULL,
        [CorrelationId] uniqueidentifier NULL,
        [InitiatorId] uniqueidentifier NULL,
        [RequestId] uniqueidentifier NULL,
        [SourceAddress] nvarchar(256) NULL,
        [DestinationAddress] nvarchar(256) NULL,
        [ResponseAddress] nvarchar(256) NULL,
        [FaultAddress] nvarchar(256) NULL,
        [ExpirationTime] datetime2 NULL,
        CONSTRAINT [PK_OutboxMessage] PRIMARY KEY ([SequenceNumber]),
        CONSTRAINT [FK_OutboxMessage_InboxState_InboxMessageId_InboxConsumerId] FOREIGN KEY ([InboxMessageId], [InboxConsumerId]) REFERENCES [masstransit].[InboxState] ([MessageId], [ConsumerId]),
        CONSTRAINT [FK_OutboxMessage_OutboxState_OutboxId] FOREIGN KEY ([OutboxId]) REFERENCES [masstransit].[OutboxState] ([OutboxId])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251109065003_Initial'
)
BEGIN
    CREATE INDEX [IX_InboxState_Delivered] ON [masstransit].[InboxState] ([Delivered]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251109065003_Initial'
)
BEGIN
    CREATE INDEX [IX_OutboxMessage_EnqueueTime] ON [masstransit].[OutboxMessage] ([EnqueueTime]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251109065003_Initial'
)
BEGIN
    CREATE INDEX [IX_OutboxMessage_ExpirationTime] ON [masstransit].[OutboxMessage] ([ExpirationTime]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251109065003_Initial'
)
BEGIN
    EXEC(N'CREATE UNIQUE INDEX [IX_OutboxMessage_InboxMessageId_InboxConsumerId_SequenceNumber] ON [masstransit].[OutboxMessage] ([InboxMessageId], [InboxConsumerId], [SequenceNumber]) WHERE [InboxMessageId] IS NOT NULL AND [InboxConsumerId] IS NOT NULL');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251109065003_Initial'
)
BEGIN
    EXEC(N'CREATE UNIQUE INDEX [IX_OutboxMessage_OutboxId_SequenceNumber] ON [masstransit].[OutboxMessage] ([OutboxId], [SequenceNumber]) WHERE [OutboxId] IS NOT NULL');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251109065003_Initial'
)
BEGIN
    CREATE INDEX [IX_OutboxState_Created] ON [masstransit].[OutboxState] ([Created]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251109065003_Initial'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20251109065003_Initial', N'9.0.10');
END;

COMMIT;
GO


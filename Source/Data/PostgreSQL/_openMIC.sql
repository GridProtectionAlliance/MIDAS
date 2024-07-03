-- *******************************************************************************************
-- IMPORTANT NOTE: When making updates to this schema, please increment the version number!
-- *******************************************************************************************
CREATE VIEW LocalSchemaVersion AS
SELECT 7 AS VersionNumber;

CREATE TABLE Setting(
    ID SERIAL NOT NULL PRIMARY KEY,
    Name VARCHAR(200) NULL,
    Value TEXT NULL,
    DefaultValue TEXT NULL,
	Description TEXT NULL
);

CREATE TABLE ConnectionProfileTaskQueue(
    ID SERIAL NOT NULL PRIMARY KEY,
    Name VARCHAR(200) NOT NULL,
    MaxThreadCount INTEGER NOT NULL DEFAULT 0,
    UseBackgroundThreads INTEGER NOT NULL DEFAULT 0,
    Description TEXT NULL,
    CreatedOn TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    CreatedBy VARCHAR(200) NOT NULL DEFAULT '',
    UpdatedOn TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    UpdatedBy VARCHAR(200) NOT NULL DEFAULT ''
);

CREATE TABLE ConnectionProfile(
    ID SERIAL NOT NULL PRIMARY KEY,
    Name VARCHAR(200) NOT NULL,
    DefaultTaskQueueID INTEGER NULL,
    Description TEXT NULL,
    CreatedOn TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    CreatedBy VARCHAR(200) NOT NULL DEFAULT '',
    UpdatedOn TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    UpdatedBy VARCHAR(200) NOT NULL DEFAULT '',
    CONSTRAINT FK_ConnectionProfile_ConnectionProfileTaskQueue FOREIGN KEY(DefaultTaskQueueID) REFERENCES ConnectionProfileTaskQueue (ID)
);

CREATE TABLE ConnectionProfileTask(
    ID SERIAL NOT NULL PRIMARY KEY,
    ConnectionProfileID INTEGER NOT NULL DEFAULT 1,
    Name VARCHAR(200) NOT NULL,
    Settings TEXT NULL,
    LoadOrder INTEGER NOT NULL DEFAULT 0,
    CreatedOn TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    CreatedBy VARCHAR(200) NOT NULL DEFAULT '',
    UpdatedOn TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    UpdatedBy VARCHAR(200) NOT NULL DEFAULT '',
    CONSTRAINT FK_ConnectionProfileTask_ConnectionProfile FOREIGN KEY(ConnectionProfileID) REFERENCES ConnectionProfile (ID)
);

CREATE TABLE OutputMirror(
    ID SERIAL NOT NULL PRIMARY KEY,
    Name VARCHAR(200) NOT NULL,
    Source TEXT NOT NULL,
    ConnectionType VARCHAR(200) NOT NULL,
    Settings TEXT NULL,
    LoadOrder INTEGER NOT NULL DEFAULT 0,
    CreatedOn DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
    CreatedBy VARCHAR(200) NOT NULL DEFAULT '',
    UpdatedOn DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
    UpdatedBy VARCHAR(200) NOT NULL DEFAULT ''
);

CREATE TABLE DownloadedFile(
    ID SERIAL NOT NULL PRIMARY KEY,
    DeviceID INTEGER NOT NULL,
    FilePath VARCHAR(200) NOT NULL,
    Timestamp TIMESTAMP NOT NULL,
    CreationTime TIMESTAMP NOT NULL,
    LastWriteTime TIMESTAMP NOT NULL,
    LastAccessTime TIMESTAMP NOT NULL,
    FileSize INTEGER NOT NULL
 );

CREATE TABLE StatusLog(		
    ID SERIAL NOT NULL PRIMARY KEY,
    DeviceID INTEGER NOT NULL,
    LastDownloadedFileID INTEGER NULL,
    LastOutcome VARCHAR(50) NULL,
    LastRun TIMESTAMP NULL,
    LastFailure TIMESTAMP NULL,
    LastErrorMessage TEXT NULL,
    LastDownloadStartTime TIMESTAMP NULL,
    LastDownloadEndTime TIMESTAMP NULL,
    LastDownloadFileCount INTEGER NULL,
    CONSTRAINT IX_StatusLog_DeviceID UNIQUE (DeviceID)
);

 CREATE TABLE SentEmail(
    ID SERIAL NOT NULL PRIMARY KEY,
    DeviceID INTEGER NOT NULL,
    Message TEXT NOT NULL,
    Timestamp TIMESTAMP NOT NULL
 );

CREATE TABLE IONWaveformCheckpoint(
    ID SERIAL NOT NULL PRIMARY KEY,
    Device VARCHAR(200) NOT NULL,
    TimeRecorded TIMESTAMP NOT NULL,
    LogPositions VARCHAR(MAX) NOT NULL DEFAULT '[]',
    CONSTRAINT IX_IONWaveformCheckpoint_Device UNIQUE (Device ASC, TimeRecorded ASC)
);

CREATE TABLE IONTrendingCheckpoint(
    ID SERIAL NOT NULL PRIMARY KEY,
    Device VARCHAR(200) NOT NULL,
    TimeRecorded TIMESTAMP NOT NULL,
    LogPositions VARCHAR(MAX) NOT NULL DEFAULT '[]',
    CONSTRAINT IX_IONTrendingCheckpoint_Device UNIQUE (Device ASC, TimeRecorded ASC)
);

CREATE TABLE DranetzWaveformCheckpoint(
    ID SERIAL NOT NULL PRIMARY KEY,
    Device VARCHAR(200) NOT NULL,
    TimeRecorded TIMESTAMP NOT NULL,
    CONSTRAINT IX_DranetzWaveformCheckpoint_Device UNIQUE (Device ASC)
);

CREATE TABLE DranetzTrendingCheckpoint(
    ID SERIAL NOT NULL PRIMARY KEY,
    Device VARCHAR(200) NOT NULL,
    TimeRecorded TIMESTAMP NOT NULL,
    CONSTRAINT IX_DranetzTrendingCheckpoint_Device UNIQUE (Device ASC)
);

DROP VIEW TrackedTable;

CREATE VIEW TrackedTable AS
SELECT 'Measurement' AS Name  WHERE 1 < 0;

DROP TRIGGER Company_UpdateTracker;
DROP TRIGGER Device_InsertTracker;
DROP TRIGGER Device_UpdateTracker1;
DROP TRIGGER Device_UpdateTracker2;
DROP TRIGGER Device_DeleteTracker;
DROP TRIGGER Historian_UpdateTracker;
DROP TRIGGER Measurement_InsertTracker;
DROP TRIGGER Measurement_UpdateTracker1;
DROP TRIGGER Measurement_UpdateTracker2;
DROP TRIGGER Measurement_DeleteTracker;
DROP TRIGGER OutputStream_InsertTracker;
DROP TRIGGER OutputStream_UpdateTracker;
DROP TRIGGER OutputStream_DeleteTracker;
DROP TRIGGER OutputStreamDevice_InsertTracker;
DROP TRIGGER OutputStreamDevice_UpdateTracker;
DROP TRIGGER OutputStreamDevice_DeleteTracker;
DROP TRIGGER OutputStreamMeasurement_InsertTracker;
DROP TRIGGER OutputStreamMeasurement_UpdateTracker;
DROP TRIGGER OutputStreamMeasurement_DeleteTracker;
DROP TRIGGER Phasor_UpdateTracker1;
DROP TRIGGER Phasor_UpdateTracker2;
DROP TRIGGER Protocol_UpdateTracker;
DROP TRIGGER SignalType_UpdateTracker;

CREATE INDEX IX_DownloadedFile_DeviceID ON DownloadedFile (DeviceID);
CREATE INDEX IX_DownloadedFile_FilePath ON DownloadedFile (FilePath);
CREATE INDEX IX_SentEmail_DeviceID ON SentEmail (DeviceID);
CREATE INDEX IX_SentEmail_Timestamp ON SentEmail (Timestamp);


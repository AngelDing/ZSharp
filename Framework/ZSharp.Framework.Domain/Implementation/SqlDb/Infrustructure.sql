USE [master]
GO
/****** Object:  Database [ConferenceDb]    Script Date: 09/08/2015 11:12:49 ******/
IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = N'ConferenceDb')
BEGIN
CREATE DATABASE [ConferenceDb] ON  PRIMARY 
( NAME = N'ConferenceDb', FILENAME = N'E:\DB\ConferenceDb.mdf' , SIZE = 6400KB , MAXSIZE = UNLIMITED, FILEGROWTH = 1024KB )
 LOG ON 
( NAME = N'ConferenceDb_log', FILENAME = N'E:\DB\ConferenceDb_log.ldf' , SIZE = 4288KB , MAXSIZE = 2048GB , FILEGROWTH = 10%)
END
GO
ALTER DATABASE [ConferenceDb] SET COMPATIBILITY_LEVEL = 100
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [ConferenceDb].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [ConferenceDb] SET ANSI_NULL_DEFAULT OFF
GO
ALTER DATABASE [ConferenceDb] SET ANSI_NULLS OFF
GO
ALTER DATABASE [ConferenceDb] SET ANSI_PADDING OFF
GO
ALTER DATABASE [ConferenceDb] SET ANSI_WARNINGS OFF
GO
ALTER DATABASE [ConferenceDb] SET ARITHABORT OFF
GO
ALTER DATABASE [ConferenceDb] SET AUTO_CLOSE OFF
GO
ALTER DATABASE [ConferenceDb] SET AUTO_CREATE_STATISTICS ON
GO
ALTER DATABASE [ConferenceDb] SET AUTO_SHRINK OFF
GO
ALTER DATABASE [ConferenceDb] SET AUTO_UPDATE_STATISTICS ON
GO
ALTER DATABASE [ConferenceDb] SET CURSOR_CLOSE_ON_COMMIT OFF
GO
ALTER DATABASE [ConferenceDb] SET CURSOR_DEFAULT  GLOBAL
GO
ALTER DATABASE [ConferenceDb] SET CONCAT_NULL_YIELDS_NULL OFF
GO
ALTER DATABASE [ConferenceDb] SET NUMERIC_ROUNDABORT OFF
GO
ALTER DATABASE [ConferenceDb] SET QUOTED_IDENTIFIER OFF
GO
ALTER DATABASE [ConferenceDb] SET RECURSIVE_TRIGGERS OFF
GO
ALTER DATABASE [ConferenceDb] SET  DISABLE_BROKER
GO
ALTER DATABASE [ConferenceDb] SET AUTO_UPDATE_STATISTICS_ASYNC OFF
GO
ALTER DATABASE [ConferenceDb] SET DATE_CORRELATION_OPTIMIZATION OFF
GO
ALTER DATABASE [ConferenceDb] SET TRUSTWORTHY OFF
GO
ALTER DATABASE [ConferenceDb] SET ALLOW_SNAPSHOT_ISOLATION OFF
GO
ALTER DATABASE [ConferenceDb] SET PARAMETERIZATION SIMPLE
GO
ALTER DATABASE [ConferenceDb] SET READ_COMMITTED_SNAPSHOT ON
GO
ALTER DATABASE [ConferenceDb] SET HONOR_BROKER_PRIORITY OFF
GO
ALTER DATABASE [ConferenceDb] SET  READ_WRITE
GO
ALTER DATABASE [ConferenceDb] SET RECOVERY FULL
GO
ALTER DATABASE [ConferenceDb] SET  MULTI_USER
GO
ALTER DATABASE [ConferenceDb] SET PAGE_VERIFY CHECKSUM
GO
ALTER DATABASE [ConferenceDb] SET DB_CHAINING OFF
GO
EXEC sys.sp_db_vardecimal_storage_format N'ConferenceDb', N'ON'
GO
USE [ConferenceDb]
GO
/****** Object:  ForeignKey [FK_ConferenceManagement.SeatTypes_ConferenceManagement.Conferences_ConferenceId]    Script Date: 09/08/2015 11:12:51 ******/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[ConferenceManagement].[FK_ConferenceManagement.SeatTypes_ConferenceManagement.Conferences_ConferenceId]') AND parent_object_id = OBJECT_ID(N'[ConferenceManagement].[SeatTypes]'))
ALTER TABLE [ConferenceManagement].[SeatTypes] DROP CONSTRAINT [FK_ConferenceManagement.SeatTypes_ConferenceManagement.Conferences_ConferenceId]
GO
/****** Object:  ForeignKey [FK_ConferencePayments.PaymentItem_ConferencePayments.Payment_Payment_Id]    Script Date: 09/08/2015 11:12:52 ******/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[ConferencePayments].[FK_ConferencePayments.PaymentItem_ConferencePayments.Payment_Payment_Id]') AND parent_object_id = OBJECT_ID(N'[ConferencePayments].[PaymentItem]'))
ALTER TABLE [ConferencePayments].[PaymentItem] DROP CONSTRAINT [FK_ConferencePayments.PaymentItem_ConferencePayments.Payment_Payment_Id]
GO
/****** Object:  ForeignKey [FK_ConferenceManagement.OrderSeats_ConferenceManagement.SeatTypes_SeatInfoId]    Script Date: 09/08/2015 11:12:52 ******/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[ConferenceManagement].[FK_ConferenceManagement.OrderSeats_ConferenceManagement.SeatTypes_SeatInfoId]') AND parent_object_id = OBJECT_ID(N'[ConferenceManagement].[OrderSeats]'))
ALTER TABLE [ConferenceManagement].[OrderSeats] DROP CONSTRAINT [FK_ConferenceManagement.OrderSeats_ConferenceManagement.SeatTypes_SeatInfoId]
GO
/****** Object:  Table [ConferenceManagement].[OrderSeats]    Script Date: 09/08/2015 11:12:52 ******/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[ConferenceManagement].[FK_ConferenceManagement.OrderSeats_ConferenceManagement.SeatTypes_SeatInfoId]') AND parent_object_id = OBJECT_ID(N'[ConferenceManagement].[OrderSeats]'))
ALTER TABLE [ConferenceManagement].[OrderSeats] DROP CONSTRAINT [FK_ConferenceManagement.OrderSeats_ConferenceManagement.SeatTypes_SeatInfoId]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[ConferenceManagement].[OrderSeats]') AND type in (N'U'))
DROP TABLE [ConferenceManagement].[OrderSeats]
GO
/****** Object:  Table [ConferencePayments].[PaymentItem]    Script Date: 09/08/2015 11:12:51 ******/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[ConferencePayments].[FK_ConferencePayments.PaymentItem_ConferencePayments.Payment_Payment_Id]') AND parent_object_id = OBJECT_ID(N'[ConferencePayments].[PaymentItem]'))
ALTER TABLE [ConferencePayments].[PaymentItem] DROP CONSTRAINT [FK_ConferencePayments.PaymentItem_ConferencePayments.Payment_Payment_Id]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[ConferencePayments].[PaymentItem]') AND type in (N'U'))
DROP TABLE [ConferencePayments].[PaymentItem]
GO
/****** Object:  Table [ConferenceManagement].[SeatTypes]    Script Date: 09/08/2015 11:12:51 ******/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[ConferenceManagement].[FK_ConferenceManagement.SeatTypes_ConferenceManagement.Conferences_ConferenceId]') AND parent_object_id = OBJECT_ID(N'[ConferenceManagement].[SeatTypes]'))
ALTER TABLE [ConferenceManagement].[SeatTypes] DROP CONSTRAINT [FK_ConferenceManagement.SeatTypes_ConferenceManagement.Conferences_ConferenceId]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[ConferenceManagement].[SeatTypes]') AND type in (N'U'))
DROP TABLE [ConferenceManagement].[SeatTypes]
GO
/****** Object:  Table [ConferenceRegistrationProcesses].[UndispatchedMessages]    Script Date: 09/08/2015 11:12:51 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[ConferenceRegistrationProcesses].[UndispatchedMessages]') AND type in (N'U'))
DROP TABLE [ConferenceRegistrationProcesses].[UndispatchedMessages]
GO
/****** Object:  Table [ConferenceRegistrationProcesses].[RegistrationProcess]    Script Date: 09/08/2015 11:12:51 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[ConferenceRegistrationProcesses].[RegistrationProcess]') AND type in (N'U'))
DROP TABLE [ConferenceRegistrationProcesses].[RegistrationProcess]
GO
/****** Object:  Table [ConferencePayments].[Payment]    Script Date: 09/08/2015 11:12:51 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[ConferencePayments].[Payment]') AND type in (N'U'))
DROP TABLE [ConferencePayments].[Payment]
GO
/****** Object:  Table [dbo].[__MigrationHistory]    Script Date: 09/08/2015 11:12:51 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[__MigrationHistory]') AND type in (N'U'))
DROP TABLE [dbo].[__MigrationHistory]
GO
/****** Object:  Table [SqlBus].[Commands]    Script Date: 09/08/2015 11:12:51 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[SqlBus].[Commands]') AND type in (N'U'))
DROP TABLE [SqlBus].[Commands]
GO
/****** Object:  Table [ConferenceManagement].[Conferences]    Script Date: 09/08/2015 11:12:51 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[ConferenceManagement].[Conferences]') AND type in (N'U'))
DROP TABLE [ConferenceManagement].[Conferences]
GO
/****** Object:  Table [Events].[Events]    Script Date: 09/08/2015 11:12:51 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[Events].[Events]') AND type in (N'U'))
DROP TABLE [Events].[Events]
GO
/****** Object:  Table [SqlBus].[Events]    Script Date: 09/08/2015 11:12:51 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[SqlBus].[Events]') AND type in (N'U'))
DROP TABLE [SqlBus].[Events]
GO
/****** Object:  Table [MessageLog].[Messages]    Script Date: 09/08/2015 11:12:51 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[MessageLog].[Messages]') AND type in (N'U'))
DROP TABLE [MessageLog].[Messages]
GO
/****** Object:  Table [ConferenceManagement].[Orders]    Script Date: 09/08/2015 11:12:51 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[ConferenceManagement].[Orders]') AND type in (N'U'))
DROP TABLE [ConferenceManagement].[Orders]
GO
/****** Object:  Schema [ConferenceManagement]    Script Date: 09/08/2015 11:12:49 ******/
IF  EXISTS (SELECT * FROM sys.schemas WHERE name = N'ConferenceManagement')
DROP SCHEMA [ConferenceManagement]
GO
/****** Object:  Schema [ConferencePayments]    Script Date: 09/08/2015 11:12:49 ******/
IF  EXISTS (SELECT * FROM sys.schemas WHERE name = N'ConferencePayments')
DROP SCHEMA [ConferencePayments]
GO
/****** Object:  Schema [ConferenceRegistrationProcesses]    Script Date: 09/08/2015 11:12:49 ******/
IF  EXISTS (SELECT * FROM sys.schemas WHERE name = N'ConferenceRegistrationProcesses')
DROP SCHEMA [ConferenceRegistrationProcesses]
GO
/****** Object:  Schema [Events]    Script Date: 09/08/2015 11:12:49 ******/
IF  EXISTS (SELECT * FROM sys.schemas WHERE name = N'Events')
DROP SCHEMA [Events]
GO
/****** Object:  Schema [MessageLog]    Script Date: 09/08/2015 11:12:49 ******/
IF  EXISTS (SELECT * FROM sys.schemas WHERE name = N'MessageLog')
DROP SCHEMA [MessageLog]
GO
/****** Object:  Schema [SqlBus]    Script Date: 09/08/2015 11:12:49 ******/
IF  EXISTS (SELECT * FROM sys.schemas WHERE name = N'SqlBus')
DROP SCHEMA [SqlBus]
GO
/****** Object:  Schema [SqlBus]    Script Date: 09/08/2015 11:12:49 ******/
IF NOT EXISTS (SELECT * FROM sys.schemas WHERE name = N'SqlBus')
EXEC sys.sp_executesql N'CREATE SCHEMA [SqlBus] AUTHORIZATION [dbo]'
GO
/****** Object:  Schema [MessageLog]    Script Date: 09/08/2015 11:12:49 ******/
IF NOT EXISTS (SELECT * FROM sys.schemas WHERE name = N'MessageLog')
EXEC sys.sp_executesql N'CREATE SCHEMA [MessageLog] AUTHORIZATION [dbo]'
GO
/****** Object:  Schema [Events]    Script Date: 09/08/2015 11:12:49 ******/
IF NOT EXISTS (SELECT * FROM sys.schemas WHERE name = N'Events')
EXEC sys.sp_executesql N'CREATE SCHEMA [Events] AUTHORIZATION [dbo]'
GO
/****** Object:  Schema [ConferenceRegistrationProcesses]    Script Date: 09/08/2015 11:12:49 ******/
IF NOT EXISTS (SELECT * FROM sys.schemas WHERE name = N'ConferenceRegistrationProcesses')
EXEC sys.sp_executesql N'CREATE SCHEMA [ConferenceRegistrationProcesses] AUTHORIZATION [dbo]'
GO
/****** Object:  Schema [ConferencePayments]    Script Date: 09/08/2015 11:12:49 ******/
IF NOT EXISTS (SELECT * FROM sys.schemas WHERE name = N'ConferencePayments')
EXEC sys.sp_executesql N'CREATE SCHEMA [ConferencePayments] AUTHORIZATION [dbo]'
GO
/****** Object:  Schema [ConferenceManagement]    Script Date: 09/08/2015 11:12:49 ******/
IF NOT EXISTS (SELECT * FROM sys.schemas WHERE name = N'ConferenceManagement')
EXEC sys.sp_executesql N'CREATE SCHEMA [ConferenceManagement] AUTHORIZATION [dbo]'
GO
/****** Object:  Table [ConferenceManagement].[Orders]    Script Date: 09/08/2015 11:12:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[ConferenceManagement].[Orders]') AND type in (N'U'))
BEGIN
CREATE TABLE [ConferenceManagement].[Orders](
	[Id] [uniqueidentifier] NOT NULL,
	[ConferenceId] [uniqueidentifier] NOT NULL,
	[AssignmentsId] [uniqueidentifier] NULL,
	[AccessCode] [nvarchar](max) NULL,
	[RegistrantName] [nvarchar](max) NULL,
	[RegistrantEmail] [nvarchar](max) NULL,
	[TotalAmount] [decimal](18, 2) NOT NULL,
	[StatusValue] [int] NOT NULL,
 CONSTRAINT [PK_ConferenceManagement.Orders] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
/****** Object:  Table [MessageLog].[Messages]    Script Date: 09/08/2015 11:12:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[MessageLog].[Messages]') AND type in (N'U'))
BEGIN
CREATE TABLE [MessageLog].[Messages](
	[Id] [uniqueidentifier] NOT NULL,
	[Kind] [nvarchar](max) NULL,
	[SourceId] [nvarchar](max) NULL,
	[AssemblyName] [nvarchar](max) NULL,
	[Namespace] [nvarchar](max) NULL,
	[FullName] [nvarchar](max) NULL,
	[TypeName] [nvarchar](max) NULL,
	[SourceType] [nvarchar](max) NULL,
	[CreationDate] [nvarchar](max) NULL,
	[Payload] [nvarchar](max) NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
/****** Object:  Table [SqlBus].[Events]    Script Date: 09/08/2015 11:12:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[SqlBus].[Events]') AND type in (N'U'))
BEGIN
CREATE TABLE [SqlBus].[Events](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[Body] [nvarchar](max) NOT NULL,
	[MessageType] [nvarchar](256) NOT NULL,
	[DeliveryDate] [datetime] NULL,
	[CorrelationId] [nvarchar](max) NULL,
 CONSTRAINT [PK_SqlBus.Events] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
/****** Object:  Table [Events].[Events]    Script Date: 09/08/2015 11:12:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[Events].[Events]') AND type in (N'U'))
BEGIN
CREATE TABLE [Events].[Events](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[AggregateId] [uniqueidentifier] NOT NULL,
	[AggregateType] [nvarchar](128) NOT NULL,
	[Version] [int] NOT NULL,
	[VersionedEventType] [nvarchar](256) NOT NULL,
	[Payload] [nvarchar](max) NULL,
	[CorrelationId] [nvarchar](max) NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
/****** Object:  Table [ConferenceManagement].[Conferences]    Script Date: 09/08/2015 11:12:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[ConferenceManagement].[Conferences]') AND type in (N'U'))
BEGIN
CREATE TABLE [ConferenceManagement].[Conferences](
	[Id] [uniqueidentifier] NOT NULL,
	[AccessCode] [nvarchar](6) NULL,
	[OwnerName] [nvarchar](max) NOT NULL,
	[OwnerEmail] [nvarchar](max) NOT NULL,
	[Slug] [nvarchar](max) NOT NULL,
	[WasEverPublished] [bit] NOT NULL,
	[Name] [nvarchar](max) NOT NULL,
	[Description] [nvarchar](max) NOT NULL,
	[Location] [nvarchar](max) NOT NULL,
	[Tagline] [nvarchar](max) NULL,
	[TwitterSearch] [nvarchar](max) NULL,
	[StartDate] [datetime] NOT NULL,
	[EndDate] [datetime] NOT NULL,
	[IsPublished] [bit] NOT NULL,
 CONSTRAINT [PK_ConferenceManagement.Conferences] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
/****** Object:  Table [SqlBus].[Commands]    Script Date: 09/08/2015 11:12:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[SqlBus].[Commands]') AND type in (N'U'))
BEGIN
CREATE TABLE [SqlBus].[Commands](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[Body] [nvarchar](max) NOT NULL,
	[MessageType] [nvarchar](256) NOT NULL,
	[DeliveryDate] [datetime] NULL,
	[CorrelationId] [nvarchar](max) NULL,
 CONSTRAINT [PK_SqlBus.Commands] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
/****** Object:  Table [dbo].[__MigrationHistory]    Script Date: 09/08/2015 11:12:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[__MigrationHistory]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[__MigrationHistory](
	[MigrationId] [nvarchar](150) NOT NULL,
	[ContextKey] [nvarchar](300) NOT NULL,
	[Model] [varbinary](max) NOT NULL,
	[ProductVersion] [nvarchar](32) NOT NULL,
 CONSTRAINT [PK_dbo.__MigrationHistory] PRIMARY KEY CLUSTERED 
(
	[MigrationId] ASC,
	[ContextKey] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [ConferencePayments].[Payment]    Script Date: 09/08/2015 11:12:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[ConferencePayments].[Payment]') AND type in (N'U'))
BEGIN
CREATE TABLE [ConferencePayments].[Payment](
	[Id] [uniqueidentifier] NOT NULL,
	[StateValue] [int] NOT NULL,
	[PaymentSourceId] [uniqueidentifier] NOT NULL,
	[Description] [nvarchar](max) NULL,
	[TotalAmount] [decimal](18, 2) NOT NULL,
 CONSTRAINT [PK_ConferencePayments.Payment] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
/****** Object:  Table [ConferenceRegistrationProcesses].[RegistrationProcess]    Script Date: 09/08/2015 11:12:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[ConferenceRegistrationProcesses].[RegistrationProcess]') AND type in (N'U'))
BEGIN
CREATE TABLE [ConferenceRegistrationProcesses].[RegistrationProcess](
	[Id] [uniqueidentifier] NOT NULL,
	[Completed] [bit] NOT NULL,
	[ConferenceId] [uniqueidentifier] NOT NULL,
	[OrderId] [uniqueidentifier] NOT NULL,
	[ReservationId] [uniqueidentifier] NOT NULL,
	[SeatReservationCommandId] [uniqueidentifier] NOT NULL,
	[ReservationAutoExpiration] [datetime] NULL,
	[ExpirationCommandId] [uniqueidentifier] NOT NULL,
	[StateValue] [int] NOT NULL,
	[TimeStamp] [timestamp] NOT NULL,
 CONSTRAINT [PK_ConferenceRegistrationProcesses.RegistrationProcess] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
/****** Object:  Table [ConferenceRegistrationProcesses].[UndispatchedMessages]    Script Date: 09/08/2015 11:12:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[ConferenceRegistrationProcesses].[UndispatchedMessages]') AND type in (N'U'))
BEGIN
CREATE TABLE [ConferenceRegistrationProcesses].[UndispatchedMessages](
	[Id] [uniqueidentifier] NOT NULL,
	[Commands] [nvarchar](max) NULL,
 CONSTRAINT [PK_ConferenceRegistrationProcesses.UndispatchedMessages] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
/****** Object:  Table [ConferenceManagement].[SeatTypes]    Script Date: 09/08/2015 11:12:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[ConferenceManagement].[SeatTypes]') AND type in (N'U'))
BEGIN
CREATE TABLE [ConferenceManagement].[SeatTypes](
	[Id] [uniqueidentifier] NOT NULL,
	[Name] [nvarchar](70) NOT NULL,
	[Description] [nvarchar](250) NOT NULL,
	[Quantity] [int] NOT NULL,
	[Price] [decimal](18, 2) NOT NULL,
	[ConferenceId] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_ConferenceManagement.SeatTypes] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[ConferenceManagement].[SeatTypes]') AND name = N'IX_ConferenceId')
CREATE NONCLUSTERED INDEX [IX_ConferenceId] ON [ConferenceManagement].[SeatTypes] 
(
	[ConferenceId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
/****** Object:  Table [ConferencePayments].[PaymentItem]    Script Date: 09/08/2015 11:12:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[ConferencePayments].[PaymentItem]') AND type in (N'U'))
BEGIN
CREATE TABLE [ConferencePayments].[PaymentItem](
	[Id] [uniqueidentifier] NOT NULL,
	[Description] [nvarchar](max) NULL,
	[Amount] [decimal](18, 2) NOT NULL,
	[Payment_Id] [uniqueidentifier] NULL,
 CONSTRAINT [PK_ConferencePayments.PaymentItem] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[ConferencePayments].[PaymentItem]') AND name = N'IX_Payment_Id')
CREATE NONCLUSTERED INDEX [IX_Payment_Id] ON [ConferencePayments].[PaymentItem] 
(
	[Payment_Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
/****** Object:  Table [ConferenceManagement].[OrderSeats]    Script Date: 09/08/2015 11:12:52 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[ConferenceManagement].[OrderSeats]') AND type in (N'U'))
BEGIN
CREATE TABLE [ConferenceManagement].[OrderSeats](
	[OrderId] [uniqueidentifier] NOT NULL,
	[Position] [int] NOT NULL,
	[Attendee_FirstName] [nvarchar](max) NULL,
	[Attendee_LastName] [nvarchar](max) NULL,
	[Attendee_Email] [nvarchar](max) NULL,
	[SeatInfoId] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_ConferenceManagement.OrderSeats] PRIMARY KEY CLUSTERED 
(
	[OrderId] ASC,
	[Position] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[ConferenceManagement].[OrderSeats]') AND name = N'IX_OrderId')
CREATE NONCLUSTERED INDEX [IX_OrderId] ON [ConferenceManagement].[OrderSeats] 
(
	[OrderId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[ConferenceManagement].[OrderSeats]') AND name = N'IX_SeatInfoId')
CREATE NONCLUSTERED INDEX [IX_SeatInfoId] ON [ConferenceManagement].[OrderSeats] 
(
	[SeatInfoId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
/****** Object:  ForeignKey [FK_ConferenceManagement.SeatTypes_ConferenceManagement.Conferences_ConferenceId]    Script Date: 09/08/2015 11:12:51 ******/
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[ConferenceManagement].[FK_ConferenceManagement.SeatTypes_ConferenceManagement.Conferences_ConferenceId]') AND parent_object_id = OBJECT_ID(N'[ConferenceManagement].[SeatTypes]'))
ALTER TABLE [ConferenceManagement].[SeatTypes]  WITH CHECK ADD  CONSTRAINT [FK_ConferenceManagement.SeatTypes_ConferenceManagement.Conferences_ConferenceId] FOREIGN KEY([ConferenceId])
REFERENCES [ConferenceManagement].[Conferences] ([Id])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[ConferenceManagement].[FK_ConferenceManagement.SeatTypes_ConferenceManagement.Conferences_ConferenceId]') AND parent_object_id = OBJECT_ID(N'[ConferenceManagement].[SeatTypes]'))
ALTER TABLE [ConferenceManagement].[SeatTypes] CHECK CONSTRAINT [FK_ConferenceManagement.SeatTypes_ConferenceManagement.Conferences_ConferenceId]
GO
/****** Object:  ForeignKey [FK_ConferencePayments.PaymentItem_ConferencePayments.Payment_Payment_Id]    Script Date: 09/08/2015 11:12:52 ******/
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[ConferencePayments].[FK_ConferencePayments.PaymentItem_ConferencePayments.Payment_Payment_Id]') AND parent_object_id = OBJECT_ID(N'[ConferencePayments].[PaymentItem]'))
ALTER TABLE [ConferencePayments].[PaymentItem]  WITH CHECK ADD  CONSTRAINT [FK_ConferencePayments.PaymentItem_ConferencePayments.Payment_Payment_Id] FOREIGN KEY([Payment_Id])
REFERENCES [ConferencePayments].[Payment] ([Id])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[ConferencePayments].[FK_ConferencePayments.PaymentItem_ConferencePayments.Payment_Payment_Id]') AND parent_object_id = OBJECT_ID(N'[ConferencePayments].[PaymentItem]'))
ALTER TABLE [ConferencePayments].[PaymentItem] CHECK CONSTRAINT [FK_ConferencePayments.PaymentItem_ConferencePayments.Payment_Payment_Id]
GO
/****** Object:  ForeignKey [FK_ConferenceManagement.OrderSeats_ConferenceManagement.SeatTypes_SeatInfoId]    Script Date: 09/08/2015 11:12:52 ******/
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[ConferenceManagement].[FK_ConferenceManagement.OrderSeats_ConferenceManagement.SeatTypes_SeatInfoId]') AND parent_object_id = OBJECT_ID(N'[ConferenceManagement].[OrderSeats]'))
ALTER TABLE [ConferenceManagement].[OrderSeats]  WITH CHECK ADD  CONSTRAINT [FK_ConferenceManagement.OrderSeats_ConferenceManagement.SeatTypes_SeatInfoId] FOREIGN KEY([SeatInfoId])
REFERENCES [ConferenceManagement].[SeatTypes] ([Id])
ON DELETE CASCADE
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[ConferenceManagement].[FK_ConferenceManagement.OrderSeats_ConferenceManagement.SeatTypes_SeatInfoId]') AND parent_object_id = OBJECT_ID(N'[ConferenceManagement].[OrderSeats]'))
ALTER TABLE [ConferenceManagement].[OrderSeats] CHECK CONSTRAINT [FK_ConferenceManagement.OrderSeats_ConferenceManagement.SeatTypes_SeatInfoId]
GO

USE [master]
GO
/****** Object:  Database [MSS]    Script Date: 01/16/2020 22:03:08 ******/
CREATE DATABASE [MSS] ON  PRIMARY 
( NAME = N'MSS', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL10_50.SQLEXPRESS\MSSQL\DATA\MSS.mdf' , SIZE = 2304KB , MAXSIZE = UNLIMITED, FILEGROWTH = 1024KB )
 LOG ON 
( NAME = N'MSS_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL10_50.SQLEXPRESS\MSSQL\DATA\MSS_log.LDF' , SIZE = 768KB , MAXSIZE = 2048GB , FILEGROWTH = 10%)
GO
ALTER DATABASE [MSS] SET COMPATIBILITY_LEVEL = 100
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [MSS].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [MSS] SET ANSI_NULL_DEFAULT OFF
GO
ALTER DATABASE [MSS] SET ANSI_NULLS OFF
GO
ALTER DATABASE [MSS] SET ANSI_PADDING OFF
GO
ALTER DATABASE [MSS] SET ANSI_WARNINGS OFF
GO
ALTER DATABASE [MSS] SET ARITHABORT OFF
GO
ALTER DATABASE [MSS] SET AUTO_CLOSE ON
GO
ALTER DATABASE [MSS] SET AUTO_CREATE_STATISTICS ON
GO
ALTER DATABASE [MSS] SET AUTO_SHRINK OFF
GO
ALTER DATABASE [MSS] SET AUTO_UPDATE_STATISTICS ON
GO
ALTER DATABASE [MSS] SET CURSOR_CLOSE_ON_COMMIT OFF
GO
ALTER DATABASE [MSS] SET CURSOR_DEFAULT  GLOBAL
GO
ALTER DATABASE [MSS] SET CONCAT_NULL_YIELDS_NULL OFF
GO
ALTER DATABASE [MSS] SET NUMERIC_ROUNDABORT OFF
GO
ALTER DATABASE [MSS] SET QUOTED_IDENTIFIER OFF
GO
ALTER DATABASE [MSS] SET RECURSIVE_TRIGGERS OFF
GO
ALTER DATABASE [MSS] SET  ENABLE_BROKER
GO
ALTER DATABASE [MSS] SET AUTO_UPDATE_STATISTICS_ASYNC OFF
GO
ALTER DATABASE [MSS] SET DATE_CORRELATION_OPTIMIZATION OFF
GO
ALTER DATABASE [MSS] SET TRUSTWORTHY OFF
GO
ALTER DATABASE [MSS] SET ALLOW_SNAPSHOT_ISOLATION OFF
GO
ALTER DATABASE [MSS] SET PARAMETERIZATION SIMPLE
GO
ALTER DATABASE [MSS] SET READ_COMMITTED_SNAPSHOT OFF
GO
ALTER DATABASE [MSS] SET HONOR_BROKER_PRIORITY OFF
GO
ALTER DATABASE [MSS] SET  READ_WRITE
GO
ALTER DATABASE [MSS] SET RECOVERY SIMPLE
GO
ALTER DATABASE [MSS] SET  MULTI_USER
GO
ALTER DATABASE [MSS] SET PAGE_VERIFY CHECKSUM
GO
ALTER DATABASE [MSS] SET DB_CHAINING OFF
GO
USE [MSS]
GO
/****** Object:  Table [dbo].[Semester]    Script Date: 01/16/2020 22:03:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Semester](
	[Semester_ID] [varchar](50) NOT NULL,
	[Semester_Name] [nvarchar](100) NULL,
	[Start_Date] [datetime] NULL,
	[End_Date] [datetime] NULL,
 CONSTRAINT [PK_Semester] PRIMARY KEY CLUSTERED 
(
	[Semester_ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
INSERT [dbo].[Semester] ([Semester_ID], [Semester_Name], [Start_Date], [End_Date]) VALUES (N'SP2020', N'Spring 2020', CAST(0x0000AB3500000000 AS DateTime), CAST(0x0000ABAD00000000 AS DateTime))
/****** Object:  Table [dbo].[Role]    Script Date: 01/16/2020 22:03:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Role](
	[Role_ID] [varchar](50) NOT NULL,
	[Role_Name] [varchar](50) NULL,
 CONSTRAINT [PK_Role] PRIMARY KEY CLUSTERED 
(
	[Role_ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Mentor]    Script Date: 01/16/2020 22:03:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Mentor](
	[Mentor_ID] [varchar](50) NOT NULL,
	[Login] [varchar](100) NULL,
	[Email] [varchar](100) NULL,
	[Name] [varchar](100) NULL,
	[Phone] [varchar](50) NULL,
 CONSTRAINT [PK_Mentor] PRIMARY KEY CLUSTERED 
(
	[Mentor_ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Student]    Script Date: 01/16/2020 22:03:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Student](
	[Email] [varchar](50) NOT NULL,
	[Roll] [varchar](50) NOT NULL,
 CONSTRAINT [PK_Student] PRIMARY KEY CLUSTERED 
(
	[Roll] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
INSERT [dbo].[Student] ([Email], [Roll]) VALUES (N'DATNT', N'SE04909')
/****** Object:  Table [dbo].[Subject]    Script Date: 01/16/2020 22:03:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Subject](
	[Subject_ID] [varchar](50) NOT NULL,
	[Subject_Name] [nvarchar](200) NOT NULL,
 CONSTRAINT [PK_Subject_1] PRIMARY KEY CLUSTERED 
(
	[Subject_ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Campus]    Script Date: 01/16/2020 22:03:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Campus](
	[Campus_ID] [varchar](50) NOT NULL,
	[Campus_Name] [nvarchar](100) NOT NULL,
	[Address] [nvarchar](200) NOT NULL,
	[Contact_Point] [nvarchar](200) NOT NULL,
 CONSTRAINT [PK_Campus] PRIMARY KEY CLUSTERED 
(
	[Campus_ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
INSERT [dbo].[Campus] ([Campus_ID], [Campus_Name], [Address], [Contact_Point]) VALUES (N'HN', N'FPT Hà Nội', N'Hòa Lạc', N'123')
/****** Object:  Table [dbo].[User]    Script Date: 01/16/2020 22:03:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[User](
	[User_ID] [varchar](50) NOT NULL,
	[Name] [varchar](50) NOT NULL,
	[Email] [varchar](50) NULL,
	[Phone] [varchar](50) NULL,
 CONSTRAINT [PK_User] PRIMARY KEY CLUSTERED 
(
	[User_ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[User_Role]    Script Date: 01/16/2020 22:03:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[User_Role](
	[User_ID] [varchar](50) NOT NULL,
	[Role_ID] [varchar](50) NOT NULL,
	[Login] [varchar](50) NOT NULL,
	[isActive] [varchar](50) NOT NULL,
 CONSTRAINT [PK_User_Role] PRIMARY KEY CLUSTERED 
(
	[User_ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Subject_Student]    Script Date: 01/16/2020 22:03:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Subject_Student](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Subject_ID] [varchar](50) NOT NULL,
	[Roll] [varchar](50) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Class]    Script Date: 01/16/2020 22:03:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Class](
	[Class_ID] [varchar](50) NOT NULL,
	[University] [varchar](50) NULL,
	[Class_Start_Time] [datetime] NULL,
	[Enrollment_Source] [nchar](10) NULL,
	[Mentor_ID] [varchar](50) NULL,
	[Semester_ID] [varchar](50) NULL,
	[Campus_Name] [varchar](50) NULL,
 CONSTRAINT [PK_Class] PRIMARY KEY CLUSTERED 
(
	[Class_ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Specification]    Script Date: 01/16/2020 22:03:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Specification](
	[Specification_ID] [varchar](50) NOT NULL,
	[Subject_ID] [varchar](50) NOT NULL,
 CONSTRAINT [PK_Specification_1] PRIMARY KEY CLUSTERED 
(
	[Specification_ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY],
 CONSTRAINT [IX_Specification_2] UNIQUE NONCLUSTERED 
(
	[Subject_ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Course]    Script Date: 01/16/2020 22:03:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Course](
	[Course_ID] [varchar](50) NOT NULL,
	[Course_Name] [varchar](150) NOT NULL,
	[Course_Slug] [varchar](50) NOT NULL,
	[Specification_ID] [varchar](50) NULL,
 CONSTRAINT [PK_Course] PRIMARY KEY CLUSTERED 
(
	[Course_ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Class_Student]    Script Date: 01/16/2020 22:03:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Class_Student](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Roll] [varchar](50) NOT NULL,
	[Class_ID] [varchar](50) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Student_Specification_Log]    Script Date: 01/16/2020 22:03:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Student_Specification_Log](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Roll] [varchar](50) NULL,
	[Subject_ID] [varchar](50) NULL,
	[Campus] [varchar](50) NULL,
	[Specialization] [nvarchar](150) NULL,
	[Specialization_Slug] [nvarchar](50) NULL,
	[University] [nvarchar](250) NULL,
	[Specialization_Enrollment_Time] [datetime] NULL,
	[Last_Specialization_Activity_Time] [datetime] NULL,
	[Completed] [bit] NULL,
	[Status] [bit] NULL,
	[Program_Slug] [nvarchar](50) NULL,
	[Program_Name] [nvarchar](150) NULL,
	[Specialization_Completion_Time] [datetime] NULL,
	[Specification_ID] [varchar](50) NULL,
	[Course_ID] [varchar](50) NULL,
 CONSTRAINT [PK_Student_Specification_Log] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
SET IDENTITY_INSERT [dbo].[Student_Specification_Log] ON
INSERT [dbo].[Student_Specification_Log] ([ID], [Roll], [Subject_ID], [Campus], [Specialization], [Specialization_Slug], [University], [Specialization_Enrollment_Time], [Last_Specialization_Activity_Time], [Completed], [Status], [Program_Slug], [Program_Name], [Specialization_Completion_Time], [Specification_ID], [Course_ID]) VALUES (10, N'SE04909', N'PMG201c', N'HN', N'Full-Stack Web Development with React', N'full-stack-react', N'The Hong Kong University of Science and Technology', CAST(0x0000AB2E01386DFD AS DateTime), CAST(0x0000AB340097782C AS DateTime), 0, 0, N'fptu-ha-noi-efik3', N'FPT University Program', CAST(0x0000AB0100BF44D2 AS DateTime), NULL, NULL)
INSERT [dbo].[Student_Specification_Log] ([ID], [Roll], [Subject_ID], [Campus], [Specialization], [Specialization_Slug], [University], [Specialization_Enrollment_Time], [Last_Specialization_Activity_Time], [Completed], [Status], [Program_Slug], [Program_Name], [Specialization_Completion_Time], [Specification_ID], [Course_ID]) VALUES (11, N'SE04909', N'PMG201c', N'HN', N'Introduction to Project Management Principles and Practices', N'project-management', N'University of California', CAST(0x0000AAC50006428E AS DateTime), CAST(0x0000AB0500B71E84 AS DateTime), 1, 0, N'fptu-ha-noi-efik3', N'FPT University Program', CAST(0x0000AB0100BF44D2 AS DateTime), NULL, NULL)
INSERT [dbo].[Student_Specification_Log] ([ID], [Roll], [Subject_ID], [Campus], [Specialization], [Specialization_Slug], [University], [Specialization_Enrollment_Time], [Last_Specialization_Activity_Time], [Completed], [Status], [Program_Slug], [Program_Name], [Specialization_Completion_Time], [Specification_ID], [Course_ID]) VALUES (12, N'SE04909', N'PMG201c', N'HN', N'Android App Development', N'android-app-development', N'Vanderbilt University', CAST(0x0000AB180186E36A AS DateTime), CAST(0x0000AB1801889C34 AS DateTime), 0, 0, N'fptu-ha-noi-efik3', N'FPT University Program', CAST(0x0000AB0100BF44D2 AS DateTime), NULL, NULL)
SET IDENTITY_INSERT [dbo].[Student_Specification_Log] OFF
/****** Object:  Table [dbo].[Student_Course_Log]    Script Date: 01/16/2020 22:03:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Student_Course_Log](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Course_Enrollment_Time] [datetime] NULL,
	[Course_Start_Time] [datetime] NULL,
	[Last_Course_Activity_Time] [datetime] NULL,
	[Overall_Progress] [float] NULL,
	[Estimated] [float] NULL,
	[Completed] [bit] NULL,
	[Status] [bit] NULL,
	[Program_Slug] [nvarchar](50) NULL,
	[Program_Name] [nvarchar](150) NULL,
	[Completion_Time] [datetime] NULL,
	[Course_ID] [varchar](50) NULL,
	[Course_Grade] [float] NULL,
	[Roll] [varchar](50) NULL,
 CONSTRAINT [PK_Student_Course_Log] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
SET IDENTITY_INSERT [dbo].[Student_Course_Log] ON
INSERT [dbo].[Student_Course_Log] ([ID], [Course_Enrollment_Time], [Course_Start_Time], [Last_Course_Activity_Time], [Overall_Progress], [Estimated], [Completed], [Status], [Program_Slug], [Program_Name], [Completion_Time], [Course_ID], [Course_Grade], [Roll]) VALUES (28, CAST(0x0000AB2E01386E90 AS DateTime), CAST(0x0000AB2C00F73140 AS DateTime), CAST(0x0000AB340097782C AS DateTime), 0, 0, 0, 0, N'fptu-ha-noi-efik3', N'FPT University Program', CAST(0x0000AB0100BF44D2 AS DateTime), NULL, 0, N'SE04909')
INSERT [dbo].[Student_Course_Log] ([ID], [Course_Enrollment_Time], [Course_Start_Time], [Last_Course_Activity_Time], [Overall_Progress], [Estimated], [Completed], [Status], [Program_Slug], [Program_Name], [Completion_Time], [Course_ID], [Course_Grade], [Roll]) VALUES (29, CAST(0x0000AB2E01393EEA AS DateTime), CAST(0x0000AB2C00F73140 AS DateTime), CAST(0x0000AB340097782C AS DateTime), 1.1, 0.05, 0, 0, N'fptu-ha-noi-efik3', N'FPT University Program', CAST(0x0000AB0100BF44D2 AS DateTime), NULL, 0, N'SE04909')
INSERT [dbo].[Student_Course_Log] ([ID], [Course_Enrollment_Time], [Course_Start_Time], [Last_Course_Activity_Time], [Overall_Progress], [Estimated], [Completed], [Status], [Program_Slug], [Program_Name], [Completion_Time], [Course_ID], [Course_Grade], [Roll]) VALUES (30, CAST(0x0000AB0B00ABBEE8 AS DateTime), CAST(0x0000AB0900F73140 AS DateTime), CAST(0x0000AB290103B2D0 AS DateTime), 30.61, 10.74, 0, 0, N'fptu-ha-noi-efik3', N'FPT University Program', CAST(0x0000AB0100BF44D2 AS DateTime), NULL, 15, N'SE04909')
INSERT [dbo].[Student_Course_Log] ([ID], [Course_Enrollment_Time], [Course_Start_Time], [Last_Course_Activity_Time], [Overall_Progress], [Estimated], [Completed], [Status], [Program_Slug], [Program_Name], [Completion_Time], [Course_ID], [Course_Grade], [Roll]) VALUES (31, CAST(0x0000AB180186E3EE AS DateTime), CAST(0x0000AB1F00F73140 AS DateTime), CAST(0x0000AB1801889C34 AS DateTime), 0.39, 0.08, 0, 0, N'fptu-ha-noi-efik3', N'FPT University Program', CAST(0x0000AB0100BF44D2 AS DateTime), NULL, 1.2, N'SE04909')
INSERT [dbo].[Student_Course_Log] ([ID], [Course_Enrollment_Time], [Course_Start_Time], [Last_Course_Activity_Time], [Overall_Progress], [Estimated], [Completed], [Status], [Program_Slug], [Program_Name], [Completion_Time], [Course_ID], [Course_Grade], [Roll]) VALUES (32, CAST(0x0000AAEC015868C8 AS DateTime), CAST(0x0000AAFB00F73140 AS DateTime), CAST(0x0000AB0500B71E84 AS DateTime), 100, 8.98, 1, 0, N'fptu-ha-noi-efik3', N'FPT University Program', CAST(0x0000AB0100BF44D2 AS DateTime), NULL, 100, N'SE04909')
INSERT [dbo].[Student_Course_Log] ([ID], [Course_Enrollment_Time], [Course_Start_Time], [Last_Course_Activity_Time], [Overall_Progress], [Estimated], [Completed], [Status], [Program_Slug], [Program_Name], [Completion_Time], [Course_ID], [Course_Grade], [Roll]) VALUES (33, CAST(0x0000AAF5015B00D5 AS DateTime), CAST(0x0000AAFB00F73140 AS DateTime), CAST(0x0000AB01018058F8 AS DateTime), 59.27, 43.35, 0, 0, N'fptu-ha-noi-efik3', N'FPT University Program', CAST(0x0000AB0100BF44D2 AS DateTime), NULL, 15.54, N'SE04909')
INSERT [dbo].[Student_Course_Log] ([ID], [Course_Enrollment_Time], [Course_Start_Time], [Last_Course_Activity_Time], [Overall_Progress], [Estimated], [Completed], [Status], [Program_Slug], [Program_Name], [Completion_Time], [Course_ID], [Course_Grade], [Roll]) VALUES (34, CAST(0x0000AADA016C9217 AS DateTime), CAST(0x0000AADF00E6B680 AS DateTime), CAST(0x0000AADE00D621F8 AS DateTime), 100, 4.27, 1, 0, N'fptu-ha-noi-efik3', N'FPT University Program', CAST(0x0000AADE00D42ACE AS DateTime), NULL, 92.4, N'SE04909')
INSERT [dbo].[Student_Course_Log] ([ID], [Course_Enrollment_Time], [Course_Start_Time], [Last_Course_Activity_Time], [Overall_Progress], [Estimated], [Completed], [Status], [Program_Slug], [Program_Name], [Completion_Time], [Course_ID], [Course_Grade], [Roll]) VALUES (35, CAST(0x0000AADF00B44935 AS DateTime), CAST(0x0000AADF00E6B680 AS DateTime), CAST(0x0000AAEC01576434 AS DateTime), 90, 0.38, 1, 0, N'fptu-ha-noi-efik3', N'FPT University Program', CAST(0x0000AAEC01574AFE AS DateTime), NULL, 88.8, N'SE04909')
INSERT [dbo].[Student_Course_Log] ([ID], [Course_Enrollment_Time], [Course_Start_Time], [Last_Course_Activity_Time], [Overall_Progress], [Estimated], [Completed], [Status], [Program_Slug], [Program_Name], [Completion_Time], [Course_ID], [Course_Grade], [Roll]) VALUES (36, CAST(0x0000AAC500064309 AS DateTime), CAST(0x0000AAC300E6B680 AS DateTime), CAST(0x0000AAD901035F9C AS DateTime), 100, 5.36, 1, 0, N'fptu-ha-noi-efik3', N'FPT University Program', CAST(0x0000AAD90103607F AS DateTime), NULL, 90.6, N'SE04909')
SET IDENTITY_INSERT [dbo].[Student_Course_Log] OFF
/****** Object:  ForeignKey [FK_User_Role_Role]    Script Date: 01/16/2020 22:03:09 ******/
ALTER TABLE [dbo].[User_Role]  WITH CHECK ADD  CONSTRAINT [FK_User_Role_Role] FOREIGN KEY([Role_ID])
REFERENCES [dbo].[Role] ([Role_ID])
GO
ALTER TABLE [dbo].[User_Role] CHECK CONSTRAINT [FK_User_Role_Role]
GO
/****** Object:  ForeignKey [FK_User_Role_User]    Script Date: 01/16/2020 22:03:09 ******/
ALTER TABLE [dbo].[User_Role]  WITH CHECK ADD  CONSTRAINT [FK_User_Role_User] FOREIGN KEY([User_ID])
REFERENCES [dbo].[User] ([User_ID])
GO
ALTER TABLE [dbo].[User_Role] CHECK CONSTRAINT [FK_User_Role_User]
GO
/****** Object:  ForeignKey [FK_Subject_Student_Student]    Script Date: 01/16/2020 22:03:09 ******/
ALTER TABLE [dbo].[Subject_Student]  WITH CHECK ADD  CONSTRAINT [FK_Subject_Student_Student] FOREIGN KEY([Roll])
REFERENCES [dbo].[Student] ([Roll])
GO
ALTER TABLE [dbo].[Subject_Student] CHECK CONSTRAINT [FK_Subject_Student_Student]
GO
/****** Object:  ForeignKey [FK_Subject_Student_Subject]    Script Date: 01/16/2020 22:03:09 ******/
ALTER TABLE [dbo].[Subject_Student]  WITH CHECK ADD  CONSTRAINT [FK_Subject_Student_Subject] FOREIGN KEY([Subject_ID])
REFERENCES [dbo].[Subject] ([Subject_ID])
GO
ALTER TABLE [dbo].[Subject_Student] CHECK CONSTRAINT [FK_Subject_Student_Subject]
GO
/****** Object:  ForeignKey [FK_Class_Campus]    Script Date: 01/16/2020 22:03:09 ******/
ALTER TABLE [dbo].[Class]  WITH CHECK ADD  CONSTRAINT [FK_Class_Campus] FOREIGN KEY([Campus_Name])
REFERENCES [dbo].[Campus] ([Campus_ID])
GO
ALTER TABLE [dbo].[Class] CHECK CONSTRAINT [FK_Class_Campus]
GO
/****** Object:  ForeignKey [FK_Class_Mentor]    Script Date: 01/16/2020 22:03:09 ******/
ALTER TABLE [dbo].[Class]  WITH CHECK ADD  CONSTRAINT [FK_Class_Mentor] FOREIGN KEY([Mentor_ID])
REFERENCES [dbo].[Mentor] ([Mentor_ID])
GO
ALTER TABLE [dbo].[Class] CHECK CONSTRAINT [FK_Class_Mentor]
GO
/****** Object:  ForeignKey [FK_Class_Semester]    Script Date: 01/16/2020 22:03:09 ******/
ALTER TABLE [dbo].[Class]  WITH CHECK ADD  CONSTRAINT [FK_Class_Semester] FOREIGN KEY([Semester_ID])
REFERENCES [dbo].[Semester] ([Semester_ID])
GO
ALTER TABLE [dbo].[Class] CHECK CONSTRAINT [FK_Class_Semester]
GO
/****** Object:  ForeignKey [FK_Specification_Subject]    Script Date: 01/16/2020 22:03:09 ******/
ALTER TABLE [dbo].[Specification]  WITH CHECK ADD  CONSTRAINT [FK_Specification_Subject] FOREIGN KEY([Subject_ID])
REFERENCES [dbo].[Subject] ([Subject_ID])
GO
ALTER TABLE [dbo].[Specification] CHECK CONSTRAINT [FK_Specification_Subject]
GO
/****** Object:  ForeignKey [FK_Course_Specification1]    Script Date: 01/16/2020 22:03:09 ******/
ALTER TABLE [dbo].[Course]  WITH CHECK ADD  CONSTRAINT [FK_Course_Specification1] FOREIGN KEY([Specification_ID])
REFERENCES [dbo].[Specification] ([Specification_ID])
GO
ALTER TABLE [dbo].[Course] CHECK CONSTRAINT [FK_Course_Specification1]
GO
/****** Object:  ForeignKey [FK_Class_Student_Class]    Script Date: 01/16/2020 22:03:09 ******/
ALTER TABLE [dbo].[Class_Student]  WITH CHECK ADD  CONSTRAINT [FK_Class_Student_Class] FOREIGN KEY([Class_ID])
REFERENCES [dbo].[Class] ([Class_ID])
GO
ALTER TABLE [dbo].[Class_Student] CHECK CONSTRAINT [FK_Class_Student_Class]
GO
/****** Object:  ForeignKey [FK_Class_Student_Student]    Script Date: 01/16/2020 22:03:09 ******/
ALTER TABLE [dbo].[Class_Student]  WITH CHECK ADD  CONSTRAINT [FK_Class_Student_Student] FOREIGN KEY([Roll])
REFERENCES [dbo].[Student] ([Roll])
GO
ALTER TABLE [dbo].[Class_Student] CHECK CONSTRAINT [FK_Class_Student_Student]
GO
/****** Object:  ForeignKey [FK_Student_Specification_Log_Specification]    Script Date: 01/16/2020 22:03:09 ******/
ALTER TABLE [dbo].[Student_Specification_Log]  WITH CHECK ADD  CONSTRAINT [FK_Student_Specification_Log_Specification] FOREIGN KEY([Specification_ID])
REFERENCES [dbo].[Specification] ([Specification_ID])
GO
ALTER TABLE [dbo].[Student_Specification_Log] CHECK CONSTRAINT [FK_Student_Specification_Log_Specification]
GO
/****** Object:  ForeignKey [FK_Student_Specification_Log_Student]    Script Date: 01/16/2020 22:03:09 ******/
ALTER TABLE [dbo].[Student_Specification_Log]  WITH CHECK ADD  CONSTRAINT [FK_Student_Specification_Log_Student] FOREIGN KEY([Roll])
REFERENCES [dbo].[Student] ([Roll])
GO
ALTER TABLE [dbo].[Student_Specification_Log] CHECK CONSTRAINT [FK_Student_Specification_Log_Student]
GO
/****** Object:  ForeignKey [FK_Student_Course_Log_Course]    Script Date: 01/16/2020 22:03:09 ******/
ALTER TABLE [dbo].[Student_Course_Log]  WITH CHECK ADD  CONSTRAINT [FK_Student_Course_Log_Course] FOREIGN KEY([Course_ID])
REFERENCES [dbo].[Course] ([Course_ID])
GO
ALTER TABLE [dbo].[Student_Course_Log] CHECK CONSTRAINT [FK_Student_Course_Log_Course]
GO
/****** Object:  ForeignKey [FK_Student_Course_Log_Student]    Script Date: 01/16/2020 22:03:09 ******/
ALTER TABLE [dbo].[Student_Course_Log]  WITH CHECK ADD  CONSTRAINT [FK_Student_Course_Log_Student] FOREIGN KEY([Roll])
REFERENCES [dbo].[Student] ([Roll])
GO
ALTER TABLE [dbo].[Student_Course_Log] CHECK CONSTRAINT [FK_Student_Course_Log_Student]
GO

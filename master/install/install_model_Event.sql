IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Events]') AND type in (N'U'))
DROP TABLE [dbo].[Events]
go
SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
SET ANSI_PADDING ON
CREATE TABLE [dbo].[Events]( 
[Id] [int] IDENTITY(1,1) NOT NULL,
[CreateDate] [datetime] NOT NULL,
[UpdateDate] [datetime] NOT NULL,
[CreateUser] [nvarchar](20) NOT NULL,
[UpdateUser] [nvarchar](20) NOT NULL,
[UpdateComment] [nvarchar](150) NOT NULL,
[AppId] [int] NULL,
[Title] [nvarchar](150) NOT NULL,
[Description] [nvarchar](200) NULL,
[Content] [ntext] NOT NULL,
[IsPublished] [bit] NULL,
[IsPublic] [bit] NULL,
[CategoryId] [int] NULL,
[StartDate] [datetime] NOT NULL,
[EndDate] [datetime] NOT NULL,
[StartTime] [int] NULL,
[EndTime] [int] NULL,
[Year] [int] NULL,
[Month] [int] NULL,
[Day] [int] NULL,
[IsFeatured] [bit] NULL,
[IsTravelRequired] [bit] NULL,
[IsConference] [bit] NULL,
[IsAllTimes] [bit] NULL,
[Cost] [decimal] NULL,
[Skill] [int] NULL,
[Seats] [int] NULL,
[IsAgeApplicable] [bit] NULL,
[AgeFrom] [int] NULL,
[AgeTo] [int] NULL,
[HasImages] [bit] NULL,
[Email] [nvarchar](30) NULL,
[Phone] [nvarchar](14) NULL,
[Url] [nvarchar](150) NULL,
[Tags] [nvarchar](80) NULL,
[RefKey] [nvarchar](20) NULL,
[AverageRating] [int] NULL,
[TotalLiked] [int] NULL,
[TotalDisLiked] [int] NULL,
[TotalBookMarked] [int] NULL,
[TotalAbuseReports] [int] NULL,
[Street] [nvarchar](60) NULL,
[City] [nvarchar](30) NULL,
[State] [nvarchar](20) NULL,
[Country] [nvarchar](20) NULL,
[Zip] [nvarchar](10) NULL,
[CityId] [int] NULL,
[StateId] [int] NULL,
[CountryId] [int] NULL,
[IsOnline] [bit] NULL,
 CONSTRAINT [PK_Events] PRIMARY KEY CLUSTERED 
( [Id] ASC
 )WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
 ) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

go
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Events_GetByFilter]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[Events_GetByFilter]
go

CREATE PROCEDURE [dbo].[Events_GetByFilter]
(
	@Filter varchar(250),
	@PageIndex int,
	@PageSize int,
	@TotalRows int output
)
AS

BEGIN
	SET NOCOUNT ON;
	SET ANSI_NULLS ON
	SET QUOTED_IDENTIFIER ON

	-- Create a temp table TO store the select results
    CREATE TABLE #posts
    (
        row int IDENTITY (0, 1) NOT NULL,
        id int
    )
	declare @sql as nvarchar(4000)
	
	-- create dynamic sql
	set @sql = 'SELECT Events.id FROM Events '
	if @Filter is not null and @Filter <> ''
		set @sql = @sql + 'WHERE ' + Convert(VarChar(250), @Filter)

	-- print @sql
	-- If owner, get back all the posts, regardless if they expired or not.
	INSERT INTO #posts 
		exec sp_executesql @sql
	
	-- Now obtain the Events in the page requested
	SELECT    Events.*
	FROM      Events
	WHERE	  Events.Id IN
			(
				SELECT	id 
				FROM  #posts
				WHERE row between ((@PageIndex - 1) * @PageSize) and ( @PageIndex * @PageSize ) -1
			)
	ORDER BY CreateDate desc
	
	-- Get the total number of records that matched the search.
	select @TotalRows = count(*) from #posts
END
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Events_GetRecent]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[Events_GetRecent]
go


CREATE PROCEDURE [dbo].[Events_GetRecent]
(
	@PageIndex int,
	@PageSize int,
	@TotalRows int output
)
AS

BEGIN
	SET NOCOUNT ON;
	SET ANSI_NULLS ON
	SET QUOTED_IDENTIFIER ON

	-- Create a temp table TO store the select results
    CREATE TABLE #posts
    (
        row int IDENTITY (0, 1) NOT NULL,
        id int
    )

	-- If owner, get back all the posts, regardless if they expired or not.
	INSERT INTO #posts (id)
			SELECT Id 
			FROM   Events with ( nolock )
			ORDER BY CreateDate desc

	-- Now obtain the Events in the page requested
	SELECT    Events.*
	FROM      Events
	WHERE	  Events.Id IN
			(
				SELECT	id 
				FROM  #posts
				WHERE row between ((@PageIndex - 1) * @PageSize) and ( @PageIndex * @PageSize ) -1
			)
	ORDER BY CreateDate desc

	-- Get the total number of records that matched the search.
	select @TotalRows = count(*) from #posts
END

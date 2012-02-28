IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[MediaFiles]') AND type in (N'U'))
DROP TABLE [dbo].[MediaFiles]
go
SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
SET ANSI_PADDING ON
CREATE TABLE [dbo].[MediaFiles]( 
[Id] [int] IDENTITY(1,1) NOT NULL,
[CreateDate] [datetime] NOT NULL,
[UpdateDate] [datetime] NOT NULL,
[CreateUser] [nvarchar](20) NOT NULL,
[UpdateUser] [nvarchar](20) NOT NULL,
[UpdateComment] [nvarchar](150) NOT NULL,
[AppId] [int] NULL,
[Name] [nvarchar](50) NOT NULL,
[FullName] [nvarchar](200) NOT NULL,
[DirectoryName] [nvarchar](25) NOT NULL,
[Extension] [nvarchar](5) NOT NULL,
[Title] [nvarchar](100) NOT NULL,
[Description] [nvarchar](200) NOT NULL,
[LastWriteTime] [datetime] NULL,
[Length] [int] NULL,
[Contents] [image] NOT NULL,
[ContentsForThumbNail] [image] NOT NULL,
[Height] [int] NULL,
[Width] [int] NULL,
[SortIndex] [int] NULL,
[RefGroupId] [int] NULL,
[RefId] [int] NULL,
[RefImageId] [int] NULL,
[ParentId] [int] NULL,
[FileType] [int] NULL,
[IsPublic] [bit] NULL,
[IsExternalFile] [bit] NULL,
[ExternalFileSource] [nvarchar](20) NULL,
[HasThumbnail] [bit] NULL,
 CONSTRAINT [PK_MediaFiles] PRIMARY KEY CLUSTERED 
( [Id] ASC
 )WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
 ) ON [PRIMARY] 

go
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[MediaFiles_GetByFilter]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[MediaFiles_GetByFilter]
go

CREATE PROCEDURE [dbo].[MediaFiles_GetByFilter]
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
	set @sql = 'SELECT MediaFiles.id FROM MediaFiles '
	if @Filter is not null and @Filter <> ''
		set @sql = @sql + 'WHERE ' + Convert(VarChar(250), @Filter)

	-- print @sql
	-- If owner, get back all the posts, regardless if they expired or not.
	INSERT INTO #posts 
		exec sp_executesql @sql
	
	-- Now obtain the MediaFiles in the page requested
	SELECT    MediaFiles.*
	FROM      MediaFiles
	WHERE	  MediaFiles.Id IN
			(
				SELECT	id 
				FROM  #posts
				WHERE row between ((@PageIndex - 1) * @PageSize) and ( @PageIndex * @PageSize ) -1
			)
	ORDER BY CreateDate desc
	
	-- Get the total number of records that matched the search.
	select @TotalRows = count(*) from #posts
END
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[MediaFiles_GetRecent]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[MediaFiles_GetRecent]
go


CREATE PROCEDURE [dbo].[MediaFiles_GetRecent]
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
			FROM   MediaFiles with ( nolock )
			ORDER BY CreateDate desc

	-- Now obtain the MediaFiles in the page requested
	SELECT    MediaFiles.*
	FROM      MediaFiles
	WHERE	  MediaFiles.Id IN
			(
				SELECT	id 
				FROM  #posts
				WHERE row between ((@PageIndex - 1) * @PageSize) and ( @PageIndex * @PageSize ) -1
			)
	ORDER BY CreateDate desc

	-- Get the total number of records that matched the search.
	select @TotalRows = count(*) from #posts
END

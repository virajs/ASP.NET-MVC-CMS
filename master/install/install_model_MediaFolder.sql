IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[MediaFolders]') AND type in (N'U'))
DROP TABLE [dbo].[MediaFolders]
go
SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
SET ANSI_PADDING ON
CREATE TABLE [dbo].[MediaFolders]( 
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
[Description] [nvarchar](200) NOT NULL,
[Length] [int] NULL,
[LastWriteTime] [datetime] NULL,
[SortIndex] [int] NULL,
[ParentId] [int] NULL,
[FileType] [int] NULL,
[IsPublic] [bit] NULL,
[IsExternalFile] [bit] NULL,
[ExternalFileSource] [nvarchar](20) NULL,
[HasThumbnail] [bit] NULL,
 CONSTRAINT [PK_MediaFolders] PRIMARY KEY CLUSTERED 
( [Id] ASC
 )WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
 ) ON [PRIMARY] 

go
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[MediaFolders_GetByFilter]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[MediaFolders_GetByFilter]
go

CREATE PROCEDURE [dbo].[MediaFolders_GetByFilter]
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
	set @sql = 'SELECT MediaFolders.id FROM MediaFolders '
	if @Filter is not null and @Filter <> ''
		set @sql = @sql + 'WHERE ' + Convert(VarChar(250), @Filter)

	-- print @sql
	-- If owner, get back all the posts, regardless if they expired or not.
	INSERT INTO #posts 
		exec sp_executesql @sql
	
	-- Now obtain the MediaFolders in the page requested
	SELECT    MediaFolders.*
	FROM      MediaFolders
	WHERE	  MediaFolders.Id IN
			(
				SELECT	id 
				FROM  #posts
				WHERE row between ((@PageIndex - 1) * @PageSize) and ( @PageIndex * @PageSize ) -1
			)
	ORDER BY CreateDate desc
	
	-- Get the total number of records that matched the search.
	select @TotalRows = count(*) from #posts
END
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[MediaFolders_GetRecent]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[MediaFolders_GetRecent]
go


CREATE PROCEDURE [dbo].[MediaFolders_GetRecent]
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
			FROM   MediaFolders with ( nolock )
			ORDER BY CreateDate desc

	-- Now obtain the MediaFolders in the page requested
	SELECT    MediaFolders.*
	FROM      MediaFolders
	WHERE	  MediaFolders.Id IN
			(
				SELECT	id 
				FROM  #posts
				WHERE row between ((@PageIndex - 1) * @PageSize) and ( @PageIndex * @PageSize ) -1
			)
	ORDER BY CreateDate desc

	-- Get the total number of records that matched the search.
	select @TotalRows = count(*) from #posts
END

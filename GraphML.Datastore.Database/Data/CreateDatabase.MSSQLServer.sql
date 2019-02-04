-- assumes database collation is:
--    Latin1_General_CI_AI

-- drop relationship tables


-- drop data tables
DROP TABLE IF EXISTS EdgeItemAttribute;
DROP TABLE IF EXISTS NodeItemAttribute;
DROP TABLE IF EXISTS GraphItemAttribute;
DROP TABLE IF EXISTS RepositoryItemAttribute;
DROP TABLE IF EXISTS Edge;
DROP TABLE IF EXISTS Node;
DROP TABLE IF EXISTS Graph;
DROP TABLE IF EXISTS Repository;
DROP TABLE IF EXISTS RepositoryManager;

DROP TABLE IF EXISTS Log;


-- create data tables
-- NOTE:  maximum text field lengths is 425 characters because 
--        max index size (on relationship tables) is 1700 bytes (425 = 1700/4)

CREATE TABLE Log 
(
  Timestamp DATETIME2,
  Loglevel TEXT,
  Callsite TEXT,
  Message TEXT
);

-- RepositoryManager.csv
CREATE TABLE RepositoryManager
(
  Id NVARCHAR(36) NOT NULL UNIQUE,
  OwnerId NVARCHAR(36) NOT NULL,
  Name NVARCHAR(MAX) NOT NULL,
  OrganisationId NVARCHAR(425) NOT NULL UNIQUE,
  PRIMARY KEY (Id)
);

-- Repository.csv
CREATE TABLE Repository
(
  Id NVARCHAR(36) NOT NULL UNIQUE,
  OwnerId NVARCHAR(36) NOT NULL,
  Name NVARCHAR(MAX) NOT NULL,
  PRIMARY KEY (Id),
  FOREIGN KEY (OwnerId) REFERENCES RepositoryManager(Id) ON DELETE CASCADE
);

-- Graph.csv
CREATE TABLE Graph
(
  Id NVARCHAR(36) NOT NULL UNIQUE,
  OwnerId NVARCHAR(36) NOT NULL,
  Name NVARCHAR(MAX) NOT NULL,
  PRIMARY KEY (Id),
  FOREIGN KEY (OwnerId) REFERENCES Repository(Id) ON DELETE CASCADE
);

-- Node.csv
CREATE TABLE Node
(
  Id NVARCHAR(36) NOT NULL UNIQUE,
  NextId NVARCHAR(36) DEFAULT NULL,
  OwnerId NVARCHAR(36) NOT NULL,
  Name NVARCHAR(MAX) NOT NULL,
  PRIMARY KEY (Id),
  FOREIGN KEY (NextId) REFERENCES Node(Id),
  FOREIGN KEY (OwnerId) REFERENCES Graph(Id) ON DELETE CASCADE
);

-- Edge.csv
CREATE TABLE Edge
(
  Id NVARCHAR(36) NOT NULL UNIQUE,
  NextId NVARCHAR(36) DEFAULT NULL,
  OwnerId NVARCHAR(36) NOT NULL,
  SourceId NVARCHAR(36) NOT NULL,
  TargetId NVARCHAR(36) NOT NULL,
  Directed INTEGER DEFAULT 0,
  Name NVARCHAR(MAX) NOT NULL,
  PRIMARY KEY (Id),
  FOREIGN KEY (NextId) REFERENCES Edge(Id),
  FOREIGN KEY (OwnerId) REFERENCES Graph(Id) ON DELETE CASCADE,
  FOREIGN KEY (SourceId) REFERENCES Node(Id) ON DELETE NO ACTION,
  FOREIGN KEY (TargetId) REFERENCES Node(Id) ON DELETE NO ACTION
);

-- RepositoryItemAttribute.csv
CREATE TABLE RepositoryItemAttribute
(
  Id NVARCHAR(36) NOT NULL UNIQUE,
  Name NVARCHAR(MAX) NOT NULL,
  OwnerId NVARCHAR(36) NOT NULL,
  DataType NVARCHAR(MAX) NOT NULL,
  DataValueAsString NVARCHAR(MAX),
  PRIMARY KEY (Id),
  FOREIGN KEY (OwnerId) REFERENCES Repository(Id) ON DELETE CASCADE
);

-- GraphItemAttribute.csv
CREATE TABLE GraphItemAttribute
(
  Id NVARCHAR(36) NOT NULL UNIQUE,
  Name NVARCHAR(MAX) NOT NULL,
  OwnerId NVARCHAR(36) NOT NULL,
  DataType NVARCHAR(MAX) NOT NULL,
  DataValueAsString NVARCHAR(MAX),
  PRIMARY KEY (Id),
  FOREIGN KEY (OwnerId) REFERENCES Graph(Id) ON DELETE CASCADE
);

-- NodeItemAttribute.csv
CREATE TABLE NodeItemAttribute
(
  Id NVARCHAR(36) NOT NULL UNIQUE,
  Name NVARCHAR(MAX) NOT NULL,
  OwnerId NVARCHAR(36) NOT NULL,
  DataType NVARCHAR(MAX) NOT NULL,
  DataValueAsString NVARCHAR(MAX),
  PRIMARY KEY (Id),
  FOREIGN KEY (OwnerId) REFERENCES Node(Id) ON DELETE CASCADE
);

-- EdgeItemAttribute.csv
CREATE TABLE EdgeItemAttribute
(
  Id NVARCHAR(36) NOT NULL UNIQUE,
  Name NVARCHAR(MAX) NOT NULL,
  OwnerId NVARCHAR(36) NOT NULL,
  DataType NVARCHAR(MAX) NOT NULL,
  DataValueAsString NVARCHAR(MAX),
  PRIMARY KEY (Id),
  FOREIGN KEY (OwnerId) REFERENCES Edge(Id) ON DELETE CASCADE
);


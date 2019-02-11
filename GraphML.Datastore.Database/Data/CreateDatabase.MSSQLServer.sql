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
DROP TABLE IF EXISTS Contact;
DROP TABLE IF EXISTS Organisation;

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

CREATE TABLE Organisation
(
  Id NVARCHAR(36) NOT NULL UNIQUE,
  Name NVARCHAR(MAX) NOT NULL,
  PRIMARY KEY (Id)
);

CREATE TABLE Contact
(
  Id NVARCHAR(36) NOT NULL UNIQUE,
  OwnerId NVARCHAR(36) NOT NULL,
  Name NVARCHAR(MAX) NOT NULL,
  PRIMARY KEY (Id),
  FOREIGN KEY (OwnerId) REFERENCES Organisation(Id) ON DELETE CASCADE
);

CREATE TABLE RepositoryManager
(
  Id NVARCHAR(36) NOT NULL UNIQUE,
  OwnerId NVARCHAR(36) NOT NULL,
  Name NVARCHAR(MAX) NOT NULL,
  PRIMARY KEY (Id),
  FOREIGN KEY (OwnerId) REFERENCES Organisation(Id) ON DELETE CASCADE
);
CREATE INDEX IDX_RepositoryManager_Organisation ON RepositoryManager(OwnerId);

CREATE TABLE Repository
(
  Id NVARCHAR(36) NOT NULL UNIQUE,
  OwnerId NVARCHAR(36) NOT NULL,
  Name NVARCHAR(MAX) NOT NULL,
  PRIMARY KEY (Id),
  FOREIGN KEY (OwnerId) REFERENCES RepositoryManager(Id) ON DELETE CASCADE
);
CREATE INDEX IDX_Repository_RepositoryManager ON Repository(OwnerId);

CREATE TABLE Graph
(
  Id NVARCHAR(36) NOT NULL UNIQUE,
  OwnerId NVARCHAR(36) NOT NULL,
  Name NVARCHAR(MAX) NOT NULL,
  PRIMARY KEY (Id),
  FOREIGN KEY (OwnerId) REFERENCES Repository(Id) ON DELETE CASCADE
);
CREATE INDEX IDX_Graph_Repository ON Graph(OwnerId);

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
CREATE INDEX IDX_Node_Node ON Node(NextId);
CREATE INDEX IDX_Node_Graph ON Node(OwnerId);

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
CREATE INDEX IDX_Edge_NextId ON Edge(NextId);
CREATE INDEX IDX_Edge_OwnerId ON Edge(OwnerId);
CREATE INDEX IDX_Edge_SourceId ON Edge(SourceId);
CREATE INDEX IDX_Edge_TargetId ON Edge(TargetId);

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
CREATE INDEX IDX_RepositoryItemAttribute_Repository ON RepositoryItemAttribute(OwnerId);

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
CREATE INDEX IDX_GraphItemAttribute_Graph ON GraphItemAttribute(OwnerId);

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
CREATE INDEX IDX_NodeItemAttribute_Node ON NodeItemAttribute(OwnerId);

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
CREATE INDEX IDX_EdgeItemAttribute_Edge ON EdgeItemAttribute(OwnerId);


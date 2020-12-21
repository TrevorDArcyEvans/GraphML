-- assumes database collation is:
--    utf8_unicode_ci

-- drop relationship tables
DROP TABLE IF EXISTS ContactsRoles;


-- drop data tables
DROP TABLE IF EXISTS EdgeItemAttribute;
DROP TABLE IF EXISTS NodeItemAttribute;
DROP TABLE IF EXISTS GraphItemAttribute;
DROP TABLE IF EXISTS RepositoryItemAttribute;

DROP TABLE IF EXISTS GraphEdge;
DROP TABLE IF EXISTS GraphNode;
DROP TABLE IF EXISTS Graph;

DROP TABLE IF EXISTS Edge;
DROP TABLE IF EXISTS Node;

DROP TABLE IF EXISTS Repository;
DROP TABLE IF EXISTS RepositoryManager;
DROP TABLE IF EXISTS Contact;
DROP TABLE IF EXISTS Organisation;

DROP TABLE IF EXISTS Role;

DROP TABLE IF EXISTS Log;


-- create data tables



CREATE TABLE Log 
(
  Timestamp DATETIME,
  Loglevel TEXT,
  Callsite TEXT,
  Message TEXT
);
CREATE INDEX IDX_Timestamp ON Log(Timestamp);

CREATE TABLE Role
(
  Id CHAR(38) NOT NULL UNIQUE,
  PRIMARY KEY (Id)
);

CREATE TABLE Organisation
(
  Id CHAR(38) NOT NULL UNIQUE,
  Name TEXT NOT NULL,
  PRIMARY KEY (Id)
);

CREATE TABLE Contact
(
  Id CHAR(38) NOT NULL UNIQUE,
  OwnerId CHAR(38) NOT NULL,
  Name TEXT NOT NULL,
  PRIMARY KEY (Id),
  FOREIGN KEY (OwnerId) REFERENCES Organisation(Id) ON DELETE CASCADE
);

CREATE TABLE RepositoryManager
(
  Id CHAR(38) NOT NULL UNIQUE,
  OwnerId CHAR(38) NOT NULL,
  Name TEXT NOT NULL,
  PRIMARY KEY (Id),
  FOREIGN KEY (OwnerId) REFERENCES Organisation(Id) ON DELETE CASCADE
);
CREATE INDEX IDX_RepositoryManager_Organisation ON RepositoryManager(OwnerId);

CREATE TABLE Repository
(
  Id CHAR(38) NOT NULL UNIQUE,
  OwnerId CHAR(38) NOT NULL,
  Name TEXT NOT NULL,
  PRIMARY KEY (Id),
  FOREIGN KEY (OwnerId) REFERENCES RepositoryManager(Id) ON DELETE CASCADE
);
CREATE INDEX IDX_Repository_RepositoryManager ON Repository(OwnerId);

CREATE TABLE Node
(
  Id CHAR(38) NOT NULL UNIQUE,
  OwnerId CHAR(38) NOT NULL,
  Name TEXT NOT NULL,
  NextId CHAR(38) DEFAULT NULL,
  PRIMARY KEY (Id),
  FOREIGN KEY (NextId) REFERENCES Node(Id),
  FOREIGN KEY (OwnerId) REFERENCES Repository(Id) ON DELETE CASCADE
);
CREATE INDEX IDX_Node_Node ON Node(NextId);
CREATE INDEX IDX_Node_Graph ON Node(OwnerId);

CREATE TABLE Edge
(
  Id CHAR(38) NOT NULL UNIQUE,
  OwnerId CHAR(38) NOT NULL,
  Name TEXT NOT NULL,
  NextId CHAR(38) DEFAULT NULL,
  SourceId CHAR(38) NOT NULL,
  TargetId CHAR(38) NOT NULL,
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

CREATE TABLE Graph
(
  Id CHAR(38) NOT NULL UNIQUE,
  OwnerId CHAR(38) NOT NULL,
  Name TEXT NOT NULL,
  Directed INTEGER DEFAULT 1,
  PRIMARY KEY (Id),
  FOREIGN KEY (OwnerId) REFERENCES Repository(Id) ON DELETE CASCADE
);
CREATE INDEX IDX_Graph_Repository ON Graph(OwnerId);

CREATE TABLE GraphNode
(
  Id CHAR(38) NOT NULL UNIQUE,
  OwnerId CHAR(38) NOT NULL,
  Name NVARCHAR(MAX) NOT NULL,
  RepositoryItemId CHAR(38) NOT NULL,
  PRIMARY KEY (Id),
  FOREIGN KEY (OwnerId) REFERENCES Graph(Id) ON DELETE NO ACTION,
  FOREIGN KEY (RepositoryItemId) REFERENCES Node(Id) ON DELETE CASCADE
);
CREATE INDEX IDX_GraphNode_Graph ON GraphNode(OwnerId);

CREATE TABLE GraphEdge
(
  Id CHAR(38) NOT NULL UNIQUE,
  OwnerId CHAR(38) NOT NULL,
  Name NVARCHAR(MAX) NOT NULL,
  RepositoryItemId CHAR(38) NOT NULL,
  PRIMARY KEY (Id),
  FOREIGN KEY (OwnerId) REFERENCES Graph(Id) ON DELETE NO ACTION,
  FOREIGN KEY (RepositoryItemId) REFERENCES Edge(Id) ON DELETE CASCADE
);
CREATE INDEX IDX_GraphEdge_OwnerId ON GraphEdge(OwnerId);


-- item attributes
CREATE TABLE RepositoryItemAttribute
(
  Id CHAR(38) NOT NULL UNIQUE,
  Name TEXT NOT NULL,
  OwnerId CHAR(38) NOT NULL,
  DataType TEXT NOT NULL,
  DataValueAsString TEXT,
  PRIMARY KEY (Id),
  FOREIGN KEY (OwnerId) REFERENCES Repository(Id) ON DELETE CASCADE
);
CREATE INDEX IDX_RepositoryItemAttribute_Repository ON RepositoryItemAttribute(OwnerId);

CREATE TABLE GraphItemAttribute
(
  Id CHAR(38) NOT NULL UNIQUE,
  Name TEXT NOT NULL,
  OwnerId CHAR(38) NOT NULL,
  DataType TEXT NOT NULL,
  DataValueAsString TEXT,
  PRIMARY KEY (Id),
  FOREIGN KEY (OwnerId) REFERENCES Graph(Id) ON DELETE CASCADE
);
CREATE INDEX IDX_GraphItemAttribute_Graph ON GraphItemAttribute(OwnerId);

CREATE TABLE NodeItemAttribute
(
  Id CHAR(38) NOT NULL UNIQUE,
  Name TEXT NOT NULL,
  OwnerId CHAR(38) NOT NULL,
  DataType TEXT NOT NULL,
  DataValueAsString TEXT,
  PRIMARY KEY (Id),
  FOREIGN KEY (OwnerId) REFERENCES Node(Id) ON DELETE CASCADE
);
CREATE INDEX IDX_NodeItemAttribute_Node ON NodeItemAttribute(OwnerId);

CREATE TABLE EdgeItemAttribute
(
  Id CHAR(38) NOT NULL UNIQUE,
  Name TEXT NOT NULL,
  OwnerId CHAR(38) NOT NULL,
  DataType TEXT NOT NULL,
  DataValueAsString TEXT,
  PRIMARY KEY (Id),
  FOREIGN KEY (OwnerId) REFERENCES Edge(Id) ON DELETE CASCADE
);
CREATE INDEX IDX_EdgeItemAttribute_Edge ON EdgeItemAttribute(OwnerId);


-- create relationship tables
CREATE TABLE ContactsRoles
(
  ContactId CHAR(38) NOT NULL,
  RoleId CHAR(38) NOT NULL,
  FOREIGN KEY (ContactId) REFERENCES Contact(Id) ON DELETE CASCADE,
  FOREIGN KEY (RoleId) REFERENCES Role(Id) ON DELETE CASCADE,
  PRIMARY KEY (ContactId, RoleId)
);

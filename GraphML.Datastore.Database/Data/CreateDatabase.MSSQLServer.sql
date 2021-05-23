-- assumes database collation is:
--    Latin1_General_CI_AI

-- drop relationship tables
DROP TABLE IF EXISTS ContactsRoles;


-- drop data tables
DROP TABLE IF EXISTS EdgeItemAttribute;
DROP TABLE IF EXISTS NodeItemAttribute;
DROP TABLE IF EXISTS GraphItemAttribute;
DROP TABLE IF EXISTS RepositoryItemAttribute;

DROP TABLE IF EXISTS EdgeItemAttributeDefinition;
DROP TABLE IF EXISTS NodeItemAttributeDefinition;
DROP TABLE IF EXISTS GraphItemAttributeDefinition;
DROP TABLE IF EXISTS RepositoryItemAttributeDefinition;

DROP TABLE IF EXISTS TimelineEdge;
DROP TABLE IF EXISTS TimelineNode;
DROP TABLE IF EXISTS Timeline;

DROP TABLE IF EXISTS ChartEdge;
DROP TABLE IF EXISTS ChartNode;
DROP TABLE IF EXISTS Chart;

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
-- NOTE:  maximum text field lengths is 425 characters because 
--        max index size (on relationship tables) is 1700 bytes (425 = 1700/4)

CREATE TABLE Log 
(
  Timestamp DATETIME2,
  Loglevel TEXT,
  Callsite TEXT,
  Message TEXT
);
CREATE INDEX IDX_Timestamp ON Log(Timestamp);

-- top level entities
CREATE TABLE Organisation
(
  Id NVARCHAR(36) NOT NULL UNIQUE,
  OrganisationId NVARCHAR(36) NOT NULL,
  Name NVARCHAR(MAX) NOT NULL,
  PRIMARY KEY (Id)
);

CREATE TABLE Role
(
  Id NVARCHAR(36) NOT NULL UNIQUE,
  OrganisationId NVARCHAR(36) NOT NULL,
  Name NVARCHAR(MAX) NOT NULL,
  PRIMARY KEY (Id),
  FOREIGN KEY (OrganisationId) REFERENCES Organisation(Id) ON DELETE CASCADE
);

CREATE TABLE Contact
(
  Id NVARCHAR(36) NOT NULL UNIQUE,
  OrganisationId NVARCHAR(36) NOT NULL,
  OwnerId NVARCHAR(36) NOT NULL,
  Name NVARCHAR(MAX) NOT NULL,
  PRIMARY KEY (Id),
  FOREIGN KEY (OrganisationId) REFERENCES Organisation(Id) ON DELETE CASCADE,
  FOREIGN KEY (OwnerId) REFERENCES Organisation(Id) ON DELETE CASCADE
);

CREATE TABLE RepositoryManager
(
  Id NVARCHAR(36) NOT NULL UNIQUE,
  OrganisationId NVARCHAR(36) NOT NULL,
  OwnerId NVARCHAR(36) NOT NULL,
  Name NVARCHAR(MAX) NOT NULL,
  PRIMARY KEY (Id),
  FOREIGN KEY (OrganisationId) REFERENCES Organisation(Id) ON DELETE CASCADE,
  FOREIGN KEY (OwnerId) REFERENCES Organisation(Id) ON DELETE CASCADE
);
CREATE INDEX IDX_RepositoryManager_Organisation ON RepositoryManager(OwnerId);


-- repository entities
CREATE TABLE Repository
(
  Id NVARCHAR(36) NOT NULL UNIQUE,
  OrganisationId NVARCHAR(36) NOT NULL,
  OwnerId NVARCHAR(36) NOT NULL,
  Name NVARCHAR(MAX) NOT NULL,
  PRIMARY KEY (Id),
  FOREIGN KEY (OrganisationId) REFERENCES Organisation(Id) ON DELETE CASCADE,
  FOREIGN KEY (OwnerId) REFERENCES RepositoryManager(Id) ON DELETE CASCADE
);
CREATE INDEX IDX_Repository_RepositoryManager ON Repository(OwnerId);

CREATE TABLE Node
(
  Id NVARCHAR(36) NOT NULL UNIQUE,
  OrganisationId NVARCHAR(36) NOT NULL,
  OwnerId NVARCHAR(36) NOT NULL,
  Name NVARCHAR(MAX) NOT NULL,
  NextId NVARCHAR(36) DEFAULT NULL,
  PRIMARY KEY (Id),
  FOREIGN KEY (OrganisationId) REFERENCES Organisation(Id) ON DELETE CASCADE,
  FOREIGN KEY (OwnerId) REFERENCES Repository(Id) ON DELETE CASCADE,
  FOREIGN KEY (NextId) REFERENCES Node(Id)
);
CREATE INDEX IDX_Node_Node ON Node(NextId);
CREATE INDEX IDX_Node_Graph ON Node(OwnerId);

CREATE TABLE Edge
(
  Id NVARCHAR(36) NOT NULL UNIQUE,
  OrganisationId NVARCHAR(36) NOT NULL,
  OwnerId NVARCHAR(36) NOT NULL,
  Name NVARCHAR(MAX) NOT NULL,
  NextId NVARCHAR(36) DEFAULT NULL,
  SourceId NVARCHAR(36) NOT NULL,
  TargetId NVARCHAR(36) NOT NULL,
  PRIMARY KEY (Id),
  FOREIGN KEY (OrganisationId) REFERENCES Organisation(Id) ON DELETE CASCADE,
  FOREIGN KEY (OwnerId) REFERENCES Repository(Id) ON DELETE CASCADE,
  FOREIGN KEY (NextId) REFERENCES Edge(Id),
  FOREIGN KEY (SourceId) REFERENCES Node(Id) ON DELETE NO ACTION,
  FOREIGN KEY (TargetId) REFERENCES Node(Id) ON DELETE NO ACTION
);
CREATE INDEX IDX_Edge_NextId ON Edge(NextId);
CREATE INDEX IDX_Edge_OwnerId ON Edge(OwnerId);
CREATE INDEX IDX_Edge_SourceId ON Edge(SourceId);
CREATE INDEX IDX_Edge_TargetId ON Edge(TargetId);


-- graph entities
CREATE TABLE Graph
(
  Id NVARCHAR(36) NOT NULL UNIQUE,
  OrganisationId NVARCHAR(36) NOT NULL,
  OwnerId NVARCHAR(36) NOT NULL,
  Name NVARCHAR(MAX) NOT NULL,
  Directed INTEGER DEFAULT 1,
  PRIMARY KEY (Id),
  FOREIGN KEY (OrganisationId) REFERENCES Organisation(Id) ON DELETE CASCADE,
  FOREIGN KEY (OwnerId) REFERENCES Repository(Id) ON DELETE CASCADE
);
CREATE INDEX IDX_Graph_Repository ON Graph(OwnerId);

CREATE TABLE GraphNode
(
  Id NVARCHAR(36) NOT NULL UNIQUE,
  OrganisationId NVARCHAR(36) NOT NULL,
  OwnerId NVARCHAR(36) NOT NULL,
  Name NVARCHAR(MAX) NOT NULL,
  RepositoryItemId NVARCHAR(36) NOT NULL,
  PRIMARY KEY (Id),
  FOREIGN KEY (OrganisationId) REFERENCES Organisation(Id) ON DELETE CASCADE,
  FOREIGN KEY (OwnerId) REFERENCES Graph(Id) ON DELETE NO ACTION,
  FOREIGN KEY (RepositoryItemId) REFERENCES Node(Id) ON DELETE CASCADE
);
CREATE INDEX IDX_GraphNode_Graph ON GraphNode(OwnerId);

CREATE TABLE GraphEdge
(
  Id NVARCHAR(36) NOT NULL UNIQUE,
  OrganisationId NVARCHAR(36) NOT NULL,
  OwnerId NVARCHAR(36) NOT NULL,
  Name NVARCHAR(MAX) NOT NULL,
  RepositoryItemId NVARCHAR(36) NOT NULL,
  GraphSourceId NVARCHAR(36) NOT NULL,
  GraphTargetId NVARCHAR(36) NOT NULL,
  PRIMARY KEY (Id),
  FOREIGN KEY (OrganisationId) REFERENCES Organisation(Id) ON DELETE CASCADE,
  FOREIGN KEY (OwnerId) REFERENCES Graph(Id) ON DELETE NO ACTION,
  FOREIGN KEY (RepositoryItemId) REFERENCES Edge(Id) ON DELETE CASCADE,
  FOREIGN KEY (GraphSourceId) REFERENCES GraphNode(Id) ON DELETE CASCADE,
  FOREIGN KEY (GraphTargetId) REFERENCES GraphNode(Id) ON DELETE CASCADE
);
CREATE INDEX IDX_GraphEdge_OwnerId ON GraphEdge(OwnerId);


-- chart entities
CREATE TABLE Chart
(
  Id NVARCHAR(36) NOT NULL UNIQUE,
  OrganisationId NVARCHAR(36) NOT NULL,
  OwnerId NVARCHAR(36) NOT NULL,
  Name NVARCHAR(MAX) NOT NULL,
  PRIMARY KEY (Id),
  FOREIGN KEY (OrganisationId) REFERENCES Organisation(Id) ON DELETE CASCADE,
  FOREIGN KEY (OwnerId) REFERENCES Graph(Id) ON DELETE CASCADE
);
CREATE INDEX IDX_Chart_Graph ON Chart(OwnerId);

CREATE TABLE ChartNode
(
  Id NVARCHAR(36) NOT NULL UNIQUE,
  OrganisationId NVARCHAR(36) NOT NULL,
  OwnerId NVARCHAR(36) NOT NULL,
  Name NVARCHAR(MAX) NOT NULL,
  IconName NVARCHAR(MAX),
  GraphItemId NVARCHAR(36) NOT NULL,
  X INT NOT NULL,
  Y INT NOT NULL,
  PRIMARY KEY (Id),
  FOREIGN KEY (OrganisationId) REFERENCES Organisation(Id) ON DELETE CASCADE,
  FOREIGN KEY (OwnerId) REFERENCES Chart(Id) ON DELETE CASCADE,
  FOREIGN KEY (GraphItemId) REFERENCES GraphNode(Id) ON DELETE CASCADE
);
CREATE INDEX IDX_ChartNode_GraphItem ON ChartNode(OwnerId);

CREATE TABLE ChartEdge
(
  Id NVARCHAR(36) NOT NULL UNIQUE,
  OrganisationId NVARCHAR(36) NOT NULL,
  OwnerId NVARCHAR(36) NOT NULL,
  Name NVARCHAR(MAX) NOT NULL,
  GraphItemId NVARCHAR(36) NOT NULL,
  ChartSourceId NVARCHAR(36) NOT NULL,
  ChartTargetId NVARCHAR(36) NOT NULL,
  PRIMARY KEY (Id),
  FOREIGN KEY (OrganisationId) REFERENCES Organisation(Id) ON DELETE CASCADE,
  FOREIGN KEY (OwnerId) REFERENCES Chart(Id) ON DELETE CASCADE,
  FOREIGN KEY (GraphItemId) REFERENCES GraphEdge(Id) ON DELETE CASCADE,
  FOREIGN KEY (ChartSourceId) REFERENCES ChartNode(Id) ON DELETE CASCADE,
  FOREIGN KEY (ChartTargetId) REFERENCES ChartNode(Id) ON DELETE CASCADE
);
CREATE INDEX IDX_ChartEdge_GraphItem ON ChartEdge(OwnerId);


-- timeline entities
CREATE TABLE Timeline
(
  Id NVARCHAR(36) NOT NULL UNIQUE,
  OrganisationId NVARCHAR(36) NOT NULL,
  OwnerId NVARCHAR(36) NOT NULL,
  Name TEXT NOT NULL,
  PRIMARY KEY (Id),
  FOREIGN KEY (OrganisationId) REFERENCES Organisation(Id) ON DELETE CASCADE,
  FOREIGN KEY (OwnerId) REFERENCES Graph(Id) ON DELETE CASCADE
);
CREATE INDEX IDX_Chart_Timeline ON Timeline(OwnerId);

CREATE TABLE TimelineNode
(
  Id NVARCHAR(36) NOT NULL UNIQUE,
  OrganisationId NVARCHAR(36) NOT NULL,
  OwnerId NVARCHAR(36) NOT NULL,
  Name TEXT NOT NULL,
  GraphItemId NVARCHAR(36) NOT NULL,
  PRIMARY KEY (Id),
  FOREIGN KEY (OrganisationId) REFERENCES Organisation(Id) ON DELETE CASCADE,
  FOREIGN KEY (OwnerId) REFERENCES Chart(Id) ON DELETE CASCADE,
  FOREIGN KEY (GraphItemId) REFERENCES GraphNode(Id) ON DELETE CASCADE
);
CREATE INDEX IDX_TimelineNode_GraphItem ON TimelineNode(OwnerId);

CREATE TABLE TimelineEdge
(
  Id NVARCHAR(36) NOT NULL UNIQUE,
  OrganisationId NVARCHAR(36) NOT NULL,
  OwnerId NVARCHAR(36) NOT NULL,
  Name TEXT NOT NULL,
  GraphItemId NVARCHAR(36) NOT NULL,
  TimelineSourceId NVARCHAR(36) NOT NULL,
  TimelineTargetId NVARCHAR(36) NOT NULL,
  PRIMARY KEY (Id),
  FOREIGN KEY (OrganisationId) REFERENCES Organisation(Id) ON DELETE CASCADE,
  FOREIGN KEY (OwnerId) REFERENCES Chart(Id) ON DELETE CASCADE,
  FOREIGN KEY (GraphItemId) REFERENCES GraphEdge(Id) ON DELETE CASCADE,
  FOREIGN KEY (TimelineSourceId) REFERENCES TimelineNode(Id) ON DELETE CASCADE,
  FOREIGN KEY (TimelineTargetId) REFERENCES TimelineNode(Id) ON DELETE CASCADE
);
CREATE INDEX IDX_TimelineEdge_GraphItem ON TimelineEdge(OwnerId);


-- item attribute definitions
CREATE TABLE RepositoryItemAttributeDefinition
(
  Id NVARCHAR(36) NOT NULL UNIQUE,
  OrganisationId NVARCHAR(36) NOT NULL,
  OwnerId NVARCHAR(36) NOT NULL,
  Name NVARCHAR(MAX) NOT NULL,
  DataType NVARCHAR(MAX) NOT NULL,
  PRIMARY KEY (Id),
  FOREIGN KEY (OrganisationId) REFERENCES Organisation(Id) ON DELETE CASCADE,
  FOREIGN KEY (OwnerId) REFERENCES RepositoryManager(Id) ON DELETE CASCADE
);
CREATE INDEX IDX_RepositoryItemAttributeDefinition_RepositoryManager ON RepositoryItemAttributeDefinition(OwnerId);

CREATE TABLE GraphItemAttributeDefinition
(
  Id NVARCHAR(36) NOT NULL UNIQUE,
  OrganisationId NVARCHAR(36) NOT NULL,
  OwnerId NVARCHAR(36) NOT NULL,
  Name NVARCHAR(MAX) NOT NULL,
  DataType NVARCHAR(MAX) NOT NULL,
  PRIMARY KEY (Id),
  FOREIGN KEY (OrganisationId) REFERENCES Organisation(Id) ON DELETE CASCADE,
  FOREIGN KEY (OwnerId) REFERENCES RepositoryManager(Id) ON DELETE CASCADE
);
CREATE INDEX IDX_GraphItemAttributeDefinition_RepositoryManager ON GraphItemAttributeDefinition(OwnerId);

CREATE TABLE NodeItemAttributeDefinition
(
  Id NVARCHAR(36) NOT NULL UNIQUE,
  OrganisationId NVARCHAR(36) NOT NULL,
  OwnerId NVARCHAR(36) NOT NULL,
  Name NVARCHAR(MAX) NOT NULL,
  DataType NVARCHAR(MAX) NOT NULL,
  PRIMARY KEY (Id),
  FOREIGN KEY (OrganisationId) REFERENCES Organisation(Id) ON DELETE CASCADE,
  FOREIGN KEY (OwnerId) REFERENCES RepositoryManager(Id) ON DELETE CASCADE
);
CREATE INDEX IDX_NodeItemAttributeDefinition_RepositoryManager ON NodeItemAttributeDefinition(OwnerId);

CREATE TABLE EdgeItemAttributeDefinition
(
  Id NVARCHAR(36) NOT NULL UNIQUE,
  OrganisationId NVARCHAR(36) NOT NULL,
  OwnerId NVARCHAR(36) NOT NULL,
  Name NVARCHAR(MAX) NOT NULL,
  DataType NVARCHAR(MAX) NOT NULL,
  PRIMARY KEY (Id),
  FOREIGN KEY (OrganisationId) REFERENCES Organisation(Id) ON DELETE CASCADE,
  FOREIGN KEY (OwnerId) REFERENCES RepositoryManager(Id) ON DELETE CASCADE
);
CREATE INDEX IDX_EdgeItemAttributeDefinition_RepositoryManager ON EdgeItemAttributeDefinition(OwnerId);


-- item attributes
CREATE TABLE RepositoryItemAttribute
(
  Id NVARCHAR(36) NOT NULL UNIQUE,
  OrganisationId NVARCHAR(36) NOT NULL,
  OwnerId NVARCHAR(36) NOT NULL,
  Name NVARCHAR(MAX) NOT NULL,
  DefinitionId NVARCHAR(36) NOT NULL,
  DataValueAsString NVARCHAR(MAX),
  PRIMARY KEY (Id),
  FOREIGN KEY (OrganisationId) REFERENCES Organisation(Id) ON DELETE CASCADE,
  FOREIGN KEY (OwnerId) REFERENCES Repository(Id) ON DELETE CASCADE,
  FOREIGN KEY (DefinitionId) REFERENCES RepositoryItemAttributeDefinition(Id) ON DELETE CASCADE
);
CREATE INDEX IDX_RepositoryItemAttribute_Repository ON RepositoryItemAttribute(OwnerId);

CREATE TABLE GraphItemAttribute
(
  Id NVARCHAR(36) NOT NULL UNIQUE,
  OrganisationId NVARCHAR(36) NOT NULL,
  OwnerId NVARCHAR(36) NOT NULL,
  Name NVARCHAR(MAX) NOT NULL,
  DefinitionId NVARCHAR(36) NOT NULL,
  DataValueAsString NVARCHAR(MAX),
  PRIMARY KEY (Id),
  FOREIGN KEY (OrganisationId) REFERENCES Organisation(Id) ON DELETE CASCADE,
  FOREIGN KEY (OwnerId) REFERENCES Graph(Id) ON DELETE CASCADE,
  FOREIGN KEY (DefinitionId) REFERENCES GraphItemAttributeDefinition(Id) ON DELETE CASCADE
);
CREATE INDEX IDX_GraphItemAttribute_Graph ON GraphItemAttribute(OwnerId);

CREATE TABLE NodeItemAttribute
(
  Id NVARCHAR(36) NOT NULL UNIQUE,
  OrganisationId NVARCHAR(36) NOT NULL,
  OwnerId NVARCHAR(36) NOT NULL,
  Name NVARCHAR(MAX) NOT NULL,
  DefinitionId NVARCHAR(36) NOT NULL,
  DataValueAsString NVARCHAR(MAX),
  PRIMARY KEY (Id),
  FOREIGN KEY (OrganisationId) REFERENCES Organisation(Id) ON DELETE CASCADE,
  FOREIGN KEY (OwnerId) REFERENCES Node(Id) ON DELETE CASCADE,
  FOREIGN KEY (DefinitionId) REFERENCES NodeItemAttributeDefinition(Id) ON DELETE CASCADE
);
CREATE INDEX IDX_NodeItemAttribute_Node ON NodeItemAttribute(OwnerId);

CREATE TABLE EdgeItemAttribute
(
  Id NVARCHAR(36) NOT NULL UNIQUE,
  OrganisationId NVARCHAR(36) NOT NULL,
  OwnerId NVARCHAR(36) NOT NULL,
  Name NVARCHAR(MAX) NOT NULL,
  DefinitionId NVARCHAR(36) NOT NULL,
  DataValueAsString NVARCHAR(MAX),
  PRIMARY KEY (Id),
  FOREIGN KEY (OrganisationId) REFERENCES Organisation(Id) ON DELETE CASCADE,
  FOREIGN KEY (OwnerId) REFERENCES Edge(Id) ON DELETE CASCADE,
  FOREIGN KEY (DefinitionId) REFERENCES EdgeItemAttributeDefinition(Id) ON DELETE CASCADE
);
CREATE INDEX IDX_EdgeItemAttribute_Edge ON EdgeItemAttribute(OwnerId);


-- create relationship tables
CREATE TABLE ContactsRoles
(
  ContactId NVARCHAR(36) NOT NULL,
  RoleId NVARCHAR(36) NOT NULL,
  FOREIGN KEY (ContactId) REFERENCES Contact(Id) ON DELETE CASCADE,
  FOREIGN KEY (RoleId) REFERENCES Role(Id) ON DELETE CASCADE,
  PRIMARY KEY (ContactId, RoleId)
);

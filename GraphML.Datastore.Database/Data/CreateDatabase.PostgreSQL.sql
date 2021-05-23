-- assumes database collation is:
--    utf8_unicode_ci

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



CREATE TABLE Log 
(
  "Timestamp" TIMESTAMP,
  "Loglevel" TEXT,
  "Callsite" TEXT,
  "Message" TEXT
);
CREATE INDEX IDX_Timestamp ON Log("Timestamp");


-- top level entities
CREATE TABLE Organisation
(
  "Id" CHAR(36) NOT NULL UNIQUE,
  "OrganisationId" CHAR(36) NOT NULL,
  "Name" TEXT NOT NULL,
  PRIMARY KEY ("Id")
);

CREATE TABLE Role
(
  "Id" CHAR(36) NOT NULL UNIQUE,
  "OrganisationId" CHAR(36) NOT NULL,
  "Name" TEXT NOT NULL,
  PRIMARY KEY (Id),
  FOREIGN KEY ("OrganisationId") REFERENCES Organisation("Id") ON DELETE CASCADE
);

CREATE TABLE Contact
(
  "Id" CHAR(36) NOT NULL UNIQUE,
  "OrganisationId" CHAR(36) NOT NULL,
  "OwnerId" CHAR(36) NOT NULL,
  "Name" TEXT NOT NULL,
  PRIMARY KEY ("Id"),
  FOREIGN KEY ("OrganisationId") REFERENCES Organisation("Id") ON DELETE CASCADE,
  FOREIGN KEY ("OwnerId") REFERENCES Organisation("Id") ON DELETE CASCADE
);

CREATE TABLE RepositoryManager
(
  "Id" CHAR(36) NOT NULL UNIQUE,
  "OrganisationId" CHAR(36) NOT NULL,
  "OwnerId" CHAR(36) NOT NULL,
  "Name" TEXT NOT NULL,
  PRIMARY KEY ("Id"),
  FOREIGN KEY ("OrganisationId") REFERENCES Organisation("Id") ON DELETE CASCADE,
  FOREIGN KEY ("OwnerId") REFERENCES Organisation("Id") ON DELETE CASCADE
);
CREATE INDEX IDX_RepositoryManager_Organisation ON RepositoryManager("OwnerId");


-- repository entities
CREATE TABLE Repository
(
  "Id" CHAR(36) NOT NULL UNIQUE,
  "OrganisationId" CHAR(36) NOT NULL,
  "OwnerId" CHAR(36) NOT NULL,
  "Name" TEXT NOT NULL,
  PRIMARY KEY ("Id"),
  FOREIGN KEY ("OrganisationId") REFERENCES Organisation("Id") ON DELETE CASCADE,
  FOREIGN KEY ("OwnerId") REFERENCES RepositoryManager("Id") ON DELETE CASCADE
);
CREATE INDEX IDX_Repository_RepositoryManager ON Repository("OwnerId");

CREATE TABLE Node
(
  "Id" CHAR(36) NOT NULL UNIQUE,
  "OrganisationId" CHAR(36) NOT NULL,
  "OwnerId" CHAR(36) NOT NULL,
  "Name" TEXT NOT NULL,
  "NextId" CHAR(36) DEFAULT NULL,
  PRIMARY KEY ("Id"),
  FOREIGN KEY ("OrganisationId") REFERENCES Organisation("Id") ON DELETE CASCADE,
  FOREIGN KEY ("OwnerId") REFERENCES Repository("Id") ON DELETE CASCADE,
  FOREIGN KEY ("NextId") REFERENCES Node("Id")
);
CREATE INDEX IDX_Node_Node ON Node("NextId");
CREATE INDEX IDX_Node_Graph ON Node("OwnerId");

CREATE TABLE Edge
(
  "Id" CHAR(36) NOT NULL UNIQUE,
  "OrganisationId" CHAR(36) NOT NULL,
  "OwnerId" CHAR(36) NOT NULL,
  "Name" TEXT NOT NULL,
  "NextId" CHAR(36) DEFAULT NULL,
  "SourceId" CHAR(36) NOT NULL,
  "TargetId" CHAR(36) NOT NULL,
  PRIMARY KEY ("Id"),
  FOREIGN KEY ("OrganisationId") REFERENCES Organisation("Id") ON DELETE CASCADE,
  FOREIGN KEY ("OwnerId") REFERENCES Repository("Id") ON DELETE CASCADE,
  FOREIGN KEY ("NextId") REFERENCES Edge("Id"),
  FOREIGN KEY ("SourceId") REFERENCES Node("Id") ON DELETE NO ACTION,
  FOREIGN KEY ("TargetId") REFERENCES Node("Id") ON DELETE NO ACTION
);
CREATE INDEX IDX_Edge_NextId ON Edge("NextId");
CREATE INDEX IDX_Edge_OwnerId ON Edge("OwnerId");
CREATE INDEX IDX_Edge_SourceId ON Edge("SourceId");
CREATE INDEX IDX_Edge_TargetId ON Edge("TargetId");


-- item attribute definitions
CREATE TABLE RepositoryItemAttributeDefinition
(
    "Id" CHAR(36) NOT NULL UNIQUE,
    "OrganisationId" CHAR(36) NOT NULL,
    "OwnerId" CHAR(36) NOT NULL,
    "Name" NVARCHAR(MAX) NOT NULL,
    "DataType" NVARCHAR(MAX) NOT NULL,
    PRIMARY KEY ("Id"),
    FOREIGN KEY ("OrganisationId") REFERENCES Organisation("Id") ON DELETE CASCADE,
    FOREIGN KEY ("OwnerId") REFERENCES RepositoryManager("Id") ON DELETE CASCADE
);
CREATE INDEX IDX_RepositoryItemAttributeDefinition_RepositoryManager ON RepositoryItemAttributeDefinition(OwnerId);

CREATE TABLE GraphItemAttributeDefinition
(
    "Id" CHAR(36) NOT NULL UNIQUE,
    "OrganisationId" CHAR(36) NOT NULL,
    "OwnerId" CHAR(36) NOT NULL,
    "Name" NVARCHAR(MAX) NOT NULL,
    "DataType" NVARCHAR(MAX) NOT NULL,
    PRIMARY KEY ("Id"),
    FOREIGN KEY ("OrganisationId") REFERENCES Organisation("Id") ON DELETE CASCADE,
    FOREIGN KEY ("OwnerId") REFERENCES RepositoryManager("Id") ON DELETE CASCADE
);
CREATE INDEX IDX_GraphItemAttributeDefinition_RepositoryManager ON GraphItemAttributeDefinition(OwnerId);

CREATE TABLE NodeItemAttributeDefinition
(
    "Id" CHAR(36) NOT NULL UNIQUE,
    "OrganisationId" CHAR(36) NOT NULL,
    "OwnerId" CHAR(36) NOT NULL,
    "Name" NVARCHAR(MAX) NOT NULL,
    "DataType" NVARCHAR(MAX) NOT NULL,
    PRIMARY KEY ("Id"),
    FOREIGN KEY ("OrganisationId") REFERENCES Organisation("Id") ON DELETE CASCADE,
    FOREIGN KEY ("OwnerId") REFERENCES RepositoryManager("Id") ON DELETE CASCADE
);
CREATE INDEX IDX_NodeItemAttributeDefinition_RepositoryManager ON NodeItemAttributeDefinition(OwnerId);

CREATE TABLE EdgeItemAttributeDefinition
(
    "Id" CHAR(36) NOT NULL UNIQUE,
    "OrganisationId" CHAR(36) NOT NULL,
    "OwnerId" CHAR(36) NOT NULL,
    "Name" NVARCHAR(MAX) NOT NULL,
    "DataType" NVARCHAR(MAX) NOT NULL,
    PRIMARY KEY ("Id"),
    FOREIGN KEY ("OrganisationId") REFERENCES Organisation("Id") ON DELETE CASCADE,
    FOREIGN KEY ("OwnerId") REFERENCES RepositoryManager("Id") ON DELETE CASCADE
);
CREATE INDEX IDX_EdgeItemAttributeDefinition_RepositoryManager ON EdgeItemAttributeDefinition(OwnerId);


-- graph entities
CREATE TABLE Graph
(
  "Id" CHAR(36) NOT NULL UNIQUE,
  "OrganisationId" CHAR(36) NOT NULL,
  "OwnerId" CHAR(36) NOT NULL,
  "Name" TEXT NOT NULL,
  "Directed" INTEGER DEFAULT 1,
  PRIMARY KEY ("Id"),
  FOREIGN KEY ("OrganisationId") REFERENCES Organisation("Id") ON DELETE CASCADE,
  FOREIGN KEY ("OwnerId") REFERENCES Repository("Id") ON DELETE CASCADE
);
CREATE INDEX IDX_Graph_Repository ON Graph("OwnerId");

CREATE TABLE GraphNode
(
  "Id" CHAR(36) NOT NULL UNIQUE,
  "OrganisationId" CHAR(36) NOT NULL,
  "OwnerId" CHAR(36) NOT NULL,
  "Name" NVARCHAR(MAX) NOT NULL,
  "RepositoryItemId" CHAR(36) NOT NULL,
  PRIMARY KEY ("Id"),
  FOREIGN KEY ("OrganisationId") REFERENCES Organisation("Id") ON DELETE CASCADE,
  FOREIGN KEY ("OwnerId") REFERENCES Graph("Id") ON DELETE NO ACTION,
  FOREIGN KEY ("RepositoryItemId") REFERENCES Node("Id") ON DELETE CASCADE
);
CREATE INDEX IDX_GraphNode_Graph ON GraphNode("OwnerId");

CREATE TABLE GraphEdge
(
  "Id" CHAR(36) NOT NULL UNIQUE,
  "OrganisationId" CHAR(36) NOT NULL,
  "OwnerId" CHAR(36) NOT NULL,
  "Name" NVARCHAR(MAX) NOT NULL,
  "RepositoryItemId" CHAR(36) NOT NULL,
  "GraphSourceId" CHAR(36) NOT NULL,
  "GraphTargetId" CHAR(36) NOT NULL,
  PRIMARY KEY ("Id"),
  FOREIGN KEY ("OrganisationId") REFERENCES Organisation("Id") ON DELETE CASCADE,
  FOREIGN KEY ("OwnerId") REFERENCES Graph("Id") ON DELETE NO ACTION,
  FOREIGN KEY ("RepositoryItemId") REFERENCES Edge("Id") ON DELETE CASCADE,
  FOREIGN KEY ("GraphSourceId") REFERENCES GraphNode("Id") ON DELETE NO ACTION,
  FOREIGN KEY ("GraphTargetId") REFERENCES GraphNode("Id") ON DELETE NO ACTION
);
CREATE INDEX IDX_GraphEdge_OwnerId ON GraphEdge("OwnerId");


-- chart entities
CREATE TABLE Chart
(
  "Id" CHAR(36) NOT NULL UNIQUE,
  "OrganisationId" CHAR(36) NOT NULL,
  "OwnerId" CHAR(36) NOT NULL,
  "Name" NVARCHAR(MAX) NOT NULL,
  PRIMARY KEY ("Id"),
  FOREIGN KEY ("OrganisationId") REFERENCES Organisation("Id") ON DELETE CASCADE,
  FOREIGN KEY ("OwnerId") REFERENCES RepositoryManager("Id") ON DELETE CASCADE
);
CREATE INDEX IDX_Chart_Graph ON Chart(OwnerId);

CREATE TABLE ChartNode
(
  "Id" CHAR(36) NOT NULL UNIQUE,
  "OrganisationId" CHAR(36) NOT NULL,
  "OwnerId" CHAR(36) NOT NULL,
  "Name" NVARCHAR(MAX) NOT NULL,
  "IconName" NVARCHAR(MAX),
  "GraphItemId" TEXT NOT NULL,
  "X" INTEGER NOT NULL,
  "Y" INTEGER NOT NULL,
  PRIMARY KEY ("Id"),
  FOREIGN KEY ("OrganisationId") REFERENCES Organisation("Id") ON DELETE CASCADE,
  FOREIGN KEY ("OwnerId") REFERENCES RepositoryManager("Id") ON DELETE CASCADE,
  FOREIGN KEY ("GraphItemId") REFERENCES GraphNode("Id") ON DELETE CASCADE
);
CREATE INDEX IDX_ChartNode_GraphItem ON ChartNode(OwnerId);

CREATE TABLE ChartEdge
(
  "Id" CHAR(36) NOT NULL UNIQUE,
  "OrganisationId" CHAR(36) NOT NULL,
  "OwnerId" CHAR(36) NOT NULL,
  "Name" NVARCHAR(MAX) NOT NULL,
  "GraphItemId" TEXT NOT NULL,
  "ChartSourceId" TEXT NOT NULL,
  "ChartTargetId" TEXT NOT NULL,
  PRIMARY KEY ("Id"),
  FOREIGN KEY ("OrganisationId") REFERENCES Organisation("Id") ON DELETE CASCADE,
  FOREIGN KEY ("OwnerId") REFERENCES RepositoryManager("Id") ON DELETE CASCADE,
  FOREIGN KEY ("GraphItemId") REFERENCES GraphEdge("Id") ON DELETE CASCADE,
  FOREIGN KEY ("ChartSourceId") REFERENCES ChartNode("Id") ON DELETE CASCADE,
  FOREIGN KEY ("ChartTargetId") REFERENCES ChartNode("Id") ON DELETE CASCADE
);
CREATE INDEX IDX_ChartEdge_GraphItem ON ChartEdge(OwnerId);


-- timeline entities
CREATE TABLE Timeline
(
  "Id" CHAR(36) NOT NULL UNIQUE,
  "OrganisationId" CHAR(36) NOT NULL,
  "OwnerId" CHAR(36) NOT NULL,
  "Name" NVARCHAR(MAX) NOT NULL,
  "DateTimeIntervalAttributeDefinitionId" CHAR(36)  NOT NULL,
  PRIMARY KEY ("Id"),
  FOREIGN KEY ("OrganisationId") REFERENCES Organisation("Id") ON DELETE CASCADE,
  FOREIGN KEY ("OwnerId") REFERENCES Graph("Id") ON DELETE CASCADE,
  FOREIGN KEY ("DateTimeIntervalAttributeDefinitionId") REFERENCES EdgeItemAttributeDefinition("Id") ON DELETE CASCADE
);
CREATE INDEX IDX_Chart_Timeline ON Timeline(OwnerId);

CREATE TABLE TimelineNode
(
  "Id" CHAR(36) NOT NULL UNIQUE,
  "OrganisationId" CHAR(36) NOT NULL,
  "OwnerId" CHAR(36) NOT NULL,
  "Name" NVARCHAR(MAX) NOT NULL,
  "GraphItemId" CHAR(36) NOT NULL,
  PRIMARY KEY ("Id"),
  FOREIGN KEY ("OrganisationId") REFERENCES Organisation("Id") ON DELETE CASCADE,
  FOREIGN KEY ("OwnerId") REFERENCES Chart("Id") ON DELETE CASCADE,
  FOREIGN KEY ("GraphItemId") REFERENCES GraphNode("Id") ON DELETE CASCADE
);
CREATE INDEX IDX_TimelineNode_GraphItem ON TimelineNode(OwnerId);

CREATE TABLE TimelineEdge
(
  "Id" CHAR(36) NOT NULL UNIQUE,
  "OrganisationId" CHAR(36) NOT NULL,
  "OwnerId" CHAR(36) NOT NULL,
  "Name" NVARCHAR(MAX) NOT NULL,
  "GraphItemId" CHAR(36) NOT NULL,
  "TimelineSourceId" CHAR(36) NOT NULL,
  "TimelineTargetId" CHAR(36) NOT NULL,
  PRIMARY KEY ("Id"),
  FOREIGN KEY ("OrganisationId") REFERENCES Organisation("Id") ON DELETE CASCADE,
  FOREIGN KEY ("OwnerId") REFERENCES Chart("Id") ON DELETE CASCADE,
  FOREIGN KEY ("GraphItemId") REFERENCES GraphEdge("Id") ON DELETE CASCADE,
  FOREIGN KEY ("TimelineSourceId") REFERENCES TimelineNode("Id") ON DELETE CASCADE,
  FOREIGN KEY ("TimelineTargetId") REFERENCES TimelineNode("Id") ON DELETE CASCADE
);
CREATE INDEX IDX_TimelineEdge_GraphItem ON TimelineEdge(OwnerId);


-- item attributes
CREATE TABLE RepositoryItemAttribute
(
  "Id" CHAR(36) NOT NULL UNIQUE,
  "OrganisationId" CHAR(36) NOT NULL,
  "Name" NVARCHAR(MAX) NOT NULL,
  "OwnerId" CHAR(36) NOT NULL,
  "DataType" NVARCHAR(MAX) NOT NULL,
  "DataValueAsString" NVARCHAR(MAX),
  PRIMARY KEY ("Id"),
  FOREIGN KEY ("OrganisationId") REFERENCES Organisation("Id") ON DELETE CASCADE,
  FOREIGN KEY ("OwnerId") REFERENCES Repository("Id") ON DELETE CASCADE,
  FOREIGN KEY ("DefinitionId") REFERENCES RepositoryItemAttributeDefinition("Id") ON DELETE CASCADE
);
CREATE INDEX IDX_RepositoryItemAttribute_Repository ON RepositoryItemAttribute("OwnerId");

CREATE TABLE GraphItemAttribute
(
  "Id" CHAR(36) NOT NULL UNIQUE,
  "OrganisationId" CHAR(36) NOT NULL,
  "OwnerId" CHAR(36) NOT NULL,
  "Name" TEXT NOT NULL,
  "DataType" TEXT NOT NULL,
  "DataValueAsString" TEXT,
  PRIMARY KEY ("Id"),
  FOREIGN KEY ("OrganisationId") REFERENCES Organisation("Id") ON DELETE CASCADE,
  FOREIGN KEY ("OwnerId") REFERENCES Graph("Id") ON DELETE CASCADE,
  FOREIGN KEY ("DefinitionId") REFERENCES GraphItemAttributeDefinition("Id") ON DELETE CASCADE
);
CREATE INDEX IDX_GraphItemAttribute_Graph ON GraphItemAttribute("OwnerId");

CREATE TABLE NodeItemAttribute
(
  "Id" CHAR(36) NOT NULL UNIQUE,
  "OrganisationId" CHAR(36) NOT NULL,
  "OwnerId" CHAR(36) NOT NULL,
  "Name" TEXT NOT NULL,
  "DataType" TEXT NOT NULL,
  "DataValueAsString" TEXT,
  PRIMARY KEY ("Id"),
  FOREIGN KEY ("OrganisationId") REFERENCES Organisation("Id") ON DELETE CASCADE,
  FOREIGN KEY ("OwnerId") REFERENCES Node("Id") ON DELETE CASCADE,
  FOREIGN KEY ("DefinitionId") REFERENCES NodeItemAttributeDefinition("Id") ON DELETE CASCADE
);
CREATE INDEX IDX_NodeItemAttribute_Node ON NodeItemAttribute("OwnerId");

CREATE TABLE EdgeItemAttribute
(
  "Id" CHAR(36) NOT NULL UNIQUE,
  "OrganisationId" CHAR(36) NOT NULL,
  "OwnerId" CHAR(36) NOT NULL,
  "Name" TEXT NOT NULL,
  "DataType" TEXT NOT NULL,
  "DataValueAsString" TEXT,
  PRIMARY KEY ("Id"),
  FOREIGN KEY ("OrganisationId") REFERENCES Organisation("Id") ON DELETE CASCADE,
  FOREIGN KEY ("OwnerId") REFERENCES Edge("Id") ON DELETE CASCADE,
  FOREIGN KEY ("DefinitionId") REFERENCES EdgeItemAttributeDefinition("Id") ON DELETE CASCADE
);
CREATE INDEX IDX_EdgeItemAttribute_Edge ON EdgeItemAttribute("OwnerId");


-- create relationship tables
CREATE TABLE ContactsRoles
(
  "ContactId" TEXT NOT NULL,
  "RoleId" TEXT NOT NULL,
  FOREIGN KEY ("ContactId") REFERENCES Contact("Id") ON DELETE CASCADE,
  FOREIGN KEY ("RoleId") REFERENCES Role("Id") ON DELETE CASCADE,
  PRIMARY KEY ("ContactId", "RoleId")
);

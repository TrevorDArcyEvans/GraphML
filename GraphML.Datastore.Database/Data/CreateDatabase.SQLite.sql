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


-- create data tables

-- RepositoryManager.csv
CREATE TABLE RepositoryManager
(
  Id TEXT NOT NULL UNIQUE,
  Name TEXT NOT NULL,
  OwnerId TEXT NOT NULL,
  OrganisationId TEXT NOT NULL UNIQUE,
  PRIMARY KEY (Id)
);

-- Repository.csv
CREATE TABLE Repository
(
  Id TEXT NOT NULL UNIQUE,
  Name TEXT NOT NULL,
  OwnerId TEXT NOT NULL,
  PRIMARY KEY (Id),
  FOREIGN KEY (OwnerId) REFERENCES RepositoryManager(Id) ON DELETE CASCADE
);

-- Graph.csv
CREATE TABLE Graph
(
  Id TEXT NOT NULL UNIQUE,
  Name TEXT NOT NULL,
  OwnerId TEXT NOT NULL,
  PRIMARY KEY (Id),
  FOREIGN KEY (OwnerId) REFERENCES Repository(Id) ON DELETE CASCADE
);

-- Node.csv
CREATE TABLE Node
(
  Id TEXT NOT NULL UNIQUE,
  Name TEXT NOT NULL,
  OwnerId TEXT NOT NULL,
  PRIMARY KEY (Id),
  FOREIGN KEY (OwnerId) REFERENCES Graph(Id) ON DELETE CASCADE
);

-- Edge.csv
CREATE TABLE Edge
(
  Id TEXT NOT NULL UNIQUE,
  Name TEXT NOT NULL,
  OwnerId TEXT NOT NULL,
  Source TEXT NOT NULL,
  Target TEXT NOT NULL,
  Directed INTEGER DEFAULT 0,
  PRIMARY KEY (Id),
  FOREIGN KEY (OwnerId) REFERENCES Graph(Id) ON DELETE CASCADE,
  FOREIGN KEY (Source) REFERENCES Node(Id) ON DELETE CASCADE,
  FOREIGN KEY (Target) REFERENCES Node(Id) ON DELETE CASCADE
);

-- RepositoryItemAttribute.csv
CREATE TABLE RepositoryItemAttribute
(
  Id TEXT NOT NULL UNIQUE,
  Name TEXT NOT NULL,
  OwnerId TEXT NOT NULL,
  DataType TEXT NOT NULL,
  DataValueAsString TEXT,
  PRIMARY KEY (Id),
  FOREIGN KEY (OwnerId) REFERENCES Repository(Id) ON DELETE CASCADE
);

-- GraphItemAttribute.csv
CREATE TABLE GraphItemAttribute
(
  Id TEXT NOT NULL UNIQUE,
  Name TEXT NOT NULL,
  OwnerId TEXT NOT NULL,
  DataType TEXT NOT NULL,
  DataValueAsString TEXT,
  PRIMARY KEY (Id),
  FOREIGN KEY (OwnerId) REFERENCES Graph(Id) ON DELETE CASCADE
);

-- NodeItemAttribute.csv
CREATE TABLE NodeItemAttribute
(
  Id TEXT NOT NULL UNIQUE,
  Name TEXT NOT NULL,
  OwnerId TEXT NOT NULL,
  DataType TEXT NOT NULL,
  DataValueAsString TEXT,
  PRIMARY KEY (Id),
  FOREIGN KEY (OwnerId) REFERENCES Node(Id) ON DELETE CASCADE
);

-- EdgeItemAttribute.csv
CREATE TABLE EdgeItemAttribute
(
  Id TEXT NOT NULL UNIQUE,
  Name TEXT NOT NULL,
  OwnerId TEXT NOT NULL,
  DataType TEXT NOT NULL,
  DataValueAsString TEXT,
  PRIMARY KEY (Id),
  FOREIGN KEY (OwnerId) REFERENCES Edge(Id) ON DELETE CASCADE
);


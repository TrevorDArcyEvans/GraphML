INSERT INTO Organisation(Id, Name)
VALUES
  ('c018775d-ac42-46d2-bf8d-7fadced870d5', 'GraphML'),
  ('05dfbd09-3252-42a1-901b-85691802d9bc', 'Kool Organisation');

INSERT INTO Contact(Id, OwnerId, Name)
VALUES
  ('907bf1ab-fb91-494b-b1a8-376f9a9f03d8', 'c018775d-ac42-46d2-bf8d-7fadced870d5', 'admin@GraphML.com'),
  ('db9d2dc4-cf7f-4177-8f72-c04da623f1be', '05dfbd09-3252-42a1-901b-85691802d9bc', 'tester@KoolOrganisation.org');

INSERT INTO RepositoryManager(Id, OwnerId, Name)
VALUES
  ('874852c8-794a-4748-89bc-68ce50823093', 'c018775d-ac42-46d2-bf8d-7fadced870d5', 'GraphML Repository Manager'),
  ('66852486-9780-4aad-863a-b7fb2e903192', '05dfbd09-3252-42a1-901b-85691802d9bc', 'Kool Organisation Repository Manager');

INSERT INTO Repository(Id, OwnerId, Name)
VALUES
  ('b556969c-bd2c-4378-ae95-972923118295', '874852c8-794a-4748-89bc-68ce50823093', 'GraphML Repository 0'),
  ('100fba96-f33d-4242-a551-722b73bc9c6d', '874852c8-794a-4748-89bc-68ce50823093', 'GraphML Repository 1'),
  ('45783182-4912-45c5-9589-11f66c69bddb', '66852486-9780-4aad-863a-b7fb2e903192', 'Kool Organisation Repository');

INSERT INTO Graph(Id, OwnerId, Directed, Name)
VALUES
  ('470a1e4f-c0c4-400c-a726-9ffe9697135a', 'b556969c-bd2c-4378-ae95-972923118295', 0, 'GraphML Graph 0'),
  ('6d574bb5-7f80-4e33-a373-59dd18e8e47c', 'b556969c-bd2c-4378-ae95-972923118295', 0, 'GraphML Graph 1'),
  ('68e65dd3-940e-41ce-8274-6e2518390605', '100fba96-f33d-4242-a551-722b73bc9c6d', 0, 'GraphML Graph 0'),
  ('a5f2ab9c-2fe0-4072-a9ab-c38f21d364b1', '45783182-4912-45c5-9589-11f66c69bddb', 0, 'Kool Organisation Graph');

INSERT INTO Node(Id, NextId, OwnerId, Name)
VALUES
  ('c7718065-af28-40fa-8403-197bc6d23909', null, '470a1e4f-c0c4-400c-a726-9ffe9697135a', 'GraphML Node A'),
  ('9505402f-a60f-4bd2-91dc-9a02845989fa', null, '470a1e4f-c0c4-400c-a726-9ffe9697135a', 'GraphML Node B'),
  ('9fa72ac4-9b9a-4bf7-8901-c640881bf4a5', null, '470a1e4f-c0c4-400c-a726-9ffe9697135a', 'GraphML Node C'),
  ('2c962a0d-bff2-4f3d-8f8a-49c27418001b', null, '470a1e4f-c0c4-400c-a726-9ffe9697135a', 'GraphML Node D'),
  ('2c4cccb1-7873-4732-b0e3-6425d4d24922', null, '470a1e4f-c0c4-400c-a726-9ffe9697135a', 'GraphML Node E');

INSERT INTO Edge(Id, NextId, OwnerId, SourceId, TargetId, Name)
VALUES
  ('fdb37c86-62c3-42b4-a41c-8e648533609c', null, '470a1e4f-c0c4-400c-a726-9ffe9697135a', 'c7718065-af28-40fa-8403-197bc6d23909', '9505402f-a60f-4bd2-91dc-9a02845989fa', 'GraphML Edge A-B'),
  ('7e352e70-164d-4489-ba73-5a9fcb17e6cf', null, '470a1e4f-c0c4-400c-a726-9ffe9697135a', '9505402f-a60f-4bd2-91dc-9a02845989fa', '9fa72ac4-9b9a-4bf7-8901-c640881bf4a5', 'GraphML Edge B-C'),
  ('04d69adb-1da6-428f-a9d5-b5e05f2e8661', null, '470a1e4f-c0c4-400c-a726-9ffe9697135a', '9fa72ac4-9b9a-4bf7-8901-c640881bf4a5', '2c962a0d-bff2-4f3d-8f8a-49c27418001b', 'GraphML Edge C-D'),
  ('fb6f545b-0ab7-4351-b35e-8d37ae3f9cd9', null, '470a1e4f-c0c4-400c-a726-9ffe9697135a', '2c962a0d-bff2-4f3d-8f8a-49c27418001b', 'c7718065-af28-40fa-8403-197bc6d23909', 'GraphML Edge D-A');

INSERT INTO RepositoryItemAttribute(Id, OwnerId, Name, DataType, DataValueAsString)
VALUES
  ('4f7d2360-f9c8-4743-80dc-08c2e7ee22e6', 'b556969c-bd2c-4378-ae95-972923118295', 'Repository 0', 'string', 'Repository 0 Attribute A'),
  ('3a987c96-a040-4abf-a0c1-fb6c4b3b5749', 'b556969c-bd2c-4378-ae95-972923118295', 'Repository 0', 'string', 'Repository 0 Attribute B'),
  ('632a2858-a170-4692-a031-930fdb55e87d', 'b556969c-bd2c-4378-ae95-972923118295', 'Repository 0', 'string', 'Repository 0 Attribute C'),
  ('92c978ac-63aa-49ac-b70c-1c945c109d86', 'b556969c-bd2c-4378-ae95-972923118295', 'Repository 0', 'string', 'Repository 0 Attribute D');

INSERT INTO GraphItemAttribute(Id, OwnerId, Name, DataType, DataValueAsString)
VALUES
  ('e7e8a9bd-563e-4c0a-9993-fbb677b62571', '470a1e4f-c0c4-400c-a726-9ffe9697135a', 'Graph 0', 'string', 'Attribute A'),
  ('c2a5c8f0-dc06-472c-ad0e-ac0e952e8934', '470a1e4f-c0c4-400c-a726-9ffe9697135a', 'Graph 0', 'string', 'Attribute B'),
  ('ea494316-0403-4d15-ac08-67bc922b5605', '470a1e4f-c0c4-400c-a726-9ffe9697135a', 'Graph 0', 'string', 'Attribute C'),
  ('9ae347c1-d1e0-45e8-a2b4-5ab636f4d265', '470a1e4f-c0c4-400c-a726-9ffe9697135a', 'Graph 0', 'string', 'Attribute D');

INSERT INTO NodeItemAttribute(Id, OwnerId, Name, DataType, DataValueAsString)
VALUES
  ('8f245a18-7c35-484a-8579-722593af71e2', 'c7718065-af28-40fa-8403-197bc6d23909', 'Node A', 'string', 'Node A Attribute'),
  ('b191108a-7dec-4c66-8df1-87f77db9153f', '9505402f-a60f-4bd2-91dc-9a02845989fa', 'Node B', 'string', 'Node B Attribute'),
  ('004499ec-856b-422f-ae61-28b22d4328bf', '9fa72ac4-9b9a-4bf7-8901-c640881bf4a5', 'Node C', 'string', 'Node C Attribute'),
  ('43b61316-9eaf-422e-81ec-da26e5eafcf5', '2c962a0d-bff2-4f3d-8f8a-49c27418001b', 'Node D', 'string', 'Node D Attribute'),
  ('1a76cefa-de74-461b-8d56-f2d1624bc0f4', '2c4cccb1-7873-4732-b0e3-6425d4d24922', 'Node E', 'string', 'Node E Attribute');

INSERT INTO EdgeItemAttribute(Id, OwnerId, Name, DataType, DataValueAsString)
VALUES
  ('2c980b19-9fad-4f2a-a469-ae948f99c03d', 'fdb37c86-62c3-42b4-a41c-8e648533609c', 'Edge A-B', 'string', 'Edge A-B Attribute'),
  ('a707532d-05cc-49ec-8d38-55fab653e7ec', '7e352e70-164d-4489-ba73-5a9fcb17e6cf', 'Edge B-C', 'string', 'Edge B-C Attribute'),
  ('4916dc3f-4030-4e89-bd15-9d00c87c04fc', '04d69adb-1da6-428f-a9d5-b5e05f2e8661', 'Edge C-D', 'string', 'Edge C-D Attribute'),
  ('acced08b-fe29-4a1a-9b93-ca668e8b3482', 'fb6f545b-0ab7-4351-b35e-8d37ae3f9cd9', 'Edge D-A', 'string', 'Edge D-A Attribute');




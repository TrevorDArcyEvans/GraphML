-- top level entities
INSERT INTO Organisation(Id, OrganisationId, Name)
VALUES
  ('a0d9403c-f03f-480f-93e8-b7ca349645c6', 'a0d9403c-f03f-480f-93e8-b7ca349645c6', 'System'),
  ('c018775d-ac42-46d2-bf8d-7fadced870d5', 'c018775d-ac42-46d2-bf8d-7fadced870d5', 'GraphML'),
  ('05dfbd09-3252-42a1-901b-85691802d9bc', '05dfbd09-3252-42a1-901b-85691802d9bc', 'Kool Organisation');

INSERT INTO RepositoryManager(Id, OrganisationId, OwnerId, Name)
VALUES
  ('874852c8-794a-4748-89bc-68ce50823093', 'c018775d-ac42-46d2-bf8d-7fadced870d5', 'c018775d-ac42-46d2-bf8d-7fadced870d5', 'GraphML Repository Manager'),
  ('66852486-9780-4aad-863a-b7fb2e903192', '05dfbd09-3252-42a1-901b-85691802d9bc', '05dfbd09-3252-42a1-901b-85691802d9bc', 'Kool Organisation Repository Manager');


-- repository entities
INSERT INTO Repository(Id, OrganisationId, OwnerId, Name)
VALUES
  ('b556969c-bd2c-4378-ae95-972923118295', 'c018775d-ac42-46d2-bf8d-7fadced870d5', '874852c8-794a-4748-89bc-68ce50823093', 'GraphML Repository'),
  ('100fba96-f33d-4242-a551-722b73bc9c6d', '05dfbd09-3252-42a1-901b-85691802d9bc', '66852486-9780-4aad-863a-b7fb2e903192', 'Kool Organisation Repository 0'),
  ('45783182-4912-45c5-9589-11f66c69bddb', '05dfbd09-3252-42a1-901b-85691802d9bc', '66852486-9780-4aad-863a-b7fb2e903192', 'Kool Organisation Repository 1');

--  node with a merged history
--  
--  
--       z                         s
--        \                         \
--         \                         \
--  y-------w------------A------------r-----p-------n--------m
--         /            /            /
--        /            /            /
--       x      v     /            q
--               \   /
--                \ /
--                 u
--                /
--               /
--              t
INSERT INTO Node(Id, OrganisationId, OwnerId, NextId, Name)
VALUES
  ('eb74f27b-61e2-4b7a-ac01-af343ac3e29b', '05dfbd09-3252-42a1-901b-85691802d9bc', '100fba96-f33d-4242-a551-722b73bc9c6d', null, 'm'),
  ('54f73a2b-13b1-4475-bcc3-5ce2105b4d35', '05dfbd09-3252-42a1-901b-85691802d9bc', '100fba96-f33d-4242-a551-722b73bc9c6d', 'eb74f27b-61e2-4b7a-ac01-af343ac3e29b', 'n'),
  ('b647b099-d647-452c-8286-f57679a46df9', '05dfbd09-3252-42a1-901b-85691802d9bc', '100fba96-f33d-4242-a551-722b73bc9c6d', '54f73a2b-13b1-4475-bcc3-5ce2105b4d35', 'p'),
  ('73084b35-377f-47ea-a369-713d31e2941d', '05dfbd09-3252-42a1-901b-85691802d9bc', '100fba96-f33d-4242-a551-722b73bc9c6d', 'b647b099-d647-452c-8286-f57679a46df9', 'r'),
  ('646be06b-2b82-412e-a9ef-e60d287bcc17', '05dfbd09-3252-42a1-901b-85691802d9bc', '100fba96-f33d-4242-a551-722b73bc9c6d', '73084b35-377f-47ea-a369-713d31e2941d', 's'),
  ('f5812247-2f58-4b05-a5a9-51e3ff72470e', '05dfbd09-3252-42a1-901b-85691802d9bc', '100fba96-f33d-4242-a551-722b73bc9c6d', '73084b35-377f-47ea-a369-713d31e2941d', 'q'),
  ('c7718065-af28-40fa-8403-197bc6d23909', '05dfbd09-3252-42a1-901b-85691802d9bc', '100fba96-f33d-4242-a551-722b73bc9c6d', '73084b35-377f-47ea-a369-713d31e2941d', 'Kool Organisation Node A'),
  ('51659a69-901e-4f72-b516-2c1f83f1499f', '05dfbd09-3252-42a1-901b-85691802d9bc', '100fba96-f33d-4242-a551-722b73bc9c6d', 'c7718065-af28-40fa-8403-197bc6d23909', 'u'),
  ('8f1b1d7a-75dc-44ed-bc0b-24fe5e25b3fb', '05dfbd09-3252-42a1-901b-85691802d9bc', '100fba96-f33d-4242-a551-722b73bc9c6d', '51659a69-901e-4f72-b516-2c1f83f1499f', 'v'),
  ('7e2304f6-7683-4e7a-8298-a2a2123c14c8', '05dfbd09-3252-42a1-901b-85691802d9bc', '100fba96-f33d-4242-a551-722b73bc9c6d', '51659a69-901e-4f72-b516-2c1f83f1499f', 't'),
  ('d7b116ce-8fe9-4c32-b0fc-a76583082329', '05dfbd09-3252-42a1-901b-85691802d9bc', '100fba96-f33d-4242-a551-722b73bc9c6d', 'c7718065-af28-40fa-8403-197bc6d23909', 'w'),
  ('6390449b-2eb7-4e84-97e5-4188ce7a9a74', '05dfbd09-3252-42a1-901b-85691802d9bc', '100fba96-f33d-4242-a551-722b73bc9c6d', 'd7b116ce-8fe9-4c32-b0fc-a76583082329', 'z'),
  ('f226fc98-9b1c-45b0-931f-1b3f08f75ee2', '05dfbd09-3252-42a1-901b-85691802d9bc', '100fba96-f33d-4242-a551-722b73bc9c6d', 'd7b116ce-8fe9-4c32-b0fc-a76583082329', 'x'),
  ('ef707a37-08e3-4918-bcb7-0a1a903f13d8', '05dfbd09-3252-42a1-901b-85691802d9bc', '100fba96-f33d-4242-a551-722b73bc9c6d', 'd7b116ce-8fe9-4c32-b0fc-a76583082329', 'y');

INSERT INTO Node(Id, OrganisationId, OwnerId, NextId, Name)
VALUES
--('c7718065-af28-40fa-8403-197bc6d23909', '05dfbd09-3252-42a1-901b-85691802d9bc', '100fba96-f33d-4242-a551-722b73bc9c6d', null, 'Kool Organisation Node A'),
  ('9505402f-a60f-4bd2-91dc-9a02845989fa', '05dfbd09-3252-42a1-901b-85691802d9bc', '100fba96-f33d-4242-a551-722b73bc9c6d', null, 'Kool Organisation Node B'),
  ('9fa72ac4-9b9a-4bf7-8901-c640881bf4a5', '05dfbd09-3252-42a1-901b-85691802d9bc', '100fba96-f33d-4242-a551-722b73bc9c6d', null, 'Kool Organisation Node C'),
  ('2c962a0d-bff2-4f3d-8f8a-49c27418001b', '05dfbd09-3252-42a1-901b-85691802d9bc', '100fba96-f33d-4242-a551-722b73bc9c6d', null, 'Kool Organisation Node D'),
  ('2c4cccb1-7873-4732-b0e3-6425d4d24922', '05dfbd09-3252-42a1-901b-85691802d9bc', '100fba96-f33d-4242-a551-722b73bc9c6d', null, 'Kool Organisation Node E');

INSERT INTO Edge(Id, OrganisationId, OwnerId, NextId, SourceId, TargetId, Name)
VALUES
  ('fdb37c86-62c3-42b4-a41c-8e648533609c', '05dfbd09-3252-42a1-901b-85691802d9bc', '100fba96-f33d-4242-a551-722b73bc9c6d', null, 'c7718065-af28-40fa-8403-197bc6d23909', '9505402f-a60f-4bd2-91dc-9a02845989fa', 'Kool Organisation Edge A-B'),
  ('7e352e70-164d-4489-ba73-5a9fcb17e6cf', '05dfbd09-3252-42a1-901b-85691802d9bc', '100fba96-f33d-4242-a551-722b73bc9c6d', null, '9505402f-a60f-4bd2-91dc-9a02845989fa', '9fa72ac4-9b9a-4bf7-8901-c640881bf4a5', 'Kool Organisation Edge B-C'),
  ('04d69adb-1da6-428f-a9d5-b5e05f2e8661', '05dfbd09-3252-42a1-901b-85691802d9bc', '100fba96-f33d-4242-a551-722b73bc9c6d', null, '9fa72ac4-9b9a-4bf7-8901-c640881bf4a5', '2c962a0d-bff2-4f3d-8f8a-49c27418001b', 'Kool Organisation Edge C-D'),
  ('fb6f545b-0ab7-4351-b35e-8d37ae3f9cd9', '05dfbd09-3252-42a1-901b-85691802d9bc', '100fba96-f33d-4242-a551-722b73bc9c6d', null, '2c962a0d-bff2-4f3d-8f8a-49c27418001b', 'c7718065-af28-40fa-8403-197bc6d23909', 'Kool Organisation Edge D-A'),
  ('2bfad8e2-4c89-4da6-9cf4-a9737bf3a1d1', '05dfbd09-3252-42a1-901b-85691802d9bc', '100fba96-f33d-4242-a551-722b73bc9c6d', null, 'c7718065-af28-40fa-8403-197bc6d23909', '9fa72ac4-9b9a-4bf7-8901-c640881bf4a5', 'Kool Organisation Edge A-C'),
  ('8af5d92c-d826-433c-b5ea-8a055684f241', '05dfbd09-3252-42a1-901b-85691802d9bc', '100fba96-f33d-4242-a551-722b73bc9c6d', null, '9505402f-a60f-4bd2-91dc-9a02845989fa', '2c962a0d-bff2-4f3d-8f8a-49c27418001b', 'Kool Organisation Edge B-D');


-- graph entities
INSERT INTO Graph(Id, OrganisationId, OwnerId, Directed, Name)
VALUES
  ('470a1e4f-c0c4-400c-a726-9ffe9697135a', 'c018775d-ac42-46d2-bf8d-7fadced870d5', 'b556969c-bd2c-4378-ae95-972923118295', 0, 'GraphML Graph 0'),
  ('6d574bb5-7f80-4e33-a373-59dd18e8e47c', 'c018775d-ac42-46d2-bf8d-7fadced870d5', 'b556969c-bd2c-4378-ae95-972923118295', 0, 'GraphML Graph 1'),
  ('68e65dd3-940e-41ce-8274-6e2518390605', '05dfbd09-3252-42a1-901b-85691802d9bc', '100fba96-f33d-4242-a551-722b73bc9c6d', 1, 'Kool Organisation Graph 0'),
  ('a5f2ab9c-2fe0-4072-a9ab-c38f21d364b1', '05dfbd09-3252-42a1-901b-85691802d9bc', '45783182-4912-45c5-9589-11f66c69bddb', 0, 'Kool Organisation Graph 1');

INSERT INTO GraphNode(Id, OrganisationId, OwnerId, RepositoryItemId, Name)
VALUES
  ('a05f5372-4c26-4afa-a08d-ae37578e75cc', '05dfbd09-3252-42a1-901b-85691802d9bc', '68e65dd3-940e-41ce-8274-6e2518390605', 'c7718065-af28-40fa-8403-197bc6d23909', 'Kool Organisation Graph Node A'),
  ('33047ebf-8868-42a0-9c82-0b88ca5580c3', '05dfbd09-3252-42a1-901b-85691802d9bc', '68e65dd3-940e-41ce-8274-6e2518390605', '9505402f-a60f-4bd2-91dc-9a02845989fa', 'Kool Organisation Graph Node B'),
  ('adb66f79-f3fd-4342-bcb0-18e727557692', '05dfbd09-3252-42a1-901b-85691802d9bc', '68e65dd3-940e-41ce-8274-6e2518390605', '9fa72ac4-9b9a-4bf7-8901-c640881bf4a5', 'Kool Organisation Graph Node C'),
  ('4dd192cb-4933-405a-9934-caf6986a8c71', '05dfbd09-3252-42a1-901b-85691802d9bc', '68e65dd3-940e-41ce-8274-6e2518390605', '2c962a0d-bff2-4f3d-8f8a-49c27418001b', 'Kool Organisation Graph Node D'),
  ('f494a264-4ed0-499a-a918-81cd97e96407', '05dfbd09-3252-42a1-901b-85691802d9bc', '68e65dd3-940e-41ce-8274-6e2518390605', '2c4cccb1-7873-4732-b0e3-6425d4d24922', 'Kool Organisation Graph Node E');

INSERT INTO GraphEdge(Id, OrganisationId, OwnerId, RepositoryItemId, Name, GraphSourceId, GraphTargetId)
VALUES
  ('39677015-5ca5-4bd9-a884-3a9544f94036', '05dfbd09-3252-42a1-901b-85691802d9bc', '68e65dd3-940e-41ce-8274-6e2518390605', 'fdb37c86-62c3-42b4-a41c-8e648533609c', 'Kool Organisation Graph Edge A-B', 'a05f5372-4c26-4afa-a08d-ae37578e75cc', '33047ebf-8868-42a0-9c82-0b88ca5580c3'),
  ('f3d9dc73-f83d-497f-a17c-ce0ded62616a', '05dfbd09-3252-42a1-901b-85691802d9bc', '68e65dd3-940e-41ce-8274-6e2518390605', '7e352e70-164d-4489-ba73-5a9fcb17e6cf', 'Kool Organisation Graph Edge B-C', '33047ebf-8868-42a0-9c82-0b88ca5580c3', 'adb66f79-f3fd-4342-bcb0-18e727557692'),
  ('36ca1afc-3ab0-4b48-99be-4adde3e0329d', '05dfbd09-3252-42a1-901b-85691802d9bc', '68e65dd3-940e-41ce-8274-6e2518390605', '04d69adb-1da6-428f-a9d5-b5e05f2e8661', 'Kool Organisation Graph Edge C-D', 'adb66f79-f3fd-4342-bcb0-18e727557692', '4dd192cb-4933-405a-9934-caf6986a8c71'),
  ('c9021b7a-be66-4b75-8cc1-5b1d71be8d71', '05dfbd09-3252-42a1-901b-85691802d9bc', '68e65dd3-940e-41ce-8274-6e2518390605', 'fb6f545b-0ab7-4351-b35e-8d37ae3f9cd9', 'Kool Organisation Graph Edge D-A', '4dd192cb-4933-405a-9934-caf6986a8c71', 'a05f5372-4c26-4afa-a08d-ae37578e75cc'),
  ('f24e293d-790e-477b-8285-85850e969e3c', '05dfbd09-3252-42a1-901b-85691802d9bc', '68e65dd3-940e-41ce-8274-6e2518390605', '2bfad8e2-4c89-4da6-9cf4-a9737bf3a1d1', 'Kool Organisation Graph Edge A-C', 'a05f5372-4c26-4afa-a08d-ae37578e75cc', 'adb66f79-f3fd-4342-bcb0-18e727557692'),
  ('72f1101b-d7a0-4326-9410-21d863f0870e', '05dfbd09-3252-42a1-901b-85691802d9bc', '68e65dd3-940e-41ce-8274-6e2518390605', '8af5d92c-d826-433c-b5ea-8a055684f241', 'Kool Organisation Graph Edge B-D', '33047ebf-8868-42a0-9c82-0b88ca5580c3', '4dd192cb-4933-405a-9934-caf6986a8c71');


-- chart entities
INSERT INTO Chart(Id, OrganisationId, OwnerId, Name)
VALUES
  ('ae54e3c5-31af-4be4-a602-771f4c3d2d5c', '05dfbd09-3252-42a1-901b-85691802d9bc', '68e65dd3-940e-41ce-8274-6e2518390605', 'Kool Organisation Chart 0');

INSERT INTO ChartNode(Id, OrganisationId, OwnerId, Name, GraphItemId, X, Y)
VALUES
  ('ae54e3c5-31af-4be4-a602-771f4c3d2d5c', '05dfbd09-3252-42a1-901b-85691802d9bc', 'ae54e3c5-31af-4be4-a602-771f4c3d2d5c', 'Kool Organisation Chart Node A', 'a05f5372-4c26-4afa-a08d-ae37578e75cc', 10, 110),
  ('a80dd1e6-45c8-4bac-b2bb-ee9a149e4644', '05dfbd09-3252-42a1-901b-85691802d9bc', 'ae54e3c5-31af-4be4-a602-771f4c3d2d5c', 'Kool Organisation Chart Node B', '33047ebf-8868-42a0-9c82-0b88ca5580c3', 20, 120),
  ('e070183a-e4fa-4653-9693-621dbc2451d3', '05dfbd09-3252-42a1-901b-85691802d9bc', 'ae54e3c5-31af-4be4-a602-771f4c3d2d5c', 'Kool Organisation Chart Node C', 'adb66f79-f3fd-4342-bcb0-18e727557692', 30, 130),
  ('40d2e664-10d8-4d3c-8821-7ca153c34b9c', '05dfbd09-3252-42a1-901b-85691802d9bc', 'ae54e3c5-31af-4be4-a602-771f4c3d2d5c', 'Kool Organisation Chart Node D', '4dd192cb-4933-405a-9934-caf6986a8c71', 40, 140);

INSERT INTO ChartEdge(Id, OrganisationId, OwnerId, Name, GraphItemId, ChartSourceId, ChartTargetId)
VALUES
  ('21124ff5-b439-4268-b0ba-7635f2a788c3', '05dfbd09-3252-42a1-901b-85691802d9bc', 'ae54e3c5-31af-4be4-a602-771f4c3d2d5c', 'Kool Organisation Chart Edge A-B', '39677015-5ca5-4bd9-a884-3a9544f94036', 'ae54e3c5-31af-4be4-a602-771f4c3d2d5c', 'a80dd1e6-45c8-4bac-b2bb-ee9a149e4644'),
  ('be330c82-82a9-44d7-a2dc-f68062a9e158', '05dfbd09-3252-42a1-901b-85691802d9bc', 'ae54e3c5-31af-4be4-a602-771f4c3d2d5c', 'Kool Organisation Chart Edge B-C', 'f3d9dc73-f83d-497f-a17c-ce0ded62616a', 'a80dd1e6-45c8-4bac-b2bb-ee9a149e4644', 'e070183a-e4fa-4653-9693-621dbc2451d3'),
  ('848314b8-ada6-4034-baf4-cfa9f7f0c8ed', '05dfbd09-3252-42a1-901b-85691802d9bc', 'ae54e3c5-31af-4be4-a602-771f4c3d2d5c', 'Kool Organisation Chart Edge C-D', '36ca1afc-3ab0-4b48-99be-4adde3e0329d', 'e070183a-e4fa-4653-9693-621dbc2451d3', '40d2e664-10d8-4d3c-8821-7ca153c34b9c'),
  ('c7974c96-bb1f-47e3-a7e3-0aebe90802b1', '05dfbd09-3252-42a1-901b-85691802d9bc', 'ae54e3c5-31af-4be4-a602-771f4c3d2d5c', 'Kool Organisation Chart Edge D-A', 'c9021b7a-be66-4b75-8cc1-5b1d71be8d71', '40d2e664-10d8-4d3c-8821-7ca153c34b9c', 'ae54e3c5-31af-4be4-a602-771f4c3d2d5c'),
  ('519a86c8-c3c0-4d9c-bc74-084bb64ae7ae', '05dfbd09-3252-42a1-901b-85691802d9bc', 'ae54e3c5-31af-4be4-a602-771f4c3d2d5c', 'Kool Organisation Chart Edge A-C', 'f24e293d-790e-477b-8285-85850e969e3c', 'ae54e3c5-31af-4be4-a602-771f4c3d2d5c', 'e070183a-e4fa-4653-9693-621dbc2451d3'),
  ('40598d9d-4273-4258-9fad-bc4144f460e8', '05dfbd09-3252-42a1-901b-85691802d9bc', 'ae54e3c5-31af-4be4-a602-771f4c3d2d5c', 'Kool Organisation Chart Edge B-D', '72f1101b-d7a0-4326-9410-21d863f0870e', 'a80dd1e6-45c8-4bac-b2bb-ee9a149e4644', '40d2e664-10d8-4d3c-8821-7ca153c34b9c');


-- timeline entities
INSERT INTO Timeline(Id, OrganisationId, OwnerId, Name)
VALUES
  ('16bf155b-04bb-4157-85b2-bf324971b9f4', '05dfbd09-3252-42a1-901b-85691802d9bc', '68e65dd3-940e-41ce-8274-6e2518390605', 'Kool Organisation Timeline 0');

INSERT INTO TimelineNode(Id, OrganisationId, OwnerId, Name, GraphItemId)
VALUES
  ('80d2cd93-d027-46f7-a786-65d85410b784', '05dfbd09-3252-42a1-901b-85691802d9bc', 'ae54e3c5-31af-4be4-a602-771f4c3d2d5c', 'Kool Organisation Timeline Node A', 'a05f5372-4c26-4afa-a08d-ae37578e75cc'),
  ('ec6757c2-3ff6-4598-ad05-42a96a9a81d5', '05dfbd09-3252-42a1-901b-85691802d9bc', 'ae54e3c5-31af-4be4-a602-771f4c3d2d5c', 'Kool Organisation Timeline Node B', '33047ebf-8868-42a0-9c82-0b88ca5580c3'),
  ('1e708aa5-bb6a-468e-95d5-0d06ad568d8c', '05dfbd09-3252-42a1-901b-85691802d9bc', 'ae54e3c5-31af-4be4-a602-771f4c3d2d5c', 'Kool Organisation Timeline Node C', 'adb66f79-f3fd-4342-bcb0-18e727557692'),
  ('4c0c6ee3-fadb-43f2-8691-a22810aff2a9', '05dfbd09-3252-42a1-901b-85691802d9bc', 'ae54e3c5-31af-4be4-a602-771f4c3d2d5c', 'Kool Organisation Timeline Node D', '4dd192cb-4933-405a-9934-caf6986a8c71');

INSERT INTO TimelineEdge(Id, OrganisationId, OwnerId, Name, GraphItemId, TimelineSourceId, TimelineTargetId)
VALUES
  ('2be1a19e-9358-402e-8128-fb09273ada7b', '05dfbd09-3252-42a1-901b-85691802d9bc', 'ae54e3c5-31af-4be4-a602-771f4c3d2d5c', 'Kool Organisation Chart Edge A-B', '39677015-5ca5-4bd9-a884-3a9544f94036', '80d2cd93-d027-46f7-a786-65d85410b784', 'ec6757c2-3ff6-4598-ad05-42a96a9a81d5'),
  ('5781c0e7-467d-4e28-8dd4-2e21895efe76', '05dfbd09-3252-42a1-901b-85691802d9bc', 'ae54e3c5-31af-4be4-a602-771f4c3d2d5c', 'Kool Organisation Chart Edge B-C', 'f3d9dc73-f83d-497f-a17c-ce0ded62616a', 'ec6757c2-3ff6-4598-ad05-42a96a9a81d5', '1e708aa5-bb6a-468e-95d5-0d06ad568d8c'),
  ('1c0b4182-c34e-4f20-b298-0997d2dc22da', '05dfbd09-3252-42a1-901b-85691802d9bc', 'ae54e3c5-31af-4be4-a602-771f4c3d2d5c', 'Kool Organisation Chart Edge C-D', '36ca1afc-3ab0-4b48-99be-4adde3e0329d', '1e708aa5-bb6a-468e-95d5-0d06ad568d8c', '4c0c6ee3-fadb-43f2-8691-a22810aff2a9'),
  ('766aeb80-ac62-46ed-bdf2-0916fe0a5b35', '05dfbd09-3252-42a1-901b-85691802d9bc', 'ae54e3c5-31af-4be4-a602-771f4c3d2d5c', 'Kool Organisation Chart Edge D-A', 'c9021b7a-be66-4b75-8cc1-5b1d71be8d71', '4c0c6ee3-fadb-43f2-8691-a22810aff2a9', '80d2cd93-d027-46f7-a786-65d85410b784'),
  ('2b053dde-3637-4aad-b5bf-606d0ce9aad8', '05dfbd09-3252-42a1-901b-85691802d9bc', 'ae54e3c5-31af-4be4-a602-771f4c3d2d5c', 'Kool Organisation Chart Edge A-C', 'f24e293d-790e-477b-8285-85850e969e3c', '80d2cd93-d027-46f7-a786-65d85410b784', '1e708aa5-bb6a-468e-95d5-0d06ad568d8c'),
  ('c4ece160-ff76-4158-82d9-f0a0b68f7740', '05dfbd09-3252-42a1-901b-85691802d9bc', 'ae54e3c5-31af-4be4-a602-771f4c3d2d5c', 'Kool Organisation Chart Edge B-D', '72f1101b-d7a0-4326-9410-21d863f0870e', 'ec6757c2-3ff6-4598-ad05-42a96a9a81d5', '4c0c6ee3-fadb-43f2-8691-a22810aff2a9');


-- item attributes definitions
INSERT INTO RepositoryItemAttributeDefinition(Id, OrganisationId, OwnerId, Name, DataType)
VALUES
  ('2db71b87-2183-42c9-ba18-d4dafeedc003', '05dfbd09-3252-42a1-901b-85691802d9bc', '66852486-9780-4aad-863a-b7fb2e903192', 'Repository Item Attribute Definition', 'string');

INSERT INTO GraphItemAttributeDefinition(Id, OrganisationId, OwnerId, Name, DataType)
VALUES
  ('d92de67a-6e4c-4c8d-b62a-67d21927dce8', '05dfbd09-3252-42a1-901b-85691802d9bc', '66852486-9780-4aad-863a-b7fb2e903192', 'Graph Item Attribute Definition', 'string');

INSERT INTO NodeItemAttributeDefinition (Id, OrganisationId, OwnerId, Name, DataType)
VALUES
  ('f0dae827-f8e6-4ecf-b4c1-ff6b32678b92', '05dfbd09-3252-42a1-901b-85691802d9bc', '66852486-9780-4aad-863a-b7fb2e903192', 'Node Item Attribute Definition', 'string');

INSERT INTO EdgeItemAttributeDefinition(Id, OrganisationId, OwnerId, Name, DataType)
VALUES
  ('71da5ccf-fba7-40e3-9685-8fc8fbd70212', '05dfbd09-3252-42a1-901b-85691802d9bc', '66852486-9780-4aad-863a-b7fb2e903192', 'Edge Item Attribute Definition', 'string'),
  ('5be052e8-b697-4a35-90fd-13953eb804c4', '05dfbd09-3252-42a1-901b-85691802d9bc', '66852486-9780-4aad-863a-b7fb2e903192', 'Timeline Item Attribute Definition', 'DateTimeInterval');


-- item attributes
INSERT INTO RepositoryItemAttribute(Id, OrganisationId, OwnerId, Name, DefinitionId, DataValueAsString)
VALUES
  ('4f7d2360-f9c8-4743-80dc-08c2e7ee22e6', '05dfbd09-3252-42a1-901b-85691802d9bc', 'b556969c-bd2c-4378-ae95-972923118295', 'Repository 0', '2db71b87-2183-42c9-ba18-d4dafeedc003', 'Repository 0 Attribute A'),
  ('3a987c96-a040-4abf-a0c1-fb6c4b3b5749', '05dfbd09-3252-42a1-901b-85691802d9bc', 'b556969c-bd2c-4378-ae95-972923118295', 'Repository 0', '2db71b87-2183-42c9-ba18-d4dafeedc003', 'Repository 0 Attribute B'),
  ('632a2858-a170-4692-a031-930fdb55e87d', '05dfbd09-3252-42a1-901b-85691802d9bc', 'b556969c-bd2c-4378-ae95-972923118295', 'Repository 0', '2db71b87-2183-42c9-ba18-d4dafeedc003', 'Repository 0 Attribute C'),
  ('92c978ac-63aa-49ac-b70c-1c945c109d86', '05dfbd09-3252-42a1-901b-85691802d9bc', 'b556969c-bd2c-4378-ae95-972923118295', 'Repository 0', '2db71b87-2183-42c9-ba18-d4dafeedc003', 'Repository 0 Attribute D');

INSERT INTO GraphItemAttribute(Id, OrganisationId, OwnerId, Name, DefinitionId, DataValueAsString)
VALUES
  ('e7e8a9bd-563e-4c0a-9993-fbb677b62571', '05dfbd09-3252-42a1-901b-85691802d9bc', '68e65dd3-940e-41ce-8274-6e2518390605', 'Graph 0', 'd92de67a-6e4c-4c8d-b62a-67d21927dce8', 'Attribute A'),
  ('c2a5c8f0-dc06-472c-ad0e-ac0e952e8934', '05dfbd09-3252-42a1-901b-85691802d9bc', '68e65dd3-940e-41ce-8274-6e2518390605', 'Graph 0', 'd92de67a-6e4c-4c8d-b62a-67d21927dce8', 'Attribute B'),
  ('ea494316-0403-4d15-ac08-67bc922b5605', '05dfbd09-3252-42a1-901b-85691802d9bc', '68e65dd3-940e-41ce-8274-6e2518390605', 'Graph 0', 'd92de67a-6e4c-4c8d-b62a-67d21927dce8', 'Attribute C'),
  ('9ae347c1-d1e0-45e8-a2b4-5ab636f4d265', '05dfbd09-3252-42a1-901b-85691802d9bc', '68e65dd3-940e-41ce-8274-6e2518390605', 'Graph 0', 'd92de67a-6e4c-4c8d-b62a-67d21927dce8', 'Attribute D');

INSERT INTO NodeItemAttribute(Id, OrganisationId, OwnerId, Name, DefinitionId, DataValueAsString)
VALUES
  ('8f245a18-7c35-484a-8579-722593af71e2', '05dfbd09-3252-42a1-901b-85691802d9bc', 'c7718065-af28-40fa-8403-197bc6d23909', 'Node A', 'f0dae827-f8e6-4ecf-b4c1-ff6b32678b92', 'Node A Attribute'),
  ('b191108a-7dec-4c66-8df1-87f77db9153f', '05dfbd09-3252-42a1-901b-85691802d9bc', '9505402f-a60f-4bd2-91dc-9a02845989fa', 'Node B', 'f0dae827-f8e6-4ecf-b4c1-ff6b32678b92', 'Node B Attribute'),
  ('004499ec-856b-422f-ae61-28b22d4328bf', '05dfbd09-3252-42a1-901b-85691802d9bc', '9fa72ac4-9b9a-4bf7-8901-c640881bf4a5', 'Node C', 'f0dae827-f8e6-4ecf-b4c1-ff6b32678b92', 'Node C Attribute'),
  ('43b61316-9eaf-422e-81ec-da26e5eafcf5', '05dfbd09-3252-42a1-901b-85691802d9bc', '2c962a0d-bff2-4f3d-8f8a-49c27418001b', 'Node D', 'f0dae827-f8e6-4ecf-b4c1-ff6b32678b92', 'Node D Attribute'),
  ('1a76cefa-de74-461b-8d56-f2d1624bc0f4', '05dfbd09-3252-42a1-901b-85691802d9bc', '2c4cccb1-7873-4732-b0e3-6425d4d24922', 'Node E', 'f0dae827-f8e6-4ecf-b4c1-ff6b32678b92', 'Node E Attribute');

-- TODO   DateTimeInterval.Start + DateTimeInterval.End
--    https://stackoverflow.com/questions/18193281/force-json-net-to-include-milliseconds-when-serializing-datetime-even-if-ms-com
--    https://stackoverflow.com/questions/10286204/what-is-the-right-json-date-format
INSERT INTO EdgeItemAttribute(Id, OrganisationId, OwnerId, Name, DefinitionId, DataValueAsString)
VALUES
  ('2c980b19-9fad-4f2a-a469-ae948f99c03d', '05dfbd09-3252-42a1-901b-85691802d9bc', 'fdb37c86-62c3-42b4-a41c-8e648533609c', 'Edge A-B', '71da5ccf-fba7-40e3-9685-8fc8fbd70212', 'Edge A-B Attribute'),
  ('a707532d-05cc-49ec-8d38-55fab653e7ec', '05dfbd09-3252-42a1-901b-85691802d9bc', '7e352e70-164d-4489-ba73-5a9fcb17e6cf', 'Edge B-C', '71da5ccf-fba7-40e3-9685-8fc8fbd70212', 'Edge B-C Attribute'),
  ('4916dc3f-4030-4e89-bd15-9d00c87c04fc', '05dfbd09-3252-42a1-901b-85691802d9bc', '04d69adb-1da6-428f-a9d5-b5e05f2e8661', 'Edge C-D', '71da5ccf-fba7-40e3-9685-8fc8fbd70212', 'Edge C-D Attribute'),
  ('acced08b-fe29-4a1a-9b93-ca668e8b3482', '05dfbd09-3252-42a1-901b-85691802d9bc', 'fb6f545b-0ab7-4351-b35e-8d37ae3f9cd9', 'Edge D-A', '71da5ccf-fba7-40e3-9685-8fc8fbd70212', 'Edge D-A Attribute'),
  ('4dd0ffb3-6199-4e38-bfa9-dcc41cdedbca', '05dfbd09-3252-42a1-901b-85691802d9bc', '2bfad8e2-4c89-4da6-9cf4-a9737bf3a1d1', 'Edge D-A', '71da5ccf-fba7-40e3-9685-8fc8fbd70212', 'Edge A-C Attribute'),
  ('9bfd22d6-757a-49e8-9ff2-c51ed59cdb76', '05dfbd09-3252-42a1-901b-85691802d9bc', '8af5d92c-d826-433c-b5ea-8a055684f241', 'Edge D-A', '71da5ccf-fba7-40e3-9685-8fc8fbd70212', 'Edge B-D Attribute'),
  ('70737d7b-0e5b-47bb-b947-42743948c1ce', '05dfbd09-3252-42a1-901b-85691802d9bc', 'fdb37c86-62c3-42b4-a41c-8e648533609c', 'TimelineEdge A-B', '5be052e8-b697-4a35-90fd-13953eb804c4', '{}'),
  ('30776d2e-0909-4f48-9f87-0099da9be22d', '05dfbd09-3252-42a1-901b-85691802d9bc', '7e352e70-164d-4489-ba73-5a9fcb17e6cf', 'TimelineEdge B-C', '5be052e8-b697-4a35-90fd-13953eb804c4', '{}'),
  ('84f6e067-aeef-4102-9408-360a1ee732e0', '05dfbd09-3252-42a1-901b-85691802d9bc', '04d69adb-1da6-428f-a9d5-b5e05f2e8661', 'TimelineEdge C-D', '5be052e8-b697-4a35-90fd-13953eb804c4', '{}'),
  ('970e8e19-b4f6-4304-a964-17de900df2dc', '05dfbd09-3252-42a1-901b-85691802d9bc', 'fb6f545b-0ab7-4351-b35e-8d37ae3f9cd9', 'TimelineEdge D-A', '5be052e8-b697-4a35-90fd-13953eb804c4', '{}'),
  ('e3515397-3681-4880-9871-791a384dff8c', '05dfbd09-3252-42a1-901b-85691802d9bc', '2bfad8e2-4c89-4da6-9cf4-a9737bf3a1d1', 'TimelineEdge D-A', '5be052e8-b697-4a35-90fd-13953eb804c4', '{}'),
  ('a20124cd-93ac-475d-909d-64070d866617', '05dfbd09-3252-42a1-901b-85691802d9bc', '8af5d92c-d826-433c-b5ea-8a055684f241', 'TimelineEdge D-A', '5be052e8-b697-4a35-90fd-13953eb804c4', '{}');


-- roles
INSERT INTO Role(Id, OrganisationId, Name)
VALUES
  ('7FC87DE7-BFD8-47A1-BBD4-95181A983F07', 'a0d9403c-f03f-480f-93e8-b7ca349645c6', 'User'),
  ('E56390B4-71B3-4850-82B4-EA65A497559E', 'a0d9403c-f03f-480f-93e8-b7ca349645c6', 'UserAdmin'),
  ('3127AD16-8AF9-4679-AF33-F5DF1A9BD3F3', 'a0d9403c-f03f-480f-93e8-b7ca349645c6', 'Admin');

INSERT INTO Contact(Id, OrganisationId, OwnerId, Name)
VALUES
  ('907bf1ab-fb91-494b-b1a8-376f9a9f03d8', 'c018775d-ac42-46d2-bf8d-7fadced870d5', 'c018775d-ac42-46d2-bf8d-7fadced870d5', 'admin@GraphML.com'),
  ('db9d2dc4-cf7f-4177-8f72-c04da623f1be', '05dfbd09-3252-42a1-901b-85691802d9bc', '05dfbd09-3252-42a1-901b-85691802d9bc', 'DrKool@KoolOrganisation.org'),
  ('33fbe12e-3aad-4238-bbcc-26ff6ea94ad1', '05dfbd09-3252-42a1-901b-85691802d9bc', '05dfbd09-3252-42a1-901b-85691802d9bc', 'carol@KoolOrganisation.org'),
  ('a7753555-e1f8-4569-a9ac-6f059dbc771f', '05dfbd09-3252-42a1-901b-85691802d9bc', '05dfbd09-3252-42a1-901b-85691802d9bc', 'dave@KoolOrganisation.org');

INSERT INTO ContactsRoles(ContactId, RoleId)
VALUES
  ('db9d2dc4-cf7f-4177-8f72-c04da623f1be', '3127AD16-8AF9-4679-AF33-F5DF1A9BD3F3'),
  ('33fbe12e-3aad-4238-bbcc-26ff6ea94ad1', 'E56390B4-71B3-4850-82B4-EA65A497559E'),
  ('a7753555-e1f8-4569-a9ac-6f059dbc771f', '7FC87DE7-BFD8-47A1-BBD4-95181A983F07');

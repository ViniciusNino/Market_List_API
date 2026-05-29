--SELECT * from "Areas"
INSERT INTO "Areas" ("SDescricao") VALUES
('Usuario'),
('ItemLista'),
('Lista'),
('Agrupado')

--SELECT * from "Status"
INSERT INTO "Status" ("NIdArea", "SDescricao") VALUES
(1,	'Ativo'),
(2,	'Solicitado'),
(2,	'Atualizado'),
(2,	'Excluído'),
(4,	'Ativo')

--SELECT * from "Sessao"
INSERT INTO "Sessao" ("SNome") VALUES
('Pedra'),
('Coqueiro'),
('Visão'),
('Guanabara')

--SELECT * from "Unidade"
INSERT INTO "Unidade" ("SNome") VALUES
('Casa')

--SELECT * from "Item"
INSERT INTO "Item" ("SNome", "NIdSessao", "SUnidadeMedida", "NIdUnidade") VALUES
('Maçã',	1, 'cx', 1),
('Banana',	1, 'cx', 1),
('Cheddar',	2, 'un', 1),
('Polenguinho', 	2, 'un', 1),
('Leite',	3, 'cx', 1),
('Açucar',	3, 'un', 1),
('Sucrilho', 	4, 'cx', 1),
('Iogurte',	4, 'un', 1)

--SELECT * from "PerfilUsuario"
INSERT INTO "PerfilUsuario" ("SDescricao") VALUES
('root'),
('Administrador'),
('Comprador'),
('Solicitante')


--SELECT * from "Usuario"
INSERT INTO "Usuario" ("SUsuario", "SSenha", "NIdPerfilUsuario", "NIdStatus", "DCadastro") VALUES
('sa', 'sa', 3,	1, NOW())

INSERT INTO Area (sDescricao) VALUES
('Usuário'),
('Agrupador'),
('Lista')

INSERT INTO Tipo (NIdArea, SDescricao) VALUES
((select Id from Area where SDescricao = 'Usuário'), 'root'),
((select Id from Area where SDescricao = 'Usuário'), 'Administrador'),
((select Id from Area where SDescricao = 'Usuário'), 'Comprador'),
((select Id from Area where SDescricao = 'Usuário'), 'Solicitante'),
((select Id from Area where SDescricao = 'Usuário'), 'Ativacao Email'),
((select Id from Area where SDescricao = 'Usuário'), 'Alterar Senha')

INSERT INTO Status (NIdArea, SDescricao) VALUES
((select Id from Area where SDescricao = 'Usuário'), 'Ativo'),
((select Id from Area where SDescricao = 'Lista'), 'Solicitado'),
((select Id from Area where SDescricao = 'Lista'), 'Atualizado'),
((select Id from Area where SDescricao = 'Lista'), 'Excluido'),
((select Id from Area where SDescricao = 'Agrupador'), 'Ativo'),
((select Id from Area where SDescricao = 'Usuário'), 'Aguardando Ativacao Email')


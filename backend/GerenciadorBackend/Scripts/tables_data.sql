IF NOT EXISTS (SELECT 1 FROM Comprador)
BEGIN
    INSERT INTO Comprador (Id, DataDeCriacao, Nome, Documento, Cidade, Estado) VALUES
    (NEWID(), GETDATE(), 'Ana Costa', '12345678901', 'São Paulo', 'SP'),
    (NEWID(), GETDATE(), 'Bruno Martins', '23456789012', 'Rio de Janeiro', 'RJ'),
    (NEWID(), GETDATE(), 'Carla Souza', '34567890123', 'Belo Horizonte', 'MG'),
    (NEWID(), GETDATE(), 'Daniel Lima', '45678901234', 'Porto Alegre', 'RS'),
    (NEWID(), GETDATE(), 'Eduarda Fernandes', '56789012345', 'Curitiba', 'PR'),
    (NEWID(), GETDATE(), 'Felipe Rocha', '67890123456', 'Salvador', 'BA'),
    (NEWID(), GETDATE(), 'Gabriela Alves', '78901234567', 'Brasília', 'DF'),
    (NEWID(), GETDATE(), 'Henrique Dias', '89012345678', 'Fortaleza', 'CE'),
    (NEWID(), GETDATE(), 'Isabela Ribeiro', '90123456789', 'Recife', 'PE'),
    (NEWID(), GETDATE(), 'João Pedro', '01234567890', 'Manaus', 'AM')
END

IF NOT EXISTS (SELECT 1 FROM Carne)
BEGIN
    INSERT INTO Carne (Id, DataDeCriacao, Nome, Descricao, Tipo) VALUES
    (NEWID(), GETDATE(), 'Picanha', 'Corte nobre bovino', 0),
    (NEWID(), GETDATE(), 'Filé Mignon', 'Carne macia bovina', 0),
    (NEWID(), GETDATE(), 'Costela Suína', 'Costela temperada suína', 1),
    (NEWID(), GETDATE(), 'Frango à Passarinho', 'Cortes de frango', 2),
    (NEWID(), GETDATE(), 'Salmão', 'Filé de salmão fresco', 3),
    (NEWID(), GETDATE(), 'Tilápia', 'Filé de tilápia', 3),
    (NEWID(), GETDATE(), 'Coxa de Frango', 'Coxa sem pele', 2),
    (NEWID(), GETDATE(), 'Lombo Suíno', 'Lombo temperado', 1),
    (NEWID(), GETDATE(), 'Alcatra', 'Corte bovino macio', 0),
    (NEWID(), GETDATE(), 'Peito de Frango', 'Peito sem osso', 2),
    (NEWID(), GETDATE(), 'Bacalhau', 'Lombo de bacalhau dessalgado', 3),
    (NEWID(), GETDATE(), 'Linguiça Toscana', 'Linguiça de porco', 1),
    (NEWID(), GETDATE(), 'Contra-filé', 'Corte bovino suculento', 0),
    (NEWID(), GETDATE(), 'Asa de Frango', 'Asa temperada', 2),
    (NEWID(), GETDATE(), 'Cupim', 'Corte bovino marmorizado', 0)
END

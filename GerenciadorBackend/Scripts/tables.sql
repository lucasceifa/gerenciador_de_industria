IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Comprador' AND xtype='U')
BEGIN
    CREATE TABLE Comprador (
        Id UNIQUEIDENTIFIER PRIMARY KEY,
        DataDeCriacao DATETIME NOT NULL,
        Nome NVARCHAR(100) NOT NULL,
        Documento NVARCHAR(30) NOT NULL,
        Cidade NVARCHAR(100) NULL,
        Estado NVARCHAR(100) NULL
    )
END

IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Carne' AND xtype='U')
BEGIN
    CREATE TABLE Carne (
        Id UNIQUEIDENTIFIER PRIMARY KEY,
        DataDeCriacao DATETIME NOT NULL,
        Nome NVARCHAR(100) NOT NULL,
        Descricao NVARCHAR(MAX) NULL,
        Tipo INT NOT NULL
    )
END

IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Pedido' AND xtype='U')
BEGIN
    CREATE TABLE Pedido (
        Id UNIQUEIDENTIFIER PRIMARY KEY,
        DataDeCriacao DATETIME NOT NULL,
        DataRealizada DATETIME NOT NULL,
        CompradorId UNIQUEIDENTIFIER NOT NULL,
        FOREIGN KEY (CompradorId) REFERENCES Comprador(Id)
    )
END

IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='ItemPedido' AND xtype='U')
BEGIN
    CREATE TABLE ItemPedido (
        Id UNIQUEIDENTIFIER PRIMARY KEY,
        DataDeCriacao DATETIME NOT NULL,
        PedidoId UNIQUEIDENTIFIER NOT NULL,
        CarneId UNIQUEIDENTIFIER NOT NULL,
        Preco FLOAT NOT NULL,
        Moeda INT NOT NULL,
        FOREIGN KEY (PedidoId) REFERENCES Pedido(Id),
        FOREIGN KEY (CarneId) REFERENCES Carne(Id)
    )
END

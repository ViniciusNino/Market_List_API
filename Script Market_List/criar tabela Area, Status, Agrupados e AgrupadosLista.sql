
create table Area (
    Id INT PRIMARY KEY IDENTITY(1,1),
    sDescricao VARCHAR(50) NOT null,
)

create table Status (
    Id INT PRIMARY KEY IDENTITY(1,1),
    nIdArea INT NOT NULL,
    sDescricao VARCHAR(50) NOT null,

    CONSTRAINT FK_Status_Area FOREIGN KEY (nIdArea) REFERENCES Area (Id)
)

create table AgrupadorListas (
    Id INT PRIMARY KEY IDENTITY(1,1),
    nIdUsuario INT NOT NULL,
    nIdStatus  INT NOT NULL,
    dCadastro DATETIME NOT NULL,

    CONSTRAINT FK_Usuario_Agrupado FOREIGN KEY (nIdUsuario) REFERENCES Usuario (Id),
    CONSTRAINT FK_Status_Agrupado FOREIGN KEY (nIdStatus) REFERENCES Status (Id),
)

create table ListaAgrupadorListas (
    Id INT PRIMARY KEY IDENTITY(1,1),
    nIdAgrupadorListas INT NOT NULL,
    nIdLista  INT NOT NULL,

    CONSTRAINT FK_Agrupado_AgrupadosListas FOREIGN KEY (nIdAgrupadorListas) REFERENCES AgrupadorListas (Id),
    CONSTRAINT FK_Lista_AgrupadosListas FOREIGN KEY (nIdLista) REFERENCES Lista (Id),
)

CREATE NONCLUSTERED INDEX IDX_Usuario ON AgrupadorListas (nIdUsuario ASC) INCLUDE(nIdStatus, dCadastro)
CREATE NONCLUSTERED INDEX IDX_AgrupadorListas_Lista ON ListaAgrupadorListas (nIdAgrupadorListas ASC, nIdLista ASC) 
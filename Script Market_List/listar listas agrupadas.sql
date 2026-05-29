SELECT * from "Usuario"

INSERT INTO "Usuario" ("SUsuario", "SSenha", "NIdPerfilUsuario", "NIdStatus", "DCadastro")
VALUES ('sd', 'sd', 3, 1, GETDATE())
SELECT * from "Unidade"

SELECT * from "UsuarioUnidade"
INSERT INTO "UsuarioUnidade" ("NIdUsuario", "NIdUnidade") VALUES(3,1)

SELECT * from "Status" s
JOIN "Areas" a on s."NIdArea" = a."Id"

SELECT * from "ListaAgrupadorListas" where "NIdAgrupadorListas" = 3
--SELECT * from update "AgrupadorListas" set "SDescricao" = 'Terceiro teste' where "Id" = 5

SELECT * from "ListaAgrupadorListas"
SELECT * from "AgrupadorListas"
SELECT * from "Lista"
SELECT * from "ItemLista"
SELECT * from "Item"

SELECT 
al."SDescricao" as "Agrupador", 
i."SNome",
il."NQuantidade"
--lal.*, * 
FROM "AgrupadorListas" al
JOIN "ListaAgrupadorListas" lal on al."Id" = lal."NIdAgrupadorListas"
JOIN "ItemLista" il on il."NIdLista" = lal."NIdLista"
JOIN "Item" i on il."NIdItem" = i."Id"
ORDER BY 1, 2

SELECT 
MIN(al."Id"),
al."SDescricao" as "Agrupador", 
i."SNome",
MIN(i."SUnidadeMedida") as "Unidade de Medida",
SUM(il."NQuantidade") as "Quantidade"
--lal.*, * 
FROM "AgrupadorListas" al
JOIN "ListaAgrupadorListas" lal on al."Id" = lal."NIdAgrupadorListas"
JOIN "ItemLista" il on il."NIdLista" = lal."NIdLista"
JOIN "Item" i on il."NIdItem" = i."Id"
WHERE al."NIdUsuario" = 3 
GROUP BY i."SNome", al."SDescricao"
ORDER BY 1, 2